using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_60 : MonoBehaviour {

	Syringe syr_60;

	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.Find("60ml");
		syr_60 = new Syringe (60, temp.GetComponent<Rigidbody>());
	}

	void OnTriggerEnter(Collider other) {
		if (other.name == "Top_60") {
			// Check for twist to put on before putting together
			Debug.Log ("60 together");
			syr_60.putTogether ();
		} else if (other.name == "Cap_Bottom_60") {
			Debug.Log ("60 cap on");
			syr_60.putCapOn();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.name == "Top_60") {
			syr_60.takeApart ();
		} else if (other.name == "Cap_Bottom_60") {
			syr_60.takeCapOff ();
		}
	}

	// Update is called once per frame
	void Update () {
		//syr_60.testMove ();
		syr_60.keepInBounds ();
	}

/*
	// Update is called once per frame
	void FixedUpdate () {
		// This all is from hand's perspective
		RaycastHit hit;
		Vector3 temp = Input.mousePosition;

		// GetMouseButton registers every time, GetMouseButtonDown only does the first click
		if (Input.GetMouseButton(0)) {	// Change this to if hand is in closed position
			// Change the Vector3.right to rotation of hand when have hand
			//Ray handRay = new Ray (hand.position.transform, Vector3.right);

			Debug.Log("clicked");
			// Until get hand, use mouse:
			Ray handRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(handRay, out hit)) {
				Debug.Log("Holding down");
				if (hit.transform.name == "60ml") { // Add this if doing this from hand object
					//hit.collider.gameObject now refers to the 
					//cube under the mouse cursor if present
					Debug.Log("item");
					syr_60.move (Input.mousePosition);
				}
			}
		}

		syr_60.keepInBounds ();
	}
	*/
}
