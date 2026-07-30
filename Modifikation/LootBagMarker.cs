using System.Collections.Generic;
using UnityEngine;
using LootBagMarkerMod.Config;

namespace LootBagMarkerMod.LootBagMarker
{
    public static class LootbagMarkerManager
    {
        public static bool IstAktiv { get; private set; } = false;

        // Unser Gedächtnis: Entity-ID -> UI-Marker
        private static Dictionary<int, NavObject> aktiveLootbagMarker = new Dictionary<int, NavObject>();

        // Zählt die Sekunden seit dem letzten Scan
        private static float checkTimer = 0f;

        public static void SetzeModus(bool aktiv)
        {
            if (IstAktiv == aktiv) return;

            IstAktiv = aktiv;

            // Status in der lokalen Config speichern
            ModEinstellungen.LootbagMarkerAktiv = IstAktiv;
            ModEinstellungen.Speichern();

            if (IstAktiv)
            {
                Log.Out("[LootBagMarker] AKTIVIERT.");
                // Timer auf das Maximum setzen, damit sofort beim Start ein initialer Scan ausgeführt wird
                checkTimer = ModEinstellungen.MarkerScanIntervall;
            }
            else
            {
                Log.Out("[LootBagMarker] DEAKTIVIERT. Lösche aktive Marker.");
                EntferneAlleMarker();
            }
        }

        // Wird vom Lade-Patch aufgerufen
        public static void Wiederherstellen()
        {
            IstAktiv = ModEinstellungen.LootbagMarkerAktiv;
            if (IstAktiv)
            {
                Log.Out("[LootBagMarker] Lootbag-Marker aus lokaler Config wiederhergestellt (AKTIV).");
                // Timer auf das Maximum setzen, damit sofort beim Beitritt der Welt gescannt wird
                checkTimer = ModEinstellungen.MarkerScanIntervall;
            }
        }

        public static void OnGameUpdate()
        {
            if (!IstAktiv) return;

            if (GameManager.Instance == null || GameManager.Instance.World == null) return;

            checkTimer += Time.deltaTime;
            if (checkTimer >= ModEinstellungen.MarkerScanIntervall)
            {
                checkTimer = 0f; // Timer für den nächsten Durchlauf zurücksetzen
                ScanLootbags();
            }
        }

        private static void ScanLootbags()
        {
            // Temporäre Liste der Bags, die in DIESER Sekunde physisch in der Welt existieren
            List<int> aktuellGefundeneBags = new List<int>();

            foreach (Entity ent in GameManager.Instance.World.Entities.list)
            {
                // Dank der Vererbung filtern wir hier schon 99% des Welt-Mülls heraus
                if (ent is EntityLootContainer bag)
                {
                    string lootList = bag.GetLootList()?.ToLower() ?? "";

                    // SICHERHEITSFILTER: 
                    // Spieler-Rucksäcke haben meist "backpack" im Namen oder leere Listen.
                    // Zombie-Bags haben Namen wie "zombieLootDropRegular".
                    if (!string.IsNullOrEmpty(lootList) && !lootList.Contains("backpack"))
                    {
                        int bagId = bag.entityId;
                        aktuellGefundeneBags.Add(bagId);

                        // Wenn der Bag noch keinen Marker hat, setzen wir einen!
                        if (!aktiveLootbagMarker.ContainsKey(bagId))
                        {
                            // Wir nutzen die Systemklasse "supply_drop" für das Navigations-Icon
                            NavObject marker = NavObjectManager.Instance.RegisterNavObject("supply_drop", bag.transform, "ui_game_symbol_treasure", false);
                            aktiveLootbagMarker[bagId] = marker;
                        }
                    }
                }
            }

            // AUFRÄUMEN: Wir vergleichen unser Gedächtnis mit der Realität
            List<int> zuLoeschen = new List<int>();
            foreach (var kvp in aktiveLootbagMarker)
            {
                // Wenn ein gemerkter Bag nicht mehr in der aktuellen Welt gefunden wurde...
                if (!aktuellGefundeneBags.Contains(kvp.Key))
                {
                    zuLoeschen.Add(kvp.Key);

                    // ... löschen wir seinen UI-Marker
                    if (NavObjectManager.Instance != null)
                    {
                        NavObjectManager.Instance.UnRegisterNavObject(kvp.Value);
                    }
                }
            }

            // Die gelöschten Bags nun auch aus unserem Skript-Gedächtnis entfernen
            foreach (int id in zuLoeschen)
            {
                aktiveLootbagMarker.Remove(id);
            }
        }

        public static void EntferneAlleMarker()
        {
            if (NavObjectManager.Instance != null)
            {
                foreach (var marker in aktiveLootbagMarker.Values)
                {
                    NavObjectManager.Instance.UnRegisterNavObject(marker);
                }
            }
            aktiveLootbagMarker.Clear();
        }
    }
}