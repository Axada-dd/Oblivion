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
        
    }
    public static void Build()
    {
        Instance = new JobViewWindow(BLMSetting.Instance.JobViewSave, BLMSetting.Instance.Save, "黑魔の登神长阶");


        Instance.AddQt("爆发药", false);
        Instance.AddQt("绝望", true);
        Instance.AddQt("核爆", true);
        Instance.AddQt("黑魔纹", true);
        Instance.AddQt("墨泉", true);
        Instance.AddQt("醒梦", true);
        Instance.AddQt("三连咏唱", true);
        Instance.AddQt("即刻", true);
        Instance.AddQt("雷一", true);
        Instance.AddQt("雷二", true);
        Instance.AddQt("异言", true);
        Instance.AddQt("秽浊", true);
        Instance.AddQt("星灵移位", true);
        Instance.AddQt("三连用于走位",false,"三连保留用于走位");
        Instance.AddQt("详述", true);
        Instance.AddQt("起手序列", true,"关闭会不倒计时起手");
        Instance.AddQt("AOE", false,"一键关闭所有aoe技能");
        Instance.AddQt("倾泻资源", false,"清空异言");
        Instance.AddQt("上天转圈", false);
        Instance.AddQt("Boss上天", false,"如果boss上天时间>10秒，请开启此选项,随开随关");


        Instance.AddHotkey("爆发药", new HotKeyResolver_Potion());
        Instance.AddHotkey("异言", new HotKeyResolver_NormalSpell(Spells.异言, SpellTargetType.Target));
        Instance.AddHotkey("秽浊", new HotKeyResolver_NormalSpell(Spells.秽浊, SpellTargetType.Target));
        Instance.AddHotkey("即刻", new HotKeyResolver_NormalSpell(Spells.即刻, SpellTargetType.Self));
        Instance.AddHotkey("黑魔纹", new HotKeyResolver_NormalSpell(Spells.黑魔纹, SpellTargetType.Self));
        Instance.AddHotkey("三连咏唱", new HotKeyResolver_NormalSpell(Spells.三连, SpellTargetType.Self));
        Instance.AddHotkey("冲刺", new HotKeyResolver_疾跑());
        Instance.AddHotkey("沉稳咏唱", new HotKeyResolver_NormalSpell(Spells.沉稳, SpellTargetType.Self));
        Instance.AddHotkey("混乱", new HotKeyResolver_NormalSpell(Spells.混乱, SpellTargetType.Target, true));
        Instance.AddHotkey("魔罩", new HotKeyResolver_NormalSpell(Spells.魔罩, SpellTargetType.Self));
        
        
        var myJobViewSave = new JobViewSave();
        myJobViewSave.ShowHotkey = BLMSetting.Instance.以太步窗口显示;
        myJobViewSave.QtHotkeySize = new Vector2(BLMSetting.Instance.以太步IconSize);
        以太步窗口 = new HotkeyWindow(myJobViewSave, "WardensPaeanPanel");
        
        QT.Instance.SetUpdateAction(OnUIUpdate);
        
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