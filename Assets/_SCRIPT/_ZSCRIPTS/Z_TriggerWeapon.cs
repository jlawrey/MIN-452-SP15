using UnityEngine;
using System.Collections;

public class Z_TriggerWeapon : MonoBehaviour {
	public Animator anim;
	private AnimatorStateInfo anim_info;
	public AudioClip[] weaponSounds = new AudioClip[10];
	private GestureListener gestureListener;
	public GameObject kManager;

	// Use this for initialization
	void Start () {

		gestureListener = kManager.GetComponent<GestureListener> ();
//		Wii.DeactivateMotionPlus (1);
		anim = this.gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		anim_info = anim.GetCurrentAnimatorStateInfo(0);

		Action ();
	}

	void Action(){

		//bool ShieldClosed;

		if (Input.GetKeyDown ("2") ) {
			anim.SetTrigger ("Swing 2");
		}
		if(Input.GetKeyDown("1") ){
			anim.SetTrigger("Swing 1");
		}
		if(Input.GetKeyDown("3") || gestureListener.IsSwipeLeft() ){
			anim.SetTrigger("Swing 3");
			audio.PlayOneShot(weaponSounds[0]);
			print ("BIG SWING!!!");
		}
		if(Input.GetKeyDown("space") ){
			print ("SHIELD!!!");
			if (!anim.GetBool("Close Shield")){
				anim.SetBool("Close Shield",true);
			}else{
				anim.SetBool("Close Shield",false);
			}
		}

	}
}
