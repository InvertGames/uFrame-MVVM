//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Net;
using System.IO;
using System.ComponentModel;
using UnityEngine;
using UnityEditor;

[UTActionInfo(actionCategory = "Import & Export")]
[UTDoc(title="Download Dependency", description="Downloads a dependency from a Sonatype Nexus installation.")]
[UTInspectorGroups(groups = new string[] {"Repository", "Artifact"})]
[UTDefaultAction]
public class UTDownloadDependencyAction : UTAction
{
	
	[UTDoc(description="The base URL of your Sonatype Nexus installation.")]
	[UTInspectorHint(required = true, group = "Repository", order = 0)]
	public UTString nexusUrl;

	[UTDoc(description="The id of the repository from which the dependency should be downloaded.")]
	[UTInspectorHint(required = true, group = "Repository", order = 1)]
	public UTString repositoryId;

	[UTDoc(description="The user name which should be used to log into Nexus. If your repository does not require login you can leave this empty.")]
	[UTInspectorHint(group = "Repository", order = 2)]
	public UTString userName;

	[UTDoc(description="The password which should be used to log into Nexus. If your repository does not require login you can leave this empty.")]
	[UTInspectorHint(group = "Repository", order = 3)]
	public UTString password;

	[UTDoc(description="The group ID of the dependency to download.")]
	[UTInspectorHint(required = true, group = "Artifact", order = 0)]
	public UTString groupId;

	[UTDoc(description="The artifact ID of the dependency to download.")]
	[UTInspectorHint(required = true, group = "Artifact", order = 1)]
	public UTString artifactId;

	[UTDoc(description="The version ID of the dependency to download.")]
	[UTInspectorHint(required = true, group = "Artifact", order = 2)]
	public UTString version;

	[UTDoc(description="The packaging of the dependency to download.")]
	[UTInspectorHint(group = "Artifact", order = 3, required=true)]
	public UTString packaging;

	[UTDoc(description="The classifier of the dependency to download.")]
	[UTInspectorHint(group = "Artifact", order = 4)]
	public UTString classifier;

	[UTDoc(description="The extension of the dependency to download.")]
	[UTInspectorHint(group = "Artifact", order = 5)]
	public UTString extension;

	[UTDoc(description="The file name where the dependency should be stored at. If it's parent folders do not exist, they will be created automatically.")]
	[UTInspectorHint(required=true, displayAs=UTInspectorHint.DisplayAs.SaveFileSelect, caption="Select destination file name for dependency")]
	public UTString outputFileName;

	private bool downloadFinished;
	private bool error;
	
	public override System.Collections.IEnumerator Execute (UTContext context)
	{
		if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebPlayer || 
			EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebPlayerStreamed) {
			Debug.LogWarning("You have currently set the build target to 'Web Player'. This may cause interference with actions that access the internet. If you get an error message about cross domain policy from this action, switch the target to 'PC and Mac Standalone' and try again.");
		}

		var theNexusUrl = nexusUrl.EvaluateIn (context);
		if (string.IsNullOrEmpty (theNexusUrl)) {
			throw new UTFailBuildException ("You need to specify the nexus URL", this);
		}
		
		var theRepoId = repositoryId.EvaluateIn (context);
		if (string.IsNullOrEmpty (theRepoId)) {
			throw new UTFailBuildException ("You need to specify the repository id.", this);
		}
		
		
		var theUserName = userName.EvaluateIn (context);
		var thePassword = password.EvaluateIn (context);

		var theGroupId = groupId.EvaluateIn (context);
		if (string.IsNullOrEmpty (theGroupId)) {
			throw new UTFailBuildException ("You need to specify the group id.", this);
		}
		
		var theArtifactId = artifactId.EvaluateIn (context);
		if (string.IsNullOrEmpty (theArtifactId)) {
			throw new UTFailBuildException ("You need to specify the artifact id.", this);
		}
		
		var theVersion = version.EvaluateIn (context);
		if (string.IsNullOrEmpty (theVersion)) {
			throw new UTFailBuildException ("You need to specify the version.", this);
		}
		
		
		var thePackaging = packaging.EvaluateIn(context);
		if (string.IsNullOrEmpty (thePackaging)) {
			throw new UTFailBuildException ("You need to specify the packaging.", this);
		}
		
		var theExtension = extension.EvaluateIn(context);
		var theClassifier = classifier.EvaluateIn(context);
		
		var theOutputFileName = outputFileName.EvaluateIn (context);
		if (string.IsNullOrEmpty (theOutputFileName)) {
			throw new UTFailBuildException ("You need to specify the output file name.", this);
		}
		
		if (Directory.Exists (theOutputFileName)) {
			throw new UTFailBuildException ("The specified output file " + theOutputFileName + " is a directory.", this);
		}
		
		UTFileUtils.EnsureParentFolderExists (theOutputFileName);
		
		// TODO: ignore SSL certs if required
		using (var wc = new WebClient()) {

			if (!string.IsNullOrEmpty (theUserName)) {
				wc.Credentials = new NetworkCredential (theUserName, thePassword);
			}
		
		
			Uri uri = new Uri (theNexusUrl + "/service/local/artifact/maven/content?" +
			"g=" + Uri.EscapeUriString (theGroupId) +
			"&a=" + Uri.EscapeUriString (theArtifactId) + 
			"&v=" + Uri.EscapeUriString (theVersion) + 
			"&r=" + Uri.EscapeUriString (theRepoId) + 
			"&p=" + Uri.EscapeUriString(thePackaging) +
			(!string.IsNullOrEmpty(theClassifier) ? "&c=" + Uri.EscapeUriString(theClassifier) : "") +
			(!string.IsNullOrEmpty(theExtension) ? "&e=" + Uri.EscapeUriString(theExtension) : ""));
		
		
			downloadFinished = false;
			error = false;
			wc.DownloadFileCompleted += delegate( object sender, AsyncCompletedEventArgs e) {
				downloadFinished = true;
				error = e.Error != null;
				if (error) {
					Debug.LogError ("An error occured while downloading. " + e.Error.Message, this);
				}
			};
		
			wc.DownloadFileAsync (uri, theOutputFileName);
			
			do {
				yield return "";
				if (context.CancelRequested) {
					wc.CancelAsync ();
				}
			} while(!downloadFinished);
			
			if (!error && !context.CancelRequested) {
				Debug.Log ("Successfully downloaded artifact to " + theOutputFileName + ".", this);
			}
			
			if (context.CancelRequested) {
				File.Delete(theOutputFileName);
			}
				
		}
	}
	
	[MenuItem("Assets/Create/uTomate/Import + Export/Download Dependency", false, 250)]
	public static void AddAction ()
	{
		Create<UTDownloadDependencyAction> ();
	}


}

