namespace Oblivion.BLM.SlotResolver.GCD.单体;

public class 冰单70 : ISlotResolver
{
    private readonly uint _skillId = 0;
    private Spell? GetSpell()
    {
        return _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell != null) 
            slot.Add(spell);
    }

    private uint GetSkillId()
    {
        return 0;
    }
    public int Check()
    {
        if (Core.Me.Level != 100) return -100;
        if (GetSkillId() == 0) return -1;
        return (int)_skillId;
    }
}