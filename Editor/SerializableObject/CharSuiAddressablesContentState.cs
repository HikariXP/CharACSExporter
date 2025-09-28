/*
 * Author: CharSui
 * Created On: 2025.09.28
 * Description: 基于原数据一样的结构，仅给必要的数据添加上可序列化的标记
 */

using System;
using UnityEngine;

[Serializable]
public struct CharSuiAddressablesContentState
{
	[SerializeField]
	public string playerVersion;
	
	[SerializeField]
	public string editorVersion;
	
	[SerializeField]
	public CharSuiCachedAssetState[] cachedInfos;
	
	[SerializeField]
	public string remoteCatalogLoadPath;
	
	[SerializeField]
	public CharSuiCachedBundleState[] cachedBundles;
}