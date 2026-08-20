using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Spell1018Effect : SpellEffectBase
{
	private static readonly int _materialAlphaField = Shader.PropertyToID("_OveralAlpha");

	public MaterialsByColorType ChainMaterials = new MaterialsByColorType();

	public float widthRatio;

	public float minWidth;

	public float maxWidth;

	public float defaultChainWidth;

	private static readonly int HighlightsPositionID = Shader.PropertyToID("_HighlightsPosition");

	private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

	private Transform fallLighting;

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		if (effect.Name == "Spell")
		{
			SpriteRenderer component = trans.Find("Center").GetComponent<SpriteRenderer>();
			component.material.SetFloat(_materialAlphaField, 0f);
			component.material.DOFloat(1f, _materialAlphaField, 0.1f);
		}
		else if (effect.Name == "FallLighting")
		{
			fallLighting = trans;
		}
	}

	protected override void OnWillRecycleEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnWillRecycleEffect(effect, trans);
		if (effect.Name == "Spell")
		{
			trans.Find("Center").GetComponent<SpriteRenderer>().material.DOFloat(0f, _materialAlphaField, 0.1f);
		}
	}

	public void CreateChainEffect(Vector3[] positions)
	{
		if ((!SpellEffectBase.FullTransparency || !IgnoreEffectCreateWhenFullTransparency) && positions.Length != 0)
		{
			for (int i = 0; i < positions.Length; i++)
			{
				positions[i] += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), -0.2f);
			}
			CreateChain(positions[0] + new Vector3(0f, 30f, -30f), positions[0]);
			for (int j = 0; j < positions.Length - 1; j++)
			{
				CreateChain(positions[j], positions[j + 1]);
			}
		}
	}

	private void CreateChain(Vector3 startPos, Vector3 endPos)
	{
		LineRenderer component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/10181/10181_Chain", 0.3f).GetComponent<LineRenderer>();
		component.SetPosition(0, Tool2D.GetLayerPoint(startPos));
		component.SetPosition(1, Tool2D.GetLayerPoint(endPos));
		component.sharedMaterial = ChainMaterials.Get(base.Spell.ColorType);
		float num = Mathf.Pow(base.Spell.spellCfg.damage / SpellConfig.dic[base.Spell.spellCfg.id].damage, 0.5f);
		num *= widthRatio;
		num = Mathf.Clamp(num, minWidth, maxWidth);
		component.widthMultiplier = num * defaultChainWidth;
	}

	public void CreateFallLighting(Vector3[] positions, bool isRebounce)
	{
		int num = positions.Length;
		if (num > 3 || num < 2)
		{
			Debug.LogError("反弹所需要的点不足");
			return;
		}
		ManualCreateEffect("FallLighting");
		if (fallLighting == null)
		{
			DOTween.Sequence().AppendInterval(0.1f).AppendCallback(delegate
			{
				((Spell1018ThunderAura)base.Spell).OnFallingLightingOnGround(positions[^1]);
			});
			return;
		}
		LineRenderer component = fallLighting.GetComponent<LineRenderer>();
		component.material.SetFloat(HighlightsPositionID, 0f);
		Color color = component.material.GetColor(BaseColorID);
		color.a = 1f;
		component.material.SetColor(BaseColorID, color);
		DOTween.Sequence().Append(component.material.DOFloat(1f, HighlightsPositionID, 0.1f)).AppendCallback(delegate
		{
			((Spell1018ThunderAura)base.Spell).OnFallingLightingOnGround(positions[^1]);
		})
			.Append(component.material.DOColor(new Color(0f, 0f, 0f, 0f), BaseColorID, 0.15f));
		if (isRebounce)
		{
			Vector3 vector = default(Vector3);
			Vector3 vector2 = default(Vector3);
			Vector3 v = default(Vector3);
			if (positions.Length == 2)
			{
				vector = Tool2D.IgnoreZPoint(positions[0]);
				vector2 = Tool2D.IgnoreZPoint(positions[1]);
				v = (vector + vector2) / 2f;
				v.y += 6f;
			}
			else if (positions.Length == 3)
			{
				vector = Tool2D.IgnoreZPoint(positions[0]);
				v = Tool2D.IgnoreZPoint(positions[1]);
				vector2 = Tool2D.IgnoreZPoint(positions[2]);
				v.y += 6f;
			}
			component.positionCount = 21;
			for (int i = 0; i <= 20; i++)
			{
				Vector3 position = GeneralTool.QuadraticBezierCurve(vector, v, vector2, (float)i / 20f);
				component.SetPosition(i, position);
			}
		}
		else
		{
			component.positionCount = positions.Length;
			component.SetPositions(positions.Select(Tool2D.GetLayerPoint).ToArray());
		}
	}
}
