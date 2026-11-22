using System;
using System.Collections;
using System.Collections.Generic;
using LastOrders.Combat;
using LastOrders.Data;
using UnityEngine;

namespace LastOrders.Enemies
{
    /// <summary>
    /// A basic drunk who walks up beside you, lines up on your lane, and swings.
    /// Simple state machine: Idle > Chase > Attack, with Hurt and Dead.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(HitReaction))]
    public class EnemyAI : MonoBehaviour
    {
        enum State { Idle, Chase, Attack, Hurt, Dead }

        [Header("Attack")]
        [SerializeField] WeaponData attack;
        [Tooltip("Set this to the Player layer.")]
        [SerializeField] LayerMask hittableLayers;
        [SerializeField] float attackCooldown = 1.2f;

        [Header("Movement")]
        [SerializeField] float moveSpeed = 2.5f;
        [SerializeField] float aggroRange = 8f;
        [Tooltip("How close in Y (lane) the player must be before we swing.")]
        [SerializeField] float laneTolerance = 0.2f;
        [SerializeField] float stopDeceleration = 20f;
        [SerializeField] Transform visual;

        [Header("Death")]
        [SerializeField] float corpseTime = 1f;

        Rigidbody2D rb;
        Health health;
        HitReaction reaction;
        Transform player;
        Health playerHealth;

        State state = State.Idle;
        int facing = -1;
        float nextAttackTime;
        Coroutine attackRoutine;
        readonly HashSet<IDamageable> hitThisSwing = new();

        /// <summary>Fired when this enemy dies. The wave spawner will use this in week 3.</summary>
        public event Action<EnemyAI> Died;

        // Close enough that the front of the hitbox will reach the player.
        float AttackReach => attack.hitboxOffset + attack.hitboxSize.x * 0.5f - 0.15f;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            reaction = GetComponent<HitReaction>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            if (attack == null) attack = WeaponData.Default;
        }

        void OnEnable()
        {
            health.OnHit += HandleHit;
            health.OnDeath += HandleDeath;
        }

        void OnDisable()
        {
            health.OnHit -= HandleHit;
            health.OnDeath -= HandleDeath;
        }

        void Start()
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                playerHealth = p.GetComponent<Health>();
            }
        }

        void Update()
        {
            if (state == State.Dead) return;

            if (state == State.Hurt && !reaction.IsStunned) state = State.Chase;

            bool playerAlive = player != null && (playerHealth == null || !playerHealth.IsDead);

            if (state == State.Idle && playerAlive &&
                Vector2.Distance(transform.position, player.position) < aggroRange)
                state = State.Chase;

            if (state != State.Chase) return;

            if (!playerAlive) { state = State.Idle; return; }

            Vector2 toPlayer = player.position - transform.position;
            if (Mathf.Abs(toPlayer.x) > 0.05f) SetFacing(toPlayer.x > 0f ? 1 : -1);

            bool inRange = Mathf.Abs(toPlayer.x) <= AttackReach && Mathf.Abs(toPlayer.y) <= laneTolerance;
            if (inRange && Time.time >= nextAttackTime)
                attackRoutine = StartCoroutine(Attack());
        }

        void FixedUpdate()
        {
            if (state == State.Chase && player != null)
            {
                // Stand BESIDE the player on the same lane, not on top of them.
                float side = transform.position.x < player.position.x ? -1f : 1f;
                Vector2 spot = new(player.position.x + side * AttackReach * 0.8f, player.position.y);
                Vector2 toSpot = spot - rb.position;
                rb.linearVelocity = toSpot.magnitude > 0.05f ? toSpot.normalized * moveSpeed : Vector2.zero;
            }
            else
            {
                rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, Vector2.zero,
                                                        stopDeceleration * Time.fixedDeltaTime);
            }
        }

        IEnumerator Attack()
        {
            state = State.Attack;

            // The windup is the "tell": players need time to see it coming.
            yield return new WaitForSeconds(attack.windup);

            hitThisSwing.Clear();
            HitInfo hit = attack.ToHitInfo(facing, gameObject);
            float activeEnd = Time.time + attack.active;
            do
            {
                CombatUtils.HitBox(attack.HitboxCenter(transform.position, facing), attack.hitboxSize,
                                   hittableLayers, hit, gameObject, hitThisSwing);
                yield return null;
            } while (Time.time < activeEnd);

            yield return new WaitForSeconds(attack.recovery);

            nextAttackTime = Time.time + attackCooldown;
            attackRoutine = null;
            if (state == State.Attack) state = State.Chase;
        }

        void HandleHit(HitInfo _)
        {
            if (state == State.Dead) return;
            CancelAttack();
            state = State.Hurt;
            nextAttackTime = Mathf.Max(nextAttackTime, Time.time + 0.4f); // no instant revenge punch
        }

        void HandleDeath()
        {
            CancelAttack();
            state = State.Dead;
            foreach (var col in GetComponentsInChildren<Collider2D>()) col.enabled = false;
            Died?.Invoke(this);
            StartCoroutine(FadeAndDestroy());
        }

        void CancelAttack()
        {
            if (attackRoutine != null) StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        IEnumerator FadeAndDestroy()
        {
            yield return new WaitForSeconds(0.3f); // let the knockback play out
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            for (float t = 0f; t < corpseTime; t += Time.deltaTime)
            {
                bool visible = Mathf.FloorToInt(t * 15f) % 2 == 0; // classic arcade blink
                foreach (var r in renderers) r.enabled = visible;
                yield return null;
            }
            Destroy(gameObject);
        }

        void SetFacing(int direction)
        {
            facing = direction;
            Transform t = visual != null ? visual : transform;
            Vector3 s = t.localScale;
            s.x = Mathf.Abs(s.x) * direction;
            t.localScale = s;
        }

        void OnDrawGizmosSelected()
        {
            var a = attack != null ? attack : WeaponData.Default;
            CombatUtils.DrawBoxGizmo(a.HitboxCenter(transform.position, facing), a.hitboxSize, new Color(1f, 0.5f, 0f));
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
        }
    }
}
