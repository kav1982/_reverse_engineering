using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1010Effect : SpellEffectBase
{
	private LineRenderer body;

	private LineRenderer head;

	private LineRenderer shadow;

	public float baseBodyWidth = 0.2f;

	public float baseBodyTrailWidth = 0.4f;

	private readonly List<(Vector3 pos, float dis)> positions = new List<(Vector3, float)>();

	private float bodyLength;

	private float _disappearSpeed;

	private float maxDis;

	private static readonly int SpeedID = Shader.PropertyToID("_Speed");

	public float disappearSpeed
	{
		get
		{
			return _disappearSpeed;
		}
		set
		{
			_disappearSpeed = value;
			if ((bool)body)
			{
				body.material.SetFloat(SpeedID, _disappearSpeed);
			}
			if ((bool)shadow)
			{
				shadow.material.SetFloat(SpeedID, _disappearSpeed);
			}
		}
	}

	protected override void OnFirstFrame()
	{
		base.OnFirstFrame();
		disappearSpeed = 0f;
		maxDis = base.Spell.spellCfg.float1;
		ShowHead();
	}

	public void PushNewPosition(Vector3 position)
	{
		if (!base.FirstFrameIsRun || !firstUpdateIsRun)
		{
			return;
		}
		if (positions.Count == 0)
		{
			positions.Insert(0, (position, 0f));
			return;
		}
		float sqrMagnitude = (positions[0].pos - position).sqrMagnitude;
		if (!(sqrMagnitude < 0.0225f))
		{
			float num = Mathf.Sqrt(sqrMagnitude);
			bodyLength += num;
			positions.Insert(0, (position, num));
			CleanPositions();
			ApplyPositionsToEffect();
		}
	}

	private void OnDisable()
	{
		body = null;
		head = null;
		shadow = null;
		positions.Clear();
		bodyLength = 0f;
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		LineRenderer[] componentsInChildren = trans.GetComponentsInChildren<LineRenderer>();
		foreach (LineRenderer lineRenderer in componentsInChildren)
		{
			if (lineRenderer.name == "Body")
			{
				lineRenderer.widthMultiplier = baseBodyWidth * base.transform.localScale.x;
			}
			else
			{
				lineRenderer.widthMultiplier = baseBodyTrailWidth * base.transform.localScale.x;
			}
		}
		if (effect.Name == "Spell")
		{
			body = trans.Find("Body").GetComponent<LineRenderer>();
			body.positionCount = 0;
			body.material.SetFloat(SpeedID, 0f);
			Transform transform = trans.Find("HeadTrail");
			if ((bool)transform)
			{
				head = transform.GetComponentInParent<LineRenderer>();
				head.positionCount = 0;
			}
		}
		else if (effect.Name == "Shadow")
		{
			trans.Find("FallShadow").gameObject.SetActive(base.Spell.SIP.spellIsFall);
			trans.Find("ModelShadow").gameObject.SetActive(!base.Spell.SIP.spellIsFall);
			shadow = trans.GetComponentInChildren<LineRenderer>();
			shadow.positionCount = 0;
			shadow.material.SetFloat(SpeedID, 0f);
		}
	}

	public Vector3[] GetBodyPoints()
	{
		return positions.Select(((Vector3 pos, float dis) e) => e.pos).ToArray();
	}

	protected override void Update()
	{
		base.Update();
		if ((bool)body)
		{
			body.widthMultiplier = baseBodyWidth * base.Spell.tsf_Layer.localScale.x;
		}
		if ((bool)shadow)
		{
			shadow.widthMultiplier = baseBodyTrailWidth * base.Spell.tsf_Layer.localScale.x;
		}
		if (!(disappearSpeed <= 0f))
		{
			float num = disappearSpeed * Time.deltaTime;
			maxDis -= num;
			CleanPositions();
			ApplyPositionsToEffect();
		}
	}

	private void CleanPositions()
	{
		while (bodyLength > maxDis && positions.Count > 2)
		{
			float num = bodyLength;
			List<(Vector3 pos, float dis)> list = positions;
			bodyLength = num - list[list.Count - 1].dis;
			positions.RemoveAt(positions.Count - 1);
		}
	}

	private void ApplyPositionsToEffect()
	{
		Vector3[] array = new Vector3[positions.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Tool2D.GetLayerPoint(positions[i].pos);
		}
		if ((bool)body)
		{
			body.positionCount = array.Length;
			body.SetPositions(array);
		}
		if ((bool)head)
		{
			head.positionCount = array.Length;
			head.SetPositions(array);
		}
		if ((bool)shadow)
		{
			shadow.positionCount = array.Length;
			shadow.SetPositions(positions.Select(((Vector3 pos, float dis) e) => Tool2D.GetLayerPoint(e.pos.IgnoreZ(), LayerCorrectType.Shadow)).ToArray());
		}
	}

	public void HideHead()
	{
		if (!(body == null) && !(shadow == null))
		{
			body.GetComponentInParent<SpriteRenderer>().enabled = false;
			shadow.transform.parent.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}
	}

	public void ShowHead()
	{
		if (!(body == null) && !(shadow == null))
		{
			body.GetComponentInParent<SpriteRenderer>().enabled = true;
			shadow.transform.parent.GetComponentInChildren<SpriteRenderer>().enabled = true;
		}
	}

	public void OffsetBody(Vector3 offset)
	{
		if (positions.Count > 0)
		{
			for (int i = 0; i < positions.Count; i++)
			{
				(Vector3, float) value = positions[i];
				value.Item1 += offset;
				positions[i] = value;
			}
		}
	}
}
