# Anleitung SRM-Datei

Dies ist eine Schritt-für-Schritt Anleitung der SRAM-Comparer Konsolenanwendung und dem Vergleichen von SRM-Dateien.

<a href=guides>Übersicht</a>

## ***1)*** Vorbereitung

### Nützliche Links
Schau in <a href="unknowns">Unknowns</a> um Beispiele zu sehen, welche Teile der SRAM-Struktur noch als unbekannt gelten. Schau dir <a href="imagery">Bilder</a> an wie die Vergleichsergebnisse zu interpretieren sind.

### Emulator konfigurieren
Die meisten Emulatoren haben die Einstellung das SRAM eines Spiels zu speichern, sobald eine Änderung eintritt. 
Stelle sicher, dass diese Einstellung aktiviert ist. Andernfalls musst du selbst sicherstellen, dass der Emulator die *.srm-Datei aktualisiert. 

### Konsolenanwendung starten
Starte die Anwendung, indem du den Pfad zur Speicher-Datei (*.srm) des Spiels als ersten Kommandozeilen Parameter übergibst. Die Datei kann auch per Drag 'n' Drop auf die Anwendung gezogen werden.

## ***2)*** SRAM Vergleichs-Datei erstellen
Anschließend drücke (overwrite) um eine Kopie deiner aktuellen SRAM Datei zu erstellen. Diese ermöglicht einen Vergleich nach einer Änderung deiner aktuellen Speicher-Datei.

## ***3)*** Veränderung im Spiel auslösen und SRAM vergleichen

### SRAM-Änderung auslösen
Verursache im Spiel eine Änderung des SRAMs (z.B. löse ein Spielereignis aus oder öffne eine ungeöffnete Truhe). Damit die Speicher-Datei aktualisiert wird speichere deinen Spielstand im Spiel selbst bei einer Speichermöglichkeit.

### SRAM vergleichen
Drücke (Compare) um die aktuelle Speicher-Datei mit der Vergleichs-Datei zu vergleichen. 
     (Falls sich die Speicher-Datei überhaupt nicht geändert hat, hat *vermutlich* der Emulator die Datei (noch!) nicht automatisch aktualisiert. Überprüfe das Änderungsdatum der *.srm-Datei.
     Beispiel: der von Snes9x voreingestellte Aktualisierungszeitraum ist 30 Sekunden. Verringere den Wert auf z. B. 1 Sekunde,
     aber nicht auf 0! (was zur Deaktivierung des Automatismus führt)

## ***4)*** Vergleichsergebnis interpretieren und Fund dokumentieren

### Störendes Rauschen reduzieren
Versuche so wenig wie möglich zwischen zwei Speicherungen zu ändern um unnötiges Bit-Rauschen zu vermeiden. 

### Optimale Vergleichsbedingungen
* Halte Ausschau nach sich ändernden, einzelnen Bits oder kompletten Bytes. Das Vergleichserbnis wird farblich anders eingefärbt wenn sich entweder nur 1 Bit oder 1 ganzes Byte geändert hat. Schau es dir ggf. <a href="imagery">hier</a> nochmal an.
* Oft werden durch eine Aktion im Spiel zahlreihe Werte geändert. Versuche durch das Wiederholen mit verschiedenen Savegames die Änderungen zu isolieren, die sich immer wieder gleichbleibend ändern.

### Reproduzierbarkeit sicherstellen
Oft bedeuten Wert-Änderungen doch etwas anderes als angenommen. Stelle die Reproduzierbarkeit sicher, indem du folgende Dinge versuchst.

Überprüfe ob die festgestellte Änderung…: 
* auch in anderen Spiel-Versionen auftritt. Z.B. ungepatchte oder gepatchte Versionen
* auch nach dem erneuten Laden deines Spielstandes noch reproduzierbar ist 

### Exportieren des Vergleichs-Ergebnisses
Sobald du reproduzierbar eine einzelne Änderung im SRAM einer Veränderung im Spiel zuordnen kannst, drücke (Export) um das Vergleichsergebnis als Text-Datei deines Export-Verzeichnisses zu exportieren. Benenne die Datei entsprechend deines Fundes oder deiner Vermutung um.

### Dokumentation
Dokumentiere deinen Fund oder deine Vermutung über die <a href="community">Community</a> um zu vermeiden, dass andere dass selbe vergleichen und dir beim Interpretieren deiner Vergleichsergebnisse helfen können.

## ***5)*** Neuer Vergleich ohne bisherige Änderungen
Um einen Vergleich ohne vorherige SRAM-Änderungen zu ermöglichen, drücke (overwrite) um die aktuelle Speicher-Datei als Vergleichs-Datei zu speichern. Anschließend beginne wieder bei Schritt 3.1.

## ***6)*** Vergleichs-Optionen

### (optional) Einzelne oder verschiedene Speicherslots vergleichen
Wenn du mehr als einen Speicherslot mit Änderungen zur Vergleichsdatei hast um den Vergleich des Spiels auf den jeweiligen Speicherslot (1-4) zu beschränken, drücke (set_slot). Wenn zwei verschiedene Speicherslot verglichen werden sollen, drücken zusätzlich (SetSlot_Comp), um auch den Speicherplatzz der Vergleichs-Datei festzulegen.

### (optional) Alle oder nur unbekannte Speicherslot-Bereiche vergleichen
Um alle Bytes (inkl. der bekannten Bereiche) eines Speicherslots Byte für Byte zu vergleichen, drücke (sbc). Wenn du unsicher bist, lass es bei der Voreinstellung um so wenig wie möglich Bytes zu vergleichen.

### (optional) Nicht-Speicherslot Bytes vergleichen
Um die Bytes hinter allen Speicherslots zu vergleichen drücke (nsbc). Derzeit hat es den Anschein, als sei dieser Bereich leer.

## ***7)*** (optional) SRAM-Dateien sichern und wiederherstellen
Die aktuelle und die Vergleichs-Datei können einzeln gesichert (Backup) bzw. (Backup_Comp) oder wiederhergestellt (Restore) bzw. (Restore_Comp) werden.

## ***8)*** (optional) Offset-Werte anzeigen und ändern
SRAM Offset-Werte für bestimmte Speicherslot können durch durch Drücken von (Offset) angezeigt oder durch (EditOffset) manipuliert werden. Du kannst entscheiden, ob du die aktuelle Speicher-Datei aktualisieren (Sicherung empfohlen) oder eine neue Datei erstellen möchtest.
