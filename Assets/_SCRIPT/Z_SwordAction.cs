using UnityEngine;
using System.Collections;
#if Wii
print("");
public class Z_SwordAction : MonoBehaviour {

	public Transform saber;
	public float mult = 1;
	public int WiiRemote = 1;
	public Vector3 MotionPlus = new Vector3();
	public Vector3 Accelerometer = new Vector3();
	public bool xOn;
	public bool yOn;
	public bool zOn;
	Vector3 sep = Vector3.zero;



	// Use this for initialization
	void Start () {

		Wii.CalibrateMotionPlus (WiiRemote);

		
	}
	
	// Update is called once per frame
	void Update () {

		print (Wii.IsMotionPlusCalibrated (WiiRemote));

		MotionPlus = Wii.GetMotionPlus (WiiRemote);
		Accelerometer = Wii.GetWiimoteAcceleration (WiiRemote);
		lSaber ();
	
	}

	public void lSaber(){


		if(xOn){
			sep.x = Accelerometer.z * mult;
		}
		if(yOn){
			sep.y = -Accelerometer.y * mult;
		}
		if(zOn){
			sep.z = -Accelerometer.x * mult;
		}

		Quaternion sepq = Quaternion.Euler (sep);
		saber.localRotation = sepq;
		//saber.Rotate (sep.y, sep.x, sep.z);

		//saber.localEulerAngles = new Vector3 (Wii.GetMotionPlus (1).x * mult, Wii.GetMotionPlus(1).y * mult, Wii.GetMotionPlus(1).z * mult);
	}
}
#endif
