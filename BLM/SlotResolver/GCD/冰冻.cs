using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 冰冻 : ISlotResolver
{
    public int Check()
    {
        if (!Skill.冰冻.GetActionChange().GetSpell().IsReadyWithCanCast()) return -1;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -2;
        if (!QT.Instance.GetQt("AOE")) return -3;
        if (BLMHelper.冰状态)
        {
            if (Helper.可瞬发())
            {
                if (BLMHelper.冰层数 == 3) return -3;
                return 1;
            }

            if (BattleData.Instance.强制补冰)
            {
                return 2;
            }
        }

        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp >= 800) return -3;
            if (BattleData.Instance.能使用耀星) return -4;
            if (BLMHelper.耀星层数 == 6) return -5;
            if (Helper.可瞬发() || BLMHelper.能星灵转冰() || BattleData.Instance.三连转冰) return -6;
            if (Skill.墨泉.RecentlyUsed()) return -7;
            return 3;
        }
        return -99;
    }

    public void Build(Slot slot)
    {
        var canTargetObjects = Core.Me.GetCurrTarget().目标周围可选中敌人数量(5) > 3 ? Skill.玄冰.最优aoe目标(Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)) : Core.Me.GetCurrTarget();
        Spell spell = Skill.冰冻.GetActionChange().GetSpell(QT.Instance.GetQt("智能AOE目标") ? canTargetObjects : Core.Me.GetCurrTarget());
        if (spell == null) return;
        slot.Add(spell);
    }
}