/*
 * Author: CharSui
 * Created On: 2025.09.28
 * Description: 点击后选择一个AddressableContentAsset，然后点击导出，将其导出为CharSuiAddressablesContentState.json保存
 */


using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;

public class CharACSExporter
{
	[MenuItem("Tools/CharSui/Export AddressableContentState to JSON")]
	public static void Export()
	{ 
		var acsPath = EditorUtility.OpenFilePanel("Select AddressableContentAsset", "Assets", "bin");
		if(string.IsNullOrEmpty(acsPath))return;
		
		EditorUtility.DisplayProgressBar(nameof(CharACSExporter), "Loading AddressablesContentState...", 0.0f);
		
		var acs = LoadAddressablesContentState(acsPath);

		if (acs == null)
		{
			EditorUtility.ClearProgressBar();
			return;
		}
		
		EditorUtility.DisplayProgressBar(nameof(CharACSExporter), "Converting...", 0.0f);
		var canConvert = TryConvert(acs, out var convertedAcs);
		if(!canConvert)
		{
			EditorUtility.ClearProgressBar();
			Debug.LogError("Failed to convert Addressables content state");
			return;
		}
		
		EditorUtility.DisplayProgressBar(nameof(CharACSExporter), "SerializeToJson...", 0.0f);
		var json = Newtonsoft.Json.JsonConvert.SerializeObject(convertedAcs, Newtonsoft.Json.Formatting.Indented);
		var savePath = EditorUtility.SaveFilePanel("Save AddressablesContentState", "Assets", "CharSuiAddressablesContentState.json", "json");
		if (string.IsNullOrEmpty(savePath))
		{
			EditorUtility.ClearProgressBar();
			return;
		}

		EditorUtility.ClearProgressBar();
		File.WriteAllText(savePath, json);
		Debug.Log("Exported Success, Path:" + savePath);
	}
	
	public static AddressablesContentState LoadAddressablesContentState(string filePath)
	{
		if (!File.Exists(filePath))
		{
			Debug.LogError($"File not found: {filePath}");
			return null;
		}

		using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
		{
			var formatter = new BinaryFormatter();
			var cacheData = formatter.Deserialize(stream) as AddressablesContentState;

			if (cacheData == null)
			{
				Debug.LogError("Failed to deserialize Addressables content state");
				return null;
			}
			return cacheData;
		}
	}
	
	/// <summary>
	/// AddressablesContentState(ACS)是Addressable的清单文件，热更资源更新的记录清单就在此地。
	/// 你只需要通过资源加载方式加载到ACS，传进来做一层转义就行。
	/// </summary>
	/// <param name="acs"></param>
	/// <returns></returns>
	public static bool TryConvert(AddressablesContentState acs, out CharSuiAddressablesContentState result)
	{
		if (acs == null)
		{
			result = default;
			return false;
		}
		
		result = new CharSuiAddressablesContentState
		{
			// basic info
			playerVersion = acs.playerVersion,
			editorVersion = acs.editorVersion,
			remoteCatalogLoadPath = acs.remoteCatalogLoadPath,
			cachedInfos = new CharSuiCachedAssetState[acs.cachedInfos.Length]
		};

		for(int i = 0;i < acs.cachedInfos.Length; i ++)
		{
			result.cachedInfos[i] = Convert(acs.cachedInfos[i]);
		}
		
		result.cachedBundles = new CharSuiCachedBundleState[acs.cachedBundles.Length];
		for(int i = 0;i < acs.cachedBundles.Length; i ++)
		{
			result.cachedBundles[i] = Convert(acs.cachedBundles[i]);
		}
		
		return true;
	}

	/// <summary>
	/// CCB
	/// 将Unity的没马资源管理替换成狗屎
	/// </summary>
	/// <param name="cas"></param>
	/// <returns></returns>
	private static CharSuiCachedAssetState Convert(CachedAssetState cas)
	{
		if(cas == null)return default;
		
		// 处理Asset部分
		var ccas = new CharSuiCachedAssetState();
		ccas.asset = new CharSuiAssetState().Transform(cas.asset);
		
		// 处理依赖部分
		var dependenciesCount = cas.dependencies.Length;
		ccas.dependencies = new CharSuiAssetState[dependenciesCount];
		for(int i = 0;i < dependenciesCount; i ++)
		{
			ccas.dependencies[i] = new CharSuiAssetState().Transform(cas.dependencies[i]);
		}
		
		ccas.groupGuid = cas.groupGuid;
		ccas.bundleFileId = cas.bundleFileId;
		ccas.data = cas.data;

		return ccas;
	}
	
	private static CharSuiCachedBundleState Convert(CachedBundleState cbs)
	{
		if(cbs == null)return default;
		
		var ccs = new CharSuiCachedBundleState
		{
			bundleFileId = cbs.bundleFileId,
			data = cbs.data
		};

		return ccs;
	}
}
