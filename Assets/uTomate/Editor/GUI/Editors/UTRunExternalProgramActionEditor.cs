//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UTRunExternalProgramAction))]
public class UTRunExternalProgramActionEditor : UTInspectorBase
{
	
	public override UTVisibilityDecision IsVisible (System.Reflection.FieldInfo fieldInfo)
	{
		var self = (UTRunExternalProgramAction)target;
		switch (fieldInfo.Name) {
		case "basePath":
		case "includes":
		case "excludes":
		case "relativePaths":
		case "pathSeparator":
		case "runOncePerFile":
			return (self.useFileset.UseExpression || self.useFileset.Value) ? 
				UTVisibilityDecision.Visible : UTVisibilityDecision.Invisible;
		}
		
		return base.IsVisible (fieldInfo);
	}
	
	public override UTFieldValidity CheckValidity (UTFieldWrapper wrapper, out string errorMessage)
	{
		var isValid = base.CheckValidity (wrapper, out errorMessage);
		if (isValid == UTFieldValidity.Valid) {
			// don't overwrite error messages from above.
			if (wrapper.FieldName == "arguments") {
				if (wrapper.UseExpression ? wrapper.Expression.Contains ("\\\"") : (wrapper.Value as String).Contains ("\"")) {
					errorMessage = "You've typed a quote or quoting expression. Note that all quoting is done automatically by the action, so this might not do what you expect.";
					return UTFieldValidity.ValidWithWarning;
				}
				
				if (!wrapper.UseExpression && ((wrapper.Value)as string ).Contains(" ")) {
					errorMessage = "You've typed a space character in this parameter. Depending on your use case this might be ok, but make sure that you place one parameter per line.";
					return UTFieldValidity.ValidWithWarning;
				}
			}
			
		}
		return isValid;
	}
	
}


