🌐 [English](README.md) | 🇩🇪 [Deutsch](README_DE.md)
---

# LootBagMarker (für 7D2D Version 3.x.x)

## Über diese Mod
Die Mod sucht Loot Bags in deiner Umgebung und markiert diese wie ein Abwurf aus der Luft.

## Installation
1. Lade dir die aktuellste Version der Mod hier herunter: [LootBagMarker Release](../../releases/tag/LootBagMarker)
2. Entpacke die heruntergeladene ZIP-Datei.
3. Platziere den entpackten Ordner `LootBagMarker` in deinem Mod-Verzeichnis unter `%AppData%\7DaysToDie\`.

Die Mod ist rein client seitig und braucht nicht auf dem Server installiert werden. Diese Mod unterstütz kein EAC, d. h. der Server muss EAC abgeschaltet haben.

---

## Konsolenbefehle
Die Mod bringt eigene Befehle für die Ingame-Konsole mit. Du kannst als Präfix wahlweise `lootbagmarker` oder die Kurzform `lbm` verwenden.

### User-Befehle
*   `lbm <on/off>` (oder `true/false`): Aktiviert oder deaktiviert die Marker der Mod.
*   `lbm timer <Sekunden>`: Passt das Scan-Intervall des Radars an (erlaubt sind Werte von 0.1 bis 5.0).[cite: 4] Beispiel: `lbm timer 1.5`.

---

### Bekannte Probleme
Es wird oben im Kompass ein LootBags angezeigt, aber um euch herum ist nirgends ein Marker zu sehen?
Pausiert das Spiel, drückt F1, gebt "le" ein und drückt ENTER.
Nun sollte eine Liste an Objekten auftauchen. Schaut dort nach den Y-Koordinaten des LootBags. Sehr wahrscheinlich ist er unter die Karte gefallen (bzw. fällt noch).
Ihr könnt mit "kill <ID des Lootbags>" und ENTER den Lootbag manuell löschen. Schließt die Konsole dann mit "F1" wieder. Nach dem Beenden der Pause sollte der Marker verschwinden.

---

This mod uses Harmony by Andreas Pardeike, licensed under the MIT License. Many thanks for his work.