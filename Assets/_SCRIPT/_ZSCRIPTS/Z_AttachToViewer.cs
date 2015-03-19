using UnityEngine;
using System.Collections;

public class Z_AttachToViewer : MonoBehaviour {

	public Transform VRHead;
	private Vector3 offset;

	// Use this for initialization
	void Start () {

		VRHead = GameObject.FindGameObjectWithTag ("Player").transform;
		offset = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.position = VRHead.localPosition + offset;
	
	}
}
