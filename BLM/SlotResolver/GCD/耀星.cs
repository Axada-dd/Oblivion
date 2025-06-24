namespace Oblivion.BLM.SlotResolver.GCD;

public class 耀星 : ISlotResolver
{
    public void Build(Slot slot)
    {
        var canTargetObjects = Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)>2 ? Spells.耀星.最优aoe目标(3) : Core.Me.GetCurrTarget();
        Spell spell = Spells.耀星.GetActionChange().GetSpell(canTargetObjects);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!Spells.耀星.GetSpell().IsReadyWithCanCast()) return -1;
        if (!BLMHelper.火状态) return -6;
        if (BLMHelper.耀星层数 != 6) return -5;

        if (BattleData.Instance.使用三连转冰 && Core.Me.CurrentMp < 800 && BLMHelper.悖论指示) return 3;
        if (!Helper.IsMove) return 1;
        if (Helper.IsMove && BattleData.Instance.可瞬发) return 2;
        
        return -99;
    }
}
