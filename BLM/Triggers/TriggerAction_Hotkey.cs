using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.Triggers;

//这个类也可以完全复制 改一下上面的namespace和对QT的引用就行
public class TriggerActionHotkey : ITriggerAction
{
    public string DisplayName { get; } = "BLM/Hotkey";
    public string Remark { get; set; }

    public string Key = "";
    public bool Value;

    // 辅助数据 因为是private 所以不存档
    private int _selectIndex;
    private string[] _hotkeyArray;

    public TriggerActionHotkey()
    {
        _hotkeyArray = QT.Instance.GetHotkeyArray();
    }

    public bool Draw()
    {
        _selectIndex = Array.IndexOf(_hotkeyArray, Key);
        if (_selectIndex == -1)
        {
            _selectIndex = 0;
        }

        ImGuiHelper.LeftCombo("使用Hotkey", ref _selectIndex, _hotkeyArray);
        Key = _hotkeyArray[_selectIndex];
        return true;
    }

    public bool Handle()
    {
        QT.Instance.SetHotkey(Key);
        if (BLMSetting.Instance.TimeLinesDebug) LogHelper.Print("时间轴", $"使用hotkey => {Key}");
        return true;
    }
}