namespace Oblivion.BLM.SlotResolver.GCD;

public class 异言 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.异言.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Spells.异言.GetSpell().IsReadyWithCanCast()) return -1;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount >= 2) return -2;
        if (MoveHelper.IsMoving()) return 1;
        if(BLMHelper.通晓层数==3&&BLMHelper.通晓剩余时间<=4000)return 2;
        if(BLMHelper.通晓层数==3&&Spells.详述.GetSpell().Cooldown.TotalMilliseconds<4000)return 3;
        return -99;
    }
}
