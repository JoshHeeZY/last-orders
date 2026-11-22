# Last Orders — rules for AI assistants (Cursor, Claude Code, Bezi)

## Project
- 2D side-on beat 'em up (belt-scroller) in **Unity 6**, C#, URP 2D.
- Player is a pub bouncer clearing out drunks at closing time. Comedy tone.
- This is a **portfolio project**: the owner is learning. Explain changes clearly
  and prefer readable code over clever code.

## Code conventions
- All game code lives in `Assets/_Project/Scripts/<Area>/` with namespace `LastOrders.<Area>`
  (Core, Combat, Data, Player, Enemies, UI).
- One MonoBehaviour per file, file name = class name.
- Private serialized fields: `[SerializeField] float moveSpeed;` (camelCase, no underscores).
- Tunable numbers go in `[SerializeField]` fields or ScriptableObjects, not magic numbers.
- Weapons/items are `ScriptableObject` data (`WeaponData`). Don't hard-code weapon stats.
- Use the **new Input System** through `LastOrders.Core.GameInput` — don't use `UnityEngine.Input`.
- Use `Rigidbody2D.linearVelocity` (Unity 6), not `velocity`.
- Damage goes through `IDamageable.TakeHit(HitInfo)`; hits are found with `CombatUtils.HitBox`.

## Never do without asking
- Don't edit `.unity`, `.prefab`, `.asset` or `.meta` files as text.
- Don't add packages or third-party assets.
- Don't touch `ProjectSettings/` or `Packages/manifest.json`.
- Don't delete or rename existing serialized fields (it breaks inspector values).

## When making changes
- Keep changes small and focused on the task.
- After writing code, briefly explain what it does and how to test it in the editor.
