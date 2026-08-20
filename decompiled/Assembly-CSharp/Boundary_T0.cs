using System.Collections;
using UnityEngine;

public class Boundary_T0 : BoundaryBase
{
	[Space(50f)]
	public SpriteRenderer sr_AO;

	public SpriteRenderer sr_Wall;

	[Header("AO")]
	public Sprite[] sprite_AO_LUR;

	public Sprite[] sprite_AO_RD;

	public Sprite[] sprite_AO_RDL;

	public Sprite[] sprite_AO_UR;

	public Sprite[] sprite_AO_URD;

	[Header("Wall")]
	public Sprite[] sprite_Wall_Corner_RD;

	public Sprite[] sprite_Wall_Corner_RD_Short;

	public Sprite[] sprite_Wall_Corner_UR;

	public Sprite[] sprite_Wall_LUR;

	public Sprite[] sprite_Wall_LUR_Short;

	public Sprite[] sprite_Wall_RD;

	public Sprite[] sprite_Wall_RDL;

	public Sprite[] sprite_Wall_UR;

	public Sprite[] sprite_Wall_UR_Short;

	public Sprite[] sprite_Wall_URAndCorner;

	public Sprite[] sprite_Wall_URD;

	[Range(0f, 1f)]
	[Header("Detail")]
	public float detailChance;

	public int detailMinInterval;

	public GameObject[] pfb_DetailLUR;

	public GameObject[] pfb_DetailURD;

	public GameObject[] pfb_DetailRDL;

	[Range(0f, 1f)]
	public float detailChanceCorner;

	public GameObject[] pfb_Detail_Corner_RD;

	public GameObject[] pfb_Detail_Corner_UR;

	[Range(0f, 1f)]
	[Header("OuterBoundary")]
	public float ironChainChance;

	public GameObject pfb_IronChain;

	public int ironChainPerMeter = 1;

	private void RandomDetail(Vector2Data selfPoint, RoomController roomCtrller, GameObject[] prefabs, bool flip)
	{
		if (base.IsAccessBoundary || (LevelMgr.Inst.NextRewardTypes != null && ((LevelMgr.Inst.NextRewardTypes.Count >= 1 && (selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(-1f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(0f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(1f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(2f, 1f))) || (LevelMgr.Inst.NextRewardTypes.Count >= 2 && (selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(-3f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(-2f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(3f, 1f) || selfPoint == roomCtrller.roomCfg.accessUp + new Vector2Data(4f, 1f))) || (LevelMgr.Inst.NextExtraDoorRewardType != LevelRewardType.None && roomCtrller.roomCfg.extraDoor != Vector2Data.Up1000 && (selfPoint == roomCtrller.roomCfg.extraDoor + new Vector2Data(-1f, 1f) || selfPoint == roomCtrller.roomCfg.extraDoor + new Vector2Data(0f, 1f) || selfPoint == roomCtrller.roomCfg.extraDoor + new Vector2Data(1f, 1f) || selfPoint == roomCtrller.roomCfg.extraDoor + new Vector2Data(2f, 1f))))))
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
		if (!flag)
		{
			return;
		}
		float num = detailChance;
		if (prefabs == pfb_Detail_Corner_RD || prefabs == pfb_Detail_Corner_UR)
		{
			num = detailChanceCorner;
		}
		if (Random.value <= num)
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
			if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(-1f, 1f)))
			{
				sr_Wall.sprite = sprite_Wall_URAndCorner[Random.Range(0, sprite_Wall_URAndCorner.Length)];
			}
			else if (roomCtrller.roomCfg.allTileList[0].Contains(selfPoint + new Vector2Data(0f, 2f)))
			{
				sr_Wall.sprite = sprite_Wall_UR_Short[Random.Range(0, sprite_Wall_UR_Short.Length)];
			}
			else
			{
				sr_Wall.sprite = sprite_Wall_UR[Random.Range(0, sprite_Wall_UR.Length)];
			}
			sr_AO.sprite = sprite_AO_UR[Random.Range(0, sprite_AO_UR.Length)];
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr_Wall.sprite = sprite_Wall_RD[Random.Range(0, sprite_Wall_RD.Length)];
			sr_AO.sprite = sprite_AO_RD[Random.Range(0, sprite_AO_RD.Length)];
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr_Wall.sprite = sprite_Wall_RD[Random.Range(0, sprite_Wall_RD.Length)];
			sr_Wall.flipX = true;
			sr_AO.sprite = sprite_AO_RD[Random.Range(0, sprite_AO_RD.Length)];
			sr_AO.flipX = true;
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(1f, 1f)))
			{
				sr_Wall.sprite = sprite_Wall_URAndCorner[Random.Range(0, sprite_Wall_URAndCorner.Length)];
			}
			else if (roomCtrller.roomCfg.allTileList[0].Contains(selfPoint + new Vector2Data(0f, 2f)))
			{
				sr_Wall.sprite = sprite_Wall_UR_Short[Random.Range(0, sprite_Wall_UR_Short.Length)];
			}
			else
			{
				sr_Wall.sprite = sprite_Wall_UR[Random.Range(0, sprite_Wall_UR.Length)];
			}
			sr_Wall.flipX = true;
			sr_AO.sprite = sprite_AO_UR[Random.Range(0, sprite_AO_UR.Length)];
			sr_AO.flipX = true;
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			if (roomCtrller.roomCfg.allTileList[0].Contains(selfPoint + new Vector2Data(-1f, 2f)) || roomCtrller.roomCfg.allTileList[0].Contains(selfPoint + new Vector2Data(0f, 2f)) || roomCtrller.roomCfg.allTileList[0].Contains(selfPoint + new Vector2Data(1f, 2f)))
			{
				sr_Wall.sprite = sprite_Wall_LUR_Short[Random.Range(0, sprite_Wall_LUR_Short.Length)];
			}
			else
			{
				sr_Wall.sprite = sprite_Wall_LUR[Random.Range(0, sprite_Wall_LUR.Length)];
			}
			sr_AO.sprite = sprite_AO_LUR[Random.Range(0, sprite_AO_LUR.Length)];
			if (!roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(-1f, -1f)) && !roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(1f, -1f)))
			{
				RandomDetail(selfPoint, roomCtrller, pfb_DetailLUR, flip: false);
			}
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr_Wall.sprite = sprite_Wall_URD[Random.Range(0, sprite_Wall_URD.Length)];
			sr_AO.sprite = sprite_AO_URD[Random.Range(0, sprite_AO_URD.Length)];
			if (!roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(-1f, 1f)) && !roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(-1f, -1f)) && (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(0f, -2f)) || roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(0f, -2f))))
			{
				RandomDetail(selfPoint, roomCtrller, pfb_DetailURD, flip: false);
			}
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr_Wall.sprite = sprite_Wall_RDL[Random.Range(0, sprite_Wall_RDL.Length)];
			sr_AO.sprite = sprite_AO_RDL[Random.Range(0, sprite_AO_RDL.Length)];
			if (!roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(-1f, 1f)) && !roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(1f, 1f)))
			{
				RandomDetail(selfPoint, roomCtrller, pfb_DetailRDL, flip: false);
			}
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr_Wall.sprite = sprite_Wall_URD[Random.Range(0, sprite_Wall_URD.Length)];
			sr_Wall.flipX = true;
			sr_AO.sprite = sprite_AO_URD[Random.Range(0, sprite_AO_URD.Length)];
			sr_AO.flipX = true;
			if (!roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(1f, 1f)) && !roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(1f, -1f)) && (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(0f, -2f)) || roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(0f, -2f))))
			{
				RandomDetail(selfPoint, roomCtrller, pfb_DetailURD, flip: true);
			}
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			sr_Wall.transform.position += new Vector3(0f, 0f, -0.01f);
			if (!flag2)
			{
				sr_Wall.sprite = sprite_Wall_Corner_UR[Random.Range(0, sprite_Wall_Corner_UR.Length)];
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_Corner_UR, flip: false);
			}
			else if (!flag4)
			{
				sr_Wall.transform.position += new Vector3(0f, 0f, -0.01f);
				if (roomCtrller.roomCfg.allTileList[0].Contains(selfPoint + new Vector2Data(0f, 2f)) || roomCtrller.roomCfg.allTileList[0].Contains(selfPoint + new Vector2Data(1f, 2f)))
				{
					sr_Wall.sprite = sprite_Wall_Corner_RD_Short[Random.Range(0, sprite_Wall_Corner_RD_Short.Length)];
				}
				else
				{
					sr_Wall.sprite = sprite_Wall_Corner_RD[Random.Range(0, sprite_Wall_Corner_RD.Length)];
				}
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_Corner_RD, flip: false);
			}
			else if (!flag6)
			{
				sr_Wall.transform.position += new Vector3(0f, 0f, -0.01f);
				if (roomCtrller.roomCfg.allTileList[0].Contains(selfPoint + new Vector2Data(0f, 2f)) || roomCtrller.roomCfg.allTileList[0].Contains(selfPoint + new Vector2Data(-1f, 2f)))
				{
					sr_Wall.sprite = sprite_Wall_Corner_RD_Short[Random.Range(0, sprite_Wall_Corner_RD_Short.Length)];
				}
				else
				{
					sr_Wall.sprite = sprite_Wall_Corner_RD[Random.Range(0, sprite_Wall_Corner_RD.Length)];
				}
				sr_Wall.flipX = true;
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_Corner_RD, flip: true);
			}
			else if (!flag8)
			{
				sr_Wall.sprite = sprite_Wall_Corner_UR[Random.Range(0, sprite_Wall_Corner_UR.Length)];
				sr_Wall.flipX = true;
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_Corner_UR, flip: true);
			}
		}
		StartCoroutine(DestroySelf());
	}

	public override void Correct2(Vector2Data selfPoint, RoomController levelCtrller)
	{
		Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/OuterBoundary"), base.transform.position, Quaternion.identity, base.transform.parent);
		if (selfPoint.x % (float)ironChainPerMeter == 0f && selfPoint.y % (float)ironChainPerMeter == 0f && Random.value <= ironChainChance)
		{
			Object.Instantiate(pfb_IronChain, base.transform.position, Quaternion.identity, base.transform.parent);
		}
		Object.Destroy(base.gameObject);
	}

	private IEnumerator DestroySelf()
	{
		yield return new WaitForSeconds(0.1f);
		Object.Destroy(this);
	}
}
