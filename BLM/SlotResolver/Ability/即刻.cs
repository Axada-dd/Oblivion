using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 即刻 : ISlotResolver
{
    private readonly uint _skillId = Skill.即刻;
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
        if (!QT.Instance.GetQt(QTkey.即刻)) return -2;
        if (_skillId.GetSpell().Cooldown.TotalMilliseconds > 0) return -1;
        if (Helper.可瞬发()) return -3;
        if (BLMHelper.冰状态 && BLMHelper.冰层数 < 3 )
        {
            return 2;
        }
        
        return -99;
    }   
}
