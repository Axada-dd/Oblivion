using Oblivion.BLM.QtUI;
using Oblivion.BLM.SlotResolver.GCD;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.SlotResolver.Ability;

public class 星灵移位 : ISlotResolver
{
    private readonly uint _skillId = Skill.星灵移位;
    private Spell? GetSpell()
    {
        return !_skillId.GetSpell().IsReadyWithCanCast() ? null : _skillId.GetSpell(SpellTargetType.Self);
    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        if (spell == null) return;
        slot.Add(spell);
    }
    public int Check()
    {
        if (_skillId.GetSpell().Cooldown.TotalMilliseconds > 0) return -1;
        if (!BLMHelper.冰状态 && !BLMHelper.火状态) return -2;
        if (Core.Me.Level < 90) return -5;
        if (BLMHelper.火状态)
        {
            if (Core.Me.CurrentMp < 800)
            {
                if (BLMHelper.耀星层数 == 6) return -4;
                if (Skill.墨泉.RecentlyUsed(1500)) return -7;
                if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 3) return -6;
                if (Helper.可瞬发() ) return 6;
                if (BLMHelper.能星灵转冰()) return 7;
            }
        }

        if (BLMHelper.冰状态)
        {
            int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);

            if (BLMHelper.悖论指示 && !(nearbyEnemyCount > 2 && QT.Instance.GetQt("AOE")) ) return -3;
            if (BLMHelper.冰层数 != 3) return -4;
            if (BLMHelper.冰针 != 3) return -6;
            if (Core.Me.CurrentMp < 9800 && !(Skill.冰澈.RecentlyUsed(5000) || Skill.玄冰.RecentlyUsed(5000))) return -7;
            if (QT.Instance.GetQt("使用特供循环") && (new 开满转火().StartCheck() > 0 || BattleData.Instance.正在特殊循环中)) return -8;
            if (BattleData.Instance.需要瞬发gcd) return -9;
            if (GCDHelper.GetGCDCooldown() < 600 )
            {
                BattleData.Instance.需要瞬发gcd = true;
                return -5;
            }
            return 1;
        }

        return -99;
    }
}
