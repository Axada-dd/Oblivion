using System;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using Oblivion.BLM.SlotResolver.Data;
using Oblivion.Common;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 黑魔纹 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.黑魔纹.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Spells.黑魔纹.GetSpell().IsReadyWithCanCast()) return -1;
        if(BattleData.Instance.已使用黑魔纹)return -3;
        if(!BattleData.Instance.已使用瞬发)return -2;
        return 1;
    }
}
