using UnityEngine;

public class FogController : MonoBehaviour
{
	private enum FogState
	{
		Idle,
		Show,
		Hide
	}

	public SpriteRenderer sr_Fog;

	public float fogExpansion;

	public float alphaDisappearSpeed;

	[Header("Mask")]
	public Transform tsf_FogMask;

	public float maskMaxScaleRatioOfLevelSize;

	public float maskScaleSpeed;

	private FogState state;

	private RoomController roomCtrller;

	private float currentAlpha;

	private float MaxMaskScale;

	private void Update()
	{
		switch (state)
		{
		case FogState.Show:
			currentAlpha = Mathf.MoveTowards(currentAlpha, 1f, alphaDisappearSpeed * Time.deltaTime);
			sr_Fog.color = new Color(1f, 1f, 1f, currentAlpha);
			if (currentAlpha == 1f)
			{
				state = FogState.Idle;
			}
			break;
		case FogState.Hide:
		{
			float num = Mathf.MoveTowards(tsf_FogMask.localScale.x, MaxMaskScale, maskScaleSpeed * Time.deltaTime);
			if (num >= MaxMaskScale)
			{
				num = MaxMaskScale;
				state = FogState.Idle;
			}
			tsf_FogMask.localScale = Vector3.one * num;
			break;
		}
		default:
			Debug.LogError(state);
			break;
		case FogState.Idle:
			break;
		}
	}

	public void Initialize(RoomController roomCtrller)
	{
		this.roomCtrller = roomCtrller;
		tsf_FogMask.position = Tool2D.IgnoreZPoint(roomCtrller.CenterPoint, -49.9f);
		sr_Fog.transform.position = Tool2D.IgnoreZPoint(roomCtrller.CenterPoint, -50f);
		if (GuideMgr.Inst != null)
		{
			sr_Fog.size = new Vector2(40f, 40f);
		}
		else
		{
			sr_Fog.size = new Vector2(60f, 60f);
		}
		if (roomCtrller.RoomScale.x > roomCtrller.RoomScale.y)
		{
			MaxMaskScale = roomCtrller.RoomScale.x * maskMaxScaleRatioOfLevelSize;
		}
		else
		{
			MaxMaskScale = roomCtrller.RoomScale.y * maskMaxScaleRatioOfLevelSize;
		}
		ShowDirect();
	}

	public void ShowDirect()
	{
		state = FogState.Idle;
		sr_Fog.maskInteraction = SpriteMaskInteraction.None;
		sr_Fog.color = Color.white;
		tsf_FogMask.gameObject.SetActive(value: false);
	}

	public void Show()
	{
		state = FogState.Show;
		sr_Fog.maskInteraction = SpriteMaskInteraction.None;
		sr_Fog.color = new Color(1f, 1f, 1f, 0f);
		tsf_FogMask.gameObject.SetActive(value: false);
		currentAlpha = 0f;
	}

	public void Hide(FourDir fogDir)
	{
		state = FogState.Hide;
		sr_Fog.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		sr_Fog.color = Color.white;
		tsf_FogMask.gameObject.SetActive(value: true);
		tsf_FogMask.position = Tool2D.IgnoreZPoint(roomCtrller.GetAccessCenterPoint(fogDir), -49.9f);
		tsf_FogMask.localScale = Vector3.one * 0.01f;
	}

	public void Hide(Vector3 point)
	{
		state = FogState.Hide;
		sr_Fog.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		sr_Fog.color = Color.white;
		tsf_FogMask.gameObject.SetActive(value: true);
		tsf_FogMask.position = Tool2D.IgnoreZPoint(point, -49.9f);
		tsf_FogMask.localScale = Vector3.one * 0.01f;
	}

	public void HideDirect()
	{
		state = FogState.Idle;
		sr_Fog.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		sr_Fog.color = Color.white;
		tsf_FogMask.gameObject.SetActive(value: true);
		tsf_FogMask.position = Tool2D.IgnoreZPoint(roomCtrller.CenterPoint, -49.9f);
		tsf_FogMask.localScale = Vector3.one * MaxMaskScale;
	}
}
