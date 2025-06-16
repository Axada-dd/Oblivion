using AEAssist.CombatRoutine.View.JobView;
using AEAssist.IO;

namespace Oblivion.BLM;

public class BLMSetting
{
    public static BLMSetting Instance;
    
    #region 标准模板代码 可以直接复制后改掉类名即可

    private static string path;

    public static void Build(string settingPath)
    {
        path = Path.Combine(settingPath, $"{nameof(BLMSetting)}.json");
        if (!File.Exists(path))
        {
            Instance = new BLMSetting();
            Instance.Save();
            return;
        }

        try
        {
            Instance = JsonHelper.FromJson<BLMSetting>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Instance = new();
            LogHelper.Error(e.ToString());
        }
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, JsonHelper.ToJson(this));
    }

    #endregion
    public JobViewSave JobViewSave = new()
    {
    }; // QT设置存档
    public int 以太步IconSize = 24;
    public bool 以太步窗口显示 = true;
    public bool AutoUpdataTimeLines = true;
    public bool TimeLinesDebug = false;

    public bool 核爆起手 = false;
    public bool 标准57 = false;
    public double 起手预读时间 = 3.5;
    public bool 提前黑魔纹 = false;
}