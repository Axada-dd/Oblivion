namespace Oblivion.BLM.SlotResolver.Ability;

public class 详述 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.详述.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Spells.详述.GetSpell().IsReadyWithCanCast()) return -1;
        if (BLMHelper.通晓层数 >= 3 || (BLMHelper.耀星层数 == 2 && BLMHelper.通晓剩余时间 < 4000)) return -2;
        return 1;
    }
}
