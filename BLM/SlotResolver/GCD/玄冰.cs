using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 玄冰: ISlotResolver
{
    public int Check()
    {
        if (!QT.Instance.GetQt("AOE")) return -1;
        if(!BLMHelper.冰状态)return -2;
        if(!Skill.玄冰.GetActionChange().GetSpell().IsReadyWithCanCast())return -3;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -4;
        if (BLMHelper.冰层数 != 3) return -5;
        if (!Helper.可读条()) return -6;
        if (Skill.冰澈.RecentlyUsed(1500)) return -4;
        return 1;
    }

    public void Build(Slot slot)
    {
        var canTargetObjects = Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)>3 ? Skill.玄冰.最优aoe目标(Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)) : Core.Me.GetCurrTarget();
        Spell spell = Skill.玄冰.GetActionChange().GetSpell(QT.Instance.GetQt("智能AOE目标") ? canTargetObjects : Core.Me.GetCurrTarget());
        if (spell == null) return;
        slot.Add(spell);
    }
}