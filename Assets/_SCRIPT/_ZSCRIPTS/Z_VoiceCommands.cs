using UnityEngine;
using System.Collections;

public class Z_VoiceCommands : MonoBehaviour {


	private SpeechManager speechManager;
	private bool initSpeech;

	// Use this for initialization
	void Start () {

		initSpeech = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	

		if(speechManager == null && initSpeech){
			speechManager = SpeechManager.Instance;
		}
		if(speechManager != null && speechManager.IsSapiInitialized()){
			if(speechManager.IsPhraseRecognized()){
				string sPhraseTag = speechManager.GetPhraseTagRecognized();
				switch(sPhraseTag){
				
				case "PLAY":
					initSpeech = false;
					Z_LoadInterlude.to_scene = "MMM_Level_01";
					print ("Playrecognized");
					Application.LoadLevel("Loading");
					Destroy(this.gameObject);
					break;
				
				case "OPTIONS":
					initSpeech = false;
					Z_LoadInterlude.to_scene = "Options";
					Application.LoadLevel("Loading");
					Destroy (this.gameObject);
					break;

				case "INSTRUCTIONS":
					initSpeech = false;
					Z_LoadInterlude.to_scene = "Instructions";
					Application.LoadLevel("Loading");
					Destroy (this.gameObject);
					break;

				case "MAIN":

					initSpeech = false;
					Z_LoadInterlude.to_scene = "MainMenuAlt";
					Application.LoadLevel("Loading");
					Destroy (this.gameObject);
					break;
				}

				
				speechManager.ClearPhraseRecognized();
			}
			
		}
	}
}
