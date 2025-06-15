using System.Diagnostics;
using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using ImGuiNET;

namespace Oblivion.BLM.QtUI;

public static class SettingTab
{
    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("Oblivion", window =>
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
        instance.AddTab("Debug", window =>
        {
            ImGui.Text($"上一G：{BattleData.Instance.前一GCD}");
            ImGui.Text($"使用瞬发：{BattleData.Instance.已使用瞬发}");
            ImGui.Text($"可瞬发：{BattleData.Instance.可瞬发}");
            ImGui.Text($"已使用耀星：{BattleData.Instance.已使用耀星}");
            ImGui.Text($"已使用黑魔纹：{BattleData.Instance.已使用黑魔纹}");
            ImGui.Text($"三连咏唱层数：{Spells.三连.GetSpell().Charges}");
            ImGui.Text($"三连咏唱下一层转好时间：{Spells.三连.GetSpell().Charges*60}");
        });
    }
}