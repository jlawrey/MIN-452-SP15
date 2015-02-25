//using UnityEngine;
//using System.Collections;
//
//
//public class Z_HeadTrack : MonoBehaviour {
//	
//
//	public Vector3 mHeadPosition = new Vector3(0,0,0);   // position of the users head when the last frame was rendered
//	public float mHeadX = 0;                            // last calculated X position of the users head
//	public float mHeadY = 0;                            // last calculated Y position of the users head
//	public float mHeadDist = 0;                            // last calculated Z position of the users head
//	const float mRadiansPerPixel = (float)(Mathf.PI / 4.0f) / 1024.0f;    // WiiMote infrared camera radians per pixel in mm
//	public float mIRDotDistanceInMM = 180.5f;                // distance of the IR dots on shop glasses
//	public float mScreenHeightInMM = 353.0f;
//	public float mScreenWidthInMM = 353.0f;
//	// height of your screen
//	// height of your screen
//	public bool mWiiMoteIsAboveScreen = true;                    // is the WiiMote mounted above or below the screen?
//	public float mWiiMoteVerticleAngle = 0;// vertical angle of your WiiMote (as radian) pointed straight forward for me.
//	Matrix4x4 worldTransform = Matrix4x4.identity;
//	
//	
//	//vectors from the wii IR lights
//	public Vector2 firstPoint = new Vector2();//first IR Point XY Position
//	public Vector2 secondPoint = new Vector2();//second IR Point XY Position
//	int numvisible = 0;
//	
//	//Z stuff
//	public int pixel_width = 1680;
//	public int pixel_height = 1050;
//	private float screenAspect;
//	
//	//switches for different ways to calculate the perspective
//	public bool camMove = false;
//	public bool lookatswitch = false;
//	public bool Matrix = false;
//	public enum LookAtMatrixSide {RH,LH,OFF};
//	public enum TrackingMethod {Fake, Wiimote, Kinect, WebCam, Other};
//	public LookAtMatrixSide LookAtState = LookAtMatrixSide.OFF;
//	public TrackingMethod TrackingSetup = TrackingMethod.Fake;
//	
//	//Cameras and lookats
//	public Camera HeadCam;
//	public float FOVMult=1;
//	public Transform lookAtObject;
//	public float near = .05f;
//	public float far = 100;
//	
//	// Use this for initialization
//	void Start () {
//		
//		Wii.SetIRSensitivity (0, 90);
//		screenAspect = pixel_width / pixel_height;
//	}
//	
//	// Update is called once per frame
//	void LateUpdate () {
//
//		if(TrackingSetup == TrackingMethod.Wiimote){
//			TrackHead ();
//		}
//		if(TrackingSetup == TrackingMethod.Fake){
//			FakeTrack ();
//		}
//
//		
//	}
//	
//
//	public void FakeTrack(){
//
//		mHeadPosition = new Vector3 (Input.GetAxis ("Mouse X")/10, Input.GetAxis ("Mouse Y")/10, .6f);
//		mHeadX = mHeadPosition.x;
//		mHeadY = mHeadPosition.y;
//		mHeadDist = mHeadPosition.z;
//		HeadCam.ResetProjectionMatrix();
//		HeadCam.worldToCameraMatrix = LookAtMatrixRH(new Vector3(mHeadX, mHeadY, -mHeadDist),new Vector3(mHeadX, mHeadY,0),new Vector3(0,1,0));
//		Matrix4x4 m = PerspectiveOffCenter (
//			                                    near*(-.5f * screenAspect + mHeadX)/(mHeadDist),//left,  
//			                                    near*(.5f * screenAspect + mHeadX)/(mHeadDist),//right
//			                                    near*(-.5f - mHeadY)/(mHeadDist),//bottom
//			                                    near*(.5f - mHeadY)/(mHeadDist),//top
//			                                    near,//near
//			                                    100//far 
//			                                 );
//		HeadCam.projectionMatrix = m;
//
//
//	}
//
//	public void TrackHead(){
//
//		//load points based on Raw IR Data Vector 2 Values
//		firstPoint.x = Mathf.Abs(
//			((Wii.GetRawIRData (0) [0].x) * 1024)-1024
//			);
//		firstPoint.y = Mathf.Abs(
//			((Wii.GetRawIRData (0) [0].y) * 768)-768
//			);
//		secondPoint.x = Mathf.Abs(
//			((Wii.GetRawIRData (0) [1].x) * 1024)-1024
//			);
//		secondPoint.y = Mathf.Abs(
//			((Wii.GetRawIRData (0) [1].y) * 768)-768
//			);
//		
//		float dx = firstPoint.x - secondPoint.x;
//		float dy = firstPoint.y - secondPoint.y;
//		float pointDist = Mathf.Sqrt (dx * dx + dy * dy);
//		float angle = mRadiansPerPixel * pointDist / 2;
//		
//		//stuff I really don't understand that calculates where your head is
//		mHeadDist = (float)((((mIRDotDistanceInMM/2)/Mathf.Tan(angle))/mScreenHeightInMM))/	FOVMult;
//		;
//		
//		float avgX = (firstPoint.x + secondPoint.x)/2.0f;
//		float avgY = (firstPoint.y + secondPoint.y)/2.0f;
//		
//		mHeadX = (Mathf.Sin(mRadiansPerPixel * (avgX - 512)) * mHeadDist);
//		
//		float relativeVerticalAngle = (avgY - 384) * mRadiansPerPixel;
//		
//		if(mWiiMoteIsAboveScreen){
//			mHeadY =  .5f + (Mathf.Sin(relativeVerticalAngle + mWiiMoteVerticleAngle) * mHeadDist);
//			
//		}else{
//			mHeadY = -.5f + (Mathf.Sin(relativeVerticalAngle + mWiiMoteVerticleAngle) * mHeadDist);
//		}
//		
//		//now apply that crap to the camera transforms
//		//Vector3 newHeadPosition = new Vector3 (mHeadX, mHeadY, -mHeadDist);
//		
//		if(Matrix){//only do this if the user chooses, for experimenting with getting the perspective correct
//			
//			HeadCam.ResetProjectionMatrix();
//			Matrix4x4 m = PerspectiveOffCenter (//original Lee values
//			                                    near*(-.5f * screenAspect + mHeadX)/(mHeadDist),//left,  
//			                                    near*(.5f * screenAspect + mHeadX)/(mHeadDist),//right
//			                                    near*(-.5f - mHeadY)/(mHeadDist),//bottom
//			                                    near*(.5f - mHeadY)/(mHeadDist),//top
//			                                    near,//near
//			                                    100//far 
//			                                    );
//			HeadCam.projectionMatrix = m;
//		}
//		
//		switch(LookAtState){
//			
//		case (LookAtMatrixSide.LH): {
//			HeadCam.worldToCameraMatrix = LookAtMatrixLH(new Vector3(mHeadX, mHeadY, -mHeadDist),new Vector3(mHeadX, mHeadY,0),new Vector3(0,1,0));
//			break;		
//		}
//		case(LookAtMatrixSide.RH): {
//			HeadCam.worldToCameraMatrix = LookAtMatrixRH(new Vector3(mHeadX, mHeadY, -mHeadDist),new Vector3(mHeadX, mHeadY,0),new Vector3(0,1,0));
//			break;		
//		}
//		case(LookAtMatrixSide.OFF): {
//			HeadCam.ResetWorldToCameraMatrix();
//			break;		
//		}
//		}
//		
//		if(camMove){
//			HeadCam.transform.localPosition = new Vector3 (0, 0, -mHeadDist);
//		}
//
//		if (lookatswitch){//makes head cam look at lookat point when switched on
//			HeadCam.transform.LookAt (lookAtObject);
//		}
//
//	}
//	
//	static Matrix4x4 PerspectiveOffCenter(
//		float left, float right,
//		float bottom, float top,
//		float near, float far )
//	{    
//		//unity's projection matrix original
//		float x =  (2.0f * near) / (right - left);
//		float y =  (2.0f * near) / (top - bottom);
//		float a =  (right + left) / (right - left);
//		float b =  (top + bottom) / (top - bottom);
//		float c = - (far + near) / (far - near);
//		float d = -(2.0f * far * near )/ (far - near);
//		float e = -1.0f;
//
//		Matrix4x4 m = new Matrix4x4 ();
//
//		
//		m[0,0] = x;
//		m[0,1] = 0;
//		m[0,2] = a;
//		m[0,3] = 0;
//		m[1,0] = 0;
//		m[1,1] = y;
//		m[1,2] = b;
//		m[1,3] = 0;
//		m[2,0] = 0;
//		m[2,1] = 0;
//		m[2,2] = c;
//		m[2,3] = d;
//		m[3,0] = 0;
//		m[3,1] = 0;
//		m[3,2] = e;
//		m[3,3] = 0;
//		
//		
//		return m;
//		
//	}
//	
//	static Matrix4x4 LookAtMatrixRH(
//		Vector3 eye, Vector3 at,
//		Vector3 up)
//	{    
//		//unity's projection matrix original
//		
//		Vector3 zaxis = Vector3.Normalize (eye - at) ;
//		Vector3 xaxis = -Vector3.Normalize(Vector3.Cross (up, zaxis));
//		Vector3 yaxis = -Vector3.Cross(zaxis,xaxis);
//		
//		
//		Matrix4x4 m = new Matrix4x4 ();
//		
//		
//		
//		m [0, 0] = xaxis.x;
//		m [0, 1] = xaxis.y;
//		m [0, 2] = xaxis.z;
//		m [0, 3] = Vector3.Dot(xaxis, eye);
//		
//		m [1,0] = yaxis.x;
//		m [1,1] = yaxis.y;
//		m [1, 2] = yaxis.z;
//		m [1, 3] = -Vector3.Dot(yaxis, eye);
//		
//		m [2, 0] = zaxis.x;
//		m [2, 1] = zaxis.y;
//		m [2, 2] = zaxis.z;
//		m [2, 3] = -Vector3.Dot(zaxis, eye);
//		
//		m[3,0] = 0;
//		m[3,1] = 0;
//		m[3,2] = 0;
//		m[3,3] = 1;
//
//		return m;	
//	}
//
//	static Matrix4x4 LookAtMatrixLH(
//		Vector3 eye, Vector3 at,
//		Vector3 up)
//	{    
//		//unity's projection matrix original
//		
//		Vector3 zaxis = Vector3.Normalize (eye - at) ;
//		Vector3 xaxis = Vector3.Normalize(Vector3.Cross (up, zaxis));
//		Vector3 yaxis = (Vector3.Cross(zaxis,xaxis));
//		
//		
//		Matrix4x4 m = new Matrix4x4 ();
//		
//		
//		
//		m [0, 0] = xaxis.x;
//		m [0, 1] = yaxis.x;
//		m [0, 2] = zaxis.x;
//		m [0, 3] = 0;
//		
//		m [1,0] = xaxis.y;
//		m [1,1] = yaxis.y;
//		m [1, 2] = zaxis.y;
//		m [1, 3] = 0;
//		
//		m [2, 0] = xaxis.z;
//		m [2, 1] = yaxis.z;
//		m [2, 2] = zaxis.z;
//		m [2, 3] = 0;
//		
//		m[3,0] = -Vector3.Dot(xaxis, eye);
//		m[3,1] = -Vector3.Dot(yaxis, eye);
//		m[3,2] = -Vector3.Dot(zaxis, eye);
//		m[3,3] = 1;
//		
//		
//		return m;
//		
//	}
//	
//}
//
//
//
