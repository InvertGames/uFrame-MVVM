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
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

[UTActionInfo(actionCategory = "Import & Export")]
[UTDoc(title="Upload Dependency", description="Uploads a dependency into a Sonatype Nexus installation.")]
[UTInspectorGroups(groups=new string[]{"Repository", "Artifact"})]
[UTDefaultAction]
public class UTUploadDependencyAction : UTAction
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
	[UTDoc(description="The group ID of the dependency to upload.")]
	[UTInspectorHint(required = true, group = "Artifact", order = 0)]
	public UTString groupId;
	[UTDoc(description="The artifact ID of the dependency to upload.")]
	[UTInspectorHint(required = true, group = "Artifact", order = 1)]
	public UTString artifactId;
	[UTDoc(description="The version of the dependency to upload.")]
	[UTInspectorHint(required = true, group = "Artifact", order = 2)]
	public UTString version;
	[UTDoc(description="Unless you have special needs, put the extension of your file name here (e.g. 'dll' or 'unitypackage')")]
	[UTInspectorHint(required = true, group = "Artifact", order = 3)]
	public UTString packaging;
	[UTDoc(description="The classifier of the dependency to upload.")]
	[UTInspectorHint(group = "Artifact", order = 4)]
	public UTString classifier;
	[UTDoc(description="The extension of the dependency to upload.")]
	[UTInspectorHint(group = "Artifact", order = 5)]
	public UTString extension;
	[UTDoc(description="The actual file that should be uploaded.")]
	[UTInspectorHint(required = true, displayAs=UTInspectorHint.DisplayAs.OpenFileSelect, caption="Select dependency to upload.")]
	public UTString inputFileName;
	
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
		
		
		var thePackaging = packaging.EvaluateIn (context);
		if (string.IsNullOrEmpty (thePackaging)) {
			throw new UTFailBuildException ("You need to specify the packaging.", this);
		}
		
		var theExtension = extension.EvaluateIn (context);
		var theClassifier = classifier.EvaluateIn (context);
		
		var theInputFileName = inputFileName.EvaluateIn (context);
		if (string.IsNullOrEmpty (theInputFileName)) {
			throw new UTFailBuildException ("You need to specify the input file name.", this);
		}
		
		if (Directory.Exists (theInputFileName)) {
			throw new UTFailBuildException ("The specified input file " + theInputFileName + " is a directory.", this);
		}

		if (!File.Exists (theInputFileName)) {
			throw new UTFailBuildException ("The specified input file " + theInputFileName + " does not exist.", this);		
		}
		
		WWWForm form = new WWWForm ();
		form.AddField ("r", theRepoId);
		form.AddField ("g", theGroupId);
		form.AddField ("a", theArtifactId);
		form.AddField ("v", theVersion);
		
		if (!string.IsNullOrEmpty (thePackaging)) {
			form.AddField ("p", thePackaging);
		}

		if (!string.IsNullOrEmpty (theClassifier)) {
			form.AddField ("c", theClassifier);
		}

		if (!string.IsNullOrEmpty (theExtension)) {
			form.AddField ("e", theExtension);
		}
		
		var bytes = File.ReadAllBytes (theInputFileName);
		form.AddBinaryData ("file", bytes, new FileInfo (theInputFileName).Name);
		
		var hash = UTils.ComputeHash (bytes);
		if (UTPreferences.DebugMode) {
			Debug.Log ("SHA1-Hash of file to upload: " + hash);
		}
		
		string authString = theUserName + ":" + thePassword;
		var authBytes = System.Text.UTF8Encoding.UTF8.GetBytes (authString);

#if UNITY_WP8 || UNITY_METRO
		// i have no idea why this thing has a different API on Win Phone 8
		// but it has.
		var headers = new System.Collections.Generic.Dictionary<string,string>();
#else 	
		// this is for the rest of the pack. Windows, Mac, Android, iOS.
		var headers = new Hashtable ();
#endif
		foreach (var key in form.headers.Keys) {
			headers.Add (key, form.headers [key]);
		}
		
		headers.Add ("Authorization", "Basic " + System.Convert.ToBase64String (authBytes));
		var url = UTils.BuildUrl (theNexusUrl, "/service/local/artifact/maven/content");
		using (var www = new WWW (url, form.data, headers)) {
			do {
				yield return "";
			} while(!www.isDone && !context.CancelRequested);
				
			
			if (UTPreferences.DebugMode) {
				Debug.Log ("Server Response: " + www.text);
			}
		}

		if (!context.CancelRequested) {
		
		
			using (var wc = new WebClient()) {

				if (!string.IsNullOrEmpty (theUserName)) {
					Debug.Log("Setting credentials" );
					wc.Credentials = new NetworkCredential (theUserName, thePassword);
				}
		
		
				Uri uri = new Uri (UTils.BuildUrl (theNexusUrl, "/service/local/artifact/maven/resolve?") +
			"g=" + Uri.EscapeUriString (theGroupId) +
			"&a=" + Uri.EscapeUriString (theArtifactId) + 
			"&v=" + Uri.EscapeUriString (theVersion) + 
			"&r=" + Uri.EscapeUriString (theRepoId) + 
			"&p=" + Uri.EscapeUriString (thePackaging) +
			(!string.IsNullOrEmpty (theClassifier) ? "&c=" + Uri.EscapeUriString (theClassifier) : "") +
			(!string.IsNullOrEmpty (theExtension) ? "&e=" + Uri.EscapeUriString (theExtension) : ""));
		
		
				var downloadFinished = false;
				var error = false;
				string result = null;
				wc.DownloadStringCompleted += delegate( object sender, DownloadStringCompletedEventArgs e) {
					downloadFinished = true;
					error = e.Error != null;
					if (error) {
						Debug.LogError ("An error occured while downloading artifact information. " + e.Error.Message, this);
					} else {
						result = (string)e.Result;
					}
				};
		
				wc.DownloadStringAsync (uri);
			
				do {
					yield return "";
					if (context.CancelRequested) {
						wc.CancelAsync ();
					}
				} while(!downloadFinished);
			
				if (!context.CancelRequested) {
					
					if (!error) {
						if (UTPreferences.DebugMode) {
							Debug.Log ("Server Response: " + result);
						}	
						if (result.Contains ("<sha1>" + hash + "</sha1>")) {
							Debug.Log ("Successfully uploaded artifact " + theInputFileName + ".", this);
						} else {
							throw new UTFailBuildException ("Upload failed. Checksums do not match.", this);
						}
					} else {
						throw new UTFailBuildException ("Artifact verification failed", this);
					}
				}
			}
		}
	}
	
	[MenuItem("Assets/Create/uTomate/Import + Export/Upload Dependency", false, 260)]
	public static void AddAction ()
	{
		Create<UTUploadDependencyAction> ();
	}


}

