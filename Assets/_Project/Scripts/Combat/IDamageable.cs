using UnityEngine;

namespace LastOrders.Combat
{
    /// <summary>Everything about a single hit: how hard, which way, and how much "juice".</summary>
    public struct HitInfo
    {
        public int damage;
        public Vector2 knockback;   // velocity applied to whoever gets hit
        public float hitStop;       // freeze-frame length in seconds
        public float shake;         // screen shake strength
        public GameObject attacker;
    }

    /// <summary>Anything that can be hit: players, enemies, and later breakable props.</summary>
    public interface IDamageable
    {
        void TakeHit(HitInfo hit);
    }
}
