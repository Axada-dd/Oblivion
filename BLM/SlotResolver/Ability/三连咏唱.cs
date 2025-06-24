using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.GCD;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 三连咏唱 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.三连.GetSpell(SpellTargetType.Self);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("三连咏唱")) return -2;
        if (!Spells.三连.GetSpell().IsReadyWithCanCast()) return -1;
        if (BLMHelper.火状态 && BattleData.Instance.火循环剩余gcd<2 && Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 10) return -3;
        if (BattleData.Instance.可瞬发) return -4;
        if (GCDHelper.Is2ndAbilityTime()) return -5;
        if (Helper.IsMove && BLMHelper.通晓层数 < 2  && !(new 悖论().Check() > 0 ||
                new 绝望().Check() > 0 || new 雷1().Check() > 0 || new 雷2().Check() > 0 || new 异言().Check() > 0)) return 2;
        if (BattleData.Instance.火循环剩余gcd<2 && (new 雷1().Check() < 0 || new 雷2().Check() < 0) &&
            Spells.即刻.GetSpell().Cooldown.TotalSeconds > 3 && BLMHelper.火状态 && !QT.Instance.GetQt("三连用于走位")) return 3;
        //if (BLMHelper.火状态 && Spells.三连.GetSpell().Charges * 60 >= 110 && !QT.Instance.GetQt("三连用于走位")) return 1;
        if (BattleData.Instance.使用三连转冰 && !BLMHelper.悖论指示 && Core.Me.CurrentMp < 800)return 4;
        return -99;
    }
}
