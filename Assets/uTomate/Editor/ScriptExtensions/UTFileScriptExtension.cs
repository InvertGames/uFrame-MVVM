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
/// Script extension for handling of files and folders. All functions are available under the prefix <c>ut:file</c>.
/// </summary>
[UTScriptExtension("ut:file")]
public class UTFileScriptExtension
{
	/// <summary>
	/// Returns the filename of the given path.
	/// </summary>
	public string FileName (string path)
	{
		return Path.GetFileName (path);
	}
	
	/// <summary>
	/// Returns the filename of the given path without it's extension.
	/// </summary>
	public string FileNameWithoutExtension (string path)
	{
		return Path.GetFileNameWithoutExtension (path);
	}
	
	/// <summary>
	/// Returns the extension of the given path.
	/// </summary>
	public string FileExtension(string path) {
		return Path.GetExtension(path);
	}
	
	/// <summary>
	/// Converts the given full path to a path relative to the project's root folder.
	/// </summary>
	public string ToProjectRelativePath (string path)
	{
		return UTFileUtils.FullPathToProjectPath (path);
	}
	
	/// <summary>
	/// Converts the given full path to a path relative to the project's assets folder.
	/// </summary>
	public string ToAssetsRelativePath (string path)
	{
		return UTFileUtils.StripBasePath (path, Application.dataPath);		
	}
	
	/// <summary>
	/// Combines the given path segments into a path.
	/// </summary>
	public string CombinePath(params string[] parts) {
		return UTFileUtils.CombineToPath(parts);
	}
	
	/// <summary>
	/// Checks if the file or directory specified by the given path exists.
	/// </summary>
	public bool Exists(string path) {
		return File.Exists(path) || Directory.Exists(path);	
	}
	
	/// <summary>
	/// Determines whether the file identified by source is newer than the file identified by target. If the
	/// target file does not exist, this will always return true.
	/// </summary>
	public bool IsOutOfDate (string source, string target)
	{
		if (!File.Exists (source)) {
			throw new ArgumentException ("The source file  " + source + " does not exist.");
		}
		return File.GetLastWriteTime (source).CompareTo (File.GetLastWriteTime (target)) > 0;
	}
	
	/// <summary>
	/// Builds a file set.
	/// </summary>
	public string[] BuildFileSet(string baseFolder, string[] includes, string[] excludes, bool includeDirectories) {
		return UTFileUtils.CalculateFileset(baseFolder, includes, excludes, 
		                                    includeDirectories ? UTFileUtils.FileSelectionMode.FilesAndFolders : UTFileUtils.FileSelectionMode.Files);
	}

	/// <summary>
	/// Builds a file set.
	/// </summary>
	public string[] BuildFileSet(string baseFolder, string[] includes, string[] excludes, bool includeDirectories, bool includeFiles) {
		if (!includeDirectories && !includeFiles) {
			throw new ArgumentException("Please set at least one of the includeDirectories and includeFiles parameters to true.");
		}

		UTFileUtils.FileSelectionMode mode;
		if (includeDirectories) {
			if (includeFiles) {
				mode = UTFileUtils.FileSelectionMode.FilesAndFolders;
			}
			else {
				mode = UTFileUtils.FileSelectionMode.Folders;
			}
		}
		else {
			mode = UTFileUtils.FileSelectionMode.Files;
		}

		return UTFileUtils.CalculateFileset(baseFolder, includes, excludes, mode);
	}

}

