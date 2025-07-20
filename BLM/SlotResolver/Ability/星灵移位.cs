using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.GCD;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 星灵移位 : ISlotResolver
{
    private readonly uint _skillId = Skill.星灵移位;
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
        if (_skillId.GetSpell().Cooldown.TotalMilliseconds > 0) return -1;
        if (!BLMHelper.冰状态 && !BLMHelper.火状态) return -2;
        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp < 800)
            {
                if (BLMHelper.耀星层数 == 6) return -4;
                if (Skill.墨泉.RecentlyUsed(1500)) return -7;
                if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
                {
                    if (Skill.墨泉.GetSpell().IsReadyWithCanCast()) return -6;
                    return 22;
                }
                if (Skill.墨泉.GetSpell().AbilityCoolDownInNextXgcDsWindow(2) && BLMHelper.可用瞬发() != 0) return -6;
                if (Helper.可瞬发() ) return 6;
                if (BLMHelper.能星灵转冰()) return 7;
                if (BLMHelper.冰针 >1) return 787;
            }
        }

        if (BLMHelper.冰状态)
        {
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
            {
                if (BLMHelper.冰针 != 3) return -6;
                if (_skillId.GetSpell().IsReadyWithCanCast()) return 21;
            }
            if (BLMHelper.悖论指示) return -3;
            if (BLMHelper.冰层数 != 3) return -4;
            if (BLMHelper.冰针 != 3) return -6;
            if (Core.Me.CurrentMp < 9800 && !(Skill.冰澈.RecentlyUsed(5000) || Skill.玄冰.RecentlyUsed(5000))) return -7;
            if (QT.Instance.GetQt(QTkey.使用特供循环) && (new 开满转火().StartCheck() > 0 || BattleData.Instance.正在特殊循环中)) return -8;
            return 1;
        }

        return -99;
    }
}
