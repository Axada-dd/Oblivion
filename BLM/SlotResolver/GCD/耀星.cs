using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 耀星 : ISlotResolver
{
    public void Build(Slot slot)
    {
        var canTargetObjects = Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)>2 ? Skill.耀星.最优aoe目标(Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)) : Core.Me.GetCurrTarget();
        Spell spell = Skill.耀星.GetActionChange().GetSpell(QT.Instance.GetQt("智能AOE目标") ? canTargetObjects : Core.Me.GetCurrTarget());
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Skill.耀星.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.火状态) return -6;
        if (BLMHelper.耀星层数 != 6) return -5;

        if (BattleData.Instance.三连转冰 && Core.Me.CurrentMp < 800 && BLMHelper.悖论指示) return 3;
        if (!Helper.IsMove) return 1;
        if (Helper.IsMove && Helper.可瞬发()) return 2;
        
        return -99;
    }
}
