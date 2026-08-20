using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObj222_GameGrabDoll : SpecialObj222_GameBase
{
	public enum rewardType
	{
		SpellCommonlv1,
		SpellCommonlv2,
		SpellCommonlv3,
		SpellRarelv1,
		SpellRarelv2,
		SpellRarelv3,
		SpellEpic,
		SpellSpecial,
		RelicCommon,
		RelicSpecial,
		RelicRare,
		RelicEpic,
		Curse,
		Chest,
		Coin,
		Dimond,
		Potion,
		Wand
	}

	[Serializable]
	public class Special22201Reward
	{
		public rewardType rewardtype;

		public int rewardid;

		public int positionid;

		public GameObject obj;
	}

	public SpecialObj222 gameHolder222;

	public SpecialObj222_PayInteract PayInteract;

	public GameObject pfb_Ball;

	public GameObject Grabber;

	public GameObject pfb_GrabDollPrefab;

	public GameObject Hand;

	public Transform ThrowBall;

	public Transform DropRewardTransform;

	public Transform DropRewardTransformFlip;

	public Cable cable;

	public Transform Reward;

	public float RotateSpeed;

	public float ReachSpeed;

	public float maxlength;

	public float CastSphereRadius;

	public Coroutine _ieShoot;

	public float LaunghchSpeed;

	public float moveRange;

	private bool _rotateRight;

	public float rotateSpeed;

	public float rotateRange;

	[Header("Sprite")]
	public Sprite Chest;

	public Sprite Coin;

	public Sprite Dimond;

	[Header("Reward")]
	public List<Special22201Reward> rewards = new List<Special22201Reward>();

	public int rewardnum;

	public List<int> relicid = new List<int>();

	public float interval;

	public GameObject SpellIcon;

	public GameObject SpellIconStar;

	public GameObject SpellIconBackground;

	public Vector2 StarPositionOffset;

	public Vector2 SecondStarPositionOffset;

	public float SpriteSize = 10f;

	public Vector3 nextDropPoint;

	[Header("具体掉落")]
	public int rewardSpellnumCommonlv2;

	public int rewardSpellnumCommonlv3;

	public int rewardSpellnumRarelv1;

	public int rewardSpellnumRarelv0;

	public int rewardSpellEpic;

	public int rewardRelicnumCommon;

	public int rewardRelicnumRare;

	public int rewardCoin;

	public int rewardDimond;

	public int rewardCurse;

	public int rewardChest;

	public int rewardPotion;

	public int numberOfObjects => rewardSpellnumCommonlv2 + rewardSpellnumCommonlv3 + rewardSpellnumRarelv1 + rewardSpellnumRarelv0 + rewardRelicnumCommon + rewardRelicnumRare + rewardCurse + rewardChest + rewardCoin + rewardDimond + rewardSpellEpic + rewardPotion;

	public override void Initialize()
	{
		base.Initialize();
		GetRewards();
		GenerateRewards();
	}

	public override void DirectionControl(Vector2 vector2)
	{
	}

	public override void InteractControl()
	{
		SEMgr.Inst.so222Shoot.PlaySE();
		GameObject obj = UnityEngine.Object.Instantiate(pfb_Ball, base.transform);
		obj.SetActive(value: true);
		obj.transform.position = ThrowBall.position;
		obj.GetComponent<Rigidbody>().linearVelocity = Hand.transform.up * -5f;
		obj.GetComponent<SpecialObj222_BallCollision>().game222 = this;
		GetNextDropPoint();
	}

	public void GetNextDropPoint()
	{
		nextDropPoint = Tool2D.GetNavMeshPointIngoreZ(DropRewardTransform.position, 1f);
		if (!gameHolder222.roomCtrller.roomCfg.isFlipped)
		{
			nextDropPoint = Tool2D.GetNavMeshPointIngoreZ(DropRewardTransformFlip.position, 1f);
		}
	}

	public override void BackControl()
	{
	}

	public override void Update()
	{
		float num = Hand.gameObject.transform.localRotation.eulerAngles.z;
		if (num > 180f)
		{
			num = -360f + num;
		}
		if (!_rotateRight)
		{
			if (num > 0f - rotateRange)
			{
				Hand.gameObject.transform.Rotate(new Vector3(0f, 0f, (0f - rotateSpeed) * Time.deltaTime), Space.World);
			}
			else
			{
				_rotateRight = true;
			}
		}
		else if (num < rotateRange)
		{
			Hand.gameObject.transform.Rotate(new Vector3(0f, 0f, rotateSpeed * Time.deltaTime), Space.World);
		}
		else
		{
			_rotateRight = false;
		}
	}

	public IEnumerator ieShoot()
	{
		yield return new WaitForEndOfFrame();
	}

	public void GenerateRewards()
	{
		for (int i = 0; i < rewards.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(pfb_GrabDollPrefab, Reward);
			gameObject.SetActive(value: true);
			gameObject.transform.position = Reward.transform.position + rewards[i].positionid * new Vector3(interval, 0f, 0f);
			GenerateReward(rewards[i], Reward.transform.position + rewards[i].positionid * new Vector3(interval, 0f, 0f), gameObject.transform);
		}
	}

	private void GenerateReward(Special22201Reward dropItem, Vector3 position, Transform newreward)
	{
		newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().rewardType = dropItem.rewardtype;
		switch (dropItem.rewardtype)
		{
		case rewardType.SpellCommonlv1:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Common.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.SpellCommonlv2:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			InitOneStar();
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Common.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.SpellCommonlv3:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			InitTwoStar();
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Common.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.SpellEpic:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x - 0.02f, position.y - 0.25f, newreward.position.z - 0.03f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Rare.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.SpellRarelv1:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Rare.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.SpellRarelv2:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			InitOneStar();
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Rare.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.SpellRarelv3:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			InitTwoStar();
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Rare.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.RelicCommon:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(RelicConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Common.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.RelicRare:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(RelicConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y - 0.25f, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Rare.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.Curse:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(CurseConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Common.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.Dimond:
		{
			Sprite sprite = Dimond;
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width * 0.7f, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width * 0.7f, 1f);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Common.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.Coin:
		{
			Sprite sprite = Coin;
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.positionid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width * 0.7f, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width * 0.7f, 1f);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Common.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.Chest:
		{
			Sprite sprite = Chest;
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			dropItem.obj = gameObject;
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Common.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.Potion:
		{
			Sprite sprite = ABResources.LoadAsset<Sprite>(PotionConfig.dic[dropItem.rewardid].GetIconPath());
			SpellIcon.GetComponent<SpriteRenderer>().sprite = sprite;
			SpellIcon.name = dropItem.rewardid.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(SpellIcon, new Vector3(position.x, position.y + 0.05f, newreward.position.z - 0.01f), Quaternion.identity, newreward);
			gameObject.transform.localScale = new Vector3(SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, SpriteSize * sprite.pixelsPerUnit / sprite.rect.width, 1f);
			dropItem.obj = gameObject;
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().Common.SetActive(value: true);
			newreward.transform.GetChild(1).GetComponent<SpecialObj222_GrabDollReward>().id = dropItem.rewardid;
			break;
		}
		case rewardType.SpellSpecial:
		case rewardType.RelicSpecial:
		case rewardType.RelicEpic:
			break;
		}
		void InitOneStar()
		{
			UnityEngine.Object.Instantiate(SpellIconStar, new Vector3(position.x + StarPositionOffset.x, position.y + StarPositionOffset.y, tsf_Layer.transform.position.z - 0.05f), Quaternion.identity, newreward);
		}
		void InitTwoStar()
		{
			UnityEngine.Object.Instantiate(SpellIconStar, new Vector3(position.x + StarPositionOffset.x - 0.3f, position.y + StarPositionOffset.y, tsf_Layer.transform.position.z - 0.05f), Quaternion.identity, newreward);
			UnityEngine.Object.Instantiate(SpellIconStar, new Vector3(position.x + SecondStarPositionOffset.x, position.y + SecondStarPositionOffset.y, tsf_Layer.transform.position.z - 0.05f), Quaternion.identity, newreward);
		}
	}

	public void OnDestroy()
	{
		foreach (Special22201Reward reward in rewards)
		{
			switch (reward.rewardtype)
			{
			case rewardType.RelicCommon:
			case rewardType.RelicRare:
				if (PlayerMgr.Inst.BaData != null)
				{
					PlayerMgr.Inst.BaData.BackRelicToPool(reward.rewardid, 1);
				}
				break;
			case rewardType.Curse:
				if (PlayerMgr.Inst.BaData != null)
				{
					PlayerMgr.Inst.BaData.BackCurseToPool(reward.rewardid, 1);
				}
				break;
			}
		}
	}

	public void CreatNewReward(int positionid, rewardType reward)
	{
		StartCoroutine(IeCreatNewReward(positionid, reward));
	}

	private IEnumerator IeCreatNewReward(int positionid, rewardType reward)
	{
		yield return new WaitForSecondsRealtime(2f);
		rewards.Add(AddReward(positionid, reward));
		GameObject gameObject = UnityEngine.Object.Instantiate(pfb_GrabDollPrefab, Reward);
		gameObject.SetActive(value: true);
		gameObject.transform.position = Reward.transform.position + rewards[rewards.Count - 1].positionid * new Vector3(interval, 0f, 0f);
		_ = rewards[rewards.Count - 1].rewardid;
		GenerateReward(rewards[rewards.Count - 1], Reward.transform.position + rewards[rewards.Count - 1].positionid * new Vector3(interval, 0f, 0f), gameObject.transform);
	}

	private int GetDropID(rewardType rewardType)
	{
		switch (rewardType)
		{
		case rewardType.SpellCommonlv1:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common);
		case rewardType.SpellCommonlv2:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Common);
		case rewardType.SpellCommonlv3:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(3, ItemDropType.Common);
		case rewardType.SpellRarelv1:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Rare);
		case rewardType.SpellRarelv2:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(2, ItemDropType.Rare);
		case rewardType.SpellRarelv3:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(3, ItemDropType.Rare);
		case rewardType.SpellEpic:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Epic);
		case rewardType.RelicCommon:
			return PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Common);
		case rewardType.RelicRare:
			return PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Rare);
		case rewardType.Curse:
			return PlayerMgr.Inst.BaData.GetCurseFromPool(ItemDropType.Common);
		case rewardType.Chest:
			return PlayerMgr.Inst.BaData.GetCurseFromPool(ItemDropType.Common);
		case rewardType.Potion:
			return PlayerMgr.Inst.BaData.GetPotionFromPool();
		default:
			return PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Common);
		case rewardType.Coin:
		case rewardType.Dimond:
			return 1;
		}
	}

	private Special22201Reward AddReward(int i, rewardType rewardType)
	{
		return new Special22201Reward
		{
			rewardtype = rewardType,
			rewardid = GetDropID(rewardType),
			positionid = i
		};
	}

	private void GetRewards()
	{
		int num = rewardSpellnumCommonlv2;
		int num2 = rewardSpellnumCommonlv3;
		int num3 = rewardSpellnumRarelv1;
		int num4 = rewardSpellnumRarelv0;
		int num5 = rewardSpellEpic;
		int num6 = rewardCoin;
		int num7 = rewardDimond;
		int num8 = rewardRelicnumCommon;
		int num9 = rewardRelicnumRare;
		int num10 = rewardCurse;
		int num11 = rewardChest;
		int num12 = rewardPotion;
		List<int> list = new List<int>();
		rewards.Clear();
		for (int i = 0; i < numberOfObjects; i++)
		{
			list.Add(i);
		}
		while (num > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index], rewardType.SpellCommonlv2));
			list.Remove(list[index]);
			num--;
		}
		while (num2 > 0)
		{
			int index2 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index2], rewardType.SpellCommonlv3));
			list.Remove(list[index2]);
			num2--;
		}
		while (num3 > 0)
		{
			int index3 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index3], rewardType.SpellRarelv2));
			list.Remove(list[index3]);
			num3--;
		}
		while (num4 > 0)
		{
			int index4 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index4], rewardType.SpellRarelv1));
			list.Remove(list[index4]);
			num4--;
		}
		while (num8 > 0)
		{
			int index5 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index5], rewardType.RelicCommon));
			list.Remove(list[index5]);
			num8--;
		}
		while (num9 > 0)
		{
			int index6 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index6], rewardType.RelicRare));
			list.Remove(list[index6]);
			num9--;
		}
		while (num11 > 0)
		{
			int index7 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index7], rewardType.Chest));
			list.Remove(list[index7]);
			num11--;
		}
		while (num10 > 0)
		{
			int index8 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index8], rewardType.Curse));
			list.Remove(list[index8]);
			num10--;
		}
		while (num7 > 0)
		{
			int index9 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index9], rewardType.Dimond));
			list.Remove(list[index9]);
			num7--;
		}
		while (num6 > 0)
		{
			int index10 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index10], rewardType.Coin));
			list.Remove(list[index10]);
			num6--;
		}
		while (num5 > 0)
		{
			int index11 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index11], rewardType.SpellEpic));
			list.Remove(list[index11]);
			num5--;
		}
		while (num12 > 0)
		{
			int index12 = UnityEngine.Random.Range(0, list.Count);
			rewards.Add(AddReward(list[index12], rewardType.Potion));
			list.Remove(list[index12]);
			num12--;
		}
	}

	public override void SetRoomCtrlller(RoomController roomCtrller)
	{
		base.SetRoomCtrlller(roomCtrller);
	}
}
