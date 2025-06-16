namespace Oblivion.BLM.SlotResolver.GCD;

public class 冰三 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.冰三.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Spells.冰三.GetSpell().IsReadyWithCanCast()) return -1;
        if (BLMHelper.火状态 && Spells.墨泉.GetSpell().Cooldown.TotalSeconds > 10 && Core.Me.CurrentMp < 800 && !BattleData.Instance.可瞬发) return 1;
        if (BLMHelper.冰状态 && BLMHelper.冰层数 < 3 && BattleData.Instance.可瞬发) return 2;
        if (!BLMHelper.冰状态 && !BLMHelper.火状态 && Core.Me.CurrentMp < 5000) return 4;

        return -99;
    }
}
