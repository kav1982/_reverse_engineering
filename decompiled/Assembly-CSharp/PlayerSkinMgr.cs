using System.Collections.Generic;
using System.Linq;
using Spine;
using Spine.Unity;
using UnityEngine;

public class PlayerSkinMgr : MonoBehaviour
{
	public SkeletonAnimation sAnima;

	public const string part_lian = "lian";

	public const string part_toufa = "toufa";

	public const string part_yifu = "yifu";

	public const string part_kuzi = "kuzi";

	public const string part_xie = "xie";

	public const string part_bozi = "bozi";

	public const string part_bozi_1 = "bozi_1";

	public const string part_pifeng = "pifeng";

	public const string part_shou = "shou";

	public const string part_shouchi = "shouchi";

	public const string part_maozi = "maozi";

	public const string part_jianjia = "jianjia";

	public const string part_mianju = "mianju";

	public const string part_beishi = "beishi";

	public const string part_yanjing = "yanjing";

	public const string part_erduo = "erduo";

	public const string part_erduo2 = "erduo2";

	public const string part_weiba = "weiba";

	public const string part_shipin = "shipin";

	public const string SkinStr_Base = "jichu";

	public const string SkinStr_Original = "chushi";

	public const string SkinStr_Default_lian = "lian/lian1";

	public const string SkinStr_Default_bozi = "bozi/bozi1";

	public const string SkinStr_Default_bozi_1 = "bozi/bozi1_1";

	public const string SkinStr_Default_toufa = "toufa/toufa1";

	public const string SkinStr_Default_yifu = "yifu/yifu1";

	public const string SkinStr_Default_kuzi = "kuzi/kuzi1";

	public const string SkinStr_Default_xie = "xie/xie1";

	public const string SkinStr_Jojo_lian = "lian/lian2";

	public const string SkinStr_PrettyGirl_lian = "lian/lian4";

	public const string SkinStr_PrettyGirl_bozi = "bozi/bozi2";

	public const string SkinStr_PrettyGirl_bozi_1 = "bozi/bozi2_1";

	public const string SkinStr_PrettyGirl_toufa = "toufa/toufa4";

	public const string SkinStr_PrettyGirl_yifu = "yifu/yifu6";

	public const string SkinStr_PrettyGirl_kuzi = "kuzi/kuzi2";

	public const string SkinStr_PrettyGirl_xie = "xie/xie6";

	public const string SkinStr_TVMan_lian = "lian/lian8";

	public const string SkinStr_TVMan_bozi = "bozi/bozi3";

	public const string SkinStr_TVMan_bozi_1 = "bozi/bozi3_1";

	public const string SkinStr_TVMan_toufa = "toufa/toufa7";

	public const string SkinStr_TVMan_yifu = "yifu/yifu9";

	public const string SkinStr_TVMan_kuzi = "kuzi/kuzi5";

	public const string SkinStr_TVMan_xie = "xie/xie9";

	public const string SkinStr_TVMan_shou = "shou/shou4";

	public const string SkinStr_Nvliu_lian = "lian/lian9";

	public const string SkinStr_Nvliu_toufa = "toufa/toufa9";

	public const string SkinStr_Nvliu_yifu = "yifu/yifu10";

	public const string SkinStr_Nvliu_kuzi = "kuzi/kuzi6";

	public const string SkinStr_Nvliu_xie = "xie/xie10";

	public const string SkinStr_Tomato_lian = "lian/lian3";

	public const string SkinStr_Tomato_toufa = "toufa/toufa3";

	public const string SkinStr_Tomato_yifu = "yifu/yifu5";

	public const string SkinStr_Tomato_kuzi = "kuzi/kuzi7";

	public const string SkinStr_Tomato_xie = "xie/xie11";

	public const string SkinStr_Frog_lian = "lian/lian5";

	public const string SkinStr_Frog_bozi = "bozi/bozi4";

	public const string SkinStr_Frog_bozi_1 = "bozi/bozi4_1";

	public const string SkinStr_Frog_toufa = "toufa/toufa10";

	public const string SkinStr_Frog_yifu = "yifu/yifu11";

	public const string SkinStr_Frog_kuzi = "kuzi/kuzi8";

	public const string SkinStr_Frog_xie = "xie/xie12";

	public const string SkinStr_Frog_shou = "shou/shou5";

	public const string SkinStr_Halloween_pifeng = "pifeng/pifeng7";

	public const string SkinStr_Halloween_yifu = "yifu/yifu15";

	public const string SkinStr_Halloween_toufa = "toufa/toufa13";

	public const string SkinStr_Halloween_xie = "xie/xie15";

	public const string SkinStr_Halloween_kuzi = "kuzi/kuzi11";

	public const string SkinStr_Halloween_lian = "lian/lian12";

	public const string SkinStr_TapTap_toufa = "toufa/toufa14_tap";

	public const string SkinStr_TapTap_xie = "xie/xie16_tap";

	public const string SkinStr_TapTap_kuzi = "kuzi/kuzi12_tap";

	public const string SkinStr_TapTap_lian = "lian/lian13_tap";

	public const string SkinStr_TapTap_yifu = "yifu/yifu16_tap";

	public const string SkinStr_TapTap_shou = "shou/shou6_tap";

	public const string SkinStr_HaoYou_toufa = "toufa/toufa15_haoyou";

	public const string SkinStr_HaoYou_xie = "xie/xie17_haoyou";

	public const string SkinStr_HaoYou_kuzi = "kuzi/kuzi13_haoyou";

	public const string SkinStr_HaoYou_lian = "lian/lian14_haoyou";

	public const string SkinStr_HaoYou_pifeng = "pifeng/pifeng8_haoyou";

	public const string SkinStr_HaoYou_yifu = "yifu/yifu17_haoyou";

	public const string SkinStr_Cat_toufa = "toufa/toufa16_mao";

	public const string SkinStr_Cat_xie = "xie/xie18_maoniang";

	public const string SkinStr_Cat_kuzi = "kuzi/kuzi14_maoniang";

	public const string SkinStr_Cat_lian = "lian/lian15_mao";

	public const string SkinStr_Cat_weiba = "weiba/weiba_maoniang";

	public const string SkinStr_Cat_erduo = "erduo2/erduo2_maoniang";

	public const string SkinStr_Cat_yifu = "yifu/yifu18_mao";

	public const string SkinStr_Cat_beishi = "beishi/beishi7_maoniang";

	public const string SkinStr_XingNan_toufa = "toufa/toufa17";

	public const string SkinStr_XingNan_xie = "xie/xie19";

	public const string SkinStr_XingNan_kuzi = "kuzi/kuzi15";

	public const string SkinStr_XingNan_yifu = "yifu/yifu19";

	public const string SkinStr_XingNan_lian = "lian/lian16";

	public const string SkinStr_Christmas_toufa = "toufa/toufa18_xueren";

	public const string SkinStr_Christmas_xie = "xie/xie20_xueren";

	public const string SkinStr_Christmas_kuzi = "kuzi/kuzi16_xueren";

	public const string SkinStr_Christmas_yifu = "yifu/yifu20_xueren";

	public const string SkinStr_Christmas_lian = "lian/lian17_xueren";

	public const string SkinStr_Christmas_bozi = "bozi/bozi7";

	public const string SkinStr_Christmas_bozi_1 = "bozi/bozi7_1";

	public const string SkinStr_Spring_toufa = "toufa/toufa19_ma";

	public const string SkinStr_Spring_erduo = "erduo2/erduo2_ma";

	public const string SkinStr_Spring_xie = "xie/xie21_ma";

	public const string SkinStr_Spring_kuzi = "kuzi/kuzi17_ma";

	public const string SkinStr_Spring_yifu = "yifu/yifu21_ma";

	public const string SkinStr_Spring_lian = "lian/lian18_ma";

	public const string SkinStr_Spring_bozi = "bozi/bozi8";

	public const string SkinStr_Spring_bozi_1 = "bozi/bozi8_1";

	public const string SkinStr_Spring_shou = "shou/shou7_ma";

	public const string SkinStr_Spring_weiba = "weiba/weiba_ma";

	public const string SkinStr_SummerBoy_toufa = "toufa/toufa21_xia_nan";

	public const string SkinStr_SummerBoy_mianju = "mianju/mianju6_xia_nan";

	public const string SkinStr_SummerBoy_lian = "lian/lian20_xia_nan";

	public const string SkinStr_SummerBoy_yifu = "yifu/yifu23_xia_nan";

	public const string SkinStr_SummerBoy_shipin = "shipin/yaodai3_xia_nan";

	public const string SkinStr_SummerBoy_kuzi = "kuzi/kuzi19_xia_nan";

	public const string SkinStr_SummerBoy_xie = "xie/xie23_xia_nan";

	public const string SkinStr_SummerGirl_maozi = "maozi/maozi10_xia_nv";

	public const string SkinStr_SummerGirl_toufa = "toufa/toufa20_xia_nv";

	public const string SkinStr_SummerGirl_lian = "lian/lian19_xia_nv";

	public const string SkinStr_SummerGirl_yifu = "yifu/yifu22_xia_nv";

	public const string SkinStr_SummerGirl_kuzi = "kuzi/kuzi18_xia_nv";

	public const string SkinStr_SummerGirl_xie = "xie/xie22_xia_nv";

	public const string SkinStr_SummerGirl_bozi = "bozi/bozi2";

	public const string SkinStr_SummerGirl_bozi_1 = "bozi/bozi2_1";

	public const string SkinStr_WarmSnow_maozi = "maozi/maozi6";

	public const string SkinStr_WarmSnow_pifeng = "pifeng/pifeng6";

	public const string SkinStr_WarmSnow_lian = "lian/lian6";

	public const string SkinStr_WarmSnow_kuzi = "kuzi/kuzi3";

	public const string SkinStr_WarmSnow_bozi = "bozi/bozi1";

	public const string SkinStr_WarmSnow_yifu = "yifu/yifu7";

	public const string SkinStr_WarmSnow_xie = "xie/xie7";

	public const string SkinStr_WarmSnow_toufa = "toufa/toufa5";

	public const string SkinStr_Reaper_lian = "lian/lian7";

	public const string SkinStr_Reaper_kuzi = "kuzi/kuzi4";

	public const string SkinStr_Reaper_bozi = "bozi/bozi2";

	public const string SkinStr_Reaper_yifu = "yifu/yifu8";

	public const string SkinStr_Reaper_xie = "xie/xie8";

	public const string SkinStr_Reaper_toufa = "toufa/toufa6";

	public const string SkinStr_Reaper_guancai = "shipin/guancai";

	public const string SkinStr_Dave_bozi = "bozi/bozi5";

	public const string SkinStr_Dave_bozi_1 = "bozi/bozi5_1";

	public const string SkinStr_Dave_lian = "lian/lian10";

	public const string SkinStr_Dave_kuzi = "kuzi/kuzi9";

	public const string SkinStr_Dave_xie = "xie/xie13";

	public const string SkinStr_Dave_toufa = "toufa/toufa11";

	public const string SkinStr_Dave_maozi = "maozi/maozi8";

	public const string SkinStr_Dave_yifu = "yifu/yifu12";

	public const string SkinStr_DaveSwim_bozi = "bozi/bozi6";

	public const string SkinStr_DaveSwim_bozi_1 = "bozi/bozi6_1";

	public const string SkinStr_DaveSwim_lian = "lian/lian11";

	public const string SkinStr_DaveSwim_kuzi = "kuzi/kuzi10";

	public const string SkinStr_DaveSwim_xie = "xie/xie14";

	public const string SkinStr_DaveSwim_toufa = "toufa/toufa12";

	public const string SkinStr_DaveSwim_maozi = "maozi/maozi9";

	public const string SkinStr_DaveSwim_yifu = "yifu/yifu13";

	public const string SkinStr_DaveSwim_beishi = "beishi/beishi6";

	public const string SkinStr_DaveSwimDLCRoom_mianju = "mianju/mianju5";

	public const string SkinStr_DaveSwimDLCRoom_beishi = "beishi/beishi6_2";

	public const string Slot_BodyH = "Body_H";

	public const string Slot_BodyQ = "Body_Q";

	public const string Slot_HandL = "Hand_L";

	public const string Slot_HandR = "Hand_R";

	public const string Slot_shouchiL = "shouchi_l";

	public const string Slot_shouchiR = "shouchi_r";

	public const string Slot_UpperLeg_L = "tui_l";

	public const string Slot_UpperLeg_R = "tui_r";

	public const string Slot_Shoes_L = "xie_l";

	public const string Slot_Shoes_R = "xie_r";

	public const string Slot_Shoes_L2 = "bilibili_xie_l";

	public const string Slot_Shoes_R2 = "bilibili_xie_r";

	public const string Slot_LowerLeg_L = "xiaotui_l";

	public const string Slot_LowerLeg_R = "xiaotui_r";

	public const string Slot_DaveLeg1 = "dave_tui_l";

	public const string Slot_DaveLeg2 = "dave_tui_r";

	public const string Slot_DaveLeg3 = "dave_xiaotui_l";

	public const string Slot_DaveLeg4 = "dave_xiaotui_r";

	public const string Slot_DaveLeg5 = "dave_xie_l";

	public const string Slot_DaveLeg6 = "dave_xie_r";

	public const string Bone_bozi1 = "bozi";

	public const string Bone_bozi2 = "bozi2";

	public const string Bone_zhuan1 = "zhuan1";

	public const string Bone_zhuan2 = "zhuan2";

	public const float boneZhuanAngle = 60f;

	public const string SkinStr_maozi2 = "maozi/maozi2";

	public const string SkinStr_maozi5 = "maozi/maozi5";

	public const string SkinStr_maozi7 = "maozi/maozi7";

	public const string SkinStr_toufa8 = "toufa/toufa8";

	public const string SkinStr_toufa8_tv = "toufa/toufa8_tv";

	public const string SkinStr_toufa8_lfq = "toufa/toufa8_lfq";

	public const string SkinStr_toufa8_qw = "toufa/toufa8_qw";

	public const string SkinStr_toufa8_2 = "toufa/toufa8_2";

	public const string SkinStr_toufa8_2_tv = "toufa/toufa8_2_tv";

	public const string SkinStr_toufa8_2_lfq = "toufa/toufa8_2_lfq";

	public const string SkinStr_toufa8_2_qw = "toufa/toufa8_2_qw";

	public const string SkinStr_toufa8_dave = "toufa/toufa8_dave";

	public const string SkinStr_toufa8_2_dave = "toufa/toufa8_2_dave";

	private Skeleton skeleton;

	private SkeletonData skeletonData;

	public static PlayerSkinMgr Inst { get; private set; }

	public Vector2 Originalbozi2LocalPoint { get; private set; }

	public Vector2 Originalbozi1LocalPoint { get; private set; }

	public float Originalzhuan1Rotation { get; private set; }

	public float Originalzhuan2Rotation { get; private set; }

	private Skin GetSkin(string path)
	{
		return skeletonData.FindSkin(path);
	}

	public void Initialize()
	{
		Inst = this;
		skeleton = sAnima.skeleton;
		skeletonData = skeleton.Data;
		Originalbozi1LocalPoint = sAnima.skeleton.FindBone("bozi").GetLocalPosition();
		Originalbozi2LocalPoint = sAnima.skeleton.FindBone("bozi2").GetLocalPosition();
		Originalzhuan1Rotation = sAnima.skeleton.FindBone("zhuan1").Rotation;
		Originalzhuan2Rotation = sAnima.skeleton.FindBone("zhuan2").Rotation;
	}

	public void SetSkin(Skeleton skeleton, PlayerLook playerLook, List<RelicConfig> relicCfgs, bool ignoreDisableRelicSkin = false, bool inBattle = false)
	{
		if (!ignoreDisableRelicSkin)
		{
			relicCfgs = relicCfgs.Where((RelicConfig e) => !DataMgr.settingData.DisableRelicSkins.Contains(e.id)).ToList();
		}
		Skin skin = new Skin("NewSkin");
		skin.AddSkin(GetSkin("jichu"));
		RelicConfig relicConfig = null;
		RelicConfig relicConfig2 = null;
		RelicConfig relicConfig3 = null;
		RelicConfig relicConfig4 = null;
		RelicConfig relicConfig5 = null;
		RelicConfig relicConfig6 = null;
		RelicConfig relicConfig7 = null;
		RelicConfig relicConfig8 = null;
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		string text9 = "";
		string text10 = "";
		string text11 = "";
		string text12 = "";
		string text13 = "";
		string text14 = "";
		string text15 = "";
		string text16 = "";
		string text17 = "";
		string text18 = "";
		switch (playerLook)
		{
		case PlayerLook.Default:
			text = "lian/lian1";
			text5 = "xie/xie1";
			text3 = "yifu/yifu1";
			text4 = "kuzi/kuzi1";
			text2 = "toufa/toufa1";
			break;
		case PlayerLook.Jojo:
			text = "lian/lian2";
			text5 = "xie/xie1";
			text3 = "yifu/yifu1";
			text4 = "kuzi/kuzi1";
			text2 = "toufa/toufa1";
			break;
		case PlayerLook.PrettyGril:
			text = "lian/lian4";
			text5 = "xie/xie6";
			text3 = "yifu/yifu6";
			text4 = "kuzi/kuzi2";
			text2 = "toufa/toufa4";
			break;
		case PlayerLook.TVMan:
			text = "lian/lian8";
			text5 = "xie/xie9";
			text3 = "yifu/yifu9";
			text4 = "kuzi/kuzi5";
			text2 = "toufa/toufa7";
			text9 = "shou/shou4";
			break;
		case PlayerLook.Nvliu:
			text = "lian/lian9";
			text5 = "xie/xie10";
			text3 = "yifu/yifu10";
			text4 = "kuzi/kuzi6";
			text2 = "toufa/toufa9";
			break;
		case PlayerLook.Tomato:
			text = "lian/lian3";
			text5 = "xie/xie11";
			text3 = "yifu/yifu5";
			text4 = "kuzi/kuzi7";
			text2 = "toufa/toufa3";
			break;
		case PlayerLook.Frog:
			text = "lian/lian5";
			text5 = "xie/xie12";
			text3 = "yifu/yifu11";
			text4 = "kuzi/kuzi8";
			text2 = "toufa/toufa10";
			text9 = "shou/shou5";
			break;
		case PlayerLook.Halloween:
			text8 = "pifeng/pifeng7";
			text3 = "yifu/yifu15";
			text2 = "toufa/toufa13";
			text5 = "xie/xie15";
			text4 = "kuzi/kuzi11";
			text = "lian/lian12";
			break;
		case PlayerLook.TapTap:
			text2 = "toufa/toufa14_tap";
			text5 = "xie/xie16_tap";
			text4 = "kuzi/kuzi12_tap";
			text = "lian/lian13_tap";
			text3 = "yifu/yifu16_tap";
			text9 = "shou/shou6_tap";
			break;
		case PlayerLook.HaoYou:
			text2 = "toufa/toufa15_haoyou";
			text5 = "xie/xie17_haoyou";
			text4 = "kuzi/kuzi13_haoyou";
			text = "lian/lian14_haoyou";
			text3 = "yifu/yifu17_haoyou";
			text8 = "pifeng/pifeng8_haoyou";
			break;
		case PlayerLook.MaoNiang:
			text2 = "toufa/toufa16_mao";
			text5 = "xie/xie18_maoniang";
			text4 = "kuzi/kuzi14_maoniang";
			text = "lian/lian15_mao";
			text3 = "yifu/yifu18_mao";
			text14 = "beishi/beishi7_maoniang";
			text17 = "erduo2/erduo2_maoniang";
			text18 = "weiba/weiba_maoniang";
			break;
		case PlayerLook.XingNan:
			text2 = "toufa/toufa17";
			text5 = "xie/xie19";
			text4 = "kuzi/kuzi15";
			text = "lian/lian16";
			text3 = "yifu/yifu19";
			break;
		case PlayerLook.Horse:
			text2 = "toufa/toufa19_ma";
			text17 = "erduo2/erduo2_ma";
			text5 = "xie/xie21_ma";
			text4 = "kuzi/kuzi17_ma";
			text = "lian/lian18_ma";
			text3 = "yifu/yifu21_ma";
			text9 = "shou/shou7_ma";
			text18 = "weiba/weiba_ma";
			break;
		case PlayerLook.SummerBoy:
			text2 = "toufa/toufa21_xia_nan";
			text5 = "xie/xie23_xia_nan";
			text4 = "kuzi/kuzi19_xia_nan";
			text3 = "yifu/yifu23_xia_nan";
			text13 = "mianju/mianju6_xia_nan";
			text = "lian/lian20_xia_nan";
			skin.AddSkin(GetSkin("shipin/yaodai3_xia_nan"));
			break;
		case PlayerLook.SummerGirl:
			text2 = "toufa/toufa20_xia_nv";
			text5 = "xie/xie22_xia_nv";
			text4 = "kuzi/kuzi18_xia_nv";
			text3 = "yifu/yifu22_xia_nv";
			text = "lian/lian19_xia_nv";
			text11 = "maozi/maozi10_xia_nv";
			text6 = "bozi/bozi2";
			text7 = "bozi/bozi2_1";
			break;
		case PlayerLook.SnowMan:
			text2 = "toufa/toufa18_xueren";
			text5 = "xie/xie20_xueren";
			text4 = "kuzi/kuzi16_xueren";
			text = "lian/lian17_xueren";
			text3 = "yifu/yifu20_xueren";
			text9 = "shou/shou4";
			break;
		default:
			Debug.LogError(playerLook);
			break;
		}
		for (int i = 0; i < relicCfgs.Count; i++)
		{
			switch (relicCfgs[i].abilityType)
			{
			case RelicAbilityType.LongNeck:
				relicConfig = relicCfgs[i];
				break;
			case RelicAbilityType.ShowUnitHP:
				relicConfig2 = relicCfgs[i];
				break;
			case RelicAbilityType.PowerfulMan:
				relicConfig3 = relicCfgs[i];
				break;
			case RelicAbilityType.MadWarrior:
				relicConfig4 = relicCfgs[i];
				break;
			case RelicAbilityType.PickMoreRelic:
				relicConfig5 = relicCfgs[i];
				break;
			case RelicAbilityType.WarmSnow:
				relicConfig6 = relicCfgs[i];
				text11 = "maozi/maozi6";
				text8 = "pifeng/pifeng6";
				text = "lian/lian6";
				text4 = "kuzi/kuzi3";
				text6 = "bozi/bozi1";
				text3 = "yifu/yifu7";
				text5 = "xie/xie7";
				text2 = "toufa/toufa5";
				text9 = "";
				text17 = "";
				text18 = "";
				text14 = "";
				text13 = "";
				break;
			case RelicAbilityType.Reaper:
				relicConfig7 = relicCfgs[i];
				text = "lian/lian7";
				text4 = "kuzi/kuzi4";
				text6 = "bozi/bozi2";
				text3 = "yifu/yifu8";
				text5 = "xie/xie8";
				text2 = "toufa/toufa6";
				text9 = "";
				text8 = "";
				text17 = "";
				text18 = "";
				text14 = "";
				text13 = "";
				skin.AddSkin(GetSkin("shipin/guancai"));
				break;
			case RelicAbilityType.Hunag:
				return;
			case RelicAbilityType.DivingSuit:
				relicConfig8 = relicCfgs[i];
				text = (inBattle ? "lian/lian11" : "lian/lian10");
				text4 = (inBattle ? "kuzi/kuzi10" : "kuzi/kuzi9");
				text6 = (inBattle ? "bozi/bozi6" : "bozi/bozi5");
				text11 = (inBattle ? "maozi/maozi9" : "maozi/maozi8");
				text5 = (inBattle ? "xie/xie14" : "xie/xie13");
				text2 = (inBattle ? "toufa/toufa12" : "toufa/toufa11");
				text3 = (inBattle ? "yifu/yifu13" : "yifu/yifu12");
				text14 = (inBattle ? "beishi/beishi6" : "");
				if (DataMgr.selectedWorldData.InDaveRoom)
				{
					text13 = "mianju/mianju5";
					text14 = "beishi/beishi6_2";
					text11 = "";
				}
				else
				{
					text13 = "";
				}
				text9 = "";
				text8 = "";
				text17 = "";
				text18 = "";
				break;
			}
			if (!(relicCfgs[i].skinName != ""))
			{
				continue;
			}
			string[] array = relicCfgs[i].skinName.Split('/');
			if (array.Length != 2)
			{
				Debug.LogError(relicCfgs[i].skinName + " 该皮肤路径不对");
				continue;
			}
			string text19 = relicCfgs[i].skinName;
			if (relicConfig8 != null && relicCfgs[i].skinNameDave != "")
			{
				text19 = relicCfgs[i].skinNameDave;
			}
			else if (playerLook == PlayerLook.TVMan && relicConfig6 == null && relicConfig7 == null && relicConfig8 == null && relicCfgs[i].skinNameTvMan != "")
			{
				text19 = relicCfgs[i].skinNameTvMan;
			}
			else if (playerLook == PlayerLook.Tomato && relicConfig6 == null && relicConfig7 == null && relicConfig8 == null && relicCfgs[i].skinNameTomato != "")
			{
				text19 = relicCfgs[i].skinNameTomato;
			}
			else if (playerLook == PlayerLook.Frog && relicConfig6 == null && relicConfig7 == null && relicConfig8 == null && relicCfgs[i].skinNameFrog != "")
			{
				text19 = relicCfgs[i].skinNameFrog;
			}
			else if (playerLook == PlayerLook.Halloween && relicConfig6 == null && relicConfig7 == null && relicConfig8 == null && relicCfgs[i].skinNameHalloween != "")
			{
				text19 = relicCfgs[i].skinNameHalloween;
			}
			else if (playerLook == PlayerLook.SnowMan && relicConfig6 == null && relicConfig7 == null && relicConfig8 == null && relicCfgs[i].skinNameChristmas != "")
			{
				text19 = relicCfgs[i].skinNameChristmas;
			}
			else if (playerLook == PlayerLook.Horse && relicConfig6 == null && relicConfig7 == null && relicConfig8 == null && relicCfgs[i].skinNameSpring != "")
			{
				text19 = relicCfgs[i].skinNameSpring;
			}
			switch (array[0])
			{
			case "toufa":
				text2 = text19;
				break;
			case "yifu":
				text3 = text19;
				break;
			case "kuzi":
				text4 = text19;
				break;
			case "xie":
				text5 = text19;
				break;
			case "lian":
				text = text19;
				break;
			case "bozi":
				text6 = text19;
				break;
			case "bozi_1":
				text7 = text19;
				break;
			case "pifeng":
				text8 = text19;
				break;
			case "shou":
				text9 = text19;
				break;
			case "shouchi":
				text10 = text19;
				break;
			case "maozi":
				text11 = text19;
				break;
			case "jianjia":
				text12 = text19;
				break;
			case "mianju":
				text13 = text19;
				break;
			case "beishi":
				text14 = text19;
				break;
			case "yanjing":
				text15 = text19;
				break;
			case "erduo":
				text16 = text19;
				break;
			case "erduo2":
				text17 = text19;
				break;
			case "weiba":
				text18 = text19;
				break;
			case "shipin":
				if (!string.IsNullOrEmpty(text19))
				{
					try
					{
						skin.AddSkin(GetSkin(text19));
					}
					catch
					{
						Debug.LogError("添加皮肤报错:" + text19);
					}
				}
				break;
			default:
				Debug.LogError(array[0] + "这是什么部位？");
				break;
			}
		}
		bool flag = relicConfig8 != null || relicConfig7 != null || relicConfig6 != null;
		if (relicConfig != null)
		{
			text6 = ((!flag) ? (playerLook switch
			{
				PlayerLook.PrettyGril => "bozi/bozi2", 
				PlayerLook.TVMan => "bozi/bozi3", 
				PlayerLook.Nvliu => "bozi/bozi2", 
				PlayerLook.Frog => "bozi/bozi4", 
				PlayerLook.SnowMan => "bozi/bozi7", 
				PlayerLook.Horse => "bozi/bozi8", 
				_ => "bozi/bozi1", 
			}) : ((relicConfig8 == null) ? "bozi/bozi1" : (inBattle ? "bozi/bozi6" : "bozi/bozi5")));
		}
		Bone bone = skeleton.FindBone("bozi");
		if (relicConfig != null && !ignoreDisableRelicSkin && !DataMgr.settingData.DisableRelicSkins.Contains(50))
		{
			bone.SetLocalPosition(Originalbozi1LocalPoint + new Vector2(relicConfig.float1.result, 0f));
		}
		else
		{
			bone.SetLocalPosition(Originalbozi1LocalPoint);
		}
		if (relicConfig2 != null)
		{
			text7 = ((!flag) ? (playerLook switch
			{
				PlayerLook.PrettyGril => "bozi/bozi2_1", 
				PlayerLook.TVMan => "bozi/bozi3_1", 
				PlayerLook.Nvliu => "bozi/bozi2_1", 
				PlayerLook.Frog => "bozi/bozi4_1", 
				PlayerLook.SnowMan => "bozi/bozi7_1", 
				PlayerLook.Horse => "bozi/bozi8_1", 
				_ => "bozi/bozi1_1", 
			}) : ((relicConfig8 == null) ? "bozi/bozi1_1" : (inBattle ? "bozi/bozi6_1" : "bozi/bozi5_1")));
		}
		Bone bone2 = skeleton.FindBone("bozi2");
		if (relicConfig2 != null && !ignoreDisableRelicSkin && !DataMgr.settingData.DisableRelicSkins.Contains(61))
		{
			bone2.SetLocalPosition(Originalbozi2LocalPoint + new Vector2(0f, relicConfig2.float1.result));
		}
		else
		{
			bone2.SetLocalPosition(Originalbozi2LocalPoint);
		}
		if (relicConfig3 != null)
		{
			switch (text2)
			{
			case "toufa/toufa1":
			case "toufa/toufa4":
			case "toufa/toufa7":
			case "toufa/toufa9":
			case "toufa/toufa3":
			case "toufa/toufa10":
			case "toufa/toufa5":
			case "toufa/toufa6":
			case "toufa/toufa11":
			case "toufa/toufa13":
			case "toufa/toufa14_tap":
			case "toufa/toufa15_haoyou":
			case "toufa/toufa16_mao":
			case "toufa/toufa17":
			case "toufa/toufa18_xueren":
			case "toufa/toufa21_xia_nan":
			case "toufa/toufa20_xia_nv":
			case "toufa/toufa19_ma":
				text2 = "";
				break;
			}
		}
		if (relicConfig4 != null)
		{
			switch (text11)
			{
			case "maozi/maozi2":
			case "maozi/maozi5":
			case "maozi/maozi7":
			case "maozi/maozi8":
			case "maozi/maozi10_xia_nv":
				switch (text2)
				{
				case "toufa/toufa8":
					text2 = "toufa/toufa8_2";
					break;
				case "toufa/toufa8_tv":
					text2 = "toufa/toufa8_2_tv";
					break;
				case "toufa/toufa8_lfq":
					text2 = "toufa/toufa8_2_lfq";
					break;
				case "toufa/toufa8_qw":
					text2 = "toufa/toufa8_2_qw";
					break;
				case "toufa/toufa8_dave":
					text2 = "toufa/toufa8_2_dave";
					break;
				}
				break;
			default:
				if (relicConfig8 != null)
				{
					text7 = (inBattle ? "bozi/bozi6_1" : "bozi/bozi5_1");
				}
				break;
			}
		}
		Bone bone3 = skeleton.FindBone("zhuan1");
		Bone bone4 = skeleton.FindBone("zhuan2");
		if (relicConfig5 != null)
		{
			bone3.Rotation = Originalzhuan1Rotation - 60f;
			bone4.Rotation = Originalzhuan2Rotation + 60f;
		}
		else
		{
			bone3.Rotation = Originalzhuan1Rotation;
			bone4.Rotation = Originalzhuan2Rotation;
		}
		if (!string.IsNullOrEmpty(text))
		{
			skin.AddSkin(GetSkin(text));
		}
		if (!string.IsNullOrEmpty(text2))
		{
			skin.AddSkin(GetSkin(text2));
		}
		if (!string.IsNullOrEmpty(text6))
		{
			skin.AddSkin(GetSkin(text6));
		}
		if (!string.IsNullOrEmpty(text7))
		{
			skin.AddSkin(GetSkin(text7));
		}
		if (!string.IsNullOrEmpty(text5))
		{
			skin.AddSkin(GetSkin(text5));
		}
		if (!string.IsNullOrEmpty(text3))
		{
			skin.AddSkin(GetSkin(text3));
		}
		if (!string.IsNullOrEmpty(text4))
		{
			skin.AddSkin(GetSkin(text4));
		}
		if (!string.IsNullOrEmpty(text8))
		{
			skin.AddSkin(GetSkin(text8));
		}
		if (!string.IsNullOrEmpty(text9))
		{
			skin.AddSkin(GetSkin(text9));
		}
		if (!string.IsNullOrEmpty(text10))
		{
			skin.AddSkin(GetSkin(text10));
		}
		if (!string.IsNullOrEmpty(text11))
		{
			skin.AddSkin(GetSkin(text11));
		}
		if (!string.IsNullOrEmpty(text12))
		{
			skin.AddSkin(GetSkin(text12));
		}
		if (!string.IsNullOrEmpty(text13))
		{
			skin.AddSkin(GetSkin(text13));
		}
		if (!string.IsNullOrEmpty(text14))
		{
			skin.AddSkin(GetSkin(text14));
		}
		if (!string.IsNullOrEmpty(text15))
		{
			skin.AddSkin(GetSkin(text15));
		}
		if (!string.IsNullOrEmpty(text16))
		{
			skin.AddSkin(GetSkin(text16));
		}
		if (!string.IsNullOrEmpty(text17))
		{
			skin.AddSkin(GetSkin(text17));
		}
		if (!string.IsNullOrEmpty(text18))
		{
			skin.AddSkin(GetSkin(text18));
		}
		skeleton.SetSkin(skin);
		skeleton.SetSlotsToSetupPose();
	}

	public void SetSkinButBuild(Skeleton skeleton, FinishGameBuild build, GameObject portrait, bool _ignoreDisableRelicSkin)
	{
		SetSkin(skeleton, build.playerLook, build.relicCfgs, _ignoreDisableRelicSkin, DataMgr.selectedWorldData.inBattle9);
		portrait.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
		ProgressGraphicSkeleton(skeleton, DataMgr.selectedWorldData.selectedSetID);
		RelicConfig relicConfig = null;
		for (int i = 0; i < build.relicCfgs.Count; i++)
		{
			if (build.relicCfgs[i].abilityType == RelicAbilityType.ShowUnitHP)
			{
				relicConfig = build.relicCfgs[i];
			}
		}
		Bone bone = skeleton.FindBone("bozi2");
		if (relicConfig != null && !DataMgr.settingData.DisableRelicSkins.Contains(61))
		{
			bone.SetLocalPosition(Originalbozi2LocalPoint + new Vector2(0f, relicConfig.float1.result));
			switch (relicConfig.level)
			{
			case 1:
				portrait.GetComponent<RectTransform>().localPosition = new Vector3(-30f, 0f, 0f);
				break;
			case 2:
				portrait.GetComponent<RectTransform>().localPosition = new Vector3(-86f, 0f, 0f);
				break;
			case 3:
				portrait.GetComponent<RectTransform>().localPosition = new Vector3(-100f, 0f, 0f);
				break;
			default:
				Debug.LogError("未知等级" + relicConfig.level);
				break;
			}
		}
		else
		{
			bone.SetLocalPosition(Originalbozi2LocalPoint);
		}
	}

	private void ProgressGraphicSkeleton(Skeleton skeleton, int id)
	{
		if (id == 5)
		{
			skeleton.FindSlot("tui_l").A = 0f;
			skeleton.FindSlot("tui_r").A = 0f;
			skeleton.FindSlot("xiaotui_l").A = 0f;
			skeleton.FindSlot("xiaotui_r").A = 0f;
			skeleton.FindSlot("xie_l").A = 0f;
			skeleton.FindSlot("xie_r").A = 0f;
			skeleton.FindSlot("bilibili_xie_l").A = 0f;
			skeleton.FindSlot("bilibili_xie_r").A = 0f;
			skeleton.FindSlot("Hand_L").A = 0f;
			skeleton.FindSlot("Hand_R").A = 0f;
			skeleton.FindSlot("dave_tui_l").A = 0f;
			skeleton.FindSlot("dave_tui_r").A = 0f;
			skeleton.FindSlot("dave_xiaotui_l").A = 0f;
			skeleton.FindSlot("dave_xiaotui_r").A = 0f;
			skeleton.FindSlot("dave_xie_l").A = 0f;
			skeleton.FindSlot("dave_xie_r").A = 0f;
		}
	}

	public static bool IsCanHideRelic(int id)
	{
		RelicConfig relicConfig = RelicConfig.dic[id];
		RelicAbilityType abilityType = relicConfig.abilityType;
		if (abilityType == RelicAbilityType.AddMoveSpeed || abilityType == RelicAbilityType.Fly || abilityType == RelicAbilityType.RainbowRibbon || abilityType == RelicAbilityType.LongNeck || abilityType == RelicAbilityType.ShowUnitHP || abilityType == RelicAbilityType.PowerfulMan || abilityType == RelicAbilityType.PickMoreRelic || abilityType == RelicAbilityType.WarmSnow || abilityType == RelicAbilityType.Reaper)
		{
			return true;
		}
		abilityType = relicConfig.abilityType;
		if (abilityType == RelicAbilityType.MirrorOfSoul || abilityType == RelicAbilityType.Hunag)
		{
			return false;
		}
		return !string.IsNullOrEmpty(relicConfig.skinName);
	}
}
