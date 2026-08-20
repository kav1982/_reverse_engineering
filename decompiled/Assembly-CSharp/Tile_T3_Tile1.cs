using System.Collections.Generic;
using UnityEngine;

public class Tile_T3_Tile1 : TileBase
{
	[Space(50f)]
	public SpriteRenderer sr;

	public Sprite[] sprite_D;

	public Sprite[] sprite_DL;

	public Sprite[] sprite_DLU;

	public Sprite[] sprite_L;

	public Sprite[] sprite_LR;

	public Sprite[] sprite_LU;

	public Sprite[] sprite_LUR;

	public Sprite[] sprite_R;

	public Sprite[] sprite_RD;

	public Sprite[] sprite_RDL;

	public Sprite[] sprite_U;

	public Sprite[] sprite_UD;

	public Sprite[] sprite_UR;

	public Sprite[] sprite_URD;

	[Header("Corner")]
	public SpriteRenderer sr_CornerDL;

	public SpriteRenderer sr_CornerLU;

	public SpriteRenderer sr_CornerRD;

	public SpriteRenderer sr_CornerUR;

	public Sprite sprite_CornerUR;

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
		if (flag && !flag3 && !flag5 && !flag7)
		{
			sr.sprite = sprite_U[Random.Range(0, sprite_U.Length)];
		}
		else if (!flag && flag3 && !flag5 && !flag7)
		{
			sr.sprite = sprite_R[Random.Range(0, sprite_R.Length)];
		}
		else if (!flag && !flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_D[Random.Range(0, sprite_D.Length)];
		}
		else if (!flag && !flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_L[Random.Range(0, sprite_L.Length)];
		}
		else if (!flag && flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_LR[Random.Range(0, sprite_LR.Length)];
		}
		else if (flag && !flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_UD[Random.Range(0, sprite_UD.Length)];
		}
		else if (flag && flag3 && !flag5 && !flag7)
		{
			sr.sprite = sprite_UR[Random.Range(0, sprite_UR.Length)];
			sr_CornerUR.gameObject.SetActive(value: true);
			if (!flag2)
			{
				sr_CornerUR.sprite = sprite_CornerUR;
			}
		}
		else if (!flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_RD[Random.Range(0, sprite_RD.Length)];
			sr_CornerRD.gameObject.SetActive(value: true);
			if (!flag4)
			{
				sr_CornerRD.sprite = sprite_CornerUR;
			}
		}
		else if (!flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_DL[Random.Range(0, sprite_DL.Length)];
			sr_CornerDL.gameObject.SetActive(value: true);
			if (!flag6)
			{
				sr_CornerDL.sprite = sprite_CornerUR;
			}
		}
		else if (flag && !flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_LU[Random.Range(0, sprite_LU.Length)];
			sr_CornerLU.gameObject.SetActive(value: true);
			if (!flag8)
			{
				sr_CornerLU.sprite = sprite_CornerUR;
			}
		}
		else if (flag && flag3 && !flag5 && flag7)
		{
			sr.sprite = sprite_LUR[Random.Range(0, sprite_LUR.Length)];
			sr_CornerUR.gameObject.SetActive(value: true);
			if (!flag8)
			{
				sr_CornerLU.sprite = sprite_CornerUR;
			}
			sr_CornerLU.gameObject.SetActive(value: true);
			if (!flag2)
			{
				sr_CornerUR.sprite = sprite_CornerUR;
			}
		}
		else if (flag && flag3 && flag5 && !flag7)
		{
			sr.sprite = sprite_URD[Random.Range(0, sprite_URD.Length)];
			sr_CornerUR.gameObject.SetActive(value: true);
			if (!flag2)
			{
				sr_CornerUR.sprite = sprite_CornerUR;
			}
			sr_CornerRD.gameObject.SetActive(value: true);
			if (!flag4)
			{
				sr_CornerRD.sprite = sprite_CornerUR;
			}
		}
		else if (!flag && flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_RDL[Random.Range(0, sprite_RDL.Length)];
			sr_CornerRD.gameObject.SetActive(value: true);
			if (!flag4)
			{
				sr_CornerRD.sprite = sprite_CornerUR;
			}
			sr_CornerDL.gameObject.SetActive(value: true);
			if (!flag6)
			{
				sr_CornerDL.sprite = sprite_CornerUR;
			}
		}
		else if (flag && !flag3 && flag5 && flag7)
		{
			sr.sprite = sprite_DLU[Random.Range(0, sprite_DLU.Length)];
			sr_CornerDL.gameObject.SetActive(value: true);
			if (!flag6)
			{
				sr_CornerDL.sprite = sprite_CornerUR;
			}
			sr_CornerLU.gameObject.SetActive(value: true);
			if (!flag8)
			{
				sr_CornerLU.sprite = sprite_CornerUR;
			}
		}
		else if (flag && flag3 && flag5 && flag7)
		{
			Object.Destroy(sr.gameObject);
			sr_CornerUR.gameObject.SetActive(value: true);
			if (!flag2)
			{
				sr_CornerUR.sprite = sprite_CornerUR;
			}
			sr_CornerRD.gameObject.SetActive(value: true);
			if (!flag4)
			{
				sr_CornerRD.sprite = sprite_CornerUR;
			}
			sr_CornerDL.gameObject.SetActive(value: true);
			if (!flag6)
			{
				sr_CornerDL.sprite = sprite_CornerUR;
			}
			sr_CornerLU.gameObject.SetActive(value: true);
			if (!flag8)
			{
				sr_CornerLU.sprite = sprite_CornerUR;
			}
		}
	}
}
