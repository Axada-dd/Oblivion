using System.Numerics;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.CombatRoutine.Trigger.Node;
using AEAssist.MemoryApi;
using AEAssist.Helper;
using AEAssist;

namespace Oblivion.ScriptTest;

/// <summary>
/// FruP3自动强制旋转 - 根据光位置自动调整角色朝向
/// 将光位置转换为8方向索引，并设置角色旋转角度
/// </summary>
public class FruP3AutoForceRotation : ITriggerScript
{
    /// <summary>
    /// 检查并执行角色旋转逻辑
    /// </summary>
    /// <param name="scriptEnv">脚本环境，包含共享变量</param>
    /// <param name="condParams">触发条件参数</param>
    /// <returns>是否完成处理</returns>
    public bool Check(ScriptEnv scriptEnv, ITriggerCondParams condParams)
    {
        // 尝试从KV存储中获取光位置
        if (scriptEnv.KV.TryGetValue("lightPosition", out var lightPositionObj))
        {
            string direction = "";

            // 将光位置转换为Vector3
            Vector3 lightPosition = (Vector3)lightPositionObj;
            // 计算光位置对应的8方向索引
            var lightIndex = PositionTo8Dir(lightPosition, new Vector3(100, 0, 100));
            // 根据光位置索引设置对应的旋转角度
            // 将8方向索引转换为对应的角度值
            var myRot = lightIndex switch
            {
                0 => float.Pi,
                1 => float.Pi/4,
                2 => float.Pi/2,
                3 => float.Pi*3/4,
                4 => 0,
                5 => -float.Pi/4,
                6 => -float.Pi/2,
                7 => -float.Pi*3/4,
                _ => 0,
            };

            // 设置角色的旋转角度
            Core.Resolve<MemApiMove>().SetRot(myRot);
            LogHelper.Print($"灯在：{lightIndex} 面向：{myRot}");
            return true;
        }

        return false;
    }

    /// <summary>
    /// 将位置转换为8方向索引
    /// </summary>
    /// <param name="point">目标点位置</param>
    /// <param name="centre">中心点位置</param>
    /// <returns>方向索引(0-7)</returns>
    private int PositionTo8Dir(Vector3 point, Vector3 centre)
    {
        // 方向索引: 北 = 0, 东北 = 1, 东 = 2, 东南 = 3, 南 = 4, 西南 = 5, 西 = 6, 西北 = 7
        var r = Math.Round(4 - 4 * Math.Atan2(point.X - centre.X, point.Z - centre.Z) / Math.PI) % 8;
        return (int)r;

    }
}