namespace Oblivion.BLM.SlotResolver.GCD;

public class 冰澈 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.冰澈.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Spells.冰澈.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.冰状态) return -6;
        if (BLMHelper.冰层数 == 3 && BLMHelper.冰针 < 3 && !Helper.IsMove) return 1;
        if (BLMHelper.冰层数 == 3 && BLMHelper.冰针 < 3 && Helper.IsMove && BattleData.Instance.可瞬发) return 2;
        return -99;
    }
}
