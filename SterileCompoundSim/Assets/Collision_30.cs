using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_30 : MonoBehaviour {

	Syringe syr_30;

	// Use this for initialization
	void Start () {
		syr_30 = new Syringe (30, GetComponent<Rigidbody>());
	}

	void OnTriggerEnter(Collider other) {
		if (other.name == "Top_30") {
			// Check for twist to put on before putting together
			Debug.Log ("30 together");
			syr_30.putTogether ();
		} else if (other.name == "Cap_Bottom_30") {
			Debug.Log ("30 cap on");
			syr_30.putCapOn ();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.name == "Top_30") {
			syr_30.takeApart ();
		} else if (other.name == "Cap_Bottom_30") {
			syr_30.takeCapOff ();
		}
	}

	// Update is called once per frame
	void Update () {
		syr_30.keepInBounds ();
	}

}
