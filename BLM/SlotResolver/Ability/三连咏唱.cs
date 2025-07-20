using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.GCD;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 三连咏唱 : ISlotResolver
{
    private readonly uint _skillId = Skill.三连;
    private Spell? GetSpell()
    {
        return !_skillId.GetSpell().IsReadyWithCanCast() ? null : _skillId.GetSpell(SpellTargetType.Self);
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt(QTkey.三连咏唱)) return -2;
        if (_skillId.GetSpell().Charges < 1) return -1;
        if (QT.Instance.GetQt(QTkey.使用特供循环))
        {
            if (!BLMHelper.火状态) return -3;
            if (Skill.即刻.GetSpell().Cooldown.TotalSeconds < 3) return -6;
            if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 8) return -7;
            if (BattleData.Instance.能使用的火四个数 == 1 && !Helper.可瞬发() && BattleData.Instance.能使用耀星)
            {
                return 66;
            }
        }

        if (Helper.可瞬发()) return -4;
        if (BLMHelper.火状态)
        {
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
            {
                if (_skillId.GetSpell().Charges > 1.8 && GCDHelper.GetGCDCooldown() > 500 && !Skill.墨泉.GetSpell().AbilityCoolDownInNextXgcDsWindow(3)) return 22;
            }
        }
        if (BLMHelper.火状态 && BattleData.Instance.火循环剩余gcd < 2 &&
            Skill.墨泉.GetSpell().AbilityCoolDownInNextXgcDsWindow(3)) return -3;
        //if (Helper.IsMove &&!QT.Instance.GetQt("关闭即刻三连的移动判断") && BLMHelper.可用瞬发() != 0) return 2;
        if (BattleData.Instance.火循环剩余gcd<2 && Skill.即刻.GetSpell().Cooldown.TotalSeconds > 2 
                                           && BLMHelper.火状态 && !QT.Instance.GetQt(QTkey.三连用于走位)) return 3;
        if (BattleData.Instance.三连转冰 && !BLMHelper.悖论指示 && Core.Me.CurrentMp < 800)return 4;
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
