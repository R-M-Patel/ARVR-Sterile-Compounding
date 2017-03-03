using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICamera : MonoBehaviour 
{
	private Camera MainCamera;
	private Camera uiCamera;
	private Animator animator;
	private bool hasStartedPan = false;
	public GameObject MainMenuUI;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
		uiCamera = GetComponent<Camera> ();
		MainCamera = GameObject.Find ("Main Camera").GetComponent<Camera> ();

		MainCamera.targetDisplay = 1;
		uiCamera.targetDisplay = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("PanToStation") && !hasStartedPan) 
		{
			hasStartedPan = true;
			MainMenuUI.SetActive (false);
		}
		
		if (hasStartedPan && animator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) 
		{
			MainCamera.targetDisplay = 0;
			uiCamera.targetDisplay = 1;
		}
	}
}
