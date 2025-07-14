using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 异言 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Skill.异言.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("异言")) return -2;
        if (!Skill.异言.GetSpell().IsReadyWithCanCast()) return -1;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount >= 2 && QT.Instance.GetQt("AOE")) return -2;
        if (QT.Instance.GetQt("倾泻资源")) return 666;
        if (MoveHelper.IsMoving() && !Helper.可瞬发()) return 1;
        if (BLMHelper.通晓层数 == 3 && BLMHelper.通晓剩余时间 <= 10000) return 2;
        if (BLMHelper.通晓层数 == 3 && Skill.详述.GetSpell().Cooldown.TotalSeconds < 5) return 3;
        var 复唱时间 = Helper.复唱时间();
        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp < 800 && BLMHelper.耀星层数 != 6)
            {
                if (Skill.墨泉.GetSpell().IsReadyWithCanCast()) return -3;
                if (Skill.墨泉.GetSpell().Cooldown.TotalMilliseconds < 复唱时间) return 4;
            }
        }

        if (BLMHelper.冰状态)
        {
            if (Skill.墨泉.GetSpell().IsReadyWithCanCast()) return -3;
            if (Skill.墨泉.GetSpell().Cooldown.TotalMilliseconds > 复唱时间 * 3.0) return -4;
            if (BLMHelper.悖论指示) return -5;
            return 5;
        }
        //if ((BLMHelper.通晓层数 == 3 || (BLMHelper.通晓剩余时间 < 5000 && BLMHelper.通晓层数 == 2)) && Skill.详述.GetSpell().Cooldown.TotalMilliseconds < 10000) return 3;
        //if (BLMHelper.冰状态 && !Helper.可瞬发() && BLMHelper.冰层数 < 3 && Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 10 && !BLMHelper.悖论指示) return 4;

        return -99;
    }
}
