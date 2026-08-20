using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct DoorBase_Dots : IComponentData, IQueryTypeParameter
{
	public float3 refreshEFOffset;

	[Header("Reward")]
	public Entity ett_Reward0;

	public Entity ett_Reward1;

	public Entity ett_Reward2;

	public Entity ett_Reward3;

	public Entity ett_Reward4;

	public Entity ett_Reward5;

	public Entity ett_Reward6;

	public Entity ett_Reward7;

	public Entity ett_Reward100;

	public Entity ett_Reward101;

	public Entity ett_Reward131;

	public Entity ett_Reward200;

	[Header("Door")]
	public Entity ett_Portal;

	public Entity ett_Door;

	public Entity ett_DoorRuined;

	[Header("OpenDoor")]
	public Entity ett_Door2;

	public float openDoorSpeed;

	public float openDoorFinalOffsetY;

	public LevelRewardType rewardType;

	public bool isExtraDoor;

	public Entity ett_CurrentDisplayReward;

	public bool onRefreshType;

	public bool onUpdateDisplay;

	public bool onOpen;

	public bool onOpenDirect;

	public bool isOpening;

	public float doorCurrentOffsetY;

	public void RefreshType(LevelRewardType newType)
	{
		if (rewardType != LevelRewardType.Ruined && rewardType != LevelRewardType.Chapter)
		{
			rewardType = newType;
			onRefreshType = true;
			onUpdateDisplay = true;
		}
	}
}
