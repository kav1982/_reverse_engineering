using UnityEngine;
using UnityEngine.UI;

public class UIReroll_Spell : MonoBehaviour
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

	public GameObject go_Star1;

	public GameObject go_Star2;

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

	private int index;

	private int selectedIndexOfMove;

	private UIReroll_Spell flyTo;

	private Vector2 flyOriginalPoint;

	private float flyLerpTimer;

	public bool flyOutlookOnly;

	public int SpellID { get; set; }

	public int Count { get; private set; }

	private void Update()
	{
		switch (state)
		{
		case UIState.Move:
		{
			Vector2 vector = new Vector2(space, 0f) * (index - selectedIndexOfMove);
			rtsf_Self.anchoredPosition = Vector2.Lerp(rtsf_Self.anchoredPosition, vector, moveLerp * Time.deltaTime);
			if (Vector2.SqrMagnitude(rtsf_Self.anchoredPosition - vector) < lerpThreshold * lerpThreshold)
			{
				state = UIState.Idle;
			}
			UpdateAlpha();
			break;
		}
		case UIState.Fly:
		{
			flyLerpTimer += flyLerpSpeed * Time.deltaTime;
			Vector3 v = (flyOriginalPoint + flyTo.rtsf_Self.anchoredPosition) / 2f + new Vector2(0f, flyHight);
			rtsf_Self.anchoredPosition = GeneralTool.QuadraticBezierCurve(flyOriginalPoint, v, flyTo.rtsf_Self.anchoredPosition, flyLerpTimer);
			if (flyLerpTimer >= 1f)
			{
				if (!flyOutlookOnly)
				{
					flyTo.ChangeCount(flyTo.Count + Count);
				}
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

	private void UpdateInfo()
	{
		SpellConfig spellConfig = SpellConfig.dic[SpellID];
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(spellConfig.GetIconPath());
		if (spellConfig.level > 1)
		{
			go_Star1.SetActive(value: true);
		}
		else
		{
			go_Star1.SetActive(value: false);
		}
		if (spellConfig.level > 2)
		{
			go_Star2.SetActive(value: true);
		}
		else
		{
			go_Star2.SetActive(value: false);
		}
		if (Count == 1)
		{
			text_Count.gameObject.SetActive(value: false);
			return;
		}
		text_Count.gameObject.SetActive(value: true);
		text_Count.text = "×" + Count;
	}

	private void UpdateAlpha()
	{
		float num = Vector2.Distance(rtsf_Self.anchoredPosition, Vector2.zero) / space;
		float num2 = 0f;
		num2 = ((num <= (float)totalShowSpace) ? 1f : ((!(num >= (float)totalHideSpace)) ? Mathf.Lerp(1f, 0f, (num - (float)totalShowSpace) / (float)(totalHideSpace - totalShowSpace)) : 0f));
		cg.alpha = num2;
	}

	public void Initialize(int index, int id, int count)
	{
		this.index = index;
		SpellID = id;
		Count = count;
		UpdateInfo();
		UpdateAlpha();
		SetMove(0);
	}

	public void SetMove(int selectedIndexOfMove)
	{
		state = UIState.Move;
		this.selectedIndexOfMove = selectedIndexOfMove;
	}

	public void ChangeID(int newID)
	{
		SpellID = newID;
		UpdateInfo();
		anima.SetTrigger("Action");
	}

	public void ChangeCount(int newCount)
	{
		Count = newCount;
		UpdateInfo();
		anima.SetTrigger("Action");
		if (GameUISingletonMono<UICompound>.StaticIsOpen && GameUISingletonMono<UICompound>.Inst.SelectedUIRS == this)
		{
			GameUISingletonMono<UICompound>.Inst.UpdateInfo();
		}
	}

	public void ChangeIndex(int newIndex, int SelectedSpellIndex)
	{
		index = newIndex;
		SetMove(SelectedSpellIndex);
	}

	public void Fly(UIReroll_Spell flyTo)
	{
		flyOriginalPoint = rtsf_Self.anchoredPosition;
		this.flyTo = flyTo;
		state = UIState.Fly;
	}
}
