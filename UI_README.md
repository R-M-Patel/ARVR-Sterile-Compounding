# User Interface (UI) Main Menu README
#### Sterile Compounding Training Game
#### April 2017
#### Author: Nick Taglianetti

#### Note: this UI_README refers specifically to the Unity `MainMenu` project stored in the `UI` branch of the `ARVR-CS-Capstone-Project` repository found [HERE](../ARVR-CS-Capstone-Project/tree/UI/MainMenu).

## References
* The Main Menu UI was created in Unity Version 5.5.1f1.
* Design decisions were based off of recommendations by CS capstone stakeholders, Pitt School of Pharmacy faculty members, and [Usability.gov](https://www.usability.gov/). 
* All features implemented in the Main Menu were adapted from three (3) Unity Live Sessions on UI in the following order:
⋅⋅1. [Creating A Main Menu](https://unity3d.com/learn/tutorials/topics/user-interface-ui/creating-main-menu?playlist=17111)
⋅⋅2. [Polishing Your Game Menu](https://unity3d.com/learn/tutorials/topics/user-interface-ui/polishing-your-game-menu?playlist=17111)
⋅⋅3. [Game jam menu template](https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/game-jam-template?playlist=17111)

## Assets
* Most panels, buttons, materials, fonts, scripts, etc. used in the Main Menu UI were taken from the free [Unity Samples: UI](https://www.assetstore.unity3d.com/en/?&_ga=1.217841878.1725716731.1485819273#!/content/25468) asset package. 
* Audio-related assets such as the `MasterMixer` Audio Mixer as well as `SetAudioLevels.cs` were adapted from assets in the free [Game Jam Menu Template](https://www.assetstore.unity3d.com/en/?&_ga=1.256517323.1725716731.1485819273#!/content/40465) asset package.

## Unity Project Folder Hierarchy Description
### 'Animation':
Contains opening and closing animations for the different panels of the MainMenu scene which are triggered in the selection of certain buttons. `HelpPanelOpen` and `HelpPanelClose` were the first animations created in the UI animator, and `AudioPanelOverride` as well as `ProgressPanelOverride` were created to override the HelpPanel animations with their unique opening and closing animations so as to not repeat work (for simplicity). The panel animation implementations are further explained in [Polishing Your Game Menu](https://unity3d.com/learn/tutorials/topics/user-interface-ui/polishing-your-game-menu?playlist=17111).
### 'Audio''""":""""''''''''''''''''''''''''''''''''''''''''''"""""""'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

## Known Bugs in This Version'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''"""""""""""""'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
* When transitioning between buttons on different panels (eg. 'Back' buttons) with arrow key keyboard strokes, the user can get stuck navigating buttons on a previously visited panel that is not currently displaying. Possible solutions to this bug are further explained [here](https://youtu.be/pgtZLc-gTEk?t=59m30s "Polishing Your Game Menu - minute 59:30").
⋅⋅* Troubleshooting Note: The user could also navigate between visible and invisible menu panels by toggling between the left, right, and down arrow keys on the keyboard to find on which panel a button is currently selected. Using a mouse also solves the problem.
* The sliders in the 'Audio' panel are not currently connected to the `MasterMixer` Audio Mixer in a variable manner. Any adjustment to the sliders mutes the related audio. Possible solutions to this bug may be found [here](https://youtu.be/j9CqczkeYJY?t=45m37s "Game Jam Menu Template - minute 45:37").
