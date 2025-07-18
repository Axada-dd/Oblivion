using Oblivion.BLM.SlotResolver.Ability;
using Oblivion.BLM.SlotResolver.GCD;

namespace Oblivion.BLM;

public class 求解器
{
    private static readonly List<SlotResolverData> _slotResolverData =
    [
        //GCD
        new(new 异言(), SlotMode.Gcd),
        new(new 秽浊(), SlotMode.Gcd),
        new (new 瞬发gcd触发器(), SlotMode.Gcd),
        new(new 雷1(), SlotMode.Gcd),
        new(new 雷2(), SlotMode.Gcd),
        new(new 悖论(), SlotMode.Gcd),
        new(new 耀星(), SlotMode.Gcd),
        new(new 绝望(), SlotMode.Gcd),
        new(new 火三(), SlotMode.Gcd),
        new(new 冰三(), SlotMode.Gcd),
        new(new 火4(), SlotMode.Gcd),
        new(new 冰澈(), SlotMode.Gcd),
        new(new 核爆(), SlotMode.Gcd),
        new(new 核爆补耀星(), SlotMode.Gcd),
        new(new 玄冰(), SlotMode.Gcd),
        new(new 冰冻(), SlotMode.Gcd),
        // new(new 火一(), SlotMode.Gcd),
        // new(new 火二(), SlotMode.Gcd),


        //Ability
        new(new 醒梦(),SlotMode.OffGcd),
        new(new 墨泉(),SlotMode.OffGcd),
        new(new 详述(),SlotMode.OffGcd),
        new(new 即刻(),SlotMode.OffGcd),
        new(new 星灵移位(),SlotMode.OffGcd),
        new(new 三连咏唱(),SlotMode.OffGcd),
        new(new 黑魔纹(),SlotMode.OffGcd),

    ];
    public static string 下一个GCD()
    {
        // 尝试找到第一个槽模式为全局冷却且可用的技能解析器
        var firstGCDSkill =
            _slotResolverData.FirstOrDefault(srd => srd.SlotMode == SlotMode.Gcd && srd.SlotResolver.Check() >= 0);
        // 如果找到了，则返回该技能解析器的类型名称；否则，返回"无技能"
        return firstGCDSkill != null ? firstGCDSkill.SlotResolver.GetType().Name: "无技能";
    }
    /// <summary>
    /// 检查并返回第一个可用的非全局冷却（Off-GCD）技能的名称。
    /// </summary>
    /// <returns>返回第一个可用的Off-GCD技能的名称，如果没有可用的Off-GCD技能，则返回"无技能"。</returns>
    public static string 下一个OGCD()
    {
        // 尝试找到第一个槽模式为非全局冷却且可用的技能解析器
        var firstoffGCDSkill =
            _slotResolverData.FirstOrDefault(srd => srd.SlotMode == SlotMode.OffGcd && srd.SlotResolver.Check() >= 0);
        // 如果找到了，则返回该技能解析器的类型名称；否则，返回"无技能"
        return firstoffGCDSkill != null ? firstoffGCDSkill.SlotResolver.GetType().Name : "无技能";
    }
}