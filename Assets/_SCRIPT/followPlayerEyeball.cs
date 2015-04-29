using UnityEngine;
using System.Collections;

public class followPlayerEyeball : MonoBehaviour {
	

	private float force = 1000f;
	private float moveForce = 900f;
	private float maxSpeed = .5f;
	private float orbitSpeed = 1f;
	private float orbitCorrect = 5f;
	private GameObject player;
	private float waitTime;
	private bool attacking;
	private bool reachedPos = false;

	public Animator anim;
	public Material[] materials;

	private Vector3 laserDistance;
	private float speed = 1f;

	public GameObject laserSpawnPoint;

	public GameObject laserShot;

	public float fireRate = 2f;	//How often the ship shoots
	private float nextShot;	//Timer

	// Use this for initialization
	void Start () {
		anim = this.gameObject.GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag("Player");
		waitTime = Random.Range (0, 1f);
		StartCoroutine (Attack ());
		nextShot = 0;


		laserShot.GetComponent<LaserShot>().force = 150;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		/*if (!attacking) {

		}*/

		transform.LookAt(player.transform);

		if (attacking) 
		{
			float distance=Vector3.Distance(transform.position, laserDistance);
			transform.position = Vector3.Lerp (
				transform.position, laserDistance,
				Time.deltaTime* speed/distance);

		}

		if (Vector3.Distance (transform.position, laserDistance) < 2f) 
		{
			reachedPos = true;

		}

		if (reachedPos) 
		{
			nextShot -= Time.deltaTime;

			if (nextShot <= 0 )
			{
				StartCoroutine (FireShot());
				nextShot = fireRate;//Reset timer
			}
			
			
		}
		

	}


		

public IEnumerator FireShot()
{
	anim.SetTrigger ("fireShot");
	yield return new WaitForSeconds (0.75f);
	//Instantiate shot
	Vector3 createPos = laserSpawnPoint.transform.position;
	GameObject nextShot = Instantiate(laserShot, createPos, transform.rotation) as GameObject;
	nextShot.transform.localScale = new Vector3 (0.01f, 0.01f, 0.02f);
	//Point it at the player, its own script handles movement
	nextShot.transform.LookAt(player.transform);
}


public IEnumerator Attack(){

		attacking = false;
		float waittime = Random.Range (1, 18.5f);
		yield return new WaitForSeconds (waittime);
		//anim.SetBool ("attack", true);
		laserDistance = new Vector3( player.transform.position.x + Random.Range(0,0.5f) ,player.transform.position.y,player.transform.position.z +2.5f );
		attacking = true;


		//gameObject.rigidbody.AddRelativeForce (0, 0, force);
		//gameObject.rigidbody.AddRelativeTorque (force/4,0,0);
		
	}
	
}
