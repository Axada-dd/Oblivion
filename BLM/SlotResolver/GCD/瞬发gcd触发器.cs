namespace Oblivion.BLM.SlotResolver.GCD;

public class 瞬发gcd触发器 : ISlotResolver
{
    public int Check()
    {
        if (Helper.可瞬发()) return -2;
        if (BattleData.Instance.需要瞬发) return 1;
        return -1;
    }


    public void Build(Slot slot)
    {
        if (BLMHelper.可用瞬发() == 0) return;
        Spell spell = BLMHelper.可用瞬发().GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
        BattleData.Instance.需要瞬发 = false;
    }
}