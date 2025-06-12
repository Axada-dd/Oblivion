using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 爆发药:ISlotResolver
{
    public int Check()
    {
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Spell.CreatePotion());
    }
}