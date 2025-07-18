using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 冰三 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Skill.冰三.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Skill.冰三.GetSpell().IsReadyWithCanCast()) return -1;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount > 2 && QT.Instance.GetQt("AOE")) return -100;
        if (Skill.墨泉.RecentlyUsed()) return -2;
        if (BLMHelper.冰状态)
        {
            if (Helper.可瞬发())
            {
                if (BLMHelper.冰层数 == 3) return -3;
                return 1;
            }
            else
            {
                if (BattleData.Instance.需要即刻) return -4;
                if (Skill.即刻.IsReady() || Skill.三连.GetSpell().Charges >= 1)
                {
                    BattleData.Instance.需要即刻 = true;
                    BattleData.Instance.需要瞬发gcd = true;
                    return -5;
                }
            }

            if (BLMHelper.强制补冰())
            {
                return 2;
            }
        }

        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp >= 800) return -3;
            if (BattleData.Instance.能使用耀星) return -4;
            if (BLMHelper.耀星层数 == 6) return -5;
            if (Helper.可瞬发() || BLMHelper.能星灵转冰() || BattleData.Instance.三连转冰) return -6;
            if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 3) return -7;
            if (Skill.墨泉.RecentlyUsed()) return -8;
            return 3;
        }

        if (!BLMHelper.火状态 && !BLMHelper.冰状态 && Core.Me.CurrentMp < 8000) return 77;
        return -99;
    }
}
