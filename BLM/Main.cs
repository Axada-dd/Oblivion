using System;
using System.Collections.Generic;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.GCD;
using Oblivion.BLM.Triggers;
using Oblivion.Common;

namespace Oblivion.BLM;

public class BLMRotationEntry: IRotationEntry,IDisposable
{
    public string AuthorName { get; set; } = Helper.AuthorName;
    private readonly Jobs _job = Jobs.BlackMage;
    private readonly AcrType _acrType = AcrType.HighEnd;
    private readonly int _minLevel = 100;
    private readonly int _maxLevel = 100;
    private readonly string _description = "BLM";
    private readonly List<SlotResolverData> _slotResolverData =
    [
        //GCD
        new(new 火三(), SlotMode.Gcd),
        new(new 冰三(), SlotMode.Gcd),
        new(new 火4(), SlotMode.Gcd),
        new(new 绝望(), SlotMode.Gcd),
        new(new 耀星(), SlotMode.Gcd),
        new(new 悖论(), SlotMode.Gcd),
        new(new 冰澈(), SlotMode.Gcd),
        
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
        rot.SetRotationEventHandler(new BLMEvetHandle());
        rot.AddTriggerAction(new TriggerActionQt(), new TriggerActionHotkey());
        rot.AddTriggerCondition(new TriggerCondQt());
        return rot;
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