using System;
using UnityEngine;

public class Spell1030Effect : SpellEffectBase
{
	private Spell1030Harpoons harpoon;

	private LineRenderer ropeLine;

	private Transform harpoonsTipTrans;

	private Transform harpoonsTipShadowTrans;

	private Transform harpooStartNodeTrans;

	protected override void Awake()
	{
		base.Awake();
		harpoon = (Spell1030Harpoons)base.Spell;
	}

	private void OnDisable()
	{
		ropeLine = null;
		harpoonsTipTrans = null;
		harpooStartNodeTrans = null;
		harpoonsTipShadowTrans = null;
	}

	protected override void Update()
	{
		base.Update();
	}

	private void LateUpdate()
	{
		UpdateRopePoint();
		UpdateStartNodePosition();
		UpdateHarpoonsTipDir();
	}

	private void UpdateStartNodePosition()
	{
		if ((bool)harpooStartNodeTrans)
		{
			harpooStartNodeTrans.position = harpoon.GetAroundTargetBasePoint() + new Vector3(0f, 0.3f, -0.3f);
		}
	}

	private void UpdateHarpoonsTipDir()
	{
		if ((bool)harpoonsTipTrans && (bool)harpoonsTipShadowTrans)
		{
			Vector3 zero = Vector3.zero;
			switch (harpoon.currentState)
			{
			case Spell1030Harpoons.HarpoonsState.Shooting:
			case Spell1030Harpoons.HarpoonsState.Holding:
				zero = new Vector2(harpoon.Direction.x * harpoon.CurrentSpeed, harpoon.CurrentUpSpeed + harpoon.Direction.y * harpoon.CurrentSpeed);
				break;
			case Spell1030Harpoons.HarpoonsState.HookHolding:
				zero = (base.transform.position - harpoon.GetAroundTargetBasePoint()).normalized;
				break;
			case Spell1030Harpoons.HarpoonsState.PullingBack:
				zero = ((harpoon.currentSpellMovement == SpellSpecialMovementType.Rotation) ? ((Vector3)new Vector2(harpoon.Direction.x * harpoon.CurrentSpeed, harpoon.CurrentUpSpeed + harpoon.Direction.y * harpoon.CurrentSpeed)) : (base.transform.position - harpoon.GetAroundTargetBasePoint()).normalized);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			float z = Vector2.SignedAngle(Vector2.right, (Vector2)zero);
			Quaternion rotation = Quaternion.Euler(0f, 0f, z);
			harpoonsTipTrans.rotation = rotation;
			harpoonsTipShadowTrans.position = harpoonsTipTrans.position + new Vector3(0f, harpoon.transform.position.z, 1.05f);
			harpoonsTipShadowTrans.localPosition += new Vector3(0.2f, 0f, 0f);
		}
	}

	private void UpdateRopePoint()
	{
		if ((bool)ropeLine)
		{
			ropeLine.positionCount = 2;
			Vector3 vector = new Vector3(0f, 0.3f, -0.3f);
			ropeLine.SetPosition(0, harpoon.transform.position + vector);
			ropeLine.SetPosition(1, harpoon.GetAroundTargetBasePoint() + vector);
		}
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		string text = effect.Name;
		switch (text)
		{
		default:
			_ = text == "Hit";
			break;
		case "Rope":
			ropeLine = trans.Find("Rope").GetComponent<LineRenderer>();
			ropeLine.positionCount = 2;
			break;
		case "StartGate":
			harpooStartNodeTrans = trans;
			break;
		case "HarpoonsTip":
			harpoonsTipTrans = trans;
			harpoonsTipShadowTrans = trans.Find("Shadow");
			break;
		}
	}
}
