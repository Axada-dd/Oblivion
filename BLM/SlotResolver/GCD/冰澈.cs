using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 冰澈 : ISlotResolver
{
    private readonly uint _skillId = Skill.冰澈;
    private Spell? GetSpell()
    {
        return !_skillId.GetSpell().IsReadyWithCanCast() ? null : _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {
        Spell? spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (QT.Instance.GetQt("使用特供循环") && new 开满转火().StartCheck() > 0 && new 开满转火().StopCheck(2) < 0) return -999;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount > 2 && QT.Instance.GetQt("AOE")) return -100;
        if (Skill.冰澈.RecentlyUsed(3000) || Skill.玄冰.RecentlyUsed(3000)) return -2;
        if (Core.Me.CurrentMp >= 9800) return -3;
        if (BLMHelper.冰状态)
        {
            if (BLMHelper.冰层数 != 3) return -2;
            if (!Helper.可读条()) return -3;
            if (BLMHelper.冰针 == 3)
            {
                if (Skill.冰澈.RecentlyUsed(5000) || Skill.玄冰.RecentlyUsed(5000)) return -4;
                if (Core.Me.CurrentMp < 10000)
                {
                    return 2;
                }
            }
            return 1;
        }
        return -99;
    }
}
