using UnityEngine;
using System.Collections;

public class Z_Crossbow : MonoBehaviour {

	public Animator anim;
	private AnimatorStateInfo anim_info;
	public AudioClip[] weaponSounds = new AudioClip[10];
	private GestureListener gestureListener;
	public GameObject kManager;
	public GameObject proxyweapon;
	public GameObject kinectweapon;
	public Transform rightHand;
	private Vector3 bowPosition;
	private Vector3 bowRotation;
	private Quaternion bowQuat;
	public AnimationClip[] animations;
	public int firespeed = 500;
	// Use this for initialization
	void Start () {
	
		gestureListener = kManager.GetComponent<GestureListener> ();
		anim = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("7") || gestureListener.IsPush ()) {
			StartCoroutine (CrossBowFire (0));
			StartCoroutine (CrossBowFire (15));
			StartCoroutine (CrossBowFire (-15));

		}
	}

	IEnumerator CrossBowFire(int angle){

		//for (int i=0; i<3; i++) {//loop this for rapid fire
			bowPosition = kinectweapon.transform.position;
			bowQuat = kinectweapon.transform.rotation;
			bowRotation = kinectweapon.transform.localEulerAngles;
			print ("bolting!!!");
			GameObject bolt_pfab = Resources.Load <GameObject> ("bolt");
			GameObject bolt = Instantiate (bolt_pfab, bowPosition, Quaternion.identity) as GameObject;
			bolt.transform.localEulerAngles = new Vector3 (0, angle, 0);
			bolt.transform.parent = this.gameObject.transform;
			audio.PlayOneShot (weaponSounds [1]);
			anim.SetTrigger ("Fire Bolt");
			bolt.transform.parent = null;
			bolt.rigidbody.AddRelativeForce (0, 0, firespeed);

			//yield return new WaitForSeconds(weaponSounds[1].length);
			yield return new WaitForSeconds (3);
			Destroy (bolt);
		}
	//}
		
		
}
