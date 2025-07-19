using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 玄冰: ISlotResolver
{
    private readonly uint _skillId = Skill.玄冰;
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
        if (!QT.Instance.GetQt("AOE")) return -1;
        if (!BLMHelper.冰状态) return -2;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -4;
        if (Core.Me.CurrentMp >= 10000) return -5;
        if (Skill.冰澈.RecentlyUsed(3000) || Skill.玄冰.RecentlyUsed(3000)) return -6;
        if (BLMHelper.冰状态)
        {
            if (BLMHelper.冰层数 != 3) return -2;
            if (!Helper.可读条()) return -3;
            if (BLMHelper.冰针 == 3)
            {
                if (Core.Me.CurrentMp < 10000)
                {
                    return 2;
                }
            }
        }
        return 1;
    }
    
}