//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// This is a special kind of wrapper around a string, which is used to format command line parameter arguments
/// for <see cref="UTRunExternalProgramAction"/>. This wrapper basically allows you to specify a list of parameters
/// as input for a single string. The wrapper will  automatically quote the parameters and format them into a 
/// single string.
/// </summary>
[Serializable]
public class UTExecutableParam : UTString
{
	
	protected override string CustomCast (object val)
	{
		string result;
		if (val is IEnumerable && !(val is string)) {
			List<string> parts = Flatten ((IEnumerable)val);
			for (int i = 0; i < parts.Count; i++) {
				parts [i] = Quote (parts [i]);
			}
			result = string.Join (" ", parts.ToArray ());
		} else {
			result = Quote (val == null ? "null" : val.ToString ());
		}
		LogConversion (val, result);
		return result;
	}
	
	public override bool AlwaysCast {
		get {
			return true;
		}
	}
	
	private List<string> Flatten (IEnumerable input)
	{
		List<string> result = new List<string> ();
		foreach (var item in input) {
			if (item is IEnumerable && !(item is string)) {
				result.AddRange (Flatten ((IEnumerable)item));
			} else {
				result.Add (item.ToString ());
			}
		}
		return result;
	}
	
	public static string Quote (string input)
	{
		// first replace all quotes with double quotes
		var result = input.Replace("\"", "\"\"");
		// finally wrap quotes
		return "\"" + result + "\"";
	}
}

