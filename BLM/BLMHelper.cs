using AEAssist.JobApi;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM;
public static class BLMHelper
{
    private static readonly List<uint> AoeSkill = [Skill.核爆, Skill.雷二, Skill.玄冰, Skill.冰冻, Skill.秽浊,Skill.耀星];
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

    public static bool 补dot => Helper.目标Buff时间小于(Buffs.雷一dot, 3500, false) && Helper.目标Buff时间小于(Buffs.雷二dot, 3500, false);
    public static bool 提前补dot => Helper.目标Buff时间小于(Buffs.雷一dot, 6000, false) && Helper.目标Buff时间小于(Buffs.雷二dot, 6000, false);

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
        if (!QT.Instance.GetQt(QTkey.双目标aoe)) return false;
        var count = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (count < 2) return false;
        if (三目标aoe()) return false;
        return true;
    }
    public static uint 可用瞬发()
    {
        var aoe = 三目标aoe() || 双目标aoe();
    
        if (补dot && Helper.有buff(Buffs.雷云)) return aoe ? Skill.雷二 : Skill.雷一;
        if (火状态 && 火层数 < 3 && Helper.有buff(Buffs.火苗) && !三目标aoe()) return Skill.火三;
        if (悖论指示)
        {
            if (火状态 && Core.Me.CurrentMp >= 2400) return Skill.悖论;
            if (冰状态) return Skill.悖论;
        }
        if (火状态 && Core.Me.CurrentMp < 2400 && Core.Me.CurrentMp >= 800) return 0;
        if (通晓层数 >= 1 && Core.Me.Level>=80) return aoe ? Skill.秽浊 : Skill.异言;
        if (提前补dot && Helper.有buff(Buffs.雷云)) return aoe ? Skill.雷二 : Skill.雷一;
        if (Skill.即刻.GetSpell().Cooldown.TotalMilliseconds > 0 && Skill.三连.GetSpell().Charges < 1)
        {
            if (Helper.有buff(Buffs.火苗)&&BLMHelper.火状态) return Skill.火三;
            if (Helper.有buff(Buffs.雷云)) return aoe ? Skill.雷二 : Skill.雷一;
        }
        return 0;
    }

    public static int 可用瞬发数()
    {
        int i = 0;
        if (悖论指示) i++;
        if (通晓层数 >= 1) i += 通晓层数;
        if (提前补dot && Helper.有buff(Buffs.雷云)) i++;
        return i;
    }
    public static bool 能使用耀星()
    {
        if (BLMHelper.火状态)
        {
            var 能使用的火四个数 = 0;
            var mp = (int)(Core.Me.CurrentMp - 2400);
            if (BLMHelper.冰针 > 0)
            {
                mp -= 800 * BLMHelper.冰针;
                能使用的火四个数 += BLMHelper.冰针;
            }

            能使用的火四个数 += mp / 1600;
            return (能使用的火四个数 + BLMHelper.耀星层数) == 6;
        }

        if (火状态 && QT.Instance.GetQt("AOE"))
        {
            var mp = (int)Core.Me.CurrentMp;
            var 耀星层数 = 0;
            if (冰针 > 0 && mp > 800)
            {
                耀星层数 += 3;
                mp = (int)(mp * 0.33334f);
            }

            if (BLMHelper.耀星层数 > 0)
            {
                耀星层数 += BLMHelper.耀星层数;
            }
            if (mp > 800) 耀星层数+=3;
            return 耀星层数 == 6;
        }
        return false;
    }
    public static int 能使用的火四个数()
    {
        if (BLMHelper.火状态)
        {
            var 模拟mp = (int)Core.Me.CurrentMp;
            int 火四 = 0;
            if (BLMHelper.悖论指示)
            {
                模拟mp -= 1600;
            }
            if (!BattleData.Instance.已使用绝望)//绝望
            {
                模拟mp -= 800;
            }
            if (BLMHelper.冰针 > 0)
            {
                模拟mp -= 800 * BLMHelper.冰针;
                火四 += BLMHelper.冰针;
            }

            火四 += 模拟mp / 1600;
            return 火四;
        }

        return 0;
    }

    public static int 火循环gcd()
    {
        int i = 0;
        if (火状态)
        {

            var 模拟mp = (int)Core.Me.CurrentMp;
            if (悖论指示)
            {
                模拟mp -= 1600;
                i++;
            }

            if (能使用耀星())
            {
                i++;
            }
            //绝望
            模拟mp -= 800;
            i++;
            if (火层数 < 3) i++;
            if (冰针 > 0)
            {
                模拟mp -= 800 * BLMHelper.冰针;
                i += 冰针;
            }
            i += 模拟mp / 1600;
            if (Helper.目标Buff时间小于(Buffs.雷一dot, BattleData.Instance.火循环剩余gcd * BattleData.Instance.复唱时间, false) &&
                Core.Me.HasAura(Buffs.雷云)) i++;
            return i;
        }
        return 0;
    }

    public static int 冰循环gcd()
    {
        int i = 0;
        if (BLMHelper.冰状态)
        {
            if (BLMHelper.冰层数 < 3) i++;
            if (BLMHelper.冰针 < 3) i++;
            if (BLMHelper.悖论指示) i++;
            if (Helper.目标Buff时间小于(Buffs.雷一dot, i * BattleData.Instance.复唱时间, false) &&
                Core.Me.HasAura(Buffs.雷云)) i++;
            return i;
        }

        return 0;
    }
    public static bool 三连转冰()
    {
        if (火状态)
        {
            if (能使用耀星() && Skill.三连.GetSpell().Charges > 1 && !BattleData.Instance.已使用耀星 &&
                !QT.Instance.GetQt("三连用于走位"))
            {
                if (Skill.墨泉.GetSpell().AbilityCoolDownInNextXgcDsWindow(3)) return false;
                if (Skill.即刻.GetSpell().AbilityCoolDownInNextXgcDsWindow(2)) return false;
                return true;
            }
        }
        return false;
    }
    public static bool 能星灵转冰()
    {
        if (火状态)
        {
            if (QT.Instance.GetQt("即刻") && Skill.即刻.GetSpell().Cooldown.TotalMilliseconds < BattleData.Instance.复唱时间 * BattleData.Instance.火循环剩余gcd) return true;
            if (QT.Instance.GetQt("三连咏唱") && Skill.三连.GetSpell().Charges >= 1) return true;
        }
        if (冰状态)
        {
            if (悖论指示)
            {

                if (QT.Instance.GetQt("即刻") && Skill.即刻.GetSpell().Cooldown.TotalMilliseconds < BattleData.Instance.复唱时间) return true;
            }
            if (QT.Instance.GetQt("三连咏唱") && Skill.三连.GetSpell().Charges >= 1) return true;
        }
        return false;
    }

    public static bool 强制补冰()
    {
        if (火状态) return false;
        if (冰层数 == 3) return false;
        if (Skill.冰三.RecentlyUsed(2500)) return false;
        if (Skill.即刻.GetSpell().Cooldown.TotalSeconds > 3 && Skill.三连.GetSpell().Charges < 1) return true;
        return false;
    }
}