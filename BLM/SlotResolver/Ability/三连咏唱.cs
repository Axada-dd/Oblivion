using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.GCD;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 三连咏唱 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Skill.三连.GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("三连咏唱")) return -2;
        if (!Skill.三连.GetSpell().IsReadyWithCanCast()) return -1;
        if (QT.Instance.GetQt("使用特供循环"))
        {
            if (!BLMHelper.火状态) return -3;
            if (Skill.即刻.GetSpell().Cooldown.TotalSeconds < 3) return -6;
            if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 12) return -7;
            if (BattleData.Instance.能使用的火四个数 == 1 && !Helper.可瞬发() && BattleData.Instance.能使用耀星)
            {
                if (BLMHelper.悖论指示)
                {
                    BattleData.Instance.需要瞬发 = true;
                    return -55;
                }
                return 66;
            }
        }

        if (Helper.可瞬发()) return -4;
        if (BLMHelper.火状态 && BattleData.Instance.火循环剩余gcd < 2 &&
            Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 8) return -3;
        if (GCDHelper.Is2ndAbilityTime()) return -5;
        if (Helper.IsMove &&!QT.Instance.GetQt("关闭即刻三连的移动判断") && BLMHelper.可用瞬发() != 0) return 2;
        if (BattleData.Instance.火循环剩余gcd<2 && !BLMHelper.补dot &&
            Skill.即刻.GetSpell().Cooldown.TotalSeconds > 3 && BLMHelper.火状态 && !QT.Instance.GetQt("三连用于走位")) return 3;
        if (BattleData.Instance.三连转冰 && !BLMHelper.悖论指示 && Core.Me.CurrentMp < 800)return 4;
        return -99;
    }
}
