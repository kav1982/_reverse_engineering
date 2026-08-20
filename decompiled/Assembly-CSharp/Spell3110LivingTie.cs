using UnityEngine;

public class Spell3110LivingTie : MonoBehaviour
{
	public enum lifeTieType
	{
		cross,
		horizon
	}

	public SpriteRenderer Tie1;

	public SpriteRenderer Tie2;

	public SpriteRenderer fireTie1;

	public SpriteRenderer fireTie2;

	private bool starting;

	public float Tie1StartOffset;

	public float Tie2StartOffset;

	public float tieSpeed;

	public float tieEndSpeed;

	public Sprite mat_ECFrozen;

	public Sprite mat_ECMucus;

	public Sprite mat_ECPlayer;

	public Sprite mat_ECVenom;

	public Sprite mat_ECVoid;

	public GameObject model;

	private void OnEnable()
	{
		ResetMaterial(Tie1);
		ResetMaterial(Tie2);
		ResetMaterial(fireTie1);
		ResetMaterial(fireTie2);
		starting = true;
		model.transform.localPosition = Vector3.zero;
		Tie1.material.SetFloat("_Offset", Tie1StartOffset);
		Tie2.material.SetFloat("_Offset", Tie2StartOffset);
		fireTie1.material.SetFloat("_Offset", Tie1StartOffset);
		fireTie2.material.SetFloat("_Offset", Tie2StartOffset);
	}

	public void TieStart(SpellColorType colorType, bool fireState, lifeTieType type = lifeTieType.cross)
	{
		switch (colorType)
		{
		case SpellColorType.Frozen:
			Tie1.sprite = mat_ECFrozen;
			Tie2.sprite = mat_ECFrozen;
			break;
		case SpellColorType.Mucus:
			Tie1.sprite = mat_ECMucus;
			Tie2.sprite = mat_ECMucus;
			break;
		case SpellColorType.Player:
			Tie1.sprite = mat_ECPlayer;
			Tie2.sprite = mat_ECPlayer;
			break;
		case SpellColorType.Venom:
			Tie1.sprite = mat_ECVenom;
			Tie2.sprite = mat_ECVenom;
			break;
		case SpellColorType.Void:
			Tie1.sprite = mat_ECVoid;
			Tie2.sprite = mat_ECVoid;
			break;
		}
		fireTie1.gameObject.SetActive(fireState);
		fireTie2.gameObject.SetActive(fireState);
		Tie1.gameObject.SetActive(value: false);
		Tie2.gameObject.SetActive(value: false);
		switch (type)
		{
		case lifeTieType.cross:
			Tie1.gameObject.SetActive(value: true);
			Tie2.gameObject.SetActive(value: true);
			Tie1.transform.eulerAngles = new Vector3(0f, 0f, 210f);
			Tie2.transform.eulerAngles = new Vector3(0f, 0f, -30f);
			break;
		case lifeTieType.horizon:
			Tie1.gameObject.SetActive(value: true);
			Tie1.transform.eulerAngles = new Vector3(0f, 0f, 180f);
			break;
		}
	}

	public void TieEnd()
	{
		starting = false;
	}

	private void Update()
	{
		if (starting)
		{
			Tie1.material.SetFloat("_Offset", Mathf.Lerp(Tie1.material.GetFloat("_Offset"), 0f, tieSpeed));
			Tie2.material.SetFloat("_Offset", Mathf.Lerp(Tie2.material.GetFloat("_Offset"), 0f, tieSpeed));
			if (fireTie1.gameObject.activeSelf)
			{
				fireTie1.material.SetFloat("_Offset", Mathf.Lerp(Tie1.material.GetFloat("_Offset"), 0f, tieSpeed));
				fireTie2.material.SetFloat("_Offset", Mathf.Lerp(Tie2.material.GetFloat("_Offset"), 0f, tieSpeed));
			}
		}
		else
		{
			Tie1.material.SetFloat("_Offset", Mathf.Lerp(Tie1.material.GetFloat("_Offset"), Tie1StartOffset, tieSpeed));
			Tie2.material.SetFloat("_Offset", Mathf.Lerp(Tie2.material.GetFloat("_Offset"), Tie2StartOffset, tieSpeed));
			if (fireTie1.gameObject.activeSelf)
			{
				fireTie1.material.SetFloat("_Offset", Mathf.Lerp(Tie1.material.GetFloat("_Offset"), 0f, tieSpeed));
				fireTie2.material.SetFloat("_Offset", Mathf.Lerp(Tie2.material.GetFloat("_Offset"), 0f, tieSpeed));
			}
		}
	}

	private void ResetMaterial(SpriteRenderer tieSprite)
	{
		Material material = tieSprite.material;
		material = (tieSprite.material = Object.Instantiate(material));
	}
}
