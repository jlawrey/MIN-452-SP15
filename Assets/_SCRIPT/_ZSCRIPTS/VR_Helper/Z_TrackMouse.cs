using UnityEngine;
using System.Collections;

public class Z_TrackMouse : MonoBehaviour {
	
	
	public Transform VRHead;
	public float MouseMult = .001f;
	public bool mouseActive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButton(0) && mouseActive){
			VRHead.Translate(-Input.GetAxis("Mouse X")*MouseMult,Input.GetAxis("Mouse Y")*MouseMult,0);
		}
		if(VRHead.position.z <= -0.3f){
			VRHead.Translate(0,0,Input.GetAxis("Mouse ScrollWheel")/40);
		}
		                 
		

	}	
}
