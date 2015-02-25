using UnityEngine;
using System.Collections;

public class keyMoveCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.Translate(-Input.GetAxis("Horizontal"),0,-Input.GetAxis("Vertical"), null);
	
	
	}
}
