# User Interface (UI) Main Menu README
#### Sterile Compounding Training Game
#### April 2017
#### Author: Nick Taglianetti

#### Note: this UI_README refers specifically to the Unity `MainMenu` project stored in the `UI` branch of the `ARVR-CS-Capstone-Project` repository found [HERE](https://github.com/RMP40/ARVR-Sterile-Compounding/tree/UI/MainMenu).

## References
* The Main Menu UI was created in Unity Version 5.5.1f1.
* Design decisions were based off of recommendations by CS capstone stakeholders, Pitt School of Pharmacy faculty members, and [Usability.gov](https://www.usability.gov/). 
* All features implemented in the Main Menu were adapted from three (3) Unity Live Sessions on UI in the following order:
  1. [Creating A Main Menu](https://unity3d.com/learn/tutorials/topics/user-interface-ui/creating-main-menu?playlist=17111)
  2. [Polishing Your Game Menu](https://unity3d.com/learn/tutorials/topics/user-interface-ui/polishing-your-game-menu?playlist=17111)
  3. [Game jam menu template](https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/game-jam-template?playlist=17111)

## Assets
* Most panels, buttons, materials, fonts, scripts, etc. used in the Main Menu UI were taken from the free [Unity Samples: UI](https://www.assetstore.unity3d.com/en/?&_ga=1.217841878.1725716731.1485819273#!/content/25468) asset package. 
* Audio-related assets such as the `MasterMixer` Audio Mixer as well as `SetAudioLevels.cs` were adapted from assets in the free [Game Jam Menu Template](https://www.assetstore.unity3d.com/en/?&_ga=1.256517323.1725716731.1485819273#!/content/40465) asset package.

## Unity Project Folder Hierarchy Description
### 'Animation':
* Contains opening and closing animations for the different panels of the MainMenu scene which are triggered in the selection of certain buttons. `HelpPanelOpen` and `HelpPanelClose` were the first animations created in the UI animator, and `AudioPanelOverride` as well as `ProgressPanelOverride` were created to override the HelpPanel animations with their unique opening and closing animations so as to not repeat work (for simplicity). The panel animation implementations are further explained in [Polishing Your Game Menu](https://unity3d.com/learn/tutorials/topics/user-interface-ui/polishing-your-game-menu?playlist=17111).
### 'Audio':
* Contains (in 'Audio Mixers') the `MasterMixer` Audio Mixer with three groups:
  1. `Master` for master volume control, connected to the `masterVol` exposed parameter in `SetAudioLevels.cs`. This controls the levels of the following two children.
  2. `InGameSoundFx` for in-game sound effects, such as audio feedback (eg. beeps/dings for correct/incorrect procedure steps - yet to be implemented), connected to the `sfxVol` exposed parameter in `SetAudioLevels.cs`.
  3. `MenuSounds` for menu sound effects, such as beeps when buttons are highlighted/selected, connected to the `menuVol` exposed parameter in `SetAudioLevels.cs`.
* Also contains 'Sounds' folder of `.WAV` files taken from the Windows 10 operating system, two of which (`Speech Misrecognition` and `Windows Balloon`) are implemented in the `MenuSounds` object.
### 'Scenes':
* Contains the single 'MainMenu' Unity UI scene.
### 'Scripts':
* Contains all scripts implemented in the current version of the Main Menu UI:
  1. `LoadSceneOnClick.cs` - used in `StartButton` of MainMenu to load start scene.
  2. `QuitOnClick.cs` - used in `QuitButton` of MainMenu to quit the game.
  3. `SelectOnInput.cs` - used in all panels for keyboard navigation.
  4. `SetAudioLevels.cs` - used in `MasterVolumeSlider`, `SFXVolumeSlider`, and `MenuVolumeSlider` in the `AudioPanel` for slider adjustment of the `MasterMixer` Audio Mixer.
### 'Unity UI Sample':
* Includes useful assets from [Unity Samples: UI](https://www.assetstore.unity3d.com/en/?&_ga=1.217841878.1725716731.1485819273#!/content/25468) and [Game Jam Menu Template](https://www.assetstore.unity3d.com/en/?&_ga=1.256517323.1725716731.1485819273#!/content/40465) asset packages such as fonts, sprites, prefabs (including those used as panels, buttons, and sliders in this implementation), example scenes, etc. They are included in this project to help learn the Unity UI tools as well as make improvements to the game Main Menu UI.

## Known Bugs in This Version
* When transitioning between buttons on different panels (eg. 'Back' buttons) with arrow key keyboard strokes, the user can get stuck navigating buttons on a previously visited panel that is not currently displaying. Possible solutions to this bug are further explained [here](https://youtu.be/pgtZLc-gTEk?t=59m30s "Polishing Your Game Menu - minute 59:30").
  * _Troubleshooting Note:_ The user could also navigate between visible and invisible menu panels by toggling between the left, right, and down arrow keys on the keyboard to find on which panel a button is currently selected. Using a mouse also solves the problem.
* The sliders in the 'Audio' panel are not currently connected to the `MasterMixer` Audio Mixer in a variable manner. Any adjustment to the sliders mutes the related audio. Possible solutions to this bug may be found [here](https://youtu.be/j9CqczkeYJY?t=45m37s "Game Jam Menu Template - minute 45:37").

## Features Yet To Be Implemented
* Progress Report Panel: see text displayed in `ProgressReportPanel` of the `MainMenu` scene. This panel could potentially display the quantitative/qualitative progress of a User across all attempts at the game. This panel could be accessed from the Main Menu at all times and potentially also at the end of a game session. A User Sign-in may be implemented in order to keep progress private and succinct.
* Help Panel: see text displayed in `HelpPanel` of the `MainMenu` scene. After the rules of the game are implemented, a video of a baseline "good" sterile compounding performance could be displayed here. The baseline could be performed by a practicing pharmacist in order to establish proper practices for student Users to emulate in their game sessions.
* In-Game UI: the actual assessment according to established rules (in the code-base) during gameplay. This would entail feedback including (but not necessarily limited to) different sounds (ie. beeps) for proper/improper sterile compounding practice steps during gameplay.
