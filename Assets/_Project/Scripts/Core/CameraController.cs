using UnityEngine;

namespace LastOrders.Core
{
    /// <summary>
    /// Smoothly follows the player left/right and handles screen shake.
    /// Put this on the Main Camera.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }

        [SerializeField] Transform target;
        [SerializeField] float smoothTime = 0.15f;
        [SerializeField] float offsetX = 0f;
        [Tooltip("Beat 'em ups usually only scroll sideways. Turn on to follow up/down too.")]
        [SerializeField] bool followY = false;
        [SerializeField] float offsetY = 1f;

        Vector3 basePosition;
        Vector3 velocity;
        float shakeTimeLeft;
        float shakeStrength;

        void Awake()
        {
            Instance = this;
            basePosition = transform.position;
        }

        void LateUpdate()
        {
            if (target != null)
            {
                Vector3 desired = new Vector3(
                    target.position.x + offsetX,
                    followY ? target.position.y + offsetY : basePosition.y,
                    basePosition.z);
                basePosition = Vector3.SmoothDamp(basePosition, desired, ref velocity, smoothTime);
            }

            Vector3 shake = Vector3.zero;
            if (shakeTimeLeft > 0f)
            {
                shakeTimeLeft -= Time.unscaledDeltaTime; // keeps shaking during hit-stop
                shake = (Vector3)(Random.insideUnitCircle * shakeStrength);
            }

            transform.position = basePosition + shake;
        }

        public static void Shake(float strength, float duration = 0.12f)
        {
            if (Instance == null || strength <= 0f) return;
            Instance.shakeStrength = Instance.shakeTimeLeft > 0f
                ? Mathf.Max(Instance.shakeStrength, strength)
                : strength;
            Instance.shakeTimeLeft = Mathf.Max(Instance.shakeTimeLeft, duration);
        }
    }
}
