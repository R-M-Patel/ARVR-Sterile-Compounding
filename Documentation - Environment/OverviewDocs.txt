# Environment                                                                           
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
                                                                                                                                           
