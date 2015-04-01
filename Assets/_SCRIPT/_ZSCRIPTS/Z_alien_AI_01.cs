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

	// Use this for initialization
	void Start () {
		StartCoroutine (Attack ());
		random_seed = Random.Range (.1f, .3f);

	}
	
	// Update is called once per frame
	void Update () {

		//start the attack function
		//sets up the floating Y values
		float floatY = (Mathf.Sin (Time.frameCount * y_freq) * random_seed) + y_offset;
		//moves the char left and right on the X-axis
		if(floater.transform.position.x < 1){
			floater.transform.Translate(.01f,0,0,Space.World);
		}else{
			floater.transform.Translate(-.01f,0,0,Space.World);
		}
		//look at player head if not attacking
		if(!isAttacking){
			floater.transform.LookAt(target);
		}
		floater.transform.localPosition = new Vector3 (floater.transform.localPosition.x, floatY, floater.transform.localPosition.z);

	}

	public IEnumerator Attack(){

		float waittime = Random.Range (1, 18.5f);
		yield return new WaitForSeconds (waittime);
		isAttacking = true;
		print (waittime + " ATTACK!!!");
		floater.gameObject.rigidbody.AddRelativeForce (0, 0, force);
		floater.gameObject.rigidbody.AddRelativeTorque (force/4,0,0);

	}



}
