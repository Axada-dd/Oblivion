using System.Diagnostics;
using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using ImGuiNET;
using Oblivion.Common;

namespace Oblivion.BLM.QtUI;

public static class SettingTab
{
    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("Oblivion", winow =>
        {
            if (ImGui.CollapsingHeader("时间轴", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Dummy(new Vector2(5, 0));
                ImGui.SameLine();
                ImGui.BeginGroup();
                /*ImGui.Checkbox("启用时间轴debug", ref BLMSetting.Instance.TimeLinesDebug);
                ImGui.Checkbox("启用自动更新", ref BLMSetting.Instance.AutoUpdataTimeLines);
                if (ImGui.Button("手动更新")) TimeLineUpdater.UpdateFiles(Helper.DncTimeLineUrl);
                ImGui.SameLine();
                if (ImGui.Button("源码"))
                    Process.Start(new ProcessStartInfo(Helper.TimeLineLibraryUrl) { UseShellExecute = true });*/
                ImGui.EndGroup();
                ImGui.Dummy(new Vector2(0, 10));
            }

            if (ImGui.CollapsingHeader("以太步窗口", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Dummy(new Vector2(5, 0));
                ImGui.SameLine();
                ImGui.BeginGroup();
                //ImGui.Checkbox("显示舞伴窗口", ref DncSettings.Instance.DancePartnerPanelShow);
                //ImGui.Checkbox("4人本自动绑舞伴", ref DncSettings.Instance.AutoPartner);
                //ImGui.DragInt("舞伴窗口大小", ref DncSettings.Instance.DancePartnerPanelIconSize, 1, 20, 100);
                ImGui.EndGroup();
                ImGui.Dummy(new Vector2(0, 10));
            }

            ImGuiHelper.Separator();

        });
    }
}