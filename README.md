# Last Orders 🍺🏏

> It's closing time at the roughest pub in town, and nobody wants to leave.
> You're the bouncer. You've got your dad's old cricket bat.

A 2D comedy beat 'em up made in Unity 6.

<!-- Replace with a gameplay GIF: docs/media/gameplay.gif -->
![Gameplay](docs/media/gameplay.gif)

**▶ Play in your browser:** _itch.io link coming soon_

## Features
- 3-hit combo with input buffering and a finisher
- Hit-stop, screen shake, knockback and hit flash for crunchy combat
- Data-driven weapons (ScriptableObjects): fists, cricket bat, more to come
- Enemy AI that lines up on your lane and telegraphs its attacks
- *(coming)* Pickup weapons, waves, shop, comic-panel cutscenes, the "hugger" mechanic

## Controls
| Action | Keyboard | Gamepad |
|---|---|---|
| Move | WASD / Arrows | Left stick |
| Attack | J / Left mouse | X / Square |

## What I built
<!-- Recruiters read this part. Say what YOU designed and coded. -->
- Combat system: hit detection, combo timing, hit reactions
- Enemy AI state machine
- Weapon data system

## Tech
- Unity 6 (URP 2D), C#, new Input System
- Code: `Assets/_Project/Scripts/`

## AI and asset disclosure
<!-- Be honest; it reads as professional. Update as you go. -->
- The initial starter code (movement, combo, enemy AI, hit feel) was generated with Claude Code as a foundation. I'm extending, tuning and rewriting it from there.
- I use AI assistants (Cursor, Claude Code, Bezi) for help and debugging. Design decisions and tuning are my own.
- Art: placeholder shapes (for now).
- Sound: _tbd_

## Running the project
1. Clone with Git LFS installed: `git lfs install && git clone <repo-url>`
2. Open in Unity 6 via Unity Hub.
3. Open `Assets/_Project/Scenes/TestPub.unity` and press Play.

See [docs/DESIGN.md](docs/DESIGN.md) for the design doc.
