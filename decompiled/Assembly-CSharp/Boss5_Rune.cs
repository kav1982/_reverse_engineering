using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Rune : MonoBehaviour
{
	public enum colorState
	{
		Hide,
		Show,
		Active,
		InActive,
		Born
	}

	public float allowedOffset;

	public float fromBorder;

	public SpriteRenderer mainSprite;

	public Sprite sprite1;

	public Sprite sprite2;

	public Sprite sprite3;

	public Sprite sprite4;

	public ParticleSystem bornParticle;

	public ParticleSystem liveParticle;

	public ParticleSystem triggerParticle;

	[Header("颜色")]
	public float shineFrequency;

	public float shineAmplitude;

	private float nowColor;

	public float inActiveColor;

	public float colorChangeSpeed;

	public float showMaxColor;

	public float showMaxTime;

	public float showTotalTime;

	public float bornMaxTime;

	public float bornTotalTime;

	private bool showMaxColored;

	[Header("线状提示")]
	public LineRenderer signRenderer;

	public float connectionMiddlePointHeight;

	public float connectionMiddle2PointHeight;

	public int connectionNodeCount;

	[Header("子弹")]
	public Transform attackBullet;

	public Vector3 bulletFlyOffset;

	private bool bulletLaunched;

	public bool bulletReached;

	public ParticleSystem bulletTrailParticle;

	public ParticleSystem bulletExplosionParticle;

	public float lerpTime;

	private Vector3 toTargetDir;

	private float nowLerp;

	[Header("状态机")]
	public colorState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public static int order;

	private int index;

	private static List<float> runeQuadrant = new List<float> { 1f, 2f, 3f, 4f };

	public colorState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
		}
	}

	public static void Rearrange()
	{
		GeneralTool.RandomizeList(runeQuadrant);
	}

	public void Initialize(int index)
	{
		this.index = index;
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		if (runeQuadrant[index - 1] == 1f)
		{
			base.transform.position = new Vector3(UnityEngine.Random.Range(roomCenterPoint.x - allowedOffset, roomCenterPoint.x + roomWidth / 2f - fromBorder), UnityEngine.Random.Range(roomCenterPoint.y + roomHeight / 2f - fromBorder, roomCenterPoint.y - allowedOffset), 0f);
			mainSprite.sprite = sprite1;
		}
		else if (runeQuadrant[index - 1] == 2f)
		{
			mainSprite.sprite = sprite2;
			base.transform.position = new Vector3(UnityEngine.Random.Range(roomCenterPoint.x - roomWidth / 2f + fromBorder, roomCenterPoint.x + allowedOffset), UnityEngine.Random.Range(roomCenterPoint.y - allowedOffset, roomCenterPoint.y + roomHeight / 2f - fromBorder), 0f);
		}
		else if (runeQuadrant[index - 1] == 3f)
		{
			mainSprite.sprite = sprite3;
			base.transform.position = new Vector3(UnityEngine.Random.Range(roomCenterPoint.x - roomWidth / 2f + fromBorder, roomCenterPoint.x + allowedOffset), UnityEngine.Random.Range(roomCenterPoint.y - roomHeight / 2f + fromBorder, roomCenterPoint.y + allowedOffset), 0f);
		}
		else if (runeQuadrant[index - 1] == 4f)
		{
			mainSprite.sprite = sprite4;
			base.transform.position = new Vector3(UnityEngine.Random.Range(roomCenterPoint.x - allowedOffset, roomCenterPoint.x + roomWidth / 2f - fromBorder), UnityEngine.Random.Range(roomCenterPoint.y - roomHeight / 2f + fromBorder, roomCenterPoint.y + allowedOffset), 0f);
		}
		base.transform.position = new Vector3(Mathf.Round(base.transform.position.x) + 0.5f, Mathf.Round(base.transform.position.y) - 0.5f, 0f);
		state = colorState.Born;
		signRenderer.positionCount = connectionNodeCount;
	}

	private void Update()
	{
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case colorState.Hide:
			if (changedState)
			{
				bulletReached = false;
				liveParticle.Stop();
			}
			if (nowColor > 0f)
			{
				nowColor -= Time.deltaTime * colorChangeSpeed;
			}
			else
			{
				nowColor = 0f;
			}
			break;
		case colorState.Born:
			if (changedState)
			{
				showMaxColored = false;
				bornParticle.Play();
			}
			if (!showMaxColored)
			{
				nowColor += Time.deltaTime * showMaxColor / bornMaxTime;
				if (nowColor > showMaxColor)
				{
					showMaxColored = true;
					bornParticle.Stop();
				}
			}
			else if (nowColor > 1f)
			{
				nowColor -= Time.deltaTime * (showMaxColor - 1f) / (bornTotalTime - bornMaxTime);
			}
			if (stateExistTime > bornTotalTime)
			{
				state = colorState.Active;
			}
			break;
		case colorState.Show:
			if (changedState)
			{
				showMaxColored = false;
			}
			if (!showMaxColored)
			{
				nowColor += Time.deltaTime * showMaxColor / showMaxTime;
				if (nowColor > showMaxColor)
				{
					showMaxColored = true;
				}
			}
			else if (nowColor > 1f)
			{
				nowColor -= Time.deltaTime * (showMaxColor - 1f) / (showTotalTime - showMaxTime);
			}
			if (stateExistTime > showTotalTime)
			{
				state = colorState.Active;
			}
			break;
		case colorState.Active:
			if (changedState)
			{
				liveParticle.Play();
			}
			nowColor = Mathf.Sin(stateExistTime * shineFrequency * 2f * MathF.PI) * shineAmplitude + 1f;
			if (Boss5.Inst.state == Boss5.MonsterState.Shield || order != index)
			{
				state = colorState.InActive;
			}
			else
			{
				CheckEnteracted();
			}
			break;
		case colorState.InActive:
			if (changedState)
			{
				liveParticle.Stop();
			}
			if (Boss5.Inst.state != Boss5.MonsterState.Shield && order == index)
			{
				state = colorState.Show;
			}
			else if (nowColor > inActiveColor)
			{
				nowColor -= Time.deltaTime * colorChangeSpeed;
			}
			break;
		}
		Vector3 position = base.transform.position;
		Vector3 position2 = Boss5.Inst.transform.position;
		Vector3 v = position + new Vector3(0f, 0f, 0f - connectionMiddlePointHeight);
		Vector3 v2 = position2 + new Vector3(0f, 0f, 0f - connectionMiddle2PointHeight);
		for (int i = 0; i < connectionNodeCount; i++)
		{
			Vector3 rootPoint = GeneralTool.CubicBezierCurve(position, v, v2, position2, (float)i / ((float)connectionNodeCount - 1f));
			signRenderer.SetPosition(i, Tool2D.GetLayerPoint(rootPoint));
		}
		signRenderer.material.SetFloat("_Transparency", nowColor);
		mainSprite.material.SetFloat("_Transparency", nowColor);
		if (!bulletLaunched)
		{
			return;
		}
		if (!bulletReached)
		{
			nowLerp += Time.deltaTime / lerpTime;
			attackBullet.position = GeneralTool.QuadraticBezierCurve(Tool2D.GetLayerPoint(base.transform.position), Tool2D.GetLayerPoint(base.transform.position + bulletFlyOffset), Boss5.Inst.shieldTransform.position - toTargetDir, nowLerp);
			attackBullet.right = Tool2D.IgnoreZPoint(attackBullet.position - GeneralTool.QuadraticBezierCurve(Tool2D.GetLayerPoint(base.transform.position), Tool2D.GetLayerPoint(base.transform.position + bulletFlyOffset), Boss5.Inst.shieldTransform.position - toTargetDir, nowLerp - 0.01f));
			if (nowLerp > 1f)
			{
				bulletTrailParticle.Stop();
				bulletExplosionParticle.transform.position = attackBullet.position;
				bulletExplosionParticle.transform.right = attackBullet.right;
				bulletExplosionParticle.Play();
				bulletReached = true;
				Boss5.Inst.ShieldHit();
			}
		}
		_ = bulletReached;
	}

	public void BulletReset()
	{
		bulletReached = false;
		bulletLaunched = false;
		Mute();
	}

	private void CheckEnteracted()
	{
		float num = PlayerMgr.Inst.PlayerCtrller.myPpt.CC_Self.radius + 0.5f;
		if ((PlayerMgr.Inst.PlayerCtrller.transform.position - base.transform.position).sqrMagnitude < num * num)
		{
			SEMgr.Inst.boss5_RuneTrigger.PlaySE();
			nowLerp = 0f;
			toTargetDir = (Tool2D.IgnoreZPoint(Boss5.Inst.shieldTransform.position) - base.transform.position).normalized * 1.5f;
			attackBullet.transform.position = Tool2D.GetLayerPoint(base.transform.position);
			liveParticle.Stop();
			bulletTrailParticle.Play();
			triggerParticle.Play();
			bulletLaunched = true;
			state = colorState.Hide;
			order++;
		}
	}

	public void Mute()
	{
		state = colorState.Hide;
	}

	public void Active()
	{
		state = colorState.Show;
	}
}
