using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 雷2 : ISlotResolver
{
    public void Build(Slot slot)
    {
        var canTargetObjects = Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)>2 ? Spells.雷二.最优aoe目标(3) : Core.Me.GetCurrTarget();
        Spell spell = Spells.雷二.GetActionChange().GetSpell(canTargetObjects);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (QT.Instance.GetQt("AOE")) return -4;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -3;
        if (!Spells.雷二.GetSpell().IsReadyWithCanCast()) return -1;
        if (!QT.Instance.GetQt("雷二")) return -2;
        if (Core.Me.HasAura(Buffs.雷云) && (Helper.目标Buff时间小于(Buffs.雷二dot, 3000, false) && Helper.目标Buff时间小于(Buffs.雷一dot, 5000))) return 1;
        return -99;
    }
}
