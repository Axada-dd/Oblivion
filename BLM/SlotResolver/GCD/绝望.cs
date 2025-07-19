using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 绝望 : ISlotResolver
{
    private readonly uint _skillId = Skill.绝望;
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
        if (nearbyEnemyCount >= 2 && QT.Instance.GetQt("核爆")&& QT.Instance.GetQt("AOE")) return -3;
        //if (!QT.Instance.GetQt("绝望")) return -4;
        if (!BLMHelper.火状态) return -6;
        if (BattleData.Instance.三连转冰)
        {
            if (BLMHelper.悖论指示) return -4;
            if (Core.Me.CurrentMp < 2400)return 5;
        }
        if (BLMHelper.火层数 == 3 && BattleData.Instance.已使用耀星 )
        {
            if (Core.Me.CurrentMp < 2400) return 3;
            if (!BattleData.Instance.能使用耀星) return 4;
        }
        if (BLMHelper.火层数 == 3 && BLMHelper.耀星层数 == 6 && Core.Me.CurrentMp < 2400 && MoveHelper.IsMoving()) return 1;
        return -99;
    }
}
