using System.Collections.Generic;
using UnityEngine;

public class Tile_T8_Tile5 : TileBase
{
	[Space(50f)]
	public SpriteRenderer sr;

	public Sprite sprite_DL;

	public Sprite sprite_DLU;

	public Sprite[] sprite_Full;

	public Sprite sprite_LU;

	public Sprite sprite_LUR;

	public Sprite sprite_Null;

	public Sprite sprite_RD;

	public Sprite sprite_RDL;

	public Sprite sprite_UR;

	public Sprite sprite_URD;

	public GameObject go_CornerDL;

	public GameObject go_CornerLU;

	public GameObject go_CornerRD;

	public GameObject go_CornerUR;

	public override void TileCorrect(Vector2Data selfPoint, List<Vector2Data> otherTilePoints)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		if (otherTilePoints.Contains(selfPoint + new Vector2Data(0f, 1f)))
		{
			flag = true;
		}
		if (otherTilePoints.Contains(selfPoint + new Vector2Data(1f, 1f)))
		{
			flag2 = true;
		}
		if (otherTilePoints.Contains(selfPoint + new Vector2Data(1f, 0f)))
		{
			flag3 = true;
		}
		if (otherTilePoints.Contains(selfPoint + new Vector2Data(1f, -1f)))
		{
			flag4 = true;
		}
		if (otherTilePoints.Contains(selfPoint + new Vector2Data(0f, -1f)))
		{
			flag5 = true;
		}
		if (otherTilePoints.Contains(selfPoint + new Vector2Data(-1f, -1f)))
		{
			flag6 = true;
		}
		if (otherTilePoints.Contains(selfPoint + new Vector2Data(-1f, 0f)))
		{
			flag7 = true;
		}
		if (otherTilePoints.Contains(selfPoint + new Vector2Data(-1f, 1f)))
		{
			flag8 = true;
		}
		if (flag && flag3 && !flag5 && !flag7)
		{
			sr.sprite = sprite_UR;
			if (!flag2)
			{
				go_CornerUR.SetActive(value: true);
			}
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_RD;
			if (!flag4)
			{
				go_CornerRD.SetActive(value: true);
			}
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_DL;
			if (!flag6)
			{
				go_CornerDL.SetActive(value: true);
			}
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_LU;
			if (!flag8)
			{
				go_CornerLU.SetActive(value: true);
			}
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_LUR;
			if (!flag8)
			{
				go_CornerLU.SetActive(value: true);
			}
			if (!flag2)
			{
				go_CornerUR.SetActive(value: true);
			}
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_URD;
			if (!flag2)
			{
				go_CornerUR.SetActive(value: true);
			}
			if (!flag4)
			{
				go_CornerRD.SetActive(value: true);
			}
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_RDL;
			if (!flag4)
			{
				go_CornerRD.SetActive(value: true);
			}
			if (!flag6)
			{
				go_CornerDL.SetActive(value: true);
			}
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_DLU;
			if (!flag6)
			{
				go_CornerDL.SetActive(value: true);
			}
			if (!flag8)
			{
				go_CornerLU.SetActive(value: true);
			}
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_Full[Random.Range(0, sprite_Full.Length)];
			if (!flag2)
			{
				go_CornerUR.SetActive(value: true);
			}
			if (!flag4)
			{
				go_CornerRD.SetActive(value: true);
			}
			if (!flag6)
			{
				go_CornerDL.SetActive(value: true);
			}
			if (!flag8)
			{
				go_CornerLU.SetActive(value: true);
			}
		}
	}
}
