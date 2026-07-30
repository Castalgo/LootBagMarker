using System.Reflection;
using HarmonyLib;
using UnityEngine;
using LootBagMarkerMod.LootBagMarker;
using LootBagMarkerMod.Config; // Für ModEinstellungen

namespace LootBagMarkerMod
{
    public class LootBagMarkerInit : IModApi
    {
        // Unsere Sperr-Variable ist nun hier zuhause
        public static bool IstInitialisiert = false;

        public void InitMod(Mod mod)
        {
            if (GameManager.IsDedicatedServer)
            {
                Log.Out("[LootBagMarker] Dedicated Server erkannt. Mod-Initialisierung abgebrochen.");
                return;
            }

            Log.Out("[LootBagMarker] Initialisiere Mod...");

            var harmony = new Harmony("com.castalgo.lootbagmarker");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // Hook in die Update-Schleife für das Radar
            ModEvents.GameUpdate.RegisterHandler((ref ModEvents.SGameUpdateData data) =>
            {
                LootbagMarkerManager.OnGameUpdate();
            });

            // Hook für den Moment, wenn der Spieler (nach Klick auf Spawn) in der Welt erscheint
            ModEvents.PlayerSpawnedInWorld.RegisterHandler((ref ModEvents.SPlayerSpawnedInWorldData data) =>
            {
                // Prüfen: Haben wir das schon geladen? Ist DER LOKALE SPIELER wirklich da?
                if (!IstInitialisiert &&
                    GameManager.Instance != null &&
                    GameManager.Instance.World != null &&
                    GameManager.Instance.World.GetPrimaryPlayer() != null)
                {
                    IstInitialisiert = true; // Sperre rein

                    string savePath = GameIO.GetSaveGameDir();
                    if (!string.IsNullOrEmpty(savePath))
                    {
                        ModEinstellungen.Laden(savePath);
                        LootbagMarkerManager.Wiederherstellen();
                        Log.Out("[LootBagMarker] Lokaler Spieler gespawnt. Mod-Einstellungen erfolgreich geladen.");
                    }
                }
            });

            Log.Out("[LootBagMarker] Mod erfolgreich geladen!");
        }
    }
}