using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 冰澈 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Skill.冰澈.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (QT.Instance.GetQt("使用特供循环") && new 开满转火().StartCheck() > 0 && new 开满转火().StopCheck(2) < 0) return -999;
        if (!Skill.冰澈.GetSpell().IsReadyWithCanCast()) return -1;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount > 2 && QT.Instance.GetQt("AOE")) return -100;
        if (BLMHelper.冰状态)
        {
            if (BLMHelper.冰层数 != 3) return -2;
            if (!Helper.可读条()) return -3;
            if (BLMHelper.冰针 == 3)
            {
                if (Skill.冰澈.RecentlyUsed(5000)) return -4;
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
