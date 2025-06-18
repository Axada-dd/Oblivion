using System.Drawing;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using ImGuiNET;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM.Triggers;

//这个类也可以完全复制 改一下上面的namespace和对QT的引用就行
public class TriggerCondQt : ITriggerCond
{
    public string DisplayName => "BLM/QT检测";
    public string Remark { get; set; }

    public string Key = "";
    public bool Value;

    private int _selectIndex;
    private string[] _qtArray;

    public TriggerCondQt()
    {
        _qtArray = QT.Instance.GetQtArray();
    }

    public bool Draw()
    {
        _selectIndex = Array.IndexOf(_qtArray, Key);
        if (_selectIndex == -1)
        {
            _selectIndex = 0;
        }
        ImGuiHelper.LeftCombo("选择Key", ref _selectIndex, _qtArray);
        Key = _qtArray[_selectIndex];
        ImGui.SameLine();
        using (new GroupWrapper())
        {
            ImGui.Checkbox("", ref Value);
        }
        ImGuiHelper.TextColor(Color.Orange, "判断该qt的状态");
        return true;
    }

    public bool Handle(ITriggerCondParams triggerCondParams)
    {
        return QT.Instance.GetQt(Key) == Value;
    }
}