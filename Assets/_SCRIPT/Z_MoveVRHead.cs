using UnityEngine;
using System.Collections;

public class Z_MoveVRHead : MonoBehaviour {

	public Transform VRHead;
	public float mult = .001f;
	public enum InputMethod {Mouse,Wii,Face};
	public InputMethod HeadInput;
	private float x;
	private float y;
	private float z;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButton(0) && HeadInput == InputMethod.Mouse && VRHead.position.z < 0) {
			VRHead.Translate(-Input.GetAxis("Horizontal")*mult,Input.GetAxis("Vertical")*mult,Input.GetAxis("Mouse ScrollWheel")/10);
		}
	}



}
