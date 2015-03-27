// comment or uncomment the following #define directives
// depending on whether you use KinectExtras together with KinectManager

//#define USE_KINECT_MANAGER

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;


public class FacetrackingWrapper 
{
	public enum NuiImageResolution
	{
		resolutionInvalid = -1,
		resolution80x60 = 0,
		resolution320x240 = 1,
		resolution640x480 = 2,
		resolution1280x960 = 3     // for hires color only
	}
	
	public static class Constants
	{
		public const int ColorImageWidth = 640;
		public const int ColorImageHeight = 480;
		public const NuiImageResolution ColorImageResolution = NuiImageResolution.resolution640x480;
		
		public const int DepthImageWidth = 640;
		public const int DepthImageHeight = 480;
		public const NuiImageResolution DepthImageResolution = NuiImageResolution.resolution640x480;
		
		public const bool IsNearMode = true;
	}
	
	[Flags]
    public enum NuiInitializeFlags : uint
    {
		UsesAudio = 0x10000000,
        UsesDepthAndPlayerIndex = 0x00000001,
        UsesColor = 0x00000002,
        UsesSkeleton = 0x00000008,
        UsesDepth = 0x00000020,
		UsesHighQualityColor = 0x00000040
    }

	public struct FaceRect
	{
	    public int x;
	    public int y;
	    public int width;
	    public int height;
	}
	

#if USE_KINECT_MANAGER
	public static int InitKinectSensor(int iColorResolution, int iDepthResolution, bool bNearMode)
	{
		return 0;
	}
	
	public static void ShutdownKinectSensor()
	{
	}
#else
	[DllImport(@"KinectUnityWrapper", EntryPoint = "InitKinectSensor")]
	public static extern int InitKinectSensor(NuiInitializeFlags dwFlags, bool bEnableEvents, int iColorResolution, int iDepthResolution, bool bNearMode);

	[DllImport(@"KinectUnityWrapper", EntryPoint = "ShutdownKinectSensor")]
    public static extern void ShutdownKinectSensor();
	
	public static int InitKinectSensor(int iColorResolution, int iDepthResolution, bool bNearMode)
	{
		int hr = InitKinectSensor(NuiInitializeFlags.UsesColor|NuiInitializeFlags.UsesDepthAndPlayerIndex|NuiInitializeFlags.UsesSkeleton, true, iColorResolution, iDepthResolution, bNearMode);
		return hr;
	}
#endif
	
	// DLL Imports to pull in the necessary Unity functions to make the Kinect go.
	[DllImport("KinectUnityWrapper")]
	public static extern int InitFaceTracking();
	[DllImport("KinectUnityWrapper")]
	public static extern void FinishFaceTracking();
	[DllImport("KinectUnityWrapper")]
	public static extern int UpdateFaceTracking();
	
	[DllImport("KinectUnityWrapper")]
	public static extern bool IsFaceTracked();
	[DllImport("KinectUnityWrapper")]
	public static extern uint GetFaceTrackingID();
	[DllImport(@"KinectUnityWrapper")]
	public static extern uint GetSkeletonTrackingID();
	[DllImport(@"KinectUnityWrapper")]
	public static extern void SetSkeletonTrackingID(uint iPrimaryTrackingID);
	[DllImport(@"KinectUnityWrapper")]
	public static extern uint GetSkeletonTrackingID( uint player );
	[DllImport(@"KinectUnityWrapper")]
	public static extern int GetInteractorsCount();
	
	[DllImport("KinectUnityWrapper", EntryPoint = "GetAnimUnitsCount")]
	public static extern int GetAnimUnitsCountNative();

	[DllImport("KinectUnityWrapper", EntryPoint = "GetAnimUnits")]
	public static extern bool GetAnimUnitsNative(IntPtr afAU, ref int iAUCount);

//	[DllImport("KinectUnityWrapper")]
//	public static extern bool IsShapeConverged();

	[DllImport("KinectUnityWrapper", EntryPoint = "GetShapeUnitsCount")]
	public static extern int GetShapeUnitsCountNative();

	[DllImport("KinectUnityWrapper", EntryPoint = "GetShapeUnits")]
	public static extern bool GetShapeUnitsNative(IntPtr afSU, ref int iSUCount);

	[DllImport("KinectUnityWrapper", EntryPoint = "GetShapePointsCount")]
	public static extern int GetShapePointsCountNative();

	[DllImport("KinectUnityWrapper", EntryPoint = "GetShapePoints")]
	public static extern bool GetShapePointsNative(IntPtr avPoints, ref int iPointsCount);

	[DllImport("KinectUnityWrapper", EntryPoint = "Get3DShapePointsCount")]
	private static extern int GetModelPointsCountNative();
	
	[DllImport("KinectUnityWrapper", EntryPoint = "Get3DShapePoints")]
	private static extern bool GetModelPointsNative(IntPtr avPoints, ref int iPointsCount);
	
	[DllImport("KinectUnityWrapper", EntryPoint = "GetTriangleCount")]
	private static extern int GetTriangleCountNative();
	
	[DllImport("KinectUnityWrapper", EntryPoint = "GetTriangles")]
	private static extern bool GetTrianglesNative(IntPtr aiTriangles, ref int iPointsCount);
	

	[DllImport(@"KinectUnityWrapper.dll")]
	//public static extern bool GetColorFrameData([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.U1)] byte[] btVideoBuf, ref uint iVideoBufLen, bool bGetNewFrame);
	//public static extern bool GetColorFrameData([MarshalAs(UnmanagedType.LPArray, SizeConst = 640 * 480 * 4, ArraySubType = UnmanagedType.U1)] byte[] btVideoBuf, ref uint iVideoBufLen, bool bGetNewFrame);
	public static extern bool GetColorFrameData(IntPtr btVideoBuf, ref uint iVideoBufLen, bool bGetNewFrame);

	[DllImport(@"KinectUnityWrapper.dll")]
	public static extern bool GetHeadPosition(ref Vector4 pvHeadPos);

	[DllImport(@"KinectUnityWrapper.dll")]
	public static extern bool GetHeadRotation(ref Vector4 pvHeadRot);

	[DllImport(@"KinectUnityWrapper.dll")]
	public static extern bool GetHeadScale(ref Vector4 pvHeadScale);
	
	[DllImport(@"KinectUnityWrapper.dll")]
	public static extern bool GetFaceRect(ref FaceRect pRectFace);

	
	public static int GetImageWidth()
	{
		return Constants.ColorImageWidth;
	}
	
	public static int GetImageHeight()
	{
		return Constants.ColorImageHeight;
	}
	
	public static bool PollVideo(ref byte[] videoBuffer, ref Color32[] colorImage)
	{
		uint videoBufLen = (uint)videoBuffer.Length;

		var pVideoData = GCHandle.Alloc(videoBuffer, GCHandleType.Pinned);
		bool newColor = GetColorFrameData(pVideoData.AddrOfPinnedObject(), ref videoBufLen, true);
		pVideoData.Free();

		if (newColor)
		{
			int totalPixels = colorImage.Length;
			
			for (int pix = 0; pix < totalPixels; pix++)
			{
				int ind = totalPixels - pix - 1;
				int src = pix << 2;
				
				colorImage[ind].r = videoBuffer[src + 2]; // pixels[pix].r;
				colorImage[ind].g = videoBuffer[src + 1]; // pixels[pix].g;
				colorImage[ind].b = videoBuffer[src]; // pixels[pix].b;
				colorImage[ind].a = 255;
			}
		}
		
		return newColor;
	}
	
	
	public static int GetAnimUnitsCount()
	{
		return GetAnimUnitsCountNative();
	}
	
	public static bool GetAnimUnits(ref float[] afAU)
	{
		if(afAU != null)
		{
			int iArrayCount = afAU.Length;
			
			var pArrayData = GCHandle.Alloc(afAU, GCHandleType.Pinned);
			bool bSuccess = GetAnimUnitsNative(pArrayData.AddrOfPinnedObject(), ref iArrayCount);
			pArrayData.Free();
			
			return bSuccess;
		}
		
		return false;
	}
	
	public static int GetShapeUnitsCount()
	{
		return GetShapeUnitsCountNative();
	}
	
	public static bool GetShapeUnits(ref float[] afSU)
	{
		if(afSU != null)
		{
			int iArrayCount = afSU.Length;
			
			var pArrayData = GCHandle.Alloc(afSU, GCHandleType.Pinned);
			bool bSuccess = GetShapeUnitsNative(pArrayData.AddrOfPinnedObject(), ref iArrayCount);
			pArrayData.Free();
			
			return bSuccess;
		}
		
		return false;
	}
	
	public static int GetShapePointsCount()
	{
		return GetShapePointsCountNative();
	}
	
	public static bool GetShapePoints(ref Vector2[] avPoints)
	{
		if(avPoints != null)
		{
			int iArrayCount = avPoints.Length << 1;
			
			var pArrayData = GCHandle.Alloc(avPoints, GCHandleType.Pinned);
			bool bSuccess = GetShapePointsNative(pArrayData.AddrOfPinnedObject(), ref iArrayCount);
			pArrayData.Free();
			
			return bSuccess;
		}
		
		return false;
	}
	
	public static int GetFaceModelVerticesCount()
	{
		return GetModelPointsCountNative();
	}
	
	public static bool GetFaceModelVertices(ref Vector3[] avVertices)
	{
		if(avVertices != null)
		{
			int iPointsCount = avVertices.Length;
			
			var pArrayData = GCHandle.Alloc(avVertices, GCHandleType.Pinned);
			bool bSuccess = GetModelPointsNative(pArrayData.AddrOfPinnedObject(), ref iPointsCount);
			pArrayData.Free();

			return bSuccess;
		}
		
		return false;
	}
	
	public static int GetFaceModelTrianglesCount()
	{
		return GetTriangleCountNative();
	}
	
	public static bool GetFaceModelTriangles(bool bMirrored, ref int[] avTriangles)
	{
		if(avTriangles != null)
		{
			int iTriangleCount = avTriangles.Length;
			
			var pArrayData = GCHandle.Alloc(avTriangles, GCHandleType.Pinned);
			bool bSuccess = GetTrianglesNative(pArrayData.AddrOfPinnedObject(), ref iTriangleCount);
			pArrayData.Free();
			
			if(bMirrored)
			{
				Array.Reverse(avTriangles);
			}
			
			return bSuccess;
		}
		
		return false;
	}
	
}
