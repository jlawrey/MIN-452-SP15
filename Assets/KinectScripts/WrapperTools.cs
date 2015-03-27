using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System;


public class WrapperTools
{
	// Copies the needed resources into the project directory
	public static bool EnsureKinectWrapperPresence()
	{
		bool bOneCopied = false, bAllCopied = true;
		
		if(!Is64bitArchitecture())
		{
			Debug.Log("x32-architecture detected.");
			string sTargetPath = GetTargetDllPath(".", false) + "/";
			
			CopyResourceFile(sTargetPath + "KinectUnityWrapper.dll", "KinectUnityWrapper.dll", ref bOneCopied, ref bAllCopied);
			CopyResourceFile(sTargetPath + "KinectInteraction180_32.dll", "KinectInteraction180_32.dll", ref bOneCopied, ref bAllCopied);
			CopyResourceFile(sTargetPath + "FaceTrackData.dll", "FaceTrackData.dll", ref bOneCopied, ref bAllCopied);
			CopyResourceFile(sTargetPath + "FaceTrackLib.dll", "FaceTrackLib.dll", ref bOneCopied, ref bAllCopied);
			CopyResourceFile(sTargetPath + "KinectBackgroundRemoval180_32.dll", "KinectBackgroundRemoval180_32.dll", ref bOneCopied, ref bAllCopied);
			
			CopyResourceFile(sTargetPath + "msvcp100d.dll", "msvcp100d.dll", ref bOneCopied, ref bAllCopied);
			CopyResourceFile(sTargetPath + "msvcr100d.dll", "msvcr100d.dll", ref bOneCopied, ref bAllCopied);
		}
		else
		{
			Debug.Log("x64-architecture detected.");
			string sTargetPath = GetTargetDllPath(".", true) + "/";
			
			CopyResourceFile(sTargetPath + "KinectUnityWrapper.dll", "KinectUnityWrapper.dll.x64", ref bOneCopied, ref bAllCopied);
			CopyResourceFile(sTargetPath + "KinectInteraction180_64.dll", "KinectInteraction180_64.dll.x64", ref bOneCopied, ref bAllCopied);
			CopyResourceFile(sTargetPath + "FaceTrackData.dll", "FaceTrackData.dll", ref bOneCopied, ref bAllCopied); // use the same data-dll
			CopyResourceFile(sTargetPath + "FaceTrackLib.dll", "FaceTrackLib.dll.x64", ref bOneCopied, ref bAllCopied);
			CopyResourceFile(sTargetPath + "KinectBackgroundRemoval180_64.dll", "KinectBackgroundRemoval180_64.dll.x64", ref bOneCopied, ref bAllCopied);
			
			CopyResourceFile(sTargetPath + "msvcp100d.dll", "msvcp100d.dll.x64", ref bOneCopied, ref bAllCopied);
			CopyResourceFile(sTargetPath + "msvcr100d.dll", "msvcr100d.dll.x64", ref bOneCopied, ref bAllCopied);
		}
		
		return (bOneCopied && bAllCopied);
	}
	

	// Copy a resource file to the target
	public static bool CopyResourceFile(string targetFilePath, string resFileName, ref bool bOneCopied, ref bool bAllCopied)
	{
		TextAsset textRes = Resources.Load(resFileName, typeof(TextAsset)) as TextAsset;
		if(textRes == null)
		{
			bOneCopied = false;
			bAllCopied = false;
			
			return false;
		}
		
		FileInfo targetFile = new FileInfo(targetFilePath);
		if(!targetFile.Directory.Exists)
		{
			targetFile.Directory.Create();
		}
		
		if(!targetFile.Exists || targetFile.Length !=  textRes.bytes.Length)
		{
			Debug.Log("Copying " + resFileName + "...");
			
			if(textRes != null)
			{
				using (FileStream fileStream = new FileStream (targetFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
				{
					fileStream.Write(textRes.bytes, 0, textRes.bytes.Length);
				}
				
				bool bFileCopied = File.Exists(targetFilePath);
				
				bOneCopied = bOneCopied || bFileCopied;
				bAllCopied = bAllCopied && bFileCopied;
				
				return bFileCopied;
			}
		}
		
		return false;
	}
	
	// returns true if the project is running on 64-bit architecture, false if 32-bit
	public static bool Is64bitArchitecture()
	{
		int sizeOfPtr = Marshal.SizeOf(typeof(IntPtr));
		return (sizeOfPtr > 4);
	}
	
	// returns the target dll path for the current platform (x86 or x64)
	public static string GetTargetDllPath(string sAppPath, bool bIs64bitApp)
	{
		string sTargetPath = sAppPath;
		string sPluginsPath = Application.dataPath + "/Plugins";
		
		if(Directory.Exists(sPluginsPath))
		{
			sTargetPath = sPluginsPath;
			
			if(Application.isEditor)
			{
				string sPlatformPath = sPluginsPath + "/" + (!bIs64bitApp ? "x86" : "x86_64");
				
				if(Directory.Exists(sPlatformPath))
				{
					sTargetPath = sPlatformPath;
				}
			}
		}
		
		return sTargetPath;
	}
	
	// cleans up objects and restarts the current level
	public static void RestartLevel(GameObject parentObject, string callerName)
	{
		Debug.Log(callerName + " is restarting level...");
		
		// destroy parent object if any
		if(parentObject)
		{
			GameObject.Destroy(parentObject);
		}
		
		// clean up memory assets
		Resources.UnloadUnusedAssets();
		GC.Collect();
		
		// reload the same level
		Application.LoadLevel(Application.loadedLevel);
	}
	
}
