//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// Helper class which calculates filesets. All nodes expect normalized file/folder names. You can normalize a name
/// using the NormalizeSlashes method.
/// </summary>
public class UTFileUtils
{
	
	/// <summary>
	/// Calculates a file set from Application.dataPath using the given includes and excludes. If includes are empty
	/// everything will be included. If excludes are empty nothing will be excluded.
	/// </summary>
	/// <returns>
	/// The calculated list of files.
	/// </returns>
	/// <param name='includes'>
	/// The set of includes. Must not be null.
	/// </param>
	/// <param name='excludes'>
	/// The set of excludes. Must not be null.
	/// </param>	
	public static string[] CalculateFileset(string[] includes, string[] excludes) {
		return CalculateFileset(Application.dataPath, includes, excludes, FileSelectionMode.Files);
	}

	/// <summary>
	/// Calculates a file set from Application.dataPath using the given includes and excludes and mode. If includes are empty
	/// everything will be included. If excludes are empty nothing will be excluded.
	/// </summary>
	/// <returns>
	/// The calculated list of files.
	/// </returns>
	/// <param name='includes'>
	/// The set of includes. Must not be null.
	/// </param>
	/// <param name='excludes'>
	/// The set of excludes. Must not be null.
	/// </param>	
	/// <param name="mode">
	/// Mode, determining whether files, folders or both should be included in the file set.
	/// </param>
	public static string[] CalculateFileset(string[] includes, string[] excludes, FileSelectionMode mode) {
		return CalculateFileset(Application.dataPath, includes, excludes, mode);
	}

	
	/// <summary>
	/// Calculates a file set from the given base path using the given includes and excludes. If includes are empty
	/// everything will be included. If excludes are empty nothing will be excluded.
	/// </summary>
	/// <returns>
	/// The calculated list of files.
	/// </returns>
	/// <param name='basePath'>
	/// The path where to start.
	/// </param>
	/// <param name='includes'>
	/// The set of includes. Must not be null.
	/// </param>
	/// <param name='excludes'>
	/// The set of excludes. Must not be null.
	/// </param>
	/// <param name="includeDirectories">
	/// If true, files and directories will be included in the file set.
	/// </param>
	[Obsolete("Use the variant that takes a FileSelectionMode instead.")]
	public static string[] CalculateFileset(string basePath, string[] includes, string[] excludes, bool includeDirectories) {
		return CalculateFileset(basePath, includes, excludes, 
		                         includeDirectories ? FileSelectionMode.FilesAndFolders : FileSelectionMode.Files);
	}

	/// <summary>
	/// Calculates a file set from the given base path using the given includes and excludes. If includes are empty
	/// everything will be included. If excludes are empty nothing will be excluded.
	/// </summary>
	/// <returns>
	/// The calculated list of files.
	/// </returns>
	/// <param name='basePath'>
	/// The path where to start.
	/// </param>
	/// <param name='includes'>
	/// The set of includes. Must not be null.
	/// </param>
	/// <param name='excludes'>
	/// The set of excludes. Must not be null.
	/// </param>
	/// <param name="mode">
	/// Mode, determining whether files, folders or both should be included in the file set.
	/// </param>
	public static string[] CalculateFileset(string basePath, string[] includes, string[] excludes, FileSelectionMode mode) {
		var root = new DirectoryInfo(basePath);
		if (includes.Length == 0) {
			includes = new string[]{"**"};
		}
		
		HashSet<string> paths = new HashSet<string>();
		foreach(var include in includes) {
			AddToPaths(paths, root, include, mode);
		}
		foreach(var exclude in excludes) {
			RemoveFromPaths(paths, root, exclude, mode);
		}
		var result = new string[paths.Count];
		
		int idx = 0;
		foreach(var path in paths) {
			result[idx++] = path;
		}
		
		NormalizeSlashes(result);
		
		return result;

	}

	
	/// <summary>
	/// Converts all slashes in the given paths to windows backslashes. The conversion is done in-place and modifies
	/// the given array.
	/// </summary>
	/// <param name='paths'>
	/// the array of paths
	/// </param>
	public static void SlashesToWindowsSlashes(string[] paths) {
		for ( int i= 0; i < paths.Length; i++) {
			paths[i] = paths[i].Replace("/", "\\");
		}
	}
	
	/// <summary>
	/// Converts all backslashes in the given paths to forward slashes. The conversion is done in-place and modifies the
	/// given array.
	/// </summary>
	/// <param name='paths'>
	/// the array of paths.
	/// </param>
	public static void NormalizeSlashes(string[] paths) {
		for( int i = 0; i < paths.Length; i++) {
			paths[i] = NormalizeSlashes(paths[i]);
		}
	}
	
	/// <summary>
	/// Converts all backslashes in the given path to forward slashes.
	/// </summary>
	/// <returns>
	/// The normalized path.
	/// </returns>
	/// <param name='path'>
	/// The path to normalize.
	/// </param>
	public static string NormalizeSlashes(string path) {
		return Regex.Replace(path.Replace("\\", "/"), "//+", "/");
	}
	
	/// <summary>
	/// Strips the given base path from the paths in the given array. Useful to convert absolute file names
	/// to relative ones. The modification is done in-place and will change the given paths array. 
	/// </summary>
	/// <param name='paths'>
	/// The list of paths to be modified.
	/// </param>
	/// <param name='basePath'>
	/// The base path. Note that it is required that all paths in the paths array start with this path. This function
	/// will not calculate relative path names to arbitrary paths.
	/// </param>
	public static void StripBasePath(string[] paths, string basePath) {
		for (int i = 0; i < paths.Length; i++) {
			paths[i] = StripBasePath(paths[i], basePath);
		}
	}
	/// <summary>
	/// Strips the given base path from the given path. Useful to convert absolute file names
	/// to relative ones. 
	/// </summary>
	/// <param name='path'>
	/// The path to be modified
	/// </param>
	/// <param name='basePath'>
	/// The base path. Note that it is required tha the given path starts with the base path. This function
	/// will not calculate relative path names to arbitrary paths.
	/// </param>
	/// <returns>
	/// The modified path.
	/// </returns>
	public static string StripBasePath(string path, string basePath) {
		basePath = NormalizeSlashes(basePath);
		path = NormalizeSlashes(path);
		if (path.StartsWith(basePath)) {
			if (path.Length > basePath.Length) {
				return path.Substring(basePath.EndsWith("/") ? basePath.Length : basePath.Length+1);
			}
			return "";
		}
		throw new ArgumentException("Path " + path + " does not start with the base path " + basePath);
	}
	
	/// <summary>
	/// Converts a full path to a relative path inside the currently open project. Converts all 
	/// paths in the given array. The conversion is done in-place and will modify the array.
	/// </summary>
	/// <param name='paths'>
	/// The paths to convert. Each path must be below the currently own project.
	/// </param>
	public static void FullPathToProjectPath(string[] paths) {
		for( int i = 0; i < paths.Length; i++ ) {
			paths[i] = FullPathToProjectPath(paths[i]);
		}
	}
	
	/// <summary>
	/// Converts a full path to a relative path inside the currently open project.
	/// </summary>
	/// <param name='paths'>
	/// The path to convert. Must be below the currently open project.
	/// </param>	
	public static string FullPathToProjectPath(string path) {
		var dataPath = UTFileUtils.ProjectRoot;
		if (path.StartsWith(dataPath) && path.Length > dataPath.Length) {
			return path.Substring(dataPath.Length+1);
		}
		if (path.StartsWith("Assets")) {
			return path;
		}
		throw new ArgumentException("Path is not inside the current project: " + path);
	}
	
	
	/// <summary>
	/// Modifies the given paths by stripping the old base dir from the paths and replacing it with
	/// the new base dir. If flatten is true, the paths below the old base dir are flattened so all
	/// files will be directly below the new base dir. The modification is done in-place and will modify
	/// the given array.
	/// </summary>
	/// <param name='paths'>
	/// The paths to modify.
	/// </param>
	/// <param name='oldBaseDir'>
	/// Old base dir.
	/// </param>
	/// <param name='newBaseDir'>
	/// New base dir.
	/// </param>
	/// <param name='flatten'>
	/// Flag telling whether or not the paths should be flattened.
	/// </param>
	public static string[] Repath(string[] paths, string oldBaseDir, string newBaseDir, bool flatten) {
		var result = new string[paths.Length];
		for(var i = 0; i < paths.Length; i++ ) {
			result[i] = Repath (paths[i], oldBaseDir, newBaseDir, flatten);
		}
		return result;
	}
	
	/// <summary>
	/// Modifies the given path by stripping the old base dir from the path and replacing it with
	/// the new base dir. If flatten is true, the path below the old base dir is flattened the file 
	/// will be directly below the new base dir. T
	/// </summary>
	/// <param name='path'>
	/// The path to modify.
	/// </param>
	/// <param name='oldBaseDir'>
	/// Old base dir.
	/// </param>
	/// <param name='newBaseDir'>
	/// New base dir.
	/// </param>
	/// <param name='flatten'>
	/// Flag telling whether or not the paths should be flattened.
	/// </param>
	/// <returns>the modified path.</returns>
	public static string Repath(string path, string oldBasedir, string newBaseDir, bool flatten) {
		if (path.StartsWith(oldBasedir)) {
			var relativePath = StripBasePath(path, oldBasedir);
			if (flatten) {
				if (relativePath.EndsWith("/")) {
					relativePath = relativePath.Substring(0, relativePath.Length -1);
				}
				var idx = relativePath.LastIndexOf("/");
				if (idx > -1 && !relativePath.EndsWith("/")) {
					relativePath = relativePath.Substring(idx+1);
				}
			}
			
			return newBaseDir + (newBaseDir.EndsWith("/") ? "" : "/") + relativePath;
		}
		throw new ArgumentException("Path " + path+ " does not start with " + oldBasedir);
	}
	
	/// <summary>
	/// Ensures that the parent folder of the given file name exists.
	/// </summary>
	/// <param name='fileName'>
	/// The file name.
	/// </param>
	public static void EnsureParentFolderExists(string fileName) {
		FileInfo info = new FileInfo(fileName);
		info.Directory.Create();	
	}
	
	
	/// <summary>
	/// Determines whether or not the file given in toCheck is below the directory given in root.
	/// </summary>
	/// <returns>
	/// <c>true</c> if the file is below root; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='root'>
	/// The root folder
	/// </param>
	/// <param name='toCheck'>
	/// The file to check.
	/// </param>
	public static bool IsBelow(string root, string toCheck) {
		DirectoryInfo rootInfo = new DirectoryInfo(root);
		DirectoryInfo toCheckInfo = new DirectoryInfo(toCheck);
		
		do {
			if (rootInfo.FullName.Equals(toCheckInfo.FullName)) {
				return true;
			}
			toCheckInfo = toCheckInfo.Parent;
		}
		while ( toCheckInfo != null);
		return false;
	}
	
	/// <summary>
	/// Gets the path to the parent directory of the given file name.
	/// </summary>
	/// <returns>
	/// The parent path.
	/// </returns>
	/// <param name='fileName'>
	/// The file name.
	/// </param>
	public static string GetParentPath(string fileName) {
		return NormalizeSlashes(new FileInfo(fileName).Directory.FullName);
	}
	
	/// <summary>
	/// Gets the current project's root folder
	/// </summary>
	/// <value>
	/// The project root.
	/// </value>
	public static string ProjectRoot {
		get {
			var dataDir = new DirectoryInfo(Application.dataPath);
			return NormalizeSlashes(dataDir.Parent.FullName);
		}
	}

	/// <summary>
	/// Gets the current project's settings folder.
	/// </summary>
	public static string ProjectSettings {
		get {
			var settingsDir = new DirectoryInfo(Application.dataPath);
			return CombineToPath(settingsDir.Parent.FullName, "ProjectSettings");
		}
	}

	/// <summary>
	/// Gets the current project's Assets folder.
	/// </summary>
	/// <value>
	/// The project's Assets folder.
	/// </value>
	public static string ProjectAssets {
		get {
			return NormalizeSlashes(Application.dataPath);
		}
	}
	
	/// <summary>
	/// Combines the given parts to a single path using forward slashes.
	/// </summary>
	/// <returns>
	/// The combined path.
	/// </returns>
	/// <param name='parts'>
	/// The parts to be combined.
	/// </param>
	public static string CombineToPath(params string[] parts) {
		string result = string.Join("/", parts);
		return NormalizeSlashes(result);
	}
	
	
	private static void AddToPaths(HashSet<string>paths, DirectoryInfo root, string selector, FileSelectionMode mode) {
		var selectedPaths = BuildSelector(root, selector, mode);
		paths.UnionWith(selectedPaths);
	}
	
	private static void RemoveFromPaths(HashSet<string>paths, DirectoryInfo root, string selector, FileSelectionMode mode) {
		var selectedPaths = BuildSelector(root, selector, mode);
		paths.ExceptWith(selectedPaths);
	}
	
	
	private static HashSet<string> BuildSelector(DirectoryInfo root, string selector, FileSelectionMode mode) {
		var result = new HashSet<string>();
		var parts = selector.Split(new string[]{"/"}, StringSplitOptions.RemoveEmptyEntries);
		var thisPart = parts[0];

		if (thisPart == "**") {
			// it's a super-wildcard.
			if (parts.Length == 1) {
				// we only got the super wildcard left, match anything in this folder and it's subfolders
				if (mode != FileSelectionMode.Folders) {
					var files = root.GetFiles();
					foreach(var file in files) {
						result.Add (file.FullName);
					}
				}
				if (mode != FileSelectionMode.Files) {
					var directories = root.GetDirectories();
					foreach(var directory in directories) {
						result.Add (directory.FullName);
					}
				}
			}
			else {
				// there is something behind the super-wildcard.
				// try to apply the thing behind the super wildcard
				// to this folder
				result.UnionWith(BuildSelector(root, string.Join("/", parts, 1, parts.Length-1), mode));
			}
			// recurse into subfolders
			var subfolders = root.GetDirectories();
			foreach(var dir in subfolders) {
				result.UnionWith(BuildSelector(dir, selector, mode));
			}
			return result;
		}
		else {
			// not a super wildcard.
			if (parts.Length == 1) {
				// last part, get the files that match
				if (mode != FileSelectionMode.Folders) {
					var files = root.GetFiles(thisPart);
					foreach(var file in files) {
						result.Add(file.FullName);
					}
				}
				if (mode != FileSelectionMode.Files) {
					var directories = root.GetDirectories(thisPart);
					foreach(var directory in directories) {
						result.Add (directory.FullName);
					}
				}
			}
			else {
				// there is something below, apply this to all matching directories
				var directories = root.GetDirectories(thisPart);
				foreach(var dir in directories) {
					result.UnionWith(BuildSelector(dir, string.Join("/", parts, 1, parts.Length-1), mode));
				}
			}
		}
		
		return result;
	}

	public enum FileSelectionMode {
		Files,
		Folders,
		FilesAndFolders
	}
}


