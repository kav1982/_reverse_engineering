using System.Collections;
using UnityEngine;

public class Boundary_T11 : BoundaryBase
{
	[Space(50f)]
	public GameObject go_ColliderAndShadow;

	public SpriteRenderer sr_AO;

	public SpriteRenderer sr_Wall;

	public SpriteRenderer sr_Tile;

	[Header("AO")]
	public Sprite[] sprite_AO_D;

	public Sprite[] sprite_AO_LUR;

	public Sprite[] sprite_AO_R;

	public Sprite[] sprite_AO_RD;

	public Sprite[] sprite_AO_RDL;

	public Sprite[] sprite_AO_U;

	public Sprite[] sprite_AO_UR;

	public Sprite[] sprite_AO_URD;

	[Header("Wall")]
	public Sprite[] sprite_Wall_Corner_RD;

	public Sprite[] sprite_Wall_Corner_UR;

	public Sprite[] sprite_Wall_Down;

	public Sprite[] sprite_Wall_LUR;

	public Sprite[] sprite_Wall_RD;

	public Sprite[] sprite_Wall_RDL;

	public Sprite[] sprite_Wall_U;

	public Sprite[] sprite_Wall_UR;

	public Sprite[] sprite_Wall_URD;

	[Header("Collider")]
	public GameObject go_ColliderFull;

	public GameObject go_ColliderBig;

	public GameObject go_ColliderSmall;

	[Header("Detail")]
	[Range(0f, 1f)]
	public float detailChange;

	public int detailMinInterval;

	public GameObject[] pfb_Detail_UR;

	public GameObject[] pfb_Detail_RD;

	public GameObject[] pfb_Detail_LUR;

	public GameObject[] pfb_Detail_URD;

	public GameObject[] pfb_Detail_RDL;

	private void RandomDetail(Vector2Data selfPoint, RoomController roomCtrller, GameObject[] prefabs, bool flip = false)
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
		sr_Tile.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile0);
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
		if (flag && !flag3 && !flag5 && !flag7)
		{
			sr_Wall.sprite = sprite_Wall_U[Random.Range(0, sprite_Wall_U.Length)];
			sr_Wall.transform.position += new Vector3(0f, 0f, 0.49f);
			sr_AO.sprite = sprite_AO_U[Random.Range(0, sprite_AO_U.Length)];
			go_ColliderSmall.SetActive(value: true);
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavAction"), base.transform.position, Quaternion.identity, roomCtrller.tsf_Action);
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGround"), base.transform.position, Quaternion.identity, roomCtrller.tsf_Ground);
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFly"), base.transform.position, Quaternion.identity, roomCtrller.tsf_Fly);
		}
		else if (!flag && flag3 && !flag5 && !flag7)
		{
			sr_AO.sprite = sprite_AO_R[Random.Range(0, sprite_AO_R.Length)];
			Object.Destroy(sr_Wall.gameObject);
			Object.Destroy(go_ColliderAndShadow);
		}
		else if (!flag && !flag3 && flag5 && !flag7)
		{
			sr_Wall.sprite = sprite_Wall_Down[Random.Range(0, sprite_Wall_Down.Length)];
			sr_Wall.transform.position += new Vector3(0f, 0f, -0.49f);
			sr_AO.sprite = sprite_AO_D[Random.Range(0, sprite_AO_D.Length)];
			go_ColliderSmall.SetActive(value: true);
			go_ColliderSmall.transform.rotation = Tool2D.GetRotation(180f);
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavAction"), base.transform.position, Quaternion.identity, roomCtrller.tsf_Action);
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGround"), base.transform.position, Quaternion.identity, roomCtrller.tsf_Ground);
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFly"), base.transform.position, Quaternion.identity, roomCtrller.tsf_Fly);
		}
		else if (!flag && !flag3 && !flag5 && flag7)
		{
			sr_AO.sprite = sprite_AO_R[Random.Range(0, sprite_AO_R.Length)];
			sr_AO.flipX = true;
			Object.Destroy(sr_Wall.gameObject);
			Object.Destroy(go_ColliderAndShadow);
		}
		else if (flag && flag3 && !flag5 && !flag7)
		{
			if (Physics.Raycast(base.transform.position, Vector3.down, out var hitInfo, 100f, LayerMask.GetMask("Wall")) && hitInfo.transform.tag == "Access")
			{
				int themeType = (int)roomCtrller.roomCfg.themeType;
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Scene/Theme" + themeType + "/Boundary_RightAngle"), base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<BoundaryBase>().Correct(selfPoint, roomCtrller);
			}
			else
			{
				sr_Wall.sprite = sprite_Wall_UR[Random.Range(0, sprite_Wall_UR.Length)];
				sr_Wall.transform.position += new Vector3(0f, 0f, 0.49f);
				sr_AO.sprite = sprite_AO_UR[Random.Range(0, sprite_AO_UR.Length)];
				go_ColliderBig.SetActive(value: true);
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_UR);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavActionTriangle"), base.transform.position, Tool2D.GetRotation(180f), roomCtrller.tsf_Action);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGroundTriangle"), base.transform.position, Tool2D.GetRotation(180f), roomCtrller.tsf_Ground);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFlyTriangle"), base.transform.position, Tool2D.GetRotation(180f), roomCtrller.tsf_Fly);
			}
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			if (Physics.Raycast(base.transform.position, Vector3.up, out var hitInfo2, 100f, LayerMask.GetMask("Wall")) && hitInfo2.transform.tag == "Access")
			{
				int themeType = (int)roomCtrller.roomCfg.themeType;
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Scene/Theme" + themeType + "/Boundary_RightAngle"), base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<BoundaryBase>().Correct(selfPoint, roomCtrller);
			}
			else
			{
				sr_Wall.sprite = sprite_Wall_RD[Random.Range(0, sprite_Wall_RD.Length)];
				sr_Wall.transform.position += new Vector3(0f, 0f, -0.49f);
				sr_AO.sprite = sprite_AO_RD[Random.Range(0, sprite_AO_RD.Length)];
				go_ColliderBig.SetActive(value: true);
				go_ColliderBig.transform.rotation = Tool2D.GetRotation(270f);
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_RD);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavActionTriangle"), base.transform.position, Tool2D.GetRotation(90f), roomCtrller.tsf_Action);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGroundTriangle"), base.transform.position, Tool2D.GetRotation(90f), roomCtrller.tsf_Ground);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFlyTriangle"), base.transform.position, Tool2D.GetRotation(90f), roomCtrller.tsf_Fly);
			}
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			if (Physics.Raycast(base.transform.position, Vector3.up, out var hitInfo3, 100f, LayerMask.GetMask("Wall")) && hitInfo3.transform.tag == "Access")
			{
				int themeType = (int)roomCtrller.roomCfg.themeType;
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Scene/Theme" + themeType + "/Boundary_RightAngle"), base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<BoundaryBase>().Correct(selfPoint, roomCtrller);
			}
			else
			{
				sr_Wall.sprite = sprite_Wall_RD[Random.Range(0, sprite_Wall_RD.Length)];
				sr_Wall.transform.localScale = new Vector3(-1f, 1f, 1f);
				sr_Wall.transform.position += new Vector3(0f, 0f, -0.49f);
				sr_AO.sprite = sprite_AO_RD[Random.Range(0, sprite_AO_RD.Length)];
				sr_AO.flipX = true;
				go_ColliderBig.SetActive(value: true);
				go_ColliderBig.transform.rotation = Tool2D.GetRotation(180f);
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_RD, flip: true);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavActionTriangle"), base.transform.position, Tool2D.GetRotation(0f), roomCtrller.tsf_Action);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGroundTriangle"), base.transform.position, Tool2D.GetRotation(0f), roomCtrller.tsf_Ground);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFlyTriangle"), base.transform.position, Tool2D.GetRotation(0f), roomCtrller.tsf_Fly);
			}
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			if (Physics.Raycast(base.transform.position, Vector3.down, out var hitInfo4, 100f, LayerMask.GetMask("Wall")) && hitInfo4.transform.tag == "Access")
			{
				int themeType = (int)roomCtrller.roomCfg.themeType;
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Scene/Theme" + themeType + "/Boundary_RightAngle"), base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<BoundaryBase>().Correct(selfPoint, roomCtrller);
			}
			else
			{
				sr_Wall.sprite = sprite_Wall_UR[Random.Range(0, sprite_Wall_UR.Length)];
				sr_Wall.transform.localScale = new Vector3(-1f, 1f, 1f);
				sr_Wall.transform.position += new Vector3(0f, 0f, 0.49f);
				sr_AO.sprite = sprite_AO_UR[Random.Range(0, sprite_AO_UR.Length)];
				sr_AO.flipX = true;
				go_ColliderBig.SetActive(value: true);
				go_ColliderBig.transform.rotation = Tool2D.GetRotation(90f);
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_UR, flip: true);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavActionTriangle"), base.transform.position, Tool2D.GetRotation(270f), roomCtrller.tsf_Action);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavGroundTriangle"), base.transform.position, Tool2D.GetRotation(270f), roomCtrller.tsf_Ground);
				Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/NavFlyTriangle"), base.transform.position, Tool2D.GetRotation(270f), roomCtrller.tsf_Fly);
			}
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr_Wall.sprite = sprite_Wall_LUR[Random.Range(0, sprite_Wall_LUR.Length)];
			sr_AO.sprite = sprite_AO_LUR[Random.Range(0, sprite_AO_LUR.Length)];
			go_ColliderFull.SetActive(value: true);
			if (!roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(-1f, -1f)) && !roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(1f, -1f)) && roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(-2f, 0f)) && roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(2f, 0f)))
			{
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_LUR);
			}
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr_Wall.sprite = sprite_Wall_URD[Random.Range(0, sprite_Wall_URD.Length)];
			sr_Wall.transform.position += new Vector3(0f, 0f, -0.49f);
			sr_AO.sprite = sprite_AO_URD[Random.Range(0, sprite_AO_URD.Length)];
			go_ColliderFull.SetActive(value: true);
			if (!roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(-1f, -1f)) && !roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(-1f, 1f)) && roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(0f, -2f)) && roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(0f, 2f)))
			{
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_URD);
			}
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr_Wall.sprite = sprite_Wall_RDL[Random.Range(0, sprite_Wall_RDL.Length)];
			sr_AO.sprite = sprite_AO_RDL[Random.Range(0, sprite_AO_RDL.Length)];
			go_ColliderFull.SetActive(value: true);
			if (!roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(-1f, 1f)) && !roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(1f, 1f)) && roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(-2f, 0f)) && roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(2f, 0f)))
			{
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_RDL);
			}
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr_Wall.sprite = sprite_Wall_URD[Random.Range(0, sprite_Wall_URD.Length)];
			sr_Wall.flipX = true;
			sr_Wall.transform.position += new Vector3(0f, 0f, -0.49f);
			sr_AO.sprite = sprite_AO_URD[Random.Range(0, sprite_AO_URD.Length)];
			sr_AO.flipX = true;
			go_ColliderFull.SetActive(value: true);
			if (!roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(1f, 1f)) && !roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(1f, -1f)) && roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(0f, -2f)) && roomCtrller.roomCfg.boundarys.Contains(selfPoint + new Vector2Data(0f, 2f)))
			{
				RandomDetail(selfPoint, roomCtrller, pfb_Detail_URD, flip: true);
			}
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			Object.Destroy(sr_Tile.gameObject);
			go_ColliderFull.SetActive(value: true);
			if (!flag2 && flag4 && flag6 && flag8)
			{
				sr_Wall.sprite = sprite_Wall_Corner_UR[Random.Range(0, sprite_Wall_Corner_UR.Length)];
			}
			else if (flag2 && !flag4 && flag6 && flag8)
			{
				sr_Wall.sprite = sprite_Wall_Corner_RD[Random.Range(0, sprite_Wall_Corner_RD.Length)];
			}
			else if (flag2 && flag4 && !flag6 && flag8)
			{
				sr_Wall.sprite = sprite_Wall_Corner_RD[Random.Range(0, sprite_Wall_Corner_RD.Length)];
				sr_Wall.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (flag2 && flag4 && flag6 && !flag8)
			{
				sr_Wall.sprite = sprite_Wall_Corner_UR[Random.Range(0, sprite_Wall_Corner_UR.Length)];
				sr_Wall.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		StartCoroutine(DestroySelf());
	}

	public override void Correct2(Vector2Data selfPoint, RoomController levelCtrller)
	{
		Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/OuterBoundary"), base.transform.position, Quaternion.identity, base.transform.parent);
		Object.Destroy(base.gameObject);
	}

	private IEnumerator DestroySelf()
	{
		yield return new WaitForSeconds(0.1f);
		Object.Destroy(this);
	}
}
