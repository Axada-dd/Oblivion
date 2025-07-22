using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 核爆补耀星 : ISlotResolver
{
    private uint _skillId = 0;
    private Spell? GetSpell()
    {
        _skillId = UseSkill();
        if (_skillId == 0) return null;
        //if (!_skillId.GetSpell().IsReadyWithCanCast()) return null;
        if (_skillId.IsAoe()) return QT.Instance.GetQt(QTkey.智能aoe目标)  ? _skillId.GetSpellBySmartTarget() : _skillId.GetSpell();
        return  _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell != null) 
            slot.Add(spell);
    }
    public int Check()
    {
        if (!BLMHelper.火状态) return -1;
        if (Core.Me.CurrentMp < 800) return -2;
        if (BattleData.Instance.已使用耀星) return -3;
        if (BLMHelper.耀星层数 + BattleData.Instance.能使用的火四个数 < 6)
        {
            return 1;
        }
        //可以再精修，先计算火4个数，火4数>3且还有800+蓝可以先火4再核爆
        return -99;
    }

    private uint UseSkill()
    {
        if (BLMHelper.冰针 >= 1 && (int)Core.Me.CurrentMp * 0.333 >= 800) return Skill.核爆;
        if (BLMHelper.耀星层数 + 3 >= 6) return Skill.核爆;
        return Skill.绝望;
    }

}