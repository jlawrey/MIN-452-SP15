using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class FacetrackingManager : MonoBehaviour 
{
	// Public bool to determine whether the head rotation and movement should be mirrored or normal
	public bool mirroredHeadMovement = true;
	
	// Public Bool to determine whether to receive and compute the color map
	public bool computeColorMap = false;
	
	// Public Bool to determine whether to display color map on the GUI
	public bool displayColorMap = false;
	
	// Tolerance (in seconds) allowed to miss the tracked face before losing it
	public float faceTrackingTolerance = 0.5f;
	
	// Public Bool to determine whether to display face rectangle on the GUI
	public bool displayFaceRect = false;
	
	// Public Bool to determine whether to visualize facetracker lines on the GUI
	public bool visualizeFacetracker = false;
	
	// GUI Text to show messages.
	public GameObject debugText;

	// Is currently tracking
	private bool isTrackingFace = false;
	
	// Skeleton ID of the tracked face
	private uint faceTrackingID = 0;

	// last time when a face was tracked
	private float lastFaceTrackedTime = 0f;
	
	// Are shape units converged
	//private bool isConverged = false;
	
	// Animation units
	private float[] afAU = null;
	private bool bGotAU = false;

	// Shape units
	private float[] afSU = null;
	private bool bGotSU = false;
	
	// Shape points to visualize (2d points)
	private Vector2[] avShapePoints = null;
	private bool bGotShapePoints = false;

	// Vertices of the 3d face model
	private Vector3[] avFaceModelPoints = null;
	private int[] avFaceModelTriangles = null;
	private bool bGotFaceModelPoints = false;

	// Head position and rotation
	private Vector3 headPos = Vector3.zero;
	private Quaternion headRot = Quaternion.identity;
	
	// Tracked face rectangle
	private FacetrackingWrapper.FaceRect faceRect;
	
	// Bool to keep track of whether Kinect and FT-library have been initialized
	private bool facetrackingInitialized = false;
	
	// The single instance of FacetrackingManager
	private static FacetrackingManager instance;
	
	// Color image data, if used
	private Texture2D usersClrTex;
	private Rect usersClrRect;
	private Color32[] colorImage;
	private byte[] videoBuffer;
	

	// returns the single FacetrackingManager instance
    public static FacetrackingManager Instance
    {
        get
        {
            return instance;
        }
    }
	
	// returns true if SAPI is successfully initialized, false otherwise
	public bool IsFacetrackingInitialized()
	{
		return facetrackingInitialized;
	}
	
	// returns true if the facetracking library is tracking a face at the moment
	public bool IsTrackingFace()
	{
		return isTrackingFace;
	}

	// returns the skeleton ID of the tracked face, or 0 if no skeleton was associated with the face
	public uint GetFaceTrackingID()
	{
		return faceTrackingID;
	}
	
	// returns the user ID (primary skeleton ID), or 0 if no user is currently tracked
	public uint GetUserID()
	{
		return FacetrackingWrapper.GetSkeletonTrackingID();
	}
	
	// sets new user ID (primary skeleton ID) to be used by the native wrapper
	public void SetUserId(uint userId)
	{
		FacetrackingWrapper.SetSkeletonTrackingID(userId);
	}
	
	// returns the number of Kinect users
	public int GetUsersCount()
	{
		return FacetrackingWrapper.GetInteractorsCount();
	}
	
	// returns the user ID at the given index 
	// the index must be between 0 and (usersCount - 1)
	public uint GetUserIdAt(int index)
	{
		return FacetrackingWrapper.GetSkeletonTrackingID((uint)index);
	}
	

	// returns the color image texture,if ComputeColorMap is true
    public Texture2D GetUsersClrTex()
    { 
		return usersClrTex;
	}
	
	// returns the tracked head position
	public Vector3 GetHeadPosition()
	{
		return headPos;
	}
	
	// returns the tracked head rotation
	public Quaternion GetHeadRotation()
	{
		return headRot;
	}
	
	// returns true if there are Anim Units got
	public bool IsGotAU()
	{
		return bGotAU;
	}
	
	// returns animation units count, or 0 if no face has been tracked
	public int GetAnimUnitsCount()
	{
		if(afAU != null)
		{
			return afAU.Length;
		}
		
		return 0;
	}

	// returns the animation unit at given index, or 0 if the index is invalid
	public float GetAnimUnit(int index)
	{
		if(afAU != null && index >= 0 && index < afAU.Length)
		{
			return afAU[index];
		}
		
		return 0.0f;
	}
	
	// returns true if there are Shape Units got
	public bool IsGotSU()
	{
		return bGotSU;
	}
	
	// returns shape units count, or 0 if no face has been tracked
	public int GetShapeUnitsCount()
	{
		if(afSU != null)
		{
			return afSU.Length;
		}
		
		return 0;
	}
	
	// returns the shape unit at given index, or 0 if the index is invalid
	public float GetShapeUnit(int index)
	{
		if(afSU != null && index >= 0 && index < afSU.Length)
		{
			return afSU[index];
		}
		
		return 0.0f;
	}
	
//	// returns true if shape is converged, false otherwise
//	public bool IsShapeConverged()
//	{
//		return isConverged;
//	}

	// returns face model vertex point of the tracked face, or Vector3.zero if no face is being tracked
	public Vector3 GetFaceModelVertex(int index)
	{
		if (bGotFaceModelPoints) 
		{
			if(index >= 0 && index < avFaceModelPoints.Length)
			{
				return avFaceModelPoints[index];
			}
		}
		
		return Vector3.zero;
	}
	
	// returns the count of face model vertices. returns true if a face is being tracked, false otherwise
	public int GetFaceModelVerticesCount()
	{
		if (bGotFaceModelPoints) 
		{
			return avFaceModelPoints.Length;
		}

		return 0;
	}

	// returns the face model vertices of the tracked face. returns true if a face is being tracked, false otherwise
	public Vector3[] GetFaceModelVertices()
	{
		if (bGotFaceModelPoints) 
		{
			return avFaceModelPoints;
		}

		return null;
	}

	// returns the count of face model triangles. returns true if a face is being tracked, false otherwise
	public int GetFaceModelTrianglesCount()
	{
		if (bGotFaceModelPoints) 
		{
			return avFaceModelTriangles.Length;
		}
		
		return 0;
	}
	
	// returns the face model triangles of the tracked face. returns true if a face is being tracked, false otherwise
	public int[] GetFaceModelTriangles()
	{
		if (bGotFaceModelPoints) 
		{
			return avFaceModelTriangles;
		}
		
		return null;
	}
	
	//----------------------------------- end of public functions --------------------------------------//
	
	
	void Awake() 
	{
		// ensure the needed dlls are in place
		if(WrapperTools.EnsureKinectWrapperPresence())
		{
			// reload the same level
			WrapperTools.RestartLevel(gameObject, "FM");
		}
	}
	
	void StartFacetracker() 
	{
		try 
		{
			if(debugText != null)
				debugText.GetComponent<GUIText>().text = "Please, wait...";
			
			// initialize Kinect sensor as needed
			int rc = FacetrackingWrapper.InitKinectSensor((int)FacetrackingWrapper.Constants.ColorImageResolution, (int)FacetrackingWrapper.Constants.DepthImageResolution, FacetrackingWrapper.Constants.IsNearMode);
			if(rc != 0)
			{
				throw new Exception("Initialization of Kinect sensor failed");
			}
			
			// Initialize the kinect speech wrapper
			rc = FacetrackingWrapper.InitFaceTracking();
	        if (rc < 0)
	        {
	            throw new Exception(String.Format("Error initializing Kinect/FT: hr=0x{0:X}", rc));
	        }
			
			if(computeColorMap)
			{
				// Initialize color map related stuff
		        usersClrTex = new Texture2D(FacetrackingWrapper.GetImageWidth(), FacetrackingWrapper.GetImageHeight(), TextureFormat.ARGB32, false);
		        usersClrRect = new Rect(Screen.width, Screen.height - usersClrTex.height, -usersClrTex.width, usersClrTex.height);
				
				colorImage = new Color32[FacetrackingWrapper.GetImageWidth() * FacetrackingWrapper.GetImageHeight()];
				videoBuffer = new byte[FacetrackingWrapper.GetImageWidth() * FacetrackingWrapper.GetImageHeight() * 4];
			}
			
			instance = this;
			facetrackingInitialized = true;
			
			DontDestroyOnLoad(gameObject);

			if(debugText != null)
				debugText.GetComponent<GUIText>().text = "Ready.";
		} 
		catch(DllNotFoundException ex)
		{
			Debug.LogError(ex.ToString());
			if(debugText != null)
				debugText.GetComponent<GUIText>().text = "Please check the Kinect and FT-Library installations.";
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
		FacetrackingWrapper.FinishFaceTracking();
		FacetrackingWrapper.ShutdownKinectSensor();
		
		facetrackingInitialized = false;
		instance = null;
	}
	
	void Update() 
	{
		// start Kinect face tracker as needed
		if(!facetrackingInitialized)
		{
			StartFacetracker();
			
			if(!facetrackingInitialized)
			{
				Application.Quit();
				return;
			}
		}
		
		if(facetrackingInitialized)
		{
			// update the face tracker
			int rc = FacetrackingWrapper.UpdateFaceTracking();
			
			if(rc >= 0)
			{
				// poll the video frame as needed
				if(computeColorMap)
				{
					if(FacetrackingWrapper.PollVideo(ref videoBuffer, ref colorImage))
					{
				        usersClrTex.SetPixels32(colorImage);
				        usersClrTex.Apply();
					}
				}
				
				// estimate the tracking state
				isTrackingFace = FacetrackingWrapper.IsFaceTracked();
				faceTrackingID = FacetrackingWrapper.GetFaceTrackingID();

				// get the facetracking parameters
				if(isTrackingFace)
				{
					lastFaceTrackedTime = Time.realtimeSinceStartup;
					
					// get face rectangle
					bool bGotFaceRect = FacetrackingWrapper.GetFaceRect(ref faceRect);
					
					// get head position and rotation
					Vector4 vHeadPos = Vector4.zero, vHeadRot = Vector4.zero;
					if(FacetrackingWrapper.GetHeadPosition(ref vHeadPos))
					{
						headPos = (Vector3)vHeadPos;
						
						if(!mirroredHeadMovement)
						{
							headPos.z = -headPos.z;
						}
					}
					
					if(FacetrackingWrapper.GetHeadRotation(ref vHeadRot))
					{
						if(mirroredHeadMovement)
						{
							vHeadRot.x = -vHeadRot.x;
							vHeadRot.z = -vHeadRot.z;
						}
						else
						{
							vHeadRot.x = -vHeadRot.x;
							vHeadRot.y = -vHeadRot.y;
						}
						
						headRot = Quaternion.Euler((Vector3)vHeadRot);
					}
					
					// get the animation units
					bGotAU = false;
					int iNumAU = FacetrackingWrapper.GetAnimUnitsCount();

					if(iNumAU > 0)
					{
						if(afAU == null || afAU.Length == 0)
						{
							afAU = new float[iNumAU];
						}

						if(afAU != null && afAU.Length > 0)
						{
							bGotAU = FacetrackingWrapper.GetAnimUnits(ref afAU);
						}
					}
					
					// get the shape units
					//isConverged = FacetrackingWrapper.IsShapeConverged();

					bGotSU = false;
					int iNumSU = FacetrackingWrapper.GetShapeUnitsCount();

					if(iNumSU > 0)
					{
						if(afSU == null || afSU.Length == 0)
						{
							afSU = new float[iNumSU];
						}

						if(afSU != null && afSU.Length > 0)
						{
							bGotSU = FacetrackingWrapper.GetShapeUnits(ref afSU);
						}
					}
					
					// get the shape points
					bGotShapePoints = false;
					int iNumPoints = FacetrackingWrapper.GetShapePointsCount();

					if(iNumPoints > 0)
					{
						if(avShapePoints == null || avShapePoints.Length == 0)
						{
							avShapePoints = new Vector2[iNumPoints];
						}

						if(avShapePoints != null || avShapePoints.Length > 0)
						{
							bGotShapePoints = FacetrackingWrapper.GetShapePoints(ref avShapePoints);
						}
					}

					// get the 3D model points
					bGotFaceModelPoints = false;
					int iFaceModelVertexCount = FacetrackingWrapper.GetFaceModelVerticesCount();

					if(iFaceModelVertexCount > 0)
					{
						if(avFaceModelTriangles == null || avFaceModelTriangles.Length == 0)
						{
							int iFaceModelTrianglesCount = FacetrackingWrapper.GetFaceModelTrianglesCount();
							avFaceModelTriangles = new int[iFaceModelTrianglesCount];

							FacetrackingWrapper.GetFaceModelTriangles(mirroredHeadMovement, ref avFaceModelTriangles);
						}

						if(avFaceModelPoints == null || avFaceModelPoints.Length == 0)
						{
							avFaceModelPoints = new Vector3[iFaceModelVertexCount];
						}

						if(avFaceModelPoints != null && avFaceModelPoints.Length > 0)
						{
							bGotFaceModelPoints = FacetrackingWrapper.GetFaceModelVertices(ref avFaceModelPoints);
						}
					}

					if(computeColorMap)
					{
						if(displayFaceRect && bGotFaceRect)
						{
							DrawFacetrackerRect(usersClrTex, !visualizeFacetracker);
						}
						
						if(visualizeFacetracker && bGotShapePoints)
						{
							DrawFacetrackerLines(usersClrTex, avShapePoints, true);
						}
					}
				}
				else if((Time.realtimeSinceStartup - lastFaceTrackedTime) <= faceTrackingTolerance)
				{
					// allow tolerance in tracking
					isTrackingFace = true;
				}

			}
		}
	}
	
	void OnGUI()
	{
		if(facetrackingInitialized)
		{
			if(computeColorMap && displayColorMap)
			{
				GUI.DrawTexture(usersClrRect, usersClrTex);
			}
			
			if(debugText != null)
			{
				string sAuDebug = string.Empty;
				
//				// debug anim. units
//				sAuDebug = "AU: ";
//				int iNumAU = GetAnimUnitsCount();
//				
//				for(int i = 0; i < iNumAU; i++)
//				{
//					sAuDebug += String.Format("{0}:{1:F2} ", i, afAU[i]);
//				}

				if(isTrackingFace)
					debugText.GetComponent<GUIText>().text = "Tracking - skeletonID: " + faceTrackingID + " " + sAuDebug;
				else
					debugText.GetComponent<GUIText>().text = "Not tracking...";
			}
		}
	}
	
	// draws the lines of tracked face
	private void DrawFacetrackerLines(Texture2D aTexture, Vector2[] avPoints, bool bApplyTexture)
	{
		if(avPoints == null || avPoints.Length < 87)
			return;
		
		Color color = Color.yellow;
		
	    for (int ipt = 0; ipt < 8; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt+1)%8];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 8; ipt < 16; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt - 8 + 1) % 8 + 8];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 16; ipt < 26; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt - 16 + 1) % 10 + 16];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 26; ipt < 36; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt - 26 + 1) % 10 + 26];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 36; ipt < 47; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[ipt + 1];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 48; ipt < 60; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt - 48 + 1) % 12 + 48];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 60; ipt < 68; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt - 60 + 1) % 8 + 60];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 68; ipt < 86; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[ipt + 1];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
		
		if(bApplyTexture)
		{
			aTexture.Apply();
		}
	}
	
	// draws the bounding rectangle of tracked face
	private void DrawFacetrackerRect(Texture2D aTexture, bool bApplyTexture)
	{
		if(avShapePoints == null || avShapePoints.Length < 87)
			return;
		
		Color color = Color.magenta;
		Vector2 pt1, pt2;
		
		// bottom
		pt1.x = faceRect.x; pt1.y = faceRect.y;
		pt2.x = faceRect.x + faceRect.width - 1; pt2.y = pt1.y;
		DrawLine(aTexture, pt1, pt2, color);
		
		// right
		pt1.x = pt2.x; pt1.y = pt2.y;
		pt2.x = pt1.x; pt2.y = faceRect.y + faceRect.height - 1;
		DrawLine(aTexture, pt1, pt2, color);
		
		// top
		pt1.x = pt2.x; pt1.y = pt2.y;
		pt2.x = faceRect.x; pt2.y = pt1.y;
		DrawLine(aTexture, pt1, pt2, color);
		
		// left
		pt1.x = pt2.x; pt1.y = pt2.y;
		pt2.x = pt1.x; pt2.y = faceRect.y;
		DrawLine(aTexture, pt1, pt2, color);
		
		if(bApplyTexture)
		{
			aTexture.Apply();
		}
	}
	
	// draws a line in a texture
	private void DrawLine(Texture2D a_Texture, Vector2 ptStart, Vector2 ptEnd, Color a_Color)
	{
		int width = FacetrackingWrapper.Constants.ColorImageWidth;
		int height = FacetrackingWrapper.Constants.ColorImageHeight;
		
		DrawLine(a_Texture, width - (int)ptStart.x, height - (int)ptStart.y, 
					width - (int)ptEnd.x, height - (int)ptEnd.y, a_Color, width, height);
	}
	
	// draws a line in a texture
	private void DrawLine(Texture2D a_Texture, int x1, int y1, int x2, int y2, Color a_Color, int width, int height)
	{
		int dy = y2 - y1;
		int dx = x2 - x1;
	 
		int stepy = 1;
		if (dy < 0) 
		{
			dy = -dy; 
			stepy = -1;
		}
		
		int stepx = 1;
		if (dx < 0) 
		{
			dx = -dx; 
			stepx = -1;
		}
		
		dy <<= 1;
		dx <<= 1;
	 
		if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
			a_Texture.SetPixel(x1, y1, a_Color);
//			for(int x = -1; x <= 1; x++)
//				for(int y = -1; y <= 1; y++)
//					a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
		
		if (dx > dy) 
		{
			int fraction = dy - (dx >> 1);
			
			while (x1 != x2) 
			{
				if (fraction >= 0) 
				{
					y1 += stepy;
					fraction -= dx;
				}
				
				x1 += stepx;
				fraction += dy;
				
				if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
					a_Texture.SetPixel(x1, y1, a_Color);
//					for(int x = -1; x <= 1; x++)
//						for(int y = -1; y <= 1; y++)
//							a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
			}
		}
		else 
		{
			int fraction = dx - (dy >> 1);
			
			while (y1 != y2) 
			{
				if (fraction >= 0) 
				{
					x1 += stepx;
					fraction -= dy;
				}
				
				y1 += stepy;
				fraction += dx;
				
				if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
					a_Texture.SetPixel(x1, y1, a_Color);
//					for(int x = -1; x <= 1; x++)
//						for(int y = -1; y <= 1; y++)
//							a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
			}
		}
		
	}
	
	
}
