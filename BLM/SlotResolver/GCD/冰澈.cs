using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 冰澈 : ISlotResolver
{
    private readonly uint _skillId = Skill.冰澈;
    private Spell? GetSpell()
    {
        return  _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell != null)
        {
            slot.Add(spell);
            BattleData.Instance.三冰针进冰 = false;
        }
    }

    public int Check()
    {
        if (QT.Instance.GetQt(QTkey.使用特供循环) && new 开满转火().StartCheck() > 0 && new 开满转火().StopCheck(2) < 0) return -999;
        if (BLMHelper.双目标aoe() || BLMHelper.三目标aoe()) return -55;
        if (Skill.玄冰.RecentlyUsed(3000)) return -2;
        if (Core.Me.CurrentMp >= 9800) return -3;
        if (BLMHelper.冰状态)
        {
            if (BLMHelper.冰层数 != 3) return -2;
            if (!Helper.可读条()) return -3;
            if (BLMHelper.冰针 < 3) return 1;
            if (BattleData.Instance.三冰针进冰) return 2;
        }
        return -99;
    }
}
