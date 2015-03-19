using UnityEngine;
using System.Collections;

public class Z_TriggerWeapon : MonoBehaviour {
	public Animator anim;

	// Use this for initialization
	void Start () {

		anim = GetComponentInChildren<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
	

		if (Input.GetKeyDown ("1")) {
			anim.SetTrigger ("Swing 1");
		}
		if(Input.GetKeyDown("2")){
			anim.SetTrigger("Swing 2");
		}
		if(Input.GetKeyDown("3")){
			anim.SetTrigger("Swing 3");
		}
	}
}
