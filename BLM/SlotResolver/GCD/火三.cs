using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 火三 : ISlotResolver
{
    public int Check()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount >= 3 && QT.Instance.GetQt("AOE")) return -3;
        if (!new Spell(Skill.火三, SpellTargetType.Target).IsReadyWithCanCast()) return -2;
        if (BLMHelper.火状态)
        {
            if (BLMHelper.火层数 < 3 && Core.Me.HasAura(Buffs.火苗)) return 1;
        }

        if (BLMHelper.冰状态)
        {
            if (Core.Me.HasAura(Buffs.火苗)) return -4;
            if (BLMHelper.冰层数 < 3) return -5;
            if (BLMHelper.冰针 < 3) return -6;
            if (!Skill.冰澈.RecentlyUsed() || !Skill.玄冰.RecentlyUsed() || Core.Me.CurrentMp < 10000) return -7;
            return 1;
        }
        if (!BLMHelper.火状态 && !BLMHelper.冰状态 && Core.Me.CurrentMp >= 5000) return 2;
        return -1;
    }

    public void Build(Slot slot)
    {
        Spell spell = Skill.火三.GetActionChange().GetSpell(SpellTargetType.Target);;
        if (spell == null) return;
        slot.Add(spell);
    }
}