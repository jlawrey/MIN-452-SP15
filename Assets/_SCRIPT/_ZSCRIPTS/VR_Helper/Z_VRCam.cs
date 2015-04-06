using UnityEngine;
using System.Collections;

public class Z_VRCam : MonoBehaviour {

	public Camera HeadCam;
	private float near = .05f;
	private float far = 1000;
	public float screenResHeight;
	public float screenResWidth;
	private float screenAspect;
	public float mHeadX;
	public float mHeadY;
	public float mHeadDist;
	private Vector3 mHeadPos;
	public Transform VRHead;
	private float offsetY = 0;
	private float offsetX = 0;
	private float offsetZ = 0;




	// Use this for initialization
	void Start () {

		screenAspect = screenResWidth / screenResHeight;
	}
	
	// Update is called once per frame
	void Update () {

		offsetX = HeadCam.transform.position.x;
		offsetY = HeadCam.transform.position.y;
		mHeadX = VRHead.position.x;
		mHeadY = VRHead.position.y ;
		mHeadDist = Mathf.Abs (VRHead.position.z);
		MatrixFOV ();//runs the projection matrix based on the x,y,z values
	
	}
	
	public void MatrixFOV(){
		HeadCam.ResetProjectionMatrix();
		Matrix4x4 m = PerspectiveOffCenter (//original values
		                                    near*(-.5f * screenAspect + mHeadX )/(mHeadDist),//left,  
		                                    near*(.5f * screenAspect + mHeadX )/(mHeadDist),//right
		                                    near*(-.5f - mHeadY )/(mHeadDist),//bottom
		                                    near*(.5f - mHeadY )/(mHeadDist),//top
		                                    near,//near
		                                    far//far 
		                                    );
		HeadCam.projectionMatrix = m;
		HeadCam.worldToCameraMatrix = LookAtMatrixRH(new Vector3(mHeadX, mHeadY, -mHeadDist),new Vector3(mHeadX, mHeadY ,0),new Vector3(0,1,0));

	}


	
	//off center projection matrix from Unity
	static Matrix4x4 PerspectiveOffCenter(
		float left, float right,
		float bottom, float top,
		float near, float far )
	{    
		//unity's projection matrix original
		float x =  (2.0f * near) / (right - left);
		float y =  (2.0f * near) / (top - bottom);
		float a =  (right + left) / (right - left);
		float b =  (top + bottom) / (top - bottom);
		float c = - (far + near) / (far - near);
		float d = -(2.0f * far * near )/ (far - near);
		float e = -1.0f;
		
		Matrix4x4 m = new Matrix4x4 ();
		
		
		m[0,0] = x;
		m[0,1] = 0;
		m[0,2] = a;
		m[0,3] = 0;
		m[1,0] = 0;
		m[1,1] = y;
		m[1,2] = b;
		m[1,3] = 0;
		m[2,0] = 0;
		m[2,1] = 0;
		m[2,2] = c;
		m[2,3] = d;
		m[3,0] = 0;
		m[3,1] = 0;
		m[3,2] = e;
		m[3,3] = 0;
		
		
		return m;
		
	}

	static Matrix4x4 LookAtMatrixRH(
		Vector3 eye, Vector3 at,
		Vector3 up)
	{    
		//unity's projection matrix original
		
		Vector3 zaxis = Vector3.Normalize (eye - at) ;
		Vector3 xaxis = -Vector3.Normalize(Vector3.Cross (up, zaxis));
		Vector3 yaxis = -Vector3.Cross(zaxis,xaxis);
		
		
		Matrix4x4 m = new Matrix4x4 ();
		
		
		
		m [0, 0] = xaxis.x;
		m [0, 1] = xaxis.y;
		m [0, 2] = xaxis.z;
		m [0, 3] = Vector3.Dot(xaxis, eye);
		
		m [1,0] = yaxis.x;
		m [1,1] = yaxis.y;
		m [1, 2] = yaxis.z;
		m [1, 3] = -Vector3.Dot(yaxis, eye);
		
		m [2, 0] = zaxis.x;
		m [2, 1] = zaxis.y;
		m [2, 2] = zaxis.z;
		m [2, 3] = -Vector3.Dot(zaxis, eye);
		
		m[3,0] = 0;
		m[3,1] = 0;
		m[3,2] = 0;
		m[3,3] = 1;
		
		return m;	
	}
}
