using UnityEngine;
using System.Collections;

public class Z_TrackInput : MonoBehaviour {

	public Transform kinectInput;//the input node of the kinect head tracking object
	public float GlobalDistanceDiv = 1.0f;
	private float offsetZ;
	public float offset;

	public Transform VRHead;
	public float MouseMult = .001f;
	public enum InputMethod {Mouse,Wii,Face,Kinect};
	public InputMethod HeadInput;
	
	//WiiMote global variables
	//public Vector3 mHeadPosition = new Vector3(0,0,0);  // position of the users head
	private float mHeadX = 0;                           // last calculated X position of the users head
	private float mHeadY = 0;                           // last calculated Y position of the users head
	private float mHeadDist = 0;                        // last calculated Z position of the users head
	const float mRadiansPerPixel = (float)(Mathf.PI / 4.0f) / 1024.0f;    // don't change this! it's a fixed value for the WiiMote infrared camera
	public float mIRDotDistanceInMM = 180.5f;           // distance of the IR dots in mm. change it, if you are not using the original nintendo sensor bar//*this is set to the Lee shop glasses
	public float mScreenHeightInMM = 353.0f;
	public float mScreenWidthInMM = 353.0f;
	public bool mWiiMoteIsAboveScreen = true;           // is the WiiMote mounted above or below the screen?
	public float mWiiMoteVerticleAngle = 0;				// vertical angle of your WiiMote (as radian) pointed straight forward for me.
	public float FOVMult = 1.0f;						//offsets the Z-distance value of the head, which seems to calculate double the actual value in real-space
	public Vector2 IR1;									//IR 1 position in 2-d space
	public Vector2 IR2;									//IR 2 position in 2-d space
	
	// Use this for initialization
	void Start () {

		offsetZ = kinectInput.position.z;

	}
	
	// Update is called once per frame
	void Update () {
	
		// Mouse Movement Tracking
		if (HeadInput == InputMethod.Mouse && Input.GetMouseButton(0)  && VRHead.position.z < 0) {
			VRHead.Translate(-Input.GetAxis("Horizontal")*MouseMult,Input.GetAxis("Vertical")*MouseMult,-Input.GetAxis("Mouse ScrollWheel")/40);
			//mHeadPosition = VRHead.position;
		}
		//Wii Head Tracking
		if (HeadInput == InputMethod.Wii && Wii.IsActive(0)){//if we are using 2-point WiiMote IR tracking

			//first get the pixel values from the WiiMote IR cam and de-normalize and flip them (so that they are using the actual pixel values from the camera)
			IR1 = new Vector2(
							Mathf.Abs(
							((Wii.GetRawIRData (0) [0].x) * 1024)-1024)
							,
			                Mathf.Abs(
							((Wii.GetRawIRData (0) [0].y) * 768)-768)
			                );
			IR2 = new Vector2(
							Mathf.Abs(
							((Wii.GetRawIRData (0) [1].x) * 1024)-1024)
							,
							Mathf.Abs(
							((Wii.GetRawIRData (0) [1].y) * 768)-768)
							);
			//then set head location to calculated position from IR points
			VRHead.localPosition = HeadXYZFrom2Points(IR1,IR2);
			//mHeadPosition = HeadXYZFrom2Points(IR1,IR2);
		}
		//Kinect Head Tracking
		if (HeadInput == InputMethod.Kinect) {

			Vector3 newpos = new Vector3 (kinectInput.position.x/GlobalDistanceDiv * -1, kinectInput.position.y/GlobalDistanceDiv , ((kinectInput.position.z)/GlobalDistanceDiv) + (offsetZ + offset));
			VRHead.position = newpos;
		}


	}
	public Vector3 HeadXYZFrom2Points(Vector2 firstPoint, Vector2 secondPoint){

		//calculate Vector3 position of head
		float dx = firstPoint.x - secondPoint.x;
		float dy = firstPoint.y - secondPoint.y;
		float pointDist = Mathf.Sqrt (dx * dx + dy * dy);
		float angle = mRadiansPerPixel * pointDist / 2;
		mHeadDist = (float)((((mIRDotDistanceInMM/2)/Mathf.Tan(angle))/mScreenHeightInMM))/	FOVMult;//FOVMult is used to offset the distance of the head if need be
		float avgX = (firstPoint.x + secondPoint.x)/2.0f;
		float avgY = (firstPoint.y + secondPoint.y)/2.0f;
		mHeadX = (Mathf.Sin(mRadiansPerPixel * (avgX - 512)) * mHeadDist);
		float relativeVerticalAngle = (avgY - 384) * mRadiansPerPixel;
		if(mWiiMoteIsAboveScreen){
			mHeadY =  .5f + (Mathf.Sin(relativeVerticalAngle + mWiiMoteVerticleAngle) * mHeadDist);
		}else{
			mHeadY = -.5f + (Mathf.Sin(relativeVerticalAngle + mWiiMoteVerticleAngle) * mHeadDist);
		}
		//build a Vec3 based on the virtual head position
		return new Vector3 (mHeadX, mHeadY, -mHeadDist);
	}



}
