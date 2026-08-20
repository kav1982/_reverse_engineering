using System.Collections;
using UnityEngine;

public class Boundary_T11_RightAngle : BoundaryBase
{
	[Space(50f)]
	public SpriteRenderer sr_AO;

	public SpriteRenderer sr_Wall;

	public SpriteRenderer sr_Shadow;

	[Header("AO")]
	public Sprite[] sprite_AO_RD;

	public Sprite[] sprite_AO_UR;

	[Header("Wall")]
	public Sprite[] sprite_Wall_RD;

	public Sprite[] sprite_Wall_UR;

	public override void Correct(Vector2Data selfPoint, RoomController roomCtrller)
	{
		sr_AO.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.BoundaryAO) + new Vector3(0f, 0f, Random.Range(-0.1f, 0.1f));
		sr_Shadow.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Shadow);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(0f, 1f)))
		{
			flag = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(0f, 1f)))
		{
			flag = true;
		}
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(1f, 0f)))
		{
			flag2 = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(1f, 0f)))
		{
			flag2 = true;
		}
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(0f, -1f)))
		{
			flag3 = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(0f, -1f)))
		{
			flag3 = true;
		}
		if (roomCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(-1f, 0f)))
		{
			flag4 = true;
		}
		else if (roomCtrller.roomCfg.boundary2s.Contains(selfPoint + new Vector2Data(-1f, 0f)))
		{
			flag4 = true;
		}
		if (flag && flag2 && !flag3 && !flag4)
		{
			sr_Wall.sprite = sprite_Wall_UR[Random.Range(0, sprite_Wall_UR.Length)];
			sr_AO.sprite = sprite_AO_UR[Random.Range(0, sprite_AO_UR.Length)];
		}
		else if (!flag && flag2 && flag3 && !flag4)
		{
			sr_Wall.sprite = sprite_Wall_RD[Random.Range(0, sprite_Wall_RD.Length)];
			sr_AO.sprite = sprite_AO_RD[Random.Range(0, sprite_AO_RD.Length)];
		}
		else if (!flag && !flag2 && flag3 && flag4)
		{
			sr_Wall.sprite = sprite_Wall_RD[Random.Range(0, sprite_Wall_RD.Length)];
			sr_Wall.flipX = true;
			sr_AO.sprite = sprite_AO_RD[Random.Range(0, sprite_AO_RD.Length)];
			sr_AO.flipX = true;
		}
		else if (flag && !flag2 && !flag3 && flag4)
		{
			sr_Wall.sprite = sprite_Wall_UR[Random.Range(0, sprite_Wall_UR.Length)];
			sr_Wall.flipX = true;
			sr_AO.sprite = sprite_AO_UR[Random.Range(0, sprite_AO_UR.Length)];
			sr_AO.flipX = true;
		}
		StartCoroutine(DestroySelf());
	}

	private IEnumerator DestroySelf()
	{
		yield return new WaitForSeconds(0.1f);
		Object.Destroy(this);
	}
}
