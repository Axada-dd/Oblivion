using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD.单体;

public class 雷1 : ISlotResolver
{
    private readonly uint _skillId = Skill.雷一.GetActionChange();
    private Spell? GetSpell()
    {
        return  _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell != null) 
            slot.Add(spell);
    }

    public int Check()
    {
        if (BLMHelper.双目标aoe()||BLMHelper.三目标aoe()) return -100;
        if (!QT.Instance.GetQt(QTkey.Dot)) return -3;
        if (QT.Instance.GetQt(QTkey.TTK)) return -5;
        if (BattleData.Instance.正在特殊循环中) return -4;
        if (BLMHelper.补dot() && Core.Me.HasAura(Buffs.雷云)) return 1;
        return -99;
    }
}
