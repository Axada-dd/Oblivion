using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 火三 : ISlotResolver
{
    private readonly uint _skillId = Skill.火三;
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
        if (BLMHelper.火状态)
        {
            if (BLMHelper.火层数 < 3 && Core.Me.HasAura(Buffs.火苗)) return 1;
        }

        if (BLMHelper.冰状态)
        {
            if (Core.Me.HasAura(Buffs.火苗)) return -4;
            if (BLMHelper.冰层数 < 3) return -5;
            if (BLMHelper.冰针 < 3) return -6;
            if (!Skill.冰澈.RecentlyUsed() || !Skill.玄冰.RecentlyUsed() || Core.Me.CurrentMp < 10000) return -7;
            return 1;
        }
        if (!BLMHelper.火状态 && !BLMHelper.冰状态 && Core.Me.CurrentMp >= 8000) return 2;
        return -1;
    }

}