namespace Oblivion.BLM.SlotResolver.Ability;

public class 即刻三连 : ISlotResolver
{
    private uint _skillId = 0;
    public int Check()
    {
        if (BattleData.Instance.需要即刻)
        {
            _skillId = SkillId();
            if (_skillId == 0) return -1;
            return 1;
        }

        return -99;
    }

    private uint SkillId()
    {
        if (Helper.可瞬发()) return 0;
        if (Skill.三连.GetSpell().Charges >= 1) return Skill.三连;
        if (Skill.即刻.GetSpell().Cooldown.TotalSeconds == 0) return Skill.即刻;
        return 0;
    }
    public void Build(Slot slot)
    {
        Spell spell = _skillId.GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
        BattleData.Instance.需要即刻 = false;
    }
}