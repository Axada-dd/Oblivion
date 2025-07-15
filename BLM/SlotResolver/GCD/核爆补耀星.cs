using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 核爆补耀星 : ISlotResolver
{
    public int Check()
    {
        if (!BLMHelper.火状态) return -1;
        if (Core.Me.CurrentMp < 800) return -2;
        if (BattleData.Instance.已使用耀星) return -3;
        if (BLMHelper.耀星层数 + BattleData.Instance.能使用的火四个数 < 6)
        {
            return 1;
        }

        return -99;
    }

    private uint UseSkill()
    {
        if (BLMHelper.冰针 >= 1 && (int)Core.Me.CurrentMp * 0.333 >= 800) return Skill.核爆;
        if (BLMHelper.耀星层数 + 3 >= 6) return Skill.核爆;
        return Skill.绝望;
    }
    public void Build(Slot slot)
    {
        var canTargetObjects = Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)>2 ? Skill.核爆.最优aoe目标(2) : Core.Me.GetCurrTarget();
        Spell spell = UseSkill().GetActionChange().GetSpell(QT.Instance.GetQt("智能AOE目标") ? canTargetObjects : Core.Me.GetCurrTarget());
        if (spell == null) return;
        slot.Add(spell);
    }
}