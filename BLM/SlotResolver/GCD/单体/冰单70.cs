namespace Oblivion.BLM.SlotResolver.GCD.单体;

public class 冰单70 : ISlotResolver
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
        if (BLMHelper.冰状态)
        {
            if (BLMHelper.冰层数 < 3) return Skill.冰三;
            if (BLMHelper.冰针 < 3) return Skill.冰澈;
            return 0;
        }

        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp < 1600) return Skill.冰三;
        }
        if (!BLMHelper.冰状态 && !BLMHelper.火状态) return Skill.冰三;
        return 0;
    }
    public int Check()
    {
        if (Core.Me.Level < 70 || Core.Me.Level >= 80) return -80;
        if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe()) return -234;
        if (Helper.IsMove&&!Helper.可瞬发()) return -99;
        _skillId = GetSkillId();
        if (_skillId == 0) return -1;
        return (int)_skillId;
    }
}