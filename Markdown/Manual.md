## How to use
**Steps**:

***1.1)*** Before you start, have a look into <a href="http://unknowns.xeth.de">Unknowns</a> to see examples of what parts of SRAM structure are still considered as unknown. Then see some <a href="http://imagery.xeth.de">Imagery</a>).
***1.2)*** Most emulators have the option to save the game's S-RAM automatically after a change occurs. 
     Make sure this is enabled if existing. Otherwise you have manually ensure that the emulator updates 
     the *.srm file.
***1.3)*** Start the application by passing the game's srm filepath as first command parameter. The file can also be 
     dragged onto the application.

***2)***   Then press (ow) to create a comparison copy of your current SRAM file. This allows to compare with after the current SRAM file changed.

***3.1)*** Cause in-game a SRAM change. (e.g. let a game progress event happen or 
    open an unopened chest) To force the SRAM file to be updated, save your game in-game at the inn.
***3.2)*** Press (c) to compare the current srm file with comparison file. 
     (If the SRAM file did not change at all, you probably did not save in-game or the emulator didn't
     (yet!) update the SRAM (srm) file automatically. Check the modification date of the game's srm file.
     For Example: Snes9x's default srm update period is 30 seconds. Decrease it to a lower value, like 1 second,
     but not to 0 (which deactivates the automatism).  

***4.1)*** Make sure to change as little as possible between two saves to avoid unnecessary noise. Alleged assignments are often just coincidence.
***4.2)*** As soon as you can clearly assign a change in the game to a change in the SRAM, you have found a meaning for this specific offset. Then press (e) to export the comparison result as a text file in your export directory.
***4.3)*** Rename the file according to your find.
***4.4)*** Check whether the change found also occurs in other game versions. E.g. unpatched or patched versions.
***4.5) If it is reproducible, report the find via community.xeth.de. If necessary, create a pull request for a change to the documentation and the SRAM structure in the GitHub repository..

***5)***   To start a "fresh" comparison without previous SRAM changes, press again (ow) to save your current SRAM file 
     as comparison file. Then start again at step 3.1.

***6.1)*** (optional, advanced) If you have more than one slot with changes to comparison file, press (ss) to
     set the game's save slot (1-4) to avoid comparing other save slots. If two different save slots should be 
     compared with each other, additionally press (ssc) to set the the slot of comparison file, too.
***6.2)*** (optional, advanced) Press (asbc | nsbc) to set comparison modes. 
     If you are unsure, leave at default to compare as less as possible bytes.

***7)***   (optional) Current and comparison srm file can be backed-up (b|bc) or restored (r|rc) individually.

***8)***   (optional) SRAM offset values for specific save slots can be displayed by pressing (ov) or manipulated by (mov). You can decide whether to update your current SRAM file (backup recommended) or creating a new file.
