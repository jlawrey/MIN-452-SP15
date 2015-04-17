using UnityEngine;
using System.Collections;

public class Z_KinectMouseClick : MonoBehaviour {

	private InteractionManager manager;
	public int level;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(manager == null)
		{
			manager = InteractionManager.Instance;
		}
	}

	void OnMouseOver(){

		if (manager.GetRightHandEvent() == InteractionWrapper.InteractionHandEventType.Grip) {
			manager = null;
			print ("PlayMode!!");
			Application.LoadLevel(level);
		}
	}
}
