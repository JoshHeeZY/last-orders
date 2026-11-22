using System.Collections.Generic;
using UnityEngine;

namespace LastOrders.Combat
{
    public static class CombatUtils
    {
        static readonly List<Collider2D> results = new();

        /// <summary>
        /// Checks a box for anything hittable and hits each target once per swing.
        /// <paramref name="alreadyHit"/> stops one swing hitting the same enemy every frame.
        /// Returns how many new targets were hit.
        /// </summary>
        public static int HitBox(Vector2 center, Vector2 size, LayerMask layers, HitInfo hit,
                                 GameObject attacker, HashSet<IDamageable> alreadyHit)
        {
            var filter = new ContactFilter2D { useTriggers = true };
            filter.SetLayerMask(layers);

            results.Clear();
            Physics2D.OverlapBox(center, size, 0f, filter, results);

            int count = 0;
            foreach (var col in results)
            {
                if (col.transform.root == attacker.transform.root) continue; // don't hit yourself

                var target = col.GetComponentInParent<IDamageable>();
                if (target == null || alreadyHit.Contains(target)) continue;

                alreadyHit.Add(target);
                target.TakeHit(hit);
                count++;
            }
            return count;
        }

        public static void DrawBoxGizmo(Vector2 center, Vector2 size, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
