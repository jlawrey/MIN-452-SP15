using UnityEngine;
using System.Collections;

public class Z_ChuckHelmet : MonoBehaviour {

	public int force;
	public int torque;

	// Use this for initialization
	void Start () {
	
		this.gameObject.GetComponent<Rigidbody>().AddForce (0, 0, force);
		this.gameObject.GetComponent<Rigidbody>().AddTorque (torque, 0, 0);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
