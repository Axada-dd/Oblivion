using System;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Oblivion.Common;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 耀星 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = 3577U.GetActionChange().GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!3577U.GetSpell().IsReadyWithCanCast()) return -1;
        if (Core.Me.Level < 96) return -5;
        if (!BLMHelper.火状态) return -6;
        if(BLMHelper.耀星层数 == 6&&!Core.Me.IsMoving()) return 1;
        if (BLMHelper.耀星层数 == 6 && Core.Me.IsMoving() && BattleData.Instance.可瞬发) return 2;
        return -99;
    }
}
