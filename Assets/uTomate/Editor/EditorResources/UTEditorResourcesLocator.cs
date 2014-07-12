using UnityEngine;
using System.Collections;

public class UTEditorResourcesLocator {
	
	// If you decide to move the uTomate folder to some other place, please update the
	// path to the location of the uTomate EditorResources folder here.
	// The folder path must have NO leading or trailing slash.

	// Example: If you moved the uTomate folder to 3rdParty/uTomate then the new path would be
	// public const string ResourcePath = "3rdParty/uTomate/Editor/EditorResources";
#if UTOMATE_DEMO	
	// Please ignore this line, it is only relevant for the uTomate demo version.
	public const string ResourcePath = "uTomate Demo/Editor/EditorResources";
#else
	public const string ResourcePath = "uTomate/Editor/EditorResources";
#endif
}
