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

    // Patch fürs Laden unserer Mod-Einstellungen, sobald der lokale Spieler da ist
    [HarmonyPatch(typeof(GameManager), "Update")]
    public class Patch_GameManager_Update
    {
        // Unsere neue, eigene Sperr-Variable
        public static bool IstInitialisiert = false;

        [HarmonyPostfix]
        public static void Postfix()
        {
            // Prüfen: Haben wir es schon gemacht? Ist die Welt geladen? Ist DER LOKALE SPIELER da?
            if (!IstInitialisiert &&
                GameManager.Instance != null &&
                GameManager.Instance.World != null &&
                GameManager.Instance.World.GetPrimaryPlayer() != null)
            {
                IstInitialisiert = true; // Sperre aktivieren, damit es nur 1x läuft

                string savePath = GameIO.GetSaveGameDir();
                if (!string.IsNullOrEmpty(savePath))
                {
                    // LootbagMarker basierend auf den Einstellungen laden und wiederherstellen
                    ModEinstellungen.Laden(savePath);
                    LootbagMarkerManager.Wiederherstellen();
                }
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

            // Update-Schleife für die nächste Sitzung wieder freigeben
            Patch_GameManager_Update.IstInitialisiert = false;

            // UI-Marker für Lootbags zerstören und das statische Gedächtnis leeren
            LootbagMarkerManager.EntferneAlleMarker();
        }
    }
}