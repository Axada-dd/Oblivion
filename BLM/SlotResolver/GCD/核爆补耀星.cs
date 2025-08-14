using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 核爆补耀星 : ISlotResolver
{
    private uint _skillId = 0;
    private Spell? GetSpell()
    {
        _skillId = UseSkill();
        if (_skillId == 0) return null;
        //if (!_skillId.GetSpell().IsReadyWithCanCast()) return null;
        if (_skillId.IsAoe()) return QT.Instance.GetQt(QTkey.智能aoe目标)  ? _skillId.GetSpellBySmartTarget() : _skillId.GetSpell();
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

        return -99;
    }

    private uint UseSkill()
    {
        if (BLMHelper.冰针 >= 1 && (int)Core.Me.CurrentMp * 0.333 >= 800) return Skill.核爆;
        if (BLMHelper.耀星层数 + 3 >= 6) return Skill.核爆;
        return Skill.绝望;
    }

}