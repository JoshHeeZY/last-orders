using UnityEngine;
using UnityEngine.InputSystem;

namespace LastOrders.Core
{
    /// <summary>
    /// One place to read player input (keyboard, mouse and gamepad).
    /// Uses the new Input System directly so there's no input asset to set up yet.
    /// </summary>
    public static class GameInput
    {
        public static Vector2 Move()
        {
            Vector2 move = Vector2.zero;

            var kb = Keyboard.current;
            if (kb != null)
            {
                if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  move.x -= 1f;
                if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) move.x += 1f;
                if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  move.y -= 1f;
                if (kb.wKey.isPressed || kb.upArrowKey.isPressed)    move.y += 1f;
            }

            var pad = Gamepad.current;
            if (pad != null)
            {
                Vector2 stick = pad.leftStick.ReadValue();
                if (stick.sqrMagnitude > move.sqrMagnitude) move = stick;
            }

            return Vector2.ClampMagnitude(move, 1f);
        }

        /// <summary>Light attack: J, left mouse, or X / Square on a gamepad.</summary>
        public static bool AttackPressed()
        {
            var kb = Keyboard.current;
            var mouse = Mouse.current;
            var pad = Gamepad.current;

            return (kb != null && kb.jKey.wasPressedThisFrame)
                || (mouse != null && mouse.leftButton.wasPressedThisFrame)
                || (pad != null && pad.buttonWest.wasPressedThisFrame);
        }
    }
}
