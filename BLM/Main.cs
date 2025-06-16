using AEAssist.CombatRoutine.Module.Opener;
using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Ability;
using Oblivion.BLM.SlotResolver.GCD;
using Oblivion.BLM.SlotResolver.Opener;
using Oblivion.BLM.Triggers;

namespace Oblivion.BLM;

public class BLMRotationEntry: IRotationEntry,IDisposable
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
        new(new 异言(), SlotMode.Gcd),
        new(new 秽浊(), SlotMode.Gcd),
        new(new 悖论(), SlotMode.Gcd),
        new(new 火三(), SlotMode.Gcd),
        new(new 冰三(), SlotMode.Gcd),
        new(new 火4(), SlotMode.Gcd),
        new(new 绝望(), SlotMode.Gcd),
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
        new(new 星灵移位(),SlotMode.OffGcd),
        new(new 黑魔纹(),SlotMode.OffGcd),
        new(new 三连咏唱(),SlotMode.OffGcd),
        new(new 即刻(),SlotMode.OffGcd),
        
    ];

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
        if (level == 100)
        {
            if(BLMSetting.Instance.标准57)return new Opener_57();
            if(BLMSetting.Instance.核爆起手)return new Oener_核爆();
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