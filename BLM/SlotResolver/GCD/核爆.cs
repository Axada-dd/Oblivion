using System;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Data;
using Oblivion.Common;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 核爆 : ISlotResolver
{
    public void Build(Slot slot)
    {
        var canTargetObjects = Helper.最优aoe目标(Spells.核爆, 2);
        Spell spell = Spells.核爆.GetActionChange().GetSpell(canTargetObjects);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -1;
        if (!Spells.核爆.GetSpell().IsReadyWithCanCast()) return -2;
        if (Core.Me.Level < 60) return -3;
        if (!BLMHelper.火状态) return -4;
        if (!QT.Instance.GetQt("核爆")) return -5;
        if (BLMHelper.耀星层数 < 6 && !BattleData.Instance.已使用耀星 && BLMHelper.冰针 >= 1&&(!Helper.IsMove||BattleData.Instance.可瞬发)) return 1;
        if (BLMHelper.耀星层数 < 6 && !BattleData.Instance.已使用耀星 && BattleData.Instance.前一GCD == Spells.核爆) return 2;
        return -99;
    }
}
