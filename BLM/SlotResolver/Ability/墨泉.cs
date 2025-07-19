using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 墨泉 : ISlotResolver
{
    private readonly uint _skillId = Skill.墨泉;
    private Spell? GetSpell()
    {
        return !_skillId.GetSpell().IsReadyWithCanCast() ? null : _skillId.GetSpell(SpellTargetType.Self);
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("墨泉")) return -5;
        if (_skillId.GetSpell().Cooldown.TotalMilliseconds > 0) return -1;
        if (!BLMHelper.火状态) return -2;
        if (Core.Me.CurrentMp > 800) return -3;
        if (!BattleData.Instance.已使用瞬发 && !QT.Instance.GetQt("能力技卡G放")) return -4;
        if (BLMHelper.耀星层数 == 6 )return -6;
        return 1;
    }
}
