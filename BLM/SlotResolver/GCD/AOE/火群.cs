using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD.AOE;

public class 火群 : ISlotResolver
{
    private uint _skillId = 0;
    private Spell? GetSpell()
    {
        return QT.Instance.GetQt(QTkey.智能aoe目标)? _skillId.GetSpellBySmartTarget() : _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell != null) 
            slot.Add(spell);
    }

    private uint GetSkillId()
    {
        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp >= 800) return Skill.核爆;
            if (BLMHelper.耀星层数 == 6) return Skill.耀星;
        }

        if (BLMHelper.冰状态)
        {
            if (BLMHelper.冰针 < 3) return 0;
            if (BLMHelper.三目标aoe()&&QT.Instance.GetQt(QTkey.火二)) return Skill.火二.GetActionChange();
            //if (BLMHelper.双目标aoe()) return Skill.火三;
        }
        return 0;
    }
    public int Check()
    {
        if (Core.Me.Level < 70) return -70;
        if (!(BLMHelper.三目标aoe() || BLMHelper.双目标aoe())) return -234;
        _skillId = GetSkillId();
        if (_skillId == 0) return -1;
        return (int)_skillId;
    }
}