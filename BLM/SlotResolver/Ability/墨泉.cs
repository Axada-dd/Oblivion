using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 墨泉 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Skill.墨泉.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("墨泉")) return -5;
        if (!Skill.墨泉.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.火状态) return -2;
        if (Core.Me.CurrentMp > 800) return -3;
        if (!BattleData.Instance.已使用瞬发 && !QT.Instance.GetQt("能力技卡G放")) return -4;
        if (BLMHelper.耀星层数 == 6 )return -6;
        return 1;
    }
}
