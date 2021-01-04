## Anleitung
**Schritte**:

***1.1)*** Schau in <a href="unknowns">Unknowns</a> um Beispiele zu sehen, welche Teile der SRAM-Struktur noch als unbekannt gelten. Schau dir <a href="imagery">Bilder</a> an wie die Vergleichsergebnisse zu interpretieren sind.
***1.2)*** Die meisten Emulatoren haben die Einstellung das SRAM eines Spiels zu speichern sobald eine Änderung eintritt. 
     Stelle sicher, dass diese Einstellung aktiviert ist. Andernfalls musst du selbst sicherstellen, dass der Emulator die *.srm-Datei aktualisiert.
***1.3)*** Starte die Anwendung indem du den Pfad zur Srm Datei des Spiels als ersten Kommandozeilen Parameter übergibst. Die Datei kann auch per Drag 'n' Drop auf die Anwendung gezogen werden.

***2)***   Anschließend drücke (ow) um eine Kopie deiner aktuellen SRAM Datei zu erstellen. Diese ermöglicht einen Vergleich nach einer Änderung deiner aktuellen SRAM-Datei.

***3.1)*** Verursache im Spiel eine Änderung des SRAMs. (z.B. löse ein Spielereignis aus oder öffne eine ungeöffnete Truhe) Damit die SRAM-Datei aktualisiert wird speichere deinen Spielstand im Spiel selbst bei einer Speichermöglichkeit.
***3.2)*** Drücke (c) um die aktuelle SRAM-Datei mit der Vergleichs-Datei zu verlgeichen. 
     (Fass sich die SRAM-Datei überhaupt nicht geändert hat, hast du vermutlich nicht im Spiel gespeichert oder der Emulator hat die Datei (noch!) nicht automatisch aktualisiert. Überprüfe das Änderungsdatum der *.srm-Datei.
     Beispiel: der von Snes9x voreingestellte Aktualisierungszeitraum ist 30 Sekunden. Verringere den Wert auf z. B. 1 Sekunde,
     aber nicht auf 0 (was zur Deaktivierung des Automatismus führt).

***4.1)*** Stelle sicher so wenig wie möglich zwischen zwei Speicherungen zu ändern um unnötiges Rauschen zu vermeiden. Oft sind vermeintliche Zuordnungen nur Zufall.
***4.2)*** Sobald du eine Änderung im Spiel einer Veränderung im SRAM eindeutig zuordnen kannst, hast du eine Bedeutung für dieses spezifische Offset gefunden. Anschließend drücke (e) um das Vergleichsergebnis als Text-Datei deines Export-Verzeichnisses zu exportieren.
***4.3)*** Benenne die Datei entsprechend deines Fundes um. 
***4.4)*** Überprüfe ob die festgestellte Änderung auch in anderen Spiel-Versionen auftritt. Z.B. ungepatchte oder gepatchte Versionen.
***4.5) Stelle sicher, dass es reproduzierbar ist, anschlieénd melde den Fund über die <a href="community">Community</a>.

***5)***   Um einen Vergleich ohne vorherige SRAM Änderungen zu ermöglichen, drücke erneut (c) um die aktuelle SRAM-Datei als Vergleichs-Datei zu speichern. Anschließend beginne wieder bei Schritt 3.1.

***6.1)*** (optional) Wenn du mehr als einen Speicherslot mit Änderungen zur Vergleichsdatei hast, um 
       den Vergleich des Spiels auf den jeweiligen Speicherslot (1-4) zu beschränken drücke (ss). Wenn zwei verschiedene Speicherslot verglichen werden sollen, drücken zusätzlich (ssc), um auch den Speicherplatzz der Vergleichs-Datei festzulegen.
***6.2)*** (optional) Um die Vergleichsmodi festzulegen drücke (asbc) bzw. (nsbc). Wenn du unsicher bist, lass es bei der Voreinstellung um so wenig wie möglich Bytes zu vergleichen.

***7)***   (optional) Die aktuelle und die Vergleichs-Datei können einzeln gesichert (b) bzw. (bc) oder wiederhergestellt (r) bzw. (rc) werden.

***8)***   (optional) SRAM Offset-Werte für bestimmte Speicherslot können durch durch Drücken von (ov) angezeigt oder durch (mov) manipuliert werden. Du kannst entscheiden, ob du die aktuelle SRAM-Datei aktualisieren (Sicherung empfohlen) oder eine neue Datei erstellen möchtest.
