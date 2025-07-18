using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 悖论 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Skill.悖论.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Skill.悖论.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.悖论指示) return -6;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount > 2 && QT.Instance.GetQt("AOE")) return -100;
        if (BLMHelper.冰状态)
        {
            if (BLMHelper.冰层数 == 3 && BLMHelper.冰针 == 3) return 1;
            //if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 8) return 3;
        }

        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp < 2400) return -2;
            if (BLMHelper.火层数 < 3 && !Core.Me.HasAura(Buffs.火苗)) return 4;
            if (BLMHelper.火层数 == 3)
            {
                if (BattleData.Instance.已使用耀星 || (BattleData.Instance.三连转冰 && BLMHelper.耀星层数 == 6)) return 6;
            }
        }
        return -99;
    }
}
