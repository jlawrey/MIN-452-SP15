using UnityEngine;
using System.Collections;

public class Z_KinectMouseClick : MonoBehaviour {

	private InteractionManager manager;
	public int level;
	private bool kinect_be_on = true;

	// Use this for initialization
	void Start () {
	
		kinect_be_on = true;

	}
	
	// Update is called once per frame
	void Update () {

		if(kinect_be_on){
			if(manager == null){
				manager = InteractionManager.Instance;
			}
		}
	}

	void OnMouseOver(){

		if (manager.GetRightHandEvent() == InteractionWrapper.InteractionHandEventType.Grip && manager.IsInteractionInited()) {
			kinect_be_on = false;
			print ("PlayMode!!");
			Application.LoadLevel(level);
		}
	}
}
