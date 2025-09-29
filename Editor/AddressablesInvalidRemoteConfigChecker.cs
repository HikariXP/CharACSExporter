/*
 * Copyright (c) PeroPeroGames Co., Ltd.
 * Author: CharSui
 * Created On: #CREATE_TIME#
 * Description: 
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class AddressablesInvalidRemoteConfigChecker
{
	[MenuItem("CharTools/Addressables/CheckInvlidRemoteConfig", false, 154)]
	public static void Excute()
	{
		var folderPath = "Assets/AddressableAssetsData/AssetGroups/";
		var settingsPath = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";

		var settings = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(settingsPath);

		if (!Directory.Exists(folderPath))
		{
			Debug.LogError($"Directory does not exist: {folderPath}");
			return;
		}

		// 获取文件夹的相对路径（相对于Assets文件夹）
		var relativePath = folderPath.Replace(Application.dataPath, "Assets");

		// 获取文件夹下的所有资源GUID
		var guids = AssetDatabase.FindAssets("", new[] { relativePath });
		
		var count = 0;

		foreach (var guid in guids)
		{
			var assetPath = AssetDatabase.GUIDToAssetPath(guid);
			
			EditorUtility.DisplayProgressBar("CheckGroup",$"正在检查资源组:{assetPath}",(float)count/guids.Length);

			// 跳过子文件夹
			if (IsSubDirectory(assetPath, relativePath))
			{
				continue;
			}

			// 加载资源
			var group = AssetDatabase.LoadAssetAtPath<AddressableAssetGroup>(assetPath);
			if (group == null)
			{
				// Debug.LogError($"There are some null group with assetPath : {assetPath}");
				continue;
			}
			
			// 跳过Assets/AddressableAssetsData/AssetGroups/Built In Data.asset
			if(group.Name.StartsWith("Built In Data"))
				continue;

			if (group.GetSchema<ContentUpdateGroupSchema>() == null ||
			    group.GetSchema<BundledAssetGroupSchema>() == null)
			{
				Debug.LogError($"There are some group with wrong schema at assetPath : {assetPath}");
				continue;
			}
			
			var bags =  group.GetSchema<BundledAssetGroupSchema>();
			var bagsLoadPath = bags.LoadPath.GetName(settings);
			

			if (bagsLoadPath == AddressableAssetSettings.kLocalLoadPath)
			{
				group.GetSchema<ContentUpdateGroupSchema>().StaticContent = true;
			}
			else
			{
				group.GetSchema<ContentUpdateGroupSchema>().StaticContent = false;
			}
		}
		
		EditorUtility.ClearProgressBar();
	}

	// 检查路径是否是子文件夹
	private static bool IsSubDirectory(string assetPath, string parentFolderPath)
	{
		var directory = Path.GetDirectoryName(assetPath);
		return directory != parentFolderPath && directory.StartsWith(parentFolderPath);
	}
}
