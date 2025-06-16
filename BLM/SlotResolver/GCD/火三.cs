namespace Oblivion.BLM.SlotResolver.GCD;

public class 火三 : ISlotResolver
{
    public int Check()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (!new Spell(Spells.火三, SpellTargetType.Target).IsReadyWithCanCast()) return -2;
        if (nearbyEnemyCount >= 3) return -3;
        if (BLMHelper.火状态 && !Core.Me.HasAura(Buffs.火苗)) return -5;
        if (BLMHelper.火状态 && Core.Me.HasAura(Buffs.火苗) && BLMHelper.火层数 < 3) return 3;
        if (BLMHelper.冰状态 && !Core.Me.HasAura(Buffs.火苗) && BLMHelper.冰层数 >= 3 && BLMHelper.冰针 >= 3 && Core.Me.CurrentMp >= 10000) return 1;
        //if (BLMHelper.火状态 && Core.Me.HasAura(Buffs.火苗) && BLMHelper.天语剩余时间 < 3000) return 4;
        if (!BLMHelper.火状态 && !BLMHelper.冰状态 && Core.Me.CurrentMp >= 10000) return 2;
        if (BLMHelper.火状态 && Core.Me.HasAura(Buffs.火苗) && BLMHelper.火层数 < 3) return 4;
        return -1;
    }

    private Spell GetSpell() => Spells.火三.GetActionChange().GetSpell(SpellTargetType.Target);
    public void Build(Slot slot)
    {
        Spell spell = this.GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }
}