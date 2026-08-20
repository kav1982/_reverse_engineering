using UnityEngine;
using UnityEngine.UI;

public class UIRerollRelic_Relic : MonoBehaviour
{
	private enum UIState
	{
		Idle,
		Move,
		Fly
	}

	public RectTransform rtsf_Self;

	public CanvasGroup cg;

	public Image image_Icon;

	public Text text_Count;

	public Animator anima;

	public float moveLerp;

	public float lerpThreshold;

	public float flyLerpSpeed;

	public float flyHight;

	public int totalShowSpace;

	public int totalHideSpace;

	public float space;

	private UIState state;

	private UIRerollRelic uiRerollRelic;

	public RelicConfig RelicCfg;

	private int index;

	private UIRerollRelic_Relic flyToURR;

	private Vector2 flyOriginalPoint;

	private float flyLerpTimer;

	private void Update()
	{
		switch (state)
		{
		case UIState.Move:
		{
			Vector2 vector = new Vector2(space, 0f) * (index - uiRerollRelic.SelectedIndex);
			rtsf_Self.anchoredPosition = Vector2.Lerp(rtsf_Self.anchoredPosition, vector, moveLerp * Time.unscaledDeltaTime);
			if (Vector2.SqrMagnitude(rtsf_Self.anchoredPosition - vector) < lerpThreshold * lerpThreshold)
			{
				state = UIState.Idle;
			}
			UpdateAlpha();
			break;
		}
		case UIState.Fly:
		{
			flyLerpTimer += flyLerpSpeed * Time.unscaledDeltaTime;
			Vector3 v = (flyOriginalPoint + flyToURR.rtsf_Self.anchoredPosition) / 2f + new Vector2(0f, flyHight);
			rtsf_Self.anchoredPosition = GeneralTool.QuadraticBezierCurve(flyOriginalPoint, v, flyToURR.rtsf_Self.anchoredPosition, flyLerpTimer);
			if (flyLerpTimer >= 1f)
			{
				flyToURR.RelicCfg.level++;
				flyToURR.RelicCfg.CalculateAbility();
				flyToURR.UpdateInfo();
				Object.Destroy(base.gameObject);
			}
			UpdateAlpha();
			break;
		}
		default:
			Debug.LogError(state);
			break;
		case UIState.Idle:
			break;
		}
	}

	private void UpdateAlpha()
	{
		float num = Vector2.Distance(rtsf_Self.anchoredPosition, Vector2.zero) / space;
		float num2 = 0f;
		num2 = ((num <= (float)totalShowSpace) ? 1f : ((!(num >= (float)totalHideSpace)) ? Mathf.Lerp(1f, 0f, (num - (float)totalShowSpace) / (float)(totalHideSpace - totalShowSpace)) : 0f));
		cg.alpha = num2;
	}

	public void Initialize(UIRerollRelic uiRerollRelic, int index, RelicConfig relicCfg)
	{
		this.uiRerollRelic = uiRerollRelic;
		this.index = index;
		RelicCfg = relicCfg;
		UpdateInfo();
		UpdateAlpha();
		SetMove(index);
	}

	public void UpdateInfo()
	{
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(RelicCfg.GetIconPath());
		if (RelicCfg.level == 1)
		{
			text_Count.gameObject.SetActive(value: false);
			return;
		}
		text_Count.gameObject.SetActive(value: true);
		text_Count.text = RelicCfg.level.ToString();
	}

	public void SetMove(int index)
	{
		this.index = index;
		state = UIState.Move;
	}

	public void ChangeConfig(RelicConfig relicCfg)
	{
		RelicCfg = relicCfg;
		RelicCfg.CalculateAbility();
		UpdateInfo();
		anima.SetTrigger("Action");
	}

	public void Fly(UIRerollRelic_Relic flyToURR)
	{
		flyOriginalPoint = rtsf_Self.anchoredPosition;
		this.flyToURR = flyToURR;
		state = UIState.Fly;
	}
}
