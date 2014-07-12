//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Text.RegularExpressions;

/// <summary>
/// Default validity checker.
/// </summary>
public class UTDefaultFieldValidityChecker : UTFieldValidityChecker
{
	
	private Regex patternFinder = new Regex (UTContext.PlaceholderRegexp);
	
	public UTFieldValidity CheckValidity (UTFieldWrapper wrapper, out string errorMessage)
	{
		
		if (wrapper.IsString) {
			if (!wrapper.SupportsExpressions || !wrapper.UseExpression) {
				if (wrapper.InspectorHint.required) {
					if (string.IsNullOrEmpty (wrapper.Value as string)) {
						errorMessage = "This field is required.";
						return UTFieldValidity.Invalid;
					}
				}
			}
			if (wrapper.SupportsExpressions && !wrapper.UseExpression && !wrapper.InspectorHint.containsExpression) {
				var match = patternFinder.Match (wrapper.Value as string);
				if (match.Success) {
					errorMessage = "You are using a parameter reference in a plain property. Did you forget to enable expression mode?";
					return UTFieldValidity.ValidWithWarning;
				}
			}
			
			if (wrapper.InspectorHint.displayAs == UTInspectorHint.DisplayAs.Password && 
				wrapper.SupportsExpressions && !wrapper.UseExpression && !string.IsNullOrEmpty(wrapper.Value as string)) {
				errorMessage = "It is recommended to use Editor Properties to store passwords.";
				return UTFieldValidity.ValidWithWarning;
			}
			
		}
		
		if (wrapper.IsAsset) {
			if (!wrapper.SupportsExpressions || !wrapper.UseExpression) {
				if (wrapper.InspectorHint.required && wrapper.Value == null) {
					errorMessage = "This field is required.";
					return UTFieldValidity.Invalid;
				}
			}
		}
		errorMessage = "";
		return UTFieldValidity.Valid;
	}
	
}

