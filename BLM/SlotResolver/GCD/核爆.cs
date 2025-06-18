using AEAssist.MemoryApi;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 核爆 : ISlotResolver
{
    public void Build(Slot slot)
    {
        var canTargetObjects = Spells.核爆.最优aoe目标(2);
        Spell spell = Spells.核爆.GetActionChange().GetSpell(canTargetObjects);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -1;
        if (!Spells.核爆.GetSpell().IsReadyWithCanCast()) return -2;
        if (!BLMHelper.火状态) return -4;
        if (!QT.Instance.GetQt("核爆")) return -5;
        if (BLMHelper.耀星层数 < 6 && !BattleData.Instance.已使用耀星 && BLMHelper.冰针 >= 1 && (!Helper.IsMove || BattleData.Instance.可瞬发)) return 1;
        if (BLMHelper.耀星层数 < 6 && !BattleData.Instance.已使用耀星 && BattleData.Instance.前一gcd == Spells.核爆) return 2;
        return -99;
    }
}
