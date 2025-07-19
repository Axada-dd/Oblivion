using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 详述 : ISlotResolver
{
    private readonly uint _skillId = Skill.详述;
    private Spell? GetSpell()
    {
        return !_skillId.GetSpell().IsReadyWithCanCast() ? null : _skillId.GetSpell(SpellTargetType.Self);
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("详述")) return -5;
        if (_skillId.GetSpell().Cooldown.TotalMilliseconds > 0) return -1;
        if (BLMHelper.通晓层数 == 3) return -2;
        if (BLMHelper.通晓层数 == 2)
        {
            if (BLMHelper.通晓剩余时间 < 4000) return -3;
        }
        return 1;
    }
}
