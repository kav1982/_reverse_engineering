# -*- coding: utf-8 -*-
"""Combine SpellConfig.json + TextConfig_Spell.json into a readable Markdown
appendix table, grouped by useType (Missile / Summon / Enhance / Passive).
"""
import json
import os

BASE = r"D:\SteamLibrary\steamapps\common\Magicraft\_reverse_engineering\assets_export"
OUT = r"D:\SteamLibrary\steamapps\common\Magicraft\_reverse_engineering\法术数值总表.md"

USE_TYPE_NAMES = {0: "主动弹幕 Missile", 1: "召唤 Summon", 2: "强化 Enhance", 3: "被动 Passive"}
DROP_TYPE_NAMES = {0: "无/初始 None", 1: "普通 Common", 2: "稀有 Rare", 3: "史诗 Epic", 4: "特殊 Special"}

ABILITY_TYPE_NAMES = {
    1000: "DefaultEmptySpell 空", 1001: "Bullet 魔法弹", 1002: "Rollball 滚球", 1003: "Butterfly 蝴蝶",
    1004: "Laser 激光", 1005: "PreFirework 烟花", 1006: "HoverTorch 悬浮火把", 1007: "BlackHole 黑洞",
    1008: "ArcaneExplosion 奥术爆炸", 1009: "BackMP 回蓝", 1010: "SnakeWalk 蛇形弹",
    1011: "DisintegrationRay 分解射线", 1012: "FireBall 火球", 1013: "Meteor 陨石", 1014: "Rainbow 彩虹",
    1015: "ArcaneNova 奥术新星", 1016: "Dash 冲刺", 1017: "DeathAdder 死亡毒蛇", 1018: "ThunderAura 雷电光环",
    1019: "HighPressureWasher 高压水枪", 1020: "ManaCoin 魔力金币", 1021: "MagicBreaker 法术破坏者",
    1022: "Boomerang 回旋镖", 1023: "JudgementBlade 审判之刃", 1024: "GiantBubble 巨大气泡",
    1025: "DragonBreath 龙息", 1026: "ShiningStar 闪耀之星", 1027: "SuperNova 超新星",
    1028: "MrBingArrow 冰先生之箭", 1029: "DimensionTraveller 次元旅者", 1030: "Harpoons 鱼叉",
    1031: "ShotGun 霰弹枪",
    2001: "Summon1 召唤物1", 2002: "Summon2 召唤物2(融合头)", 2003: "Summon3 召唤物3",
    2004: "Summon4 召唤物4(光柱)", 2005: "Summon5 召唤物5(法典书)", 2006: "Summon6 召唤物6",
    2007: "Summon7 召唤物7", 2008: "Summon8 召唤物8", 2009: "Summon9 召唤物9",
    3001: "Volley 齐射", 3002: "Multishot 多重射击", 3003: "TotalScattering 全方位散射",
    3004: "MucusCrystal 粘液水晶", 3005: "VenomCrystal 毒液水晶", 3006: "Penetrate 穿透",
    3007: "LightningChain 闪电链", 3008: "SpellHover 悬浮", 3009: "AroundOwner 环绕自身",
    3010: "AroundMouse 环绕鼠标", 3011: "FollowTarget 跟踪目标", 3012: "Rebound 反弹",
    3013: "SpellSplit 法术分裂", 3014: "Frozen 冰冻", 3015: "ParasiticWorm 寄生虫",
    3101: "ThunderCrystal 雷电水晶", 3102: "EnhanceAttackRatio 攻击力强化(加算)",
    3103: "EnhanceDurationValue 持续时间强化", 3104: "PowerSavingMode 节能模式(伤害*倍率)",
    3105: "EnhanceCriticalChance 暴击率强化", 3106: "EnhanceSpeedValue 速度强化",
    3107: "EnhanceRadiusRatio 范围强化", 3108: "EnhanceSummonHPRecover 召唤物回血强化",
    3109: "Mimic 模拟(复制目标法术)", 3110: "LifeLine 生命线", 3111: "FireCrystal 火焰水晶(伤害加算)",
    3112: "PullForceCrystal 引力水晶", 3113: "RadiuRatioDown 范围衰减",
    3114: "FollowOwner 跟随主人", 3115: "FusionSummon 融合召唤物", 3116: "SpellEndTeleport 法术结束传送",
    3117: "RandomRotationRadiu 随机旋转半径", 3118: "TeammateSacrifice 队友献祭",
    3119: "Fall 下落", 3120: "TeammateSprite 队友精灵", 3121: "Refraction 折射(伤害*倍率)",
    3122: "Unyielding 不屈", 3123: "OverDrive 超载(伤害加算)", 3124: "FatSpell 肥法术",
    3125: "ReverseCast 反向施法", 3126: "SpellLevelEnhance 法术等级强化", 3127: "SoulMate 灵魂伴侣",
    3128: "SpeedToDuration 速度转持续时间", 3129: "DeathInfect 死亡感染", 3130: "RandomTeleport 随机传送",
    3201: "OnOverTrigger 结束时触发器", 3202: "OnOverSplitTrigger 结束分裂触发器",
    3203: "OnMoveTrigger 移动触发器", 3204: "OnStartRotationTrigger 起始旋转触发器",
    3205: "OnHitTrigger 命中触发器",
    4001: "SacrificeBall 献祭球", 4002: "EmptyContainer 空的容器(+MP上限)", 4003: "ManaEssence 魔力精华(+回蓝)",
    4004: "ChargeMode 蓄力模式", 4005: "WandSpirit 法杖之魂", 4006: "ForceCoolDown 强制冷却",
    4007: "UltimateExtender 终极延伸器", 4008: "EchoRune 回声符文", 4009: "ManaInterface 魔力接口",
    4010: "AllFieldEnhance 全域强化", 4011: "EqualDistributionAngle 等分角度",
    4012: "Umbrella 保护伞", 4013: "RuneHammer 符文之锤", 4014: "LaserBeam 激光束",
    4015: "PostSlotExtenderMove 蓄能槽延伸器(移动)", 4016: "PostSlotExtenderStand 蓄能槽延伸器(静止)",
    4017: "PostSlotExtenderTime 蓄能槽延伸器(时间)", 4018: "PostSlotExtenderCastSpell 蓄能槽延伸器(施法)",
    4019: "BiAnLethalBlade 彼岸致命之刃", 4020: "SpellEmbryo 法术胚胎", 4021: "ManaTendril 魔力卷须",
    4022: "RandomPosFocusMouse 随机位置聚焦鼠标", 4023: "ManaToPostChargeRatio 魔力转蓄能比",
    4024: "DaveHarpoons 戴夫鱼叉", 4025: "RedRune 红符文", 4026: "GreenRune 绿符文", 4027: "BlueRune 蓝符文",
    9001: "BulletParabola 抛物线弹道", 9002: "BounceBone 弹跳骨头", 9003: "LongTrail 长拖尾",
    9004: "SoundWave 声波", 9005: "ChainStar 链星", 9006: "BulltHell 弹幕地狱", 9007: "BulletSin 正弦弹道",
    9008: "BulletSinSpeed 正弦速度弹道", 9009: "BladeWave 刀刃波", 9010: "BounceBullet 弹跳子弹",
    9011: "RotateArrow 旋转箭", 9012: "Bat 蝙蝠", 9013: "BladeWaveVertical 竖直刀刃波",
    9014: "Spear 长矛", 9015: "IcnBall 冰球", 9016: "ChaseBullet 追踪子弹",
    0: "None 无",
}


def f(v, nd=1):
    if v is None:
        return ""
    if isinstance(v, float):
        if abs(v) < 1e-9:
            return ""
        return f"{v:.{nd}f}".rstrip('0').rstrip('.') if '.' in f"{v:.{nd}f}" else f"{v:.{nd}f}"
    return str(v) if v else ""


def load(name):
    with open(os.path.join(BASE, f"{name}.json"), encoding="utf-8") as fp:
        return json.load(fp)


def main():
    spells = load("SpellConfig")
    texts = {t["id"]: t for t in load("TextConfig_Spell")}

    def name_of(spell_id):
        t = texts.get(spell_id + 7000000)
        if not t:
            return ""
        cn = t.get("chineseS", "") or ""
        en = t.get("english", "") or ""
        return cn, en

    groups = {0: [], 1: [], 2: [], 3: []}
    for s in spells:
        groups[s["useType"]].append(s)

    lines = []
    lines.append("# 《魔法工艺》法术数值总表（自动生成，来源：Resources/Configs/SpellConfig.json）\n")
    lines.append(f"共 {len(spells)} 条法术记录（含同一法术的 Lv1/Lv2/Lv3 等多条等级记录）。\n")
    lines.append("字段说明见主报告正文。float1/int1 等通用字段的含义随 `abilityType` 不同而不同，仅在非 0 时列出，供交叉查阅源码使用。\n")

    for use_type, group in groups.items():
        group_sorted = sorted(group, key=lambda s: s["id"])
        lines.append(f"\n## {USE_TYPE_NAMES[use_type]} （{len(group_sorted)} 条）\n")
        lines.append("| ID | 中文名 | 英文名 | 能力类型 abilityType | Lv | 稀有度 | 弹格 | 伤害 | 蓝耗 | 连发 | 速度 | 持续 | 暴击% | 半径 | 冷却调整 | 额外字段(非0) |")
        lines.append("|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|")
        for s in group_sorted:
            cn, en = name_of(s["id"])
            ability = ABILITY_TYPE_NAMES.get(s["abilityType"], str(s["abilityType"]))
            rarity = DROP_TYPE_NAMES.get(s["dropType"], str(s["dropType"]))
            cooldown_bits = []
            if s.get("coolDownAddSubRevise"):
                cooldown_bits.append(f"CD{'+' if s['coolDownAddSubRevise']>0 else ''}{f(s['coolDownAddSubRevise'],2)}s")
            if s.get("coolDownRatio") and abs(s["coolDownRatio"] - 1.0) > 0.001:
                cooldown_bits.append(f"CD×{f(s['coolDownRatio']*100,0)}%")
            if s.get("shootIntervalAddSubRevise"):
                cooldown_bits.append(f"射速间隔{'+' if s['shootIntervalAddSubRevise']>0 else ''}{f(s['shootIntervalAddSubRevise'],2)}s")
            cooldown_str = "; ".join(cooldown_bits)

            extra_bits = []
            for fld in ("float1", "float2", "float3", "int1", "int2", "int3"):
                v = s.get(fld)
                if v:
                    extra_bits.append(f"{fld}={f(v,2)}")
            if s.get("summonID"):
                extra_bits.append(f"summonID={s['summonID']}")
            if s.get("summonLimit"):
                extra_bits.append(f"summonLimit={s['summonLimit']}")
            if s.get("knockback"):
                extra_bits.append(f"击退={f(s['knockback'],1)}")
            if s.get("recoil"):
                extra_bits.append(f"后坐力={f(s['recoil'],1)}")
            if s.get("mpCostAddSubCorrection"):
                extra_bits.append(f"耗蓝{'+' if s['mpCostAddSubCorrection']>0 else ''}{f(s['mpCostAddSubCorrection'],1)}%")
            if s.get("mpCostMulDivCorrection") and abs(s["mpCostMulDivCorrection"]) > 0.01:
                extra_bits.append(f"耗蓝×{f(s['mpCostMulDivCorrection'],1)}%")
            if s.get("isDPS"):
                extra_bits.append(f"DPS间隔={f(s.get('DPSDamageInterval',0),2)}s")
            if s.get("slotNumModifyValue"):
                extra_bits.append(f"格数调整={s['slotNumModifyValue']}")
            if s.get("gravity"):
                extra_bits.append(f"重力={f(s['gravity'],2)}")
            if s.get("upSpeed"):
                extra_bits.append(f"上升速度={f(s['upSpeed'],2)}")
            extra_str = "; ".join(extra_bits)

            row = [
                str(s["id"]), cn, en, ability, str(s.get("level", 1)), rarity,
                str(s.get("slotCost", 1)) if s.get("slotCost", 1) != 1 else "",
                f(s.get("damage")), str(s.get("mpCost") or ""),
                str(s.get("shootCount")) if s.get("shootCount", 1) != 1 else "",
                f(s.get("speed")), f(s.get("duration")),
                f(s.get("criticalChance")), f(s.get("radius")),
                cooldown_str, extra_str,
            ]
            lines.append("| " + " | ".join(x.replace("|", "/") for x in row) + " |")

    with open(OUT, "w", encoding="utf-8") as fp:
        fp.write("\n".join(lines))
    print("Wrote", OUT, "total lines", len(lines))


if __name__ == "__main__":
    main()
