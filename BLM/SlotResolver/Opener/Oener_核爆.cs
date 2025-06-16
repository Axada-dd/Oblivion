using AEAssist.CombatRoutine.Module.Opener;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.SlotResolver.Opener;

public class Oener_核爆: IOpener
{
    public int StartCheck()
    {
        /*if(!Spells.即刻.GetSpell().IsReadyWithCanCast())return -1;
        if(!Spells.黑魔纹.GetSpell().IsReadyWithCanCast())return -2;
        if(Spells.三连.GetSpell().Charges < 2.0)return -3;
        if(Spells.详述.GetSpell().IsReadyWithCanCast())return -4;
        if(Spells.墨泉.GetSpell().IsReadyWithCanCast())return -5;
        if(Core.Me.CurrentMp<10000)return -6;*/
        return 0;
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
            
            countDownHandler.AddAction(startTime+600, Spells.黑魔纹);
            countDownHandler.AddAction(startTime, Spells.火三,SpellTargetType.Target);
            countDownHandler.AddAction(startTime-2800,Spells.雷一.GetActionChange(),SpellTargetType.Target);
        }
        else
        {
            countDownHandler.AddAction(startTime, Spells.火三,SpellTargetType.Target);
            countDownHandler.AddAction(startTime-3000,Spells.雷一.GetActionChange(),SpellTargetType.Target);
        }
    }
    public List<Action<Slot>> Sequence { get; } =
    [
        Step1,Step3,Step4,Step5,Step6,Step7
    ];

    private static void Step1(Slot slot)
    {
        slot.Add(new Spell(Spells.即刻, SpellTargetType.Self));
        slot.Add(new Spell(Spells.详述, SpellTargetType.Self));
        slot.Add(new Spell(Spells.火四, SpellTargetType.Target));
        if (QT.Instance.GetQt("爆发药"))
            slot.Add(Spell.CreatePotion());
        if(!BLMSetting.Instance.提前黑魔纹)
            slot.Add(new Spell(Spells.黑魔纹, Core.Me.Position));
    }

    private static void Step2(Slot slot)
    {
        
    }

    private static void Step3(Slot slot)
    {
        slot.Add(new Spell(Spells.火四, SpellTargetType.Target));
        slot.Add(new Spell(Spells.异言, SpellTargetType.Target));
        slot.Add(new Spell(Spells.火四, SpellTargetType.Target));
        slot.Add(new Spell(Spells.火四, SpellTargetType.Target));
    }

    private static void Step4(Slot slot)
    {
        slot.Add(new Spell(Spells.绝望, SpellTargetType.Target));
        slot.Add(new Spell(Spells.墨泉, SpellTargetType.Self));
    }

    private static void Step5(Slot slot)
    {
        slot.Add(new Spell(Spells.火四,SpellTargetType.Target));
        slot.Add(new Spell(Spells.火四,SpellTargetType.Target));
        slot.Add(new Spell(Spells.耀星,SpellTargetType.Target));
        slot.Add(new Spell(Spells.火四,SpellTargetType.Target));
        slot.Add(new Spell(Spells.雷一.GetActionChange(),SpellTargetType.Target));
        slot.Add(new Spell(Spells.火四,SpellTargetType.Target));
        slot.Add(new Spell(Spells.火四,SpellTargetType.Target));
        slot.Add(new Spell(Spells.火四,SpellTargetType.Target));
    }

    private static void Step6(Slot slot)
    {
        slot.Add(new Spell(Spells.悖论,SpellTargetType.Target));
        slot.Add(new Spell(Spells.三连,SpellTargetType.Self));
    }

    private static void Step7(Slot slot)
    {
        slot.Add(new Spell(Spells.核爆,SpellTargetType.Target));
        slot.Add(new Spell(Spells.耀星,SpellTargetType.Target));
        slot.Add(new Spell(Spells.星灵移位, SpellTargetType.Self));
    }
}