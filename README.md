# Vehicles2D

A small Unity 2D top-down tank game demonstrating modular gameplay architecture, AI behaviors, save/load system and extensible vehicle logic.

## Features

- Enemy AI with interchangeable behavior modules
- Save / Load system
- Tilemap-based level
- Pause and Result windows
- Modular architecture for extending gameplay
- Team-based projectile system

---

## Project Structure

```
Assets
│
├── GameResources
│   ├── DefaultMap        # Tilemap resources
│   └── Game              # Game prefabs, sprites and gameplay resources
│
└── Scripts
    ├── Config            # ScriptableObject configurations
    ├── Core              # Core systems
    ├── Enums             # Enumerations
    ├── Gameplay          # Gameplay logic
    └── UI                # User Interface
```

---

## Controls

| Key | Action |
|------|--------|
| W / S | Move Forward / Backward |
| A / D | Rotate Tank |
| Esc | Pause Menu |

---

## Architecture

### Enemy AI

Enemy behaviour consists of independent modules.

The enemy type is selected inside **EnemySpawner**, while the target detection logic is selected inside each enemy implementation.

Example:

```
EnemySpawner
    ↓
SimpleEnemy
    ↓
PlayerFinder
```

This allows different enemy types to reuse movement while changing only the detection logic.

---

### Vehicle Movement

Currently only one movement implementation exists:

```
VehicleMovement
      ↑
TankMovement
```

Additional movement types can easily be introduced by inheriting from `VehicleMovement`.

---

### Weapons

The current implementation uses `SimpleGun`.

Each projectile receives a **Team** value which allows friendly/enemy hit detection.

Bot shooting behaviour can be implemented by creating another enemy type using the same weapon system.

---

### Save System

Currently the game state can be saved only from:

- Pause Window
- Result Window

Current project limitations:

- Existing projectiles are not saved.
- Existing projectiles are not restored after loading.

---

### UI

Each window owns its own Canvas.

```
Gameplay UI
Pause UI
Result UI
```

Only the currently active Canvas is enabled, reducing unnecessary UI updates.

---

## Known Limitations

- Menus can only be opened using **Esc**.
- Existing projectiles are not included in save files.
- Rarely an enemy after collision with another enemy can go backwards for 1 sec and ignore player finding

---

## Future Improvements

- Unlock mouse while holding key "ctrl"
- Multiple AI behaviours
- Additional weapon types
- Better save serialization
- Multiple movement implementations
- Audio system
- Effect system
- Menu Scene

---

# Notes

- Enemy pursuit can be escaped by moving outside their detection area.
- Mouse cursor remains locked during gameplay and unlock during opened popups.
- Enemy pursuit can be escaped by moving outside their detection area.
- There is my ServiceLocator in the project.
- There is my ServiceLocator in the project.

---

# Author

Gleb Omelnitskji

Unity Developer