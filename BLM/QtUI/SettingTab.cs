using System.Diagnostics;
using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using ImGuiNET;
using Oblivion.BLM.SlotResolver.Opener;

namespace Oblivion.BLM.QtUI;

public static class SettingTab
{

    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("设置", window =>
        {
            if (ImGui.CollapsingHeader("起手设置", ImGuiTreeNodeFlags.DefaultOpen))
            {
                bool opner1 = BLMSetting.Instance.标准57;
                if(ImGui.Checkbox("标准5+7起手", ref opner1))
                {
                    if (opner1)
                    {
                        BLMSetting.Instance.标准57 = true;
                        BLMSetting.Instance.核爆起手 = false;
                    }
                    BLMSetting.Instance.Save();
                }
                bool opener2 = BLMSetting.Instance.核爆起手;
                if (ImGui.Checkbox("核爆起手", ref opener2))
                {
                    if (opener2)
                    {
                        BLMSetting.Instance.核爆起手 = true;
                        BLMSetting.Instance.标准57 = false;
                    }
                    BLMSetting.Instance.Save();
                }
                bool 倒计时黑魔纹 = BLMSetting.Instance.提前黑魔纹;
                if (ImGui.Checkbox("提前黑魔纹", ref 倒计时黑魔纹))
                {
                    if(倒计时黑魔纹)BLMSetting.Instance.提前黑魔纹 = true;
                    else BLMSetting.Instance.提前黑魔纹 = false; 
                    BLMSetting.Instance.Save();
                }

            }
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
                ImGui.Checkbox("显示以太步窗口", ref BLMSetting.Instance.以太步窗口显示);
                ImGui.DragInt("以太步窗口大小", ref BLMSetting.Instance.以太步IconSize, 1, 20, 100);
                ImGui.EndGroup();
                ImGui.Dummy(new Vector2(0, 10));
            }

            ImGuiHelper.Separator();

        });
        instance.AddTab("Debug", window =>
        {
            ImGui.Text($"上一G：{BattleData.Instance.前一gcd}");
            ImGui.Text($"使用瞬发：{BattleData.Instance.已使用瞬发}");
            ImGui.Text($"可瞬发：{BattleData.Instance.可瞬发}");
            ImGui.Text($"已使用耀星：{BattleData.Instance.已使用耀星}");
            ImGui.Text($"已使用黑魔纹：{BattleData.Instance.已使用黑魔纹}");
            ImGui.Text($"三连咏唱层数：{Spells.三连.GetSpell().Charges}");
            ImGui.Text($"三连咏唱下一层转好时间：{Spells.三连.GetSpell().Charges*60}");
            ImGui.Text($"火循环将结束：{BattleData.Instance.火循环剩余gcd小于3}");
        });
    }
}