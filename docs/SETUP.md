# Initial setup — get punching in about 30 minutes

You'll end up with a blue square (you) punching red squares (drunks) with
a 3-hit combo, hit-stop, screen shake and knockback. Ugly on purpose:
**combat feel first, art later.**

## 1. Create the Unity project
1. Open **Unity Hub** > **New project**.
2. Pick **Unity 6** (any 6000.x LTS version) and the **Universal 2D** template.
3. Name it `LastOrders` and create it.
4. Close Unity once it opens.

## 2. Add the starter kit
1. Unzip `last-orders-starter.zip`.
2. Copy **everything inside it** (including hidden `.gitignore` / `.gitattributes`)
   into the `LastOrders` project folder, the one that already has `Assets/`,
   `Packages/` and `ProjectSettings/`. Say yes to merging the `Assets` folder.
3. Reopen the project in Unity. It will compile the scripts.
   - If it asks to enable the new Input System backends, click **Yes** (Unity restarts).

## 3. Layers and tags
**Edit > Project Settings > Tags and Layers**
- Add layers: `Player`, `Enemy`.
- The `Player` tag already exists.

*Optional:* **Project Settings > Physics 2D > Layer Collision Matrix**, untick
`Enemy`/`Enemy` so drunks don't shove each other around.

## 4. Weapon assets
In `Assets/_Project/`, make a folder `Data/Weapons`. Right-click in it >
**Create > Last Orders > Weapon** three times and fill in:

| Field            | Fists | Cricket Bat | Enemy Punch |
|------------------|-------|-------------|-------------|
| Display Name     | Fists | Cricket Bat | Drunk Swing |
| Damage           | 8     | 18          | 10          |
| Knockback        | 2.5   | 5           | 3           |
| Hitbox Offset    | 0.7   | 1.0         | 0.7         |
| Hitbox Size      | 0.9, 0.5 | 1.4, 0.6 | 1.0, 0.5    |
| Windup           | 0.05  | 0.1         | **0.4** (the "tell") |
| Active           | 0.06  | 0.08        | 0.08        |
| Recovery         | 0.12  | 0.2         | 0.4         |
| Hit Stop         | 0.04  | 0.07        | 0.05        |
| Shake            | 0.05  | 0.15        | 0.1         |

## 5. Build the test scene
Save the open scene as `Assets/_Project/Scenes/TestPub.unity`, then add it in
**File > Build Profiles > Scene List** (needed for restart-on-death).

### Floor
- **Create > 2D Object > Sprites > Square**, name it `Floor`.
- Position `(0, -1.75, 0)`, Scale `(40, 4, 1)`, colour dark grey/brown.
- Sprite Renderer > **Order in Layer: -1000**.

### Player
1. **Create Empty**, name `Player`, position `(-3, -1.5, 0)`.
   Tag **Player**, Layer **Player**.
2. Add components:
   - **Rigidbody2D**: Gravity Scale 0, Interpolate = Interpolate, Constraints > Freeze Rotation Z.
   - **BoxCollider2D**: Size `(0.8, 0.3)`, Offset `(0, 0)`. This is the *feet*.
   - **Health** (Max 100), **HitReaction**, **PlayerController**, **PlayerCombat**, **YSort**.
3. Right-click Player > **2D Object > Sprites > Square**, name it `Visual`.
   Position `(0, 1, 0)`, Scale `(1, 2, 1)`, colour **blue**.
   Add a tiny child square on its right side as a "fist" so you can see which way you face.
4. On **PlayerController**: drag `Visual` into *Visual*.
5. On **PlayerCombat**: Weapon = **Cricket Bat** (or Fists), Hittable Layers = **Enemy**.

### Enemy
1. Duplicate the Player, rename it `Drunk`. Position `(3, -1.5, 0)`.
   Tag **Untagged**, Layer **Enemy** (apply to children too).
2. Remove **PlayerController** and **PlayerCombat**. Add **EnemyAI**.
3. Colour the Visual **red**. Health Max = 40.
4. On **EnemyAI**: Attack = **Enemy Punch**, Hittable Layers = **Player**, Visual = its `Visual`.
5. Drag it into `Assets/_Project/Prefabs/` to make a prefab, then place 2–3 in the scene.

### Camera and systems
1. **Main Camera**: add **CameraController**, drag Player into *Target*.
   Set camera position Y to about `0` so the floor fills the bottom of the screen.
2. **Create Empty** named `GameSystems`: add **HitStop** and **DebugHUD**
   (drag the Player into both HUD slots).

## 6. Play
| Action | Keyboard | Gamepad |
|---|---|---|
| Move | WASD / arrows | Left stick |
| Attack (3-hit combo) | J or left mouse | X / Square |

## 7. Tune it until it feels good
This is the real work right now. Things to try:
- Hit stop 0.04 vs 0.1. Too long feels laggy, too short feels weak.
- Finisher knockback multiplier on PlayerCombat.
- Enemy Punch windup. Can you react to it? Should you be able to?
- Select a character in the Scene view to see its hitbox (red/orange) and aggro range (yellow).

## 8. First commit
```bash
git lfs install
git init
git add .
git commit -m "Add Unity project files"
```
Then create an empty repo on GitHub called `last-orders` and follow its
"push an existing repository" instructions.
