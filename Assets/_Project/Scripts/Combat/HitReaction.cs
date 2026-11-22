using System.Collections;
using LastOrders.Core;
using UnityEngine;

namespace LastOrders.Combat
{
    /// <summary>
    /// Makes getting hit look and feel good: white flash, knockback, stun,
    /// hit-stop and screen shake. Shared by the player and enemies.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class HitReaction : MonoBehaviour
    {
        [SerializeField] float stunTime = 0.3f;
        [Range(0f, 1f)]
        [SerializeField] float knockbackResistance = 0f;

        [Header("Flash")]
        [Tooltip("Leave empty to use every SpriteRenderer in children.")]
        [SerializeField] SpriteRenderer[] renderers;
        [SerializeField] Color flashColor = Color.white;
        [SerializeField] float flashTime = 0.08f;

        Health health;
        Rigidbody2D rb;
        Color[] originalColors;
        float stunnedUntil;
        Coroutine flashRoutine;

        public bool IsStunned => Time.time < stunnedUntil;

        void Awake()
        {
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody2D>();
            if (renderers == null || renderers.Length == 0)
                renderers = GetComponentsInChildren<SpriteRenderer>();

            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++) originalColors[i] = renderers[i].color;
        }

        void OnEnable()  => health.OnHit += React;
        void OnDisable() => health.OnHit -= React;

        void React(HitInfo hit)
        {
            stunnedUntil = Time.time + stunTime;

            if (rb != null) rb.linearVelocity = hit.knockback * (1f - knockbackResistance);

            HitStop.Play(hit.hitStop);
            CameraController.Shake(hit.shake);

            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(Flash());
        }

        // NOTE: tinting works for the coloured placeholder squares.
        // With real sprite art you'll want a "flash white" shader instead.
        IEnumerator Flash()
        {
            for (int i = 0; i < renderers.Length; i++) renderers[i].color = flashColor;
            yield return new WaitForSecondsRealtime(flashTime);
            for (int i = 0; i < renderers.Length; i++) renderers[i].color = originalColors[i];
            flashRoutine = null;
        }
    }
}
