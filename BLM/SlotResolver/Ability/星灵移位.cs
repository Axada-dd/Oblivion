using System;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Oblivion.BLM.SlotResolver.Data;
using Oblivion.Common;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 星灵移位 : ISlotResolver
{
    public int Check()
    {
        if (!Spells.星灵移位.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.冰状态 && !BLMHelper.火状态) return -2;
        if (Core.Me.Level < 90) return -5;
        if(!BattleData.Instance.已使用瞬发)return -4;
        if (BLMHelper.冰状态 && Core.Me.HasAura(Buffs.火苗) && BLMHelper.冰层数 == 3 && BLMHelper.冰针 == 3) return 1;
        if (BLMHelper.火状态 && Core.Me.CurrentMp < 800 && (BattleData.Instance.可瞬发||new 即刻().Check()==1)&&BattleData.Instance.已使用耀星) return 2;
        return -99;
    }
    public void Build(Slot slot)
    {
        Spell spell = Spells.星灵移位.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }
}
