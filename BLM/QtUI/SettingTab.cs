using System.Diagnostics;
using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using AEAssist.MemoryApi;
using ImGuiNET;
using Oblivion.BLM.SlotResolver.Opener;
using Oblivion.BLM.SlotResolver.Special;

namespace Oblivion.BLM.QtUI;

public static class SettingTab
{

    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("设置", window =>
        {
            if (ImGui.CollapsingHeader("起手设置", ImGuiTreeNodeFlags.DefaultOpen))
            {
                var openerSelectionIndex = 0;
                var openerSelection = "";
                if (BLMSetting.Instance.标准57)
                    openerSelectionIndex = 1;
                if (BLMSetting.Instance.核爆起手)
                    openerSelectionIndex = 2;
                if (BLMSetting.Instance.开挂循环)
                    openerSelectionIndex = 3;


                switch (openerSelectionIndex)
                {
                    case 0:
                        openerSelection = "请选择";
                        break;
                    case 1:
                        openerSelection = "标准57";
                        break;
                    case 2:
                        openerSelection = "核爆起手";
                        break;
                    case 3:
                        openerSelection = "特供起手";
                        break;
                }

                if (ImGui.BeginCombo("起手选择", openerSelection))
                {
                    if (ImGui.Selectable("请选择"))
                    {
                    }

                    if (ImGui.Selectable("标准57"))
                    {
                        BLMSetting.Instance.标准57 = true;
                        BLMSetting.Instance.核爆起手 = false;
                        BLMSetting.Instance.开挂循环 = false;
                    }

                    if (ImGui.Selectable("核爆起手"))
                    {

                        BLMSetting.Instance.标准57 = false;
                        BLMSetting.Instance.核爆起手 = true;
                        BLMSetting.Instance.开挂循环 = false;
                    }

                    if (ImGui.Selectable("特供起手"))
                    {

                        BLMSetting.Instance.标准57 = false;
                        BLMSetting.Instance.核爆起手 = false;
                        BLMSetting.Instance.开挂循环 = true;
                    }

                    ImGui.EndCombo();
                }
                
                bool 倒计时黑魔纹 = BLMSetting.Instance.提前黑魔纹;
                if (ImGui.Checkbox("提前黑魔纹", ref 倒计时黑魔纹))
                {
                    if(倒计时黑魔纹)BLMSetting.Instance.提前黑魔纹 = true;
                    else BLMSetting.Instance.提前黑魔纹 = false; 
                    BLMSetting.Instance.Save();
                }

            }
            if (ImGui.CollapsingHeader("循环设置"))
            {
                ImGui.Text("以下所有设置都能在时间轴中控制，非轴作者可以不用在意这些设置，不改也已经是优秀循环了");
                ImGui.Checkbox("关闭起手", ref BattleData.Instance.起手);
                ImGui.Checkbox("留下所有三连用于走位，只针对100级循环", ref BattleData.Instance.三连走位);
                ImGui.Checkbox("低等级aoe循环中会使用火二进火",ref BattleData.Instance.aoe火二);
                ImGui.Checkbox("使用特供循环，注意可能会被绿玩出警", ref BattleData.Instance.特供循环);
                ImGui.Checkbox("压缩冰悖论，即在冰循环中不会主动打悖论，移动时正常悖论",ref BattleData.Instance.压缩冰悖论);
                ImGui.Checkbox("压缩火悖论，同上",ref BattleData.Instance.压缩火悖论);
                ImGui.Checkbox("70级循环中使用核爆收尾", ref BattleData.Instance.核爆收尾);
            }
            if (ImGui.CollapsingHeader("以太步窗口设置"))
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
            if (ImGui.CollapsingHeader("TTK设置"))
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
            
            

            ImGuiHelper.Separator();

        });
        
        
        instance.AddTab("debug", window =>
        {

            ImGui.Text($"上一G：{BattleData.Instance.前一gcd}");
            ImGui.Text($"复唱时间:{GCDHelper.GetGCDDuration()}");
            ImGui.Text($"剩余GCD时间：{GCDHelper.GetGCDCooldown()}");
            ImGui.Text($"能力技卡G：{BLMHelper.能力技卡g}");
            ImGui.Text($"发呆中：{BLMHelper.在发呆()}");
            ImGui.Text($"使用瞬发：{BattleData.Instance.已使用瞬发}");
            ImGui.Text($"可瞬发：{Helper.可瞬发()}");
            //ImGui.Text($"已使用耀星：{BattleData.Instance.已使用耀星}");
            ImGui.Text($"已存在黑魔纹：{Helper.有buff(737)}");
            ImGui.Text($"三连咏唱CD：{BLMHelper.三连cd()}");
            //ImGui.Text($"火循环剩余gcd：{BattleData.Instance.火循环剩余gcd}");
            //ImGui.Text($"冰循环剩余gcd：{BattleData.Instance.冰循环剩余gcd}");
            //ImGui.Text($"能使用火四个数：{BattleData.Instance.能使用的火四个数}");
            //ImGui.Text($"能使用耀星：{BattleData.Instance.能使用耀星}");
            //ImGui.Text($"三连转冰：{BattleData.Instance.三连转冰}");
            ImGui.Text($"需要瞬发：{BattleData.Instance.需要瞬发gcd}");
            ImGui.Text($"需要即刻: {BattleData.Instance.需要即刻}");
            //ImGui.Text($"特供循环判断:{new 开满转火().StartCheck()}");
            //ImGui.Text($"双星灵墨泉：{new 双星灵墨泉().StartCheck()}");
            ImGui.Text($"三冰针进冰：{BattleData.Instance.三冰针进冰}");
            ImGui.Text($"双目标：{BLMHelper.双目标aoe()}-----三目标: {BLMHelper.三目标aoe()}");
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
        instance.AddTab("已释放技能", window =>
        {
            if (BattleData.Instance.冰状态gcd.Count > 0)
            {
                ImGui.Text("当前冰循环：");
                ImGui.Text($"{string.Join(",", BattleData.Instance.冰状态gcd.Select(s=>s.GetSpell().Name))}");
            }

            if (BattleData.Instance.火状态gcd.Count > 0)
            {
                ImGui.Text("当前火循环：");
                ImGui.Text($"{string.Join(",", BattleData.Instance.火状态gcd.Select(s=>s.GetSpell().Name))}");
            }

            if (BattleData.Instance.上一轮循环.Count > 0)
            {
                ImGui.Text("上一轮循环：");
                ImGui.Text($"{string.Join(",", BattleData.Instance.上一轮循环.Select(s=>s.GetSpell().Name))}");
            }
        });
        
        
    }
}