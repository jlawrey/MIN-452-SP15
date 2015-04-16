using UnityEngine;
using System.Collections;

public class Z_TriggerWeapon : MonoBehaviour {
	public Animator anim;
	private AnimatorStateInfo anim_info;
	public AudioClip[] weaponSounds = new AudioClip[10];
	private GestureListener gestureListener;
	public GameObject kManager;
	public GameObject proxyweapon;
	public GameObject kinectweapon;

	// Use this for initialization
	void Awake () {
		gestureListener = kManager.GetComponent<GestureListener> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		anim_info = anim.GetCurrentAnimatorStateInfo(0);
		StartCoroutine(Action ());
	}

	IEnumerator Action(){

		//bool ShieldClosed;

		if (Input.GetKeyDown ("2") ) {
			anim.SetTrigger ("Swing 2");
		}
		if(Input.GetKeyDown("1") ){
			anim.SetTrigger("Swing 1");
		}
		if(Input.GetKeyDown("3") || gestureListener.IsSwipeLeft() ){
			proxyweapon.SetActive(true);
			kinectweapon.SetActive(false);
			anim.SetTrigger("Special Hammer 1");
			audio.PlayOneShot(weaponSounds[0]);
			yield return new WaitForSeconds(2.0f);
			proxyweapon.SetActive(false);
			kinectweapon.SetActive(true);

		}
		if(Input.GetKeyDown("4") || gestureListener.IsPush() ){
			proxyweapon.SetActive(true);
			kinectweapon.SetActive(false);
			anim.SetTrigger("Special Sword 1");
			audio.PlayOneShot(weaponSounds[0]);
			yield return new WaitForSeconds(2.0f);
			proxyweapon.SetActive(false);
			kinectweapon.SetActive(true);
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
