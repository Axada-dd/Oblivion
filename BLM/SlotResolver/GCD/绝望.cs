using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 绝望 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.绝望.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (!new Spell(Spells.绝望, SpellTargetType.Target).IsReadyWithCanCast()) return -2;
        if (nearbyEnemyCount >= 2 && QT.Instance.GetQt("核爆")&& QT.Instance.GetQt("AOE")) return -3;
        if (!QT.Instance.GetQt("绝望")) return -4;
        if (!BLMHelper.火状态) return -6;
        if (BLMHelper.火状态 && BLMHelper.火层数 == 3 && BLMHelper.耀星层数 == 6 && Core.Me.CurrentMp <= 1600 && Helper.IsMove && !BattleData.Instance.可瞬发) return 1;
        if (BLMHelper.火层数 == 3 && BattleData.Instance.已使用耀星 && Core.Me.CurrentMp <= 1600) return 3;
        if (BLMHelper.火状态 && BLMHelper.火层数 <= 3 && Core.Me.CurrentMp < 2000 && BLMHelper.耀星层数 < 6 && !Core.Me.HasAura(Buffs.火苗)) return 2;
        return -99;
    }
}
