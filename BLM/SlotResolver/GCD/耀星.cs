using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 耀星 : ISlotResolver
{
    private readonly uint _skillId = Skill.耀星;
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
        if (!BLMHelper.火状态) return -6;
        if (BLMHelper.耀星层数 != 6) return -5;

        if (BattleData.Instance.三连转冰 && Core.Me.CurrentMp < 800 && !BLMHelper.悖论指示) return 3;
        if (!Helper.IsMove) return 1;
        if (Helper.IsMove && Helper.可瞬发()) return 2;
        
        return -99;
    }
}
