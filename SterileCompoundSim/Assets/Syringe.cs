using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe {

	// Visible objects
	private GameObject barrel;
	private GameObject plunger;
	private GameObject needle;
	private GameObject cap;
	private GameObject main;

	Vector3 screenPoint;
	Vector3 offset;
	public Rigidbody rb;

	// 30ml dist = -3 to 0.6 localPosition.y
	// 60ml dist = -4.3 to 0
	// 3ml dist = 0 to 1.65
	private float MAX;
	private float MIN;
	private float b_p_diff;		// Difference between barrel and plunger when attached

	// Needle and syringe together?
	private int together = 0;
	private int cap_on = 0;
	private Vector3 b_n_diff;	// Difference between barrel and needle when attached

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

	public float getMax() {
		return MAX;
	}

	public float getMin() {
		return MIN;
	}

	public void putTogether() {
		together = 1;
		needle.transform.localEulerAngles = barrel.transform.localEulerAngles;
		needle.transform.localPosition = barrel.transform.localPosition - b_n_diff;
	}

	public void putCapOn() {
		cap_on = 1;
		cap.transform.localPosition = needle.transform.localPosition - new Vector3(0.0f, -0.3f, 0.0f);
	}

	public void takeApart() {
		together = 0;
	}

	public void takeCapOff() {
		cap_on = 0;
	}

	public int isTogether() {
		return together;
	}

	public int isCapOn() {
		return cap_on;
	}

	// Keeps the plunger within the bounds of the barrel if it is in the update() loop - this
	// is needed since the plunger and barrel do not have separate rigidbodies (if they have
	// separate rigidbodies, then they fly apart)
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
