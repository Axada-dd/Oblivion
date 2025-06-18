using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 醒梦 : ISlotResolver
{
    public int Check()
    {
        if (!QT.Instance.GetQt("醒梦")) return -2;
        if (!Spells.醒梦.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BattleData.Instance.可瞬发 && BLMHelper.冰状态 && Core.Me.CurrentMp < 800 && Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 10) return 1;
        return -99;
    }

    public void Build(Slot slot)
    {
        Spell spell = Spells.醒梦.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }
}