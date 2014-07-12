//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEditor	;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

/// <summary>
/// External runner which allows invoking uTomate from a CI server.
/// </summary>
public class UTExternalRunner {

	public static void RunPlan() {
		var plan = GetArg("-plan");
		if (plan == null) {
			throw new ArgumentException("You need to specify a -plan argument.");
		}
		
		
		UTAutomationPlan thePlan = UTomate.UTAutomationPlanByName(plan);	
		if (thePlan == null) {
			throw new ArgumentException("There is no plan named '" + plan + "'");
		}

		var props = GetArgs ("-prop");
		var realProps = ParseProps(props);

		var debugMode = GetArg("-debugMode");
		if (debugMode == "true") {
			UTPreferences.DebugMode = true;
		}
		else {
			UTPreferences.DebugMode = false;
		}
	
		
		UTomateRunner.Instance.OnRunnerFinished += delegate(bool cancelled, bool failed) {
			EditorApplication.Exit(cancelled || failed ? 1 : 0);
		};
		
		UTomate.Run (thePlan, realProps);
		
		
	}

	/// <summary>
	/// Gets the argument with the given name.
	/// </summary>
	/// <returns>
	/// The argument or null if there is no such argument.
	/// </returns>
	/// <param name='name'>
	/// The name of the argument to get.
	/// </param>
	private static string GetArg(string name) {
		var args = System.Environment.GetCommandLineArgs();
		for(int i=0; i<args.Length;i++) {
			if (args[i] == name && args.Length > i+1) {
				return args[i+1];
			} 
		}
		return null;
	}
	
	/// <summary>
	/// Gets all arguments with the given name
	/// </summary>
	/// <returns>
	/// The arguments.
	/// </returns>
	/// <param name='name'>
	/// The name of the arguments
	/// </param>
	private static IList<string> GetArgs(string name) {
		var result = new List<string>();
		
		var args = System.Environment.GetCommandLineArgs();
		for(int i=0; i<args.Length;i++) {
			if (args[i] == name && args.Length > i+1) {
				result.Add(args[i+1]);
				i++;
			} 
		}
		return result;
	}
	
	private static Dictionary<string,string> ParseProps(IList<string> unparsedProps) {
		var result = new Dictionary<string,string>();
		foreach(var prop in unparsedProps) {
			var idx = prop.IndexOf("=");
			if (idx == -1) {
				throw new ArgumentException("Invalid property argument: " + prop);
			}
			
			string name = prop.Substring(0,idx);
			string val = "";
			if (idx+1 < prop.Length) {
				val = prop.Substring(idx+1);
			}
			
			result[name] = val;
		}
		return result;
	}
}
