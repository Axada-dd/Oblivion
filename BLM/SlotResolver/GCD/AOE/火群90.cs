using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD.AOE;

public class 火群90 : ISlotResolver
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
        return 0;
    }
    public int Check()
    {
        if (Core.Me.Level != 100) return -100;
        _skillId = GetSkillId();
        if (_skillId == 0) return -1;
        return (int)_skillId;
    }
}