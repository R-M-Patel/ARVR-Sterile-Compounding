# Syringe                                                                             
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
function in an 'Update()' loop, as can be seen in Collision_3.cs.
                                                                                
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
