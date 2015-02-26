using UnityEngine;
using System.Collections;

public class Z_MoveVRHead : MonoBehaviour {

	public Transform VRHead;
	public float mult;
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
	
		if (Input.GetMouseButton(0) && HeadInput == InputMethod.Mouse) {
			Vector3 headmover = new Vector3(Input.GetAxis ("Horizontal") * mult, Input.GetAxis("Vertical")*mult, VRHead.localPosition.z);
			VRHead.localPosition = headmover;
		}
	}
}
