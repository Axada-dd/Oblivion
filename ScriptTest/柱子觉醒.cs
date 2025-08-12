using AEAssist.CombatRoutine.Trigger;
using AEAssist.CombatRoutine.Trigger.Node;

namespace Oblivion.ScriptTest;

/// <summary>
/// 柱子觉醒 - 监控特定状态ID的出现次数，并在达到特定次数时设置KV值
/// 用于追踪柱子觉醒状态并通知其他脚本
/// </summary>
public class 柱子觉醒 : ITriggerScript
{
    /// <summary>
    /// 状态出现次数计数器
    /// </summary>
    private int _count = 0;
    /// <summary>
    /// 检查并处理柱子觉醒状态
    /// </summary>
    /// <param name="scriptEnv">脚本环境，包含共享变量</param>
    /// <param name="condParams">触发条件参数</param>
    /// <returns>是否完成处理</returns>
    public bool Check(ScriptEnv scriptEnv, ITriggerCondParams condParams)
    {
        // 检查参数类型并验证状态ID
        if (condParams is not AddStatusCondParams addStatus) return false;
        // 当状态ID为1544且计数为2时
        if (addStatus.StatusId == 1544 && addStatus.count ==2)
        {
            // 增加计数器
            _count++;
            // 当计数达到2时，设置12柱觉醒标记
            if (_count == 2)
            {
                // 在KV存储中标记12柱已觉醒
                scriptEnv.KV.TryAdd("12柱觉醒", true);
            }
        }

        // 当计数达到4时，设置34柱觉醒标记并重置计数
        if (_count == 4)
        {
            // 在KV存储中标记34柱已觉醒
            scriptEnv.KV.TryAdd("34柱觉醒", true);
            // 重置计数器，准备下一轮计数
            _count = 0;
            return true;
        }
        return false;
    }
}