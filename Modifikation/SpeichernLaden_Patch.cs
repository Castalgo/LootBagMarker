using LootBagMarkerMod.Config;
using LootBagMarkerMod.LootBagMarker;
using HarmonyLib;

namespace LootBagMarkerMod.SaveLoadPatches
{
    // Patch für das Speichern
    [HarmonyPatch(typeof(GameManager), "SaveWorld")]
    public class Patch_SaveGame
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            string savePath = GameIO.GetSaveGameDir();
            if (!string.IsNullOrEmpty(savePath))
            {
                // Einstellungen für dieses Savegame speichern
                ModEinstellungen.Speichern();
            }
        }
    }

    // Patch für das Aufräumen beim Verlassen ins Hauptmenü
    [HarmonyPatch(typeof(GameManager), "SaveAndCleanupWorld")]
    public class Patch_CleanupWorld
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            Log.Out("[LootBagMarker] Spiel wird verlassen. Leere den Arbeitsspeicher...");

            // Update-Schleife für die nächste Sitzung über die Main-Klasse freigeben
            LootBagMarkerInit.IstInitialisiert = false;

            // UI-Marker für Lootbags zerstören und das statische Gedächtnis leeren
            LootbagMarkerManager.EntferneAlleMarker();
        }
    }

}