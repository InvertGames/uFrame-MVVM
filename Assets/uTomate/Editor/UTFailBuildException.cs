//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// Exception being thrown to signal that the build should be aborted.
/// </summary>
public class UTFailBuildException : UnityException
{
	UnityEngine.Object context;
	
	public UTFailBuildException (string message, UnityEngine.Object context) : base(message) {
		this.context = context;	
	}
	
	public void LogToConsole() {
		Debug.LogError(Message, context);
	}
}

