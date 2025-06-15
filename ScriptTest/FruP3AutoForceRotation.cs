using System.Numerics;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.CombatRoutine.Trigger.Node;
using AEAssist.MemoryApi;

namespace Oblivion.ScriptTest;

public class FruP3AutoForceRotation : ITriggerScript
{
    public bool Check(ScriptEnv scriptEnv, ITriggerCondParams condParams)
    {
        if (scriptEnv.KV.TryGetValue("lightPosition", out var lightPositionObj))
        {
            string direction = "";

            Vector3 lightPosition = (Vector3)lightPositionObj;

            float angle = MathF.Atan2(-(lightPosition.X - 100), -(lightPosition.Z - 100)); // 北为0，逆时针为正
            angle = NormalizeAngle(angle); // 归一化到 [-π, π]
            Core.Resolve<MemApiMove>().SetRot(angle);
            LogHelper.Print($"Auto face to {direction}，Rotation: {angle}");
            return true;
        }

        return false;
    }

    private static float NormalizeAngle(float angle)
    {
        while (angle <= -MathF.PI) angle += 2 * MathF.PI;
        while (angle > MathF.PI) angle -= 2 * MathF.PI;
        return angle;
    }
}