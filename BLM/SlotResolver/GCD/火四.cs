using System;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Data;
using Oblivion.Common;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 火4 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.火四.GetActionChange().GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (!new Spell(Spells.火四, SpellTargetType.Target).IsReadyWithCanCast()) return -2;
        if (nearbyEnemyCount >= 2 && QT.Instance.GetQt("核爆")) return -3;
        //if (BLMHelper.天语剩余时间 < 4000) return -5;
        if (!BLMHelper.火状态) return -6;
        if (BLMHelper.火层数 >= 3 && BLMHelper.耀星层数 < 6 && Core.Me.CurrentMp > 2400L && !Helper.IsMove) return 1;
        if (BLMHelper.火层数 >= 3 && BLMHelper.耀星层数 < 6 && Core.Me.CurrentMp > 2400L && BattleData.Instance.可瞬发) return 2;
        return -99;
    }
}
