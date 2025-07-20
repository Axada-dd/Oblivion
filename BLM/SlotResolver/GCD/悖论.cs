using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 悖论 : ISlotResolver
{
    private readonly uint _skillId = Skill.悖论;
    private Spell? GetSpell()
    {
        return !_skillId.GetSpell().IsReadyWithCanCast() ? null : _skillId.GetSpell();
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!BLMHelper.悖论指示) return -6;
        
        if (BLMHelper.冰状态)
        {
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe())
            {
                if (BLMHelper.冰针 < 3) return -21;
                if (BattleData.Instance.Aoe循环填充) return -22;
                return 21;
            }

            if (new 开满转火().StartCheck() == -5) return 40;
            if (BLMHelper.冰层数 == 3 && BLMHelper.冰针 == 3) return 1;
            //if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 8) return 3;
        }

        if (BLMHelper.火状态)
        {
            if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe()) return -20;
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
