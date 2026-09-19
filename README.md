# Ironclad Arena

3D first-person tank combat built in **C# / Unity 6**.

You sit in the hatch of a green hull. The turret aims independently of the tracks. Red enemy armor patrols the yard, acquires you on sight, and returns fire. Clear the field or get hulled.

The level, tanks, lighting, projectiles, and HUD are all spawned from code. No imported art pack required.

## Open in Unity

1. Install **Unity 6.3 LTS** (`6000.3.x`) from Unity Hub. Unity 6.0–6.6 should also import.
2. Add the project folder (`IroncladArena`) in Unity Hub → Open.
3. Open `Assets/Scenes/Arena.unity` if it is not already loaded.
4. Press **Play**.

If Unity asks to upgrade the project version, accept. Built-in render pipeline is used; if a URP template overwrites shaders, the code already falls back to `Universal Render Pipeline/Lit`.

## Controls

| Input | Action |
| --- | --- |
| **W / S** | Forward / reverse |
| **A / D** | Pivot hull |
| **Mouse** | Aim turret (yaw + pitch) |
| **LMB** or **Space** | Fire AP shell |
| **Esc** | Unlock / relock cursor |
| **R** | Restart after the match ends |

## What’s in the project

| Script | Role |
| --- | --- |
| `GameBootstrap` | Starts the match after scene load |
| `ArenaBuilder` | Ground, walls, bunkers, crates, sun, fog |
| `TankFactory` | Player and enemy hull / turret / camera rigs |
| `PlayerTankController` | Drive, independent turret, firing |
| `EnemyTankAI` | Patrol, acquire, aim, shoot |
| `Projectile` | Trigger shells with trails |
| `Health` | Armor, death, explosion kick |
| `GameManager` | Score, remaining hostiles, win / lose |
| `HudUI` | Crosshair, armor bar, end card |
| `CameraShake` | Recoil and hit punch |

## Notes

- Tanks use `Rigidbody` motion with X/Z rotation frozen so they stay planted.
- Enemy count is set in `GameBootstrap` (default 6).
- Tune speed, reload, and damage on `PlayerTankController` and `EnemyTankAI`.
- Playable prototype: primitives instead of authored meshes, OnGUI HUD instead of uGUI canvases.

## License

MIT. Use it, fork it, re-skin it.
