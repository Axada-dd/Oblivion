using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 爆发药 : ISlotResolver
{
    
    public int Check()
    {
        if (!QT.Instance.GetQt("爆发药")) return -2;
        if (!ItemHelper.CheckCurrJobPotion()) return -3;
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(Spell.CreatePotion());
    }
}