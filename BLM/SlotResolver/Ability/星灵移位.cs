using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.GCD;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 星灵移位 : ISlotResolver
{
    public int Check()
    {
        if (!Skill.星灵移位.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.冰状态 && !BLMHelper.火状态) return -2;
        if (Core.Me.Level < 90) return -5;
        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp < 800)
            {
                if (BLMHelper.耀星层数 == 6) return -4;
                if (Skill.墨泉.RecentlyUsed(1500)) return -7;
                if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 3) return -6;
                if (Helper.可瞬发() || Skill.即刻.GetSpell().Cooldown.TotalSeconds < 1.2) return 6;
            }
        }

        if (BLMHelper.冰状态)
        {
            if (BLMHelper.悖论指示 ) return -3;
            if (BLMHelper.冰层数 != 3) return -4;
            if (BLMHelper.冰针 != 3) return -6;
            if (Core.Me.CurrentMp < 10000 && !(Skill.冰澈.RecentlyUsed() || Skill.玄冰.RecentlyUsed())) return -7;
            if (GCDHelper.GetGCDCooldown() < 600 && !QT.Instance.GetQt("使用特供循环"))
            {
                BattleData.Instance.需要瞬发 = true;
                return -5;
            }
            return 1;
        }

        return -99;
    }
    public void Build(Slot slot)
    {
        Spell spell = Skill.星灵移位.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }
}
