using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.IO;

public class BackgroundManager : MonoBehaviour 
{
	// whether to display the foreground images on the screen or not
	public bool displayForeground = true;

	// GUI-Text object to be used for displaying debug information
	public GameObject debugText;

	// buffer for the raw foreground image (width * height * 4 bytes)
	private byte[] foregroundImage;

	// the foreground texture
	private Texture2D foregroundTex;

	// rectangle taken by the foreground texture (in pixels)
	private Rect foregroundRect;
	
	// Bool to keep track whether Kinect and BackgroundRemoval library have been initialized
	private bool backgroundRemovalInited = false;
	
	// The single instance of BackgroundManager
	private static BackgroundManager instance;
	
	

	// returns the single BackgroundManager instance
	public static BackgroundManager Instance
    {
        get
        {
            return instance;
        }
    }
	
	// returns true if the BackgroundRemoval library is initialized, otherwise returns false
	public bool IsBackgroundRemovalInited()
	{
		return backgroundRemovalInited;
	}


	// returns the width of the foreground image
	public int GetForegroundWidth()
	{
		return BackgroundWrapper.Constants.ColorImageWidth;
	}

	// returns the width of the foreground image
	public int GetForegroundHeight()
	{
		return BackgroundWrapper.Constants.ColorImageHeight;
	}
	
	// returns the raw foreground image
	public byte[] GetRawForegroundImage()
	{
		return foregroundImage;
	}

	// returns the foreground image texture
	public Texture2D GetForegroundTex()
	{ 
		return foregroundTex;
	}


	// returns the user ID (primary skeleton ID), or 0 if no user is currently tracked
	public uint GetUserID()
	{
		return BackgroundWrapper.GetSkeletonTrackingID();
	}
	
	// sets new user ID (primary skeleton ID) to be used by the native wrapper
	public void SetUserId(uint userId)
	{
		BackgroundWrapper.SetSkeletonTrackingID(userId);
	}
	
	// returns the number of Kinect users
	public int GetUsersCount()
	{
		return BackgroundWrapper.GetInteractorsCount();
	}
	
	// returns the user ID at the given index 
	// the index must be between 0 and (usersCount - 1)
	public uint GetUserIdAt(int index)
	{
		return BackgroundWrapper.GetSkeletonTrackingID((uint)index);
	}
	

	//----------------------------------- end of public functions --------------------------------------//
	
	void Awake() 
	{
		// ensure the needed dlls are in place
		if(WrapperTools.EnsureKinectWrapperPresence())
		{
			// reload the same level
			WrapperTools.RestartLevel(gameObject, "BM");
		}
	}
	

	void StartBackgroundRemoval() 
	{
		int hr = 0;
		
		try 
		{
			// initialize Kinect sensor as needed
			hr = BackgroundWrapper.InitKinectSensor((int)BackgroundWrapper.Constants.ColorImageResolution, (int)BackgroundWrapper.Constants.DepthImageResolution, BackgroundWrapper.Constants.IsNearMode);
			if(hr != 0)
			{
				throw new Exception("Initialization of Kinect sensor failed");
			}
			
			// initialize Kinect background removal
			hr = BackgroundWrapper.InitBackgroundRemoval();
			if(hr != 0)
			{
				throw new Exception("Initialization of BackgroundRemoval failed");
			}
			
			// Initialize the foreground buffer and texture
			foregroundTex = new Texture2D(BackgroundWrapper.Constants.ColorImageWidth, BackgroundWrapper.Constants.ColorImageHeight, TextureFormat.RGBA32, false);

			Rect cameraRect = Camera.main.pixelRect;
			float rectHeight = cameraRect.height;
			float rectWidth = cameraRect.width;

			if(rectWidth > rectHeight)
				rectWidth = rectHeight * BackgroundWrapper.Constants.ColorImageWidth / BackgroundWrapper.Constants.ColorImageHeight;
			else
				rectHeight = rectWidth * BackgroundWrapper.Constants.ColorImageHeight / BackgroundWrapper.Constants.ColorImageWidth;

			foregroundRect = new Rect((cameraRect.width - rectWidth) / 2, cameraRect.height - (cameraRect.height - rectHeight) / 2, rectWidth, -rectHeight);

			int imageLength = BackgroundWrapper.Constants.ColorImageWidth * BackgroundWrapper.Constants.ColorImageHeight * 4;
			foregroundImage = new byte[imageLength];

			// kinect background removal was successfully initialized
			instance = this;
			backgroundRemovalInited = true;
		} 
		catch(DllNotFoundException ex)
		{
			Debug.LogError(ex.ToString());
			if(debugText != null)
				debugText.GetComponent<GUIText>().text = "Please check the Kinect SDK installation.";
		}
		catch (Exception ex) 
		{
			string message = ex.Message + " - " + BackgroundWrapper.GetNuiErrorString(hr);
			Debug.LogError(ex.ToString());
			
			if(debugText != null)
			{
				debugText.GetComponent<GUIText>().text = message;
			}
				
			return;
		}
		
		// don't destroy the object on loading levels
		DontDestroyOnLoad(gameObject);
	}
	
	void OnApplicationQuit()
	{
		// finish background removal
		if(backgroundRemovalInited)
		{
			BackgroundWrapper.FinishBackgroundRemoval();
			BackgroundWrapper.ShutdownKinectSensor();
			
			backgroundRemovalInited = false;
			instance = null;
		}
	}
	
	void Update () 
	{
		// start KinectBackgroundRemoval as needed
		if(!backgroundRemovalInited)
		{
			StartBackgroundRemoval();
			
			if(!backgroundRemovalInited)
			{
				Application.Quit();
				return;
			}
		}
		
		// update KinectBackgroundRemoval
		if(BackgroundWrapper.UpdateBackgroundRemoval() == 0)
		{
			if(BackgroundWrapper.PollForegroundData(ref foregroundImage))
			{
				foregroundTex.LoadRawTextureData(foregroundImage);
				foregroundTex.Apply ();
			}
		}

		if(debugText && debugText.GetComponent<GUIText>())
		{
			uint userId = GetUserID();
			string sDebug = userId != 0 ? "Tracked user ID: " + userId : string.Empty;

			if(debugText.GetComponent<GUIText>().text != sDebug)
			{
				debugText.GetComponent<GUIText>().text = sDebug;
			}
		}
		
	}
	
	void OnGUI()
	{
		if(displayForeground && foregroundTex)
		{
			GUI.DrawTexture(foregroundRect, foregroundTex);
		}
	}
	
}
