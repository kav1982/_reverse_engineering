using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Relic_ShowUnitHP : IComponentData, IQueryTypeParameter
{
	public Entity ett_Hight;

	public Entity ett_HPBarRoot;

	public Entity ett_HPBar_Monster;

	public Entity ett_HPBar_Teammate;

	[Header("Text")]
	public Entity ett_TextRoot;

	public Entity ett_CurrentHP;

	public Entity ett_MaxHP;

	public float2 oneNumberScale;

	public float hpOffset;

	public bool isInitialized;

	public Entity unitEtt;

	public int relicLevel;

	public float lastRecordCurrentHP;

	public float lastRecordMaxHP;

	public bool onTeammateTransparentChange;

	public float teammateTransparent;

	public void Initialized(Entity unitEtt, int relicLevel)
	{
		isInitialized = false;
		this.unitEtt = unitEtt;
		this.relicLevel = relicLevel;
		onTeammateTransparentChange = true;
		teammateTransparent = DataMgr.settingData.SummonTransparent;
	}

	public void SetTeammateTransparentChange()
	{
		onTeammateTransparentChange = true;
		teammateTransparent = DataMgr.settingData.SummonTransparent;
	}
}
