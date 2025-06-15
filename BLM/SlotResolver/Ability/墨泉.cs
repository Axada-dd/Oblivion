namespace Oblivion.BLM.SlotResolver.Ability;

public class 墨泉 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.墨泉.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Spells.墨泉.GetSpell().IsReadyWithCanCast()) return -1;
        if(!BLMHelper.火状态)return -2;
        if (Core.Me.CurrentMp > 800) return -3;
        if(!BattleData.Instance.已使用瞬发)return -4;
        return 1;
    }
}
