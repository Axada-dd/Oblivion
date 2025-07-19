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
        if (!QT.Instance.GetQt("即刻")) return -2;
        if (_skillId.GetSpell().Cooldown.TotalMilliseconds > 0) return -1;
        if (Helper.可瞬发()) return -3;
        if (BLMHelper.冰状态 && BLMHelper.冰层数 < 3 )
        {
            if(BattleData.Instance.需要瞬发gcd) return -4;
            if (GCDHelper.GetGCDCooldown() < 600)
            {
                BattleData.Instance.需要瞬发gcd = true;
                return -4;
            }
            return 2;
        }
        //if (Helper.IsMove && GCDHelper.Is2ndAbilityTime() && Skill.三连.GetSpell().Charges < 1.4 && !QT.Instance.GetQt("即刻不用于走位") && !QT.Instance.GetQt("关闭即刻三连的移动判断"))return 3;
        
        return -99;
    }   
}
