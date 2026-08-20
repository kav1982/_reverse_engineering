using System.Collections;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;

public class Boss56Elite52RoadRoller : MonoBehaviour
{
	[Header("Boss面向状态")]
	public Transform ModelTransform;

	private bool isFaceRight = true;

	private bool lockCurrentFaceDirection;

	private float modelScaleX = 1f;

	public float FaceDirectionChangeDuration;

	public Animator Anima;

	private Entity shooterEntity;

	public ShockParam shock;

	[Header("Warning")]
	public LineRenderer warningLine1;

	public LineRenderer warningLine2;

	public float warningLineLength;

	public float MaxJumpHeight;

	public float JumpPrepareTime;

	public float JumpFallingAt;

	public float JumpingXYPosLerpDuration;

	public float JumpingZPosLerpDuration;

	public float LandingZPosLerpDuration;

	public float AfterJumpBackToNormalDuration;

	private Vector3 targetPoint;

	private float rotateAngle;

	private float bonusWaitTime;

	[Header("中央下压弹幕")]
	private float CB_BulletCount;

	private float CB_BulletSpeed;

	private float CB_BulletDuration;

	[Header("边墙下压弹幕")]
	private float SB_BulletCount;

	private float SB_BulletSpeed;

	private float SB_BulletDuration;

	private void OnEnable()
	{
		HideDropWarning();
	}

	public void InitializeData(Entity shooterEntity)
	{
		this.shooterEntity = shooterEntity;
	}

	public void AttackNewTarget(Vector3 targetPoint, float rotateAngle, float bonusFallWaitTime = 0f)
	{
		this.targetPoint = targetPoint;
		this.rotateAngle = rotateAngle;
		bonusWaitTime = bonusFallWaitTime;
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(StartAttack());
		}
	}

	private IEnumerator StartAttack()
	{
		Anima.Play("JumpBefore");
		yield return new WaitForSeconds(JumpPrepareTime);
		ShowDropWarning(targetPoint);
		Anima.Play("Jump");
		SEMgr.Inst.monster310_Jump.PlaySE();
		base.transform.position = base.transform.position.IgnoreZ();
		base.transform.DOMoveX(targetPoint.x, JumpingXYPosLerpDuration).SetEase(Ease.InOutSine);
		base.transform.DOMoveY(targetPoint.y, JumpingXYPosLerpDuration).SetEase(Ease.InOutSine);
		base.transform.DOMoveZ(0f - MaxJumpHeight, JumpingZPosLerpDuration);
		yield return new WaitForSeconds(JumpFallingAt + bonusWaitTime);
		base.transform.DOMoveZ(0f, LandingZPosLerpDuration).SetEase(Ease.InOutBounce);
		yield return new WaitForSeconds(LandingZPosLerpDuration);
		Anima.Play("JumpAfter");
		HideDropWarning();
		CreateDropPattern();
	}

	public void Update()
	{
	}

	private void UpdateFaceDirection(bool instantLerp = false)
	{
		if (!lockCurrentFaceDirection)
		{
			float num = (isFaceRight ? Mathf.Abs(modelScaleX) : (0f - Mathf.Abs(modelScaleX)));
			if (instantLerp)
			{
				num = Mathf.Lerp(base.transform.localScale.x, num, 10f * Time.deltaTime);
				ModelTransform.localScale = new Vector3(num, ModelTransform.localScale.y, ModelTransform.localScale.z);
			}
			else
			{
				ModelTransform.DOScaleX(num, FaceDirectionChangeDuration);
			}
		}
	}

	private void FaceToPlayer()
	{
		isFaceRight = PlayerMgr.Inst.PlayerPoint.x >= base.transform.position.x;
	}

	private void CreateDropPattern()
	{
		CamController.Inst.SetShock(shock);
		SEMgr.Inst.boss51LineAttack.PlaySE().pitch = 1.2f;
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite52_Drop", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<Elite52_Drop>().Initialize(shooterEntity, useCrossDrop: true, useTargetAngle: true, rotateAngle);
	}

	private void ShowDropWarning(Vector3 center)
	{
		Vector3 dir = Tool2D.GetDir(rotateAngle);
		Vector3 dir2 = Tool2D.GetDir(rotateAngle + 90f);
		SetWarningLine(warningLine1, center, dir);
		SetWarningLine(warningLine2, center, dir2);
	}

	private void SetWarningLine(LineRenderer line, Vector3 center, Vector3 dir)
	{
		if (!(line == null))
		{
			line.positionCount = 10;
			line.enabled = true;
			Vector3 a = center - dir * warningLineLength * 0.5f;
			Vector3 b = center + dir * warningLineLength * 0.5f;
			for (int i = 0; i < line.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(a, b, (float)i / (float)(line.positionCount - 1));
				line.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
		}
	}

	private void HideDropWarning()
	{
		if (warningLine1 != null)
		{
			warningLine1.enabled = false;
		}
		if (warningLine2 != null)
		{
			warningLine2.enabled = false;
		}
	}
}
