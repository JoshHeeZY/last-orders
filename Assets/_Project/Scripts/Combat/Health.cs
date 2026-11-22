using System;
using UnityEngine;

namespace LastOrders.Combat
{
    /// <summary>Tracks health and tells other scripts when we get hit or die.</summary>
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] int maxHealth = 100;
        [Tooltip("Short window after a hit where further hits are ignored.")]
        [SerializeField] float invulnerableTime = 0.1f;

        public int Current { get; private set; }
        public int Max => maxHealth;
        public bool IsDead => Current <= 0;

        public event Action<HitInfo> OnHit;
        public event Action OnDeath;

        float invulnerableUntil;

        void Awake() => Current = maxHealth;

        public void TakeHit(HitInfo hit)
        {
            if (IsDead || Time.time < invulnerableUntil) return;

            Current = Mathf.Max(0, Current - hit.damage);
            invulnerableUntil = Time.time + invulnerableTime;

            OnHit?.Invoke(hit);
            if (IsDead) OnDeath?.Invoke();
        }

        public void Heal(int amount)
        {
            if (IsDead) return;
            Current = Mathf.Min(maxHealth, Current + amount);
        }
    }
}
