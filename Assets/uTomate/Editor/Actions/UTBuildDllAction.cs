//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;
using System.IO;

[UTActionInfo(actionCategory = "Build")]
[UTDoc(title="Build DLL", description="Builds a DLL from C# sources.")]
[UTDefaultAction]
public class UTBuildDllAction : UTAction
{
	[UTDoc(description="The base directory where to look for sources. Defaults to the current project's assets folder.")]
	[UTInspectorHint(order=0)]
	public UTString baseDirectory;
	[UTDoc(description="The sources to include.")]
	[UTInspectorHint(order=1)]
	public UTString[] includes;
	[UTDoc(description="The sources to exclude.")]
	[UTInspectorHint(order=2)]
	public UTString[] excludes;
	[UTDoc(description="Include UnityEngine.dll als dependency.")]
	[UTInspectorHint(order=3)]
	public UTBool includeEngineDll;
	[UTDoc(description="Include UnityEditor.dll als dependency.")]
	[UTInspectorHint(order=4)]
	public UTBool includeEditorDll;
	[UTDoc(description="A list of DLL files which should be referenced when building. The UnityEngine.dll and UnityEditor.dll will be added automatically.")]
	[UTInspectorHint(order=5)]
	public UTString[] referencedAssemblies;
	[UTDoc(description="The full path of the output DLL.")]
	[UTInspectorHint(order=6, required=true, displayAs=UTInspectorHint.DisplayAs.SaveFileSelect, caption="Select output file.")]
	public UTString outputFile;
	[UTDoc(description="Any symbols that should be defined,separated by a semicolon or comma, e.g. 'UNITY_EDITOR;UNITY_4_0'")]
	[UTInspectorHint(order=7)]
	public UTString defineSymbols;
	[UTDoc(title="Compiler Options",description="Additional compiler options that you want to set.")]
	[UTInspectorHint(order=8)]
	public UTString additionalCompilerOptions;
	
	
	
	public override IEnumerator Execute (UTContext context)
	{
		var realIncludes = EvaluateAll (includes, context);
		var realExcludes = EvaluateAll (excludes, context);
		
		var theBaseDirectory = baseDirectory != null ? baseDirectory.EvaluateIn(context) : null;
		if (string.IsNullOrEmpty(theBaseDirectory)) {
			theBaseDirectory = Application.dataPath;
		}
		
		if (!Directory.Exists(theBaseDirectory)) {
			throw new UTFailBuildException("The base directory " + theBaseDirectory + " does not exist or is not a directory", this);
		}
		
		
		var fileList = UTFileUtils.CalculateFileset (theBaseDirectory, realIncludes, realExcludes, UTFileUtils.FileSelectionMode.Files);
		
		if (fileList.Length == 0) {
			throw new UTFailBuildException ("No files were selected for build. Please check includes and excludes.", this);
		}
		
		var realOutputFile = outputFile.EvaluateIn (context);
		if (!realOutputFile.EndsWith (".dll")) {
			if (UTPreferences.DebugMode) {
				Debug.LogWarning("The output file does not end with .dll. The built DLL will not be picked up by Unity.");
			}
		}
		
		UTFileUtils.EnsureParentFolderExists (realOutputFile);
		
		if (UTPreferences.DebugMode) {
			foreach (var file in fileList) {
				Debug.Log ("Compiling " + file);
			}
		} else {
			Debug.Log ("Compiling " + fileList.Length + " files.");
		}
		
		var compiler = new CSharpCodeProvider ();
		CompilerParameters parameters = new CompilerParameters ();
		var doIncludeEngineDll = true;
		if (includeEngineDll != null) { // can happen when we migrate older automation plans which didn't have this setting.
			doIncludeEngineDll = includeEngineDll.EvaluateIn(context);
		}
		if (doIncludeEngineDll) {
			parameters.ReferencedAssemblies.Add (UnityDll ());
		}
		if (includeEditorDll.EvaluateIn (context)) {
			parameters.ReferencedAssemblies.Add (UnitEditorDll ());	
		}
		
		var realAssemblies = EvaluateAll (referencedAssemblies, context);
		foreach (var asm in realAssemblies) {
			parameters.ReferencedAssemblies.Add (asm);
		}
		
		parameters.GenerateExecutable = false;
		parameters.OutputAssembly = realOutputFile;
		
		var theSymbols = defineSymbols.EvaluateIn(context);
		if (!string.IsNullOrEmpty(theSymbols)) {
			parameters.CompilerOptions = "/define:"+theSymbols.Trim();
		}
		
		var theAdditionalOptions = additionalCompilerOptions.EvaluateIn(context);
		if (!string.IsNullOrEmpty(theAdditionalOptions)) {
			parameters.CompilerOptions += " " + theAdditionalOptions.Trim();
		}
		
		SetPath ();
		CompilerResults results = compiler.CompileAssemblyFromFile (parameters, fileList);
		
		if (UTPreferences.DebugMode) {
			var output = results.Output;
			foreach (var line in output) {
				Debug.Log (line);
			}
		}
		
		var errors = results.Errors;
		var hadErrors = false;
		foreach (CompilerError error in errors) {
			if (error.IsWarning) {
				Debug.LogWarning (error.ToString ());
			} else {
				hadErrors = true;
				Debug.LogError (error.ToString ()); // TODO link errors to file.
			}
		}
		if (hadErrors) {
			throw new UTFailBuildException ("There were compilation errors.", this);
		}
		Debug.Log ("Built " + realOutputFile + " from " + fileList.Length + " source file(s).");
		yield return "";
	}
	
	private static string UnityDll ()
	{
		if (Application.platform == RuntimePlatform.OSXEditor) {
			return EditorApplication.applicationContentsPath + "/Frameworks/Managed/UnityEngine.dll";
		} else {		
			return EditorApplication.applicationContentsPath + "/Managed/UnityEngine.dll";
		}
	}
	
	private static string UnitEditorDll ()
	{
		if (Application.platform == RuntimePlatform.OSXEditor) {
			return EditorApplication.applicationContentsPath + "/Frameworks/Managed/UnityEditor.dll";
		} else {		
			return EditorApplication.applicationContentsPath + "/Managed/UnityEditor.dll";
		}
	}
	
	private static void SetPath ()
	{
		if (Application.platform == RuntimePlatform.OSXEditor) {
			var monoPath = EditorApplication.applicationPath + "/Contents/Frameworks/Mono/bin";
			if (!Environment.GetEnvironmentVariable ("PATH").Contains (monoPath)) {
				Environment.SetEnvironmentVariable ("PATH", monoPath + ":" + Environment.GetEnvironmentVariable ("PATH"));
			}
		}
	}
	
	[MenuItem("Assets/Create/uTomate/Build/Build DLL", false, 390)]
	public static void AddAction ()
	{
		Create<UTBuildDllAction> ();
	}

	
}

