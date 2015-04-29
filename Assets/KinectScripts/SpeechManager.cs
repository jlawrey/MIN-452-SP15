using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;


public class SpeechManager : MonoBehaviour 
{
	// Grammar XML file name
	public string grammarFileName = "SpeechGrammar.grxml";
	
	// Grammar language (English by default)
	public int languageCode = 1033;
	
	// Required confidence
	public float requiredConfidence = 0.0f;
	
	// GUI Text to show messages.
	public GameObject debugText;

	// Is currently listening
	private bool isListening;
	
	// Current phrase recognized
	private bool isPhraseRecognized;
	private string phraseTagRecognized;

	// error handler when needed
	private SpeechErrorHandler errorHandler = null;
	
	// Bool to keep track of whether Kinect and SAPI have been initialized
	private bool sapiInitialized = false;
	
	// The single instance of SpeechManager
	private static SpeechManager instance;
	
	
	// returns the single SpeechManager instance
    public static SpeechManager Instance
    {
        get
        {
            return instance;
        }
    }
	
	// returns true if SAPI is successfully initialized, false otherwise
	public bool IsSapiInitialized()
	{
		return sapiInitialized;
	}
	
	// returns true if the speech recognizer is listening at the moment
	public bool IsListening()
	{
		return isListening;
	}
	
	// returns true if the speech recognizer has recognized a phrase
	public bool IsPhraseRecognized()
	{
		return isPhraseRecognized;
	}
	
	// returns the tag of the recognized phrase
	public string GetPhraseTagRecognized()
	{
		return phraseTagRecognized;
	}
	
	// clears the recognized phrase
	public void ClearPhraseRecognized()
	{
		isPhraseRecognized = false;
		phraseTagRecognized = String.Empty;
	}
	
	//----------------------------------- end of public functions --------------------------------------//
	
	void Awake() 
	{
		// ensure the needed dlls are in place
		if(WrapperTools.EnsureKinectWrapperPresence())
		{
			// reload the same level
			WrapperTools.RestartLevel(gameObject, "SM");
		}

		// copy the grammar file from the resources, if not found
		if((grammarFileName != "") && !File.Exists(grammarFileName))
		{
			TextAsset textRes = Resources.Load(grammarFileName, typeof(TextAsset)) as TextAsset;
			
			if(textRes != null)
			{
				string sResText = textRes.text;
				File.WriteAllText(grammarFileName, sResText);
			}
		}
	}
	
	void StartRecognizer() 
	{
		try 
		{
			if(errorHandler == null && debugText != null)
			{
				debugText.GetComponent<GUIText>().text = "Please, wait...";
			}
			
			// initialize Kinect sensor as needed
			int rc = SpeechWrapper.InitKinectSensor();
			if(rc != 0)
			{
				throw new Exception("Initialization of Kinect sensor failed");
			}
			
			// Initialize the kinect speech wrapper
			string sCriteria = String.Format("Language={0:X};Kinect=True", languageCode);
			rc = SpeechWrapper.InitSpeechRecognizer(sCriteria, true, false);

	        if (rc < 0 && errorHandler == null)
	        {
				errorHandler = new SpeechErrorHandler();
				string sErrorMessage = errorHandler.GetSapiErrorMessage(rc);

				throw new Exception(String.Format("Error initializing Kinect/SAPI: " + sErrorMessage));
	        }
			
			if(requiredConfidence > 0)
			{
				SpeechWrapper.SetRequiredConfidence(requiredConfidence);
			}
			
			if(grammarFileName != string.Empty)
			{
				rc = SpeechWrapper.LoadSpeechGrammar(grammarFileName, (short)languageCode);

				if (rc < 0 && errorHandler == null)
		        {
					errorHandler = new SpeechErrorHandler();
					string sErrorMessage = errorHandler.GetSapiErrorMessage(rc);

					throw new Exception("Error loading grammar file " + grammarFileName + ": " + sErrorMessage);
		        }
			}
			
			instance = this;
			sapiInitialized = true;
			
			DontDestroyOnLoad(gameObject);

			if(errorHandler == null && debugText != null)
			{
				debugText.GetComponent<GUIText>().text = "Ready.";
			}
		} 
		catch(DllNotFoundException ex)
		{
			Debug.LogError(ex.ToString());
			if(debugText != null)
				debugText.GetComponent<GUIText>().text = "Please check the Kinect and SAPI installations.";
		}
		catch (Exception ex) 
		{
			Debug.LogError(ex.ToString());
			if(debugText != null)
				debugText.GetComponent<GUIText>().text = ex.Message;
		}
	}

	// Make sure to kill the Kinect on quitting.
	void OnApplicationQuit()
	{
		// Shutdown Speech Recognizer and Kinect
		SpeechWrapper.FinishSpeechRecognizer();
		SpeechWrapper.ShutdownKinectSensor();
		
		sapiInitialized = false;
		instance = null;
	}
	
	void Update () 
	{
		// start Kinect speech recognizer as needed
		if(!sapiInitialized)
		{
			StartRecognizer();
			
			if(!sapiInitialized)
			{
				Application.Quit();
				return;
			}
		}
		
		if(sapiInitialized)
		{
			// update the speech recognizer
			int rc = SpeechWrapper.UpdateSpeechRecognizer();
			
			if(rc >= 0)
			{
				// estimate the listening state
				if(SpeechWrapper.IsSoundStarted())
				{
					isListening = true;
				}
				else if(SpeechWrapper.IsSoundEnded())
				{
					isListening = false;
				}

				// check if a grammar phrase has been recognized
				if(SpeechWrapper.IsPhraseRecognized())
				{
					isPhraseRecognized = true;
					
					IntPtr pPhraseTag = SpeechWrapper.GetRecognizedTag();
					phraseTagRecognized = Marshal.PtrToStringUni(pPhraseTag);
					
					SpeechWrapper.ClearPhraseRecognized();
					
					//Debug.Log(phraseTagRecognized);
				}
			}
		}
	}
	
	void OnGUI()
	{
		if(sapiInitialized)
		{
			if(debugText != null)
			{
				if(isPhraseRecognized)
					debugText.GetComponent<GUIText>().text = phraseTagRecognized;
				else if(isListening)
					debugText.GetComponent<GUIText>().text = "Listening...";
			}
		}
	}
	
	
}
