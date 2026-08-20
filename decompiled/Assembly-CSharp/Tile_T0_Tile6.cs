using System.Collections.Generic;
using UnityEngine;

public class Tile_T0_Tile6 : TileBase
{
	[Space(50f)]
	public SpriteRenderer sr;

	public Sprite sprite_Full;

	public Sprite sprite_LUR;

	public Sprite sprite_UR;

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
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_UR;
			sr.transform.rotation = Tool2D.GetRotation(270f);
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_UR;
			sr.transform.rotation = Tool2D.GetRotation(180f);
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_UR;
			sr.transform.rotation = Tool2D.GetRotation(90f);
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_LUR;
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_LUR;
			sr.transform.rotation = Tool2D.GetRotation(270f);
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_LUR;
			sr.transform.rotation = Tool2D.GetRotation(180f);
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_LUR;
			sr.transform.rotation = Tool2D.GetRotation(90f);
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_Full;
			if (!flag2)
			{
				go_CornerUR.SetActive(value: true);
			}
			else if (!flag4)
			{
				go_CornerUR.SetActive(value: true);
				go_CornerUR.transform.rotation = Tool2D.GetRotation(270f);
			}
			else if (!flag6)
			{
				go_CornerUR.SetActive(value: true);
				go_CornerUR.transform.rotation = Tool2D.GetRotation(180f);
			}
			else if (!flag8)
			{
				go_CornerUR.SetActive(value: true);
				go_CornerUR.transform.rotation = Tool2D.GetRotation(90f);
			}
			else
			{
				Object.Destroy(go_CornerUR);
			}
		}
		tsf_Layer.SetParent(base.transform.parent);
		Object.Destroy(base.gameObject);
	}
}
