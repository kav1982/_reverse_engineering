using System;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
	public Camera cam_Main;

	public Camera cam_UI;

	public Transform tsf_Shock;

	public Transform tsf_Focus;

	[Header("MouseOffset")]
	public float mouseOffset;

	public float mouseOffsetMobile;

	public float mouseLerp;

	[Header("follow")]
	public float followThemeLimitX;

	public float followThemeLimitYUp;

	public float followThemeLimitYDown;

	public float followThemeLimitX_T8;

	public float followThemeLimitYUp_T8;

	public float followThemeLimitYDown_T8;

	public float followLerp;

	[Header("Chapter3")]
	public GameObject go_Chapter3CamRoot;

	public Transform tsf_CamUpRoot;

	public Transform tsf_CamRightRoot;

	public Transform tsf_CamDownRoot;

	public Transform tsf_CamLeftRoot;

	public RTCamController camCtrller_Up;

	public RTCamController camCtrller_Left;

	public RTCamController camCtrller_Down;

	public RTCamController camCtrller_Right;

	public MeshRenderer mr_Up;

	public MeshRenderer mr_Left;

	public MeshRenderer mr_Down;

	public MeshRenderer mr_Right;

	public float camMeterPerSize;

	public int upRightDownLeftCamShowPeter;

	[Header("boss距离过远时额外视距")]
	public float specificFocuSizeSmoothTime;

	private float specificBorderConstraintTargetValue;

	private float specificBorderConstraintChangeSpeed;

	public float specificBorderConstraint;

	private Transform followTargetT;

	private bool pauseMouseOffset;

	private Vector3 currentMouseOffset;

	private bool isShocking;

	private bool isShockOut = true;

	private Vector3 shockDir;

	private ShockParam currentShockParam;

	private float shockTimer;

	private float focusOriginalSize;

	private float focusTargetSize;

	private Vector3 focusPoint;

	private Transform focusTransform;

	private bool isFocussingTransform;

	private float focusSizeSpeed = 1f;

	private float focusMoveSpeed = 1f;

	private float lastFrameFocusProgressTime = 0.33f;

	private float specificFocusTargetSize;

	private float specificFocusSizeLastFrame;

	private float specificFocusSize;

	private float specificFocusMoveSpeed;

	public List<CameraFocusSizeData> ExtraFocusSizeRequirementList = new List<CameraFocusSizeData>();

	[HideInInspector]
	public bool playerInRotateDash;

	[HideInInspector]
	public Vector3 dashCamFollowPoint;

	private int lastRsolutionWidth;

	public bool finialClampAfterFocus;

	private Vector3 dampSpeed;

	private float _sizeVel;

	public static CamController Inst { get; private set; }

	public float FocusCamSizeRatio { get; set; } = 1f;


	private float FinalLimitX
	{
		get
		{
			float num = ((LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme8_Chapter4) ? followThemeLimitX_T8 : followThemeLimitX) + LevelMgr.Inst.CurrentRoomCfg.camExtraLimitX;
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_LongNeck != null)
			{
				num += PlayerMgr.Inst.ItemCtrller.relicCfg_LongNeck.float3.result;
			}
			return num + specificBorderConstraint;
		}
	}

	private float FinalLimitYUp
	{
		get
		{
			float num = ((LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme8_Chapter4) ? followThemeLimitYUp_T8 : followThemeLimitYUp) + LevelMgr.Inst.CurrentRoomCfg.camExtraLimitYUp;
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_LongNeck != null)
			{
				num += PlayerMgr.Inst.ItemCtrller.relicCfg_LongNeck.float3.result;
			}
			return num + specificBorderConstraint;
		}
	}

	private float FinalLimitYDown
	{
		get
		{
			float num = ((LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme8_Chapter4) ? followThemeLimitYDown_T8 : followThemeLimitYDown) + LevelMgr.Inst.CurrentRoomCfg.camExtraLimitYDown;
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_LongNeck != null)
			{
				num += PlayerMgr.Inst.ItemCtrller.relicCfg_LongNeck.float3.result;
			}
			return num + specificBorderConstraint;
		}
	}

	public bool isFocussing { get; private set; }

	public float FocusOriginalSize
	{
		get
		{
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_LongNeck != null)
			{
				return focusOriginalSize + PlayerMgr.Inst.ItemCtrller.relicCfg_LongNeck.float2.result;
			}
			return focusOriginalSize;
		}
	}

	private void OnEnable()
	{
		EventMgr.OnChangeResolution = (Action)Delegate.Combine(EventMgr.OnChangeResolution, new Action(OnChangeResolution));
	}

	private void OnDisable()
	{
		EventMgr.OnChangeResolution = (Action)Delegate.Remove(EventMgr.OnChangeResolution, new Action(OnChangeResolution));
	}

	private void OnChangeResolution()
	{
		if (lastRsolutionWidth != Screen.width)
		{
			lastRsolutionWidth = Screen.width;
			if (!GameMgr.IsMobile_Static)
			{
				int num = Screen.height / (int)(focusOriginalSize * camMeterPerSize);
				int num2 = upRightDownLeftCamShowPeter * num;
				camCtrller_Up.cam.orthographicSize = (float)upRightDownLeftCamShowPeter / 2f;
				camCtrller_Down.cam.orthographicSize = (float)upRightDownLeftCamShowPeter / 2f;
				camCtrller_Up.cam.targetTexture = new RenderTexture(Screen.width, num2, 0);
				camCtrller_Right.cam.targetTexture = new RenderTexture(num2, Screen.height, 0);
				camCtrller_Down.cam.targetTexture = new RenderTexture(Screen.width, num2, 0);
				camCtrller_Left.cam.targetTexture = new RenderTexture(num2, Screen.height, 0);
				mr_Up.material.SetTexture("_MainTex", camCtrller_Up.cam.targetTexture);
				mr_Right.material.SetTexture("_MainTex", camCtrller_Right.cam.targetTexture);
				mr_Down.material.SetTexture("_MainTex", camCtrller_Down.cam.targetTexture);
				mr_Left.material.SetTexture("_MainTex", camCtrller_Left.cam.targetTexture);
				float x = (float)Screen.width / (float)num;
				mr_Up.transform.localScale = new Vector3(x, upRightDownLeftCamShowPeter, 1f);
				mr_Right.transform.localScale = new Vector3(upRightDownLeftCamShowPeter, focusOriginalSize * camMeterPerSize, 1f);
				mr_Down.transform.localScale = new Vector3(x, upRightDownLeftCamShowPeter, 1f);
				mr_Left.transform.localScale = new Vector3(upRightDownLeftCamShowPeter, focusOriginalSize * camMeterPerSize, 1f);
			}
		}
	}

	public void Initialize()
	{
		Inst = this;
		focusOriginalSize = cam_Main.orthographicSize;
		go_Chapter3CamRoot.SetActive(value: false);
		if (GameMgr.IsMobile_Static)
		{
			UnityEngine.Object.Destroy(tsf_CamUpRoot.gameObject);
			UnityEngine.Object.Destroy(tsf_CamRightRoot.gameObject);
			UnityEngine.Object.Destroy(tsf_CamDownRoot.gameObject);
			UnityEngine.Object.Destroy(tsf_CamLeftRoot.gameObject);
			UnityEngine.Object.Destroy(mr_Up);
			UnityEngine.Object.Destroy(mr_Right);
			UnityEngine.Object.Destroy(mr_Down);
			UnityEngine.Object.Destroy(mr_Left);
		}
		OnChangeResolution();
	}

	private void LateUpdate()
	{
		Shocking();
		Following();
		SpecificFocusing();
		Focussing();
		Chapter3CamRender();
	}

	private void Following()
	{
		if (followTargetT == null || LevelMgr.Inst == null)
		{
			return;
		}
		if (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme14_CustomRectangleEmptyScene || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			base.transform.position = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			return;
		}
		Vector3 zero = Vector3.zero;
		float num;
		if (GameMgr.IsMobile_Static)
		{
			num = ((PlayerMgr.Inst.PlayerCtrller.inputRightStick == Vector3.zero) ? PlayerMgr.Inst.PlayerCtrller.inputLeftStick.y : PlayerMgr.Inst.PlayerCtrller.inputRightStick.y);
		}
		else
		{
			float num2 = Input.mousePosition.y;
			if (GameMgr.IsSteamDeck_Static)
			{
				num2 = (float)Screen.height / 2f;
			}
			num = (num2 - (float)Screen.height / 2f) / ((float)Screen.height / 2f);
		}
		float num3 = Mathf.Pow(Mathf.Abs(num), 2f) * (GameMgr.IsMobile_Static ? mouseOffsetMobile : mouseOffset);
		if (!pauseMouseOffset)
		{
			if (num < 0f)
			{
				zero.y = 0f - num3;
			}
			else
			{
				zero.y = num3;
			}
			currentMouseOffset = Vector3.Lerp(currentMouseOffset, zero, mouseLerp * Time.deltaTime);
		}
		FinalCamClamp();
	}

	public void FinalCamClamp()
	{
		Vector3 position = followTargetT.position;
		if (PlayerMgr.Inst.inDashSpell && playerInRotateDash && LevelMgr.Inst.CurrentRoomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			position = dashCamFollowPoint;
		}
		float num;
		float num2;
		if (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme7_Chapter4_Store || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme8_Chapter4 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme9_Chapter4_2 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme12_Chapter5_2 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme15_Chapter5_Boss || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme21_Chapter5_Store || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme30_EndlessBattle)
		{
			num = LevelMgr.Inst.CurrentRoomCfg.theme8Width;
			num2 = LevelMgr.Inst.CurrentRoomCfg.theme8Height;
		}
		else if (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.theme26_Chapter1_Dave || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.theme27_Store_Dave || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.theme28_Chapter4Boss_Dave || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.theme29_Chapter5Boss_Dave)
		{
			num = LevelMgr.Inst.CurrentRoomCfg.themeEmptyWidth;
			num2 = LevelMgr.Inst.CurrentRoomCfg.themeEmptyHeight;
		}
		else
		{
			num = LevelMgr.Inst.CurrentRoomCfg.width;
			num2 = LevelMgr.Inst.CurrentRoomCfg.height;
		}
		Vector3 vector = new Vector3(FinalLimitX, FinalLimitYUp, FinalLimitYDown);
		if (GameMgr.IsMobile_Static && Inst.FocusCamSizeRatio != 1f)
		{
			vector = ClampDataCalculate();
		}
		if (num <= vector.x * 2f)
		{
			position.x = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x;
		}
		else
		{
			float min = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x - num / 2f + vector.x;
			float max = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x + num / 2f - vector.x;
			position.x = Mathf.Clamp(position.x, min, max);
		}
		if (num2 <= vector.z + vector.y + 0.01f)
		{
			position.y = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + (vector.z - vector.y) / 2f;
		}
		else
		{
			float num3 = LevelMgr.Inst.CurrentRoomCtrller.transform.position.y + (float)LevelMgr.Inst.CurrentRoomCfg.localMinY + vector.z;
			float num4 = LevelMgr.Inst.CurrentRoomCtrller.transform.position.y + (float)LevelMgr.Inst.CurrentRoomCfg.localMaxY - vector.y;
			if (num3 >= num4)
			{
				position.y = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + (vector.z - vector.y) / 2f;
			}
			else
			{
				position.y += currentMouseOffset.y;
				position.y = Mathf.Clamp(position.y, num3, num4);
			}
		}
		base.transform.position = Vector3.SmoothDamp(base.transform.position, position, ref dampSpeed, 1f / followLerp, float.PositiveInfinity, Time.unscaledDeltaTime);
	}

	private void Shocking()
	{
		if (!isShocking)
		{
			return;
		}
		shockTimer += Time.deltaTime;
		if (shockTimer >= currentShockParam.time)
		{
			shockTimer = 0f;
			isShocking = false;
			tsf_Shock.localPosition = Vector3.zero;
			return;
		}
		if (isShockOut)
		{
			tsf_Shock.localPosition += shockDir * currentShockParam.speed * Time.deltaTime;
			if (tsf_Shock.localPosition.sqrMagnitude > currentShockParam.radius * currentShockParam.radius)
			{
				isShockOut = false;
			}
			return;
		}
		float num = currentShockParam.speed * Time.deltaTime;
		if ((tsf_Shock.localPosition - Vector3.zero).sqrMagnitude <= num * num)
		{
			tsf_Shock.localPosition = Vector3.zero;
			isShockOut = true;
			shockDir = Tool2D.GetDir();
		}
		else
		{
			tsf_Shock.localPosition += -tsf_Shock.localPosition * num;
		}
	}

	private void Focussing()
	{
		if (isFocussing)
		{
			if (isFocussingTransform && focusTransform != null)
			{
				focusPoint = focusTransform.position;
				focusPoint = FocusTransformPositionClamp(focusPoint);
			}
			if (focusPoint != Vector3.zero)
			{
				tsf_Focus.position = Vector3.MoveTowards(tsf_Focus.position, focusPoint, focusMoveSpeed * Time.unscaledDeltaTime);
			}
			if (cam_Main.orthographicSize != focusTargetSize)
			{
				float target = focusTargetSize + DefaultSizeToCurrentPlatformSize(specificFocusSize);
				float orhographicSize = Mathf.MoveTowards(cam_Main.orthographicSize, target, focusSizeSpeed * Time.unscaledDeltaTime);
				SetOrhographicSize(orhographicSize);
			}
		}
		else
		{
			if (tsf_Focus.localPosition != Vector3.zero)
			{
				tsf_Focus.localPosition = Vector3.MoveTowards(tsf_Focus.localPosition, Vector3.zero, focusMoveSpeed * Time.unscaledDeltaTime);
			}
			float num = DefaultSizeToCurrentPlatformSize(FocusOriginalSize + specificFocusSize);
			if (!Mathf.Approximately(cam_Main.orthographicSize, num))
			{
				cam_Main.orthographicSize = Mathf.MoveTowards(cam_Main.orthographicSize, num, focusSizeSpeed * Time.unscaledDeltaTime);
			}
		}
	}

	private Vector3 FocusTransformPositionClamp(Vector3 targetPos)
	{
		Vector3 vector = ClampDataCalculate();
		float num;
		float num2;
		if (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme7_Chapter4_Store || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme8_Chapter4 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme9_Chapter4_2 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme12_Chapter5_2 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme15_Chapter5_Boss || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme21_Chapter5_Store || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme30_EndlessBattle)
		{
			num = LevelMgr.Inst.CurrentRoomCfg.theme8Width;
			num2 = LevelMgr.Inst.CurrentRoomCfg.theme8Height;
		}
		else if (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.theme26_Chapter1_Dave || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.theme27_Store_Dave || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.theme28_Chapter4Boss_Dave || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.theme29_Chapter5Boss_Dave)
		{
			num = LevelMgr.Inst.CurrentRoomCfg.themeEmptyWidth;
			num2 = LevelMgr.Inst.CurrentRoomCfg.themeEmptyHeight;
		}
		else
		{
			num = LevelMgr.Inst.CurrentRoomCfg.width;
			num2 = LevelMgr.Inst.CurrentRoomCfg.height;
		}
		float num3 = DefaultSizeToCurrentPlatformSize(FocusOriginalSize + specificFocusSize) - focusTargetSize;
		float num4 = (float)Screen.width / (float)Screen.height * num3;
		vector = new Vector3(vector.x - num4, vector.y - num3, vector.z - num3);
		if (num <= vector.x * 2f)
		{
			targetPos.x = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x;
		}
		else
		{
			float min = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x - num / 2f + vector.x;
			float max = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x + num / 2f - vector.x;
			targetPos.x = Mathf.Clamp(targetPos.x, min, max);
		}
		if (num2 <= vector.z + vector.y + 0.01f)
		{
			targetPos.y = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + (vector.z - vector.y) / 2f;
		}
		else
		{
			float num5 = LevelMgr.Inst.CurrentRoomCtrller.transform.position.y + (float)LevelMgr.Inst.CurrentRoomCfg.localMinY + vector.z;
			float num6 = LevelMgr.Inst.CurrentRoomCtrller.transform.position.y + (float)LevelMgr.Inst.CurrentRoomCfg.localMaxY - vector.y;
			if (num5 >= num6)
			{
				targetPos.y = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + (vector.z - vector.y) / 2f;
			}
			else
			{
				targetPos.y += currentMouseOffset.y;
				targetPos.y = Mathf.Clamp(targetPos.y, num5, num6);
			}
		}
		return targetPos;
	}

	private Vector3 ClampDataCalculate()
	{
		float num = MobileMgr.inst.screenRatio / 1.7777778f;
		float focusCamSizeRatio = FocusCamSizeRatio;
		return new Vector3(FinalLimitX * focusCamSizeRatio * num + (float)((GameMgr.IsMobile_Static && !Guide2Mgr.Inst) ? (-3) : 0), FinalLimitYUp * focusCamSizeRatio / num, FinalLimitYDown * focusCamSizeRatio / num);
	}

	private void Chapter3CamRender()
	{
	}

	private void SetOrhographicSize(float size)
	{
		cam_Main.orthographicSize = size;
	}

	private void SetOrhographicSizeConsiderPlatform(float size)
	{
		cam_Main.orthographicSize = DefaultSizeToCurrentPlatformSize(size);
	}

	public float DefaultSizeToCurrentPlatformSize(float size)
	{
		return size * (GameMgr.IsMobile_Static ? FocusCamSizeRatio : 1f);
	}

	public void ApplyFocusSize()
	{
		SetOrhographicSizeConsiderPlatform(FocusOriginalSize + specificFocusSize);
	}

	public void SetFollow(Transform followTargetT)
	{
		this.followTargetT = followTargetT;
	}

	public void CorrectCamera()
	{
		base.transform.position = followTargetT.position;
		float num = mouseLerp;
		float num2 = followLerp;
		mouseLerp = 10000f;
		followLerp = 10000f;
		currentMouseOffset = Vector3.zero;
		Following();
		mouseLerp = num;
		followLerp = num2;
		if (!(LevelMgr.Inst != null))
		{
			return;
		}
		if (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			go_Chapter3CamRoot.SetActive(value: true);
			if (!GameMgr.IsMobile_Static)
			{
				tsf_CamUpRoot.localPosition = new Vector3(0f, (float)(LevelMgr.Inst.CurrentRoomCfg.theme6Height + upRightDownLeftCamShowPeter) / 2f, 0f);
				tsf_CamLeftRoot.localPosition = new Vector3((float)(-(LevelMgr.Inst.CurrentRoomCfg.theme6Width + upRightDownLeftCamShowPeter)) / 2f, 0f, 0f);
				tsf_CamDownRoot.localPosition = new Vector3(0f, (float)(-(LevelMgr.Inst.CurrentRoomCfg.theme6Height + upRightDownLeftCamShowPeter)) / 2f, 0f);
				tsf_CamRightRoot.localPosition = new Vector3((float)(LevelMgr.Inst.CurrentRoomCfg.theme6Width + upRightDownLeftCamShowPeter) / 2f, 0f, 0f);
				mr_Up.transform.localPosition = new Vector3(0f, (float)(-LevelMgr.Inst.CurrentRoomCfg.theme6Height + upRightDownLeftCamShowPeter) / 2f, 0.9f);
				mr_Right.transform.localPosition = new Vector3((float)(-LevelMgr.Inst.CurrentRoomCfg.theme6Width + upRightDownLeftCamShowPeter) / 2f, 0f, 0.9f);
				mr_Down.transform.localPosition = new Vector3(0f, (float)(LevelMgr.Inst.CurrentRoomCfg.theme6Height - upRightDownLeftCamShowPeter) / 2f, 0.9f);
				mr_Left.transform.localPosition = new Vector3((float)(LevelMgr.Inst.CurrentRoomCfg.theme6Width - upRightDownLeftCamShowPeter) / 2f, 0f, 0.9f);
			}
		}
		else
		{
			go_Chapter3CamRoot.SetActive(value: false);
		}
	}

	public void MouseOffsetPause()
	{
		pauseMouseOffset = true;
	}

	public void MouseOffsetContinue()
	{
		pauseMouseOffset = false;
	}

	public void SetShock(float shockRadius, float shockSpeed, float shockTime)
	{
		SetShock(new ShockParam(shockRadius, shockSpeed, shockTime));
	}

	public void SetShock(ShockParam shockParam, Vector3 shockDirection = default(Vector3))
	{
		if (DataMgr.settingData.screenShockRatio == 0f)
		{
			return;
		}
		shockParam.radius *= DataMgr.settingData.screenShockRatio;
		shockParam.speed *= DataMgr.settingData.screenShockRatio;
		if (isShocking)
		{
			if (currentShockParam == shockParam)
			{
				currentShockParam.time = shockParam.time;
				return;
			}
			if (currentShockParam.time - shockTimer > shockParam.time)
			{
				return;
			}
		}
		currentShockParam = shockParam;
		isShocking = true;
		isShockOut = true;
		shockTimer = 0f;
		if (shockDirection != default(Vector3))
		{
			shockDir = shockDirection.normalized;
		}
		else
		{
			shockDir = Tool2D.GetDir();
		}
	}

	public void FocusOn(float focusSize, float focusTime)
	{
		FocusOn(focusSize, focusTime, Vector3.zero);
	}

	public void FocusOn(float focusSize, float focusTime, Vector3 point)
	{
		focusTime /= (GameMgr.IsMobile_Static ? FocusCamSizeRatio : 1f);
		isFocussing = true;
		focusPoint = point;
		focusTargetSize = DefaultSizeToCurrentPlatformSize(focusSize);
		if (focusTime <= 0f)
		{
			SetOrhographicSize(focusTargetSize);
			if (point == Vector3.zero)
			{
				tsf_Focus.localPosition = Vector3.zero;
			}
			else
			{
				tsf_Focus.position = point;
			}
		}
		else
		{
			focusSizeSpeed = Mathf.Abs(cam_Main.orthographicSize - focusTargetSize) / focusTime;
			if (point != Vector3.zero)
			{
				focusMoveSpeed = (point - tsf_Focus.position).magnitude / focusTime;
			}
		}
	}

	public void FocusOnTransform(float focusSize, float focusTime, Transform _t)
	{
		focusTransform = _t;
		focusTime /= (GameMgr.IsMobile_Static ? FocusCamSizeRatio : 1f);
		isFocussing = true;
		isFocussingTransform = true;
		focusPoint = _t.position;
		focusTargetSize = DefaultSizeToCurrentPlatformSize(focusSize);
		if (focusTime <= 0f)
		{
			SetOrhographicSize(focusTargetSize);
			if (!_t)
			{
				tsf_Focus.localPosition = Vector3.zero;
			}
			else
			{
				tsf_Focus.position = _t.position;
			}
			return;
		}
		focusSizeSpeed = Mathf.Abs(cam_Main.orthographicSize - focusTargetSize) / focusTime;
		if ((bool)_t)
		{
			Vector3 vector = FocusTransformPositionClamp(focusTransform.position);
			focusMoveSpeed = (tsf_Focus.position - vector).magnitude / focusTime;
		}
	}

	public void FocusRecover(float recoverTime)
	{
		isFocussing = false;
		isFocussingTransform = false;
		finialClampAfterFocus = false;
		float num = DefaultSizeToCurrentPlatformSize(FocusOriginalSize);
		if (recoverTime <= 0f)
		{
			tsf_Focus.localPosition = Vector3.zero;
			SetOrhographicSize(num);
		}
		else
		{
			focusSizeSpeed = Mathf.Abs(cam_Main.orthographicSize - num) / recoverTime;
			focusMoveSpeed = tsf_Focus.localPosition.magnitude / recoverTime;
			SetFollow(PlayerMgr.Inst.PlayerT);
		}
		ClearExtraCameraFocusRequirement();
		ClearSpecificConstraint();
	}

	public void AddNewCameraFocusRequirement(CameraFocusSizeData data)
	{
		ExtraFocusSizeRequirementList.Add(data);
	}

	public void ClearExtraCameraFocusRequirement()
	{
		ExtraFocusSizeRequirementList.Clear();
		specificFocusSizeLastFrame = 0f;
		specificFocusSize = 0f;
	}

	public void SetSpecificFocusSize(float distance, float sizeDistanceRatio = 1f, float limit = float.PositiveInfinity)
	{
		specificFocusTargetSize = Mathf.Min(limit, Mathf.Max(0f, distance - FocusOriginalSize) * sizeDistanceRatio);
	}

	public void SetSpecificFocusSize(float size)
	{
		specificFocusTargetSize = size;
	}

	public float GetSpecificFocusSize(Vector3 distance, float sizeDistanceRatio = 1f, float limit = float.PositiveInfinity)
	{
		distance.x = Mathf.Abs(distance.x);
		distance.y = Mathf.Abs(distance.y);
		Vector3 vector = distance - new Vector3(FocusOriginalSize / 9f * 16f, FocusOriginalSize, 0f);
		vector.x = Mathf.Max(vector.x, 0f);
		vector.y = Mathf.Max(vector.y, 0f);
		return Mathf.Min(limit, Mathf.Max(0f, vector.magnitude) * sizeDistanceRatio);
	}

	public void SetSpecificConstraint(float size)
	{
		specificBorderConstraintTargetValue = size;
	}

	public void ClearSpecificConstraint()
	{
		specificBorderConstraintTargetValue = 0f;
	}

	public void SpecificFocusing()
	{
		float num = 0f;
		float a = lastFrameFocusProgressTime;
		int num2 = 0;
		int num3 = 1;
		for (int num4 = ExtraFocusSizeRequirementList.Count; num4 > 0; num4--)
		{
			CameraFocusSizeData cameraFocusSizeData = ExtraFocusSizeRequirementList[num4 - 1];
			if ((cameraFocusSizeData.hasDuration && cameraFocusSizeData.buffDuration > 0f) || !cameraFocusSizeData.hasDuration)
			{
				if (cameraFocusSizeData.buffLevel > num2)
				{
					num = cameraFocusSizeData.extraFocusSize;
					a = cameraFocusSizeData.focusSizeSmoothDuration;
					num2 = cameraFocusSizeData.buffLevel;
					num3 = 1;
				}
				else if (cameraFocusSizeData.buffLevel == num2)
				{
					num += cameraFocusSizeData.extraFocusSize;
					a = Mathf.Min(a, cameraFocusSizeData.focusSizeSmoothDuration);
					num3++;
				}
			}
			else
			{
				if (cameraFocusSizeData.buffLevel >= num2)
				{
					a = cameraFocusSizeData.OnExitFocusSizeSmoothChangeDuratio;
				}
				ExtraFocusSizeRequirementList.Remove(cameraFocusSizeData);
			}
			cameraFocusSizeData.buffDuration -= Time.deltaTime * Time.timeScale;
		}
		specificFocusTargetSize = num;
		specificFocuSizeSmoothTime = a;
		lastFrameFocusProgressTime = a;
		specificFocusSizeLastFrame = specificFocusSize;
		specificFocusSize = Mathf.SmoothDamp(specificFocusSize, specificFocusTargetSize, ref specificFocusMoveSpeed, specificFocuSizeSmoothTime);
		specificBorderConstraint = Mathf.SmoothDamp(specificBorderConstraint, specificBorderConstraintTargetValue, ref specificBorderConstraintChangeSpeed, specificFocuSizeSmoothTime);
	}
}
