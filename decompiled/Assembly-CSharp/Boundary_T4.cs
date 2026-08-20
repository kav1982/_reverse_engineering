using System.Collections;
using UnityEngine;

public class Boundary_T4 : BoundaryBase
{
	[Space(50f)]
	public SpriteRenderer sr_Wall;

	public SpriteRenderer sr_AO;

	public float destroySelfDelay;

	[Header("AO")]
	public Sprite[] sprite_AO_LUR;

	public Sprite[] sprite_AO_RD;

	public Sprite[] sprite_AO_RDL;

	public Sprite[] sprite_AO_UR;

	public Sprite[] sprite_AO_URD;

	[Header("Wall")]
	public Sprite[] sprite_Wall_Corner_RD;

	public Sprite[] sprite_Wall_Corner_UR;

	public Sprite[] sprite_Wall_LUR;

	public Sprite[] sprite_Wall_RD;

	public Sprite[] sprite_Wall_RDL;

	public Sprite[] sprite_Wall_UR;

	public Sprite[] sprite_Wall_UR_NoGrass;

	public Sprite[] sprite_Wall_URD;

	[Header("Detail")]
	[Range(0f, 1f)]
	public float detailChange;

	public int detailMinInterval;

	public GameObject[] pfb_Detail_LUR;

	public GameObject[] pfb_Detail_RDL;

	public GameObject[] pfb_Detail_URD;

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
			if (Physics.Raycast(base.transform.position, Vector3.down, out var hitInfo, 100f, LayerMask.GetMask("Wall")) && hitInfo.transform.tag == "Access")
			{
				sr_Wall.sprite = sprite_Wall_UR_NoGrass[Random.Range(0, sprite_Wall_UR_NoGrass.Length)];
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
			if (Physics.Raycast(base.transform.position, Vector3.down, out var hitInfo2, 100f, LayerMask.GetMask("Wall")) && hitInfo2.transform.tag == "Access")
			{
				sr_Wall.sprite = sprite_Wall_UR_NoGrass[Random.Range(0, sprite_Wall_UR_NoGrass.Length)];
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
			sr_Wall.sprite = sprite_Wall_LUR[Random.Range(0, sprite_Wall_LUR.Length)];
			sr_AO.sprite = sprite_AO_LUR[Random.Range(0, sprite_AO_LUR.Length)];
			RandomDetail(selfPoint, roomCtrller, pfb_Detail_LUR);
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr_Wall.sprite = sprite_Wall_URD[Random.Range(0, sprite_Wall_URD.Length)];
			sr_AO.sprite = sprite_AO_URD[Random.Range(0, sprite_AO_URD.Length)];
			if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(0f, -2f)) || roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(0f, -2f)))
			{
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_URD);
			}
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr_Wall.sprite = sprite_Wall_RDL[Random.Range(0, sprite_Wall_RDL.Length)];
			sr_AO.sprite = sprite_AO_RDL[Random.Range(0, sprite_AO_RDL.Length)];
			RandomDetail(selfPoint, roomCtrller, pfb_Detail_RDL);
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr_Wall.sprite = sprite_Wall_URD[Random.Range(0, sprite_Wall_URD.Length)];
			sr_Wall.flipX = true;
			sr_AO.sprite = sprite_AO_URD[Random.Range(0, sprite_AO_URD.Length)];
			sr_AO.flipX = true;
			if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(0f, -2f)) || roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(0f, -2f)))
			{
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_URD, flip: true);
			}
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			if (!flag2)
			{
				sr_Wall.sprite = sprite_Wall_Corner_UR[Random.Range(0, sprite_Wall_Corner_UR.Length)];
			}
			else if (!flag4)
			{
				sr_Wall.sprite = sprite_Wall_Corner_RD[Random.Range(0, sprite_Wall_Corner_RD.Length)];
				sr_Wall.transform.position += new Vector3(0f, 0f, -0.1f);
			}
			else if (!flag6)
			{
				sr_Wall.sprite = sprite_Wall_Corner_RD[Random.Range(0, sprite_Wall_Corner_RD.Length)];
				sr_Wall.flipX = true;
				sr_Wall.transform.position += new Vector3(0f, 0f, -0.1f);
			}
			else if (!flag8)
			{
				sr_Wall.sprite = sprite_Wall_Corner_UR[Random.Range(0, sprite_Wall_Corner_UR.Length)];
				sr_Wall.flipX = true;
			}
		}
		StartCoroutine(DestroySelf());
	}

	public override void Correct2(Vector2Data selfPoint, RoomController roomCtrller)
	{
		Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/OuterBoundary"), base.transform.position, Quaternion.identity, base.transform.parent);
		Object.Destroy(base.gameObject);
	}

	private IEnumerator DestroySelf()
	{
		yield return new WaitForSeconds(destroySelfDelay);
		Object.Destroy(this);
	}

	public void SetNoGrass()
	{
	}
}
