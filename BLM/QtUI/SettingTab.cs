using System.Diagnostics;
using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using AEAssist.MemoryApi;
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

            if (ImGui.CollapsingHeader("以太步窗口设置", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Text("以太步窗口开关：");
                bool 以太步窗口开关 = BLMSetting.Instance.以太步窗口显示;
                if (ImGui.Checkbox("##以太步窗口开关", ref 以太步窗口开关))
                {
                    BLMSetting.Instance.以太步窗口显示 = 以太步窗口开关;
                    BLMSetting.Instance.Save();
                }
                ImGui.Text("以太步图标大小：");
                int 以太步图标大小 = BLMSetting.Instance.以太步IconSize;
                if (ImGui.InputInt("##以太步图标大小", ref 以太步图标大小, 1, 50))
                {
                    BLMSetting.Instance.以太步IconSize = 以太步图标大小;
                    BLMSetting.Instance.Save();
                }
            }
            if (ImGui.CollapsingHeader("TTK设置", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Text("设置TTK阈值（毫秒）：");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("当目标剩余击杀时间低于此值时，将停止使用彩绘技能");
                    ImGui.EndTooltip();
                }
                int currentTTK = BLMSetting.Instance.TTK阈值;
                if (ImGui.InputInt("##TTK阈值", ref currentTTK, 1000, 5000))
                {
                    BLMSetting.Instance.TTK阈值 = currentTTK;
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

            ImGuiHelper.Separator();

        });
        instance.AddTab("其他设置", window =>
        {
            if (ImGui.CollapsingHeader("Debug", ImGuiTreeNodeFlags.DefaultOpen))
            {
                
                ImGui.Text($"上一G：{BattleData.Instance.前一gcd}");
                ImGui.Text($"复唱时间:{Core.Resolve<MemApiSpell>().GetGCDDuration()}");
                ImGui.Text($"使用瞬发：{BattleData.Instance.已使用瞬发}");
                ImGui.Text($"可瞬发：{BattleData.Instance.可瞬发}");
                ImGui.Text($"已使用耀星：{BattleData.Instance.已使用耀星}");
                ImGui.Text($"已使用黑魔纹：{BattleData.Instance.已使用黑魔纹}");
                ImGui.Text($"三连咏唱CD：{BattleData.Instance.三连cd}");
                ImGui.Text($"火循环剩余gcd：{BattleData.Instance.火循环剩余gcd}");
                ImGui.Text($"冰循环剩余gcd：{BattleData.Instance.冰循环剩余gcd}");
                ImGui.Text($"能使用火四个数：{BattleData.Instance.能使用的火四个数}");
                ImGui.Text($"能使用耀星：{BattleData.Instance.能使用耀星}");
                ImGui.Text($"三连转冰：{BattleData.Instance.使用三连转冰}");
                //ImGui.Text($"是否在起手：{Opener57.StartCheck()>=0||Opener核爆.StartCheck()>=0}");
            }
            if (ImGui.CollapsingHeader("技能队列", ImGuiTreeNodeFlags.DefaultOpen))
            {
                if (ImGui.Button("清除队列"))
                {
                    AI.Instance.BattleData.HighPrioritySlots_OffGCD.Clear();
                    AI.Instance.BattleData.HighPrioritySlots_GCD.Clear();
                }
                ImGui.SameLine();
                if (ImGui.Button("清除一个"))
                {
                    AI.Instance.BattleData.HighPrioritySlots_OffGCD.Dequeue();
                    AI.Instance.BattleData.HighPrioritySlots_GCD.Dequeue();
                }
                ImGui.Text("-------能力技-------");
                if (AI.Instance.BattleData.HighPrioritySlots_OffGCD.Count > 0)
                    foreach (var action in AI.Instance.BattleData.HighPrioritySlots_OffGCD.SelectMany(spell => spell.Actions))
                    {
                        ImGui.Text(action.Spell.Name);
                    }
                ImGui.Text("-------GCD-------");
                if (AI.Instance.BattleData.HighPrioritySlots_GCD.Count > 0)
                    foreach (var action in AI.Instance.BattleData.HighPrioritySlots_GCD.SelectMany(spell => spell.Actions))
                    {
                        ImGui.Text(action.Spell.Name);
                    }
                ImGui.TreePop();
            }
        });
        
    }
}