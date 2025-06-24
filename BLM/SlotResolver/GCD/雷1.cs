using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 雷1 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.雷一.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        int enemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (!Spells.雷一.GetSpell().IsReadyWithCanCast()) return -1;
        if (enemyCount >= 2&& QT.Instance.GetQt("AOE")) return -2;
        if (!QT.Instance.GetQt("雷一")) return -3;
        if ((Helper.目标Buff时间小于(Buffs.雷一dot, 3000, false) && Helper.目标Buff时间小于(Buffs.雷二dot, 3000, false)) && Core.Me.HasAura(Buffs.雷云)) return 1;
        return -99;
    }
}
