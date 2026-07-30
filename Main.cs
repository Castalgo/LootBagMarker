using System.Reflection;
using HarmonyLib;
using UnityEngine;
using LootBagMarkerMod.LootBagMarker; // Unser neuer, korrekter Namespace

namespace LootBagMarkerMod
{
    public class LootBagMarkerInit : IModApi
    {
        public void InitMod(Mod mod)
        {
            Log.Out("[LootBagMarker] Initialisiere Mod...");

            var harmony = new Harmony("com.castalgo.lootbagmarker");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // Unser Hook in die Update-Schleife der Engine für das Radar
            ModEvents.GameUpdate.RegisterHandler((ref ModEvents.SGameUpdateData data) =>
            {
                LootbagMarkerManager.OnGameUpdate();
            });

            Log.Out("[LootBagMarker] Mod erfolgreich geladen!");
        }
    }
}