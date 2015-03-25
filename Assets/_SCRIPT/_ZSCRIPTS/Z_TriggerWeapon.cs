using UnityEngine;
using System.Collections;

public class Z_TriggerWeapon : MonoBehaviour {
	public Animator anim;
	private AnimatorStateInfo anim_info;

	// Use this for initialization
	void Start () {

		Wii.DeactivateMotionPlus (1);
		anim = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		anim_info = anim.GetCurrentAnimatorStateInfo(0);

		Action ();
	}

	void Action(){

		//bool ShieldClosed;

		if (Input.GetKeyDown ("2") || Wii.IsPitchFast(1) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") ) {
			anim.SetTrigger ("Swing 2");
		}
		if(Input.GetKeyDown("1") ){
			anim.SetTrigger("Swing 1");
		}
		if(Input.GetKeyDown("3") || Wii.IsYawFast(1) && !Wii.IsPitchFast(1) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") ){
			anim.SetTrigger("Swing 3");
		}
		if(Input.GetKeyDown("space") || Wii.GetButtonDown(1,"A")){
			print ("SHIELD!!!");
			if (!anim.GetBool("Close Shield")){
				anim.SetBool("Close Shield",true);
			}else{
				anim.SetBool("Close Shield",false);
			}
		}

	}
}
