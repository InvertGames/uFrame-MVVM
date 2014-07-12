//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections;
using System.Diagnostics;
using UDebug = UnityEngine.Debug;
using System.IO;
using System.Text;
using System.ComponentModel;
using UnityEditor;

[UTActionInfo(actionCategory = "Run")]
[UTDoc(title="Run uTomate in external project", description="Opens an external project in a second copy of Unity and runs a uTomate plan in in.")]
[UTDefaultAction]
public class UTRunUTomateInExternalProjectAction : UTAction
{
	
	[UTDoc(description="Full path to the other project.")]
	[UTInspectorHint(order=0, required=true, displayAs=UTInspectorHint.DisplayAs.FolderSelect, caption="Select root path of other project.")]
	public UTString projectPath;
	[UTDoc(description="Name of the plan to run.")]
	[UTInspectorHint(required=true, order=1)]
	public UTString planName;
	[UTDoc(description="Should debug mode be enabled when running the plan?")]
	[UTInspectorHint(order=2)]
	public UTBool debugMode;
	[UTDoc(description="Properties that should be set before launching the plan.")]
	[UTInspectorHint(order=3)]
	public UTString[] properties;
	[UTDoc(description="Should the execution be aborted if the plan fails or is cancelled?")]
	[UTInspectorHint(order=4)]
	public UTBool failOnError;
	[UTDoc(description="Path to the Unity editor executable. If not set, the currently running Unity version will be used.")]
	[UTInspectorHint(order=5, displayAs=UTInspectorHint.DisplayAs.OpenFileSelect, caption="Select the path to Unity")]
	public UTString unityEditorExecutable;	

	public override IEnumerator Execute (UTContext context)
	{
		var theProjectPath = projectPath.EvaluateIn (context);
		if (!Directory.Exists (theProjectPath)) {
			throw new UTFailBuildException ("Project path " + theProjectPath + " does not exist.", this);
		}
		
		
		if (UTFileUtils.IsBelow (UTFileUtils.ProjectRoot, theProjectPath)) {
			throw new UTFailBuildException ("You cannot run uTomate externally on the current project. Use the Sub-Plan node if you want to run a plan as part of a plan.", this);
		}
		
		var thePlanName = planName.EvaluateIn (context);
		var theDebugMode = debugMode.EvaluateIn (context);
		
		var theProperties = EvaluateAll (properties, context);
		
		StringBuilder sb = new StringBuilder ();
		foreach (var prop in theProperties) {
			sb.Append (" -prop ").Append (UTExecutableParam.Quote (prop));
		}
		
		Process process = new Process ();
		process.StartInfo.FileName = UTils.GetEditorExecutable ();
		process.StartInfo.Arguments = "-projectPath " + UTExecutableParam.Quote (theProjectPath) + 
			" -executeMethod UTExternalRunner.RunPlan -plan " + UTExecutableParam.Quote (thePlanName) + 
				" -debugMode " + theDebugMode + sb.ToString ();
		if (UTPreferences.DebugMode) {
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.UseShellExecute = false;
			process.OutputDataReceived += (sender, args) => UDebug.Log ("[Unity]" + args.Data);
			UDebug.Log ("Executing: " + process.StartInfo.FileName + " with arguments " + process.StartInfo.Arguments);
		}
		
		try {
			if (!process.Start ()) {
				throw new UTFailBuildException ("Unable to start Unity3D.", this);
			}
			if (UTPreferences.DebugMode) {
				process.BeginOutputReadLine ();
			}
		} catch (Win32Exception e) {
			throw new UTFailBuildException ("Unable to start process " + e.Message, this);
		}
		do {
			yield return "";
			if (context.CancelRequested && !process.HasExited) {
				process.Kill ();
				break;
			}
			
		} while(!process.HasExited);
		
		if (!context.CancelRequested && failOnError.EvaluateIn (context)) {
			if (process.ExitCode != 0) {
				throw new UTFailBuildException ("Plan " + thePlanName + " failed or was cancelled.", this);
			}
		}
		
	}
	
	[MenuItem("Assets/Create/uTomate/Run/Run uTomate in external project", false, 270)]
	public static void AddAction ()
	{
		Create<UTRunUTomateInExternalProjectAction> ();
	}

}

