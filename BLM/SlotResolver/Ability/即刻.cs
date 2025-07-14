using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 即刻 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Skill.即刻.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("即刻")) return -2;
        if (!Skill.即刻.GetSpell().IsReadyWithCanCast()) return -1;
        if (Helper.可瞬发()) return -3;
        if (BLMHelper.火状态 && BattleData.Instance.已使用耀星 && BattleData.Instance.已使用瞬发 && Core.Me.CurrentMp < 800 &&
            (Skill.墨泉.GetSpell().Cooldown.TotalSeconds >= 12 && !Skill.墨泉.RecentlyUsed(1500))) return 1;
        if (BLMHelper.冰状态 && BLMHelper.冰层数 < 3 && BattleData.Instance.已使用瞬发  && Skill.墨泉.GetSpell().Cooldown.TotalSeconds >= 12) return 2;
        if (Helper.IsMove && GCDHelper.Is2ndAbilityTime() && Skill.三连.GetSpell().Charges < 1.4 && !QT.Instance.GetQt("即刻不用于走位") && !QT.Instance.GetQt("关闭即刻三连的移动判断"))return 3;
        
        return -99;
    }   
}
