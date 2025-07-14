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
        if (nearbyEnemyCount > 2) return -1;
        if (BLMHelper.冰状态)
        {
            if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 6) return -2;
            if (BLMHelper.冰层数 == 3) return -3;
            if (!Helper.可瞬发()) return -4;
            return 1;
        }

        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp >= 800) return -5;
            if (BLMHelper.耀星层数 == 6) return -6;
            if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 6) return -7;
            if (BattleData.Instance.前一gcd == Skill.冰澈 || BattleData.Instance.前一gcd == Skill.玄冰) return -8;
            if (Skill.墨泉.RecentlyUsed()) return -9;
            if (Helper.可瞬发()) return -10;
            return 1;
        }
        if (Core.Me.CurrentMp < 5000) return 4;

        return -99;
    }
}
