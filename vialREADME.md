# Vial/Sterilization Process                                                                             
                                                                                
                                                                                
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
