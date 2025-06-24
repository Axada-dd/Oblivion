using AEAssist.CombatRoutine.Module.Opener;
using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Ability;
using Oblivion.BLM.SlotResolver.GCD;
using Oblivion.BLM.SlotResolver.Opener;
using Oblivion.BLM.Triggers;

namespace Oblivion.BLM;

public class BLMRotationEntry : IRotationEntry, IDisposable
{
    public string AuthorName { get; set; } = Helper.AuthorName;
    private readonly Jobs _job = Jobs.BlackMage;
    private readonly AcrType _acrType = AcrType.Both;
    private readonly int _minLevel = 100;
    private readonly int _maxLevel = 100;
    private readonly string _description = "7.2黑魔，暂时只支持100级高难，非最优循环，优先保证灵活性";
    private readonly List<SlotResolverData> _slotResolverData =
    [
        //GCD
        new(new 雷1(), SlotMode.Gcd),
        new(new 雷2(), SlotMode.Gcd),
        new(new 悖论(), SlotMode.Gcd),
        new(new 异言(), SlotMode.Gcd),
        new(new 绝望(), SlotMode.Gcd),
        new(new 秽浊(), SlotMode.Gcd),
        new(new 火三(), SlotMode.Gcd),
        new(new 冰三(), SlotMode.Gcd),
        new(new 火4(), SlotMode.Gcd),
        new(new 耀星(), SlotMode.Gcd),
        new(new 冰澈(), SlotMode.Gcd),
        new(new 核爆(), SlotMode.Gcd),
        // new(new 玄冰(), SlotMode.Gcd),
        // new(new 冰冻(), SlotMode.Gcd),
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

    public string 下一个GCD()
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
    public string 下一个OGCD()
    {
        // 尝试找到第一个槽模式为非全局冷却且可用的技能解析器
        var firstoffGCDSkill =
            _slotResolverData.FirstOrDefault(srd => srd.SlotMode == SlotMode.OffGcd && srd.SlotResolver.Check() >= 0);
        // 如果找到了，则返回该技能解析器的类型名称；否则，返回"无技能"
        return firstoffGCDSkill != null ? firstoffGCDSkill.SlotResolver.GetType().Name : "无技能";
    }
    public Rotation? Build(string settingFolder)
    {
        BLMSetting.Build(settingFolder);
        QT.Build();
        var rot = new Rotation(_slotResolverData)
        {
            TargetJob = _job,
            AcrType = _acrType,
            MinLevel = _minLevel,
            MaxLevel = _maxLevel,
            Description = _description,
        };
        rot.AddOpener(GetOpener);
        rot.SetRotationEventHandler(new BLMEvetHandle());
        rot.AddTriggerAction(new TriggerActionQt(), new TriggerActionHotkey());
        rot.AddTriggerCondition(new TriggerCondQt());
        return rot;
    }

    private IOpener? GetOpener(uint level)
    {
        //if (!QT.Instance.GetQt("起手序列")) return null;
        if (level == 100)
        {
            if (BLMSetting.Instance.标准57) return new Opener57();
            if (BLMSetting.Instance.核爆起手) return new Opener核爆();
        }

        return null;
    }

    public IRotationUI GetRotationUI()
    {
        return QT.Instance;
    }
    public void OnDrawSetting()
    {
    }

    public void Dispose()
    {
    }
}