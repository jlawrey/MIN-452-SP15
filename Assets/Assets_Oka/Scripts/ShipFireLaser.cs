using UnityEngine;
using System.Collections;

public class ShipFireLaser : MonoBehaviour {

	public int gunStrength = 1;
	public float fireRate = 2f;
	private float nextShot;
	public GameObject player;
	public GameObject laser;
	public GameObject laserGun;
	public bool firing = false;
	public Animator animator;
	// Use this for initialization
	void Start () {
	
	player = GameObject.FindGameObjectWithTag("Player");
	nextShot = fireRate;
	}
	
	// Update is called once per frame
	void Update () {
	
	nextShot -= Time.deltaTime;
	
		//laserGun.transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);
		laserGun.transform.LookAt(player.transform.position );
	//laserGun.transform.rotation = Quaternion.Inverse(laserGun.transform.rotation);
	if (nextShot <0 && firing == true)
	{
		FireShot();
		nextShot = fireRate;
	}
	
	
	}
	
	void FireShot()
	{
		//Quaternion rotateShot = Quaternion.FromToRotation(player.transform.position, transform.position);
		//animator.SetTrigger("laserShot");
		GameObject nextShot = Instantiate(laser, transform.position, transform.rotation) as GameObject;
		nextShot.transform.LookAt(player.transform);
	}
	
}
