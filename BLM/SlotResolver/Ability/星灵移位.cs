using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.GCD;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 星灵移位 : ISlotResolver
{
    private readonly uint _skillId = Skill.星灵移位;
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
        if (_skillId.GetSpell().Cooldown.TotalMilliseconds > 0) return -1;
        if (!BLMHelper.冰状态 && !BLMHelper.火状态) return -2;
        if (QT.Instance.GetQt(QTkey.TTK))
        {
            if (Core.Me.Level < 90) return -90;
            if (BLMHelper.火状态 && Core.Me.CurrentMp < 800) return 88;
            if (BLMHelper.冰状态 && !BLMHelper.悖论指示 ) return 99;
        }
        if (BLMHelper.火状态)
        {
            if (Skill.墨泉.AbilityCoolDownInNextXgcDsWindow(2)||Skill.墨泉.IsReady() || Skill.墨泉.RecentlyUsed()) return -66;
            if (Core.Me.CurrentMp >= 800) return -3;
            if (BLMHelper.耀星层数 == 6 && Core.Me.Level == 100) return -4;
            if (Helper.可瞬发()) return 1;
            if (Core.Me.Level < 90) return -90;
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe()) return 234;
            if (Core.Me.Level < 100 ) return -100;
            if (BattleData.Instance.三连走位 &&!Skill.即刻.AbilityCoolDownInNextXgcDsWindow(1) ) return -5;
            if (!Helper.可瞬发() && !Skill.即刻.AbilityCoolDownInNextXgcDsWindow(1) &&
                 Skill.三连.GetSpell().Charges < 1) return -5;
            return 1;
            
        }

        if (BLMHelper.冰状态)
        {
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
            {
                if (BLMHelper.冰针 != 3) return -6;
                if (_skillId.GetSpell().IsReadyWithCanCast()) return 21;
            }
            if (BLMHelper.悖论指示 && !BattleData.Instance.压缩冰悖论) return -3;
            if (BLMHelper.冰层数 != 3) return -4;
            if (BLMHelper.冰针 != 3) return -6;
            if (Core.Me.Level < 90)
            {
                if (Helper.有buff(Buffs.火苗))return 72;
                return -90;
            }
            if (Core.Me.Level<100&&BattleData.Instance.特供循环 && (new 开满转火().StartCheck() > 0 || BattleData.Instance.正在特殊循环中)) return -8;
            return 1;
        }

        return -99;
    }
}
