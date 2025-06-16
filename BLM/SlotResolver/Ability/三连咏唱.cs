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
        if (Helper.IsMove && BLMHelper.通晓层数 < 2 && !BattleData.Instance.可瞬发 && !(new 悖论().Check() > 0 ||
                new 绝望().Check() > 0 || new 雷1().Check() > 0 || new 雷2().Check() > 0)) return 2;
        if(BattleData.Instance.火循环剩余gcd小于3&&(new 雷1().Check() > 0 || new 雷2().Check() > 0))return 3;
        if(BLMHelper.火状态&&Spells.三连.GetSpell().Charges*60>=110)return 1;
        return -99;
    }
}
