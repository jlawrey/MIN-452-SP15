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
	private Quaternion initQuat;
	public AnimationClip[] animations;
	public int firespeed = 500;
	private InteractionManager manager;

	// Use this for initialization
	void Awake () {

		gestureListener = kManager.GetComponent<GestureListener> ();
		anim = GetComponent<Animator> ();
		//proxyweapon.SetActive (false);
		initPosition = proxyweapon.transform.position;
		initRotation = proxyweapon.transform.localEulerAngles;
		initQuat = proxyweapon.transform.localRotation;

	}
	
	// Update is called once per frame
	void Update () {

		anim_info = anim.GetCurrentAnimatorStateInfo(0);
		StartCoroutine(WeaponAction ());
		StartCoroutine (CrossBowFire ());
	}


	IEnumerator CrossBowFire(){

//		if(manager == null)
//		{
//			manager = InteractionManager.Instance;
//		}
		if (Input.GetKeyDown ("7") || (gestureListener.IsPush() && gameObject.tag == "Crossbow") ) {
			//loop this for rapid fire
			print ("bolting!!!");
			GameObject bolt_pfab = Resources.Load <GameObject>("bolt");
			GameObject bolt = Instantiate(bolt_pfab,initPosition,Quaternion.identity) as GameObject ;
			//bolt.transform.parent = this.gameObject.transform;
			bolt.transform.localEulerAngles = new Vector3(0,0,0);
			audio.PlayOneShot(weaponSounds[1]);
			anim.SetTrigger("Fire Bolt");
			bolt.rigidbody.AddRelativeForce(0,0,firespeed);
			//yield return new WaitForSeconds(weaponSounds[1].length);
			yield return new WaitForSeconds(3);
			Destroy(bolt);
		}


	}

	IEnumerator WeaponAction(){

		//bool ShieldClosed;



		if(Input.GetKeyDown("3") || gestureListener.IsSwipeLeft() ){
			//proxyweapon.SetActive(true);
			//kinectweapon.SetActive(false);
			print ("LEAVING HAND");
			kinectweapon.transform.parent = null;
			kinectweapon.transform.position = new Vector3(initPosition.x,initPosition.y,initPosition.z);
			kinectweapon.transform.localEulerAngles = initRotation;

			
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
			kinectweapon.transform.localEulerAngles = initRotation;


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
		if((Input.GetKeyDown("5") && gameObject.tag == "Spear") || (gestureListener.IsPush() && gameObject.tag == "Spear") ){
			//proxyweapon.SetActive(true);
			//kinectweapon.SetActive(false);
			
			print ("LEAVING HAND");
			kinectweapon.transform.parent = null;
			kinectweapon.transform.position = new Vector3(initPosition.x,initPosition.y,initPosition.z);
			kinectweapon.transform.localEulerAngles = initRotation;
			
			
			anim.SetTrigger("Throw Spear");
			audio.PlayOneShot(weaponSounds[1]);
			yield return new WaitForSeconds(animations[1].length);

			
			print ("RETURN TO HAND");
			kinectweapon.transform.position = new Vector3(rightHand.transform.position.x,rightHand.transform.position.y,rightHand.transform.position.z);
			kinectweapon.transform.parent= rightHand;
		}


	}
}
