using UnityEngine;
using System.Collections;

public class ShipFireLaser : MonoBehaviour {

	//Connection to player, finds object with player tag
	public GameObject player;


	public float fireRate = 2f;	//How often the ship shoots


	private float nextShot;	//Timer
	private bool firing = true;//Sets where ship is firing or not, usually true
	public GameObject laser;//Connection to laser shot prefab
	public GameObject laserGun;//Connection to gun geometry
	public Animator anim;//Connection to animator

	public Transform shotLocation;



	void Start () {
	//Find object with player tag
	player = GameObject.FindGameObjectWithTag("Player");
	nextShot = Random.Range(fireRate, fireRate*2);//Initalize timer
	}
	

	void Update () {
	
	nextShot -= Time.deltaTime;//Decrease timer by deltaTime
	
	//Point gun geometry at player
	laserGun.transform.LookAt(player.transform.position );
	
	//If timer has hit 0, fire a shot
	if (nextShot <0 && firing == true)
	{
		StartCoroutine(FireShot());
		nextShot = fireRate;//Reset timer
	}
	
	
	}
	
	public IEnumerator FireShot()
	{
		anim.SetTrigger ("fireShot");
		yield return new WaitForSeconds (0.75f);
		//Instantiate shot
		//Instantiate shot
		Vector3 createPos = new Vector3 (shotLocation.position.x,shotLocation.position.y,shotLocation.position.z);
		GameObject nextShot = Instantiate(laser, createPos, transform.rotation) as GameObject;
		nextShot.GetComponent<LaserShot> ().force = 700;
		//Point it at the player, its own script handles movement
		nextShot.transform.LookAt(player.transform);
	}




	
}
