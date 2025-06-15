using System;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using Oblivion.BLM.SlotResolver.Data;
using Oblivion.Common;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 三连咏唱 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.三连.GetActionChange().GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        return -99;
    }
}
