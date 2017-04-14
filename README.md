# AR/VR Sterile Compounding
Pitt Capstone Group Project: Sterile Compounding Training Game

### System Requirements                                                                             
Windows

### Download Unity                                                                             
https://unity3d.com/get-unity/download
                                                                            
### Download Leap Motion Orion (unless there's a new version available):                          
https://developer.leapmotion.com/orion/#105

# **All Subsystems:**


# _Environment_                                                                           
Everything used for this subsystem can be found in this folder and/or the master project. 
                                                                                
                                                                                
## What is already done                                                            
* First and foremost, an entire pharmacy scene has been modeled and set up 
for use. The scene (including all models) was created in 3DS Max, and the source
file can be found within this folder. NOTE that re-importing this set of files into Unity may
result in mapping issues (normal, specular, and albedo maps were all out of place 
when I first imported). Please, if you need to access the pharmacy within Unity,
I recommend that, rather than re-importing the source files, create a prefab 
from the already-existing project's models. This will save you hours of time reapplying
textures to various materials throughout the pharmacy. As a second note, if you do
end up reapplying textures to the pharmacy, note that most materials should use 
the "Standard (Specular setup)" shader. Do NOT use legacy shaders! 
                                                                                
* Within the pharmacy scene, the main play area has been set up including the
hands, vials, and syringes. All subsystems have been integrated in their finalized
forms (this by no means should indicate that the subsystems are finished, only that
they are complete as far as we were able to get them - and those complete versions
are reflected in the master project). The main play area is located in a hood near
the center of the pharmacy. All important objects within the play area have been set up 
with proper colliders and rigidbodies, and are reasdy for use. 

* A liquid shader was used to portray liquid in the vial/syringe. The liquid shader has its 
own documentation and API information, which can be found in the same directory. Please refer 
to this documentation for scripting information and other usage concerns. This shader was cobbled 
together using 3rd party assets as well as my own, and it is by no means perfect. It lacks, 
for example, the ability to truly portray an air-tight syringe without filling it completely.

   While using this shader within the syringe, note that you can give the illusion of a full
syringe by simply filling the syringe completely, and then using the cylindrical plunger to cover 
the liquid. This does not, however, allow for the important step of filling the syringe with an
appropriate amount of air before drawing liquid. This is a major flaw in the shader, and is something
that someone in the future may choose to fix, given they have experience with shaders. 

   The shader offers options to create 'noise' within the liquid, giving the illusion of bubbles
This will allow for the air-bubble removal step, as you may decrease the noise slider with each tap 
on the syringe.

* I also used a sophisticated lighting system to aid the game's visual fidelity. To access
these files, navigate to the "/Assets/Plugins" folder. This system, too, has its own documentation.
This system is very complex, and the changes are quite subtle at times. Please remember: if you are 
editing the lighting scripts in the Unity editor, the lighting changes WILL NOT APPEAR in the scene 
view. Lighting changes using these scripts will ONLY appear in the GAME view, so check for them there
while you work. A useful method for assigning values to these scripts is to do so while in play mode, 
as this allows you to see your changes in action. Bear in mind, however, that all changes are lost 
upon exiting play mode. Simply right-click the component you are changing, copy it, end play mode,
right click the now-reverted component once more, and select the "Paste component values" option. 
All values will now be populated with the values you selected while in play mode. 

   The reason that the lighting scripts do not update Unity's scene view is simply that they are 
effects which are rendered by the camera, and so the scene view does not display them. It cannot
be avoided. 
                                                                                
* A simple camera animation has been created to accompany the UI, activated when the "Start"
button is pushed. If you need to update the UI and need to access this script, simply attach the
"OnStartButton" script to the start button, and access the function "TaskOnClick()" in the event
system. This will activate the animation when the start button is clicked. Also, be sure to assign
the main menu panel to the UI Camera game object's "UICamera" script!                                                                                                                                                               
           
           
## What needs to be done
* Airflow is as of yet unimplemented. This major facet of intended gameplay should be first on 
the environment subsystem's to-do list. As of now, the airflow is represented by a very simple 
particle system. No scripts have been included to aid in the handling of airflow.

* Liquid shader requires improvements to allow for the representation of an airtight syringe.
As of now, as mentioned above, the only way to properly show a syringe filling up, regardless 
of the angle at which it is held (which is how a syringe would work in reality, as they are 
air-tight), is by filling the entire syringe and using a cylindrical plunger to fool viewers 
into believing that the syringe is filling up. This fails to take advantage of many of the shader's
capabilities and is completely incapable of portraying the process of filling the syringe with air 
before filling it with liquid. As such, the shader either needs to be updated to allow air-tight
containers, or it needs to be replaced with one that already does so.

* Other camera animations are recommended for the UI, pending the completion of other UI elements. 

* Other models may need to be created and added to the scene depending on necessity. For example,
our user stories included a section regarding the use of an alcohol swab to clean the vial's rubber
stopper. Alcohol swabs are not included in the model. 

* Work with other subsystems must be done to implement remaining functionalities, such as angle and 
airflow detection, scoring, and so on. 
                                                                                                                                           

# _Liquid Shader_                                                                        
To get a better idea of how things work, check out the shader in action, applied to many of the
bottles and vials located in the main play area of the master project! 
                                                                                
                                                                                
## Adding the shader to a scene

* To create a liquid volume in the scene, attach a liquidvolume component to any object. Sphere,
cylinder, and box objects are supported directly out of the box. You can also assign the liquid to
a generic shape, which will conform the liquid to a provided mesh.
                                                                                
## Inspector Settings 

### General Settings 

* Topology: indicates the underlying geometry. Must be either a sphere, cylinder, cube, or irregular. 
Choose irrefgular for any non-primitive types.

* Detail: Specifies the detail level of the flask. Simple (do not use 3D textures), default, default without a 
flask (means fewer render passes), texture bump (adds additional texturing and bump mapping options),
and reflections.

* Depth Aware: allows objects inside the container 

### Liquid Settings 

* Level: fill level
* Color1 and 2, Scale1 and 2: Controls appearances of two liquid components, which are blended together to 
produce a final color.
* Emission, Emission brightness: Controls emission color/brightness of liquid
* Murkiness: Amount of noise
* Alpha: Global transparency of liquid, smoke, and foam
* Dither shadow: Applies dither to liquid shadow to simulate a partially transparent shadow
* Turbulence 1: Small turbulence 
* Turbulence 2: Larger turbulence. Can be mixed with turbulence 1 to create realistic movement
* Frequency: Increase to produce shorter turbulence waves when using models with scale greater than one.
Default value of one is usually fine for this.
*Sparkling intensity/amount: Adds sparkles to liquid.
* Deep obscurance: Gradient to darker colors at bottom of liquid 

### Foam Settings 

* Color and scale: basic color/resolution of effect
* Thickness: relative height of foam with respect to flask height
* Density: increase this to create a more opaque effect
* Weight: Controls the smoothness of the foam at the bottom, near the liquid
* Turbulence: Multiplier to turbulence factors of the liquid
* Visible from bottom: Enable this to allow the foam to be visible from the bottom up (if
the liquid's alpha is low)

### Smoke settings

* Color and scale
* Speed: Speed multiplier for the smoke
* Base Obscurance: darkens smoke at the bottom

### Flask Settings

* Tint: color applied to flask. For transparent containers, reduce the alpha component of this to zero. 
* Thickness: Flasks have walls. This controls their width
* Glossiness:
* Refraction blur: Blurs background to simulate light refraction

### Physics Settings 

* React to forces: Allows the turbulence of the liquid to react with external forces and movements
* Mass: Defines the mass of the liquid. A higher value increases the weight of the liquid, meaning 
it will move less
* Angular damp: Defines the internal friction of the liquid. Higher value makes liquid return to calm 
state more quickly
* Ignore gravity: Enable this to force the liquid to rotate with the flask. Enabling "React to forces,"
obviously, disables this. 

## API Information 
To use the shader, just use its class. To change any property, get a refrence to the liquidvolume component 
using the following code:

''' 
using LiquidVolumeFX;
.....
LiquidVolume liquid = <gameObject>.GetComponent<LiquidVolume>();
'''

To change current fill  level, as an example, simple do the following:

''' 
liquid.level = .65;
'''
                                                                                                                                          

# _User Interface (UI)_

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



# _Hands_

* The functional hand set can be found under ARVR-CS-Capstone-Project/SterileCompoundSim/Assets/Prefabs/handsset
* Download the Leap control panel from here: https://www.leapmotion.com/setup/desktop/windows
* Leap Motion developer Module & Documentation: https://developer.leapmotion.com/unity#100
## What is already done
* The hand prefab that works with Leap Motion device, once the prefab is placed in the game world the hand detection should be working
* When the hands are reaching a game object with the script and do a grab pose, the hands will grab this game object. 
The script can be found under the directory ARVR-CS-Capstone-Project/SterileCompoundSim/Assets/LeapMotionModules/InteractionEngine/Scripts/InteractionBehaviour.cs.
 For the script field "Manager", there is a game object named "InteractionManager" under the hierachy of handsset, simply drag it into the field.
 Also, the game object needs a rigidbody and collider, while the "Interpolate" of the rigidbody should be set to "Interpolate" and "Collision Detection" should be
  set to "Continuous"
## What needs to be done
* The grabbing function does not work very well in terms of stability, therefore it needs improvement
* The hands reach has space limitations, if the game object is too far then the player cannot grab it. Some ways to work around it: have the enviornment
subsystem to strictly limit the space that the objects can move; make the hands have "telekinesis" ablity, using raycast combined with gestures to grab things 
far away. 
* Interaction with vial and synringe subsystems(perhaps UI)



# _Syringe_     

Everything used for the syringes can be found in the 
SterileCompoundSim/Models/Syringes folder.
                                                                                
                                                                                
## What is already done                                                            
* There are three syringe models (a 3ml, 30 ml, and 60 ml) included with their
Blender file, which is imported into the Unity game. If you want to change
anything about the way the Syringe models look, do this through the 
syringe.blend file. Any changes made there should automatically show up in Unity.
                                                                                
* Colliders for determining if the cap is on the needle and if the needle is on
the barrel. I put one collider near the top of the barrel and one near the
bottom of the needle rather than use the colliders already belonging to the
barrel and needle. I did this because I did not want it to be possible for
the user to attach the needle by colliding it with the wrong side of the barrel,
which is not particularly close to where the needle should go.
                                                                                
* To go along with the colliders is some simple code in Syringe.cs file for
to change the values that track whether the cap is on (`cap_on`) and whether
the needle and barrel are together (`together`). These methods are called in
the three collider files when the appropriate collision is found. Such as,
if the needle collides with the barrel, `together` becomes 1. If the needle
exits the collision, `together` becomes 0. Additionally, when the needle
enters a collision with the barrel, it is popped into the proper place in
the `putTogether()` function found in Syringe.cs.
                                                                               
Note: Right now, since one collision file is shared by one entire syringe,
the needle's collision with the cap might trigger the needle to being on
the barrel, since I did not know of a way to check the exact two items
colliding. One way to fix this is by putting one collision file on the needle
to see if the cap is on and one on the barrel to see if the needle is on,
then connecting that to a file for the entire syringe.
                                                                                                                                                                                                                                                
* Code for moving the plunger within the bounds of the barrel. This is in the
`keepInBounds()` function in the Syringe.cs file, which implements a Syringe
class, used by the Collision_3.cs, Collision_30.cs, and Collision_60.cs files.
You can test this with the testMove() function followed by the `keepInBounds()`
function in an `Update()` loop, as can be seen in Collision_3.cs.
                                                                                
```cs
void Update () {
  syr_3.testMove ();
  syr_3.keepInBounds ();
}
```
                                                                                
NOTE: This `keepinBounds()` function could be changed to a fixed joint.
https://docs.unity3d.com/ScriptReference/FixedJoint.html
                                                                                
NOTE: If you change the size of the syringes, this code might break, seeing
as that would change the distance the plunger can go before reaching the end
of the barrel.

* A liquid shader and mesh is added to each of the three syringes. The plunger
covers the liquid. The liquid is revealed when the plunger is drawn back. To
make the liquid appear clear, you would need to change the `Alpha` to 0,
so the liquid is really all or nothing. You can mimic the effect of a partially
air filled syringe to an extent by increasing the number of bubbles, 
`Sparkling Intensity` and `Sparkling Amount`, though. Since the actual mesh does 
not resize as the plunger is drawn back, and usually the syringe is filled with 
the needle pointing up, the `Level` feature would show no liquid until the 
syringe is entirely filled.
                                                                                
                                                                                
## What needs to be done
* The way the colliders are done will likely need to be changed. Right now,
each syringe only interacts with one needle and one cap, but ideally you would
have one needle and one cap for all three syringes. The needle should start
unattached to a syringe at the beginning of the game, so the player would not
know which needle goes with which syringe.                                
                                                                                
* I suggest coding the hand interaction with the syringe. The algorithm that
Leap Motion uses is not very exact, so the syringe kind of sits outside of the
group of the hand right now. You could check for when the hand is within a
certain range of the syringe, say within 2 Euclidean/Manhatten distance, and 
if it is and is also gripping, then attach the syringe to that gripped hand. 
It could look something like this:         
                                                                                
```cs                                                        
// Returns hand if a hand is near the barrel and grabbing, null otherwise
public Hand isHandNear() {
  Frame frame = controller.Frame (); // controller is a Controller object
  if(frame.Hands.Count > 0){
      List<Hand> hands = frame.Hands;
      int i;

      // Check to see if any of the hands is nearby
      for (i = 0; i < hands.Count; i++) {
        Hand currHand = hands [i];
        Vector3 curr_vect = currHand.transform.localPosition;

        // Find Euclidean distance
        float handDistance = abs(barrel_vect.x - curr_vect.x) + abs(barrel_vect.y - curr_vect.y) + abs(barrel_vect.z - curr_vect.z);
        if (handDistance <= 2 && currHand.grabStength >= 0.5) {
          return currHand;
        }
      }
  }

  return null;
}
```
 
```cs
void Update () {
  Hand handNear = isHandNear();
  if (handNear != null) {
    attachSyringe(handNear);
  }
}
```
                                                                                
Check:                                                                             
https://developer.leapmotion.com/documentation/csharp/devguide/Leap_Hand.html 

You could do this and other related hand things in a hand API if you think it
should be more organized that way.
                                                                                
* You will also need to write code to check for hand gestures that mean to draw
back the plunger. You can find the hands' direction and velocity and using
the Leap API and use this to look for when one hand is pulling in the direction
of the plunger.                                                                             
https://developer.leapmotion.com/documentation/csharp/devguide/Leap_Hand.html
                                                                                
Maybe something like (pesudocode) this:
```c#
if (sum(handNearPlunger.PalmVelocity) > 1 && handNearPlunger.Direction is away from barrel) {
  drawBackPlunger();
}
```
                                                                              
* You then need to draw back the plunger to some position. Ideally, this
would be done to the specific distance of where the hand is drawn back to, but
this may be challenging. A more blunt way would be to enact an action made in
Blender. This is a good video teaching how do create a Blender action:                                
https://www.youtube.com/watch?v=yKP2Qy-yKhE
                                                                                
* You'll also need to add a certain amount of bubbles depending on the angle
that the syringe is being held at when the plunger is being held back. Check
out Brendan's liquid shader documentation to find out more about this.
                                                                                
* Finally, you'll need to put some sort of collider or other way to know if
the needle was properly disposed of in the biohazard box.
                                                                                
                                                                                
Disclaimer: I am no Unity expert by any means, so if you know Unity, you likely
know better ways for much of the above.
                                                                              
                                                                              
Good luck!



# _Vial/Sterilization Process_                                                                             
                                                                                
                                                                                
## What is already done                                                            
* There are two syringe models (10mL and 50mL) included in the Vial branch. The 10mL has been imported into the game in Unity. 
Blender was used to create the models and are the files with the blend extension. The models were created using several 
pieces which can be edited to make different sizes quickly. The labels for the vials were created with Paint.net and imported 
into Blender. 
                                                                                
* Colliders for detecting if the needle has entered the vial, is currently in the vial, and is exiting the vial were placed on
the rubber stopper of the vial in Unity. 
                                                                                
* Liquid shaders have been added to the vial to simulate liquid movement. Originally, the liquid simulation was done in Blender 
as an animation, but the performance of the game would have become compromised if it was used. The liquid shaders are a performance 
friendly way to simulate liquid.

* Retroreflective stickers were placed on the 10mL vial so that they could be detected from different positions. Retroreflective stickers 
were used because the lighting of the room will not need to change to use them. I placed them down the sides of the vial. Leap Motion does 
not have the ability to recognize objects, but does have the ability to detect light. Object recognition is important for when the software 
is being used for testing the pharmacy students for competency where they will be using real equipment. I worked on code for object recognition 
which can be found in the Object Recognition branch under SterileCompoundSim/Assets/Models/Vials/ObjectDetection.cs.The code is for general object 
recognition and not specific to the vial or objects of a certain size/shape. The code can work with the syringe or other objects added in the future 
as long as there are retroreflective stickers placed on them. I made sure to comment the code well to avoid any confusion.


## What needs done:
* The code for object recognition needs more manual testing. I ran an automated test suite on the code which passed. I wasn’t able to spend much time on manual 
testing because we were short a Leap Motion. What I recommend for manual testing, to make sure that it's picking up enough of the object to accurately detect the position, is 
to have the stickers it’s detecting draw spheres onto the screen. Included is the link to a YouTube video that demonstrates how manual testing would work. I manually 
tested the code using print statements for positions of the stickers the Leap Motion camera was picking up. 
https://www.youtube.com/watch?v=kAd3b20GCLc

* The liquid in the vial the student will be holding during testing will need to be tracked. Based off the research I did, I recommend coloring the liquid inside 
the vial and tracking it with Leap Motion. Developers have had success tracking color with Leap Motion. The pharmacy department has colored powder in vials that you 
can use to change the color of the liquid. 
