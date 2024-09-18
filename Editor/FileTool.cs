// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		FileTool.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/05 20:23:53
// *******************************************

using System.IO;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public static class FileTool
{
	public static void DirectoryCopy(string sourceDirName, string destDirName)
	{
		DirectoryInfo dir = new DirectoryInfo(sourceDirName);

		if (!dir.Exists)
			return;

		if (!Directory.Exists(destDirName))
		{
			Directory.CreateDirectory(destDirName);
		}

		FileInfo[] files = dir.GetFiles();

		foreach (FileInfo file in files)
		{
			// skip unity meta files
			Debug.Log("file.FullName===" + file.FullName);
			if(file.FullName.EndsWith(".meta"))
				continue;
			string temppath = Path.Combine(destDirName, file.Name);
			if (File.Exists(temppath)) File.Delete(temppath);
			file.CopyTo(temppath, false);
		}

		DirectoryInfo[] dirs = dir.GetDirectories();
		foreach (DirectoryInfo subdir in dirs)
		{
			string temppath = Path.Combine(destDirName, subdir.Name);
			DirectoryCopy(subdir.FullName, temppath);
		}
	}
}
