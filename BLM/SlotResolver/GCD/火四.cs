using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 火4 : ISlotResolver
{
    private readonly uint _skillId = Skill.火四;
    private Spell? GetSpell()
    {
        return !_skillId.GetSpell().IsReadyWithCanCast() ? null : _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount >= 2  && QT.Instance.GetQt("AOE")) return -3;
        if (!BLMHelper.火状态) return -6;
        if (BLMHelper.耀星层数 + BattleData.Instance.能使用的火四个数 < 6) return -7;
        if (BLMHelper.火层数 >= 3 && BLMHelper.耀星层数 < 6 && Core.Me.CurrentMp > 2400L && !Helper.IsMove) return 1;
        if (BLMHelper.火层数 >= 3 && BLMHelper.耀星层数 < 6 && Core.Me.CurrentMp > 2400L && Helper.可瞬发()) return 2;
        return -99;
    }
}
