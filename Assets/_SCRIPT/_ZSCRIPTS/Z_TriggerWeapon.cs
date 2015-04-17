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
	public Transform rightHand;
	private Vector3 initPosition;
	private Vector3 initRotation;
	public AnimationClip[] animations;

	// Use this for initialization
	void Awake () {

		gestureListener = kManager.GetComponent<GestureListener> ();
		anim = GetComponent<Animator> ();
		//proxyweapon.SetActive (false);
		initPosition = proxyweapon.transform.position;
		initRotation = proxyweapon.transform.localEulerAngles;

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
			//proxyweapon.SetActive(true);
			//kinectweapon.SetActive(false);
			print ("LEAVING HAND");
			kinectweapon.transform.parent = null;
			kinectweapon.transform.position = new Vector3(initPosition.x,initPosition.y,initPosition.z);

			
			anim.SetTrigger("Special Hammer 1");
			audio.PlayOneShot(weaponSounds[0]);
			yield return new WaitForSeconds(animations[0].length);

			print ("RETURN TO HAND");
			kinectweapon.transform.position = new Vector3(rightHand.transform.position.x,rightHand.transform.position.y,rightHand.transform.position.z);
			kinectweapon.transform.parent= rightHand;

			//proxyweapon.SetActive(false);
			//kinectweapon.SetActive(true);

		}
		if(Input.GetKeyDown("4") || gestureListener.IsSwipeDown() ){
			//proxyweapon.SetActive(true);
			//kinectweapon.SetActive(false);

			print ("LEAVING HAND");
			kinectweapon.transform.parent = null;
			kinectweapon.transform.position = new Vector3(initPosition.x,initPosition.y,initPosition.z);


			anim.SetTrigger("Special Sword 1");
			audio.PlayOneShot(weaponSounds[1]);
			yield return new WaitForSeconds(animations[1].length);
			//proxyweapon.SetActive(false);
			//kinectweapon.SetActive(true);

			print ("RETURN TO HAND");
			kinectweapon.transform.position = new Vector3(rightHand.transform.position.x,rightHand.transform.position.y,rightHand.transform.position.z);
			kinectweapon.transform.parent= rightHand;
		}

		//triggers the throwing of the spear or the crossbow bolt
		if(Input.GetKeyDown("5") || gestureListener.IsPush() ){
			//proxyweapon.SetActive(true);
			//kinectweapon.SetActive(false);
			
			print ("LEAVING HAND");
			kinectweapon.transform.parent = null;
			kinectweapon.transform.position = new Vector3(initPosition.x,initPosition.y,initPosition.z);
			kinectweapon.transform.localEulerAngles = initRotation;
			
			
			anim.SetTrigger("Throw Spear");
			audio.PlayOneShot(weaponSounds[1]);
			yield return new WaitForSeconds(animations[1].length);
			//proxyweapon.SetActive(false);
			//kinectweapon.SetActive(true);
			
			print ("RETURN TO HAND");
			kinectweapon.transform.position = new Vector3(rightHand.transform.position.x,rightHand.transform.position.y,rightHand.transform.position.z);
			kinectweapon.transform.parent= rightHand;
		}


	}
}
