using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 即刻 : ISlotResolver
{
    private readonly uint _skillId = Skill.即刻;
    private Spell? GetSpell()
    {
        return  _skillId.GetSpell(SpellTargetType.Self);
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell != null) 
            slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt(QTkey.即刻)) return -2;
        if (_skillId.GetSpell().Cooldown.TotalMilliseconds > 0) return -1;
        if (QT.Instance.GetQt(QTkey.TTK)) return 999;
        if (Helper.可瞬发()) return -3;
        if (BLMHelper.火状态)
        {
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
            {
                if (BLMHelper.耀星层数 == 6) return 24;
            }
        }
        if (BLMHelper.冰状态 && BLMHelper.冰层数 < 3 )
        {
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
            {
                return -20;
            }

            if (Skill.冰三.RecentlyUsed() || Skill.冰冻.RecentlyUsed()) return -4;
            return 2;
        }
        
        return -99;
    }   
}
