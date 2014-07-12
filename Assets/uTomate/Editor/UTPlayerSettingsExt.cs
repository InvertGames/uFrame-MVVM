//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UObject = UnityEngine.Object;
using UnityEngine;

/// <summary>
///  Extension class for accessing unexposed properties of the PlayerSettings
/// </summary>
public class UTPlayerSettingsExt
{
	private SerializedObject serializedObject;
	private SerializedProperty iPhoneSplashScreen;
	private SerializedProperty iPhoneHighResSplashScreen;
	private SerializedProperty iPhoneTallHighResSplashScreen;
	private SerializedProperty iPadPortraitSplashScreen;
	private SerializedProperty iPadHighResPortraitSplashScreen;
	private SerializedProperty iPadLandscapeSplashScreen;
	private SerializedProperty iPadHighResLandscapeSplashScreen;
	
	public UTPlayerSettingsExt ()
	{
		serializedObject = new SerializedObject (Unsupported.GetSerializedAssetInterfaceSingleton ("PlayerSettings"));
		
		iPhoneSplashScreen = FindPropertyAssert ("iPhoneSplashScreen");
		iPhoneHighResSplashScreen = FindPropertyAssert ("iPhoneHighResSplashScreen");
		iPhoneTallHighResSplashScreen = FindPropertyAssert ("iPhoneTallHighResSplashScreen");
		iPadPortraitSplashScreen = FindPropertyAssert ("iPadPortraitSplashScreen");
		iPadHighResPortraitSplashScreen = FindPropertyAssert ("iPadHighResPortraitSplashScreen");
		iPadLandscapeSplashScreen = FindPropertyAssert ("iPadLandscapeSplashScreen");
		iPadHighResLandscapeSplashScreen = FindPropertyAssert ("iPadHighResLandscapeSplashScreen");
	}
	
	public void Apply() {
		serializedObject.ApplyModifiedProperties();
	}
	
	public Texture2D MobileSplashScreen {
		get { return (Texture2D) iPhoneSplashScreen.objectReferenceValue; }
		set { iPhoneSplashScreen.objectReferenceValue = value; }
	}

	public Texture2D IPhoneHighResSplashScreen { 
		get { return (Texture2D) iPhoneHighResSplashScreen.objectReferenceValue; } 
		set { iPhoneHighResSplashScreen.objectReferenceValue = value; } 
	}

	public Texture2D IPhoneTallHighResSplashScreen { 
		get { return (Texture2D) iPhoneTallHighResSplashScreen.objectReferenceValue; } 
		set { iPhoneTallHighResSplashScreen.objectReferenceValue = value; }
	}

	public Texture2D IPadPortraitSplashScreen { 
		get { return (Texture2D) iPadPortraitSplashScreen.objectReferenceValue; } 
		set { iPadPortraitSplashScreen.objectReferenceValue = value; } 
	}

	public Texture2D IPadHighResPortraitSplashScreen { 
		get { return (Texture2D) iPadHighResPortraitSplashScreen.objectReferenceValue; } 
		set { iPadHighResPortraitSplashScreen.objectReferenceValue = value; } 
	}

	public Texture2D IPadLandscapeSplashScreen { 
		get { return (Texture2D) iPadLandscapeSplashScreen.objectReferenceValue; } 
		set { iPadLandscapeSplashScreen.objectReferenceValue = value; } 
	}

	public Texture2D IPadHighResLandscapeSplashScreen { 
		get { return (Texture2D) iPadHighResLandscapeSplashScreen.objectReferenceValue; } 
		set { iPadHighResLandscapeSplashScreen.objectReferenceValue = value; } 
	}
	
	private SerializedProperty FindPropertyAssert (string name)
	{
		SerializedProperty property = serializedObject.FindProperty (name);
		if (property == null) {
			Debug.LogError ((object)("Failed to find:" + name));
		}
		return property;
	}
}

