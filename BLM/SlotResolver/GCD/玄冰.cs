using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 玄冰: ISlotResolver
{
    private readonly uint _skillId = Skill.玄冰;
    private Spell? GetSpell()
    {
        if (!_skillId.GetSpell().IsReadyWithCanCast()) return null;
        return QT.Instance.GetQt(QTkey.智能AOE目标)? _skillId.GetSpellBySmartTarget() : _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {

        Spell? spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }
    public int Check()
    {
        if (!BLMHelper.冰状态) return -1;
        if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
        {
            if (BLMHelper.冰针 < 3) return 1;
            if (BattleData.Instance.三冰针进冰) return 2;
        }
        return -99;
    }
    
}