using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.GCD;

public class 异言 : ISlotResolver
{
    private readonly uint _skillId = Skill.异言;
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
        if (!QT.Instance.GetQt("异言")) return -2;
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
        if (nearbyEnemyCount >= 2 && QT.Instance.GetQt("AOE")) return -2;
        if (QT.Instance.GetQt("倾泻资源")) return 666;
        if (BLMHelper.通晓层数 == 3 && BLMHelper.通晓剩余时间 <= 10000) return 2;
        if (BLMHelper.通晓层数 == 3 && Skill.详述.GetSpell().Cooldown.TotalSeconds < 5) return 3;
        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp < 800 && BLMHelper.耀星层数 != 6)
            {
                if (Skill.墨泉.GetSpell().Cooldown.TotalMilliseconds < 300) return -3;
                if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 3 && Skill.墨泉.GetSpell().Cooldown.TotalSeconds > 0) return 4;
            }
        }

        return -99;
    }
}
