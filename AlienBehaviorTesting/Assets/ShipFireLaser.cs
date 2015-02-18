using UnityEngine;
using System.Collections;

public class ShipFireLaser : MonoBehaviour {

	public int gunStrength = 1;
	public float fireRate = 1f;
	private float nextShot;
	public GameObject player;
	public GameObject laser;
	// Use this for initialization
	void Start () {
	
	player = GameObject.FindGameObjectWithTag("Player");
	nextShot = fireRate;
	}
	
	// Update is called once per frame
	void Update () {
	
	nextShot -= Time.deltaTime;
	if (nextShot <0)
	{
		FireShot();
	}
	
	
	}
	
	void FireShot()
	{
		//GameObject nextShot = Instantiate();
	}
	
}
