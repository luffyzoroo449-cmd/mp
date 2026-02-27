# 🎮 Shadow Race: Core Architecture

Welcome to the Shadow Race Unity Project! This repository contains the complete C# logic and engine architecture designed to fulfill the 75-Level AAA Metroidvania PRD.

---

## 📂 Core Folder Structure

The project logic is divided cleanly into namespaces within `Assets/Scripts`:

*   `/AI` - Contains the `EnemyBrain` State Machine, `BossAI` specific interactions, and Regional variants (like `LavaBossAI`).
*   `/Audio` - Contains the singleton `AudioManager` and local `BGMTrigger` zones for managing the 5 Regional Music Themes and SFX.
*   `/Combat` - Contains the data-driven `WeaponData` (ScriptableObjects), the `WeaponManager`, `Projectile` hitboxes, and `LootDrop` systems.
*   `/Core` - Contains ultra-critical singletons like `GameManager` (Save/Load logic), `LevelManager` (Handling level instancing), `HitStopManager` (Juice), and `ObjectPooler` (Memory optimization).
*   `/Player` - Contains the tightly wound `PlayerController` (Movement), `PlayerCombat` (Attacks), `PlayerStats` (HP/MP), and `PlayerInputHandler` (New Input System hooks).
*   `/Quests` - Contains the global `QuestManager` state tracker, the `InteractableNPC` dialogue hooks, and the every-5-level `HiddenQuestBossSpawner`.
*   `/UI` - Handles Canvas interactions. Contains `UIManager` (HUD), `MainMenuManager`, `PauseMenuManager`, `InventoryUI`, `DialogueManager`, `SaveSlotUI`, and `FloatingDamageNumber`.
*   `/World` - Contains level design tools like `ParallaxBackgrounds`, `MovingPlatforms`, `EnvironmentalHazards` (Poison/Lava), `SecretStore` spawners, and the `WeatherManager`.

---

## 🛠️ Getting Started in Unity

1.  **Set up the Input System:** Ensure Unity's *New Input System* package is installed. Read the `walkthrough.md` for exact Action Map names.
2.  **Attach Singletons:** Create an empty `_GameManager` GameObject in your first scene. Attach `GameManager.cs`, `AudioManager.cs`, `QuestManager.cs`, and `ObjectPooler.cs` to it. It will persist across all 75 levels.
3.  **Build the Player:** Create your 2D Player Sprite. Attach a `Rigidbody2D`, `BoxCollider2D`, `Animator`, and the 4 Core Player scripts listed above. Assign Layers!
4.  **Create Weapons:** Right-click in your Project window -> `Create > ShadowRace > Weapon Data` to define swords and guns without needing to code.

*If you encounter any compiler errors, ensure you are utilizing standard Unity 2D URP components and that the TextMeshPro package is installed.*

---

## 📚 Project Documentation

The logic has been paired with rigorous production documents to guide your art and level design team. Check the local `brain` directory for:

*   `production_roadmap.md`
*   `asset_list.md`
*   `budget_estimation.md`
*   `world_lore.md`
*   `level_design_guide.md`
*   `marketing_plan.md`

Good luck, Weaver. The shadows await.
