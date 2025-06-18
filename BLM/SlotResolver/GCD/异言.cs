using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 异言 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.异言.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("异言")) return -2;
        if (!Spells.异言.GetSpell().IsReadyWithCanCast()) return -1;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount >= 2) return -2;
        if (QT.Instance.GetQt("倾泻资源")) return 666;
        if (BLMHelper.火状态 && Core.Me.CurrentMp < 800 && Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 4) return 5;
        if (BLMHelper.通晓层数 == 3 && BLMHelper.通晓剩余时间 <= 10000) return 2;
        if ((BLMHelper.通晓层数 == 3 || (BLMHelper.通晓剩余时间 < 5000 && BLMHelper.通晓层数 == 2)) && Spells.详述.GetSpell().Cooldown.TotalMilliseconds < 10000) return 3;
        if (MoveHelper.IsMoving() && !BattleData.Instance.可瞬发) return 1;
        if (BLMHelper.冰状态 && !BattleData.Instance.可瞬发 && BLMHelper.冰层数 < 3 && Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 10 && !BLMHelper.悖论指示) return 4;
        if (BLMHelper.火状态 && BattleData.Instance.已使用耀星 && BattleData.Instance.已使用瞬发 && Core.Me.CurrentMp < 800 &&
            Spells.即刻.GetSpell().Cooldown.TotalMilliseconds < 2200) return 5;
        if (BLMHelper.火状态 && Core.Me.CurrentMp < 800 && Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 2) return 6;
        if (BLMHelper.火状态 && Core.Me.CurrentMp < 800 && Spells.即刻.GetSpell().Cooldown.TotalSeconds < 2 &&
            Spells.即刻.GetSpell().Cooldown.TotalSeconds > 0) return 7;
        return -99;
    }
}
