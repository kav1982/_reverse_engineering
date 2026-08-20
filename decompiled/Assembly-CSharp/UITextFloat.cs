using UnityEngine;
using UnityEngine.UI;

public class UITextFloat : MonoBehaviour
{
	private enum UIState
	{
		ToLargen,
		ToNormal,
		Idle
	}

	public Text text;

	public Image image;

	public Sprite sprite_Critical;

	public Sprite sprite_Poison;

	public Sprite sprite_PoisonH;

	public Sprite sprite_Crystal;

	public Sprite sprite_Coin;

	public Sprite sprite_Shield;

	public Sprite sprite_Umbrella;

	public Sprite sprite_TempShield;

	public Sprite sprite_Key;

	public Sprite sprite_Burn;

	public Sprite sprite_DropMP;

	public int defualFontSize;

	[Header("Color")]
	public Color color_Damage;

	public Color color_Critical;

	public Color color_Posion;

	public Color color_Burn;

	public Color color_Normal;

	public Color color_GetKey;

	public Color color_GetCoin;

	public Color color_Crystal;

	public Color color_AncientBlood;

	public Color color_ChaosCore;

	public Color color_Gear;

	public Color color_GetShield;

	public Color color_GetTempShield;

	public Color color_Recovery;

	public Color color_PlayerTakeDamageInjured;

	public Color color_PlayerLostShield;

	public Color color_PlayerLostTempShield;

	public Color color_PlayerLostUmbralle;

	public Color color_DropMP;

	public Color color_DropCoin;

	public Color color_DropKey;

	[Header("Motion")]
	public RectTransform rtsf_Motion;

	public VariableFloat xSpeed;

	public VariableFloat ySpeed;

	public float startScale;

	public float largenScale;

	public float largenSpeed;

	public float normalScaleSpeed;

	public float smallTime;

	public float smallSpeed;

	public float mobileScaleMultiply = 2f;

	private UIState state;

	private Vector3 worldPoint;

	private float smallTimer;

	private float multiplier
	{
		get
		{
			if (!GameMgr.IsMobile_Static)
			{
				return 1f;
			}
			return mobileScaleMultiply;
		}
	}

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			text.fontStyle = FontStyle.Normal;
		}
		else
		{
			text.fontStyle = FontStyle.Bold;
		}
	}

	private void Update()
	{
		if (xSpeed.result != 0f)
		{
			float x = rtsf_Motion.anchoredPosition.x + xSpeed.result * Time.unscaledDeltaTime;
			rtsf_Motion.anchoredPosition = new Vector2(x, rtsf_Motion.anchoredPosition.y);
		}
		if (ySpeed.result != 0f)
		{
			float y = rtsf_Motion.anchoredPosition.y + ySpeed.result * Time.unscaledDeltaTime;
			rtsf_Motion.anchoredPosition = new Vector2(rtsf_Motion.anchoredPosition.x, y);
		}
		base.transform.localPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(worldPoint);
		switch (state)
		{
		case UIState.ToLargen:
		{
			float num2 = Mathf.MoveTowards(rtsf_Motion.localScale.x, largenScale * multiplier, largenSpeed * multiplier * Time.unscaledDeltaTime);
			rtsf_Motion.localScale = Vector3.one * num2;
			if (num2 == largenScale * (GameMgr.IsMobile_Static ? mobileScaleMultiply : 1f))
			{
				state = UIState.ToNormal;
			}
			break;
		}
		case UIState.ToNormal:
		{
			float num3 = Mathf.MoveTowards(rtsf_Motion.localScale.x, 1f * multiplier, normalScaleSpeed * multiplier * Time.unscaledDeltaTime);
			rtsf_Motion.localScale = Vector3.one * num3;
			if (num3 == 1f * multiplier)
			{
				state = UIState.Idle;
			}
			break;
		}
		case UIState.Idle:
			smallTimer += Time.unscaledDeltaTime;
			if (smallTimer >= smallTime)
			{
				float num = Mathf.MoveTowards(rtsf_Motion.localScale.x, 0f, smallSpeed * Time.unscaledDeltaTime);
				rtsf_Motion.localScale = Vector3.one * num;
				if (num == 1f)
				{
					state = UIState.Idle;
				}
				if (num == 0f)
				{
					base.gameObject.SetActive(value: false);
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void Initialize(string str, UITextFloatType type, Vector3 worldPoint)
	{
		this.worldPoint = worldPoint;
		base.transform.localPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(worldPoint);
		text.text = str;
		text.rectTransform.sizeDelta = new Vector2(text.preferredWidth, text.rectTransform.sizeDelta.y);
		state = UIState.ToLargen;
		rtsf_Motion.localPosition = Vector3.zero;
		rtsf_Motion.localScale = Vector3.one * multiplier;
		smallTimer = 0f;
		switch (type)
		{
		case UITextFloatType.PlayerTakeDamage:
			text.color = color_PlayerTakeDamageInjured;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.result = (0f - (ySpeed.value1 + ySpeed.value2)) / 2f;
			image.gameObject.SetActive(value: false);
			break;
		case UITextFloatType.PlayerLostShield:
			text.color = color_PlayerLostShield;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.result = (0f - (ySpeed.value1 + ySpeed.value2)) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_Shield;
			break;
		case UITextFloatType.PlayerLostTempShield:
			text.color = color_PlayerLostTempShield;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.result = (0f - (ySpeed.value1 + ySpeed.value2)) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_TempShield;
			break;
		case UITextFloatType.PlayerLostUmbrella:
			text.color = color_PlayerLostUmbralle;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.result = (0f - (ySpeed.value1 + ySpeed.value2)) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_Umbrella;
			break;
		case UITextFloatType.DropMP:
			text.color = color_DropMP;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.RandomResult();
			ySpeed.result = (0f - (ySpeed.value1 + ySpeed.value2)) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_DropMP;
			break;
		case UITextFloatType.Normal:
			text.color = color_Normal;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: false);
			break;
		case UITextFloatType.Damage:
			text.color = color_Damage;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.result = ySpeed.RandomResult();
			image.gameObject.SetActive(value: false);
			break;
		case UITextFloatType.Critical:
			text.color = color_Critical;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.RandomResult();
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_Critical;
			break;
		case UITextFloatType.Poison:
			text.color = color_Posion;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.RandomResult();
			image.gameObject.SetActive(value: true);
			if (GameMgr.IsHarmony_Static)
			{
				image.sprite = sprite_PoisonH;
			}
			else
			{
				image.sprite = sprite_Poison;
			}
			break;
		case UITextFloatType.Burn:
			text.color = color_Burn;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.RandomResult();
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_Burn;
			break;
		case UITextFloatType.GetCoin:
			text.color = color_GetCoin;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_Coin;
			break;
		case UITextFloatType.GetCrystal:
			text.color = color_Crystal;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_Crystal;
			break;
		case UITextFloatType.GetAnchientBlood:
			text.color = color_AncientBlood;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: false);
			break;
		case UITextFloatType.GetChaosCore:
			text.color = color_ChaosCore;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: false);
			break;
		case UITextFloatType.GetGear:
			text.color = color_Gear;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: false);
			break;
		case UITextFloatType.GetShield:
			text.color = color_GetShield;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_Shield;
			break;
		case UITextFloatType.GetTempShield:
			text.color = color_GetTempShield;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_TempShield;
			break;
		case UITextFloatType.GetKey:
			text.color = color_GetKey;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_Key;
			break;
		case UITextFloatType.Recover:
			text.color = color_Recovery;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: false);
			break;
		case UITextFloatType.DropCoin:
			text.color = color_DropCoin;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.result = (0f - (ySpeed.value1 + ySpeed.value2)) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_Coin;
			break;
		case UITextFloatType.DropKey:
			text.color = color_DropKey;
			text.fontSize = defualFontSize;
			xSpeed.RandomResult();
			ySpeed.result = (0f - (ySpeed.value1 + ySpeed.value2)) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_Key;
			break;
		case UITextFloatType.RecoverMP:
			text.color = color_DropMP;
			text.fontSize = defualFontSize;
			xSpeed.result = 0f;
			ySpeed.result = (ySpeed.value1 + ySpeed.value2) / 2f;
			image.gameObject.SetActive(value: true);
			image.sprite = sprite_DropMP;
			break;
		default:
			Debug.LogError("应该走TextFloatVFX");
			break;
		}
	}
}
