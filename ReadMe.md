# SoE-SramComparer
Allows to compare (unknown) buffers and manipulate offset values in Secret of Evermore's SRAM file.

## Features
* Display (or modify and save) offset values
* Comparison of Unknowns area only (*default*) or complete save slot area (**optional**)
* Comparison of the non-save slot area (**optional**)
* Display of differences in decimal, hex and binary format
* Comparison of all save slots (*default*) or one specific slot with same or different comparison slot (**optional**)
* Export comparison result as text file
* Backup / restore functionality of current and comparison sram file
* Transfer of SRAM data to another game name 
* Display checksums for changed or all slots (**optional**)
* All settings can be set by cmd line arguments

## What's still unknown?
Aprox. 54% of SoE's SRAM game slot format is still considered as 'unknown', meaning that we don't really know what these parts of SRAM actually represent.
Most of unknown parts are reserved for pick-up information from various ingredient sniff spots, opened chests, gourds, pots and which people were spoken twice. Despite these things there are also game relevant progress flags which need to be known to write a more capable SRAM editor.
A non exhaustive list can be found here:
[Unknown Offsets](https://raw.githubusercontent.com/CleanCodeX/SramComparer.SoE/master/Markdown/Unknowns.md)

## Can I help?
Actually you can! Compare SRAM files, it's not difficult at all.

This would be really awesome. Your help would save much time in not doing something twice what someone else already did.

Join the SoE 'exploration' community to find meanings for various SRAM offsets. This can be done either by using

* [Web Comparison](http://compare.xeth.de)
* Release

Once you found a meaning for an offset value, try to enable or disable this flag in other SRAM (*.srm) files to prove your find. Reading and modifying offset values can be done using the following: 

* [Web Offset Editor](http://offset.xeth.de)
* Release

## Prerequisites (Runtime)
This application uses the latest .NET 5 runtime.

1) Head to [.Net](https://dotnet.microsoft.com)
2) Click on Download button.
3) Users see "Run apps - Runtime" column, coders see "Build apps - SDK" column.

## Download Binaries
[Releases](http://xeth.de/Releases/SramComparer)

## How to use
**Steps**:

***1.1)*** Have a look into [Unknowns](http://unknowns.xeth.de) to see examples of what parts of SRAM structure are still 
     considered as unknown.
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
***4.5) If it is reproducible, report the find via [community](http://community.xeth.de). If necessary, create a pull request for a change to the documentation and the SRAM structure in the GitHub repository..

***5)***   To start a "fresh" comparison without previous SRAM changes, press again (ow) to save your current SRAM file 
     as comparison file. Then start again at step 3.1.

***6.1)*** (optional) If you have more than one slot with changes to comparison file, press (ss) to
     set the game's save slot (1-4) to avoid comparing other save slots. If two different save slots should be 
     compared with each other, additionally press (ssc) to set the the slot of comparison file, too.
***6.2)*** (optional) Press (asbc | nsbc) to set comparison modes. 
     If you are unsure, leave at default to compare as less as possible bytes.

***7)***   (optional) Current and comparison srm file can be backed-up (b|bc) or restored (r|rc) individually.

***8)***   (optional) SRAM offset values for specific save slots can be displayed by pressing (ov) or manipulated by (mov). You can decide whether to update your current SRAM file (backup recommended) or creating a new file.

## Screenshots
[Imagery](http://imagery.xeth.de)
