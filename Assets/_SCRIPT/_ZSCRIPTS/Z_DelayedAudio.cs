using UnityEngine;
using System.Collections;

public class Z_DelayedAudio : MonoBehaviour {

	public AudioClip[] clips;
	public bool[] audioLoop;
	public float[] delay;

	// Use this for initialization
	void Start () {

		StartCoroutine (PlayClips ());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public IEnumerator PlayClips(){

		for (int i=0; i<clips.Length;i++) {
			yield return new WaitForSeconds (delay[i]); 
			audio.PlayOneShot(clips[i]);
				
		}

	}
}
