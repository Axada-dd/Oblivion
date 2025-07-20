using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 黑魔纹 : ISlotResolver
{
    private readonly uint _skillId = Skill.黑魔纹;
    private Spell? GetSpell()
    {
        return !_skillId.GetSpell().IsReadyWithCanCast() ? null : _skillId.GetSpell(SpellTargetType.Self);
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell == null) return;
        slot.Add2NdWindowAbility(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt(QTkey.黑魔纹)) return -5;
        if (_skillId.GetSpell().Charges < 1) return -1;
        if (Helper.有buff(737)) return -3;
        return 1;
    }
}
