using LastOrders.Combat;
using LastOrders.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastOrders.Player
{
    /// <summary>
    /// Belt-scroller movement: left/right along the street, up/down to change "lane" (depth).
    /// The root's position is the character's FEET.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(HitReaction))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] float moveSpeed = 5f;
        [Tooltip("Up/down is usually slower than left/right in beat 'em ups.")]
        [SerializeField] float depthSpeed = 3.5f;
        [SerializeField] float stopDeceleration = 30f;
        [Tooltip("Lowest and highest Y the player can walk to (the floor area).")]
        [SerializeField] Vector2 depthLimits = new(-3.5f, 0f);

        [Header("Visuals")]
        [Tooltip("Child object holding the sprite. It gets flipped to face left/right.")]
        [SerializeField] Transform visual;

        [Header("Death")]
        [SerializeField] float restartDelay = 2f;

        Rigidbody2D rb;
        Health health;
        HitReaction reaction;
        Vector2 moveInput;

        public int Facing { get; private set; } = 1;
        public bool IsAttacking { get; set; }
        public bool CanStartAttack => !reaction.IsStunned && !health.IsDead;
        bool CanMove => !IsAttacking && CanStartAttack;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            reaction = GetComponent<HitReaction>();

            rb.gravityScale = 0f;      // no gravity: Y is depth, not height
            rb.freezeRotation = true;
        }

        void OnEnable()  => health.OnDeath += HandleDeath;
        void OnDisable() => health.OnDeath -= HandleDeath;

        void Update()
        {
            moveInput = GameInput.Move();
            if (CanMove && Mathf.Abs(moveInput.x) > 0.1f) SetFacing(moveInput.x > 0f ? 1 : -1);
        }

        void FixedUpdate()
        {
            if (CanMove)
                rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * depthSpeed);
            else // slide to a stop (this is also what makes knockback look right)
                rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, Vector2.zero,
                                                        stopDeceleration * Time.fixedDeltaTime);

            Vector2 p = rb.position;
            float clampedY = Mathf.Clamp(p.y, depthLimits.x, depthLimits.y);
            if (!Mathf.Approximately(clampedY, p.y)) rb.position = new Vector2(p.x, clampedY);
        }

        public void SetFacing(int direction)
        {
            Facing = direction;
            Transform t = visual != null ? visual : transform;
            Vector3 s = t.localScale;
            s.x = Mathf.Abs(s.x) * direction;
            t.localScale = s;
        }

        void HandleDeath()
        {
            Invoke(nameof(Restart), restartDelay);
        }

        // Placeholder until there's a game-over screen. The scene must be in Build Profiles.
        void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        void OnDrawGizmosSelected()
        {
            // Show the walkable floor band in the Scene view.
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3(-100f, depthLimits.x), new Vector3(100f, depthLimits.x));
            Gizmos.DrawLine(new Vector3(-100f, depthLimits.y), new Vector3(100f, depthLimits.y));
        }
    }
}
