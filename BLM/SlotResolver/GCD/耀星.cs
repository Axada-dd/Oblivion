namespace Oblivion.BLM.SlotResolver.GCD;

public class 耀星 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.耀星.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Spells.耀星.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.火状态) return -6;
        if (BLMHelper.耀星层数 == 6 && !Helper.IsMove) return 1;
        if (BLMHelper.耀星层数 == 6 && Helper.IsMove && BattleData.Instance.可瞬发) return 2;
        return -99;
    }
}
