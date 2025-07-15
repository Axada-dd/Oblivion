using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 雷1 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Skill.雷一.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        int enemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (!Skill.雷一.GetSpell().IsReadyWithCanCast()) return -1;
        if (enemyCount >= 2 && QT.Instance.GetQt("AOE")) return -2;
        if (!QT.Instance.GetQt("Dot")) return -3;
        if (BattleData.Instance.正在特殊循环中) return -4;
        if (BLMHelper.补dot && Core.Me.HasAura(Buffs.雷云)) return 1;
        return -99;
    }
}
