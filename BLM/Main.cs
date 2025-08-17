using System.Numerics;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.CombatRoutine.View.JobView;
using Oblivion.BLM.QtUI;
using Oblivion.BLM.QtUI.Hotkey;
using Oblivion.BLM.SlotResolver.Ability;
using Oblivion.BLM.SlotResolver.GCD;
using Oblivion.BLM.SlotResolver.GCD.AOE;
using Oblivion.BLM.SlotResolver.GCD.单体;
using Oblivion.BLM.SlotResolver.Opener;
using Oblivion.BLM.SlotResolver.Special;
using Oblivion.BLM.Triggers;
using static Oblivion.BLM.SlotResolver.Special.特殊序列;

namespace Oblivion.BLM;

public class BLMRotationEntry : IRotationEntry, IDisposable
{
    public string AuthorName { get; set; } = Helper.AuthorName;
    private readonly Jobs _job = Jobs.BlackMage;
    private readonly AcrType _acrType = AcrType.Both;
    private readonly int _minLevel = 70;
    private readonly int _maxLevel = 100;
    private readonly string _description = "7.2黑魔，支持70-100级";
    private readonly List<SlotResolverData> _slotResolverData =
    [
        //GCD
        new (new TTK(),SlotMode.Gcd),
        new(new 异言(), SlotMode.Gcd),
        new(new 秽浊(), SlotMode.Gcd),
        new(new 雷1(), SlotMode.Gcd),
        new(new 雷2(), SlotMode.Gcd),
        new(new 瞬发gcd触发器(), SlotMode.Gcd),
        new (new 火群(), SlotMode.Gcd),
        new (new 冰群(), SlotMode.Gcd),
        new (new 冰单100(), SlotMode.Gcd),
        new (new 火单100(), SlotMode.Gcd),
        new (new 冰单90(), SlotMode.Gcd),
        new (new 火单90(), SlotMode.Gcd),
        new (new 冰单80(), SlotMode.Gcd),
        new (new 火单80(), SlotMode.Gcd),
        new (new 冰单70(), SlotMode.Gcd),
        new (new 火单70(), SlotMode.Gcd),
        new(new 核爆补耀星(), SlotMode.Gcd),
        new(new 即刻三连(), SlotMode.Always),
        

        //Ability
        new(new 星灵移位(),SlotMode.OffGcd),
        new(new 即刻(),SlotMode.OffGcd),
        new(new 三连咏唱(),SlotMode.OffGcd),
        new(new 醒梦(),SlotMode.OffGcd),
        new(new 墨泉(),SlotMode.OffGcd),
        new(new 详述(),SlotMode.OffGcd),
        new(new 黑魔纹(),SlotMode.OffGcd),

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
        rot.AddSlotSequences(特殊序列.Build());
        rot.AddOpener(GetOpener);
        rot.SetRotationEventHandler(new BLMEvetHandle());
        rot.AddTriggerAction(new TriggerActionQt(), new TriggerActionHotkey(), new TriggerActionNewQt());
        rot.AddTriggerCondition(new TriggerCondQt());
        rot.AddCanPauseACRCheck(() =>
        {
            //这里加入判断是否要暂停acr，return >0 则暂停
            return -1;
        });
        return rot;
    }

    private IOpener? GetOpener(uint level)
    {
        //if (!QT.Instance.GetQt("起手序列")) return null;
        if (level == 100)
        {
            if (BLMSetting.Instance.标准57) return new Opener57();
            if (BLMSetting.Instance.核爆起手) return new Opener核爆();
            if (BLMSetting.Instance.开挂循环) return new Opener57开挂循环();
        }

        if (level >= 90 && level < 100) return new Opener_lv90();
        if (level >= 80 && level < 90) return new Opener_lv80();
        if (level >= 70 && level < 80) return new Opener_lv70();
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