using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartButton : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GameObject.Find("UI Camera").GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TaskOnClick()
	{
		animator.SetTrigger ("ShowPanToStation");
	}
}
