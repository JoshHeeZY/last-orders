using LastOrders.Combat;
using UnityEngine;

namespace LastOrders.Data
{
    /// <summary>
    /// A weapon's stats as a data asset. Make new ones with
    /// right-click > Create > Last Orders > Weapon.
    /// Attacks go: windup (before the hit) > active (hitbox on) > recovery (can't act).
    /// </summary>
    [CreateAssetMenu(menuName = "Last Orders/Weapon", fileName = "NewWeapon")]
    public class WeaponData : ScriptableObject
    {
        public string displayName = "Fists";
        public Sprite icon;

        [Header("Damage")]
        public int damage = 8;
        public float knockback = 2.5f;

        [Header("Hitbox (relative to the character's feet)")]
        [Tooltip("How far in front of the character the hitbox centre is.")]
        public float hitboxOffset = 0.7f;
        [Tooltip("X = reach, Y = how far up/down the lane it can hit.")]
        public Vector2 hitboxSize = new(0.9f, 0.5f);

        [Header("Timing (seconds)")]
        public float windup = 0.05f;
        public float active = 0.06f;
        public float recovery = 0.12f;

        [Header("Game feel")]
        public float hitStop = 0.04f;
        public float shake = 0.05f;

        [Header("Durability and shop")]
        [Tooltip("0 = never breaks (the cricket bat).")]
        public int durability = 0;
        public int price = 0;

        public HitInfo ToHitInfo(int facing, GameObject attacker,
                                 float damageMultiplier = 1f, float knockbackMultiplier = 1f)
        {
            return new HitInfo
            {
                damage = Mathf.RoundToInt(damage * damageMultiplier),
                knockback = new Vector2(facing * knockback * knockbackMultiplier, 0f),
                hitStop = hitStop,
                shake = shake,
                attacker = attacker,
            };
        }

        public Vector2 HitboxCenter(Vector2 origin, int facing) =>
            origin + new Vector2(facing * hitboxOffset, 0f);

        static WeaponData fallback;

        /// <summary>Plain fists, used if no weapon asset is assigned.</summary>
        public static WeaponData Default
        {
            get
            {
                if (fallback == null)
                {
                    fallback = CreateInstance<WeaponData>();
                    fallback.name = "Fists (default)";
                }
                return fallback;
            }
        }
    }
}
