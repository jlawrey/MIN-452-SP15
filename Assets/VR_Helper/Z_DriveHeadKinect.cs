using UnityEngine;
using System.Collections;

public class Z_DriveHeadKinect : MonoBehaviour {
	public Transform kinectInput;
	public Transform vrHeadOutput;
	private Vector3 newpos;
	public bool kinectActive;
	public bool invert;
	private int flip;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	if (invert) {
			flip = -1;
	} else {
			flip = 1;
	}
	if(kinectActive){
		newpos = new Vector3 (kinectInput.position.x * flip, kinectInput.position.y , kinectInput.position.z);
		vrHeadOutput.position = newpos;
	}


	}
}
