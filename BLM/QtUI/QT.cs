using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;

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
        Instance = new JobViewWindow(BLMSetting.Instance.JobViewSave, BLMSetting.Instance.Save,"嗨呀黑魔");
        
        
        Instance.AddQt("爆发药", false);
        Instance.AddQt("绝望", true);
        Instance.AddQt("核爆",true);

        
        Instance.AddHotkey("爆发药", new HotKeyResolver_Potion());
        
        ReadmeTab.Build(Instance);
        SettingTab.Build(Instance);
    }
}