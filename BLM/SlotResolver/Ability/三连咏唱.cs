using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.GCD;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 三连咏唱 : ISlotResolver
{
    private readonly uint _skillId = Skill.三连;
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
        if (!QT.Instance.GetQt(QTkey.三连咏唱)) return -2;
        if (QT.Instance.GetQt(QTkey.三连用于走位)) return -5;
        if (_skillId.GetSpell().Charges < 1) return -1;
        if (QT.Instance.GetQt(QTkey.TTK)) return 999;
        if (Helper.可瞬发()) return -4;
        if (BLMHelper.火状态)
        {
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
            {
                if (Core.Me.CurrentMp < 800 && BLMHelper.耀星层数 == 6) return -22;
                return 23;
            }

            if (Skill.墨泉.技能CD() < 500 || Skill.墨泉.AbilityCoolDownInNextXgcDsWindow(5)) return -8;
            if (Core.Me.CurrentMp <= 4400 && BLMHelper.耀星层数 >= 5 && Core.Me.Level == 100&&Skill.即刻.技能CD()>0&&!Skill.即刻.AbilityCoolDownInNextXgcDsWindow(3)) return 1;
            if (Core.Me.CurrentMp <= 2800 && Core.Me.Level == 90 && Core.Me.CurrentMp >= 800&&Skill.即刻.技能CD()>0&&!Skill.即刻.AbilityCoolDownInNextXgcDsWindow(3)) return 1;
            if (Core.Me.CurrentMp <= 2800 && Core.Me.Level == 80&&Skill.即刻.技能CD()>0&&!Skill.即刻.AbilityCoolDownInNextXgcDsWindow(3)) return 1;
            //70级三连只用于走位，需要额外使用自行排轴
        }
        
        if (BLMHelper.冰状态 && BLMHelper.冰层数 < 3)
        {
            if (Core.Me.Level < 80) return -80;
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe()) return -234;
            if (BLMHelper.悖论指示 ) return -3;
            return 2;
        }
        return -99;
    }
}
