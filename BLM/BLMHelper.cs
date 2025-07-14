using AEAssist.JobApi;

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
}