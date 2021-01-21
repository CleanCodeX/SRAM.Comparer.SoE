## Guide for Comparing Savestate files

This is a step-by-step guide of S-RAM-Comparer console app and comparing Snes9x savestate files.

<a href=guides>Overview</a>

## ***1)*** Preparation

### Useful links
Check out <a href="unknowns">Unknowns</a> to see examples of which parts of the S-RAM structure are still considered to be unknown. Take a look at <a href="imagery">pictures</a> to see how the comparison results are to be interpreted.

### Start console app
Start the application by passing the path to the savestate file (*.000-009 or * .state) of the game as the first command line parameter. The file can also be dragged and dropped onto the application.

## ***2)*** Create comparison save file
Then press (Overwrite) to create a comparison copy of your current save file. This allows to compare with after the current save file changed.

## ***3)*** Trigger a change in the game and compare S-RAM

### Trigger S-RAM change
Change the S-RAM by e.g. triggering a game event or opening a chest and create a savestate.

### Compare S-RAM
Press (Compare) to compare the current save file with comparison file. 

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
As soon as you can reproducibly assign a single change in the S-RAM to a change in the game, press (Export) to export the comparison result as a text file in your export directory. Rename the file according to your find or your guess.

### Documentation
Document your find or your assumption via the <a href="community">Community</a> to avoid others do the same and can help you with the interpretation of your comparison results.

## ***5)*** New comparison without previous changes
To enable a comparison without previous S-RAM changes, press (Overwrite) to save the current save file as a comparison file. Then start again at step 3.1.

## ***6)*** Comparison options

### (optional) Compare single or different save slots
If you have more than one slot with changes to comparison file, press (Set_Slot) to
     set the game's save slot (1-4) to avoid comparing other save slots. If two different save slots should be 
     compared with each other, additionally press (SetSlot_Comp) to set the the slot of comparison file, too.

### (optional) Compare all or unknown only areas of save slot
To compare all bytes (including the known areas) of a save slot byte by byte, press (SlotByteComp). If you are unsure, leave the default setting to compare as few bytes as possible.

### (optional) Compare non-save slot areas bytes
To compare the bytes behind all save slots press (NonSlotComp). Currently it appears that this area is empty.

## ***7)*** (optional) Bavkup or restore S-RAM files
Current and comparison srm file can be backed-up (Backup) or (Backup_Comp) or restored (Restore) or (Restore_Comp) individually.

## ***8)*** (optional) Show or edit offset values
S-RAM offset values for specific save slots can be displayed by pressing (Offset) or manipulated by (EditOffset). You can decide whether to update your current save file (backup recommended) or creating a new file.
