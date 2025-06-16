using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 醒梦:ISlotResolver
{
    public int Check()
    {
        if (!QT.Instance.GetQt("醒梦")) return -2;
        if(Spells.墨泉.GetSpell().Cooldown.TotalSeconds<10&&new 星灵移位().Check()==3)return 1;
        return -99;
    }

    public void Build(Slot slot)
    {
        Spell spell = Spells.即刻.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }
}