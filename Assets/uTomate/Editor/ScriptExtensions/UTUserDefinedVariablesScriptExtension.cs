//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Script extension for loading user defined variables.
/// </summary>
[UTScriptExtension(1)]
public class UTUserDefinedVariablesScriptExtension : UTIContextAware
{
	
	public UTContext Context {
		set {
			SetupVariables (value);
		}
	}
	
	private void SetupVariables (UTContext context)
	{
		// this is intended, so project properties can refer to editor properties
		UTEditorProperties.ApplyTo (context);
		UTomate.ProjectProperties.ApplyTo (context);
		// but editor properties overwrite project properties.
		UTEditorProperties.ApplyTo (context);
	}
	
}

