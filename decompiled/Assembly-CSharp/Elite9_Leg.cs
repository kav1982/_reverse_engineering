using System;
using System.Collections.Generic;
using UnityEngine;

public class Elite9_Leg : MonoBehaviour
{
	public enum LegState
	{
		Idle,
		Move,
		Floating
	}

	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public LineRenderer lr_Leg2;

	public float moveSpeed;

	public float rootOffsetX;

	public float normalDistance;

	public float middleHeight;

	public float leg2ExtraLength;

	public float rootOffsetXFirstFix;

	public float normalDistanceFirstFix;

	public float middleHeightFirstFix;

	public float legThickFirstFix;

	public float moveSpeedFirstFix;

	public float firstLegRepositionAngle;

	public float firstLegMaxAngle;

	public VariableFloat legMoveAngle;

	public Elite9 master;

	public float middlePointFix;

	public float repositionFix;

	public LegState state;

	private Elite9_Body body;

	private bool leftLeg;

	private Vector3 currentEndPoint;

	private Vector3 moveToEndPoint;

	public AudioSource as_Leg;

	public List<AudioClip> legSound = new List<AudioClip>();

	[Header("和谐模式")]
	public Material mt_Leg_H;

	public Material mt_Leg2_H;

	private Vector3 OriginalPoint
	{
		get
		{
			if (!body.HaveFront)
			{
				return body.transform.position + Tool2D.GetDir(body.moveDir, leftLeg ? (-90) : 90) * rootOffsetX * rootOffsetXFirstFix;
			}
			return body.transform.position + Tool2D.GetDir(body.moveDir, leftLeg ? (-90) : 90) * rootOffsetX;
		}
	}

	private void OnEnable()
	{
		as_Leg.clip = legSound[UnityEngine.Random.Range(0, legSound.Count)];
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Leg.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Update()
	{
		switch (state)
		{
		case LegState.Idle:
			if (!body.HaveFront && Vector3.Angle(body.moveDir, -Tool2D.IgnoreZPoint(OriginalPoint - currentEndPoint)) > firstLegMaxAngle)
			{
				state = LegState.Move;
				moveToEndPoint = OriginalPoint + Tool2D.GetDir(body.moveDir, leftLeg ? (0f - firstLegRepositionAngle) : firstLegRepositionAngle) * normalDistance;
				if (Physics.Raycast(OriginalPoint, moveToEndPoint - OriginalPoint, out var hitInfo, 100f, LayerMask.GetMask("Wall", "Abyss", "Cliff")) && (OriginalPoint - hitInfo.point).sqrMagnitude < (OriginalPoint - moveToEndPoint).sqrMagnitude)
				{
					moveToEndPoint = Tool2D.IgnoreZPoint(hitInfo.point);
				}
			}
			else if ((OriginalPoint - currentEndPoint).sqrMagnitude > normalDistance * normalDistance * repositionFix * repositionFix)
			{
				as_Leg.Play(0uL);
				state = LegState.Move;
				legMoveAngle.RandomResult();
				moveToEndPoint = OriginalPoint + Tool2D.GetDir(body.moveDir, leftLeg ? (0f - legMoveAngle.result) : legMoveAngle.result) * normalDistance;
				if (Physics.Raycast(OriginalPoint, moveToEndPoint - OriginalPoint, out var hitInfo2, 100f, LayerMask.GetMask("Wall", "Abyss")) && (OriginalPoint - hitInfo2.point).sqrMagnitude < (OriginalPoint - moveToEndPoint).sqrMagnitude)
				{
					moveToEndPoint = Tool2D.IgnoreZPoint(hitInfo2.point);
				}
			}
			break;
		case LegState.Move:
			if (!body.HaveFront)
			{
				currentEndPoint = Vector3.MoveTowards(currentEndPoint, moveToEndPoint, moveSpeed * Time.deltaTime * moveSpeedFirstFix);
			}
			else
			{
				currentEndPoint = Vector3.MoveTowards(currentEndPoint, moveToEndPoint, moveSpeed * Time.deltaTime);
			}
			if (currentEndPoint == moveToEndPoint)
			{
				state = LegState.Idle;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
		Vector3 vector = OriginalPoint + (currentEndPoint - OriginalPoint) * middlePointFix + new Vector3(0f, 0f, 0f - middleHeight);
		Vector3 vector2 = OriginalPoint + new Vector3(0f, 0f, 0f - body.rootHeight);
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(vector2, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(currentEndPoint, 1.05f));
		lr_Leg.SetPosition(0, Tool2D.GetLayerPoint(vector2));
		lr_Leg.SetPosition(1, Tool2D.GetLayerPoint(vector));
		lr_Leg2.SetPosition(0, Tool2D.GetLayerPoint(vector - (currentEndPoint - vector) * leg2ExtraLength));
		lr_Leg2.SetPosition(1, Tool2D.GetLayerPoint(vector));
		lr_Leg2.SetPosition(2, Tool2D.GetLayerPoint(currentEndPoint));
		if (body.moveDir.y < 0f)
		{
			if (!leftLeg)
			{
				lr_Leg.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
				lr_Leg.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
				lr_Leg2.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
				lr_Leg2.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
			}
			else
			{
				lr_Leg.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
				lr_Leg.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
				lr_Leg2.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
				lr_Leg2.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
			}
		}
		else if (leftLeg)
		{
			lr_Leg.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_Leg.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
			lr_Leg2.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_Leg2.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
		}
		else
		{
			lr_Leg.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_Leg.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
			lr_Leg2.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_Leg2.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
		}
		if (master.myPpt.SR_Models[0] != null && lr_Leg.startColor != master.myPpt.BaseColor)
		{
			lr_Leg.startColor = master.myPpt.BaseColor;
			lr_Leg.endColor = master.myPpt.BaseColor;
			lr_Leg2.startColor = master.myPpt.BaseColor;
			lr_Leg2.endColor = master.myPpt.BaseColor;
		}
	}

	public void SingleInitial(Elite9 master, Elite9_Body body, bool leftLeg)
	{
		lr_Leg.positionCount = 2;
		lr_Leg2.positionCount = 3;
		this.master = master;
		this.body = body;
		this.leftLeg = leftLeg;
		if (!body.HaveFront)
		{
			lr_Leg.widthMultiplier *= legThickFirstFix;
			lr_Leg2.widthMultiplier *= legThickFirstFix;
			middleHeight *= middleHeightFirstFix;
			normalDistance *= normalDistanceFirstFix;
		}
		if (GameMgr.IsHarmony_Static)
		{
			UnityEngine.Object.Destroy(lr_Leg.material);
			UnityEngine.Object.Destroy(lr_Leg2.material);
			lr_Leg.material = mt_Leg_H;
			lr_Leg2.material = mt_Leg2_H;
		}
		if (!leftLeg)
		{
			lr_Leg.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_Leg.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
			lr_Leg2.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_Leg2.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
		}
		else
		{
			lr_Leg.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_Leg.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
			lr_Leg2.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_Leg2.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
		}
	}

	public void EveryInitial()
	{
		lr_Leg.SetPosition(0, Vector3.zero);
		lr_Leg.SetPosition(1, Vector3.zero);
		lr_Leg2.SetPosition(0, Vector3.zero);
		lr_Leg2.SetPosition(1, Vector3.zero);
		lr_Leg2.SetPosition(2, Vector3.zero);
		lr_Shadow.SetPosition(0, Vector3.zero);
		lr_Shadow.SetPosition(1, Vector3.zero);
	}

	public void Frame1Initail()
	{
		moveToEndPoint = OriginalPoint + Tool2D.GetDir(body.moveDir, leftLeg ? (-90) : 90) * normalDistance;
		currentEndPoint = moveToEndPoint;
		Update();
	}
}
