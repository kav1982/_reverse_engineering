using System.Collections.Generic;
using UnityEngine;

public class Tile_T8_Tile9 : TileBase
{
	[Space(50f)]
	public SpriteRenderer sr_Base;

	public SpriteRenderer sr_Wall;

	[Header("Base")]
	public Sprite[] sprite_Base_D;

	public Sprite[] sprite_Base_LR;

	public Sprite[] sprite_Base_LUR;

	public Sprite[] sprite_Base_Null;

	public Sprite[] sprite_Base_R;

	public Sprite[] sprite_Base_RD;

	public Sprite[] sprite_Base_RDL;

	public Sprite[] sprite_Base_U;

	public Sprite[] sprite_Base_UD;

	public Sprite[] sprite_Base_UR;

	public Sprite[] sprite_Base_URD;

	[Header("Wall")]
	public Sprite[] sprite_Wall_D;

	public Sprite[] sprite_Wall_Full;

	public Sprite sprite_Wall_FullFog;

	public Sprite[] sprite_Wall_LR;

	public Sprite[] sprite_Wall_LUR;

	public Sprite[] sprite_Wall_Null;

	public Sprite[] sprite_Wall_R;

	public Sprite[] sprite_Wall_RD;

	public Sprite[] sprite_Wall_RDL;

	public Sprite[] sprite_Wall_U;

	public Sprite[] sprite_Wall_UD;

	public Sprite[] sprite_Wall_UR;

	public Sprite[] sprite_Wall_URD;

	public GameObject go_Wall_CornerDL;

	public GameObject go_Wall_CornerLU;

	public GameObject go_Wall_CornerRD;

	public GameObject go_Wall_CornerUR;

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
		sr_Base.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.BoundaryLow);
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
		if (!flag && !flag3 && !flag5 && !flag7)
		{
			sr_Base.sprite = sprite_Base_Null[Random.Range(0, sprite_Base_Null.Length)];
			sr_Wall.sprite = sprite_Wall_Null[Random.Range(0, sprite_Wall_Null.Length)];
		}
		else if (flag && !flag3 && !flag5 && !flag7)
		{
			sr_Base.sprite = sprite_Base_U[Random.Range(0, sprite_Base_U.Length)];
			sr_Wall.sprite = sprite_Wall_U[Random.Range(0, sprite_Wall_U.Length)];
		}
		else if (!flag && flag3 && !flag5 && !flag7)
		{
			sr_Base.sprite = sprite_Base_R[Random.Range(0, sprite_Base_R.Length)];
			sr_Wall.sprite = sprite_Wall_R[Random.Range(0, sprite_Wall_R.Length)];
		}
		else if (!flag && !flag3 && flag5 && !flag7)
		{
			sr_Base.sprite = sprite_Base_D[Random.Range(0, sprite_Base_D.Length)];
			sr_Wall.sprite = sprite_Wall_D[Random.Range(0, sprite_Wall_D.Length)];
		}
		else if (!flag && !flag3 && !flag5 && flag7)
		{
			sr_Base.sprite = sprite_Base_R[Random.Range(0, sprite_Base_R.Length)];
			sr_Wall.sprite = sprite_Wall_R[Random.Range(0, sprite_Wall_R.Length)];
			sr_Base.flipX = true;
			sr_Wall.flipX = true;
		}
		else if (flag && !flag3 && flag5 && !flag7)
		{
			sr_Base.sprite = sprite_Base_UD[Random.Range(0, sprite_Base_UD.Length)];
			sr_Wall.sprite = sprite_Wall_UD[Random.Range(0, sprite_Wall_UD.Length)];
		}
		else if (!flag && flag3 && !flag5 && flag7)
		{
			sr_Base.sprite = sprite_Base_LR[Random.Range(0, sprite_Base_LR.Length)];
			sr_Wall.sprite = sprite_Wall_LR[Random.Range(0, sprite_Wall_LR.Length)];
		}
		else if (flag && flag3 && !flag5 && !flag7)
		{
			sr_Base.sprite = sprite_Base_UR[Random.Range(0, sprite_Base_UR.Length)];
			sr_Wall.sprite = sprite_Wall_UR[Random.Range(0, sprite_Wall_UR.Length)];
			if (!flag2)
			{
				go_Wall_CornerUR.SetActive(value: true);
			}
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr_Base.sprite = sprite_Base_RD[Random.Range(0, sprite_Base_RD.Length)];
			sr_Wall.sprite = sprite_Wall_RD[Random.Range(0, sprite_Wall_RD.Length)];
			if (!flag4)
			{
				go_Wall_CornerRD.SetActive(value: true);
			}
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr_Base.sprite = sprite_Base_RD[Random.Range(0, sprite_Base_RD.Length)];
			sr_Wall.sprite = sprite_Wall_RD[Random.Range(0, sprite_Wall_RD.Length)];
			sr_Base.flipX = true;
			sr_Wall.flipX = true;
			if (!flag6)
			{
				go_Wall_CornerDL.SetActive(value: true);
			}
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr_Base.sprite = sprite_Base_UR[Random.Range(0, sprite_Base_UR.Length)];
			sr_Wall.sprite = sprite_Wall_UR[Random.Range(0, sprite_Wall_UR.Length)];
			sr_Base.flipX = true;
			sr_Wall.flipX = true;
			if (!flag8)
			{
				go_Wall_CornerLU.SetActive(value: true);
			}
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr_Base.sprite = sprite_Base_LUR[Random.Range(0, sprite_Base_LUR.Length)];
			sr_Wall.sprite = sprite_Wall_LUR[Random.Range(0, sprite_Wall_LUR.Length)];
			if (!flag8)
			{
				go_Wall_CornerLU.SetActive(value: true);
			}
			if (!flag2)
			{
				go_Wall_CornerUR.SetActive(value: true);
			}
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr_Base.sprite = sprite_Base_URD[Random.Range(0, sprite_Base_URD.Length)];
			sr_Wall.sprite = sprite_Wall_URD[Random.Range(0, sprite_Wall_URD.Length)];
			if (!flag2)
			{
				go_Wall_CornerUR.SetActive(value: true);
			}
			if (!flag4)
			{
				go_Wall_CornerRD.SetActive(value: true);
			}
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr_Base.sprite = sprite_Base_RDL[Random.Range(0, sprite_Base_RDL.Length)];
			sr_Wall.sprite = sprite_Wall_RDL[Random.Range(0, sprite_Wall_RDL.Length)];
			if (!flag4)
			{
				go_Wall_CornerRD.SetActive(value: true);
			}
			if (!flag6)
			{
				go_Wall_CornerDL.SetActive(value: true);
			}
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr_Base.sprite = sprite_Base_URD[Random.Range(0, sprite_Base_URD.Length)];
			sr_Wall.sprite = sprite_Wall_URD[Random.Range(0, sprite_Wall_URD.Length)];
			sr_Base.flipX = true;
			sr_Wall.flipX = true;
			if (!flag6)
			{
				go_Wall_CornerDL.SetActive(value: true);
			}
			if (!flag8)
			{
				go_Wall_CornerLU.SetActive(value: true);
			}
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			sr_Base.sprite = sprite_Base_Null[Random.Range(0, sprite_Base_Null.Length)];
			sr_Wall.sprite = sprite_Wall_Full[Random.Range(0, sprite_Wall_Full.Length)];
			if (!flag2)
			{
				go_Wall_CornerUR.SetActive(value: true);
			}
			if (!flag4)
			{
				go_Wall_CornerRD.SetActive(value: true);
			}
			if (!flag6)
			{
				go_Wall_CornerDL.SetActive(value: true);
			}
			if (!flag8)
			{
				go_Wall_CornerLU.SetActive(value: true);
			}
			if (flag2 && flag4 && flag6 && flag8)
			{
				sr_Wall.sprite = sprite_Wall_FullFog;
				sr_Wall.transform.position += new Vector3(0f, 0f, -1.1f);
			}
		}
	}
}
