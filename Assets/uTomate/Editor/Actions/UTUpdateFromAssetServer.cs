//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UDebug = UnityEngine.Debug;
using System.ComponentModel;

[UTDoc(title="Update from Asset server", description="Updates the current project from the asset server.")]
[UTDefaultAction]
public class UTUpdateFromAssetServer : UTAction {
	public string assetServerLocation;
	public string assetServerPort;
	public string projectOnAssetServer;
	public string username;
	public string password;
	
	public override IEnumerator Execute (UTContext context)
	{
		
		UDebug.LogWarning("This action is experimental and might break.", this);
		
		bool connError = (bool)UTInternalCall.InvokeStatic("UnityEditor.AssetServer", "HasConnectionError");
		
		if (connError) {
			throw new UTFailBuildException("Currently unable to connect to the asset server.",this);
		}
		
		UDebug.Log ("Not working properly...");
		throw new UTFailBuildException("Not implemented.",this);
		/*
		UDebug.Log("Starting update...");
		Process process = new Process();
		process.StartInfo.FileName = GetEditorExecutable();
		process.StartInfo.Arguments = "-batchMode -assetServerUpdate " + string.Join(" ", new string[] {assetServerLocation, projectOnAssetServer, username, password});
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.UseShellExecute = false;
		process.OutputDataReceived += (sender, args) => UDebug.Log("[Unity]"+ args.Data);
		try {
			process.Start();
					process.BeginOutputReadLine();
		process.WaitForExit();
		if (process.ExitCode != 0) {
			UDebug.LogError("Update failed.");
			return false;
		}

		}
		catch(Win32Exception e) {
			UDebug.LogError("Couldn't start process: " + e.Message);
			return false;
		}
		return true;*/
	}
	
	private static string GetCurrentProjectPath() {
		return Directory.GetParent(Application.dataPath).FullName;
	}
	
	private static string GetEditorExecutable() {
		if (Application.platform == RuntimePlatform.OSXEditor) {
			return EditorApplication.applicationPath + "/Contents/MacOs/Unity";
		}
		else {
			return EditorApplication.applicationPath;
		}
	}
	
//	[MenuItem("Assets/Create/uTomate/Update from Asset Server")]
	public static void AddAction() {
		Create<UTUpdateFromAssetServer>();
	}

}
