using System.Collections.Generic;
using UnityEngine;

public class Elite8_BackShine : MonoBehaviour
{
	public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

	public float MixAmount;

	public Color spriteColor;

	public bool fade;

	public Sprite back;

	public Sprite backFury;

	public Sprite backStar;

	public Sprite backStarFury;

	public bool isFury;

	public SpriteRenderer Back;

	[Header("和谐模式")]
	public Sprite back_H;

	public Sprite backStar_H;

	private void Start()
	{
		Update();
	}

	private void Update()
	{
		for (int i = 0; i < spriteRenderers.Count; i++)
		{
			if (spriteRenderers[i] == Back)
			{
				if (isFury)
				{
					Back.sprite = backFury;
				}
				else if (GameMgr.IsHarmony_Static)
				{
					Back.sprite = back_H;
				}
				else
				{
					Back.sprite = back;
				}
			}
			else if (isFury)
			{
				spriteRenderers[i].sprite = backStarFury;
			}
			else if (GameMgr.IsHarmony_Static)
			{
				spriteRenderers[i].sprite = backStar_H;
			}
			else
			{
				spriteRenderers[i].sprite = backStar;
			}
			if (fade)
			{
				spriteRenderers[i].material.SetColor("_Color", spriteColor);
			}
		}
	}
}
