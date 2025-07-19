namespace Oblivion.BLM.SlotResolver.GCD;

public class 瞬发gcd触发器 : ISlotResolver
{
    private uint _skillId = 0;
    private Spell? GetSpell()
    {
        return !_skillId.GetSpell().IsReadyWithCanCast() ? null : _skillId.GetSpell();
    }
    public int Check()
    {
        if (Helper.可瞬发()) return -2;
        if (!BattleData.Instance.需要瞬发gcd) return -3;
        _skillId = BLMHelper.可用瞬发();
        if (_skillId == 0) return -4;
        return 1;
    }


    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
        BattleData.Instance.需要瞬发gcd = false;
    }
}