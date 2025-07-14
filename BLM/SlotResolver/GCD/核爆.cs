using AEAssist.MemoryApi;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 核爆 : ISlotResolver
{
    public void Build(Slot slot)
    {
        var canTargetObjects = Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)>2 ? Skill.核爆.最优aoe目标(2) : Core.Me.GetCurrTarget();
        Spell spell = Skill.核爆.GetActionChange().GetSpell(QT.Instance.GetQt("智能AOE目标") ? canTargetObjects : Core.Me.GetCurrTarget());
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -1;
        if (!QT.Instance.GetQt("核爆") && !QT.Instance.GetQt("AOE")) return -5;
        if (!Skill.核爆.GetSpell().IsReadyWithCanCast()) return -2;
        if (!BLMHelper.火状态) return -3;
        if (Helper.IsMove && !Helper.可瞬发()) return -4;
        return 1;
    }
}
