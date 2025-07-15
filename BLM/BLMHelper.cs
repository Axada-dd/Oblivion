using AEAssist.JobApi;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM;

public static class BLMHelper
{
    public static bool 火状态 => Core.Resolve<JobApi_BlackMage>().InAstralFire;
    public static int 火层数 => Core.Resolve<JobApi_BlackMage>().AstralFireStacks;
    public static int 耀星层数 => Core.Resolve<JobApi_BlackMage>().AstralSoulStacks;

    public static bool 冰状态 => Core.Resolve<JobApi_BlackMage>().InUmbralIce;
    public static int 冰针 => Core.Resolve<JobApi_BlackMage>().UmbralHearts;
    public static int 冰层数 => Core.Resolve<JobApi_BlackMage>().UmbralIceStacks;


    public static bool 悖论指示 => Core.Resolve<JobApi_BlackMage>().IsParadoxActive;
    public static int 通晓层数 => Core.Resolve<JobApi_BlackMage>().PolyglotStacks;
    public static long 通晓剩余时间 => Core.Resolve<JobApi_BlackMage>().EnochianTimer;
    
    public static bool 补dot => Helper.目标Buff时间小于(Buffs.雷一dot, 3500, false) && Helper.目标Buff时间小于(Buffs.雷二dot, 3500, false);
    public static bool 提前补dot => Helper.目标Buff时间小于(Buffs.雷一dot, 6000, false) && Helper.目标Buff时间小于(Buffs.雷二dot, 6000, false);

    public static uint 可用瞬发()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (补dot && Helper.有buff(Buffs.雷云) ) return nearbyEnemyCount >= 2 && QT.Instance.GetQt("AOE") ? Skill.雷二 : Skill.雷一;
        if (悖论指示) return Skill.悖论;
        if (通晓层数 >= 1)return nearbyEnemyCount >= 2 ? Skill.秽浊 : Skill.异言;
        if (提前补dot && Helper.有buff(Buffs.雷云))return nearbyEnemyCount >= 2&& QT.Instance.GetQt("AOE") ? Skill.雷二 : Skill.雷一;
        return 0;
    }

    public static bool 能使用耀星()
    {
        if (BLMHelper.火状态)
        {
            var 能使用的火四个数 = 0;
            var mp = (int)(Core.Me.CurrentMp - 2400);
            if (BLMHelper.冰针 > 0)
            {
                mp -= 800*BLMHelper.冰针;
                能使用的火四个数 += BLMHelper.冰针;
            }

            能使用的火四个数 += mp / 1600;
            return (能使用的火四个数 + BLMHelper.耀星层数) == 6;
        }

        return false;
    }
    public static bool 三连转冰()
    {
        if (火状态)
        {
            if (能使用耀星() && Skill.三连.GetSpell().Charges > 1 && !BattleData.Instance.已使用耀星 &&
                !QT.Instance.GetQt("三连用于走位"))
            {
                if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 8) return false;
                if (Skill.即刻.GetSpell().Cooldown.TotalSeconds < 3) return false;
                return true;
            }
        }
        return false;
    }
}