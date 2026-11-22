using UnityEngine;

namespace LastOrders.Core
{
    /// <summary>
    /// Draws characters lower on the screen in front of characters higher up,
    /// so people standing "closer" to the camera overlap the ones behind them.
    /// Put this on the root of every character.
    /// </summary>
    public class YSort : MonoBehaviour
    {
        [SerializeField] int orderOffset = 0;
        [SerializeField] float precision = 100f;

        SpriteRenderer[] renderers;
        int[] baseOrders;

        void Awake()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();
            baseOrders = new int[renderers.Length];
            for (int i = 0; i < renderers.Length; i++) baseOrders[i] = renderers[i].sortingOrder;
        }

        void LateUpdate()
        {
            int order = orderOffset - Mathf.RoundToInt(transform.position.y * precision);
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].sortingOrder = order + baseOrders[i];
        }
    }
}
