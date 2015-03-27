// comment or uncomment the following #define directives
// depending on whether you use KinectExtras together with KinectManager

//#define USE_KINECT_MANAGER

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System;
using System.IO;

public class BackgroundWrapper
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

		public const int DepthImageWidth = 320;
		public const int DepthImageHeight = 240;
		public const NuiImageResolution DepthImageResolution = NuiImageResolution.resolution320x240;

		public const bool IsNearMode = false;
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
	
	public enum NuiErrorCodes : uint
	{
		FrameNoData = 0x83010001,
		StreamNotEnabled = 0x83010002,
		ImageStreamInUse = 0x83010003,
		FrameLimitExceeded = 0x83010004,
		FeatureNotInitialized = 0x83010005,
		DeviceNotGenuine = 0x83010006,
		InsufficientBandwidth = 0x83010007,
		DeviceNotSupported = 0x83010008,
		DeviceInUse = 0x83010009,
		
		DatabaseNotFound = 0x8301000D,
		DatabaseVersionMismatch = 0x8301000E,
		HardwareFeatureUnavailable = 0x8301000F,
		
		DeviceNotConnected = 0x83010014,
		DeviceNotReady = 0x83010015,
		SkeletalEngineBusy = 0x830100AA,
		DeviceNotPowered = 0x8301027F,
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
	
	[DllImport(@"KinectUnityWrapper", EntryPoint = "InitBackgroundRemoval")]
	public static extern int InitBackgroundRemoval();

	[DllImport(@"KinectUnityWrapper", EntryPoint = "FinishBackgroundRemoval")]
	public static extern void FinishBackgroundRemoval();

	[DllImport(@"KinectUnityWrapper", EntryPoint = "UpdateBackgroundRemoval")]
	public static extern int UpdateBackgroundRemoval();
	

	[DllImport(@"KinectUnityWrapper", EntryPoint = "IsBackgroundRemovalActive")]
	public static extern bool IsBackgroundRemovalActive();

	[DllImport(@"KinectUnityWrapper", EntryPoint = "GetBackgroundRemovalFrameLength")]
	public static extern int GetBackgroundRemovalFrameLength();

	[DllImport(@"KinectUnityWrapper.dll", EntryPoint = "GetBackgroundRemovalFrameData")]
	public static extern bool GetBackgroundRemovalFrameData(IntPtr btVideoBuf, ref uint iVideoBufLen, bool bGetNewFrame);

	
	[DllImport(@"KinectUnityWrapper", EntryPoint = "GetSkeletonTrackingID")]
	public static extern uint GetSkeletonTrackingID();
	
	[DllImport(@"KinectUnityWrapper", EntryPoint = "SetSkeletonTrackingID")]
	public static extern void SetSkeletonTrackingID(uint iPrimaryTrackingID);
	
	[DllImport( @"KinectUnityWrapper", EntryPoint = "GetInteractorSkeletonTrackingID" )]
	public static extern uint GetSkeletonTrackingID( uint player );
	
	[DllImport( @"KinectUnityWrapper", EntryPoint = "GetInteractorsCount" )]
	public static extern int GetInteractorsCount();
	
	//////////////////////////////////////////////////////////////////////////


	// polls for new foreground dara
	public static bool PollForegroundData(ref byte[] videoBuffer)
	{
		uint videoBufLen = (uint)videoBuffer.Length;

		var pVideoData = GCHandle.Alloc(videoBuffer, GCHandleType.Pinned);
		bool newImage = GetBackgroundRemovalFrameData(pVideoData.AddrOfPinnedObject(), ref videoBufLen, true);
		pVideoData.Free();

		return newImage;
	}

	// get string description for the given hresult
	public static string GetNuiErrorString(int hr)
	{
		string message = string.Empty;
		uint uhr = (uint)hr;
		
		switch(uhr)
		{
			case (uint)NuiErrorCodes.FrameNoData:
				message = "Frame contains no data.";
				break;
			case (uint)NuiErrorCodes.StreamNotEnabled:
				message = "Stream is not enabled.";
				break;
			case (uint)NuiErrorCodes.ImageStreamInUse:
				message = "Image stream is already in use.";
				break;
			case (uint)NuiErrorCodes.FrameLimitExceeded:
				message = "Frame limit is exceeded.";
				break;
			case (uint)NuiErrorCodes.FeatureNotInitialized:
				message = "Feature is not initialized.";
				break;
			case (uint)NuiErrorCodes.DeviceNotGenuine:
				message = "Device is not genuine.";
				break;
			case (uint)NuiErrorCodes.InsufficientBandwidth:
				message = "Bandwidth is not sufficient.";
				break;
			case (uint)NuiErrorCodes.DeviceNotSupported:
				message = "Device is not supported (e.g. Kinect for XBox 360).";
				break;
			case (uint)NuiErrorCodes.DeviceInUse:
				message = "Device is already in use.";
				break;
			case (uint)NuiErrorCodes.DatabaseNotFound:
				message = "Database not found.";
				break;
			case (uint)NuiErrorCodes.DatabaseVersionMismatch:
				message = "Database version mismatch.";
				break;
			case (uint)NuiErrorCodes.HardwareFeatureUnavailable:
				message = "Hardware feature is not available.";
				break;
			case (uint)NuiErrorCodes.DeviceNotConnected:
				message = "Device is not connected.";
				break;
			case (uint)NuiErrorCodes.DeviceNotReady:
				message = "Device is not ready.";
				break;
			case (uint)NuiErrorCodes.SkeletalEngineBusy:
				message = "Skeletal engine is busy.";
				break;
			case (uint)NuiErrorCodes.DeviceNotPowered:
				message = "Device is not powered.";
				break;
				
			default:
				message = "hr=0x" + uhr.ToString("X");
				break;
		}
		
		return message;
	}
	
}

