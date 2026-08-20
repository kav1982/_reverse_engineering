public static class TextMgr
{
	public const string str_upgradeValueDelimiter = "➔";

	public const string str_hairSpace = "\u200a";

	public const string str_ZeroSpace = "\u200b";

	public static char[] charCantAtStart = new char[17]
	{
		'？', ',', '。', '！', '、', '，', '?', '!', '%', '"',
		'”', '〞', ')', '）', '.', '·', '：'
	};

	public static char[] charCantAtEnd = new char[6] { '+', '-', '×', '(', '（', '“' };

	public static char[] charCanEndCantCross = new char[1] { '.' };

	public const string str_Space = " ";

	public const string str_ChineseSpace = "\u3000";

	public const string str_DescPrefixSquareSpace = "◆\u00a0\u200a";

	public const string str_RelicGroupItemPrefix = "    ▸\u00a0\u200a";

	public const int storyFinishNormalBackCampNPC6Cough = 3901;

	public const int story1NPCScreaming = 900301;

	public const int npc7Appearance_Amaze = 900501;

	public const int npc7OpenFunction_PlayerAttention = 900601;

	public const int npc7OpenFunction_DontNearMe = 900602;

	public const int npc7OpenFunction_SaySorry = 900603;

	public const int UIMainMenuStartGame = 1000001;

	public const int UIMainMenuSetting = 1000002;

	public const int UIMainMenuCredits = 1000003;

	public const int UIMainMenuQuit = 1000004;

	public const int UIMainMenuQQGroup = 1000028;

	public const int UICampMirrorTitle = 1000029;

	public const int UICampSkip_SwitchTheme = 1004151;

	public const int UICampSkip_Default = 1004152;

	public const int UICampSkip_Halloween = 1004153;

	public const int UICampSkip_Spring = 1004154;

	public const int UICampSkip_Summer = 1004155;

	public const int UICampSkip_Christmas = 1004156;

	public const int UIMainMenuAddWishlist = 1000005;

	public const int UIMainMenuBugReport = 1000006;

	public const int UIArchiveConfirmTitle = 1000007;

	public const int UIArchive_Empty = 1000008;

	public const int UIArchive_Location = 1000009;

	public const int UIArchive_MagicCrystal = 1000010;

	public const int UIArchive_AncientBlood = 1000011;

	public const int UIArchive_Time = 1000012;

	public const int UIArchive_Hour = 1000013;

	public const int UIArchive_Minete = 1000014;

	public const int UIMainMenu_SkitTutorial = 1000015;

	public const int UIArchive_Gear = 1000016;

	public const int UIArchive_Seconds = 1000019;

	public const int UIArchive_ChaosCore = 1000020;

	public const int UIMainMenuQuitGame = 1000021;

	public const int UIArchiveDifficultyFinish = 1000022;

	public const int UIArchiveDifficultyFinishNone = 1000025;

	public const int UISettingVidioAndAudio = 1000139;

	public const int UISettingControl = 1000102;

	public const int UISettingOther = 1000138;

	public const int UISettingVideo = 1000141;

	public const int UISettingAudio = 1000143;

	public const int UISettingLanguage = 1000103;

	public const int UISettingResolution = 1000142;

	public const int UISettingFrameLimit = 1000176;

	public const int UISettingWindowsMode = 1000108;

	public const int UISettingMainvolume = 1000144;

	public const int UISettingMusic = 1000106;

	public const int UISettingSound = 1000107;

	public const int UISettingReset = 1000104;

	public const int UISettingOK = 1000105;

	public const int UISettingVsync = 1000109;

	public const int UISettingTextFloat = 1000111;

	public const int UISettingTextFloatDesc = 1000112;

	public const int UISettingScreenShockRatio = 1000113;

	public const int pressanykey = 1000148;

	public const int UISettingAISummonDes = 1000160;

	public const int HardwareCursor_Des = 1000234;

	public const int CursorSize_Des = 1000235;

	public const int CursorSize = 1000236;

	public const int HardwareCursor = 1000237;

	public const int UISettingChangeControlLongpress = 1000238;

	public const int UISettingOpen = 1000135;

	public const int UISettingClose = 1000136;

	public const int UISettingBattleUIShow = 1000149;

	public const int UISettingBattleUIInfo = 1000150;

	public const int UISettingUsingMouseKey = 1000126;

	public const int UISettingUsingPad = 1000127;

	public const int UISettingLongPressToUse = 1000241;

	public const int UISettingPressDown = 1000242;

	public const int UISettingChangeControlUP = 1000114;

	public const int UISettingChangeControlDown = 1000115;

	public const int UISettingChangeControlLeft = 1000116;

	public const int UISettingChangeControlRight = 1000117;

	public const int UISettingChangeControlInteract = 1000118;

	public const int UISettingChangeControlBag = 1000119;

	public const int UISettingChangeControlShoot = 1000120;

	public const int UISettingChangeControlSwitchWand = 1000121;

	public const int UISettingChangeControlEachWand = 1000122;

	public const int UISettingChangeControlUsePotion = 1000123;

	public const int UISettingChangeControlPotionUp = 1000145;

	public const int UISettingChangeControlPotionDown = 1000146;

	public const int UISettingChangeControlQuickRemove = 1000125;

	public const int UISettingChangeControlMoveDirection = 1000128;

	public const int UISettingChangeControlAimDirection = 1000129;

	public const int UISettingChangeControlWandup = 1000130;

	public const int UISettingChangeControlWandDown = 1000131;

	public const int UISettingChangeControlThrow = 1000132;

	public const int UISettingChangeControlMoveObj = 1000133;

	public const int UISettingChangeControlCancleBack = 1000134;

	public const int UISettingChangeControlOn = 1000135;

	public const int UISettingChangeControlOff = 1000136;

	public const int UISettingChangeControlSwitchPotion = 1000124;

	public const int UISettingChangeControMenue = 1000147;

	public const int UISettingShowMutatedNeck = 1000151;

	public const int UISettingShowMutatedNeckDesc = 1000152;

	public const int UISettingSpellTransparency = 1000153;

	public const int UISettingSpellTransparencyDes = 1000154;

	public const int UISettingControllerAimAssist = 1000155;

	public const int UISettingSafeMode = 1000156;

	public const int UISettingSafeModeDes = 1000157;

	public const int UISettingSummonTransparency = 1000158;

	public const int UISettingAISummon = 1000159;

	public const int UISettingTourMode = 1000161;

	public const int UISettingTourModeDes = 1000162;

	public const int UISettingSpellTransparencyIsZero = 1000163;

	public const int UISettingSummonTransparencyIsZero = 1000164;

	public const int UISettingKeySpace = 1000165;

	public const int UISettingKeyLMB = 1000166;

	public const int UISettingKeyRMB = 1000167;

	public const int UISettingChangeControlSprint = 1000168;

	public const int UISettingSummonTransparencyDes = 1000169;

	public const int UISettingDamageInfo = 1000170;

	public const int UISettingKillSummon = 1000171;

	public const int UISettingInFullScreenMode = 1000172;

	public const int UISettingInWindowsMode = 1000173;

	public const int UISettingInBordlessWindowsMode = 1000174;

	public const int UISettingframeUnlimited = 1000175;

	public const int UIMenuContinue = 1000201;

	public const int UIMenuBackMainMenu = 1000202;

	public const int UIMenuQuitGame = 1000203;

	public const int UIMenuBackCamp = 1000204;

	public const int UIMenuSureToMainMenu = 1000205;

	public const int UIMenuSureQuit = 1000206;

	public const int UIMenuSureToCamp = 1000207;

	public const int UIMenuConfirmYes = 1000208;

	public const int UIMenuConfirmNo = 1000209;

	public const int UIMenuSureToCampFromBattle = 1000210;

	public const int UIMenuSureToMainMenuFromBattle = 1000211;

	public const int UIMenuSystem = 1000221;

	public const int UIMenuDiction = 1000222;

	public const int UIMenuAchievement = 1000223;

	public const int UIMenuCharacter = 1000224;

	public const int UIMenue_TimeUse = 1000239;

	public const int UITrainingLongpress = 1000240;

	public const int UIEntryBag = 1000301;

	public const int UIEntryChooseLanguage = 1000302;

	public const int UIEntryKeyBoardRecommand = 1000303;

	public const int UIMenuCurrentLevel = 1000304;

	public const int UIEntryWarning1 = 1003001;

	public const int UIEntryWarning2 = 1003002;

	public const int UIEntrySpiderMode = 1003003;

	public const int UIGalleryMonster = 1000401;

	public const int UIGalleryBoss = 1000402;

	public const int UIGallerySpell = 1000403;

	public const int UIGalleryBlessing = 1000404;

	public const int UIGalleryPotion = 1000405;

	public const int UIGalleryCurse = 1000406;

	public const int UIGalleryHP = 1000407;

	public const int UIGalleryAppear = 1000408;

	public const int UIGalleryKilled = 1000409;

	public const int UIGalleryGetTime = 1000410;

	public const int UIGalleryKilledCount = 1000411;

	public const int UIGalleryUseTime = 1000412;

	public const int UIGallerySwitchSpellLevel = 1000413;

	public const int UIGalleryWand = 1000414;

	public const int UIGalleryMaxLevel = 1000415;

	public const int UIGalleryMaxCount = 1000416;

	public const int UIGalleryTitle = 1000417;

	public const int UITalentTitle = 1000501;

	public const int UITalentWandLimit = 1000502;

	public const int UITalentBagLimit = 1000503;

	public const int UITalentInitialCoin = 1000504;

	public const int UITalentMaxHP = 1000505;

	public const int UITalentSpellRoom = 1000506;

	public const int UITalentRelicRoom = 1000507;

	public const int UITalentCoinRoom = 1000508;

	public const int UITalentHPRoom = 1000509;

	public const int UITalentMaxMP = 1000510;

	public const int UITalentMPRecover = 1000511;

	public const int UITalentMax = 1000512;

	public const int UITalentSpellRoomEffect = 1000513;

	public const int UITalentRelicRoomEffect = 1000514;

	public const int UITalentCoinRoomEffect = 1000515;

	public const int UITalentHPRoomEffect = 1000516;

	public const int UITalentEnterDoorRecovery = 1000517;

	public const int UITalentUpperLimit = 1000518;

	public const int UITalentUnlockMore = 1000519;

	public const int UIEndlessTalentTitle = 1000550;

	public const int UIEndless_GoodsExtraCount_Name = 1000551;

	public const int UIEndless_GoodsExtraCount_Desc = 1000552;

	public const int UIEndless_SupplyBox_Name = 1000553;

	public const int UIEndless_SupplyBox_Desc = 1000554;

	public const int UIEndless_Gallery_Name = 1000555;

	public const int UIEndless_Gallery_Desc = 1000556;

	public const int UIEndless_FinishCoin_Name = 1000557;

	public const int UIEndless_FinishCoin_Desc = 1000558;

	public const int UIEndless_LockMachine_Name = 1000559;

	public const int UIEndless_LockMachine_Desc = 1000560;

	public const int UIEndless_HightLevelSpell_Name = 1000561;

	public const int UIEndless_HightLevelSpell_Desc = 1000562;

	public const int UIEndless_ProcessSpell_Name = 1000563;

	public const int UIEndless_ProcessSpell_Desc = 1000564;

	public const int UIEndless_MaxHP_Name = 1000565;

	public const int UIEndless_MaxHP_Desc = 1000566;

	public const int UIEndless_ExtraDamage_Name = 1000567;

	public const int UIEndless_ExtraDamage_Desc = 1000568;

	public const int UIEndless_UnlockMore = 1000569;

	public const int UICampMirror5WReward = 1000016;

	public const int UICampMirror10WReward = 1000017;

	public const int UICampMirrornoReward = 1000018;

	public const int UIBattleSummaryTitle = 1000601;

	public const int UIBattleSummaryConfirm = 1000602;

	public const int UIGamepadDragTip1 = 1000701;

	public const int UIGamepadDragTip2 = 1000702;

	public const int UIWandLackMana = 1000703;

	public const int UIOtherWandControlBySprit = 1000704;

	public const int UISlotWandLackMana = 1000705;

	public const int UISlotWandRightNoShootableSpell = 1000706;

	public const int UISlotWandLeftNoShootableSpell = 1000707;

	public const int UISlotWandRightNoSpellToMimic = 1000708;

	public const int UISlotWandNoSpaceToMimic = 1000709;

	public const int UIReward_SelectWand = 1000801;

	public const int UIReward_SelectSpell = 1000802;

	public const int UIReward_SelectRelic = 1000803;

	public const int UIReward_Reroll = 1000804;

	public const int UICompound_Compound = 1000901;

	public const int UICompound_Need3SameSpeed = 1000902;

	public const int UICompound_LevelMax = 1000903;

	public const int UIRecast_Reroll = 1000904;

	public const int UIMoreInOne_PutIn = 1000905;

	public const int UIMoreInOne_Blend = 1000906;

	public const int UIMoreInOne_LevelMax = 1000907;

	public const int UIMoreInOne_NeedSameLevelRarity = 1000908;

	public const int UIRecast_SpecialSpellCantReroll = 1000909;

	public const int UISell_Sell = 1000910;

	public const int UIReward_MobileDoubleClick = 1000911;

	public const int wandPostTriggerTypeKill = 1001001;

	public const int wandPostTriggerTypeMove = 1001002;

	public const int wandPostTriggerTypeHit = 1001003;

	public const int wandPostTriggerTypeStand = 1001004;

	public const int wandPostTriggerTypeCast = 1001005;

	public const int wandPostTriggerTypeHighDamage = 1001006;

	public const int wandPostTriggerTypeCriticalHit = 1001007;

	public const int wandPostTriggerTakeDamage = 1001008;

	public const int wandPostTriggerTime = 1001009;

	public const int SpellAttr_Seconds = 1001101;

	public const int SpellAttr_Count = 1001102;

	public const int SpellAttr_Round = 1001103;

	public const int uiBag_AutoFull = 1001201;

	public const int InteractiveInfo_SO10_Name = 1001301;

	public const int InteractiveInfo_SO10_Info = 1001302;

	public const int InteractiveInfo_SO101_1_Name = 1001303;

	public const int InteractiveInfo_SO101_1_Info = 1001304;

	public const int InteractiveInfo_SO101_2_Name = 1001305;

	public const int InteractiveInfo_SO101_2_Info = 1001306;

	public const int InteractiveInfo_SO101_3_Name = 1001307;

	public const int InteractiveInfo_SO101_3_Info = 1001308;

	public const int InteractiveInfo_NoHP = 1001309;

	public const int InteractiveInfo_NoCoin = 1001310;

	public const int InteractiveInfo_NoKey = 1001311;

	public const int InteractiveInfo_NoCurse = 1001312;

	public const int InteractiveInfo_Spring = 1001313;

	public const int InteractiveInfo_SpringDesc = 1001314;

	public const int InteractiveInfo_StoreRefresh = 1001315;

	public const int InteractiveInfo_StoreRefreshDesc = 1001316;

	public const int InteractiveInfo_RussianRoll = 1001317;

	public const int InteractiveInfo_RussianRollDesc = 1001318;

	public const int InteractiveInfo_SO10_NameH = 1001319;

	public const int InteractiveInfo_SO21Name = 1001320;

	public const int InteractiveInfo_SO21Desc = 1001321;

	public const int InteractiveInfo_SpringDave = 1001322;

	public const int InteractiveInfo_EndlessNextWave = 1001323;

	public const int InteractiveInfo_EndlessLockItem = 1001324;

	public const int InteractiveInfo_EndlessLockItemInfo = 1001325;

	public const int InteractiveInfo_EndlessNextWaveLevel0 = 1001326;

	public const int InteractiveInfo_EndlessFreeRefresh = 1001327;

	public const int InteractiveInfo_EndlessSpellSeller = 1001328;

	public const int InteractiveInfo_EndlessSpellSellerInfo = 1001329;

	public const int InteractiveInfo_EndlessCompound = 1001330;

	public const int InteractiveInfo_EndlessReroll = 1001331;

	public const int InteractTip2_StartBattle = 1001401;

	public const int InteractTip2_Spell = 1001402;

	public const int InteractTip2_Relic = 1001403;

	public const int InteractTip2_MaxHP = 1001404;

	public const int InteractTip2_Coin = 1001405;

	public const int InteractTip2_Store = 1001406;

	public const int InteractTip2_Process = 1001407;

	public const int InteractTip2_Elite = 1001408;

	public const int InteractTip2_Boss = 1001409;

	public const int InteractTip2_Chapter = 1001410;

	public const int InteractTip2_Spring = 1001411;

	public const int InteractTip2_Shortcut = 1001412;

	public const int InteractTip2_EndlessBattle = 1001413;

	public const int InteractTip2_EnterEndlessMode = 1001414;

	public const int Interact_Buy = 1001501;

	public const int Interact_Pickup = 1001502;

	public const int Interact_Enter = 1001503;

	public const int Interact_Use = 1001504;

	public const int Interact_Interact = 1001505;

	public const int Interact_Open = 1001506;

	public const int Interact_Talk = 1001507;

	public const int Interact_Lock = 1001508;

	public const int Interact_Unlock = 1001509;

	public const int Interact_ClickToHideRelicSkin = 1001510;

	public const int Interact_RelicSkinIsDisable_ClickToShow = 1001511;

	public const int ItemRariry_Common = 1001601;

	public const int ItemRariry_Rare = 1001602;

	public const int ItemRariry_Epic = 1001603;

	public const int ItemRariry_Special = 1001604;

	public const int PlaceName_Camp = 1001701;

	public const int PlaceName_Chapter1 = 1001702;

	public const int PlaceName_Chapter2 = 1001703;

	public const int PlaceName_Chapter3 = 1001704;

	public const int PlaceName_Chapter4 = 1001705;

	public const int PlaceName_Chapter5 = 1001706;

	public const int PlaceName_Endless = 1001707;

	public const int guideImage_Shoot = 1001801;

	public const int guideImage_Shoot_Mobile = 1001808;

	public const int guideImage_UsePotion = 1001802;

	public const int guideImage_OpenCloseBag = 1001803;

	public const int guideImage_Move = 1001804;

	public const int guideImage_SwitchWand = 1001805;

	public const int guideImage_OpenCloseBag_Mobile = 1001806;

	public const int guideImage_UsePotion_Mobile = 1001807;

	public const int unitAttr_MoveSpeed = 1001901;

	public const int unitAttr_MPRecovery = 1001902;

	public const int unitAttr_AllDamage = 1001903;

	public const int unitAttr_AllCriticle = 1001904;

	public const int battleState_Dodge = 1002001;

	public const int battleState_Immune = 1002002;

	public const int battleState_Invalid = 1002003;

	public const int battleState_UpgradeRelic = 1002004;

	public const int battleState_RelicGreedSeedExtraDesc = 1002005;

	public const int battleState_RelicMoneyIsPowerExtraDesc = 1002006;

	public const int battleState_AlreadyRemoved = 1002007;

	public const int battleState_RelicGreedSeedGrown = 1002008;

	public const int battleState_EndlessBottleNewPotion = 1002009;

	public const int battleState_Potion_RerollRelic_NoRelic = 1002010;

	public const int battleState_DeathAdderUpgrade = 1002011;

	public const int battleState_DeathAdderMaxLevel = 1002012;

	public const int battleState_DeathAdderKillCount = 1002013;

	public const int battleState_WandLackSlotAlert = 1002014;

	public const int battleState_Elite3Hiding = 1002015;

	public const int battleState_Elite5Capture = 1002016;

	public const int battleState_PotionPetrifaction = 1002017;

	public const int SpellAttr_EverySecondPlus = 1002018;

	public const int battleState_NoShootingSpellInTheWand = 1002019;

	public const int battleState_CantRemove = 1002020;

	public const int battleState_TageDamage = 1002021;

	public const int battleState_RelicLightArmorAdditionStr = 1002022;

	public const int battleState_SpellBiAnBladeCount = 1002023;

	public const int battleState_FieldAOESpell = 1002024;

	public const int battleState_RelicLightArmorKeyboardKey = 1002025;

	public const int battleState_RelicLightArmorGamepadKey = 1002026;

	public const int battleState_RelicLightArmorMobileKey = 1002027;

	public const int battleState_RelicSwordBackKeyboardKey = 1002028;

	public const int battleState_RelicSwordBackGamepadKey = 1002029;

	public const int battleState_RelicSwordBackMobileKey = 1002030;

	public const int battleState_RelicBigSitKeyboardKey = 1002031;

	public const int battleState_RelicBigSitGamepadKey = 1002032;

	public const int battleState_RelicBigSitMobileKey = 1002033;

	public const int battleState_RelicDaveKeyboardKey = 1002051;

	public const int battleState_RelicDaveGamepadKey = 1002052;

	public const int battleState_RelicDaveMobileKey = 1002053;

	public const int battleState_RelicLCRuneUnlockPoint = 1002054;

	public const int battleState_RelicLCRuneUnlockThreshold = 1002055;

	public const int battleState_RelicLCRedRuneL1 = 1002056;

	public const int battleState_RelicLCRedRuneL2 = 1002057;

	public const int battleState_RelicLCRedRuneL3 = 1002058;

	public const int battleState_RelicLCRedRuneL4 = 1002059;

	public const int battleState_RelicLCGreenRuneL1 = 1002060;

	public const int battleState_RelicLCGreenRuneL2 = 1002061;

	public const int battleState_RelicLCGreenRuneL3 = 1002062;

	public const int battleState_RelicLCGreenRuneL4 = 1002063;

	public const int battleState_RelicLCBlueRuneL1 = 1002064;

	public const int battleState_RelicLCBlueRuneL2 = 1002065;

	public const int battleState_RelicLCBlueRuneL3 = 1002066;

	public const int battleState_RelicLCBlueRuneL4 = 1002067;

	public const int battleState_CriticalChance = 1002068;

	public const int battleState_ScareCrowVenomSelfPurify = 1002034;

	public const int battleState_Teammate6DMGHpUpRatio = 1002035;

	public const int battleState_ManaTendrilManaGenRatio = 1002036;

	public const int battleState_DamageTypePassiveAdditionInfo = 1002037;

	public const int battleState_AllfieldEnhanceAlertText = 1002038;

	public const int battleState_CantRefrshOnChapter5 = 1002039;

	public const int battleState_AutoWandCannotActivelyCastSpells = 1002040;

	public const int battleState_BingSpellCastCount = 1002041;

	public const int battleState_LostRelic = 1002042;

	public const int battleState_MaxHPAlreadyAdd = 1002043;

	public const int battleState_RelicMedicineKitExtraDesc = 1002044;

	public const int battleState_Boss9Blind = 1002045;

	public const int battleState_RelicDaveHarpoonKeyboardKey = 1002046;

	public const int battleState_RelicDaveHarpoonGamepadKey = 1002047;

	public const int battleState_RelicDaveHarpoonMobileKey = 1002048;

	public const int battleState_RelicHighLevelDivingSuitExtraDescription = 1002049;

	public const int battleState_SecondBrandOnlyOnce = 1002050;

	public const int uiResearchTitle = 1002101;

	public const int uiSetTitle = 1002102;

	public const int uiSetUnlockCondition = 1002103;

	public const int uiSetUnlockNewOutfit = 1002104;

	public const int uiSetUpgrade = 1002105;

	public const int uiSetUnlock = 1002106;

	public const int uiResearchNew = 1002107;

	public const int uiSetCurrentKill = 1002108;

	public const int uiResearchIsActive = 1002109;

	public const int uiResearchDisactive = 1002110;

	public const int uiResearchChanger = 1002111;

	public const int uiSetCurrentDrink = 1002112;

	public const int Item_Resource = 1002201;

	public const int Item_Relic = 1002202;

	public const int Item_Spell = 1002203;

	public const int Item_Wand = 1002204;

	public const int Item_SpellMissle = 1002205;

	public const int Item_SpellSummon = 1002206;

	public const int Item_SpellEnhance = 1002207;

	public const int Item_SpellPassive = 1002208;

	public const int Item_Potion = 1002209;

	public const int Item_Curse = 1002210;

	public const int youdie = 1002301;

	public const int uiTrainning_GetSpell = 1002401;

	public const int uiTrainning_GetWand = 1002402;

	public const int uiTrainning_ClearGround = 1002403;

	public const int uiTrainning_LevelPlus = 1002404;

	public const int uiTrainning_LevelPlus2 = 1002405;

	public const int uiTrainning_Title = 1002406;

	public const int uiTrainning_HighestDPS = 1002408;

	public const int uiTrainning_HighestHit = 1002409;

	public const int uiTrainning_GetRelic = 1002410;

	public const int mixed_ComingSoon = 1002501;

	public const int mixed_UsePotionHoverOnGround = 1002502;

	public const int mixed_InsufficietHP = 1002503;

	public const int uiChapterThrough_Easy = 1002601;

	public const int uiChapterThrough_Normal = 1002602;

	public const int uiChapterThrough_Hard = 1002603;

	public const int uiChapterThrough_CllickAgaimStart = 1002604;

	public const int uiChapterThrough_Nightmare = 1002605;

	public const int uiChapterThrough_Nightmare2 = 1002606;

	public const int uiChapterThrough_Nightmare3 = 1002607;

	public const int uiChapterThrough_MonsterMoreHP = 1002611;

	public const int uiChapterThrough_MonsterMoreHP2 = 1002612;

	public const int uiChapterThrough_MonsterMoreHP3 = 1002613;

	public const int uiChapterThrough_OpenChapter = 1002614;

	public const int uiChapterThrough_Output = 1002615;

	public const int uiChapterThrough_MonstersMutate = 1002616;

	public const int uiChapterThrough_MonstersMutate2 = 1002617;

	public const int uiChapterThrough_EpicSpellLootAny = 1002618;

	public const int uiChapterThrough_RelicRoomEpicDrop = 1002619;

	public const int uiChapterThrough_RelicRoomEpicDrop2 = 1002620;

	public const int uiChapterThrough_SelectDifficulty = 1002621;

	public const int uiChapterThrough_Mode = 1002622;

	public const int levelPostString = 1006401;

	public const int getRelicGroupPrefix = 1006402;

	public const int numericUnit_e3_onlyEnglish = 1006410;

	public const int numericUnit_e4_onlyChinese = 1006411;

	public const int numericUnit_e6_onlyEnglish = 1006412;

	public const int numericUnit_e8_onlyChinese = 1006413;

	public const int numericUnit_e9_onlyEnglish = 1006414;

	public const int numericUnit_e12 = 1006415;

	public const int UnlockResearch = 1006601;

	public const int UnlockSet = 1006602;

	public const int UnlockTrainingRoom = 1006603;

	public const int UnlockActicateGirl = 1006604;

	public const int UnlockSpellDisable = 1006605;

	public const int SpellAttr_EnhanceDes = 1002701;

	public const int SpellAttr_PassiveDes = 1002702;

	public const int SpellAttr_EnhanceDesMissileOnly = 1002703;

	public const int SpellAttr_EnhanceDesSummonOnly = 1002704;

	public const int SpellAttr_CantUpgrade = 1002705;

	public const int SpellAttr_CantReroll = 1002706;

	public const int SpellAttr_unableToCombineOrUpgrade = 1002707;

	public const int SpellAttr_UniqueEffect = 1002709;

	public const int uiCampMirrorTextFlow_Boy1 = 1002801;

	public const int uiCampMirrorTextFlow_Boy2 = 1002802;

	public const int uiCampMirrorTextFlow_Boy3 = 1002803;

	public const int uiCampMirrorTextFlow_Boy4 = 1002804;

	public const int uiCampMirrorTextFlow_Boy5 = 1002805;

	public const int uiCampMirrorTextFlow_Girl1 = 1002806;

	public const int uiCampMirrorTextFlow_Girl2 = 1002807;

	public const int uiCampMirrorTextFlow_Girl3 = 1002808;

	public const int uiCampMirrorTextFlow_Girl4 = 1002809;

	public const int uiCampMirrorTextFlow_Girl5 = 1002810;

	public const int uiFinishNormalReward_Title = 1002901;

	public const int uiFinishNormalReward_Confirm = 1002902;

	public const int uiLihgtWarningTitle = 1003001;

	public const int uiLihgtWarningDesc = 1003002;

	public const int uiRankingList_MainTitle = 1003101;

	public const int uiRankingList_LocalRank = 1003102;

	public const int uiRankingList_SteamRank = 1003103;

	public const int uiRankingList_UserName = 1003104;

	public const int uiRankingList_Rank = 1003105;

	public const int uiRankingList_Score = 1003106;

	public const int uiRankingList_RecordTime = 1003107;

	public const int LeaderboardRecordTime = 1003111;

	public const int uiFinishBuilsSavePicture = 1003201;

	public const int uiFinishBuilsDeleteRecord = 1003202;

	public const int uiRankinglistDifficulty = 1003203;

	public const int uiFinishBuildBuildNotSupport = 1003207;

	public const int uiFinishBuildNOFriendGameRecordCount = 1003208;

	public const int uiRankinglist_SteamNotConnected = 1003209;

	public const int uiRankinglist_Loading = 1003210;

	public const int uiRankinglist_Retry = 1003211;

	public const int uiRankinglist_CheckConnect = 1003212;

	public const int uiRankinglist_NoLoalRecords = 1003213;

	public const int uiActivateGirl_Title = 1003301;

	public const int uiActivateGirl_HoldToActivate = 1003302;

	public const int uiActivateGirl_FoundSpellDuringAdventure = 1003303;

	public const int uiActivateGirl_FoundRelicDuringAdventure = 1003304;

	public const int uiActivateGirl_Activated = 1003305;

	public const int uiActivateGirl_ShowItemTitle = 1003306;

	public const int uiActivateGirl_ShowItemConfirm = 1003307;

	public const int uiActivateGirl_YouCanFindThem = 1003308;

	public const int uiActivateGirl_Inactive = 1003309;

	public const int uiActivateGirl_FindNPC6AutoActive = 1003310;

	public const int uiActivateGirl_ActiveButton = 1003311;

	public const int uiVirtualStickType = 1003401;

	public const int uiVirtualStickType1 = 1003402;

	public const int uiVirtualStickType2 = 1003403;

	public const int uiVirtualStickType3 = 1003404;

	public const int uiVirtualStickScale = 1003405;

	public const int uiBattleUIPosition = 1003406;

	public const int uiBattleUIIn = 1003407;

	public const int uiBattleUIOut = 1003408;

	public const int uiAllUiSize = 1003409;

	public const int uiSpellDisable_Title = 1003501;

	public const int uiSpellDisable_ConfirmTitle = 1003502;

	public const int uiSpellDisable_Or = 1003503;

	public const int uiSpellDisable_ResidualDisabling = 1003504;

	public const int uiSpellDisable_freeDisableLeft = 1003505;

	public const int uiSpellDisable_HistoryApply = 1003510;

	public const int uiSpellDisable_HistoryResourceInsufficient = 1003511;

	public const int uiSpellDisable_History = 1003506;

	public const int CreditShujianTitle = 1003601;

	public const int CreditShujian = 1003602;

	public const int YuboTitle = 1003603;

	public const int Yubo = 1003604;

	public const int ZijunTitle = 1003605;

	public const int Zijun = 1003606;

	public const int YiFanTitle = 1003607;

	public const int YiFan = 1003608;

	public const int XinYiPengFeiTitle = 1003609;

	public const int XinYiPengFei = 1003610;

	public const int YongKangTitle = 1003611;

	public const int YongKang = 1003612;

	public const int YongNingTitle = 1003613;

	public const int YongNing = 1003614;

	public const int YiLangTitle = 1003615;

	public const int YiLang = 1003616;

	public const int CreditTextsStart = 1003601;

	public const int SummonSpiritEssence_PreDesc = 1003700;

	public const int SummonSpiritEssence_Teammate1 = 1003701;

	public const int SummonSpiritEssence_Teammate2 = 1003702;

	public const int SummonSpiritEssence_Teammate3Pre = 1003703;

	public const int SummonSpiritEssence_Teammate3Post = 1003704;

	public const int SummonSpiritEssence_Teammate4 = 1003705;

	public const int SummonSpiritEssence_Teammate5 = 1003706;

	public const int SummonSpiritEssence_Teammate6 = 1003707;

	public const int SummonSpiritEssence_Teammate7 = 1003708;

	public const int UIMenuTipsHead = 1003800;

	public const int UIMenuTipsFirst = 1003801;

	public const int UIMenuTipsFirstMobile = 1003901;

	public const int UIMobileGuideRightHandle = 1004001;

	public const int UIMobileGuideDrinkPotion = 1004002;

	public const int UIMobileGuideOpenCloseBag = 1004003;

	public const int Name_Elite12 = 1005001;

	public const int Name_Captain = 1005002;

	public const int Name_Lishujian = 1005003;

	public const int Name_NPC1_Dark = 1005010;

	public const int Name_NPC1 = 1005011;

	public const int Name_NPC2_Dark = 1005020;

	public const int Name_NPC2 = 1005021;

	public const int Name_NPC3 = 1005031;

	public const int Name_NPC4 = 1005041;

	public const int Name_NPC5 = 1005051;

	public const int Name_NPC6 = 1005061;

	public const int Name_NPC7 = 1005071;

	public const int Name_NPC8 = 1005081;

	public const int Name_NPC9 = 1005091;

	public const int Name_You = 1005901;

	public const int Name_Dave = 1005902;

	public const int Name_Ghost = 1005911;

	public const int UIReleaseText = 1006001;

	public const int UIReleaseButtonTitle = 1006002;

	public const int UICharacterMe = 1006101;

	public const int UICharacterMoveSpeed = 1006102;

	public const int UICharacterDamageBuff = 1006103;

	public const int UICharacterBasicProperty = 1006104;

	public const int UITourHint = 1006201;

	public const int UIGiftSetCanUnlock = 1006301;

	public const int UIGiftSetAllreadyUnlock = 1006302;

	public const int UIGiftSetTitle = 1006303;

	public const int UIGiftGetBolldRewordPreText = 1006304;

	public const int UIGiftInteractHint = 1006305;

	public const int UIGiftOtherCanBeUnlockLater = 1006306;

	public const int UIMenu_Handbook_ConvenientOpearation = 14000001;

	public const int UIMenu_Handbook_Wand = 14000002;

	public const int UIMenu_Handbook_Spell = 14000003;

	public const int WandAttr_MaxMP = 14000202;

	public const int WandAttr_MPRecovery = 14000203;

	public const int WandAttr_ShootInterval = 14000204;

	public const int WandAttr_Cooldown = 14000205;

	public const int WandAttr_Angle = 14000206;

	public const int WandAttr_ShootCount = 14000207;

	public const int WandAttr_PostSlot = 14000208;

	public const int SpellAttr_Damage = 14000301;

	public const int SpellAttr_Dps = 14000302;

	public const int SpellAttr_Radius = 14000303;

	public const int SpellAttr_SummonLimit = 14000304;

	public const int SpellAttr_CostCorrection = 14000305;

	public const int SpellAttr_KeepCasting = 14000306;

	public const int SpellAttr_CriticalChance = 14000307;

	public const int SpellAttr_FinalDamage = 14000308;

	public const int SpellAttr_Slotcost = 14000310;

	public const int SpellAttr_slotNumModifyValue = 14000311;

	public const int SpellAttr_OnceShootSpellCount = 14000312;

	public const int SpellAttr_ChargedCasting = 14000313;

	public const int SpellAttr_Hp = 14000314;

	public const int TutorialPopOut = 1006701;

	public const int FullGameTitle = 1006801;

	public const int FullGameDes1 = 1006802;

	public const int FullGameDes2 = 1006803;

	public const int FullGamePrice = 1006804;

	public const int FullGameUnlockNow = 1006805;

	public const int SuitDes1 = 1006901;

	public const int SuitDes2 = 1006902;

	public const int SuitUnlock = 1006903;

	public const int EndlessLog_StageClear = 1007201;

	public const int EndlessLog_NewWandAwailable = 1007202;

	public const int EndlessLog_ProcessAwailable = 1007203;

	public const int Endless_Finish_CurLv = 1007301;

	public const int Endless_Finish_BestLv = 1007302;

	public const int Endless_Finish_GetGear = 1007303;

	public const int Endless_Finish_CurRank = 1007304;

	public const int Endless_Finish_BestRank = 1007305;

	public const int Endless_Finish_ShowBuild = 1007306;

	public const int Endless_Finish_GobackCamp = 1007307;

	public const int Endless_Finish_Close = 1007308;

	public const int Endless_Finish_GameOver = 1007309;

	public const int Endless_Finish_GameCalculation = 1007310;
}
