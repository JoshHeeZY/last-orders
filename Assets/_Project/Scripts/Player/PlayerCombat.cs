using System;
using System.Collections;
using System.Collections.Generic;
using LastOrders.Combat;
using LastOrders.Core;
using LastOrders.Data;
using UnityEngine;

namespace LastOrders.Player
{
    /// <summary>
    /// Light attack combo: hit, hit, FINISHER. Presses are buffered so mashing feels responsive.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] WeaponData weapon;
        [Tooltip("Set this to the Enemy layer.")]
        [SerializeField] LayerMask hittableLayers;

        [Header("Combo")]
        [SerializeField] int comboLength = 3;
        [SerializeField] float finisherDamageMultiplier = 1.75f;
        [SerializeField] float finisherKnockbackMultiplier = 2.5f;
        [Tooltip("How long after an attack you can press again to continue the combo.")]
        [SerializeField] float comboWindow = 0.4f;
        [Tooltip("Presses this early are remembered and used as soon as possible.")]
        [SerializeField] float inputBufferTime = 0.2f;
        [Tooltip("Hit counter resets if you don't land a hit for this long.")]
        [SerializeField] float hitCounterTimeout = 1.5f;

        PlayerController controller;
        Health health;
        Coroutine attackRoutine;
        int comboStep;
        float comboExpiresAt;
        float bufferedUntil = -1f;
        float hitCounterExpiresAt;
        readonly HashSet<IDamageable> hitThisSwing = new();

        public WeaponData Weapon => weapon;
        public int HitCounter { get; private set; }

        /// <summary>Fired when a swing starts: (combo step, is finisher). Hook animations here later.</summary>
        public event Action<int, bool> AttackStarted;

        void Awake()
        {
            controller = GetComponent<PlayerController>();
            health = GetComponent<Health>();
            if (weapon == null) weapon = WeaponData.Default;
        }

        void OnEnable()  => health.OnHit += Interrupt;
        void OnDisable() => health.OnHit -= Interrupt;

        void Update()
        {
            if (GameInput.AttackPressed()) bufferedUntil = Time.time + inputBufferTime;

            if (HitCounter > 0 && Time.time > hitCounterExpiresAt) HitCounter = 0;

            if (attackRoutine != null) return;

            if (comboStep > 0 && Time.time > comboExpiresAt) comboStep = 0;

            if (Time.time <= bufferedUntil && controller.CanStartAttack)
            {
                bufferedUntil = -1f;
                attackRoutine = StartCoroutine(Attack());
            }
        }

        public void Equip(WeaponData newWeapon) => weapon = newWeapon != null ? newWeapon : WeaponData.Default;

        IEnumerator Attack()
        {
            controller.IsAttacking = true;
            bool finisher = comboStep >= comboLength - 1;
            AttackStarted?.Invoke(comboStep, finisher);

            yield return new WaitForSeconds(weapon.windup);

            hitThisSwing.Clear();
            HitInfo hit = finisher
                ? weapon.ToHitInfo(controller.Facing, gameObject, finisherDamageMultiplier, finisherKnockbackMultiplier)
                : weapon.ToHitInfo(controller.Facing, gameObject);

            float activeEnd = Time.time + weapon.active;
            do
            {
                Vector2 center = weapon.HitboxCenter(transform.position, controller.Facing);
                int landed = CombatUtils.HitBox(center, weapon.hitboxSize, hittableLayers, hit, gameObject, hitThisSwing);
                if (landed > 0)
                {
                    HitCounter += landed;
                    hitCounterExpiresAt = Time.time + hitCounterTimeout;
                }
                yield return null;
            } while (Time.time < activeEnd);

            yield return new WaitForSeconds(finisher ? weapon.recovery * 1.5f : weapon.recovery);

            comboStep = finisher ? 0 : comboStep + 1;
            comboExpiresAt = Time.time + comboWindow;
            controller.IsAttacking = false;
            attackRoutine = null;
        }

        // Getting hit cancels your attack and resets the combo.
        void Interrupt(HitInfo _)
        {
            if (attackRoutine != null) StopCoroutine(attackRoutine);
            attackRoutine = null;
            controller.IsAttacking = false;
            comboStep = 0;
            HitCounter = 0;
        }

        void OnDrawGizmosSelected()
        {
            var w = weapon != null ? weapon : WeaponData.Default;
            int facing = controller != null ? controller.Facing : 1;
            CombatUtils.DrawBoxGizmo(w.HitboxCenter(transform.position, facing), w.hitboxSize, Color.red);
        }
    }
}
