using UnityEngine;
using System.Collections;

public class Z_DriveHeadKinect : MonoBehaviour {
	public Transform kinectInput;
	public Transform vrHeadOutput;
	private Vector3 newpos;
	public bool kinectActive;
	public bool invert;
	private int flip;
	private float offsetZ;
	public float GlobalDistanceDiv = 1;
	// Use this for initialization
	void Start () {

		offsetZ = kinectInput.position.z;
	}
	
	// Update is called once per frame
	void Update () {
	
	if (invert) {
			flip = -1;
	} else {
			flip = 1;
	}
	if(kinectActive){
		newpos = new Vector3 (kinectInput.position.x/GlobalDistanceDiv * flip, kinectInput.position.y/GlobalDistanceDiv , (kinectInput.position.z + offsetZ)/GlobalDistanceDiv);
		vrHeadOutput.position = newpos;
	}


	}
}
