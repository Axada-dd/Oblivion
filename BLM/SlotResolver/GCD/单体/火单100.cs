using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD.单体;

public class 火单100 :ISlotResolver
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
        if (BLMHelper.冰状态) return 0;
        if (!BLMHelper.火状态) return 0;
        if (!BLMHelper.火状态 && Helper.蓝量 > 8000) return Skill.火三;
        if (BLMHelper.火层数 < 3)
        {
            if (BLMHelper.有火苗)
                return Skill.火三;
            if (BLMHelper.悖论指示)
                return Skill.悖论;
        }

        if (BLMHelper.耀星层数 == 6) return Skill.耀星;
        if (BLMHelper.悖论指示)
        {
            if (Helper.蓝量 >= 2400 && Helper.蓝量 <= 3000 && !BattleData.Instance.压缩冰悖论) return Skill.悖论;
        }

        if (BattleData.Instance.压缩冰悖论 && Core.Me.CurrentMp <= 2800 && BLMHelper.耀星层数 < 5) return Skill.绝望;
        if (Helper.蓝量 < 2400 && BLMHelper.耀星层数 < 5 ) return Skill.绝望;
        return Skill.火四;
    }
    public int Check()
    {
        if (Core.Me.Level < 100) return -100;
        if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe()) return -234;
        if (Helper.IsMove&&!Helper.可瞬发()) return -99;
        _skillId = GetSkillId();
        if (_skillId == 0) return -1;
        return (int)_skillId;
    }
}