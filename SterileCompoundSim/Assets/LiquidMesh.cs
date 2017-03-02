using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidMesh : MonoBehaviour {

	private GameObject barrel;
	private GameObject plunger;
	private GameObject needle;
	private GameObject liquid;
	private GameObject cap;
	// Needle Barrel
	// 3 = (-1, 3.4, 0)
	// 60 = (-1, 3.4, 0)
	// 30 = (0, 3.4, 0)
	// Needle - Cap
	// 3, 30, 60 = (0, -0.3, 0.0)

	// Use this for initialization
	void Start () {
		barrel = GameObject.Find("Barrel_3ml");
		plunger = GameObject.Find("Plunger_3ml");
		needle = GameObject.Find ("Needle_3");
		cap = GameObject.Find ("Cap_3");
		//liquid = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
		//liquid.transform.position = new Vector3 ();
	}
	
	// Update is called once per frame
	void Update () {
		// see if needle in liquid
		/*
		inLiquid = ...
		syringeTogether = ...	// needle, barrel, and plunger all put together
		if (inLiquid && syringeTogether) {
			// figure out color

			// resize liquid
			liquid.localScale = Vector3 (1, 1, zScale);
		}*/
		Vector3 temp = barrel.transform.localPosition - plunger.transform.localPosition;
		Debug.Log (temp.x + " " + temp.y + " " + temp.z);
	}

	public void move() {
		Vector3 temp = plunger.transform.localPosition;
		//if (dist >= -0.01900f) {
		//	temp.x += 0.005f;
		//} else i
		//		if (dist <= 0.105f) {
		//			temp.x -= 0.005f;
		//		}
		temp.y += 0.1f;
		plunger.transform.localPosition = temp;
	}
				
}
