using Unity.Entities;
using UnityEngine;

public struct LevelReward : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_Hover;

	public float hoverSpeed;

	public float hoverRange;

	[Header("Outline")]
	public Entity ett_Outline_Reward0;

	public Entity ett_Outline_Reward1;

	public Entity ett_Outline_Reward2;

	public Entity ett_Outline_Reward3;

	public Entity ett_Outline_Reward4;

	public Entity ett_Outline_Reward5;

	public Entity ett_Outline_Reward6;

	public Entity ett_Outline_Reward7;

	public Entity ett_Outline_Reward8;

	public Entity ett_Outline_Reward100;

	public Entity ett_Outline_Reward101;

	public Entity ett_Outline_Reward131;

	public Entity ett_Outline_Reward200;

	[Header("Icon")]
	public Entity ett_Icon_Reward0;

	public Entity ett_Icon_Reward1;

	public Entity ett_Icon_Reward2;

	public Entity ett_Icon_Reward3;

	public Entity ett_Icon_Reward4;

	public Entity ett_Icon_Reward5;

	public Entity ett_Icon_Reward6;

	public Entity ett_Icon_Reward7;

	public Entity ett_Icon_Reward8;

	public Entity ett_Icon_Reward100;

	public Entity ett_Icon_Reward101;

	public Entity ett_Icon_Reward131;

	public Entity ett_Icon_Reward200;

	public bool isInitialized;

	public float hoverTimer;

	public UnityObjectRef<GameObject> go_EF;

	public LevelRewardType rewardType;

	public int spellPickCounter;

	public int relicPickCounter;

	public int rerollTimer;

	public bool isUse;

	public bool isUsePlayerSE;

	public void UIUse(bool playerSE = true)
	{
		isUse = true;
		isUsePlayerSE = playerSE;
	}
}
