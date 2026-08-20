using UnityEngine;

public class Teammate5FuseBody : MonoBehaviour
{
	public Animator Anima;

	public Transform shootPos;

	public GameObject fireEffect;

	public SpriteRenderer sr;

	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	public float WidthRatioPerBook;

	public void SetColor(SpellColorType type)
	{
		fireEffect.SetActive(value: false);
		switch (type)
		{
		case SpellColorType.Frozen:
			if (sr.material != mat_ECFrozen)
			{
				sr.material = mat_ECFrozen;
			}
			break;
		case SpellColorType.Mucus:
			if (sr.material != mat_ECMucus)
			{
				sr.material = mat_ECMucus;
			}
			break;
		case SpellColorType.Fire:
			fireEffect.SetActive(value: true);
			if (sr.material != mat_ECPlayer)
			{
				sr.material = mat_ECPlayer;
			}
			break;
		case SpellColorType.Player:
			if (sr.material != mat_ECPlayer)
			{
				sr.material = mat_ECPlayer;
			}
			break;
		case SpellColorType.Venom:
			if (sr.material != mat_ECVenom)
			{
				sr.material = mat_ECVenom;
			}
			break;
		default:
			Debug.LogError(type);
			break;
		}
		sr.material.SetInt(UseGhostEffect, 0);
		sr.material.SetInt(UseFuseShineEffect, 0);
		sr.material.SetFloat(FuseShineProcess, 0f);
		fireEffect.GetComponent<SpriteRenderer>().material.SetFloat(FuseShineProcess, 0f);
		GeneralTool.InitialSpriteMaterial(sr);
	}

	public void SetBookNum(int count)
	{
		sr.size = new Vector2(WidthRatioPerBook * (1f + (float)(count - 1) / 2f), sr.size.y);
	}
}
