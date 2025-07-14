namespace Oblivion.BLM.SlotResolver.GCD;

public class 悖论 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Skill.悖论.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Skill.悖论.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.悖论指示) return -6;
        if (BLMHelper.冰状态)
        {
            if (BLMHelper.冰层数 == 3 && BLMHelper.冰针 == 3) return 1;
            if (Helper.IsMove && !Helper.可瞬发()) return 2;
            if (Skill.墨泉.GetSpell().Cooldown.TotalMilliseconds < Helper.复唱时间() * 3) return 3;
        }

        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp < 2400) return -2;
            if (BLMHelper.火层数 < 3 && !Core.Me.HasAura(Buffs.火苗)) return 4;
            if (BLMHelper.火层数 == 3)
            {
                if (Helper.IsMove && !Helper.可瞬发()) return 5;
                if (BattleData.Instance.已使用耀星 || (BattleData.Instance.使用三连转冰 && BLMHelper.耀星层数 == 6)) return 6;
            }
        }
        return -99;
    }
}
