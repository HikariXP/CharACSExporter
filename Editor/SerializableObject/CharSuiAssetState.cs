/*
 * Author: CharSui
 * Created On: 2025.09.28
 * Description: 基于原数据一样的结构，仅给必要的数据添加上可序列化的标记
 */

using System;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;

[Serializable]
public struct CharSuiAssetState : IEquatable<CharSuiAssetState>
{
	[SerializeField]
	public GUID guid;
	
	public string guidString;

	[SerializeField]
	public Hash128 hash;
	
	public string hashString;

	public override string ToString()
	{
		return $"Guid:{guid}, Hash128:{hash.ToString()}";
	}

	public bool Equals(CharSuiAssetState other)
	{
		return guid == other.guid && hash.Equals(other.hash);
	}

	public CharSuiAssetState Transform(AssetState assetState)
	{
		var answer = new CharSuiAssetState();
		answer.guid = assetState.guid;
		answer.hash = assetState.hash;
	
		answer.guidString = assetState.guid.ToString();
		answer.hashString = assetState.hash.ToString();
		
		return answer;
	}
}
