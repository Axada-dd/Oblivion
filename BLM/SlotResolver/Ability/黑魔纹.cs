using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 黑魔纹 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = new Spell(Spells.黑魔纹, Core.Me.Position);
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("黑魔纹")) return -5;
        if (!Spells.黑魔纹.GetSpell().IsReadyWithCanCast()) return -1;
        if(BattleData.Instance.已使用黑魔纹)return -3;
        if(!BattleData.Instance.已使用瞬发)return -2;
        return 1;
    }
}
