# Last Orders — Design Doc (living document)

## Pitch
It's the last night before *The Rusty Anchor* closes for good. Last orders are
called and nobody leaves. You're **Dazza** the bouncer, with your dad's cricket
bat. Clear the pub, the car park, the high street and the kebab shop.

**Genre:** 2D side-on beat 'em up (belt-scroller) · **Tone:** chaotic comedy
**Inspiration (feel, not looks):** Streets of Rage 4, Castle Crashers, Crazy Flasher, River City Girls

## Core loop
Story panel cutscene → **Back room** (inventory + shop) → **Fight stage** (waves)
→ earn cash, find weapons → next cutscene.

## Player
- Move left/right + up/down (lanes). Light attack 3-hit combo with finisher.
- Later: heavy attack, dodge, grab/throw.
- Starts with: **Dad's cricket bat** (never breaks, upgradeable), $20, 2 bags of crisps.

## Weapons
| Weapon | Source | Notes |
|---|---|---|
| Cricket bat | Starter | Signature, upgradeable (taped grip, nails) |
| Pool cue | Shop / pickup | Long reach, breaks |
| Bar stool | Pickup | Big hit, 3 uses |
| Bottle | Pickup | Smash or throw |
| Darts | Pickup | Ranged throw |
| Wheelie bin | Pickup | Throw to knock down a row |

## Enemies
- **Drunk bloke**: basic. **Footy lad**: charges in packs. **Hen party**: throws drinks.
- **Mascot bloke**: tanky. **Hugger**: wants a hug. Don't hit him!
- Bosses: Big Kev, The Captain, The Bride, the landlord's ex.

## Hugger mechanic
Huggers walk over with open arms (♥). Let them hug you for a small heal.
Hit them and you lose cash and rating. Some fakers headbutt you instead.

## Stages
1. The pub · 2. The car park · 3. The high street · 4. The kebab shop (final boss)

## Shop (back room)
Bat upgrades, pool cue, bin-lid shield, crisps/meat pie (heals), energy drink (speed), hi-vis vest (armour).

## Scope / roadmap
- [x] **Week 1:** movement, combo, one enemy, hit feel
- [ ] **Week 2:** weapon pickups and durability, heavy attack, combo counter UI
- [ ] **Week 3:** enemy waves, 3 enemy types, enemies taking turns, huggers
- [ ] **Week 4:** cash, inventory, shop, back-room hub, save/load
- [ ] **Week 5:** comic cutscenes, stages 1–2, boss
- [ ] **Week 6:** polish, audio, menus, WebGL build on itch.io, README GIFs
