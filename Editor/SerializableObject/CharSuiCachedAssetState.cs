/*
 * Author: CharSui
 * Created On: 2025.09.28
 * Description: 基于原数据一样的结构，仅给必要的数据添加上可序列化的标记
 */

using System;
using UnityEngine;

[Serializable]
public struct CharSuiCachedAssetState
{
	/// <summary>
	/// The Asset State.
	/// </summary>
	public CharSuiAssetState asset;

	/// <summary>
	/// The Asset State of all dependencies.
	/// </summary>
	public CharSuiAssetState[] dependencies;

	/// <summary>
	/// The guid for the group the cached asset state belongs to.
	/// </summary>
	[SerializeField]
	public string groupGuid;

	/// <summary>
	/// The name of the cached asset states bundle file.
	/// </summary>
	[SerializeField]
	public string bundleFileId;

	/// <summary>
	/// The cached asset state data.
	/// </summary>
	[SerializeField]
	public object data;

	/// <summary>
	/// Checks if one cached asset state is equal to another given the asset state and dependency state.
	/// </summary>
	/// <param name="other">Right hand side of comparision.</param>
	/// <returns>Returns true if the cached asset states are equal to one another.</returns>
	public bool Equals(CharSuiCachedAssetState other)
	{
		bool result = asset.Equals(other.asset);
		result &= dependencies != null && other.dependencies != null;
		result &= dependencies.Length == other.dependencies.Length;
		var index = 0;
		while (result && index < dependencies.Length)
		{
			result &= dependencies[index].Equals(other.dependencies[index]);
			index++;
		}

		return result;
	}
}