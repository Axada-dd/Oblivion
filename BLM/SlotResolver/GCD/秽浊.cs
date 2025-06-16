using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 秽浊 : ISlotResolver
{
    public void Build(Slot slot)
    {
        Spell spell = Spells.秽浊.GetActionChange().GetSpell(SpellTargetType.Target);
        if (spell == null) return;
        slot.Add(spell);
    }

    public int Check()
    {
        if (!QT.Instance.GetQt("秽浊")) return -5;
        if (!Spells.秽浊.GetSpell().IsReadyWithCanCast()) return -1;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount < 2) return -2;
        if (QT.Instance.GetQt("倾泻资源")) return 666;
        if (BLMHelper.火状态 && Core.Me.CurrentMp < 800 && Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 4) return 5;
        if(BLMHelper.通晓层数==3&&BLMHelper.通晓剩余时间<=6000)return 2;
        if(BLMHelper.通晓层数==3&&Spells.详述.GetSpell().Cooldown.TotalMilliseconds<6000)return 3;
        if (MoveHelper.IsMoving()&&!BattleData.Instance.可瞬发) return 1;
        if (new 悖论().Check() == 6) return 4;
        return -99;
    }
}
