using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class SEMgr : MonoBehaviour
{
	private Dictionary<string, AudioSource[]> ses = new Dictionary<string, AudioSource[]>();

	[HideInInspector]
	public Dictionary<string, AudioSource> loopSEs = new Dictionary<string, AudioSource>();

	[HideInInspector]
	public Dictionary<string, float> loopSEDurations = new Dictionary<string, float>();

	[HideInInspector]
	public List<GameObject> seObjs = new List<GameObject>();

	private static int asLimit = 3;

	public AudioSource dummy3dSE;

	public readonly string[] bossDeadShow = new string[3] { "SE_Dead_Flesh", "SE_Dead_Flesh2", "SE_Dead_Flesh3" };

	public readonly string bossFinish = "SE_BossFinish";

	public readonly string bossShow = "SE_BossShow";

	public readonly string chestFlyLand = "SE_ChestFlyLand";

	public readonly string chestOpen = "SE_ChestOpen";

	public readonly string curseInjuredRandomPoint = "SE_Curse_InjuredRandomPoint";

	public readonly string curse_Recall = "SE_Curse_Recall";

	public readonly string dead_Tree = "SE_Dead_Tree";

	public readonly string dead_Rock = "SE_Dead_Rock";

	public readonly string[] dead_Bone = new string[2] { "SE_Dead_Bone", "SE_Dead_Bone2" };

	public readonly string dialogueAppearHard = "SE_DialogueAppearHard";

	public readonly string dialogueAppearMiddle = "SE_DialogueAppearMiddle";

	public readonly string endlessStoreRefresh = "SE_EndlessStoreRefresh";

	public readonly string endlessStoreLock = "SE_EndlessStoreLock";

	public readonly string guide_DoorOpen = "SE_Guide_DoorOpen";

	public readonly string itemDropBase = "SE_ItemDrop_Base";

	public readonly string itemDropCoin = "SE_ItemDrop_Coin";

	public readonly string itemDropPotion = "SE_ItemDrop_Potion";

	public readonly string itemPick_AncientBlood = "SE_ItemPick_AncientBlood";

	public readonly string itemPick_ChaosCore = "SE_ItemPick_ChaosCore";

	public readonly string itemPick_Coin = "SE_ItemPick_Coin";

	public readonly string itemPick_MagicCrystal = "SE_ItemPick_MagicCrystal";

	public readonly string fallinAbyss = "SE_FallinAbyss";

	public readonly string injured_D3_T0 = "SE_Injured_D3_T0";

	public readonly string injured_D3_T3 = "SE_Injured_D3_T3";

	public readonly string injured_Frog = "SE_Injured_Frog";

	public readonly string injured_Horse = "SE_Injured_Horse";

	public readonly string injured_Huang = "SE_Injured_Huang";

	public readonly string injured_Player = "SE_Injured_Player";

	public readonly string[] injured_Dave = new string[3] { "SE_Injured_Dave1", "SE_Injured_Dave2", "SE_Injured_Dave3" };

	public readonly string levelRewardDestroy = "SE_LevelRewardDestroy";

	public readonly string monsterBorn = "SE_MonsterBorn";

	public readonly string monster996Born = "SE_Monster996Born";

	public readonly string npcCage_Open = "SE_NPCCage_Open";

	public readonly string openDoor_T0 = "SE_OpenDoor_T0";

	public readonly string openDoor_T8 = "SE_OpenDoor_T8";

	public readonly string pickItem = "SE_PickItem";

	public readonly string playerDead = "SE_PlayerDead";

	public readonly string playerDead_Frog = "SE_PlayerDead_Frog";

	public readonly string playerDead_Girl = "SE_PlayerDead_Girl";

	public readonly string playerDead_Huang = "SE_PlayerDead_Huang";

	public readonly string playerDead_Dave = "SE_PlayerDead_Dave";

	public readonly string playerDead_Horse = "SE_PlayerDead_Horse";

	public readonly string playerFootStep = "SE_PlayerFootStep";

	public readonly string playerFootStepDaveSwim = "SE_PlayerFootStepDaveSwim";

	public readonly string playerThroughT6 = "SE_PlayerThroughT6";

	public readonly string potionUseFinish = "SE_PotionUseFinish";

	public readonly string puzzleClick = "SE_PuzzleClick";

	public readonly string puzzleSucceed = "SE_PuzzleSucceed";

	public readonly string puzzleFail = "SE_PuzzleFail";

	public readonly string relic_Huang_BigSit = "SE_Relic_Huang_BigSit";

	public readonly string relic_Huang_BigSitHit = "SE_Relic_Huang_BigSitHit";

	public readonly string relic_Huang_JumpStart = "SE_Relic_Huang_JumpStart";

	public readonly string relic_FollowMissleHit = "SE_Relic_FollowMissleHit";

	public readonly string relic_InjuredAddMoveSpeed = "SE_Relic_InjuredAddMoveSpeed";

	public readonly string relic_Reroll = "SE_Relic_Reroll";

	public readonly string relic_SaintSwordHit = "SE_Relic_SaintSwordHit";

	public readonly string relic_SummonsExplode = "SE_Relic_SummonsExplode";

	public readonly string so210Blink = "SE_SO210Blink";

	public readonly string so3 = "SE_SO3";

	public readonly string so6 = "SE_SO6";

	public readonly string so6Loop = "SE_SO6Loop";

	public readonly string so17Drink = "SE_SO17Drink";

	public readonly string so2101_Recycle = "SE_SO2101Recycle";

	public readonly string so101_Compound = "SE_SO101Compound";

	public readonly string so101_Reroll = "SE_SO101Reroll";

	public readonly string so101_RerollEF = "SE_SO101RerollEF";

	public readonly string so101_RerollBroken = "SE_SO101RerollBroken";

	public readonly string so101_RerollEF_Holloween = "SE_SO101RerollEF_Holloween";

	public readonly string so101_Blend = "SE_SO101Blend";

	public readonly string so217rewardFly = "SE_217RewardFly";

	public readonly string so219Click = "SE_SO219Click";

	public readonly string so219FinishLIne = "SE_SO219FinishLine";

	public readonly string so219Start = "SE_SO219Start";

	public readonly string so219Cancle = "SE_SO219Cancle";

	public readonly string so220Slide = "SE_SO220Slide";

	public readonly string so223BoardMove = "SE_SO223BoardMove";

	public readonly string so29Dead = "SE_SO29Dead";

	public readonly string so29Hit = "SE_SO29Hit";

	public readonly string so222GetReward = "SE_SO217Bell";

	public readonly string so222Shoot = "SE_SO222Shoot";

	public readonly string spell1001Shoot = "SE_Spell1001Shoot";

	public readonly string spell1001Hit = "SE_Spell1001Hit";

	public readonly string spell1004Shoot = "SE_Spell1004Shoot";

	public readonly string spell1011Shoot = "SE_Spell1011Shoot";

	public readonly string spell1011Loop = "SE_Spell1011Loop";

	public readonly string spell1011End = "SE_Spell1011End";

	public readonly string spell1012Explosion = "SE_Spell1012Explosion";

	public readonly string spell1016Hit = "SE_Spell1016Hit";

	public readonly string spell1016Loop = "SE_Spell1016Loop";

	public readonly string spell1016Teleport = "SE_Spell1016Teleport";

	public readonly string spell3007Hit = "SE_Spell3007Hit";

	public readonly string spell3101Hit = "SE_Spell3101Hit";

	public readonly string spell3121Energy = "SE_Spell3121Energy";

	public readonly string spell31161Teleport = "SE_Spell3116Teleport";

	public readonly string spell3129Charge = "SE_Spell3129Charge";

	public readonly string spell3129Explosion = "SE_Spell3129VoidExplosion";

	public readonly string spell4004Finish = "SE_Spell4004Finish";

	public readonly string spell4004Loop = "SE_Spell4004Loop";

	public readonly string spell4004Shoot = "SE_Spell4004Shoot";

	public readonly string spell4004Success = "SE_Spell4004Success";

	public readonly string spell4012UmbrellaSpawn = "SE_UmbrellaSpawn";

	public readonly string spell4012UmbrellaBreak = "SE_UmbrellaBreak";

	public readonly string spell4012UmbrellaHit = "SE_UmbrellaHit";

	public readonly string spell4019PullReady = "SE_Spell4019PullReady";

	public readonly string[] spell9002Land = new string[3] { "SE_Spell9002Land1", "SE_Spell9002Land2", "SE_Spell9002Land3" };

	public readonly string spell9009Hit = "SE_Spell1021Hit";

	public readonly string spell9011Hit = "SE_Spell9011Hit";

	public readonly string spell9046Hit = "SE_Spell9046Hit";

	public readonly string sprint = "SE_Sprint";

	public readonly string storeRefresh = "SE_StoreRefresh";

	public readonly string uiActivateGirl_Activate = "SE_UIActivateGirl_Activate";

	public readonly string uiActivateGirl_SlotEnter = "SE_UIActivateGirl_SlotEnter";

	public readonly string uiActivateGirl_UnlockLine = "SE_UIActivateGirl_UnlockLine";

	public readonly string uiButtonHover_Button = "SE_UIButtonHover_Button";

	public readonly string uiButtonHover_Dice = "SE_UIButtonHover_Dice";

	public readonly string uiChangeLabel = "SE_UIChangeLabel";

	public readonly string uiChangeLabelClose = "SE_UIChangeLabelClose";

	public readonly string uiClick = "SE_UIClick";

	public readonly string uiCurseFly = "SE_UICurseFly";

	public readonly string uiCurseFlyFinish = "SE_UICurseFlyFinish";

	public readonly string uiFly = "SE_UIFly";

	public readonly string uiSpellFlyEnd = "SE_UISpellFlyEnd";

	public readonly string uiLevelRewardPickSpell = "SE_UILevelRewardPickSpell";

	public readonly string uiButtonSwitch = "SE_UIButtonSwitch";

	public readonly string uiClose = "SE_UIClose";

	public readonly string uiOpen = "SE_UIOpen";

	public readonly string uiResearchHover = "SE_UIResearchHover";

	public readonly string uiResearchWrong = "SE_UIResearchWrong";

	public readonly string uiRelic_Huang_Cooldown = "SE_UIRelic_Huang_Cooldown";

	public readonly string uiRelic_LightArmor_Cooldown = "SE_UIRelic_LightArmor_Cooldown";

	public readonly string uiRewardRelicHover = "SE_UIRewardRelicHover";

	public readonly string uiRewardSpellLockBtnHover = "SE_UIRewardSpellLockBtnHover";

	public readonly string uiRewardSpellChain = "SE_UIRewardSpellChain";

	public readonly string uiSlotPut = "SE_UISlotPut";

	public readonly string uiSpellDisable_Succeed = "SE_UISpellDisable_Succeed";

	public readonly string uiSwitch = "SE_UISwitch";

	public readonly string uiTalentUnlock = "SE_UITalentUnlock";

	public readonly string uiTalentReset = "SE_UITalentReset";

	public readonly string uiTalentUpgrade = "SE_UITalentUpgrade";

	public readonly string uiTrainingGetWand = "SE_UITrainingGetWand";

	public readonly string uiTrainingClearGround = "SE_UITrainingClearGround";

	public readonly string wandChange = "SE_WandChange";

	public readonly string[] teammate3Attack = new string[3] { "SE_Teammate3Attack1", "SE_Teammate3Attack2", "SE_Teammate3Attack3" };

	public readonly string teammate4HeavyAttack = "SE_Teammate4HeavyAttack";

	public readonly string teammate2Stab = "SE_Teammate2Stab";

	public readonly string teammate2StabHit = "SE_Teammate2StabHit";

	public readonly string teammate2SafeStabHit = "SE_Teammate2SafeStabHit";

	public readonly string teammate6MeleeAttack = "SE_Teammate6_MeleeAttack";

	public readonly string teammate6RangeShoot = "SE_Teammate6_CannonShoot";

	public readonly string teammate6RangeLoad = "SE_Teammate6_Load";

	public readonly string teammate7Shoot = "SE_Teammate7_Shoot";

	public readonly string teammate7EssenceShoot = "SE_Teammate7_EssenceShoot";

	public readonly string teammate7Explosion = "SE_Teammate7_Explosion";

	public readonly string teammate7EssenceExplosion = "SE_Teammate7_EssenceExplosion";

	public readonly string UIGeneralMaterialSelectUp = "SE_Spell4020SelectUp";

	public readonly string UIGeneralMaterialSelectDown = "SE_Spell4020SelectDown";

	public readonly string UIBagOpenClose = "SE_BagOpenClose";

	public readonly string CancelAllTeammate = "SE_SummonCancel";

	public readonly string UIDamageRecordBoard = "SE_DamageRecordBoardToggle";

	public readonly string UIUnlockSystem = "SE_UIUnlockSystem";

	public readonly string monster4Land = "SE_Monster4Land";

	public readonly string monster4LandFire = "SE_Monster4LandFire";

	public readonly string monster4Recover = "SE_Monster4Recover";

	public readonly string monster4Split = "SE_Monster4Split";

	public readonly string monster6Land = "SE_Monster6_Land";

	public readonly string monster7Trace = "SE_Monster7_trace";

	public readonly string monster7Blink = "SE_Monster7_Blink";

	public readonly string monster8Explode = "SE_Monster8_Explode";

	public readonly string monster12Land = "SE_Monster12Land";

	public readonly string monster12Split = "SE_Monster12Split";

	public readonly string monster15Land = "SE_Monster15Land";

	public readonly string monster15LandHeavy = "SE_Monster15LandHeavy";

	public readonly string monster16Charge = "SE_Monster16_Charge";

	public readonly string monster16Explode = "SE_Monster16_Explode";

	public readonly string monster26BoneLand = "SE_Monster26_BoneLand";

	public readonly string monster26AttackBone = "SE_Monster26_AttackBone";

	public readonly string monster26BigLand = "SE_Monster26_BigLand";

	public readonly string monster26Jump = "SE_Monster26_Jump";

	public readonly string monster26BigJump = "SE_Monster26_BigJump";

	public readonly string monster30Attack = "SE_Monster30_Attack";

	public readonly string monster32SpeedUp = "SE_Monster32_SpeedUp";

	public readonly string monster33Die = "SE_Monster33_Die";

	public readonly string monster34Explosion = "SE_Monster34_Explosion";

	public readonly string monster35BreakEarth = "SE_Monster35_BreakEarth";

	public readonly string monster35Shoot = "SE_Monster35_Shoot";

	public readonly string monster35Spike = "SE_Monster35_Spike";

	public readonly string monster35UnderGround = "SE_Monster35_UnderGround";

	public readonly string monster36Attack = "SE_Monster36_Attack";

	public readonly string monster37_KnockWall = "SE_Monster37_KnockWall";

	public readonly string monster37_KnockUnit = "SE_Monster37_KnockUnit";

	public readonly string[] monster37_Step = new string[4] { "SE_Monster37_Step", "SE_Monster37_Step2", "SE_Monster37_Step3", "SE_Monster37_Step4" };

	public readonly string monster38Shoot = "SE_Monster38_Shoot";

	public readonly string monster38Recycle = "SE_Monster38_Recycle";

	public readonly string monster39Slash = "SE_Monster39_Slash";

	public readonly string monster39BladeShow = "SE_Monster39_BladeShow";

	public readonly string monster39Hit = "SE_Monster39_Hit";

	public readonly string monster41Stretch = "SE_Monster41_Stretch";

	public readonly string monster41Bite = "SE_Monster41_Bite";

	public readonly string monster42Land = "SE_Monster42_Land";

	public readonly string monster43Magic = "SE_Monster43_Magic";

	public readonly string monster43Attack = "SE_Monster43_Attack";

	public readonly string monster43Fly = "SE_Monster43_Fly";

	public readonly string monster44Appear = "SE_Monster44_Appear";

	public readonly string monster45BubbleBreak = "SE_Monster45_BubbleBreak";

	public readonly string monster49_Missile = "SE_Monster49_Missile";

	public readonly string monster49_Trample = "SE_Monster49_Trample";

	public readonly string monster50_Swim = "SE_Monster50_Swim";

	public readonly string monstrer50_Attack = "SE_Monster50_Attack";

	public readonly string monster51_Attack = "SE_Monster51_Attack";

	public readonly string monster51_SpearEnd = "SE_Monster51_SpearEnd";

	public readonly string monster51_Teleport = "SE_Monster51_Teleport";

	public readonly string[] monster51_Shout = new string[3] { "SE_Monster51_Shout", "SE_Monster51_Shout1", "SE_Monster51_Shout2" };

	public readonly string monster52_Attack = "SE_Monster52_Attack";

	public readonly string monster52_Hit = "SE_Monster52_Hit";

	public readonly string[] monster53_Shout = new string[3] { "SE_Monster53_Shout", "SE_Monster53_Shout2", "SE_Monster53_Shout3" };

	public readonly string[] monster53_ShoutBig = new string[3] { "SE_Monster53_ShoutBig", "SE_Monster53_ShoutBig2", "SE_Monster53_ShoutBig3" };

	public readonly string monster53_Portal = "SE_Monster53_Portal";

	public readonly string monster54_Laser = "SE_Monster54_Laser";

	public readonly string monster54_Attack = "SE_Monster54_Attack";

	public readonly string monster55_Attack = "SE_Monster55_Attack";

	public readonly string monster56_FakeDie = "SE_Monster56_FakeDie";

	public readonly string monster56_Reborn = "SE_Monster56_Reborn";

	public readonly string monster56_RebornFinish = "SE_Monster56_RebornFinish";

	public readonly string monster56_Shoot = "SE_Monster56_Shoot";

	public readonly string monster305_Attack = "SE_Monster305_Attack";

	public readonly string monster305_Hit = "SE_Monster305_Hit";

	public readonly string monster306_Shoot = "SE_Monster306_Shoot";

	public readonly string monster309_Cannon = "SE_Monster309_Cannon";

	public readonly string monster310_Drop = "SE_Monster310_Drop";

	public readonly string monster310_Jump = "SE_Monster310_Jump";

	public readonly string monster312_Teleport = "SE_Monster312_Teleport";

	public readonly string monster313_GroundFire = "SE_Monster313_GroundFire";

	public readonly string monster317_Attack = "SE_Monster317_Attack";

	public readonly string monster317_Hit = "SE_Monster317_Hit";

	public readonly string monster319_Shoot = "SE_Monster319_Shoot";

	public readonly string monster319_Hit = "SE_Monster319_Hit";

	public readonly string elite1Attack = "SE_Elite1_Attack";

	public readonly string elite1Web = "SE_Elite1_Web";

	public readonly string elite1WebDrop = "SE_Elite1_WebDrop";

	public readonly string elite2Gulu = "SE_Elite2_Gulu";

	public readonly string elite2Bounce = "SE_Elite2_Bounce";

	public readonly string elite2Spring = "SE_Elite2_Spring";

	public readonly string elite2SpringUp = "SE_Elite2_SpringUp";

	public readonly string elite4Squish = "SE_Elite4_Squish";

	public readonly string elite7Bounce = "SE_Elite7_Bounce";

	public readonly string elite7Hide = "SE_Elite7_Hide";

	public readonly string elite7Ground = "SE_Elite7_Ground";

	public readonly string elite7Worming = "SE_Elite7_Worming";

	public readonly string elite7Vomit = "SE_Elite7_Vomit";

	public readonly string elite7Jump = "SE_Elite7_Jump";

	public readonly string elite7Bubble = "SE_Elite7_Bubble";

	public readonly string elite7Drop = "SE_Elite7_Drop";

	public readonly string elite7VoidExplode = "SE_Elite7_VoidExplode";

	public readonly string elite8Attack = "SE_Elite8_Attack";

	public readonly string elite8Summon = "SE_Elite8_Summon";

	public readonly string elite8ChainHit = "SE_Elite8_ChainHit";

	public readonly string elite9Slash = "SE_Elite9_Slash";

	public readonly string elite9DoubleSlash = "SE_Elite9_DoubleSlash";

	public readonly string elite9Step = "SE_Elite9_Step";

	public readonly string elite9Vomit = "SE_Elite9_Vomit";

	public readonly string elite9BeforeSlash = "SE_Elite9_BeforeSlash";

	public readonly string elite9Shout = "SE_Elite9_Shout";

	public readonly string elite9Burn = "SE_Elite9_Burn";

	public readonly string elite9BladeHit = "SE_Elite10_BladeHit";

	public readonly string elite9Knock = "SE_Elite9_Knock";

	public readonly string[] elite9Move = new string[4] { "SE_Elite9_Move", "SE_Elite9_Move1", "SE_Elite9_Move2", "SE_Elite9_Move3" };

	public readonly string elite10WingFlap = "SE_Elite10_WingFlap";

	public readonly string elite10Missile = "SE_Elite10_Missile";

	public readonly string elite10Bomb = "SE_Elite10_Bomb";

	public readonly string elite10Dead = "SE_Elite10_Dead";

	public readonly string elite10Explosion = "SE_Elite10_Explosion";

	public readonly string elite10Scratch = "SE_Elite10_Scratch";

	public readonly string elite10DropSmall = "SE_Elite10_DropSmall";

	public readonly string elite10Shoot = "SE_Elite10_Shoot";

	public readonly string elite10Attack = "SE_Elite10_Attack";

	public readonly string elite10Charge = "SE_Elite10_Charge";

	public readonly string elite10Roar1 = "SE_Elite10_Roar1";

	public readonly string elite10Roar2 = "SE_Elite10_Roar2";

	public readonly string elite10Roar3 = "SE_Elite10_Roar3";

	public readonly string elite10Roar4 = "SE_Elite10_Roar4";

	public readonly string elite10Transform = "SE_Elite10_Transform";

	public readonly string elite10Transform2 = "SE_Elite10_Transform2";

	public readonly string elite10DodgeAttack = "SE_Elite10_DodgeAttack";

	public readonly string elite11BulletHit = "SE_Elite11_BulletHit";

	public readonly string elite11Shoot = "SE_Elite11_Shoot";

	public readonly string elite11ShootLow = "SE_Elite11_ShootLow";

	public readonly string elite11Split = "SE_Elite11_Split";

	public readonly string elite11ChildBorn = "SE_Elite11_ChildBorn";

	public readonly string elite11ChildHide = "SE_Elite11_ChildHide";

	public readonly string elite11ChildAttack = "SE_Elite11_ChildAttack";

	public readonly string elite11ChildAttack1 = "SE_Elite11_ChildAttack1";

	public readonly string[] elite11BeforeAttack = new string[3] { "SE_Elite11_BeforeAttack", "SE_Elite11_BeforeAttack2", "SE_Elite11_BeforeAttack3" };

	public readonly string elite11Dead = "SE_Elite11_Dead";

	public readonly string elite11Summon = "SE_Elite11_Summon";

	public readonly string elite11BigLaserCharge = "SE_Elite11_BigLaserCharge";

	public readonly string elite11BigLaserStop = "SE_Elite11_BigLaserStop";

	public readonly string elite11BigLaserLoop = "SE_Elite11_BigLaserLoop";

	public readonly string elite11LaserLoop = "SE_Elite11_LaserLoop";

	public readonly string elite11LaserEnd = "SE_Elite11_LaserEnd";

	public readonly string elite11Grow = "SE_Elite11_Grow";

	public readonly string elite12FallRock = "SE_Elite12_FallRock";

	public readonly string elite12GroundWave = "SE_Elite12_GroundWave";

	public readonly string elite12_RockLand = "SE_Elite12_RockLand";

	public readonly string elite12Fly = "SE_Elite12_Fly";

	public readonly string elite12FlyJump = "SE_Elite12_FlyJump";

	public readonly string elite12ShootMeteor = "SE_Elite12_ShootMeteor";

	public readonly string elite12MeteorHit = "SE_Elite12_MeteorHit";

	public readonly string elite12_DroneSummon = "SE_Elite12_DroneSummon";

	public readonly string elite12DroneCast = "SE_Elite12_DroneCast";

	public readonly string elite12DroneHit = "SE_Elite12_DroneHit";

	public readonly string elite12_1Roar1 = "SE_Elite12_1Roar1";

	public readonly string elite12_1Roar2 = "SE_Elite12_1Roar2";

	public readonly string elite12_1Roar3 = "SE_Elite12_1Roar3";

	public readonly string elite12_1Die = "SE_Elite12_1Die";

	public readonly string elite12_2Roar1 = "SE_Elite12_2Roar1";

	public readonly string elite12_2Roar2 = "SE_Elite12_2Roar2";

	public readonly string elite12_2Roar3 = "SE_Elite12_2Roar3";

	public readonly string elite12_2Die = "SE_Elite12_2Die";

	public readonly string elite12_1SwitchStage = "SE_Elite12_1SwitchStage";

	public readonly string elite12_2SwitchStage = "SE_Elite12_2SwitchStage";

	public readonly string elite12_DroneAttack = "SE_Elite12_DroneAttack";

	public readonly string elite13Hit = "SE_Elite13_Hit";

	public readonly string elite13Miss = "SE_Elite13_Miss";

	public readonly string elite13Shoot = "SE_Elite13_Shoot";

	public readonly string elite13Impact = "SE_Elite13_Impact";

	public readonly string[] elite13Lightning = new string[4] { "SE_Elite13_Lightning", "SE_Elite13_Lightning_1", "SE_Elite13_Lightning_2", "SE_Elite13_Lightning_3" };

	public readonly string elite13Ball = "SE_Elite13_Ball";

	public readonly string elite13Knock = "SE_Elite13_Knock";

	public readonly string elite13Sentinel = "SE_Elite13_Sentinel";

	public readonly string elite13SendDrone = "SE_Elite13_SendDrone";

	public readonly string elite13DashCharge = "SE_Elite13_DashCharge";

	public readonly string elite13DashStart = "SE_Elite13_DashStart";

	public readonly string elite13DashEnd = "SE_Elite13_DashEnd";

	public readonly string e1ite13DashBounce = "SE_Elite13_DashBounce";

	public readonly string elite13CallLightning = "SE_Elite13_CallLightning";

	public readonly string elite13StraightRelease = "SE_Elite13_StraightRelease";

	public readonly string elite13ThunderStorm = "SE_Elite13_ThunderStorm";

	public readonly string elite13Roar1 = "SE_Elite13_Roar1";

	public readonly string elite13Roar2 = "SE_Elite13_Roar2";

	public readonly string elite13Roar3 = "SE_Elite13_Roar3";

	public readonly string elite13Roar4 = "SE_Elite13_Roar4";

	public readonly string elite13Roar5 = "SE_Elite13_Roar5";

	public readonly string elite14Die = "SE_Elite14_Die";

	public readonly string elite14Slash = "SE_Elite14_Slash";

	public readonly string elite14SideSlashShout = "SE_Elite14_SideSlashShout";

	public readonly string elite14SlashDashShout = "SE_Elite14_SlashDashShout";

	public readonly string elite14ShadowHide = "SE_Elite14_ShadowHide";

	public readonly string elite14ShadowAttackShout = "SE_Elite14_ShadowAttackShout";

	public readonly string elite14ShadowAttackShow = "SE_Elite14_ShadowAttackShow";

	public readonly string elite14ShadowAttack = "SE_Elite14_ShadowAttack";

	public readonly string elite14ChildShoot = "SE_Elite14_ChildShoot";

	public readonly string elite14ChildDash = "SE_Elite14_ChildDash";

	public readonly string elite14ChildCannonShoot = "SE_Elite14_ChildCannonShoot";

	public readonly string elite14Buff = "SE_Elite14_Buff";

	public readonly string elite14BladeHit = "SE_Elite14_BladeHit";

	public readonly string elite15Action2SHoot = "SE_Elite15_Action2Shoot";

	public readonly string elite15Action3 = "SE_Elite15_Action3";

	public readonly string elite15BlinkUp = "SE_Elite15_BlinkUp";

	public readonly string elite15BlinkDown = "SE_Elite15_BlinkDown";

	public readonly string elite15BulletBounce = "SE_Elite15_BulletBounce";

	public readonly string elite15BulletHit = "SE_Elite15_BulletHit";

	public readonly string elite15BulletMiss = "SE_Elite15_BulletMiss";

	public readonly string elite15Charge = "SE_Elite15_Charge";

	public readonly string elite15ChargeShoot = "SE_Elite15_ChargeShoot";

	public readonly string elite15ChargeShootHit = "SE_Elite15_ChargeShootHit";

	public readonly string[] elite15Shout = new string[3] { "SE_Elite15_Shout1", "SE_Elite15_Shout2", "SE_Elite15_Shout3" };

	public readonly string elite15BigShout = "SE_Elite15_BigShout";

	public readonly string elite51Launch = "SE_Elite51_Missilelaunch";

	public readonly string elite51Move = "SE_Elite51_MissileMove";

	public readonly string elite51Explosion = "SE_Elite51_MissileExplosion";

	public readonly string elite51Lock = "SE_Elite51_MissileLock";

	public readonly string elite51DashCharge = "SE_Elite51_DashCharge";

	public readonly string elite53Shoot = "SE_Elite53_Shoot";

	public readonly string elite55Charge = "SE_Elite55_Charge";

	public readonly string elite55HexAttack = "SE_Elite55_HexAttack";

	public readonly string elite55CatchAttack = "SE_Elite55_CatchAttack";

	public readonly string elite56Shoot = "SE_Elite56_CannonShoot";

	public readonly string elite56Reload = "SE_Elite56_CannonReload";

	public readonly string elite56MissileExplosion = "SE_Elite56_MissileExplosion";

	public readonly string elite56MissileSplit = "SE_Elite56_MissileSplit";

	public readonly string elite56LaserLock = "SE_Elite56_Locking";

	public readonly string elite56CounterDown = "SE_Elite56_CounterDown";

	public readonly string elite56BombExplosion = "SE_Elite56_BombExplosion";

	public readonly string elite56BombLand = "SE_Elite56_BombLand";

	public readonly string elite56LockTarget = "SE_Elite56_TargetLock";

	public readonly string elite56PopWave = "SE_Elite56_PopWave";

	public readonly string elite57VMissileLaunch = "SE_Elite57VMissileLaunch";

	public readonly string elite57HMissileLaunch = "SE_Elite57HMissileShoot";

	public readonly string elite57MissileExplosion = "SE_Elite57MissileExplosion";

	public readonly string elite57VMissileExplosion = "SE_Elite57VMissileExplosion";

	public readonly string elite58Shoot = "SE_Elite58_Shoot";

	public readonly string elite58Explosion = "SE_Elite58_Explosion";

	public readonly string elite58MineExplosion = "SE_Elite58_MineExplosion";

	public readonly string elite58PillarLand = "SE_Elite58_PillarLand";

	public readonly string elite58MineLand = "SE_Elite58_MineSet";

	public readonly string elite58PopAlert = "SE_Elite58_PopAlert";

	public readonly string elite59Alert = "SE_Elite59_Alert";

	public readonly string elite59_Wave = "SE_Elite59_WaveExplosion";

	public readonly string[] endlessMonsterDead = new string[1] { "SE_Dead_EndlessMonster" };

	public readonly string endlessMonsterSummon = "SE_EndlessMonsterSummon";

	public readonly string eliteDieFinal = "SE_DeadFleshFinal";

	public readonly string boss1Hit = "SE_Boss1_Hit";

	public readonly string boss1Land = "SE_Boss1_Land";

	public readonly string boss2_GetUp = "SE_Boss2_GetUp";

	public readonly string boss2_GetDown = "SE_Boss2_GetDown";

	public readonly string boss2_Fall = "SE_Boss2_Fall";

	public readonly string boss4_CurseCharge = "SE_Boss4_CurseCharge";

	public readonly string[] boss4_Child1 = new string[4] { "SE_Boss4_Child1_1", "SE_Boss4_Child1_2", "SE_Boss4_Child1_3", "SE_Boss4_Child1_4" };

	public readonly string boss4_CurseShoot = "SE_Boss4_CurseShoot";

	public readonly string boss4_CurseHit = "SE_Boss4_CurseHit";

	public readonly string boss3Dead2 = "SE_Boss3_Dead2";

	public readonly string boss4_RotateLeech = "SE_Boss4_RotateLeech";

	public readonly string boss5_Wave = "SE_Boss5_Wave1";

	public readonly string boss5_WaveHit = "SE_Boss5_WaveHit";

	public readonly string boss5_BubbleHit = "SE_Boss5_BubbleHit";

	public readonly string boss5_BubbleShoot = "SE_Boss5_BubbleShoot";

	public readonly string boss5_GroundTentacle = "SE_Boss5_GroundTentacle";

	public readonly string[] boss5_Shout = new string[3] { "SE_Boss5_Shout", "SE_Boss5_Shout_1", "SE_Boss5_Shout_2" };

	public readonly string boss5_ThirdStage = "SE_Boss5_ThirdStage";

	public readonly string boss5_ShieldHit = "SE_Boss5_ShieldHit";

	public readonly string boss5_ShieldDrop = "SE_Boss5_ShieldDrop";

	public readonly string boss5_RuneTrigger = "SE_Boss5_RuneTrigger";

	public readonly string boss5_ShieldProtected = "SE_Boss5_ShieldProtected";

	public readonly string boss5_RuneShow = "SE_Boss5_RuneShow";

	public readonly string boss5_TornadoDie = "SE_Boss5_TornadoDie";

	public readonly string boss5_Rune = "SE_Boss5_Rune";

	public readonly string boss5_Portal = "SE_Boss5_Portal";

	public readonly string boss6_AttackMiss = "SE_Boss6_AttackMiss";

	public readonly string boss6_BulletBounce = "SE_Boss6_BulletBounce";

	public readonly string boss6_Cannon = "SE_Boss6_Cannon";

	public readonly string boss6_Cannon1 = "SE_Boss6_Cannon1";

	public readonly string boss6_ChildAttack = "SE_Boss6_ChildAttack";

	public readonly string boss6_ChildDead = "SE_Boss6_ChildDead";

	public readonly string boss6_Dead = "SE_Boss6_Dead";

	public readonly string boss6_Explode = "SE_Boss6_Explode";

	public readonly string boss6_Hide = "SE_Boss6_Hide";

	public readonly string boss6_KnockGround = "SE_Boss6_KnockGround";

	public readonly string boss6_Miss = "SE_Boss6_Miss";

	public readonly string boss6_Roar1 = "SE_Boss6_Roar1";

	public readonly string boss6_Roar2 = "SE_Boss6_Roar2";

	public readonly string boss6_Roar3 = "SE_Boss6_Roar3";

	public readonly string boss6_Show = "SE_Boss6_Show";

	public readonly string boss6_Stage2Show = "SE_Boss6_Stage2Show";

	public readonly string boss6_Stage2FreeShow = "SE_Boss6_Stage2FreeShow";

	public readonly string boss6_SwitchStage = "SE_Boss6_SwitchStage";

	public readonly string boss6_OutOfDirt = "SE_Boss6_OutofDirt";

	public readonly string boss6_SideRotateBulletShoot = "SE_Boss6_SideRotateBulletShoot";

	public readonly string boss6_BulletShoot = "SE_Boss6_BulletShoot";

	public readonly string boss6_SideBulletShoot = "SE_Boss6_SideBulletShoot";

	public readonly string boss6_ExplodeBulletSplit = "SE_Boss6_explodeBulletSplit";

	public readonly string boss6_ExplodeBulletShoot = "SE_Boss6_explodeBulletShoot";

	public readonly string boss6_ExplodeBulletCharge = "SE_Boss6_explodeBulletCharge";

	public readonly string boss9_Bubble = "SE_Boss9_Bubble";

	public readonly string boss9_tentacleAttack = "SE_Boss9_TentacleAttack";

	public readonly string boss9_tentacleWarning = "SE_Boss9_TentacleWarning";

	public readonly string boss9_Smash = "SE_Boss9_Smash";

	public readonly string boss9_WaterFlow = "SE_Boss9_WaterFlow";

	public readonly string boss9_BreathIn = "SE_Boss9_BreathIn";

	public readonly string boss9_BreathOut = "SE_Boss9_BreathOut";

	public readonly string boss10_UpperCutUp = "SE_Boss10_UpperCutUp";

	public readonly string boss10_UpperCutDown = "SE_Boss10_UpperCutDown";

	public readonly string boss10_TNTDown = "SE_Boss10_TNTDown";

	public readonly string boss10_TNTUp = "SE_Boss10_TNTUp";

	public readonly string boss10_CrushCharge = "SE_Boss10_CrushCharge";

	public readonly string boss10_CrushOnGround = "SE_Boss10_CrushOnGround";

	public readonly string boss10_Angry = "SE_Boss10_Angry";

	public readonly string boss10_HitWall = "SE_Boss10_HitWall";

	public readonly string boss10_RockDown = "SE_Boss10_RockDown";

	public readonly string boss10_FootStep = "SE_Boss10_FootStep";

	public readonly string boss10_Roar = "SE_Boss10_Roar";

	public readonly string boss10_Die = "SE_Boss10_Die";

	public readonly string boss10_Explosion = "SE_Boss10_Explosion";

	public readonly string boss10_Escape = "SE_Boss10_Escape";

	public readonly string boss10_StoneBullet = "SE_Boss10_StoneBullet";

	public readonly string[] boss10_FallRock = new string[2] { "SE_Boss10_FallRock1", "SE_Boss10_FallRock2" };

	public readonly string boss13CallSub = "SE_Boss13_CallSub";

	public readonly string boss13Dash = "SE_Boss13Dash";

	public readonly string boss13DashDown = "SE_Boss13DashDown";

	public readonly string boss13DashBig = "SE_Boss13DashBig";

	public readonly string[] boss13BigExplosion = new string[2] { "SE_Boss13Explosion_Big_01", "SE_Boss13Explosion_Big_02" };

	public readonly string[] boss13SmallExplosion = new string[3] { "SE_Boss13Explosion_Small_01", "SE_Boss13Explosion_Small_02", "SE_Boss13Explosion_Small_03" };

	public readonly string boss13FalculaCharge = "SE_Boss13FalculaCharge";

	public readonly string boss13FalculaAttack = "SE_Boss13FalculaAttack";

	public readonly string boss13FalculaReturn = "SE_Boss13FalculaReturn";

	public readonly string boss13MineShoot = "SE_Boss13MineShoot";

	public readonly string boss13MineFloat = "SE_Boss13MineFloat";

	public readonly string boss13MineTrigger = "SE_Boss13MineTrigger";

	public readonly string boss13MineBangBefore = "SE_Boss13MineTimeCountDown";

	public readonly string boss13Stage1Shoot = "SE_Boss13Stage1Shoot";

	public readonly string boss13Stage2Aim = "SE_Boss13Stage2FollowMissileAim";

	public readonly string boss13Stage2Shoot = "SE_Boss13Stage2FollowMissileShoot";

	public readonly string boss13MegaMissileShoot = "SE_Boss13ContinuousMissiles";

	public readonly string boss13Stage3AttackWarning = "SE_Boss13_Stage3AttackWarning";

	public readonly string boss13Stage3AttackWarningBig = "SE_Boss13_Stage3AttackWarningBig";

	public readonly string boss13Stage3DelayMissileShoot = "SE_Boss13Stage3DelayMissile";

	public readonly string boss13Stage3FollowMissileShoot = "SE_Boss13Stage3FollowMissile";

	public readonly string boss13Stage3BulletHit = "SE_Boss13Stage3BulletHit";

	public readonly string boss50CannonReload = "SE_Boss50_CannonReload";

	public readonly string boss50CannonShoot = "SE_Boss50_CannonShoot";

	public readonly string boss50CannonExplode = "SE_Boss50_CannonExplode";

	public readonly string boss50HiveMissile = "SE_Boss50_HiveMissile";

	public readonly string boss50ChargePrepare = "SE_Boss50_ChargePrepare";

	public readonly string boss50RapidShoot = "SE_Boss50_RapidShoot";

	public readonly string boss50StrafeShoot = "SE_Boss50_StrafeShoot";

	public readonly string boss50Mine = "SE_Boss50_Mine";

	public readonly string Boss51_BurstFire = "SE_Boss51_BurstFire";

	public readonly string boss51JumpGround = "SE_Boss51_JumpGround";

	public readonly string boss51JumpGroundFire = "SE_Boss51_JumpGroundFire";

	public readonly string boss51FlameThrowerStart = "SE_Boss51_FlameThrowerStart";

	public readonly string boss51Strafe = "SE_Boss51_Strafe";

	public readonly string boss51StrafePrepare = "SE_Boss51_StrafePrepare";

	public readonly string boss51LineAttack = "SE_Boss51_LineAttack";

	public readonly string boss51LineAttackSingle = "SE_Boss51_LineAttackSingle";

	public readonly string boss51Cannon = "SE_Boss51_Cannon";

	public readonly string boss51LavaDrop = "SE_Boss51_LavaDrop";

	public readonly string boss52HDroneShoot = "SE_Boss52_ShootHorizontalLaser";

	public readonly string boss52MarkPoint = "SE_Boss52_MarkPoint";

	public readonly string boss52ChargeBurst = "SE_Boss52_ChargeBurst";

	public readonly string boss52Charge = "SE_Boss52_Charge";

	public readonly string boss52DashCast = "SE_Boss52_DashCast";

	public readonly string boss52Sprint = "SE_Boss52Sprint";

	public readonly string boss52DashCharge = "SE_Boss52_DashCharge";

	public readonly string boss52HDroneCharge = "SE_Boss52_HorizontalDroneCharge";

	public readonly string boss54Directional = "SE_Boss54_Directional";

	public readonly string boss54Summon = "SE_Boss54_Summon";

	public readonly string boss54Straight = "SE_Boss54_Straight";

	public readonly string boss54BulletHit = "SE_Boss54_BulletHit";

	public readonly string boss54ChildDirectional = "SE_Boss54_ChildDirectional";

	public readonly string boss54ChildStraight = "SE_Boss54_ChildStraight";

	public readonly string boss55BulletShoot = "SE_Boss54_Straight";

	public readonly string boss55BulletShootSmall = "SE_Boss55_BulletShootSmall";

	public readonly string boss55CircleBulletShoot = "SE_Boss55_CircleBulletShoot";

	public readonly string boss55LaunchCurveSword = "SE_Boss55_LaunchCurveSword";

	public readonly string boss55SwordDash = "SE_Boss55_SwordDash";

	public readonly string boss55SwordDashWarning = "SE_Boss55_SwordDashWarning";

	public readonly string boss55SpinDash = "SE_Boss55_SpinDash";

	public readonly string boss55SwordHit = "SE_Elite9_BladeHit";

	public readonly string boss55SendSword = "SE_Boss55_SendSword";

	public readonly string boss55SideSlash = "SE_Boss55_SideSlash";

	public readonly string boss55SummonSword = "SE_Boss55_SummonSword";

	private bool haveLoopSEPlaying;

	private Dictionary<string, float> SEIntervalCounter = new Dictionary<string, float>();

	private List<float> shoundDistanceSqr = new List<float>();

	public static SEMgr Inst { get; private set; }

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		foreach (KeyValuePair<string, AudioSource[]> se in ses)
		{
			for (int i = 0; i < se.Value.Length; i++)
			{
				se.Value[i].volume = DataMgr.settingData.GetFinalSound();
			}
		}
		foreach (KeyValuePair<string, AudioSource> loopSE in loopSEs)
		{
			loopSE.Value.volume = DataMgr.settingData.GetFinalSound();
		}
	}

	private void Update()
	{
		if (haveLoopSEPlaying)
		{
			bool flag = false;
			string[] array = loopSEs.Keys.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (loopSEDurations[array[i]] > 0f)
				{
					flag = true;
					loopSEDurations[array[i]] -= Time.deltaTime;
					if (loopSEDurations[array[i]] <= 0f)
					{
						loopSEs[array[i]].Stop();
					}
				}
			}
			if (!flag)
			{
				haveLoopSEPlaying = false;
			}
		}
		UpdateSEPlayIntervalState();
	}

	private void FixedUpdate()
	{
		UpdateSEPlayIntervalState();
	}

	public void Initialize()
	{
		Inst = this;
		dummy3dSE = base.gameObject.AddComponent<AudioSource>();
	}

	public AudioSource GetFreeAS(AudioSource[] se)
	{
		float time = se[0].time;
		AudioSource result = se[0];
		for (int i = 0; i < se.Length; i++)
		{
			if (se[i].time > time)
			{
				time = se[i].time;
				result = se[i];
			}
			if (se[i].time == 0f)
			{
				return se[i];
			}
		}
		return result;
	}

	public AudioSource GetFree3DAS(AudioSource[] se, Vector3 newPosition)
	{
		float num = 10f;
		float num2 = 0.7f;
		float num3 = 0.3f;
		for (int i = 0; i < se.Length; i++)
		{
			if (!se[i].isPlaying)
			{
				return se[i];
			}
		}
		Vector3 playerPoint = PlayerMgr.Inst.PlayerPoint;
		float num4 = Vector3.Distance(playerPoint, newPosition);
		float num5 = (1f - Mathf.Clamp01(num4 / num)) * num2 + 1f * num3;
		float num6 = float.MaxValue;
		AudioSource result = null;
		foreach (AudioSource audioSource in se)
		{
			float num7 = Vector3.Distance(playerPoint, audioSource.transform.position);
			float num8 = 1f - Mathf.Clamp01(num7 / num);
			float num9 = ((audioSource.clip != null) ? (audioSource.time / audioSource.clip.length) : 1f);
			float num10 = 1f - num9;
			float num11 = num8 * num2 + num10 * num3 - num5;
			if (num11 < num6)
			{
				num6 = num11;
				result = audioSource;
			}
		}
		return result;
	}

	private void UpdateSEPlayIntervalState()
	{
		if (SEIntervalCounter.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, float> item in SEIntervalCounter.Reverse())
		{
			float num = item.Value - 0.02f;
			if (num <= 0f)
			{
				SEIntervalCounter.Remove(item.Key);
			}
			else
			{
				SEIntervalCounter[item.Key] = num;
			}
		}
	}

	public AudioSource PlaySE(string seName, SEPlayMode seMode = SEPlayMode.Replay, int maxASCount = 3, float SEplayMinInterval = 0.1f)
	{
		return FinalPlaySE(seName, seMode, maxASCount, SEplayMinInterval);
	}

	public AudioSource PlaySE(FixedString128Bytes seName, SEPlayMode seMode = SEPlayMode.Replay, int maxASCount = 3, float SEplayMinInterval = 0.1f)
	{
		return FinalPlaySE(seName.ToString(), seMode, maxASCount, SEplayMinInterval);
	}

	private AudioSource FinalPlaySE(string seName, SEPlayMode seMode, int maxASCount, float SEplayMinInterval)
	{
		if (seName == "")
		{
			return null;
		}
		bool flag = false;
		if (SEIntervalCounter.ContainsKey(seName))
		{
			flag = true;
		}
		else if (SEplayMinInterval > 0f)
		{
			SEIntervalCounter.Add(seName, SEplayMinInterval);
		}
		int num = asLimit;
		if (maxASCount != 0)
		{
			num = maxASCount;
		}
		if (ses.ContainsKey(seName))
		{
			AudioSource freeAS = GetFreeAS(ses[seName]);
			if (freeAS.spatialBlend != 0f)
			{
				freeAS.spatialBlend = 0f;
			}
			switch (seMode)
			{
			case SEPlayMode.Replay:
				if (!flag)
				{
					freeAS.Play();
				}
				break;
			case SEPlayMode.Unique:
				if (!flag && !freeAS.isPlaying)
				{
					freeAS.Play();
				}
				break;
			default:
				Debug.LogError(seMode);
				break;
			}
			return freeAS;
		}
		if (ABResources.LoadAsset<AudioClip>("Sounds/" + seName) == null)
		{
			Debug.LogError("目标音效不存在: " + seName);
			return null;
		}
		AudioSource[] array = new AudioSource[num];
		for (int i = 0; i < array.Length; i++)
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.volume = DataMgr.settingData.GetFinalSound();
			if (i == 0)
			{
				audioSource.clip = ABResources.LoadAsset<AudioClip>("Sounds/" + seName);
			}
			else
			{
				audioSource.clip = array[0].clip;
			}
			audioSource.Stop();
			array[i] = audioSource;
		}
		ses.Add(seName, array);
		AudioSource freeAS2 = GetFreeAS(array);
		freeAS2.pitch = 1f;
		freeAS2.Play();
		return freeAS2;
	}

	public AudioSource PlaySE(string seName, Vector3 position, SEPlayMode seMode = SEPlayMode.Replay, int maxASCount = 3, float SEplayMinInterval = 0f)
	{
		return FinalPlaySE(seName, position, seMode, maxASCount, SEplayMinInterval);
	}

	public AudioSource PlaySE(FixedString128Bytes seName, Vector3 position, SEPlayMode seMode = SEPlayMode.Replay, int maxASCount = 3, float SEplayMinInterval = 0f)
	{
		return FinalPlaySE(seName.ToString(), position, seMode, maxASCount, SEplayMinInterval);
	}

	private AudioSource FinalPlaySE(string seName, Vector3 position, SEPlayMode seMode, int maxASCount, float SEplayMinInterval)
	{
		if (seName == "")
		{
			return null;
		}
		bool flag = false;
		if (SEIntervalCounter.ContainsKey(seName))
		{
			flag = true;
		}
		else if (SEplayMinInterval > 0f)
		{
			SEIntervalCounter.Add(seName, SEplayMinInterval);
		}
		int num = asLimit;
		if (maxASCount != 0)
		{
			num = maxASCount;
		}
		if (ses.ContainsKey(seName))
		{
			AudioSource free3DAS = GetFree3DAS(ses[seName], position);
			if (free3DAS == null)
			{
				return dummy3dSE;
			}
			if (free3DAS.spatialBlend != 1f)
			{
				ChangeASTo3D(free3DAS);
			}
			free3DAS.transform.position = position;
			free3DAS.Stop();
			switch (seMode)
			{
			case SEPlayMode.Replay:
				if (!flag)
				{
					free3DAS.Play();
				}
				break;
			case SEPlayMode.Unique:
				if (!flag && !free3DAS.isPlaying)
				{
					free3DAS.Play();
				}
				break;
			default:
				Debug.LogError(seMode);
				break;
			}
			return free3DAS;
		}
		AudioSource[] array = new AudioSource[num];
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = new GameObject(seName);
			gameObject.transform.parent = base.transform;
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			seObjs.Add(gameObject);
			audioSource.volume = DataMgr.settingData.GetFinalSound();
			if (i == 0)
			{
				audioSource.clip = ABResources.LoadAsset<AudioClip>("Sounds/" + seName);
			}
			else
			{
				audioSource.clip = array[0].clip;
			}
			audioSource.Stop();
			array[i] = audioSource;
		}
		ses.Add(seName, array);
		AudioSource free3DAS2 = GetFree3DAS(array, position);
		if (free3DAS2 == null)
		{
			return dummy3dSE;
		}
		ChangeASTo3D(free3DAS2);
		free3DAS2.transform.position = position;
		free3DAS2.Stop();
		free3DAS2.Play();
		return free3DAS2;
	}

	public AudioSource PlayLoopSE(FixedString128Bytes seName, float time, float volume = 1f)
	{
		return PlayLoopSE(seName.ToString(), time, volume);
	}

	public AudioSource PlayLoopSE(string seName, float time, float volume = 1f)
	{
		if (seName == "")
		{
			return null;
		}
		if (time <= 0f)
		{
			Debug.LogError("time没有大于0");
			return null;
		}
		if (loopSEs.ContainsKey(seName))
		{
			if (loopSEDurations[seName] <= 0f)
			{
				loopSEs[seName].Play();
				loopSEDurations[seName] = time;
				haveLoopSEPlaying = true;
				return loopSEs[seName];
			}
			if (loopSEDurations[seName] < time)
			{
				loopSEDurations[seName] = time;
			}
			return loopSEs[seName];
		}
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.volume = DataMgr.settingData.GetFinalSound() * volume;
		audioSource.clip = ABResources.LoadAsset<AudioClip>("Sounds/" + seName);
		audioSource.loop = true;
		audioSource.Play();
		loopSEs.Add(seName, audioSource);
		if (!loopSEDurations.ContainsKey(seName))
		{
			loopSEDurations.Add(seName, time);
		}
		else
		{
			loopSEDurations[seName] = time;
		}
		haveLoopSEPlaying = true;
		return audioSource;
	}

	public void ClearAllPool()
	{
		AudioSource[] components = base.transform.GetComponents<AudioSource>();
		for (int num = components.Length - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(components[num]);
		}
		for (int i = 0; i < seObjs.Count; i++)
		{
			UnityEngine.Object.Destroy(seObjs[i]);
		}
		seObjs.Clear();
		ses.Clear();
		loopSEs.Clear();
		loopSEDurations.Clear();
	}

	public void ChangeASTo3D(AudioSource audioSource)
	{
		audioSource.spatialBlend = 1f;
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.spread = 180f;
		audioSource.minDistance = 3f;
		audioSource.maxDistance = 12f;
	}

	public void StopAllLoopSE()
	{
		haveLoopSEPlaying = false;
		foreach (KeyValuePair<string, AudioSource> loopSE in loopSEs)
		{
			loopSEDurations[loopSE.Key] = 0f;
			loopSE.Value.Stop();
		}
	}

	public void PauseAllLoopSE()
	{
		foreach (KeyValuePair<string, AudioSource> loopSE in loopSEs)
		{
			loopSE.Value.Pause();
		}
	}

	public void UnPauseAllLoopSE()
	{
		foreach (KeyValuePair<string, AudioSource> loopSE in loopSEs)
		{
			loopSE.Value.UnPause();
		}
	}
}
