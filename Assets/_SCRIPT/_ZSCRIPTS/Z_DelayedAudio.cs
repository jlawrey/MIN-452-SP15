using UnityEngine;
using System.Collections;

public class Z_DelayedAudio : MonoBehaviour {

	public AudioClip[] clips;
	public bool[] audioLoop;
	public float[] delay;
	public bool usingFirstTime = true;
	public bool isFirstTime = true;

	// Use this for initialization
	void Start () {

		StartCoroutine (PlayClips ());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public IEnumerator PlayClips(){

		if (usingFirstTime && isFirstTime){
			for (int i=0; i<clips.Length;i++) {
				yield return new WaitForSeconds (delay[i]); 
				audio.PlayOneShot(clips[i]);
				isFirstTime = false;	
			}
		}else if(!usingFirstTime){
			for (int i=0; i<clips.Length;i++) {
				yield return new WaitForSeconds (delay[i]); 
				audio.PlayOneShot(clips[i]);
			}
		}

	}
}
