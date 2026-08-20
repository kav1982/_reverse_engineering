using System;
using UnityEngine;
using UnityEngine.UI;

public class RollRewardFly : MonoBehaviour
{
	public enum DropType
	{
		Spell,
		Relic,
		Potion,
		Curse,
		Chest,
		Coin,
		Dimond,
		Wand
	}

	private bool _isUI;

	private bool _dropOnEnd;

	public GameObject go_Image;

	[Header("Image")]
	public Transform tsf_Self;

	public RectTransform rtsf_Self;

	[Header("Sprite")]
	public SpriteRenderer spriteRenderer;

	public Image image;

	[Space(30f)]
	public ParticleSystem ps_Finish;

	public float ps_FinishColorAlpha;

	public ParticleSystem ps_FinishGlow;

	public float ps_FinishGlowColorAlpha;

	public ParticleSystem ps_Paush;

	public GameObject go_FlyFinish;

	public Vector3 middlePointOffset;

	private SpecialObj217.rewardType rewardtype;

	public float lerpSPeed;

	public float waitDestroyTime;

	public GameObject efSpellEpic;

	public GameObject efSpellRare;

	public GameObject efSpellCommon;

	public GameObject efRelicSpecial;

	public GameObject efRelicRare;

	public GameObject efRelicCommon;

	public Color colorCurse;

	public Color colorDefault;

	public Color colorSpellCommon;

	public Color colorSpellRare;

	public Color colorSpellEpic;

	public Color colorSpellSpecial;

	public Color colorRelicCommon;

	public Color colorRelicRare;

	public Color colorRelicEpic;

	public Color colorRelicSpecial;

	private int id;

	private Vector3 originalPoint;

	private Vector3 moveToPointWorldSpace;

	private Vector3 moveToPointAppearance;

	private Vector3 middlePoint;

	private float currentLerp;

	private bool flyFinish;

	private float waitDestroyTimer;

	public Vector2 StarPositionOffset;

	public GameObject star1;

	public GameObject star2;

	private Action dropAction;

	private Transform transformFollow;

	public bool useParticleColor;

	private RoomController overrideRoomController;

	public RoomController dropRoomController
	{
		get
		{
			if (!(overrideRoomController != null))
			{
				return LevelMgr.Inst.CurrentRoomCtrller;
			}
			return overrideRoomController;
		}
	}

	private void Update()
	{
		if (flyFinish)
		{
			waitDestroyTimer += Time.unscaledDeltaTime;
			if (waitDestroyTimer >= waitDestroyTime)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			return;
		}
		currentLerp += lerpSPeed * Time.unscaledDeltaTime;
		if (transformFollow != null)
		{
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, transformFollow.position);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint, null, out var localPoint);
			rtsf_Self.anchoredPosition = GeneralTool.QuadraticBezierCurve(originalPoint, middlePoint, new Vector3(localPoint.x, localPoint.y, 0f) + moveToPointAppearance, currentLerp);
		}
		else if (_isUI)
		{
			rtsf_Self.anchoredPosition = GeneralTool.QuadraticBezierCurve(originalPoint, middlePoint, moveToPointAppearance, currentLerp);
		}
		else
		{
			tsf_Self.transform.position = GeneralTool.QuadraticBezierCurve(originalPoint, middlePoint, moveToPointAppearance, currentLerp);
		}
		if (!(currentLerp >= 1f))
		{
			return;
		}
		if (transformFollow != null)
		{
			Vector2 screenPoint2 = RectTransformUtility.WorldToScreenPoint(null, transformFollow.position);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.canvas_10.GetComponent<RectTransform>(), screenPoint2, null, out var localPoint2);
			rtsf_Self.anchoredPosition = new Vector3(localPoint2.x, localPoint2.y, 0f) + moveToPointAppearance;
		}
		else if (_isUI)
		{
			rtsf_Self.anchoredPosition = moveToPointAppearance;
		}
		else
		{
			tsf_Self.transform.position = moveToPointAppearance;
		}
		flyFinish = true;
		go_FlyFinish.SetActive(value: true);
		go_Image.gameObject.SetActive(value: false);
		ps_Paush.Stop();
		efSpellCommon.SetActive(value: false);
		efSpellRare.SetActive(value: false);
		efRelicCommon.SetActive(value: false);
		efRelicRare.SetActive(value: false);
		efSpellEpic.SetActive(value: false);
		efRelicSpecial.SetActive(value: false);
		SEMgr.Inst.uiSpellFlyEnd.PlaySE();
		if (_dropOnEnd)
		{
			ItemType type = ItemType.Spell;
			switch (rewardtype)
			{
			case SpecialObj217.rewardType.SpellCommonlv1:
			case SpecialObj217.rewardType.SpellCommonlv2:
			case SpecialObj217.rewardType.SpellCommonlv3:
			case SpecialObj217.rewardType.SpellRarelv1:
			case SpecialObj217.rewardType.SpellRarelv2:
			case SpecialObj217.rewardType.SpellRarelv3:
			case SpecialObj217.rewardType.SpellEpic:
			case SpecialObj217.rewardType.SpellSpecial:
				type = ItemType.Spell;
				break;
			case SpecialObj217.rewardType.RelicCommon:
			case SpecialObj217.rewardType.RelicRare:
			case SpecialObj217.rewardType.RelicEpic:
				type = ItemType.Relic;
				break;
			case SpecialObj217.rewardType.Coin:
			case SpecialObj217.rewardType.Dimond:
				type = ItemType.Resource;
				break;
			case SpecialObj217.rewardType.Potion:
				type = ItemType.Potion;
				break;
			default:
				Debug.LogError("错误的奖励类型");
				break;
			case SpecialObj217.rewardType.Curse:
			case SpecialObj217.rewardType.Chest:
				break;
			}
			QuickCreateSystem.Inst.CreateItem((overrideRoomController != null) ? overrideRoomController.MapPos : LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(type, id), moveToPointWorldSpace);
		}
		dropAction?.Invoke();
		star1.SetActive(value: false);
		star2.SetActive(value: false);
	}

	public void Initialize(int id, DropType dropType, Vector3 moveToPoint, Vector3? MoveToPointAppearance = null, bool useParticleColor = false, Action dropAction = null, bool isUI = false, bool dropOnEnd = true, RoomController overrideRoomController = null)
	{
		Initialize(id, DropType2So217rewardType(dropType, id), moveToPoint, MoveToPointAppearance, useParticleColor, dropAction, isUI, dropOnEnd, overrideRoomController);
	}

	public void Initialize(int id, DropType dropType, Transform transform, Vector3? MoveToPointAppearance = null, bool useParticleColor = false, Action dropAction = null, bool isUI = false, bool dropOnEnd = true, RoomController overrideRoomController = null)
	{
		Initialize(id, DropType2So217rewardType(dropType, id), transform, MoveToPointAppearance, useParticleColor, dropAction, isUI, dropOnEnd, overrideRoomController);
	}

	public void Initialize(int id, SpecialObj217.rewardType rewardType, Vector3 moveToPoint, Vector3? MoveToPointAppearance = null, bool useParticleColor = false, Action dropAction = null, bool isUI = false, bool dropOnEnd = true, RoomController overrideRoomController = null)
	{
		Initialize(id, rewardType, moveToPoint, (!MoveToPointAppearance.HasValue) ? moveToPoint : MoveToPointAppearance.Value, useParticleColor, dropAction, isUI, dropOnEnd, overrideRoomController);
	}

	public void Initialize(int id, SpecialObj217.rewardType rewardType, Transform moveTotransform, Vector3? MoveToPointAppearance = null, bool useParticleColor = false, Action dropAction = null, bool isUI = false, bool dropOnEnd = true, RoomController overrideRoomController = null)
	{
		Initialize(id, rewardType, moveTotransform, (!MoveToPointAppearance.HasValue) ? moveTotransform.position : MoveToPointAppearance.Value, useParticleColor, dropAction, isUI, dropOnEnd, overrideRoomController);
	}

	public static SpecialObj217.rewardType DropType2So217rewardType(DropType dropType, int id)
	{
		SpecialObj217.rewardType result = SpecialObj217.rewardType.SpellCommonlv1;
		switch (dropType)
		{
		case DropType.Spell:
			if (SpellConfig.dic[id].level == 1 && SpellConfig.dic[id].dropType == ItemDropType.Common)
			{
				result = SpecialObj217.rewardType.SpellCommonlv1;
			}
			else if (SpellConfig.dic[id].level == 2 && SpellConfig.dic[id].dropType == ItemDropType.Common)
			{
				result = SpecialObj217.rewardType.SpellCommonlv2;
			}
			else if (SpellConfig.dic[id].level == 3 && SpellConfig.dic[id].dropType == ItemDropType.Common)
			{
				result = SpecialObj217.rewardType.SpellCommonlv3;
			}
			else if (SpellConfig.dic[id].level == 1 && SpellConfig.dic[id].dropType == ItemDropType.Rare)
			{
				result = SpecialObj217.rewardType.SpellRarelv1;
			}
			else if (SpellConfig.dic[id].level == 2 && SpellConfig.dic[id].dropType == ItemDropType.Rare)
			{
				result = SpecialObj217.rewardType.SpellRarelv2;
			}
			else if (SpellConfig.dic[id].level == 3 && SpellConfig.dic[id].dropType == ItemDropType.Rare)
			{
				result = SpecialObj217.rewardType.SpellRarelv3;
			}
			else if (SpellConfig.dic[id].dropType == ItemDropType.Epic)
			{
				result = SpecialObj217.rewardType.SpellEpic;
			}
			else if (SpellConfig.dic[id].dropType == ItemDropType.Special)
			{
				result = SpecialObj217.rewardType.SpellSpecial;
			}
			else
			{
				Debug.LogError("要掉什么?");
			}
			break;
		case DropType.Relic:
			result = ((RelicConfig.dic[id].dropType != ItemDropType.Common) ? ((RelicConfig.dic[id].dropType != ItemDropType.Rare) ? ((RelicConfig.dic[id].dropType != ItemDropType.Epic) ? SpecialObj217.rewardType.RelicSpecial : SpecialObj217.rewardType.RelicEpic) : SpecialObj217.rewardType.RelicRare) : SpecialObj217.rewardType.RelicCommon);
			break;
		case DropType.Potion:
			result = SpecialObj217.rewardType.Potion;
			break;
		case DropType.Coin:
			result = SpecialObj217.rewardType.Coin;
			break;
		case DropType.Curse:
			result = SpecialObj217.rewardType.Curse;
			break;
		case DropType.Chest:
			result = SpecialObj217.rewardType.Chest;
			break;
		case DropType.Dimond:
			result = SpecialObj217.rewardType.Dimond;
			break;
		case DropType.Wand:
			result = SpecialObj217.rewardType.Wand;
			break;
		default:
			Debug.LogError("要掉什么?");
			break;
		}
		return result;
	}

	public static DropType Convert217rewardType2DromType(SpecialObj217.rewardType rewardType)
	{
		switch (rewardType)
		{
		case SpecialObj217.rewardType.SpellCommonlv1:
		case SpecialObj217.rewardType.SpellCommonlv2:
		case SpecialObj217.rewardType.SpellCommonlv3:
		case SpecialObj217.rewardType.SpellRarelv1:
		case SpecialObj217.rewardType.SpellRarelv2:
		case SpecialObj217.rewardType.SpellRarelv3:
		case SpecialObj217.rewardType.SpellEpic:
		case SpecialObj217.rewardType.SpellSpecial:
			return DropType.Spell;
		case SpecialObj217.rewardType.RelicCommon:
		case SpecialObj217.rewardType.RelicSpecial:
		case SpecialObj217.rewardType.RelicRare:
		case SpecialObj217.rewardType.RelicEpic:
			return DropType.Relic;
		case SpecialObj217.rewardType.Curse:
			return DropType.Curse;
		case SpecialObj217.rewardType.Chest:
			return DropType.Chest;
		case SpecialObj217.rewardType.Coin:
			return DropType.Coin;
		case SpecialObj217.rewardType.Dimond:
			return DropType.Dimond;
		case SpecialObj217.rewardType.Potion:
			return DropType.Potion;
		case SpecialObj217.rewardType.Wand:
			return DropType.Wand;
		default:
			Debug.LogError("错误");
			return DropType.Coin;
		}
	}

	private void Initialize(int id, SpecialObj217.rewardType rewardType, Vector3 moveToPoint, Vector3 moveToPointAppearance, bool useParticleColor = false, Action dropAction = null, bool isUI = false, bool dropOnEnd = true, RoomController overrideRoomController = null)
	{
		this.overrideRoomController = overrideRoomController;
		this.useParticleColor = useParticleColor;
		this.dropAction = dropAction;
		rewardtype = rewardType;
		this.id = id;
		this.moveToPointAppearance = moveToPointAppearance;
		moveToPointWorldSpace = moveToPoint;
		_isUI = isUI;
		_dropOnEnd = dropOnEnd;
		if (_isUI)
		{
			originalPoint = rtsf_Self.anchoredPosition;
		}
		else
		{
			originalPoint = tsf_Self.position;
		}
		middlePoint = originalPoint + middlePointOffset;
		currentLerp = 0f;
		flyFinish = false;
		waitDestroyTimer = 0f;
		go_Image.SetActive(value: true);
		go_FlyFinish.SetActive(value: false);
		star1.SetActive(value: false);
		star2.SetActive(value: false);
		switch (rewardType)
		{
		case SpecialObj217.rewardType.SpellEpic:
			efSpellEpic.SetActive(value: true);
			SetSpriteSpell();
			break;
		case SpecialObj217.rewardType.SpellCommonlv1:
			efSpellCommon.SetActive(value: true);
			SetSpriteSpell();
			break;
		case SpecialObj217.rewardType.SpellCommonlv2:
			OneStar();
			efSpellCommon.SetActive(value: true);
			SetSpriteSpell();
			break;
		case SpecialObj217.rewardType.SpellCommonlv3:
			TwoStar();
			SetSpriteSpell();
			efSpellCommon.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.SpellRarelv1:
			SetSpriteSpell();
			efSpellRare.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.SpellRarelv2:
			OneStar();
			SetSpriteSpell();
			efSpellRare.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.SpellRarelv3:
			TwoStar();
			SetSpriteSpell();
			efSpellRare.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.SpellSpecial:
			SetSpriteSpell();
			efRelicSpecial.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.RelicCommon:
			efRelicCommon.SetActive(value: true);
			SetRelic();
			break;
		case SpecialObj217.rewardType.RelicRare:
			efRelicRare.SetActive(value: true);
			SetRelic();
			break;
		case SpecialObj217.rewardType.RelicEpic:
			efSpellEpic.SetActive(value: true);
			SetRelic();
			break;
		case SpecialObj217.rewardType.RelicSpecial:
			efRelicSpecial.SetActive(value: true);
			SetRelic();
			break;
		case SpecialObj217.rewardType.Curse:
			SetCurse();
			break;
		case SpecialObj217.rewardType.Coin:
			SetCoin();
			efSpellCommon.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.Dimond:
			SetDimond();
			efSpellCommon.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.Potion:
			efRelicCommon.SetActive(value: true);
			SetPotion();
			break;
		case SpecialObj217.rewardType.Wand:
			efRelicCommon.SetActive(value: true);
			SetWand();
			break;
		}
		SEMgr.Inst.so217rewardFly.PlaySE();
	}

	private void Initialize(int id, SpecialObj217.rewardType rewardType, Transform moveToPoint, Vector3 moveToPointAppearance, bool useParticleColor = false, Action dropAction = null, bool isUI = false, bool dropOnEnd = true, RoomController overrideRoomController = null)
	{
		this.overrideRoomController = overrideRoomController;
		this.useParticleColor = useParticleColor;
		this.dropAction = dropAction;
		rewardtype = rewardType;
		this.id = id;
		this.moveToPointAppearance = moveToPointAppearance;
		transformFollow = moveToPoint;
		_isUI = isUI;
		_dropOnEnd = dropOnEnd;
		if (_isUI)
		{
			originalPoint = rtsf_Self.anchoredPosition;
		}
		else
		{
			originalPoint = tsf_Self.position;
		}
		middlePoint = originalPoint + middlePointOffset;
		currentLerp = 0f;
		flyFinish = false;
		waitDestroyTimer = 0f;
		go_Image.SetActive(value: true);
		go_FlyFinish.SetActive(value: false);
		star1.SetActive(value: false);
		star2.SetActive(value: false);
		switch (rewardType)
		{
		case SpecialObj217.rewardType.SpellEpic:
			efSpellEpic.SetActive(value: true);
			SetSpriteSpell();
			break;
		case SpecialObj217.rewardType.SpellCommonlv1:
			efSpellCommon.SetActive(value: true);
			SetSpriteSpell();
			break;
		case SpecialObj217.rewardType.SpellCommonlv2:
			OneStar();
			efSpellCommon.SetActive(value: true);
			SetSpriteSpell();
			break;
		case SpecialObj217.rewardType.SpellCommonlv3:
			TwoStar();
			SetSpriteSpell();
			efSpellCommon.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.SpellRarelv1:
			SetSpriteSpell();
			efSpellRare.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.SpellRarelv2:
			OneStar();
			SetSpriteSpell();
			efSpellRare.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.SpellRarelv3:
			TwoStar();
			SetSpriteSpell();
			efSpellRare.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.SpellSpecial:
			SetSpriteSpell();
			efRelicSpecial.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.RelicCommon:
			efRelicCommon.SetActive(value: true);
			SetRelic();
			break;
		case SpecialObj217.rewardType.RelicRare:
			efRelicRare.SetActive(value: true);
			SetRelic();
			break;
		case SpecialObj217.rewardType.RelicEpic:
			efSpellEpic.SetActive(value: true);
			SetRelic();
			break;
		case SpecialObj217.rewardType.RelicSpecial:
			efRelicSpecial.SetActive(value: true);
			SetRelic();
			break;
		case SpecialObj217.rewardType.Curse:
			SetCurse();
			break;
		case SpecialObj217.rewardType.Coin:
			SetCoin();
			efSpellCommon.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.Dimond:
			SetDimond();
			efSpellCommon.SetActive(value: true);
			break;
		case SpecialObj217.rewardType.Potion:
			efRelicCommon.SetActive(value: true);
			SetPotion();
			break;
		case SpecialObj217.rewardType.Wand:
			efRelicCommon.SetActive(value: true);
			SetWand();
			break;
		}
		SEMgr.Inst.so217rewardFly.PlaySE();
	}

	private void OneStar()
	{
		star1.SetActive(value: true);
		star2.SetActive(value: false);
	}

	private void TwoStar()
	{
		star1.SetActive(value: true);
		star2.SetActive(value: true);
	}

	private void SetSpriteSpell()
	{
		setParticleColor(GetColor());
		if (id != -1)
		{
			if (_isUI)
			{
				image.sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[id].GetIconPath());
			}
			else
			{
				spriteRenderer.sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[id].GetIconPath());
			}
		}
	}

	public void OverrideSprite(Sprite sprite)
	{
		if (_isUI)
		{
			image.sprite = sprite;
		}
		else
		{
			spriteRenderer.sprite = sprite;
		}
	}

	private void SetRelic()
	{
		if (_isUI)
		{
			image.sprite = ABResources.LoadAsset<Sprite>(RelicConfig.dic[id].GetIconPath());
		}
		else
		{
			spriteRenderer.sprite = ABResources.LoadAsset<Sprite>(RelicConfig.dic[id].GetIconPath());
		}
		setParticleColor(GetColor());
	}

	private void SetPotion()
	{
		if (_isUI)
		{
			image.sprite = ABResources.LoadAsset<Sprite>(PotionConfig.dic[id].GetIconPath());
			image.gameObject.transform.localScale = Vector3.one * 0.6f;
		}
		else
		{
			spriteRenderer.sprite = ABResources.LoadAsset<Sprite>(PotionConfig.dic[id].GetIconPath());
			spriteRenderer.gameObject.transform.localScale = Vector3.one * 0.6f;
		}
		setParticleColor(colorDefault);
	}

	private void SetWand()
	{
		if (_isUI)
		{
			image.sprite = ABResources.LoadAsset<Sprite>(WandConfig.dic[id].GetIconPath());
			image.gameObject.transform.localScale = Vector3.one * 1.5f;
		}
		else
		{
			spriteRenderer.sprite = ABResources.LoadAsset<Sprite>(WandConfig.dic[id].GetIconPath());
			spriteRenderer.gameObject.transform.localScale = Vector3.one * 1.5f;
		}
		setParticleColor(colorDefault);
	}

	private void SetCurse()
	{
		if (_isUI)
		{
			image.sprite = ABResources.LoadAsset<Sprite>(CurseConfig.dic[id].GetIconPath());
		}
		else
		{
			spriteRenderer.sprite = ABResources.LoadAsset<Sprite>(CurseConfig.dic[id].GetIconPath());
		}
		setParticleColor(colorCurse);
	}

	private void OnDestroy()
	{
		ParticleSystem.MainModule main = ps_Finish.main;
		main.startColor = Color.clear;
		main = ps_Paush.main;
		main.startColor = Color.clear;
		if (_isUI)
		{
			main = ps_FinishGlow.main;
			main.startColor = Color.clear;
		}
	}

	private void SetCoin()
	{
		if (_isUI)
		{
			image.sprite = ABResources.LoadAsset<Sprite>("Textures/ResourceIcons/" + 11);
		}
		else
		{
			spriteRenderer.sprite = ABResources.LoadAsset<Sprite>("Textures/ResourceIcons/" + 11);
		}
		setParticleColor(colorDefault);
	}

	private void SetDimond()
	{
		if (_isUI)
		{
			image.sprite = ABResources.LoadAsset<Sprite>("Textures/ResourceIcons/" + 12);
		}
		else
		{
			spriteRenderer.sprite = ABResources.LoadAsset<Sprite>("Textures/ResourceIcons/" + 12);
		}
		setParticleColor(colorDefault);
	}

	private void setParticleColor(Color color)
	{
		if (!useParticleColor)
		{
			color = colorDefault;
		}
		ParticleSystem.MainModule main = ps_Finish.main;
		main.startColor = new Color(color.r, color.g, color.b, ps_FinishColorAlpha);
		main = ps_Paush.main;
		main.startColor = color;
		if (_isUI)
		{
			main = ps_FinishGlow.main;
			main.startColor = new Color(color.r, color.g, color.b, ps_FinishGlowColorAlpha);
		}
	}

	private Color GetColor()
	{
		if (rewardtype == SpecialObj217.rewardType.SpellCommonlv1 || rewardtype == SpecialObj217.rewardType.SpellCommonlv2 || rewardtype == SpecialObj217.rewardType.SpellCommonlv3)
		{
			return colorSpellCommon;
		}
		if (rewardtype == SpecialObj217.rewardType.SpellRarelv1 || rewardtype == SpecialObj217.rewardType.SpellRarelv2 || rewardtype == SpecialObj217.rewardType.SpellRarelv3)
		{
			return colorSpellRare;
		}
		if (rewardtype == SpecialObj217.rewardType.SpellEpic)
		{
			return colorSpellEpic;
		}
		if (rewardtype == SpecialObj217.rewardType.SpellSpecial)
		{
			return colorSpellSpecial;
		}
		if (rewardtype == SpecialObj217.rewardType.RelicCommon)
		{
			return colorRelicCommon;
		}
		if (rewardtype == SpecialObj217.rewardType.RelicRare)
		{
			return colorRelicRare;
		}
		if (rewardtype == SpecialObj217.rewardType.RelicEpic)
		{
			return colorRelicEpic;
		}
		if (rewardtype == SpecialObj217.rewardType.RelicSpecial)
		{
			return colorRelicSpecial;
		}
		return colorDefault;
	}
}
