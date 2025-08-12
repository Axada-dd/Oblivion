using AEAssist.CombatRoutine.Module.Target;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.CombatRoutine.Trigger.Node;
using System.Numerics;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using AEAssist;
using AEAssist.Helper;
using System.Collections.Generic;
using System.Linq;
namespace Oblivion.ScriptTest;

/// <summary>
/// 神兵柱子选择器 - 自动识别并标记四个柱子
/// 根据柱子位置计算梯形的顶边（最短边），并按顺时针顺序标记柱子
/// </summary>
public class 神兵柱子选择器 : ITriggerScript
{
    /// <summary>
    /// 存储已标记的柱子ID，用于避免重复标记
    /// </summary>
    private List<uint> 已标记柱子 = new List<uint>();
    
    /// <summary>
    /// 检查并执行柱子标记逻辑
    /// </summary>
    /// <param name="scriptEnv">脚本环境，包含共享变量</param>
    /// <param name="condParams">触发条件参数</param>
    /// <returns>是否完成处理</returns>
    public bool Check(ScriptEnv scriptEnv, ITriggerCondParams condParams)
    {
        // 获取所有柱子
        var 柱子列表 = TargetMgr.Instance.Units.Values.Where(e => e.DataId == 8731 && e.IsTargetable).ToList();
        var boss = TargetMgr.Instance.Units.Values.FirstOrDefault(e=>e.DataId == 8730 && e.IsTargetable);

        
        // 如果柱子数量不是4个，不进行处理
        if (柱子列表.Count != 4)
            return false;
            
        // 如果已经标记过所有柱子，不再重复标记
        if (柱子列表.All(e => 已标记柱子.Contains(e.EntityId)))
            return false;
            
        // 获取柱子位置
        var 柱子位置列表 = 柱子列表.Select(e => e.Position).ToList();
        
        // 计算所有柱子之间的距离
        var 距离列表 = new List<(int 柱子1索引, int 柱子2索引, float 距离)>();
        for (int i = 0; i < 柱子位置列表.Count; i++)
        {
            for (int j = i + 1; j < 柱子位置列表.Count; j++)
            {
                float 距离 = Vector3.Distance(柱子位置列表[i], 柱子位置列表[j]);
                距离列表.Add((i, j, 距离));
            }
        }
        
        // 按距离排序，找出最短的边
        距离列表.Sort((a, b) => a.距离.CompareTo(b.距离));
        
        // 最短的边是梯形的顶边
        var 顶边 = 距离列表[0];
        int 柱子A索引 = 顶边.柱子1索引;
        int 柱子B索引 = 顶边.柱子2索引;
        
        // 找出剩余的两个柱子
        var 剩余柱子索引 = Enumerable.Range(0, 4).Where(i => i != 柱子A索引 && i != 柱子B索引).ToList();
        int 柱子C索引 = 剩余柱子索引[0];
        int 柱子D索引 = 剩余柱子索引[1];
        
        // 计算柱子C和D到长边两端点的距离，确定它们的顺序
        float C到A的距离 = Vector3.Distance(柱子位置列表[柱子C索引], 柱子位置列表[柱子A索引]);
        float C到B的距离 = Vector3.Distance(柱子位置列表[柱子C索引], 柱子位置列表[柱子B索引]);
        float D到A的距离 = Vector3.Distance(柱子位置列表[柱子D索引], 柱子位置列表[柱子A索引]);
        float D到B的距离 = Vector3.Distance(柱子位置列表[柱子D索引], 柱子位置列表[柱子B索引]);
        
        // 确定顺时针顺序
        List<int> 顺时针顺序 = new List<int>();
        
        // 以顶边为基准，确定起点（A或B）
        Vector3 中心点 = new Vector3(
            (柱子位置列表[0].X + 柱子位置列表[1].X + 柱子位置列表[2].X + 柱子位置列表[3].X) / 4,
            (柱子位置列表[0].Y + 柱子位置列表[1].Y + 柱子位置列表[2].Y + 柱子位置列表[3].Y) / 4,
            (柱子位置列表[0].Z + 柱子位置列表[1].Z + 柱子位置列表[2].Z + 柱子位置列表[3].Z) / 4
        );
        
        // 计算向量叉积，确定顺时针方向
        // 使用向量叉乘来确定两个向量形成的平面的法向量
        Vector3 AB向量 = 柱子位置列表[柱子B索引] - 柱子位置列表[柱子A索引];
        Vector3 AC向量 = 柱子位置列表[柱子C索引] - 柱子位置列表[柱子A索引];
        Vector3 叉积 = Vector3.Cross(AB向量, AC向量);
        
        // 根据叉积的Y分量确定顺时针方向
        // 在游戏的坐标系中，Y轴通常是垂直方向
        // 如果叉积的Y分量为正，则A->B->C是顺时针；否则是逆时针
        bool 是顺时针 = 叉积.Y > 0;
        
        // 根据顺时针方向确定最终顺序
        // 注意：现在我们使用顶边（最短边）作为基准，而不是长边
        if (是顺时针)
        {
            // 如果C更靠近A，则顺序是A->C->D->B
            if (C到A的距离 < D到A的距离)
            {
                顺时针顺序.Add(柱子A索引);
                顺时针顺序.Add(柱子C索引);
                顺时针顺序.Add(柱子D索引);
                顺时针顺序.Add(柱子B索引);
            }
            else
            {
                // 否则顺序是A->D->C->B
                顺时针顺序.Add(柱子A索引);
                顺时针顺序.Add(柱子D索引);
                顺时针顺序.Add(柱子C索引);
                顺时针顺序.Add(柱子B索引);
            }
        }
        else
        {
            // 如果C更靠近B，则顺序是B->C->D->A
            if (C到B的距离 < D到B的距离)
            {
                顺时针顺序.Add(柱子B索引);
                顺时针顺序.Add(柱子C索引);
                顺时针顺序.Add(柱子D索引);
                顺时针顺序.Add(柱子A索引);
            }
            else
            {
                // 否则顺序是B->D->C->A
                顺时针顺序.Add(柱子B索引);
                顺时针顺序.Add(柱子D索引);
                顺时针顺序.Add(柱子C索引);
                顺时针顺序.Add(柱子A索引);
            }
        }
        Share.TrustDebugPoint.Clear();
        // 清除调试点
        Share.TrustDebugPoint.Clear();
        
        // 按顺时针顺序标记柱子（1-4）
        for (int i = 0; i < 顺时针顺序.Count; i++)
        {
            var 柱子 = 柱子列表[顺时针顺序[i]];
            if (!已标记柱子.Contains(柱子.EntityId))
            {
                // 标记柱子，使用1-4的标记
                //MemApiTarget.Instance.SetTargetSign(柱子.GameObject, (uint)(i + 1));
                //Core.Resolve<MemApiTarget>().SetTarget(柱子);
                //Core.Resolve<MemApiSendMessage>().SendMessage($"/mk attack <t>");
                //LogHelper.Print($"标记柱子{i + 1}");
                //Share.TrustDebugPoint.Add(柱子.Position);
                已标记柱子.Add(柱子.EntityId);
            }
        }
        // 将已标记的柱子ID列表存入脚本环境，供其他脚本使用
        scriptEnv.KV.TryAdd("已标记柱子",已标记柱子);
        return true;
    }
}