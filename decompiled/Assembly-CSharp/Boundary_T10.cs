using UnityEngine;

public class Boundary_T10 : BoundaryBase
{
	[Space(50f)]
	public SpriteRenderer sr;

	public SpriteRenderer sr_AO;

	[Header("Wall")]
	public Sprite[] sprite_Corner_RD;

	public Sprite[] sprite_Corner_UR;

	public Sprite[] sprite_LUR;

	public Sprite[] sprite_RD;

	public Sprite[] sprite_RDL;

	public Sprite[] sprite_UR;

	public Sprite[] sprite_URD;

	[Header("AO")]
	public Sprite[] sprite_AO_LUR;

	public Sprite[] sprite_AO_RD;

	public Sprite[] sprite_AO_RDL;

	public Sprite[] sprite_AO_UR;

	public Sprite[] sprite_AO_URD;

	[Header("Detail")]
	[Range(0f, 1f)]
	public float detailChange;

	public int detailMinInterval;

	public GameObject[] pfb_Detail_LUR;

	public GameObject[] pfb_Detail_RDL;

	public GameObject[] pfb_Detail_URD;

	[Range(0f, 1f)]
	[Header("OuterBoundary")]
	public float storeDetailChance;

	public GameObject pfb_Store;

	[Range(0f, 1f)]
	public float potionDetailChance;

	public GameObject pfb_Potion;

	[Range(0f, 1f)]
	public float processDetailChance;

	public GameObject pfb_Process;

	[Range(0f, 1f)]
	public float moreInOneDetailChance;

	public GameObject pfb_MoreInOne;

	private void RandomDetail(Vector2Data selfPoint, RoomController roomCtrller, GameObject[] prefabs, bool flip = false)
	{
		if (LevelMgr.Inst.NextRewardTypes != null && ((LevelMgr.Inst.NextRewardTypes.Count >= 1 && (selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(-1f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(0f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(1f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(2f, 1f))) || (LevelMgr.Inst.NextRewardTypes.Count >= 2 && (selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(-3f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(-2f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(3f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(4f, 1f))) || (LevelMgr.Inst.NextExtraDoorRewardType != LevelRewardType.None && roomCtrller.roomCfg.extraDoor != Vector2Data.Up1000 && (selfPoint == roomCtrller.roomCfg.extraDoor + new Vector2Data(-1f, 1f) || selfPoint == roomCtrller.roomCfg.extraDoor + new Vector2Data(0f, 1f) || selfPoint == roomCtrller.roomCfg.extraDoor + new Vector2Data(1f, 1f) || selfPoint == roomCtrller.roomCfg.extraDoor + new Vector2Data(2f, 1f)))))
		{
			return;
		}
		bool flag = true;
		for (int i = -detailMinInterval; i <= detailMinInterval; i++)
		{
			for (int j = -detailMinInterval; j <= detailMinInterval; j++)
			{
				if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(i, j)) && roomCtrller.boundaryBase1Dic[selfPoint + new Vector2Data(i, j)].HaveDetail)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag && Random.value <= detailChange)
		{
			base.HaveDetail = true;
			GameObject gameObject = Object.Instantiate(prefabs[Random.Range(0, prefabs.Length)], base.transform.position, Quaternion.identity, base.transform.parent);
			if (flip)
			{
				gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
	}

	public override void Correct(Vector2Data selfPoint, RoomController roomCtrller)
	{
		sr_AO.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.BoundaryAO) + new Vector3(0f, 0f, Random.Range(-0.1f, 0.1f));
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(0f, 1f)))
		{
			flag = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(0f, 1f)))
		{
			flag = true;
		}
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(1f, 1f)))
		{
			flag2 = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(1f, 1f)))
		{
			flag2 = true;
		}
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(1f, 0f)))
		{
			flag3 = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(1f, 0f)))
		{
			flag3 = true;
		}
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(1f, -1f)))
		{
			flag4 = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(1f, -1f)))
		{
			flag4 = true;
		}
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(0f, -1f)))
		{
			flag5 = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(0f, -1f)))
		{
			flag5 = true;
		}
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(-1f, -1f)))
		{
			flag6 = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(-1f, -1f)))
		{
			flag6 = true;
		}
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(-1f, 0f)))
		{
			flag7 = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(-1f, 0f)))
		{
			flag7 = true;
		}
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(-1f, 1f)))
		{
			flag8 = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(-1f, 1f)))
		{
			flag8 = true;
		}
		if (flag && flag3 && !flag5 && !flag7)
		{
			sr.sprite = sprite_UR[Random.Range(0, sprite_UR.Length)];
			sr.transform.position += new Vector3(0f, 0f, -0.01f);
			sr_AO.sprite = sprite_AO_UR[Random.Range(0, sprite_AO_UR.Length)];
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_RD[Random.Range(0, sprite_RD.Length)];
			sr_AO.sprite = sprite_AO_RD[Random.Range(0, sprite_AO_RD.Length)];
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_RD[Random.Range(0, sprite_RD.Length)];
			sr.flipX = true;
			sr_AO.sprite = sprite_AO_RD[Random.Range(0, sprite_AO_RD.Length)];
			sr_AO.flipX = true;
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr.transform.position += new Vector3(0f, 0f, -0.01f);
			sr.sprite = sprite_UR[Random.Range(0, sprite_UR.Length)];
			sr.flipX = true;
			sr_AO.sprite = sprite_AO_UR[Random.Range(0, sprite_AO_UR.Length)];
			sr_AO.flipX = true;
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_LUR[Random.Range(0, sprite_LUR.Length)];
			sr_AO.sprite = sprite_AO_LUR[Random.Range(0, sprite_AO_LUR.Length)];
			RandomDetail(selfPoint, roomCtrller, pfb_Detail_LUR);
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_URD[Random.Range(0, sprite_URD.Length)];
			sr_AO.sprite = sprite_AO_URD[Random.Range(0, sprite_AO_URD.Length)];
			RandomDetail(selfPoint, roomCtrller, pfb_Detail_URD);
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_RDL[Random.Range(0, sprite_RDL.Length)];
			sr_AO.sprite = sprite_AO_RDL[Random.Range(0, sprite_AO_RDL.Length)];
			RandomDetail(selfPoint, roomCtrller, pfb_Detail_RDL);
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_URD[Random.Range(0, sprite_URD.Length)];
			sr.transform.rotation = Tool2D.GetRotation(180f);
			sr_AO.sprite = sprite_AO_URD[Random.Range(0, sprite_AO_URD.Length)];
			sr_AO.flipX = true;
			RandomDetail(selfPoint, roomCtrller, pfb_Detail_URD, flip: true);
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			sr.transform.position += new Vector3(0f, 0f, 0.01f);
			if (!flag2)
			{
				sr.sprite = sprite_Corner_UR[Random.Range(0, sprite_Corner_UR.Length)];
			}
			else if (!flag4)
			{
				sr.sprite = sprite_Corner_RD[Random.Range(0, sprite_Corner_RD.Length)];
			}
			else if (!flag6)
			{
				sr.sprite = sprite_Corner_RD[Random.Range(0, sprite_Corner_RD.Length)];
				sr.flipX = true;
			}
			else if (!flag8)
			{
				sr.sprite = sprite_Corner_UR[Random.Range(0, sprite_Corner_UR.Length)];
				sr.flipX = true;
			}
		}
	}

	public override void Correct2(Vector2Data selfPoint, RoomController roomCtrller)
	{
		Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/OuterBoundary"), base.transform.position, Quaternion.identity, base.transform.parent);
		if (roomCtrller.roomCfg.id == 201)
		{
			if (Random.value <= storeDetailChance)
			{
				Object.Instantiate(pfb_Store, base.transform.position, Quaternion.identity, base.transform.parent);
			}
		}
		else if (roomCtrller.roomCfg.id == 202)
		{
			if (Random.value <= potionDetailChance)
			{
				Object.Instantiate(pfb_Potion, base.transform.position, Quaternion.identity, base.transform.parent);
			}
		}
		else if (roomCtrller.roomCfg.id == 211)
		{
			if (Random.value <= processDetailChance)
			{
				Object.Instantiate(pfb_Process, base.transform.position, Quaternion.identity, base.transform.parent);
			}
		}
		else if (roomCtrller.roomCfg.id == 212 && Random.value <= moreInOneDetailChance)
		{
			Object.Instantiate(pfb_MoreInOne, base.transform.position, Quaternion.identity, base.transform.parent);
		}
		Object.Destroy(base.gameObject);
	}
}
