using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 冰冻 : ISlotResolver
{
    private readonly uint _skillId = Skill.冰冻;
    private Spell? GetSpell()
    {
        if (!_skillId.GetSpell().IsReadyWithCanCast()) return null;
        return QT.Instance.GetQt(QTkey.智能AOE目标)? _skillId.GetSpellBySmartTarget() : _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {
        Spell? spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }
    public int Check()
    {
        if (!BLMHelper.三目标aoe()) return -1;
        return -99;
    }

}