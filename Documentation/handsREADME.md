# Hands
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
