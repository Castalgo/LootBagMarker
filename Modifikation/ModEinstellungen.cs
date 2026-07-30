using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace LootBagMarkerMod.Config
{
    public static class ModEinstellungen
    {
        public static bool LootbagMarkerAktiv = false;
        public static float MarkerScanIntervall = 2.0f; // NEU: Unser Standardwert

        public static void Laden(string saveDir)
        {
            string configPfad = Path.Combine(saveDir, "ModConfig.json");
            if (File.Exists(configPfad))
            {
                try
                {
                    string json = File.ReadAllText(configPfad);
                    var config = JsonConvert.DeserializeObject<ConfigDaten>(json);
                    if (config != null)
                    {
                        LootbagMarkerAktiv = config.LootbagMarkerAktiv;
                        MarkerScanIntervall = config.MarkerScanIntervall; // NEU
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"[LootBagMarker] Fehler beim Laden der lokalen Config: {e.Message}");
                }
            }
            else
            {
                // Standardwerte, falls noch keine Config existiert
                LootbagMarkerAktiv = false;
                MarkerScanIntervall = 2.0f; // NEU
            }
        }

        public static void Speichern()
        {
            string saveDir = GameIO.GetSaveGameDir();
            if (string.IsNullOrEmpty(saveDir)) return;

            string configPfad = Path.Combine(saveDir, "ModConfig.json");
            try
            {
                var config = new ConfigDaten
                {
                    // alphabetische Reihenfolge der Eigenschaften
                    LootbagMarkerAktiv = LootbagMarkerAktiv,
                    MarkerScanIntervall = MarkerScanIntervall // NEU
                };
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configPfad, json);
            }
            catch (Exception e)
            {
                Log.Error($"[LootBagMarker] Fehler beim Speichern der lokalen Config: {e.Message}");
            }
        }

        private class ConfigDaten
        {
            public bool LootbagMarkerAktiv { get; set; } = false;
            public float MarkerScanIntervall { get; set; } = 2.0f; // NEU
        }
    }
}