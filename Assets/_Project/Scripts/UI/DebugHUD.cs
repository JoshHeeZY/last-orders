using LastOrders.Combat;
using LastOrders.Player;
using UnityEngine;

namespace LastOrders.UI
{
    /// <summary>
    /// Quick on-screen info while prototyping. Replace with a proper UI later.
    /// Put it on the GameSystems object and drag the Player in.
    /// </summary>
    public class DebugHUD : MonoBehaviour
    {
        [SerializeField] Health playerHealth;
        [SerializeField] PlayerCombat playerCombat;

        GUIStyle style;

        void OnGUI()
        {
            style ??= new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };

            GUILayout.BeginArea(new Rect(20, 20, 500, 200));
            if (playerHealth != null)
                GUILayout.Label($"HP: {playerHealth.Current} / {playerHealth.Max}", style);
            if (playerCombat != null)
            {
                GUILayout.Label($"Weapon: {playerCombat.Weapon.displayName}", style);
                if (playerCombat.HitCounter > 1)
                    GUILayout.Label($"{playerCombat.HitCounter} HIT COMBO!", style);
            }
            GUILayout.EndArea();
        }
    }
}
