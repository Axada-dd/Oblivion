using AEAssist.JobApi;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM;
public static class BLMHelper
{
    private static readonly List<uint> AoeSkill = [Skill.核爆, Skill.雷二.GetActionChange(), Skill.玄冰, Skill.冰冻, Skill.秽浊,Skill.耀星];
    public static bool 火状态 => Core.Resolve<JobApi_BlackMage>().InAstralFire;
    public static int 火层数 => Core.Resolve<JobApi_BlackMage>().AstralFireStacks;
    public static int 耀星层数 => Core.Resolve<JobApi_BlackMage>().AstralSoulStacks;
    public static bool 有火苗 => Helper.有buff(Buffs.火苗);
    public static bool 冰状态 => Core.Resolve<JobApi_BlackMage>().InUmbralIce;
    public static int 冰针 => Core.Resolve<JobApi_BlackMage>().UmbralHearts;
    public static int 冰层数 => Core.Resolve<JobApi_BlackMage>().UmbralIceStacks;


    public static bool 悖论指示 => Core.Resolve<JobApi_BlackMage>().IsParadoxActive;
    public static int 通晓层数 => Core.Resolve<JobApi_BlackMage>().PolyglotStacks;
    public static long 通晓剩余时间 => Core.Resolve<JobApi_BlackMage>().EnochianTimer;

    public static int 技能CD(this uint skill)
    {
        return (int)skill.GetSpell().Cooldown.TotalMilliseconds;
    }
    public static bool 补dot()
    {
        if (Core.Me.Level >= 92) return Helper.目标Buff时间小于(Buffs.雷一dot, 3500, false) && Helper.目标Buff时间小于(Buffs.雷二dot, 3500, false);
        if (Core.Me.Level >= 45) return Helper.目标Buff时间小于(Buffs.暴雷, 3500, false) && Helper.目标Buff时间小于(Buffs.霹雳, 3500, false);
        return false;
    } 
    public static bool 提前补dot ()
    { 
        if (Core.Me.Level >= 92) return Helper.目标Buff时间小于(Buffs.雷一dot, 6000, false) && Helper.目标Buff时间小于(Buffs.雷二dot, 6000, false);
        if (Core.Me.Level >= 45) return Helper.目标Buff时间小于(Buffs.暴雷, 6000, false) && Helper.目标Buff时间小于(Buffs.霹雳, 6000, false);
        return false;
    } 
    public static bool 能力技卡g => !GCDHelper.CanUseOffGcd();

    public static float 三连cd()
    {
        var 三连cd = 0f;
        if (Skill.三连.GetSpell().Charges > 1)
        {
            三连cd = 60-(Skill.三连.GetSpell().Charges - 1) * 60;
        }
        else 三连cd = 60-Skill.三连.GetSpell().Charges * 60;
        return 三连cd;
    }
    public static bool IsAoe(this uint skill)
    {
        return AoeSkill.Contains(skill);
    }
    public static bool 在发呆()
    {
        if (Core.Me.IsDead) return false;
        if (!Core.Me.InCombat()) return false;
        if (Core.Me.IsCasting) return false;
        if (GCDHelper.GetGCDDuration() > 0) return false;
        return true;
    }

    public static bool 三目标aoe()
    {
        if (!QT.Instance.GetQt(QTkey.Aoe)) return false;
        var count = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        return count >= 3;
    }
    public static bool 双目标aoe()
    {
        if (!QT.Instance.GetQt(QTkey.Aoe)) return false;
        var count = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (count < 2) return false;
        if (三目标aoe()) return false;
        return true;
    }
    public static uint 可用瞬发()
    {
        var aoe = 三目标aoe() || 双目标aoe();
    
        if (补dot() && Helper.有buff(Buffs.雷云)) return aoe ? Skill.雷二.GetActionChange() : Skill.雷一.GetActionChange();
        if (火状态 && 火层数 < 3 && Helper.有buff(Buffs.火苗) && !三目标aoe()) return Skill.火三;
        if (悖论指示)
        {
            if (火状态 && Core.Me.CurrentMp >= 2400)
            {
                if (三目标aoe() || 双目标aoe())
                {
                    if (Core.Me.CurrentMp >= 4100) return Skill.悖论;
                }else
                    return Skill.悖论;
            }
            if (冰状态) return Skill.悖论;
        }
        if (火状态 && Core.Me.CurrentMp < 2400 && Core.Me.CurrentMp >= 800 && Core.Me.Level >= 100) return Skill.绝望;
        if (通晓层数 >= 1 && Core.Me.Level>=80) return aoe ? Skill.秽浊 : Skill.异言;
        if (提前补dot() && Helper.有buff(Buffs.雷云)) return aoe ? Skill.雷二.GetActionChange() : Skill.雷一.GetActionChange();
        if (Skill.即刻.GetSpell().Cooldown.TotalMilliseconds > 0 && Skill.三连.GetSpell().Charges < 1)
        {
            if (Helper.有buff(Buffs.火苗)&&BLMHelper.火状态) return Skill.火三;
            if (Helper.有buff(Buffs.雷云)) return aoe ? Skill.雷二 : Skill.雷一;
        }
        return 0;
    }




}