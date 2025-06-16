using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 即刻 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.即刻.GetActionChange().GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("即刻")) return -2;
        if(!Spells.即刻.GetSpell().IsReadyWithCanCast())return -1;
        if(BLMHelper.火状态&&BattleData.Instance.已使用耀星&&BattleData.Instance.已使用瞬发&&Core.Me.CurrentMp<800&&GCDHelper.GetGCDCooldown()>1500)return 1;
        if(BLMHelper.冰状态&&BLMHelper.冰层数<3&&BattleData.Instance.已使用瞬发&&!BattleData.Instance.可瞬发)return 2;
        return -99;
    }
}
