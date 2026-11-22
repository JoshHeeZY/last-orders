using System.Collections;
using UnityEngine;

namespace LastOrders.Core
{
    /// <summary>
    /// Freezes the game for a few hundredths of a second when a hit lands.
    /// This tiny pause is what makes hits feel heavy and crunchy.
    /// Put one of these on a "GameSystems" object in the scene.
    /// </summary>
    public class HitStop : MonoBehaviour
    {
        static HitStop instance;
        Coroutine routine;

        void Awake() => instance = this;

        void OnDisable() => Time.timeScale = 1f;

        public static void Play(float duration)
        {
            if (instance == null || duration <= 0f) return;
            if (instance.routine != null) instance.StopCoroutine(instance.routine);
            instance.routine = instance.StartCoroutine(instance.Freeze(duration));
        }

        IEnumerator Freeze(float duration)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(duration); // real time, because game time is frozen
            Time.timeScale = 1f;
            routine = null;
        }
    }
}
