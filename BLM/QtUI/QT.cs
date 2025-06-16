using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using Oblivion.BLM.QtUI.Hotkey;

namespace Oblivion.BLM.QtUI;

public class QT
{
    public static JobViewWindow Instance { get; set; }

    /// <summary>
    /// 除了爆发药以外都复原
    /// </summary>
    public static void Reset()
    {

    }
    public static void Build()
    {
        Instance = new JobViewWindow(BLMSetting.Instance.JobViewSave, BLMSetting.Instance.Save, "嗨呀黑魔");


        Instance.AddQt("爆发药", false);
        Instance.AddQt("绝望", true);
        Instance.AddQt("核爆", true);
        Instance.AddQt("黑魔纹", true);
        Instance.AddQt("墨泉", true);
        Instance.AddQt("三连咏唱", true);
        Instance.AddQt("即刻", true);
        Instance.AddQt("雷一", true);
        Instance.AddQt("雷二", true);
        Instance.AddQt("异言", true);
        Instance.AddQt("秽浊", true);
        Instance.AddQt("星灵移位", true);
        Instance.AddQt("详述", true);


        Instance.AddHotkey("爆发药", new HotKeyResolver_Potion());
        Instance.AddHotkey("异言", new HotKeyResolver_NormalSpell(Spells.异言, SpellTargetType.Target));
        Instance.AddHotkey("秽浊", new HotKeyResolver_NormalSpell(Spells.秽浊, SpellTargetType.Target));
        Instance.AddHotkey("即刻", new HotKeyResolver_NormalSpell(Spells.即刻, SpellTargetType.Target));
        Instance.AddHotkey("黑魔纹", new HotKeyResolver_NormalSpell(Spells.黑魔纹, SpellTargetType.Target));
        Instance.AddHotkey("三连咏唱", new HotKeyResolver_NormalSpell(Spells.三连, SpellTargetType.Target));
        Instance.AddHotkey("冲刺", new HotKeyResolver_疾跑());
        Instance.AddHotkey("沉稳咏唱", new HotKeyResolver_NormalSpell(Spells.沉稳, SpellTargetType.Target));
        Instance.AddHotkey("混乱", new HotKeyResolver_NormalSpell(Spells.混乱, SpellTargetType.Target, true));

        ReadmeTab.Build(Instance);
        SettingTab.Build(Instance);
        
        以太步hotkeywindow.Build(Instance);
    }
}