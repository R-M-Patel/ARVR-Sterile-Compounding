using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_3 : MonoBehaviour {

	Syringe syr_3;

	// Use this for initialization
	void Start () {
		syr_3 = new Syringe (3, GetComponent<Rigidbody>());
	}

	void OnTriggerEnter(Collider other) {
		if (other.name == "Top_3") {
			// Check for twist to put on before putting together
			Debug.Log ("3 together");
			syr_3.putTogether ();
		} else if (other.name == "Cap_Bottom_3") {
			Debug.Log ("3 cap on");
			syr_3.putCapOn ();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.name == "Top_3") {
			syr_3.takeApart ();
		} else if (other.name == "Cap_Bottom_3") {
			syr_3.takeCapOff ();
		}
	}

	// Update is called once per frame
	void Update () {
		syr_3.keepInBounds ();
	}

}
