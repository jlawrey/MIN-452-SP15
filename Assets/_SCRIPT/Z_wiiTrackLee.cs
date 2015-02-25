//using UnityEngine;
//using System.Collections;
//
//
//public class Z_wiiTrackLee : MonoBehaviour {
//
//	bool mUseWiiMotes = true;                        // to disable wiiMote support
//
//	public Vector3 mHeadPosition = new Vector3(0,0,0);           // position of the users head when the last frame was rendered
//	public float mHeadX = 0;                            // last calculated X position of the users head
//	public float mHeadY = 0;                            // last calculated Y position of the users head
//	public float mHeadDist = 0;                            // last calculated Z position of the users head
//	const float mRadiansPerPixel = (float)(Mathf.PI / 4.0f) / 1024.0f;    // don't change this! it's a fixed value for the WiiMote infrared camera
//	public float mIRDotDistanceInMM = 180.5f;                // distance of the IR dots in mm. change it, if you are not using the original nintendo sensor bar//*this is set to the Lee shop glasses
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
//
//	public Transform bleft;
//	public Transform topleft;
//	public Transform bright;
//	public Vector2 firstPoint = new Vector2();//first IR Point XY Position
//	public Vector2 secondPoint = new Vector2();//second IR Point XY Position
//	int numvisible = 0;
//
//	//Z stuff
//	public int pixel_width = 1680;
//	public int pixel_height = 1050;
//	private float screenAspect;
//
//	//switches for perspective stuff
//	public bool camMove = false;
//	public bool CAVE = false;
//	public bool lookatswitch = true;
//	public bool FOV = false;
//	public bool Matrix = false;
//	public enum LookAtMatrixSide {RH,LH,OFF};
//	public LookAtMatrixSide LookAtState = LookAtMatrixSide.OFF;
//
//	//Cameras and lookats
//	public Camera HeadCam;
//	public float FOV1;
//	public float FOVMult=1;
//	public Transform lookAtObject;
//	float left = -0.2F;
//	float right = 0.2F;
//	float top = 0.2F;
//	float bottom = -0.2F;
//	public float near = .05f;
//	public float far = 100;
//
//	// Use this for initialization
//	void Start () {
//
//		LookAtState = LookAtMatrixSide.RH;
//		Wii.SetIRSensitivity (0, 90);
//		screenAspect = pixel_width / pixel_height;
//	}
//	
//	// Update is called once per frame
//	void LateUpdate () {
//
//		TrackHead ();
//		if(CAVE){
//
//			Vector3 BottomLeftCorner = bleft.transform.position;
//			Vector3 BottomRightCorner = bright.transform.position;
//			Vector3 TopLeftCorner = topleft.transform.position;
//			Vector3 trackerPosition = new Vector3(mHeadX,mHeadY,mHeadDist);
//
//			Matrix4x4 genProjection = GeneralizedPerspectiveProjection(
//				BottomLeftCorner,//0 - screenWidth/2, currentHeight - screen Height, mHeadDist
//				BottomRightCorner,//0 + screenWidth/2, currentHeight - screen Height, mHeadDist
//				TopLeftCorner,//0-screenWidth/2, 0, mHeadDist 
//				trackerPosition,//mHeadX, mHeadY, -mHeadDist
//				(float)HeadCam.nearClipPlane,//near
//				(float)HeadCam.farClipPlane//far
//				);
//
//			HeadCam.projectionMatrix = genProjection;  
//		}
//
//	}
//
//
//	public void TrackHead(){
//
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
//		                                    near*(-.5f * screenAspect + mHeadX)/(mHeadDist),//left,  
//		                                    near*(.5f * screenAspect + mHeadX)/(mHeadDist),//right
//		                                    near*(-.5f - mHeadY)/(mHeadDist),//bottom
//		                                    near*(.5f - mHeadY)/(mHeadDist),//top
//		                                    near,//near
//		                                    100//far
//		                                    
//		                                    /*//alternate real world values
//		                                    near*(-.5f * mScreenWidthInMM - mHeadX)/mHeadDist,//left,  
//		                                    near*(.5f * mScreenWidthInMM - mHeadX)/mHeadDist,//right
//		                                    near*(-.5f * mScreenHeightInMM - mHeadY)/mHeadDist,//bottom
//		                                    near*(.5f * mScreenHeightInMM - mHeadY)/mHeadDist,//top
//		                                    near,//near
//		                                    100//far  */
//		                                    );
//		
//			HeadCam.projectionMatrix = m;
//		}
//
//		switch(LookAtState){
//
//			case (LookAtMatrixSide.LH): {
//				HeadCam.worldToCameraMatrix = LookAtMatrixLH(new Vector3(mHeadX, mHeadY, -mHeadDist),new Vector3(mHeadX, mHeadY,0),new Vector3(0,1,0));
//				break;		
//			}
//			case(LookAtMatrixSide.RH): {
//				HeadCam.worldToCameraMatrix = LookAtMatrixRH(new Vector3(mHeadX, mHeadY, -mHeadDist),new Vector3(mHeadX, mHeadY,0),new Vector3(0,1,0));
//				break;		
//			}
//			case(LookAtMatrixSide.OFF): {
//				HeadCam.ResetWorldToCameraMatrix();
//				break;		
//			}
//		}
//
//		if(camMove){
//			HeadCam.transform.localPosition = new Vector3 (0, 0, -mHeadDist);
//		}
//		//Vector3 lookAt = new Vector3(0 , 0 , -HeadCam.transform.position.z);//lookat point, currently being fakes      
//		//lookAtObject.localPosition = lookAt;
//
//
//		if (lookatswitch){//makes head cam look at lookat point when switched on
//			HeadCam.transform.LookAt (lookAtObject);
//		}
//
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
//		//D3DXMatrix values
////		float x =  (2.0f * near) / (right - left);
////		float y =  (2.0f * near) / (top - bottom);
////		float a =  (left + right) / (left - right);
////		float b =  (top + bottom) / (bottom-top);
////		float c =  (far) / (far - near);
////		float d =  (far * near) / (near - far);
////		float e =  1.0f;
//		
//		
//		Matrix4x4 m = new Matrix4x4 ();
//
//		//orig from Unity
////		m[0,0] = x;
////		m[0,1] = 0;
////		m[0,2] = a;
////		m[0,3] = 0;
////		m[1,0] = 0;
////		m[1,1] = y;
////		m[1,2] = b;
////		m[1,3] = 0;
////		m[2,0] = 0;
////		m[2,1] = 0;
////		m[2,2] = c;
////		m[2,3] = d;
////		m[3,0] = 0;
////		m[3,1] = 0;
////		m[3,2] = e;
////		m[3,3] = 0;
//
//		m[0,0] = x;
//		m[0,1] = 0;
//		m[0,2] = a;
//		m[0,3] = 0;
//					m[1,0] = 0;
//					m[1,1] = y;
//					m[1,2] = b;
//					m[1,3] = 0;
//								m[2,0] = 0;
//								m[2,1] = 0;
//								m[2,2] = c;
//								m[2,3] = d;
//											m[3,0] = 0;
//											m[3,1] = 0;
//											m[3,2] = e;
//											m[3,3] = 0;
//										
//		
//		return m;
//		
//	}
//
//	//CAVE Kooima Formula
//	public static Matrix4x4 GeneralizedPerspectiveProjection(Vector3 pa, Vector3 pb, Vector3 pc, Vector3 pe, float near, float far)
//
//	{
//		Vector3 va, vb, vc;
//		Vector3 vr, vu, vn;
//		
//		float left, right, bottom, top, eyedistance;
//		
//		Matrix4x4 transformMatrix;
//		Matrix4x4 projectionM;
//		Matrix4x4 eyeTranslateM;
//		Matrix4x4 finalProjection;
//		
//		///Calculate the orthonormal for the screen (the screen coordinate system
//		vr = pb - pa;
//		vr.Normalize();
//		vu = pc - pa;
//		vu.Normalize();
//		vn = Vector3.Cross(vr, vu);
//		vn.Normalize();
//		
//		//Calculate the vector from eye (pe) to screen corners (pa, pb, pc)
//		va = pa-pe;
//		vb = pb-pe;
//		vc = pc-pe;
//		
//		//Get the distance;; from the eye to the screen plane
//		eyedistance = -(Vector3.Dot(va, vn));
//		
//		//Get the varaibles for the off center projection
//		left = (Vector3.Dot(vr, va)*near)/eyedistance;
//		right  = (Vector3.Dot(vr, vb)*near)/eyedistance;
//		bottom  = (Vector3.Dot(vu, va)*near)/eyedistance;
//		top = (Vector3.Dot(vu, vc)*near)/eyedistance;
//		
//		//Get this projection
//		projectionM = PerspectiveOffCenter(left, right, bottom, top, near, far);
//		
//		//Fill in the transform matrix
//		transformMatrix = new Matrix4x4();
//		transformMatrix[0, 0] = vr.x;
//		transformMatrix[0, 1] = vr.y;
//		transformMatrix[0, 2] = vr.z;
//		transformMatrix[0, 3] = 0;
//		transformMatrix[1, 0] = vu.x;
//		transformMatrix[1, 1] = vu.y;
//		transformMatrix[1, 2] = vu.z;
//		transformMatrix[1, 3] = 0;
//		transformMatrix[2, 0] = vn.x;
//		transformMatrix[2, 1] = vn.y;
//		transformMatrix[2, 2] = vn.z;
//		transformMatrix[2, 3] = 0;
//		transformMatrix[3, 0] = 0;
//		transformMatrix[3, 1] = 0;
//		transformMatrix[3, 2] = 0;
//		transformMatrix[3, 3] = 1;
//		
//		//Now for the eye transform
//		eyeTranslateM = new Matrix4x4();
//		eyeTranslateM[0, 0] = 1;
//		eyeTranslateM[0, 1] = 0;
//		eyeTranslateM[0, 2] = 0;
//		eyeTranslateM[0, 3] = -pe.x;
//		eyeTranslateM[1, 0] = 0;
//		eyeTranslateM[1, 1] = 1;
//		eyeTranslateM[1, 2] = 0;
//		eyeTranslateM[1, 3] = -pe.y;
//		eyeTranslateM[2, 0] = 0;
//		eyeTranslateM[2, 1] = 0;
//		eyeTranslateM[2, 2] = 1;
//		eyeTranslateM[2, 3] = -pe.z;
//		eyeTranslateM[3, 0] = 0;
//		eyeTranslateM[3, 1] = 0;
//		eyeTranslateM[3, 2] = 0;
//		eyeTranslateM[3, 3] = 1f;
//		
//		//Multiply all together
//		finalProjection = new Matrix4x4();
//		finalProjection = Matrix4x4.identity * projectionM*transformMatrix*eyeTranslateM;
//		
//		//finally return
//		return finalProjection;
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
//				m [1,0] = yaxis.x;
//				m [1,1] = yaxis.y;
//				m [1, 2] = yaxis.z;
//				m [1, 3] = -Vector3.Dot(yaxis, eye);
//
//						m [2, 0] = zaxis.x;
//						m [2, 1] = zaxis.y;
//						m [2, 2] = zaxis.z;
//						m [2, 3] = -Vector3.Dot(zaxis, eye);
//
//										m[3,0] = 0;
//										m[3,1] = 0;
//										m[3,2] = 0;
//										m[3,3] = 1;
//										
//		
//		return m;
//		
//	}
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
