🌐 [English](README.md) | 🇩🇪 [Deutsch](README_DE.md)
---

# LootBagMarker (for 7D2D Version 3.x.x)

## About this Mod
The mod searches for loot bags in your vicinity and marks them like an airdrop.

## Installation
1. Download the latest version of the mod here: [LootBagMarker Release](../../releases/tag/LootBagMarker)
2. Extract the downloaded ZIP file.
3. Place the extracted `LootBagMarker` folder in your mods directory under `%AppData%\7DaysToDie\`.

The mod is purely client-side and does not need to be installed on the server. This mod does not support EAC, which means the server must have EAC disabled.

---

## Console Commands
The mod comes with its own commands for the in-game console. You can use either `lootbagmarker` or the short form `lbm` as a prefix.

### User Commands
*   `lbm <on/off>` (or `true/false`): Activates or deactivates the mod's markers.
*   `lbm timer <seconds>`: Adjusts the scan interval of the radar (allowed values are from 0.1 to 5.0). Example: `lbm timer 1.5`.

---

### Known Issues
Is a LootBag icon showing up on your compass, but there is no marker visible anywhere around you?
Pause the game, press F1, type "le", and press ENTER.
A list of objects should now appear. Look for the LootBag's Y-coordinates in the list. It has most likely fallen through the map (and is still falling).
You can manually delete the LootBag by typing "kill <LootBag ID>" and pressing ENTER. Then close the console by pressing F1 again. The marker should disappear once you unpause the game.

---

This mod uses Harmony by Andreas Pardeike, licensed under the MIT License. Many thanks for his work.