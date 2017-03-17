using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class provides the functionality for one of the already created 3D models of a 3, 30, 
/// or 60 milliliter Syringe object.
/// </summary>

public class Syringe {

	// Visible objects
	private GameObject barrel;
	private GameObject plunger;
	private GameObject needle;
	private GameObject cap;
	private GameObject main;

	// Non-visible objects
	Vector3 screenPoint;
	Vector3 offset;
	public Rigidbody rb;

	// For moving the objects (distances when attached to each other)
	// 30ml dist = -3 to 0.6 localPosition.y
	// 60ml dist = -4.3 to 0
	// 3ml dist = 0 to 1.65
	private float MAX;
	private float MIN;
	private float b_p_diff;		// Difference between barrel and plunger x-value when attached

	// Needle and syringe together?
	private int together = 0;
	private int cap_on = 0;
	private Vector3 b_n_diff;	// Difference between barrel and needle when attached


	/// <summary>
	/// Creates a default 3, 30, or 60 ml syringe. If any other number is passed in, the
	/// syringe will be incomplete.
	/// </summary>
	/// <param name="num">Number of milliliters of the syringe being created (3, 30 or 60)</param>
	/// <param name="rb">Rigidbody of the 3D model syringe you are trying to create functionality for.</param>
	public Syringe (int num, Rigidbody rb) {	// num = size of syringe (3, 30, 60)
		// Default setup is the needle in the needle cap, the plunger
		// in the barrel, and the needle NOT attached to the barrel.
		this.barrel = GameObject.Find("Barrel_" + num + "ml");
		this.plunger = GameObject.Find("Plunger_" + num + "ml");
		this.needle = GameObject.Find("Needle_" + num);
		this.cap = GameObject.Find("Cap_" + num);
		this.main = GameObject.Find(num + "ml");
		this.rb = rb;

		if (num == 3) {
			MAX = 1.68f;
			MIN = 0f;
			b_p_diff = -1.0f;
			b_n_diff = new Vector3(-1.0f, 3.43f, 0.0f);
		} else if (num == 30) {
			b_p_diff = 0.0f;
			MAX = 0.55f;
			MIN = -3f;
			b_n_diff = new Vector3(0.0f, 3.43f, 0.0f);
		} else if (num == 60) {
			b_p_diff = -1.0f;
			MAX = 0f;
			MIN = -4.5f;
			b_n_diff = new Vector3(-1.0f, 3.43f, 0.0f);
		}

		Debug.Log ("made");
	}

	/// <summary>
	/// Returns the distance between the syringe barrel and plunger when fully pushed together.
	/// </summary>
	/// <returns>Float of the max distance.</returns>
	public float getMax() {
		return MAX;
	}

	/// <summary>
	/// Returns the distance between the syringe barrel and plunger when the plunger is fully extended.
	/// </summary>
	/// <returns>Float of the minimum distance.</returns>
	public float getMin() {
		return MIN;
	}

	/// <summary>
	/// Pop needle into position and mark it as attached to barrel
	/// </summary>
	public void putTogether() {
		together = 1;
		needle.transform.localEulerAngles = barrel.transform.localEulerAngles;
		needle.transform.localPosition = barrel.transform.localPosition - b_n_diff;
	}

	/// <summary>
	/// Pop cap into position and mark it as on the needle
	/// </summary>
	public void putCapOn() {
		cap_on = 1;
		cap.transform.localPosition = needle.transform.localPosition - new Vector3(0.0f, -0.3f, 0.0f);
	}

	/// <summary>
	/// Change the needle to being unattached to the barrel
	/// </summary>
	public void takeApart() {
		together = 0;
	}

	/// <summary>
	/// Change the cap to being unattached to the needle
	/// </summary>
	public void takeCapOff() {
		cap_on = 0;
	}
		
	/// <returns>Returns 1 if the needle is attached to the barrel, otherwise 0.</returns>
	public int isTogether() {
		return together;
	}
		
	/// <returns>Returns 1 if the cap is on the needle, otherwise 0.</returns>
	public int isCapOn() {
		return cap_on;
	}
		
	/// <summary>
	/// Keeps the plunger within the bounds of the barrel if it is in the update() loop.
	/// </summary>
	// I originally wrote this to avoid putting rigidbodies on the barrel and plunger, which
	// a fixed joint would have required. I worried that rigidbodies on these objects would
	// cause them to fly apart, as they did in Blender. In retrospect, I could have changed
	// the rigidbodies to ignore physics then used a fixed joint.
	public void keepInBounds() {
		Vector3 temp = plunger.transform.localPosition;
		temp.x = barrel.transform.localPosition.x - b_p_diff;
		temp.z = barrel.transform.localPosition.z;

		if ((barrel.transform.localPosition.y - plunger.transform.localPosition.y) > MAX) {
			temp.y = barrel.transform.localPosition.y - MAX;
		} else if ((barrel.transform.localPosition.y - plunger.transform.localPosition.y) < MIN) {
			temp.y = barrel.transform.localPosition.y - MIN;
		}
			
		plunger.transform.localPosition = temp;
	}


	/// <summary>
	/// Moves the plunger to the left hand.
	/// </summary>
	public void leftMove() {
		GameObject thumb = GameObject.Find("RigidRoundHand_L/thumb");
		Vector3 temp = thumb.transform.localPosition;
		plunger.transform.localPosition = temp;
	}

	/// <summary>
	/// Moves the plunger to the right hand.
	/// </summary>
	public void rightMove() {
		GameObject thumb = GameObject.Find("RigidRoundHand_R/thumb");
		Vector3 temp = thumb.transform.localPosition;
		plunger.transform.localPosition = temp;
	}
	

	/// <summary>
	/// Simple method that draws back the plunger to test its functionality with.
	/// </summary>
	public void testMove() {
		Vector3 temp = plunger.transform.localPosition;
		temp.y += 0.1f;
		plunger.transform.localPosition = temp;
	}


	// Send a Vector3 of the hand here in the Update loop
	// Change the float here for the camera distance
	// This does not change depth with the mouse, but the mouse can't change
	// depth itself, so the y probably never changes.
	// Gravity makes this not very effective
	public void move(Vector3 other_obj) {
		//other_obj.z = Camera.main.WorldToScreenPoint(barrel.transform.position).z; // Set this to be the distance you want the object to be placed in front of the camera.
		//barrel.transform.position = Camera.main.ScreenToWorldPoint(other_obj);
		//rb.AddForce(other_obj - transform.position);
		//rb.velocity = (other_obj - transform.position);
		//rb.MovePosition(other_obj - transform.position);

		Vector3 v3 = Input.mousePosition;
		v3.z = Camera.main.WorldToScreenPoint(main.transform.position).z;
		v3 = Camera.main.ScreenToWorldPoint(v3);
		rb.MovePosition(v3);
	}

}
