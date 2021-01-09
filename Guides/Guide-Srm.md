## Guide for Comparing SRM files

This is a step-by-step guide of SRAM-Comparer console app and comparing SRM files.

<a href=guides>Overview</a>

## ***1)*** Preparation

### Useful links
Check out <a href="unknowns">Unknowns</a> to see examples of which parts of the SRAM structure are still considered to be unknown. Take a look at <a href="imagery">pictures</a> to see how the comparison results are to be interpreted.

### Configure emulator
Most emulators have the option to save a game's SRAM as soon as a change occurs.
Make sure this setting is enabled. Otherwise you have to make sure by yourself that the emulator updates the * .srm file.

### Start console app
Start the application by passing the path to the SRAM file (* .srm) of the game as the first command line parameter. The file can also be dragged and dropped onto the application.

## ***2)*** Create comparison SRAM file
Then press (overwrite) to create a comparison copy of your current SRAM file. This allows to compare with after the current SRAM file changed.

## ***3)*** Trigger a change in the game and compare SRAM

### Trigger SRAM change
Change the SRAM in the game by e.g. triggering a game event or opening a chest, so that the SRAM file is about to be updated. 
Then save your game in at the inn.

### Compare SRAM
Press (Compare) to compare the current SRAM file with comparison file. 
If the SRAM file has not changed at all, then the emulator *probably* has not (yet!) Updated the file automatically. Check the modification date of the * .srm file.

Example: the update period preset by Snes9x is 30 seconds. Decrease the value to 1 second, but not to 0! (which leads to the deactivation of the automatism)

## ***4)*** Interpret the comparison result and document the find

### Reduce annoying noise
Try to change as little as possible between two saves to avoid unnecessary bit noise.

### Optimal comparison conditions
* Look out for changing bits or bytes. The comparison result is colored differently if either only 1 bit or 1 whole byte has changed. If necessary, take a look <a href="imagery">here</a> again.
* Often a number of values are changed by an action in the game. Try to isolate the changes by repeating with different savegames, which keep changing over and over again.

### Ensure reproducibility
Value changes often mean something different than assumed. Ensure reproducibility by trying the following.

Check whether the found change…:
* also occurs in other game versions. E.g. unpatched or patched versions
* is still reproducible even after reloading your saved game

### Export comparison result
As soon as you can reproducibly assign a single change in the SRAM to a change in the game, press (Export) to export the comparison result as a text file in your export directory. Rename the file according to your find or your guess.

### Documentation
Document your find or your assumption via the <a href="community">Community</a> to avoid others do the same and can help you with the interpretation of your comparison results.

## ***5)*** New comparison without previous changes
To enable a comparison without previous SRAM changes, press (overwrite) to save the current SRAM file as a comparison file. Then start again at step 3.1.

## ***6)*** Comparison options

### (optional) Compare single or different save slots
If you have more than one slot with changes to comparison file, press (set_slot) to
     set the game's save slot (1-4) to avoid comparing other save slots. If two different save slots should be 
     compared with each other, additionally press (SetSlot_Comp) to set the the slot of comparison file, too.

### (optional) Compare all or unknown only areas of save slot
To compare all bytes (including the known areas) of a save slot byte by byte, press (sbc). If you are unsure, leave the default setting to compare as few bytes as possible.

### (optional) Compare non-save slot areas bytes
To compare the bytes behind all save slots press (nsbc). Currently it appears that this area is empty.

## ***7)*** (optional) Bavkup or restore SRAM files
Current and comparison srm file can be backed-up (Backup) or (Backup_Comp) or restored (Restore) or (Restore_Comp) individually.

## ***8)*** (optional) Show or edit offset values
SRAM offset values for specific save slots can be displayed by pressing (Offset) or manipulated by (EditOffset). You can decide whether to update your current SRAM file (backup recommended) or creating a new file.
