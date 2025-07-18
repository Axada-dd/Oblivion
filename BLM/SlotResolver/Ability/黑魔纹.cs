using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 黑魔纹 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = new Spell(Skill.黑魔纹, SpellTargetType.Self);
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("黑魔纹")) return -5;
        if (!Skill.黑魔纹.GetSpell().IsReadyWithCanCast()) return -1;
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
