using System;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Oblivion.BLM.SlotResolver.Data;
using Oblivion.Common;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 悖论 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.悖论.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Spells.悖论.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.悖论指示) return -6;
        if (BLMHelper.冰状态 && BLMHelper.冰层数 == 3 && BLMHelper.冰针 == 3) return 1;
        if (BLMHelper.冰状态 && Helper.IsMove) return 2;
        if (BLMHelper.火状态 && BLMHelper.火层数 == 3 && BattleData.Instance.已使用耀星 && Core.Me.CurrentMp >= 2400L) return 3;
        if (BLMHelper.火状态 && BLMHelper.火层数 == 3 && BLMHelper.耀星层数 >= 3 && Helper.IsMove) return 4;
        if (BLMHelper.火状态 && BLMHelper.火层数 == 3 && Helper.IsMove) return 5;
        return -99;
    }
}
