using System.Numerics;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.CombatRoutine.Trigger.Node;
using AEAssist.MemoryApi;
using AEAssist.Helper;
using AEAssist;

namespace Oblivion.ScriptTest;

public class FruP3AutoForceRotation : ITriggerScript
{
    public bool Check(ScriptEnv scriptEnv, ITriggerCondParams condParams)
    {
        if (scriptEnv.KV.TryGetValue("lightPosition", out var lightPositionObj))
        {
            string direction = "";

            Vector3 lightPosition = (Vector3)lightPositionObj;
            var lightIndex = PositionTo8Dir(lightPosition, new Vector3(100, 0, 100));
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

            Core.Resolve<MemApiMove>().SetRot(myRot);
            LogHelper.Print($"灯在：{lightIndex} 面向：{myRot}");
            return true;
        }

        return false;
    }

    private int PositionTo8Dir(Vector3 point, Vector3 centre)
    {
        // Dirs: N = 0, NE = 1, ..., NW = 7
        var r = Math.Round(4 - 4 * Math.Atan2(point.X - centre.X, point.Z - centre.Z) / Math.PI) % 8;
        return (int)r;

    }
}