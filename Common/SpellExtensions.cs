namespace Oblivion.Common;

public static class SpellExtensions
{
    public static Spell GetSpellBySmartTarget(this uint spellId)
    {
        var canTargetObjects = Core.Me.GetCurrTarget().目标周围可选中敌人数量(5) > 3 ? 
            spellId.最优aoe目标(Core.Me.GetCurrTarget().目标周围可选中敌人数量(5)) : Core.Me.GetCurrTarget();
        return spellId.GetSpell(canTargetObjects);
    }
}