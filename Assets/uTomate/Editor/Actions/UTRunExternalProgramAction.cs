//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UDebug = UnityEngine.Debug;

[UTActionInfo(actionCategory = "Run")]
[UTDoc(title="Run external program", description="Run an external program.")]
[UTDefaultAction]
public class UTRunExternalProgramAction : UTAction
{
	
	private const string Marker = "_____UTOMATE__FILE__MARKER______";
	[UTDoc(description="The full path to the executable that should be run.")]
	[UTInspectorHint(required=true, order=0, displayAs=UTInspectorHint.DisplayAs.OpenFileSelect, caption="Select executable.")]
	public UTString pathToExecutable;
	[UTDoc(description="Working directory in which the command should be executed. Can be empty in which case the system directory will be used as working directory.")]
	[UTInspectorHint(required=false, order=1, displayAs=UTInspectorHint.DisplayAs.FolderSelect, caption="Select working directory.")]
	public UTString workingDirectory;
	[UTDoc(description="Should ShellExecute be used to start the process?")]
	[UTInspectorHint(order=2)]
	public UTBool useShellExecute;
	[UTDoc(description="Calculate a file set that should be given as arguments to the external program?")]
	[UTInspectorHint(order=3)]
	public UTBool useFileset;
	[UTDoc(description="Base path for includes and excludes.")]
	[UTInspectorHint(required=true, order=4, displayAs=UTInspectorHint.DisplayAs.FolderSelect, caption="Select base path for fileset.")]
	public UTString basePath;
	[UTDoc(description="Files to include into the argument list.")]
	[UTInspectorHint(order=5)]
	public UTString[] includes;
	[UTDoc(description="Files to exclude from the argument list.")]
	[UTInspectorHint(order=6)]
	public UTString[] excludes;
	[UTDoc(description="Should the file paths be given relative to the base path?")]
	[UTInspectorHint(order=7)]
	public UTBool relativePaths;
	[UTDoc(description="Path separator to use for the files.")]
	[UTInspectorHint(order=8)]
	public UTPathSeparator pathSeparator;
	[UTDoc(description="Should the external program be run once for each file in the file set?")]
	[UTInspectorHint(order=9)]
	public UTBool runOncePerFile;
	[UTDoc(description="Should the execution be aborted if the external program returns with a nonzero status code?")]
	[UTInspectorHint(order=10)]
	public UTBool failOnError;
	[UTDoc(description="Command line arguments. One argument per line. These are auto-quoted, so you don't quote them manually.")]
	[UTInspectorHint(order=11)]
	public UTExecutableParam[] arguments;
	
	// transient
	private string[] currentFiles;
	private Process currentProcess;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theExecutable = pathToExecutable.EvaluateIn (context);
		if (!System.IO.File.Exists (theExecutable)) {
			throw new UTFailBuildException ("Executable " + theExecutable + " does not exist.", this);
		}
		
		string[] finalFileSet;
		if (useFileset.EvaluateIn (context)) {
			var theBasePath = basePath.EvaluateIn (context);
			if (!Directory.Exists (theBasePath)) {
				throw new UTFailBuildException ("The base path " + theBasePath + " does not exist.", this);
			}
			
			var theIncludes = EvaluateAll (includes, context);
			var theExcludes = EvaluateAll (excludes, context);
			
			var theFiles = UTFileUtils.CalculateFileset (theBasePath, theIncludes, theExcludes, UTFileUtils.FileSelectionMode.Files);
			
			if (relativePaths.EvaluateIn (context)) {
				UTFileUtils.StripBasePath (theFiles, theBasePath);
			}
			
			if (pathSeparator.EvaluateIn (context) == PathSeparator.Windows) {
				UTFileUtils.SlashesToWindowsSlashes (theFiles);
			}
			
			finalFileSet = theFiles;
		} else {
			finalFileSet = new string[0];
		}
		
		
		if (runOncePerFile.EvaluateIn (context)) {
			foreach (var file in finalFileSet) {
				if (context.CancelRequested) {
					break;
				}

				currentFiles = new string[]{file};
				InvokeProgram (theExecutable, context);	
				do {
					yield return "";
					if (context.CancelRequested && !currentProcess.HasExited) {
						currentProcess.Kill ();
						break;
					}
				} while (!currentProcess.HasExited);
				
				CheckResult (context);
			}
		} else {
			currentFiles = finalFileSet;
			InvokeProgram (theExecutable, context);
			do {
				yield return "";
				if (context.CancelRequested && !currentProcess.HasExited) {
					currentProcess.Kill ();
				}
			} while(!currentProcess.HasExited);

			CheckResult (context);
		}
	}
	
	/// <summary>
	/// Called from the program parameters when evaluating. Allows to construct a list of selected files into
	/// a parameter list.
	/// </summary>
	/// <param name='input'>
	/// An array or a single string that should be repeated for each file that was found by the fileset.
	/// </param>
	public object[] Repeat (object input)
	{
		if (currentFiles.Length == 0) {
			return new object[0];
		} else {
			var result = new object[currentFiles.Length];
			
			for (int i=0; i<currentFiles.Length; i++) {
				var file = currentFiles [i];
				var output = new List<string> ();
				
				if (input is IEnumerable && !(input is string)) {
					foreach (var item in (IEnumerable)input) {
						output.Add (item.ToString ().Replace (Marker, file));
					}
				} else {
					output.Add (input.ToString ().Replace (Marker, file));
				}
				result [i] = output.ToArray ();
			}
			
			return result;
		}
	}
	
	/// <summary>
	/// Called from the program parameters when evaluating. Does the same as Repeat(object) but constructs a single 
	/// parameter instead of a list.
	/// </summary>
	/// <param name='input'>
	/// An array or a single string that should be repeated for each file that was found by the fileset.
	/// </param>
	/// <param name='separator'>
	/// A separator char that should be used to separate the entries for each file.
	/// </param>
	public object[] Repeat (object input, string separator)
	{
		object[] repeatedInput = Repeat (input);
		
		var parts = Array.ConvertAll<object, string> (repeatedInput, item => Flatten (item));
		
		return new object[] {
			string.Join (separator, parts)
		};
	}
	
	/// <summary>
	/// Called from the program parameters when evaluating. Allows to inject a file placeholder into a repeat statement.
	/// </summary>
	public string File ()
	{
		return Marker;
	}
	
	/// <summary>
	/// Checks the result of the last program execution. Throws an exception if the execution returned a nonzero status
	/// code and failonerror is set to true.
	/// </summary>
	private void CheckResult (UTContext context)
	{
		if (!context.CancelRequested) {
			if (failOnError.EvaluateIn (context)) {			
				if (currentProcess.HasExited && currentProcess.ExitCode != 0) {
					throw new UTFailBuildException ("Process exited with non-zero exit code " + currentProcess.ExitCode, this);
				}
			}
		}
	}
	
	/// <summary>
	/// Flatten the specified input into a string.
	/// </summary>
	/// <param name='input'>
	/// An array of mixed objects that should be flattened.
	/// </param>
	private string Flatten (object input)
	{
		if (input is IEnumerable && !(input is string)) {
			IEnumerable inputEnumerable = (IEnumerable)input;
			List<string> parts = new List<string> ();
			foreach (var part in inputEnumerable) {
				parts.Add (Flatten (part));
			}
			
			return string.Join ("", parts.ToArray ());
		} else {
			return input.ToString ();
		}
	}
	
	/// <summary>
	/// Invokes the program.
	/// </summary>
	/// <param name='executable'>
	/// Executable to invoke.
	/// </param>
	/// <param name='context'>
	/// The context. Parameters will be calculated using the context.
	/// </param>
	/// <exception cref='UTFailBuildException'>
	/// Is thrown when the program cannot be started.
	/// </exception>
	private void InvokeProgram (string executable, UTContext context)
	{
		
		var args = EvaluateAll (arguments, context);
		
		string finalArgs = string.Join (" ", args);
		if (finalArgs.Contains (Marker)) {
			throw new UTFailBuildException ("You cannot use $me.File() outside of $me.Repeat(). Please check your parameters.", this);
		}
		
		var doUseShellExecute = useShellExecute.EvaluateIn (context);
		
		var theWorkingDirectory = workingDirectory.EvaluateIn(context);
		if (!string.IsNullOrEmpty(theWorkingDirectory)) {
			if (!Directory.Exists(theWorkingDirectory)) {
				throw new UTFailBuildException("Working directory " + theWorkingDirectory + " does not exist.", this);
			}
		}
		
		Process process = new Process ();
		process.StartInfo.FileName = executable;
		process.StartInfo.Arguments = finalArgs;
		process.StartInfo.UseShellExecute = doUseShellExecute;
		
		if (!string.IsNullOrEmpty(theWorkingDirectory)) {
			process.StartInfo.WorkingDirectory = theWorkingDirectory;
		}
		
		if (!doUseShellExecute) {
			if (UTPreferences.DebugMode) {
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.OutputDataReceived += (sender, argv) => UDebug.Log ("[Execute]" + argv.Data);
				process.ErrorDataReceived += (sender, argv) => UDebug.LogWarning ("[Execute]" + argv.Data);
			}
		}
		
		try {
			UDebug.Log ("Starting process " + executable);
			if (UTPreferences.DebugMode) {
				UDebug.Log ("Args: " + finalArgs);
			}
			
			process.Start ();
			if (!doUseShellExecute && UTPreferences.DebugMode) {
				process.BeginOutputReadLine ();
			}
			currentProcess = process;

		} catch (Win32Exception e) {
			throw new UTFailBuildException ("Couldn't start process: " + e.Message, this);
		}	
	}
	
	[MenuItem("Assets/Create/uTomate/Run/Run External Program", false, 260)]
	public static void AddAction ()
	{
		Create<UTRunExternalProgramAction> ();
	}
	
	/// <summary>
	/// Path separators that can be used for converting the calculated file set into path names.
	/// </summary>
	public enum PathSeparator
	{
		Windows,
		Unix
	}
	
}
