//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using System.Collections;

[UTActionInfo(actionCategory = "Import & Export", sinceUTomateVersion="1.2.1")]
[UTDoc(title="Refresh Asset Database", description="Refreshes the asset database so copied files get picked up by the editor.")]
[UTDefaultAction]
public class UTRefreshAssetDatabaseAction : UTAction
{
	[UTDoc(description="When ticked, forces an update, regardless of wether or not the asset's modification time has changed.")]
	public UTBool forceUpdate;

	public override IEnumerator Execute (UTContext context)
	{
		if (forceUpdate != null && forceUpdate.EvaluateIn(context)) { // != null check is because we added this property later.
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}
		else {
			AssetDatabase.Refresh();
		}
		yield return "";
	}

	[MenuItem("Assets/Create/uTomate/Import + Export/Refresh Asset Database", false, 400)]
	public static void AddAction() {
		Create<UTRefreshAssetDatabaseAction>();
	}
}

