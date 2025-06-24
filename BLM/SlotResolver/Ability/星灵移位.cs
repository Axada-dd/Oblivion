using Oblivion.BLM.SlotResolver.GCD;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 星灵移位 : ISlotResolver
{
    public int Check()
    {
        if (!Spells.星灵移位.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.冰状态 && !BLMHelper.火状态) return -2;
        if (Core.Me.Level < 90) return -5;
        //if (!BattleData.Instance.已使用瞬发) return -4;
        if (new 异言().Check() == 5) return -5;
        if (BLMHelper.火状态 && Core.Me.CurrentMp < 800 && Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 2) return -6;
        if (BLMHelper.冰状态 && Core.Me.HasAura(Buffs.火苗) && BLMHelper.冰层数 == 3 && BLMHelper.冰针 == 3 &&
            !BLMHelper.悖论指示) return 1;
        if (BLMHelper.冰状态 && !Core.Me.HasAura(Buffs.火苗) && BLMHelper.冰层数 == 3 && BLMHelper.冰针 == 3 &&
            !BLMHelper.悖论指示) return 5;
        if (BLMHelper.冰状态 && BLMHelper.冰层数 < 3 && !BLMHelper.悖论指示 && Core.Me.CurrentMp >= 800 &&
            Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 10) return 4;
        if (BLMHelper.火状态 && Core.Me.CurrentMp < 800 && Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 12 && BLMHelper.耀星层数 != 6) return 3;
        if (BLMHelper.火状态 && Core.Me.CurrentMp < 800 && (BattleData.Instance.可瞬发 || new 即刻().Check() == 1) && BLMHelper.耀星层数 != 6) return 2;
        return -99;
    }
    public void Build(Slot slot)
    {
        Spell spell = Spells.星灵移位.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }
}
