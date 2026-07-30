using System.Collections.Generic;
using UnityEngine;
using LootBagMarkerMod.Config;
using LootBagMarkerMod.LootBagMarker;

namespace LootBagMarkerMod.Commands
{
    public class ConsoleCmdLootBagMarker : ConsoleCmdAbstract
    {
        private const string HilfeText =
            "=== LootBag Marker Befehle ===\n" +
            "Nutze 'lbm on' oder 'lbm off' um die Mod zu aktivieren oder zu deaktivieren.\n" +
            "Nutze 'lbm timer <Sekunden>' um das Scan-Intervall anzupassen (z.B. lbm timer 1.5).";

        // BEIDE Befehle (Lang- und Kurzform) sind nun gültig
        public override string[] getCommands()
        {
            return new string[] { "lootbagmarker", "lbm" };
        }

        public override string getDescription()
        {
            return HilfeText;
        }

        // Die zwingend erforderliche Eingangstür für die Engine
        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            if (_params.Count == 0)
            {
                Log.Out(HilfeText);
                return;
            }

            string subCommand = _params[0].ToLower();

            // Direkte, saubere Verarbeitung der Parameter
            switch (subCommand)
            {
                case "on":
                case "true":
                    LootbagMarkerManager.SetzeModus(true);
                    break;
                case "off":
                case "false":
                    LootbagMarkerManager.SetzeModus(false);
                    break;
                case "timer":
                case "time":
                    CmdTimer(_params);
                    break;
                default:
                    Log.Warning($"[LootBagMarker] Unbekannter Befehl '{subCommand}'.\n{HilfeText}");
                    break;
            }
        }

        // Die ausgelagerte DAU-Sicherheitsprüfung für den Timer
        private void CmdTimer(List<string> _params)
        {
            if (_params.Count < 2 || !float.TryParse(_params[1], out float neuerTimer))
            {
                Log.Warning($"[LootBagMarker] Aktuelles Intervall: {ModEinstellungen.MarkerScanIntervall}s. Bitte nutze 'lbm timer <Zahl>', z.B. 'lbm timer 1.5'.");
                return;
            }

            if (neuerTimer < 0.1f || neuerTimer > 5.0f)
            {
                Log.Warning($"[LootBagMarker] FEHLER: Der Wert {neuerTimer} ist ungültig!");
                Log.Warning("[LootBagMarker] Das Intervall darf nicht kleiner als 0.1 oder größer als 5.0 sein.");
                Log.Warning("[LootBagMarker] Beispiel für eine gültige Eingabe: 'lbm timer 1.5'");
                return;
            }

            ModEinstellungen.MarkerScanIntervall = neuerTimer;
            ModEinstellungen.Speichern();
            Log.Out($"[LootBagMarker] ERFOLG! Das Radar-Scan-Intervall wurde auf {neuerTimer} Sekunden gesetzt.");
        }
    }
}