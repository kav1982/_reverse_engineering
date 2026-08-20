using Unity.Mathematics;
using UnityEngine;

public class Door_T8Mono : MonoBehaviour
{
	public GameObject go_Outline;

	public Transform tsf_Layer;

	public Transform tsf_LayerBase;

	public Animator anima;

	public MeshRenderer mr_Base;

	public MeshRenderer mr_Base2;

	public SpriteRenderer sr_Reward;

	public Texture tex_Base;

	public Texture tex_Base2;

	public Texture tex_BaseRuined;

	public Texture tex_BaseH;

	public Texture tex_Base2H;

	public Texture tex_BaseRuinedH;

	public void UpdateDisplay(LevelRewardType rewardType)
	{
		Transform obj = tsf_Layer;
		float3 rootPosition = base.transform.position;
		obj.localPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
		Transform obj2 = tsf_LayerBase;
		rootPosition = base.transform.position;
		obj2.localPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.AccessOpen);
		int num = (int)rewardType;
		string text = "Textures/LevelReward/" + num;
		num = (int)rewardType;
		string nameH = "Textures/LevelReward/" + num + "H";
		if (rewardType == LevelRewardType.Ruined)
		{
			if (GameMgr.IsHarmony_Static)
			{
				mr_Base.material.SetTexture("_BaseMap", tex_BaseRuinedH);
			}
			else
			{
				mr_Base.material.SetTexture("_BaseMap", tex_BaseRuined);
			}
			return;
		}
		if (GameMgr.IsHarmony_Static)
		{
			mr_Base.material.SetTexture("_BaseMap", tex_BaseH);
			mr_Base2.material.SetTexture("_BaseMap", tex_Base2H);
		}
		else
		{
			mr_Base.material.SetTexture("_BaseMap", tex_Base);
			mr_Base2.material.SetTexture("_BaseMap", tex_Base2);
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_IsInvisibleDoor)
		{
			sr_Reward.sprite = ABResources.LoadAsset<Sprite>("Textures/LevelReward/" + 200);
		}
		else
		{
			sr_Reward.sprite = ABResources.LoadHarmonizableAsset<Sprite>(text, nameH);
		}
	}

	public void Open()
	{
		anima.Play("Open", 0, 0f);
	}

	public void OpenDirect()
	{
		anima.Play("OpenDirect", 0, 0f);
	}
}
