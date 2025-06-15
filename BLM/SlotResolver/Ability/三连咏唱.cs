namespace Oblivion.BLM.SlotResolver.Ability;

public class 三连咏唱 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.三连.GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Spells.三连.GetSpell().IsReadyWithCanCast()) return -1;
        if(BLMHelper.火状态&&Spells.三连.GetSpell().Charges*60>=110)return 1;
        return -99;
    }
}
