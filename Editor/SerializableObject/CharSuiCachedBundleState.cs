/*
 * Author: CharSui
 * Created On: 2025.09.28
 * Description: 基于原数据一样的结构，仅给必要的数据添加上可序列化的标记
 */

using System;

[Serializable]
public struct CharSuiCachedBundleState: IEquatable<CharSuiCachedBundleState>
{
	public string bundleFileId;
	public object data;


	public bool Equals(CharSuiCachedBundleState other)
	{
		return bundleFileId == other.bundleFileId && Equals(data.GetHashCode(), other.data.GetHashCode());
	}
}