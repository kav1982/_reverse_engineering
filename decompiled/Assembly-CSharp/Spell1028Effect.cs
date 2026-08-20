using UnityEngine;

public class Spell1028Effect : SpellEffectBase
{
	private Spell1028MrBingArrow arrowScript;

	private Transform spellTransform;

	private int rotateDir = 1;

	public float rotateSpeed;

	private static readonly int AddGaintArrowColor = Shader.PropertyToID("_AddGaintArrowColor");

	public bool AddSubArrowColor { get; set; }

	protected override void Awake()
	{
		base.Awake();
		arrowScript = (Spell1028MrBingArrow)base.Spell;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		spellTransform = null;
		rotateDir = ((Random.Range(0f, 1f) >= 0.5f) ? 1 : (-1));
		AddSubArrowColor = false;
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		string text = effect.Name;
		if (!(text == "Spell"))
		{
			if (text == "Hit")
			{
				ParticleSystem.MainModule main = trans.Find("BreakArrow").GetComponent<ParticleSystem>().main;
				main.gravityModifierMultiplier = 2.5f * base.transform.localScale.x;
			}
		}
		else
		{
			spellTransform = trans;
			spellTransform.right = Tool2D.GetDir();
			trans.Find("SR_Spell").GetComponent<SpriteRenderer>().material.SetFloat(AddGaintArrowColor, AddSubArrowColor ? 1 : 0);
		}
	}

	protected override void Update()
	{
		base.Update();
		UpdateSpellRotate();
	}

	private void UpdateSpellRotate()
	{
		if ((bool)spellTransform)
		{
			spellTransform.eulerAngles = new Vector3(0f, 0f, spellTransform.eulerAngles.z + rotateSpeed * (float)rotateDir * Time.deltaTime);
			arrowScript.UpdateShadowDir(spellTransform.right);
		}
	}
}
