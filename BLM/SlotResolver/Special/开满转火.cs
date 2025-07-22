using System.Runtime.InteropServices;
using Dalamud.Game.ClientState.Objects.Types;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Special;

public class 开满转火: ISlotSequence
{
    private static IBattleChara? target;
    private static bool aoe;
    public List<Action<Slot>> Sequence { get; }
    public int StartCheck()
    {
        if (BLMHelper.三目标aoe() || BLMHelper.双目标aoe()) return -7;
        if (!QT.Instance.GetQt(QTkey.使用特供循环)) return -1;
        if (Core.Me.CurrentMp < 800) return -7;
        if (Skill.墨泉.GetSpell().Cooldown.TotalSeconds > 0 && Skill.墨泉.GetSpell().Cooldown.TotalSeconds < 6) return -2;
        if (!BLMHelper.冰状态) return -3;
        if (BLMHelper.冰层数 != 3) return -4;
        if (Helper.有buff(Buffs.三连Buff) || Helper.有buff(Buffs.即刻Buff)) return -7;
        if (BLMHelper.冰针 == 3 || Skill.冰澈.RecentlyUsed() || Skill.冰冻.RecentlyUsed()) return -6;
        return 1;
    }

    public int StopCheck(int index)
    {
        if (index == 2)
        {
            if (Helper.可瞬发()) return 1;
        }
        return -1;
    }
    private static void Step0(Slot slot)
    {
        BattleData.Instance.正在特殊循环中 = true;
        if (BLMHelper.通晓层数 == 3 && BLMHelper.通晓剩余时间 < 13)
            slot.Add(new Spell(aoe ? Skill.秽浊 : Skill.异言, SpellTargetType.Target).DontUseGcd());
        if (BLMHelper.提前补dot && QT.Instance.GetQt(QTkey.Dot) && Helper.有buff(Buffs.雷云))
        {
            slot.Add(new Spell(aoe ? Skill.雷二 : Skill.雷一, SpellTargetType.Target).DontUseGcd());
        }


        if (BLMHelper.悖论指示)
            slot.Add(new Spell(Skill.悖论, SpellTargetType.Target).DontUseGcd());
        slot.Add(new Spell(Skill.冰澈, SpellTargetType.Target).DontUseGcd());

    }

    private static void Step1(Slot slot)
    {
        slot.Add(new Spell(Skill.星灵移位, SpellTargetType.Self).DontUseGcd());
    }

    private static void Step2(Slot slot)
    {

        slot.Add(new Spell(Skill.绝望, SpellTargetType.Target).DontUseGcd());

        BattleData.Instance.正在特殊循环中 = false;
    }
    public 开满转火()
    {
        int num = 3;
        List<Action<Slot>> list = new List<Action<Slot>>(num);
        CollectionsMarshal.SetCount(list, num);
        Span<Action<Slot>> span = CollectionsMarshal.AsSpan(list);
        int num2 = 0;
        span[num2] = Step0;
        num2++;
        span[num2] = Step1;
        span[num2 + 1] = Step2;
        Sequence = list;
    }
}