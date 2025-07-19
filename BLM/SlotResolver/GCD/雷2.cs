using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 雷2 : ISlotResolver
{
    private readonly uint _skillId = Skill.雷二;
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
        if (!QT.Instance.GetQt("AOE")) return -4;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -3;
        if (!QT.Instance.GetQt("Dot")) return -2;
        if (BattleData.Instance.正在特殊循环中) return -4;
        if (Core.Me.HasAura(Buffs.雷云) && BLMHelper.补dot) return 1;
        return -99;
    }
}
