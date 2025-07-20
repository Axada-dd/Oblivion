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
        if (BLMHelper.双目标aoe()||BLMHelper.三目标aoe()) return -100;
        if (!BLMHelper.火状态) return -6;
        if (BLMHelper.火层数 >= 3 && BLMHelper.耀星层数 < 6 && Core.Me.CurrentMp > 2400L && !Helper.IsMove) return 1;
        if (BLMHelper.火层数 >= 3 && BLMHelper.耀星层数 < 6 && Core.Me.CurrentMp > 2400L && Helper.可瞬发()) return 2;
        return -99;
    }
}
