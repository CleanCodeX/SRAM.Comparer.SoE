## Anleitung
**Schritte**:

***1.1)*** Schau in <a href="http://unknowns.xeth.de">Unknowns</a> um Beispiele zu sehen, welche Teile der SRAM-Struktur noch als unbekannt gelten. Anschlie�end schau dir einige <a href="http://imagery.xeth.de">Bilder</a> der Anwendung an.
***1.2)*** Die meisten Emulatoren haben die Einstellung das SRAM eines Spiels zu speichern sobald eine �nderung eintritt. 
     Stelle sicher, dass diese Einstellung aktiviert ist. Andernfalls musst du selbst sicherstellen, dass der Emulator die *.srm-Datei aktualisiert.
***1.3)*** Starte die Anwendung indem du den Pfad zur Srm Datei des Spiels als ersten Kommandozeilen Parameter �bergibst. Die Datei kann auch per Drag 'n' Drop auf die Anwendung gezogen werden.

***2)***   Anschlie�end dr�cke (ow) um eine Kopie deiner aktuellen SRAM Datei zu erstellen. Diese erm�glicht einen Vergleich nach einer �nderung deiner aktuellen SRAM-Datei.

***3.1)*** Verursache im Spiel eine �nderung des SRAMs. (z.B. l�se ein Spielereignis aus oder �ffne eine unge�ffnete Truhe) Damit die SRAM-Datei aktualisiert wird speichere deinen Spielstand im Spiel selbst bei einer Speicherm�glichkeit.
***3.2)*** Dr�cke (c) um die aktuelle SRAM-Datei mit der Vergleichs-Datei zu verlgeichen. 
     (Fass sich die SRAM-Datei �berhaupt nicht ge�ndert hat, hast du vermutlich nicht im Spiel gespeichert oder der Emulator hat die Datei (noch!) nicht automatisch aktualisiert. �berpr�fe das �nderungsdatum der *.srm-Datei.
     Beispiel: der von Snes9x voreingestellte Aktualisierungszeitraum ist 30 Sekunden. Verringere den Wert auf z. B. 1 Sekunde,
     aber nicht auf 0 (was zur Deaktivierung des Automatismus f�hrt).

***4.1)*** Stelle sicher so wenig wie m�glich zwischen zwei Speicherungen zu �ndern um unn�tiges Rauschen zu vermeiden. Oft sind vermeintliche Zuordnungen nur Zufall.
***4.2)*** Sobald du eine �nderung im Spiel einer Ver�nderung im SRAM eindeutig zuordnen kannst, hast du eine Bedeutung f�r dieses spezifische Offset gefunden. Anschlie�end dr�cke (e) um das Vergleichsergebnis als Text-Datei deines Export-Verzeichnisses zu exportieren.
***4.3)*** Benenne die Datei entsprechend deines Fundes um. 
***4.4)*** �berpr�fe ob die festgestellte �nderung auch in anderen Spiel-Versionen auftritt. Z.B. ungepatchte oder gepatchte Versionen.
***4.5) Falls es reproduzierbar ist berichte den Fund �ber community.xeth.de. Ggf. erstelle zudem einen Pull Request f�r eine �nderung an der Doku und der SRAM-Struktur im GitHub-Repository.

***5)***   Um einen "frischen" Vergleich ohne vorherige SRAM �nderungen zu erm�glichen, dr�cke erneut ({0}) um die aktuelle SRAM-Datei als Vergleichs-Datei zu speichern. Anschlie�end beginne wieder bei Schritt 3.1.

***6.1)*** (optional) Wenn du mehr als einen Speicherslot mit �nderungen zur Vergleichsdatei hast, um 
       den Vergleich des Spiels auf den jeweiligen Speicherslot (1-4) zu beschr�nken dr�cke (ss). Wenn zwei verschiedene Speicherslot verglichen werden sollen, dr�cken zus�tzlich (ssc), um auch den Speicherplatzz der Vergleichs-Datei festzulegen.
***6.2)*** (optional) Um die Vergleichsmodi festzulegen dr�cke (asbc) bzw. (nsbc). Wenn du unsicher bist, lass es bei der Voreinstellung um so wenig wie m�glich Bytes zu vergleichen.

***7)***   (optional) Die aktuelle und die Vergleichs-Datei k�nnen einzeln gesichert (b) bzw. (bc) oder wiederhergestellt (r) bzw. (rc) werden.

***8)***   (optional) SRAM Offset-Werte f�r bestimmte Speicherslot k�nnen durch durch Dr�cken von (ov) angezeigt oder durch (mov) manipuliert werden. Du kannst entscheiden, ob du die aktuelle SRAM-Datei aktualisieren (Sicherung empfohlen) oder eine neue Datei erstellen m�chtest.
