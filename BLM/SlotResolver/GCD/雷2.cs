using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 雷2 : ISlotResolver
{
    public void Build(Slot slot)
    {
        var canTargetObjects = Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)>2 ? Skill.雷二.最优aoe目标(3) : Core.Me.GetCurrTarget();
        Spell spell = Skill.雷二.GetActionChange().GetSpell(QT.Instance.GetQt("智能AOE目标") ? canTargetObjects : Core.Me.GetCurrTarget());
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("AOE")) return -4;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -3;
        if (!Skill.雷二.GetSpell().IsReadyWithCanCast()) return -1;
        if (!QT.Instance.GetQt("Dot")) return -2;
        if (BattleData.Instance.正在特殊循环中) return -4;
        if (Core.Me.HasAura(Buffs.雷云) && BLMHelper.补dot) return 1;
        return -99;
    }
}
