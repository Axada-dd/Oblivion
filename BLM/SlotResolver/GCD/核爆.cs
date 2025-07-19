using AEAssist.MemoryApi;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 核爆 : ISlotResolver
{
    private readonly uint _skillId = Skill.核爆;
    private Spell? GetSpell()
    {
        if (!_skillId.GetSpell().IsReadyWithCanCast()) return null;
        return QT.Instance.GetQt("智能AOE目标")? _skillId.GetSpellBySmartTarget() : _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {

        Spell? spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("AOE")) return -5;
        if (_skillId.GetSpell().MPNeed > Core.Me.CurrentMp) return -6;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -1;
        if (!BLMHelper.火状态) return -3;
        if (Helper.IsMove && !Helper.可瞬发()) return -4;
        return 1;
    }
}
