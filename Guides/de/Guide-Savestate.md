# Anleitung Savestate-Datei (Snes9x)

Dies ist eine Schritt-für-Schritt Anleitung der S-RAM-Comparer Konsolenanwendung und dem Vergleichen von Snes9x Savestate-Dateien.

<a href=guides>Übersicht</a>

## ***1)*** Vorbereitung

### Nützliche Links
Schau in <a href="unknowns">Unknowns</a> um Beispiele zu sehen, welche Teile der S-RAM-Struktur noch als unbekannt gelten. Schau dir <a href="imagery">Bilder</a> an wie die Vergleichsergebnisse zu interpretieren sind.

### Konsolenanwendung starten
Starte die Anwendung, indem du den Pfad zur Savestate-Datei (*.000-009 or * .state) des Spiels als ersten Kommandozeilen Parameter übergibst. Die Datei kann auch per Drag 'n' Drop auf die Anwendung gezogen werden.

## ***2)*** S-RAM Vergleichs-Datei erstellen
Anschließend drücke (Overwrite) um eine Kopie deiner aktuellen Speicher-Datei zu erstellen. Diese ermöglicht einen Vergleich nach einer Änderung deiner aktuellen Speicher-Datei.

## ***3)*** Veränderung im Spiel auslösen und S-RAM vergleichen

### S-RAM-Änderung auslösen
Verursache eine Änderung des S-RAMs indem du z.B. ein Spielereignis auslöst oder eine Truhe öffnest und erstelle einen Savestate.

### S-RAM vergleichen
Drücke (Compare) um die aktuelle Speicher-Datei mit der Vergleichs-Datei zu vergleichen. 

## ***4)*** Vergleichsergebnis interpretieren und Fund dokumentieren

### Störendes Rauschen reduzieren
Versuche so wenig wie möglich zwischen zwei Speicherungen zu ändern um unnötiges Bit-Rauschen zu vermeiden. 

### Optimale Vergleichsbedingungen
* Halte Ausschau nach sich-ändernden, einzelnen Bits oder kompletten Bytes. Das Vergleichserbnis wird farblich anders eingefärbt wenn sich entweder nur 1 Bit oder 1 ganzes Byte geändert hat. Schau es dir ggf. <a href="imagery">hier</a> nochmal an.
* Oft werden durch eine Aktion im Spiel zahlreihe Werte geändert. Versuche durch das Wiederholen mit verschiedenen Savegames die Änderungen zu isolieren, die sich immer wieder gleichbleibend ändern.

### Reproduzierbarkeit sicherstellen
Oft bedeuten Wert-Änderungen doch etwas anderes als angenommen. Stelle die Reproduzierbarkeit sicher, indem du folgende Dinge versuchst.

Überprüfe ob die festgestellte Änderung…: 
* auch in anderen Spiel-Versionen auftritt. Z.B. ungepatchte oder gepatchte Versionen
* auch nach dem erneuten Laden deines Spielstandes noch reproduzierbar ist 

### Exportieren des Vergleichs-Ergebnisses
Sobald du reproduzierbar eine einzelne Änderung im S-RAM einer Veränderung im Spiel zuordnen kannst, drücke (Export) um das Vergleichsergebnis als Text-Datei deines Export-Verzeichnisses zu exportieren. Benenne die Datei entsprechend deines Fundes oder deiner Vermutung um.

### Dokumentation
Dokumentiere deinen Fund oder deine Vermutung über die <a href="community">Community</a> um zu vermeiden, dass andere dass selbe vergleichen und dir beim Interpretieren deiner Vergleichsergebnisse helfen können.

## ***5)*** Neuer Vergleich ohne bisherige Änderungen
Um einen Vergleich ohne vorherige S-RAM-Änderungen zu ermöglichen, drücke (Overwrite) um die aktuelle Speicher-Datei als Vergleichs-Datei zu speichern. Anschließend beginne wieder bei Schritt 3.1.

## ***6)*** Vergleichs-Optionen

### (optional) Einzelne oder verschiedene Speicherslots vergleichen
Wenn du mehr als einen Speicherslot mit Änderungen zur Vergleichsdatei hast um den Vergleich des Spiels auf den jeweiligen Speicherslot (1-4) zu beschränken, drücke (Set_Slot). Wenn zwei verschiedene Speicherslot verglichen werden sollen, drücken zusätzlich (SetSlot_Comp), um auch den Speicherplatzz der Vergleichs-Datei festzulegen.

### (optional) Alle oder nur unbekannte Speicherslot-Bereiche vergleichen
Um alle Bytes (inkl. der bekannten Bereiche) eines Speicherslots Byte für Byte zu vergleichen, drücke (SlotByteComp). Wenn du unsicher bist, lass es bei der Voreinstellung um so wenig wie möglich Bytes zu vergleichen.

### (optional) Nicht-Speicherslot Bytes vergleichen
Um die Bytes hinter allen Speicherslots zu vergleichen drücke (NonSlotComp). Derzeit hat es den Anschein, als sei dieser Bereich leer.

## ***7)*** (optional) S-RAM-Dateien sichern und wiederherstellen
Die aktuelle und die Vergleichs-Datei können einzeln gesichert (Backup) bzw. (Backup_Comp) oder wiederhergestellt (Restore) bzw. (Restore_Comp) werden.

## ***8)*** (optional) Offset-Werte anzeigen und ändern
S-RAM Offset-Werte für bestimmte Speicherslots können durch durch Drücken von (Offset) angezeigt oder durch (EditOffset) manipuliert werden. Du kannst entscheiden, ob du die aktuelle Speicher-Datei aktualisieren (Sicherung empfohlen) oder eine neue Datei erstellen möchtest.
