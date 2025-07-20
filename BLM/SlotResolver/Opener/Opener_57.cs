using AEAssist.CombatRoutine.Module.Opener;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Opener;

public class Opener57 : IOpener
{
    public int StartCheck()
    {
        if (BattleData.Instance.IsInnerOpener) return 1;
        if (Helper.是否在战斗中()) return -2;
        
        if (QT.Instance.GetQt(QTkey.起手序列)&&Core.Me.CurrentMp == 10000&&!(BLMHelper.火状态 || BLMHelper.冰状态)) return 2;
        return -1;
    }

    public int StopCheck(int index)
    {
        return -1;
    }

    public void InitCountDown(CountDownHandler countDownHandler)
    {
        int startTime = (int)(BLMSetting.Instance.起手预读时间 * 1000);
        if (BLMSetting.Instance.提前黑魔纹)
        {

            countDownHandler.AddAction(startTime + 600, Skill.黑魔纹, SpellTargetType.Self);
            countDownHandler.AddAction(startTime, Skill.火三, SpellTargetType.Target);
            countDownHandler.AddAction(startTime - 500, () => BattleData.Instance.IsInnerOpener = true);
            countDownHandler.AddAction(startTime - 2800, Skill.雷一.GetActionChange(), SpellTargetType.Target);
        }
        else
        {
            countDownHandler.AddAction(startTime, Skill.火三, SpellTargetType.Target);
            countDownHandler.AddAction(startTime - 500, () => BattleData.Instance.IsInnerOpener = true);
            countDownHandler.AddAction(startTime - 3000, Skill.雷一.GetActionChange(), SpellTargetType.Target);
        }
    }
    public List<Action<Slot>> Sequence { get; } =
    [
        Step1,Step3,Step4,Step5,Step6,Step7
    ];

    private static void Step1(Slot slot)
    {

        slot.Add(new Spell(Skill.即刻, SpellTargetType.Self));
        slot.Add(new Spell(Skill.详述, SpellTargetType.Self));
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        if (QT.Instance.GetQt("爆发药"))
            slot.Add(Spell.CreatePotion());
        if (!BLMSetting.Instance.提前黑魔纹)
            slot.Add(new Spell(Skill.黑魔纹, SpellTargetType.Self));
    }

    private static void Step2(Slot slot)
    {
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        if (QT.Instance.GetQt("爆发药"))
            slot.Add(Spell.CreatePotion());
        if (!BLMSetting.Instance.提前黑魔纹)
            slot.Add(new Spell(Skill.黑魔纹, SpellTargetType.Self));
    }

    private static void Step3(Slot slot)
    {
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
    }

    private static void Step4(Slot slot)
    {
        slot.Add(new Spell(Skill.异言, SpellTargetType.Target));
        slot.Add(new Spell(Skill.墨泉, SpellTargetType.Self));
    }

    private static void Step5(Slot slot)
    {
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        slot.Add(new Spell(Skill.耀星, SpellTargetType.Target));
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
    }

    private static void Step6(Slot slot)
    {
        slot.Add(new Spell(Skill.雷一.GetActionChange(), SpellTargetType.Target));
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        slot.Add(new Spell(Skill.火四, SpellTargetType.Target));
        slot.Add(new Spell(Skill.耀星, SpellTargetType.Target));
    }

    private static void Step7(Slot slot)
    {
        slot.Add(new Spell(Skill.绝望, SpellTargetType.Target));
        slot.Add(new Spell(Skill.星灵移位, SpellTargetType.Self));
        slot.Add(new Spell(Skill.三连, SpellTargetType.Self));
        if(BattleData.Instance.IsInnerOpener)
            BattleData.Instance.IsInnerOpener = false;
    }
}