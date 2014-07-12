//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;

public interface UTFieldValidityChecker
{
	/// <summary>
	/// Allows to check the validity of a field input and return an error message.
	/// </summary>
	/// <returns>
	/// True if the field is valid, false otherwise.
	/// </returns>
	/// <param name='wrapper'>
	/// the field wrapper containing the currently rendered field.
	/// </param>
	/// <param name='errorMessage'>
	///  the error message to display. 
	/// </param>
	UTFieldValidity CheckValidity(UTFieldWrapper wrapper, out string errorMessage);
	
	
}

