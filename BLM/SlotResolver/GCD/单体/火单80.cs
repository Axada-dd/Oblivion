namespace Oblivion.BLM.SlotResolver.GCD.单体;

public class 火单80 : ISlotResolver
{
    private uint _skillId = 0;
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
        if (BLMHelper.火状态)
        {
            if (BLMHelper.火层数 <3) return Skill.火三;
            if (Core.Me.CurrentMp >= 800 && Core.Me.CurrentMp < 2400) return Skill.绝望;
            return Skill.火四;
        }

        if (BLMHelper.冰状态)
        {
            if (BLMHelper.冰层数 == 3 && BLMHelper.冰针 == 3) return Skill.火三;
        }
        return 0;
    }
    public int Check()
    {
        if (Core.Me.Level < 80 || Core.Me.Level >= 90) return -80;
        if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe()) return -234;
        _skillId = GetSkillId();
        if (_skillId == 0) return -1;
        return (int)_skillId;
    }
}