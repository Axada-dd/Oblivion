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
        if (!QT.Instance.GetQt(QTkey.三连用于走位)) return -5;
        if (_skillId.GetSpell().Charges < 1) return -1;
        if (Helper.可瞬发()) return -4;
        if (BLMHelper.火状态)
        {
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
            {
                if (_skillId.GetSpell().Charges > 1.8 && GCDHelper.GetGCDCooldown() > 500 && !Skill.墨泉.GetSpell().AbilityCoolDownInNextXgcDsWindow(3)) return 22;
            }

            if (QT.Instance.GetQt(QTkey.使用特供循环))
            {
                if (Skill.即刻.GetSpell().Cooldown.TotalSeconds < 3) return -6;
                if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 8) return -7;
                if (BattleData.Instance.能使用的火四个数 == 1  && BattleData.Instance.能使用耀星)
                {
                    return 66;
                }
            }
            if (BattleData.Instance.三连转冰 && !BLMHelper.悖论指示 && Core.Me.CurrentMp < 800)return 4;
            return -5;
        }
        
        if (BLMHelper.冰状态 && BLMHelper.冰层数 < 3)
        {
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
            {
                return -20;
            }
            if (Skill.即刻.GetSpell().AbilityCoolDownInNextXgcDsWindow(1)) return -5;
            return 6;
        }
        return -99;
    }
}
