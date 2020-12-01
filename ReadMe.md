# SoE-SramComparer
Allows to compare (unknown) buffers and manipulate offset values in Secret of Evermore's SRAM file.

## Features
* Print (or change and save) offset values
* Comparison of Unknowns only (*default*) or whole game buffer (**optional**)
* Comparison of whole sram outside games (**optional**)
* Display of differences in decimal, hex and binary format
* Comparison of all 4 games (*default*) or one specific current game with same or different comparison game (**optional**)
* Export comparison result as text file
* Backup / restore functionality of current and comparison sram file
* Transfer of SRAM data to similar game 
* Display of changed or all checksums (**optional**)
* All settings can be set by cmd line arguments

## What's still unknown?
A little less than 50% of SoE's SRAM file game slot format is still considered as 'unknown', meaning that we don't really know what these parts of SRAM actually do.
Most of unknown parts are reserved for pick-up information of various ingredient sniff spots, opened chests, gourds, pots and which persons were spoken to twice. Despite these there are also game relevant progress flags which we want to know to write more capable SRAM editors.
A non exhaustive list can be found here:
[Unknown Offsets](https://raw.githubusercontent.com/CleanCodeX/SramComparer.SoE/master/UnknownOffsets.txt)

## What's to do?
You could join us to help finding meanings for various SRAM offsets.
This can be either done via [Web Comparison](http://compare.xeth.de) or downloading the Console app.
Once you found a meanings of some offsets you could try to enable or disable these flags in other SRAM (*.srm) files to prove the discovered meaning. 
Reading and manipulating offset values can either be done by using online [Web Editor](http://offset.xeth.de) or by [App Installer Setup](http://xeth.de/Releases/Comparer-SoE).

## Download binaries
[Releases](http://xeth.de/Releases/SramComparer)

[Latest Setup](http://xeth.de/Releases/Comparer-SoE/setup.exe)

## Prerequisites (Runtime)
This application is based on .NET 5 runtime.

1) Head to https://dotnet.microsoft.com
2) Click on Download button.
3) Users see "Run apps - Runtime" column, coders see "Build apps - SDK" column.

## How to use
**Steps**:

***1.1*** Most emulators have the option to save the game's S-RAM automatically after a change occurs.
     Make sure this is enabled if existing. Otherwise you have manually ensure that the emulator updates
     the srm file.  
***1.2*** Start the tool by passing the game's srm filepath as first command parameter. The file can also be
     dragged onto the tool.  
***1.3*** Have a look into UnknownOffsets.txt to see examples of what parts of SRAM structure are still
     considered "unknown".  

***2.***   Switch to this tool and press (ow) to create/overwrite a comparison copy of your current SRAM file allow
     comparison after your next change of the game's srm file.

***3.1*** Cause in-game a SRAM change by doing a specific action. (e.g. let a game progress event happen or
    open a chest) This change needs to be saved in the inn to make the game update the srm file.  
***3.2*** Press (c) to compare the current srm file with comparison file.
     (If the SRAM file did not change at all, you probably did not save in-game or the emulator didn't
     (yet!) update the SRAM (srm) file automatically. Check the modification date of the game's srm file.
     For Example: Snes9x's default srm update period is 30 seconds. Decrease it to a lower value, like 1,
     but not to 0.

***4.1*** Once you got the offset of a single changed byte (or bit) that has changed after your intended action you might have found a meaning for that 
     specific bit/byte. Then press (e) to export the comparison result to a textfile in your export
     directory.  
***4.2*** Rename the file expressing your current discovery.  
     If it is reproducable, consider to report the exported file to the tool's creator
     and/or contribute the change to SRAM structure to the tool's github repository.

***5.***   To start a "fresh" comparison without previous SRAM changes, press again (ow) to save your current SRAM file
     as comparison file and start again at step 3.1.

***6.1*** (optional, advanced) If you have more than one slot with changes to comparison file, press (sg) to
     set the game's save slot (1-4) to avoid comparing other game slots. If twi different games should be
     compared with each other, additionally press (scg) to set the the slot of comparison srm file, too.  
***6.2*** (optional, advanced) Press (fwg | fng) to set comparison modes.
     If you are unsure, leave it as it is to compare as less as possible bytes.

***7.***   (optional) Current and comparison srm file can be backed-up press (b|bc) or restored (r|rc) individually.

## Screenshots
![Commands](https://raw.githubusercontent.com/CleanCodeX/SramComparer.SoE/master/Meta/Cmd.png "Commands")

![Few Flags Change](https://raw.githubusercontent.com/CleanCodeX/SramComparer.SoE/master/Meta/FewFlagsChange.png "Few Flags Change")

![Many Values Change](https://raw.githubusercontent.com/CleanCodeX/SramComparer.SoE/master/Meta/ManyValuesChange.png "Many Values Change")

![No SRAM Change](https://raw.githubusercontent.com/CleanCodeX/SramComparer.SoE/master/Meta/NoChange.png "No SRAM Change")

![Whole Game Comparison](https://raw.githubusercontent.com/CleanCodeX/SramComparer.SoE/master/Meta/WholeGameComparison.png "Whole Game Comparison")

![Whole Game Comparison #2](https://raw.githubusercontent.com/CleanCodeX/SramComparer.SoE/master/Meta/WholeGameComparison2.png "Whole Game Comparison #2")

![Optional Game Info](https://raw.githubusercontent.com/CleanCodeX/SramComparer.SoE/master/Meta/OptionalGameInfo.png "Optional Game Info")
