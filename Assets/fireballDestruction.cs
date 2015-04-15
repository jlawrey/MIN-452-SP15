using UnityEngine;
using System.Collections;

public class fireballDestruction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider target)
	{
		if(target.tag == "Player" || target.tag == "Miss")
		{
			Destroy(gameObject, 0.5f);
		}
		
		
	}

}
