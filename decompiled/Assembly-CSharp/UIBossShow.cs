using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UIBossShow")]
public class UIBossShow : GameUISingletonMono<UIBossShow>
{
	public Animator animator;

	public Image image_BossShow;

	public Text text_BossShow;

	public Image image_TextBG;

	public Image image_BG;

	public Sprite Sprite_TextBGH;

	public Sprite sprite_BG_H;

	private int bossID;

	protected override void RegistarWhenInit()
	{
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
	}

	private void Update()
	{
		if (base.IsOpen && Time.timeScale != 0.01f)
		{
			Time.timeScale = 0.01f;
		}
	}

	protected override void OnShow(object t = null)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (t is Entity)
		{
			Entity entity = (Entity)t;
			SEMgr.Inst.bossShow.PlaySE();
			UnitProperty_Dots componentData = entityManager.GetComponentData<UnitProperty_Dots>(entity);
			int num = (bossID = componentData.unitCfg.id);
			if (num == 301201 || num == 301202)
			{
				num = 301201;
				text_BossShow.text = 1005001.GetText();
			}
			else
			{
				text_BossShow.text = componentData.unitCfg.GetName();
			}
			string text = "Textures/BossIcons/" + num;
			string nameH = "Textures/BossIconsH/" + num;
			image_BossShow.sprite = ABResources.LoadAsset<Sprite>(text);
			if (GameMgr.IsChAge14_Static)
			{
				image_TextBG.sprite = Sprite_TextBGH;
				image_BG.sprite = sprite_BG_H;
			}
			if (GameMgr.IsHarmony_Static)
			{
				image_BossShow.sprite = (componentData.unitCfg.haveGalleryH ? ABResources.LoadHarmonizableAsset<Sprite>(text, nameH) : ABResources.LoadAsset<Sprite>(text));
			}
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			animator.SetTrigger("Show");
			MusicMgr.Inst.ForcePlayMusic("", playAmbient: false);
		}
	}

	protected override void OnHide()
	{
		Time.timeScale = 1f;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		if (BattleMgr.Inst.CurrentStage == 10 && LevelMgr.Inst.CurrentRoomCfg.type != RoomType.Elite && (bossID == 509901 || bossID == 500621))
		{
			MusicMgr.Inst.ForcePlayMusic("BGM_BossChapter5");
		}
		else
		{
			MusicMgr.Inst.ForcePlayMusic(GameConstManaged.bgm_Boss);
		}
	}

	private void _BossShowEnd()
	{
		Hide();
	}
}
