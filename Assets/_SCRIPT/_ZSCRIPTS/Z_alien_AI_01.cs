using UnityEngine;
using System.Collections;

public class Z_alien_AI_01 : MonoBehaviour {

	public float y_offset;
	public float y_amp;
	public float y_freq;
	public float random_seed;
	public GameObject floater;
	private Vector3 alienPosition;
	public Transform target;
	private Vector3 targetPosition;
	private Vector3 attackPosition;
	public float speed;
	private bool isAttacking = false;
	private bool doattack;
	private bool tracking = true;
	public float force;
	public Transform[] followers;
	private Animator anim;

	// Use this for initialization
	void Start () {

		anim = gameObject.GetComponent<Animator> ();
		StartCoroutine (Attack ());
		random_seed = Random.Range (.1f, .3f);

	}
	
	// Update is called once per frame
	void FixedUpdate () {



		//look at player head if not attacking
		foreach (Transform follower in followers){
			follower.LookAt (target.transform.position,Vector3.up);
		}

		//sets up the floating Y values
//		float floatY = (Mathf.Sin (Time.frameCount * y_freq) * random_seed) + y_offset;
//		//moves the char left and right on the X-axis
//		if(floater.transform.position.x < .6){
//			floater.transform.Translate(.01f,0,0,Space.World);
//		}else{
//			floater.transform.Translate(-.01f,0,0,Space.World);
//		}
//		floater.transform.localPosition = new Vector3 (floater.transform.localPosition.x, floatY, floater.transform.localPosition.z);

	}

	public IEnumerator Attack(){// coroutine to attack randomly by adding local forward force, which will propel the alien forward in the local Z-axis, which should be facing the player's last vector 

		float waittime = Random.Range (1, 5.5f);
		yield return new WaitForSeconds (waittime);
		isAttacking = true;
		anim.SetTrigger ("Attack");
		print (waittime + " ATTACK!!!");
		anim.applyRootMotion = false;
		floater.gameObject.rigidbody.AddRelativeForce (0, 0, force);
		//floater.gameObject.rigidbody.AddRelativeTorque (force/2,0,0);

	}



}
