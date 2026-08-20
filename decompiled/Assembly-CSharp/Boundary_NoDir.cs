using UnityEngine;

public class Boundary_NoDir : BoundaryBase
{
	[Space(50f)]
	public SpriteRenderer sr;

	public SpriteRenderer sr_Tile;

	public Sprite[] sprite_Boundarys;

	public VariableFloat scale;

	public float offset;

	[Range(0f, 1f)]
	public float detailChange;

	public int detailMinInterval;

	public GameObject[] pfb_DetailLeftUpRight;

	public GameObject[] pfb_DetailRightDownLeft;

	public GameObject[] pfb_DetailUpRightDown;

	private void RandomDetail(Vector2Data selfPoint, RoomController levelCtrller, GameObject[] prefabs, float rotate)
	{
		if (selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(-2f, 1f) || selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(-1f, 1f) || selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(0f, 1f) || selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(1f, 1f) || selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(2f, 1f) || selfPoint == levelCtrller.roomCfg.accessUp + new Vector2Data(3f, 1f))
		{
			return;
		}
		bool flag = true;
		for (int i = -detailMinInterval; i <= detailMinInterval; i++)
		{
			for (int j = -detailMinInterval; j <= detailMinInterval; j++)
			{
				if (levelCtrller.boundaryBase1Dic.ContainsKey(selfPoint + new Vector2Data(i, j)) && levelCtrller.boundaryBase1Dic[selfPoint + new Vector2Data(i, j)].HaveDetail)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag && Random.value <= detailChange)
		{
			base.HaveDetail = true;
			Object.Instantiate(prefabs[Random.Range(0, prefabs.Length)], base.transform.position, Quaternion.identity, base.transform.parent).transform.rotation = Tool2D.GetRotation(rotate);
		}
	}

	public override void Correct(Vector2Data selfPoint, RoomController levelCtrller)
	{
		sr_Tile.transform.parent = base.transform.parent;
		sr_Tile.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile0);
		sr.sprite = sprite_Boundarys[Random.Range(0, sprite_Boundarys.Length)];
		sr.transform.localScale = Vector3.one * scale.RandomResult();
		base.transform.position += Tool2D.GetDir() * Random.Range(0f, offset);
		CorrectLayerOnce();
	}

	public override void Correct2(Vector2Data selfPoint, RoomController levelCtrller)
	{
		sr_Tile.transform.parent = base.transform.parent;
		sr_Tile.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile0);
		sr.sprite = sprite_Boundarys[Random.Range(0, sprite_Boundarys.Length)];
		sr.transform.localScale = Vector3.one * scale.RandomResult();
		base.transform.position += Tool2D.GetDir() * Random.Range(0f, offset);
		CorrectLayerOnce();
	}
}
