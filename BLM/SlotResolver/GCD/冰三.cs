using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 冰三 : ISlotResolver
{
    private readonly uint _skillId = Skill.冰三;
    private Spell? GetSpell()
    {
        return _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell != null) 
            slot.Add(spell);
    }
    public int Check()
    {
        if (BLMHelper.双目标aoe()||BLMHelper.三目标aoe()) return -100;
        if (Skill.墨泉.RecentlyUsed()) return -2;
        if (BLMHelper.冰状态)
        {
            if (Helper.可瞬发())
            {
                if (BLMHelper.冰层数 == 3) return -3;
                return 1;
            }
            if (BLMHelper.强制补冰())
            {
                return 2;
            }
        }

        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp >= 800) return -3;
            if (BattleData.Instance.能使用耀星) return -4;
            if (BLMHelper.耀星层数 == 6) return -5;
            if (Helper.可瞬发() || BLMHelper.能星灵转冰() || BattleData.Instance.三连转冰) return -6;
            if (Skill.墨泉.GetSpell().AbilityCoolDownInNextXgcDsWindow(2)) return -7;
            if (Skill.墨泉.RecentlyUsed()) return -8;
            return 3;
        }

        if (!BLMHelper.火状态 && !BLMHelper.冰状态 && Core.Me.CurrentMp < 8000) return 77;
        return -99;
    }
}
