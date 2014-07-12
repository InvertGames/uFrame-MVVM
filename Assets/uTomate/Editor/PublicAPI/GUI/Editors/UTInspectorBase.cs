//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using UObject = UnityEngine.Object;

/// <summary>
/// Base class for inspectors in uTomate.
/// </summary>
public class UTInspectorBase  : Editor, UTInspectorRendererDelegate, UTFieldValidityChecker
{
	private UTInspectorRenderer renderer;
	private UTDefaultFieldValidityChecker defaultChecker = new UTDefaultFieldValidityChecker();
	
	public UTInspectorBase ()
	{
		renderer = new UTInspectorRenderer ();
		renderer.rendererDelegate = this;
		renderer.fieldValidityChecker = this;
	}
	
	public override void OnInspectorGUI ()
	{
		renderer.target = target;
		renderer.OnInspectorGUI ();
		
		DrawLoadDefaultSettingsUI ();
	}
	
	/// <summary>
	/// Draws the load default settings UI for actions implementing <see cref="UTICanLoadSettingsFromEditor"/>
	/// </summary>
	public virtual void DrawLoadDefaultSettingsUI ()
	{
		if (target is UTICanLoadSettingsFromEditor) {
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("For convenience, you can load the currently set editor settings or defaults into this action, using the button below.", MessageType.None);
			EditorGUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Load current settings")) {
				LoadCurrentSettings ();
			}
			EditorGUILayout.EndHorizontal ();			
		}		
	}
	
	/// <summary>
	/// Loads the current settings into the currently selected action if this action implements <see cref="UTICanLoadSettingsFromEditor"/>
	/// </summary>
	public virtual void LoadCurrentSettings ()
	{
		if (target is UTICanLoadSettingsFromEditor) {

			var action = (UTICanLoadSettingsFromEditor)target;

			CUUndoUtility.RegisterUndo (target, action.LoadSettingsUndoText);
			action.LoadSettings ();
			GUIUtility.keyboardControl = 0; // make sure every textfield is updated.
			EditorUtility.SetDirty (target);		
		}
	}
	
	public virtual UTVisibilityDecision IsVisible (FieldInfo fieldInfo)
	{
		return UTVisibilityDecision.Undetermined;
	}
	
	public virtual bool DrawArrayMember (UTFieldWrapper wrapper, out bool deleteMember)
	{
		deleteMember = false;
		return false;
	}
	
	public virtual bool DrawProperty (UTFieldWrapper wrapper)
	{
		return false;
	}
	
	
	public virtual UTFieldValidity CheckValidity (UTFieldWrapper wrapper, out string errorMessage)
	{
		return defaultChecker.CheckValidity(wrapper, out errorMessage);
	}

	
}


