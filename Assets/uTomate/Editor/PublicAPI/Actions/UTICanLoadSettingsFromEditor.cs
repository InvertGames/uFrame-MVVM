//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using System.Collections;

/// <summary>
/// Interface which can be implemented by actions which can load defaults/settings from editor.
/// </summary>
public interface UTICanLoadSettingsFromEditor {
	
	/// <summary>
	/// Loads the settings or defaults from the editor.
	/// </summary>
	void LoadSettings();
	
	/// <summary>
	/// Gets the undo text for loading the settings. This is used by the editor to provide an undo stack item
	/// before instructing the action to load the settings from the editor.
	/// </summary>
	string LoadSettingsUndoText {
		get;
	}
}
