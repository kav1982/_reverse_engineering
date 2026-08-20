using UnityEngine;
using UnityEngine.UI;

public class UIPlayerEndlessPick : MonoBehaviour
{
	public Transform tsf_Chip;

	public Transform tsf_Coin;

	public Image image_Chip;

	public Image image_Coin;

	public Text text_Chip;

	public Text text_Coin;

	private float chipValue;

	private float coinValue;

	private float chipTimer;

	private float coinTimer;

	public AnimationCurve valueChangeCurve;

	public float valueChangeDuration;

	public AnimationCurve valueFadeScaleCurve;

	public AnimationCurve valueFadeCurve;

	public float valueFadeDuration;

	public static UIPlayerEndlessPick Inst;

	public void OnChipChange(int value)
	{
		if (value > 0)
		{
			chipValue += value;
			text_Chip.text = "+" + chipValue;
			chipTimer = 0f;
			SetChipColor(1f);
		}
	}

	public void OnCoinChange(int value)
	{
		if (value > 0)
		{
			coinValue += value;
			text_Coin.text = "+" + coinValue;
			coinTimer = 0f;
			SetCoinColor(1f);
		}
	}

	private void Start()
	{
		chipTimer = 999f;
		coinTimer = 999f;
		Inst = this;
		SetChipColor(0f);
		SetCoinColor(0f);
	}

	private void SetChipColor(float alpha)
	{
		image_Chip.color = new Color(1f, 1f, 1f, alpha);
		Color color = text_Chip.color;
		color.a = alpha;
		text_Chip.color = color;
	}

	private void SetCoinColor(float alpha)
	{
		image_Coin.color = new Color(1f, 1f, 1f, alpha);
		Color color = text_Coin.color;
		color.a = alpha;
		text_Coin.color = color;
	}

	private void Update()
	{
		chipTimer += Time.deltaTime;
		if (chipTimer <= valueChangeDuration)
		{
			tsf_Chip.localScale = Vector3.one * valueChangeCurve.Evaluate(chipTimer / valueChangeDuration);
		}
		else if (valueChangeDuration < chipTimer && chipTimer < valueChangeDuration + valueFadeDuration)
		{
			float num = chipTimer - valueChangeDuration;
			tsf_Chip.localScale = Vector3.one * valueFadeScaleCurve.Evaluate(num / valueFadeDuration);
			float chipColor = valueFadeCurve.Evaluate(num / valueFadeDuration);
			SetChipColor(chipColor);
		}
		else
		{
			chipValue = 0f;
		}
		coinTimer += Time.deltaTime;
		if (coinTimer <= valueChangeDuration)
		{
			tsf_Coin.localScale = Vector3.one * valueChangeCurve.Evaluate(coinTimer / valueChangeDuration);
		}
		else if (valueChangeDuration < coinTimer && coinTimer < valueChangeDuration + valueFadeDuration)
		{
			float num2 = coinTimer - valueChangeDuration;
			tsf_Coin.localScale = Vector3.one * valueFadeScaleCurve.Evaluate(num2 / valueFadeDuration);
			float coinColor = valueFadeCurve.Evaluate(num2 / valueFadeDuration);
			SetCoinColor(coinColor);
		}
		else
		{
			coinValue = 0f;
		}
	}
}
