using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using Dalamud.Interface.Colors;
using Dalamud.Utility;
using ImGuiNET;

namespace Oblivion.BLM.QtUI;

public static class ReadmeTab
{
    private static readonly InfoBox Box = new()
    {
        AutoResize = true,
        BorderColor = ImGuiColors.ParsedGold,
        ContentsAction = () =>
        {
            if (ImGui.CollapsingHeader("查看更新日志", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.BulletText("7.14 优化逻辑，添加玄冰、冰冻、核爆补耀星的逻辑，添加特供循环，尝试修复AOE问题");
                ImGui.BulletText("7.18 修复了一些问题");
            }

            ImGuiHelper.Separator();
            ImGui.BulletText("7.2黑魔，非最优循环，优先保证走位，其次会尽量使用星灵移位进冰打双星灵，全程标改。\n如果开上三插和DR减动画，可以无损插入能力技");
        }
    };
    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("说明", window =>
        {
            ImGui.Dummy(new Vector2(0, 1));
            ImGui.Dummy(new Vector2(5, 0));
            ImGui.SameLine();
            Box.DrawStretched();
        });
    }
}