using AEAssist.MemoryApi;

namespace Oblivion.BLM.SlotResolver.GCD.单体;

public class 火单90 :ISlotResolver
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
            if (BLMHelper.火层数 < 3)
            {
                if (Helper.有buff(Buffs.火苗)) return Skill.火三;
                if (BLMHelper.悖论指示) return Skill.悖论;
                return Skill.火三;
            }

            if (Core.Me.CurrentMp >= 800 && Core.Me.CurrentMp < 2400) return Skill.绝望;
            if (Core.Me.CurrentMp >= 2400 && Core.Me.CurrentMp < 4000 && BLMHelper.悖论指示) return Skill.悖论;
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
        if (Core.Me.Level < 90 || Core.Me.Level >= 100) return -90;
        if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe()) return -234;
        _skillId = GetSkillId();
        if (_skillId == 0) return -1;
        return (int)_skillId;
    }
}