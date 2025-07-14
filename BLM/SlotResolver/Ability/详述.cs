using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 详述 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Skill.详述.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("详述")) return -5;
        if (!Skill.详述.GetSpell().IsReadyWithCanCast()) return -1;
        if (BLMHelper.通晓层数 == 3) return -2;
        if (BLMHelper.通晓层数 == 2)
        {
            if (BLMHelper.通晓剩余时间 < 4000) return -3;
        }
        return 1;
    }
}
