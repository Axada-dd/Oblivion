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
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("黑魔纹")) return -5;
        if (_skillId.GetSpell().Charges < 1) return -1;
        if (BattleData.Instance.已使用黑魔纹) return -3;
        if (!BattleData.Instance.已使用瞬发 )
        {
            if (BattleData.Instance.需要瞬发gcd) return -2;
            BattleData.Instance.需要瞬发gcd = true;
            return -2;
        }
        return 1;
    }
}
