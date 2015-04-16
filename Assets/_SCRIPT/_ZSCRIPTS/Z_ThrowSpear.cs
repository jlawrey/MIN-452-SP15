using UnityEngine;
using System.Collections;

public class Z_ThrowSpear : MonoBehaviour {


	public Animator anim;
	private AnimatorStateInfo anim_info;
	public AudioClip[] weaponSounds = new AudioClip[10];
	private GestureListener gestureListener;
	public GameObject kManager;
	public GameObject proxyweapon;
	public GameObject kinectweapon;
	public Transform rightHand;
	private Vector3 initPosition;
	private Quaternion initRotation;
	public AnimationClip[] animations;
	// Use this for initialization


	void Awake () {

		gestureListener = kManager.GetComponent<GestureListener> ();
		anim = GetComponent<Animator> ();
		//proxyweapon.SetActive (false);
		initPosition = proxyweapon.transform.position;
		initRotation = proxyweapon.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {


		StartCoroutine (Action ());
	}

	public IEnumerator Action(){

		//triggers the throwing of the spear or the crossbow bolt
		if(Input.GetKeyDown("5") || gestureListener.IsPush() ){
			//proxyweapon.SetActive(true);
			//kinectweapon.SetActive(false);
			
			print ("LEAVING HAND");
			kinectweapon.transform.parent = null;
			kinectweapon.transform.position = new Vector3(initPosition.x,initPosition.y,initPosition.z);
			kinectweapon.transform.rotation = initRotation;
			
			
			anim.SetTrigger("Throw Spear");
			audio.PlayOneShot(weaponSounds[1]);
			yield return new WaitForSeconds(animations[1].length);
			//proxyweapon.SetActive(false);
			//kinectweapon.SetActive(true);
			
			print ("RETURN TO HAND");
			kinectweapon.transform.position = new Vector3(rightHand.transform.position.x,rightHand.transform.position.y,rightHand.transform.position.z);
			kinectweapon.transform.rotation = rightHand.transform.rotation;
			kinectweapon.transform.parent= rightHand;
		}
	}
}
