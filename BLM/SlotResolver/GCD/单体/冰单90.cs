namespace Oblivion.BLM.SlotResolver.GCD.单体;

public class 冰单90 :ISlotResolver
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
            if (BLMHelper.冰针 < 3 ) return Skill.冰澈;
            if (BLMHelper.悖论指示) return Skill.悖论;
        }

        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp < 800) return Skill.冰三;
        }
        return 0;
    }
    public int Check()
    {
        if (Core.Me.Level < 90 || Core.Me.Level >= 100) return -90;
        if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe()) return -234;
        if (Helper.IsMove&&!Helper.可瞬发()) return -99;
        _skillId = GetSkillId();
        if (_skillId == 0) return -1;
        return (int)_skillId;
    }
}