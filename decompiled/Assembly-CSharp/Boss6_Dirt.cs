using System.Collections.Generic;
using UnityEngine;

public class Boss6_Dirt : MonoBehaviour
{
	public float fillAmount;

	public Material dirtMaterial;

	public List<SpriteRenderer> SR_dirts = new List<SpriteRenderer>();

	public List<Vector3> offset = new List<Vector3>();

	public float frontDirtRegardDistance;

	public AnimationCurve showCurve;

	public float showTime;

	public float slowHideValue;

	[Header("和谐")]
	public Sprite sprite_H;

	private bool isHide;

	private bool slowHide;

	private float timer;

	private void Start()
	{
		isHide = true;
		offset.Clear();
		for (int i = 0; i < SR_dirts.Count; i++)
		{
			offset.Add(SR_dirts[i].transform.position - base.transform.position);
			if (GameMgr.IsChAge14_Static)
			{
				SR_dirts[i].sprite = sprite_H;
			}
		}
	}

	public void Show()
	{
		isHide = false;
	}

	public void Hide()
	{
		isHide = true;
	}

	public void FinalHide()
	{
		isHide = true;
		slowHide = true;
	}

	private void Update()
	{
		if (!isHide)
		{
			timer += Time.deltaTime;
			timer = Mathf.Min(timer, showTime);
		}
		else
		{
			timer -= Time.deltaTime * (slowHide ? slowHideValue : 1f);
			timer = Mathf.Max(timer, 0f);
		}
		fillAmount = showCurve.Evaluate(timer / showTime);
		for (int i = 0; i < SR_dirts.Count; i++)
		{
			SR_dirts[i].transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(offset[i] + base.transform.position));
			SR_dirts[i].material.SetFloat("_Fill", fillAmount);
		}
	}
}
