using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using Oblivion.BLM.QtUI.Hotkey;

namespace Oblivion.BLM.QtUI;

public class QT
{
    public static JobViewWindow Instance { get; set; }
    public static HotkeyWindow? 以太步窗口 { get; set; }
    

    /// <summary>
    /// 除了爆发药以外都复原
    /// </summary>
    public static void Reset()
    {
        //Instance.SetQt(QTkey.黑魔纹, true);
        Instance.SetQt(QTkey.墨泉, true);
        Instance.SetQt(QTkey.三连咏唱, true);
        Instance.SetQt(QTkey.即刻, true);
        Instance.SetQt(QTkey.Dot, true);
        Instance.SetQt(QTkey.起手序列, true);
        Instance.SetQt(QTkey.双目标aoe, true);
        Instance.SetQt(QTkey.Aoe, true);
        Instance.SetQt(QTkey.异言, true);
        Instance.SetQt(QTkey.秽浊, true);
        Instance.SetQt(QTkey.智能aoe目标, false);
        Instance.SetQt(QTkey.详述, true);
        Instance.SetQt(QTkey.倾泻资源, false);
        Instance.SetQt(QTkey.Boss上天,false);
        Instance.SetQt(QTkey.压缩火悖论, false);
        Instance.SetQt(QTkey.压缩冰悖论, false);
    }
    public static void Build()
    {
        Instance = new JobViewWindow(BLMSetting.Instance.JobViewSave, BLMSetting.Instance.Save, "黑魔の登神长阶");
        

        Instance.AddQt(QTkey.爆发药, false);
        //Instance.AddQt("绝望", true);
        //Instance.AddQt("核爆", true);
        Instance.AddQt(QTkey.黑魔纹, true);
        Instance.AddQt(QTkey.墨泉, true);
        //Instance.AddQt("醒梦", true);
        Instance.AddQt(QTkey.三连咏唱, true);
        Instance.AddQt(QTkey.即刻, true);
        Instance.AddQt(QTkey.Dot, true);

        Instance.AddQt(QTkey.异言, true);
        Instance.AddQt(QTkey.秽浊, true);
        //Instance.AddQt("星灵移位", true);
        Instance.AddQt(QTkey.智能aoe目标, false);
        Instance.AddQt(QTkey.三连用于走位, false, "三连保留用于走位");
        Instance.AddQt(QTkey.关闭即刻三连的移动判断, false, "三连即刻不会在走位的时候自动释放");
        Instance.AddQt(QTkey.详述, true);
        Instance.AddQt(QTkey.起手序列, true,"关闭会不倒计时起手");
        Instance.AddQt(QTkey.双目标aoe, true,"启动双目标AOE");
        Instance.AddQt(QTkey.Aoe, true,"三目标及以上aoe");
        Instance.AddQt(QTkey.火二, false, "开启aoe打火2进火");
        Instance.AddQt(QTkey.秽浊填充aoe, false, "使用秽浊填充aoe冰阶段");
        Instance.AddQt(QTkey.倾泻资源, false,"清空通晓");
        Instance.AddQt(QTkey.Boss上天, false,"如果boss上天时间>10秒，请开启此选项,随开随关");
        //Instance.AddQt("双星灵墨泉", false);
        Instance.AddQt(QTkey.使用特供循环, false,"仅在开启三插和DR减动画时使用");
        Instance.AddQt(QTkey.压缩冰悖论,false);
        Instance.AddQt(QTkey.压缩火悖论,false);


        Instance.AddHotkey("爆发药", new HotKeyResolver_Potion());
        Instance.AddHotkey("异言", new HotKeyResolver_NormalSpell(Skill.异言, SpellTargetType.Target));
        Instance.AddHotkey("秽浊", new HotKeyResolver_NormalSpell(Skill.秽浊, SpellTargetType.Target));
        Instance.AddHotkey("即刻", new HotKeyResolver_NormalSpell(Skill.即刻, SpellTargetType.Self));
        Instance.AddHotkey("黑魔纹", new HotKeyResolver_NormalSpell(Skill.黑魔纹, SpellTargetType.Self));
        Instance.AddHotkey("三连咏唱", new HotKeyResolver_NormalSpell(Skill.三连, SpellTargetType.Self));
        Instance.AddHotkey("冲刺", new HotKeyResolver_疾跑());
        Instance.AddHotkey("沉稳咏唱", new HotKeyResolver_NormalSpell(Skill.沉稳, SpellTargetType.Self));
        Instance.AddHotkey("混乱", new HotKeyResolver_NormalSpell(Skill.混乱, SpellTargetType.Target, true));
        Instance.AddHotkey("魔罩", new HotKeyResolver_NormalSpell(Skill.魔罩, SpellTargetType.Self));
        
        
        /*var myJobViewSave = new JobViewSave();
        myJobViewSave.ShowHotkey = BLMSetting.Instance.以太步窗口显示;
        myJobViewSave.QtHotkeySize = new Vector2(BLMSetting.Instance.以太步IconSize);
        以太步窗口 = new HotkeyWindow(myJobViewSave, "以太步窗口");*/
        
        Instance.SetUpdateAction(OnUIUpdate);
        BLMSetting.Instance.LoadQtStates(Instance);
        ReadmeTab.Build(Instance);
        SettingTab.Build(Instance);
    }

    public static void OnUIUpdate()
    {
        Update以太步窗口();
        var myJobViewSave = new JobViewSave();
        myJobViewSave.ShowHotkey = BLMSetting.Instance.以太步窗口显示;
        myJobViewSave.QtHotkeySize = new Vector2(BLMSetting.Instance.以太步IconSize);
        myJobViewSave.LockWindow = BLMSetting.Instance.锁定以太步窗口;
        以太步窗口?.DrawHotkeyWindow(new QtStyle(BLMSetting.Instance.JobViewSave));
        以太步窗口 = new HotkeyWindow(myJobViewSave, "以太步");
        以太步窗口.HotkeyLineCount = 1;

    }
    public static void Update以太步窗口()
    {
        PartyHelper.UpdateAllies();
        if (PartyHelper.Party.Count <= 1) return;
        for (var i = 1; i < PartyHelper.Party.Count; i++)
        {
            var index = i;
            以太步窗口?.AddHotkey("以太步: " + PartyHelper.Party[i].Name, new 以太步HotkeyResolver(index));
        }
    }
}