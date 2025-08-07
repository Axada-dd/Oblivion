using System.Numerics;
using AEAssist.CombatRoutine.Module.Target;

namespace Oblivion.Common;

public static class FullAutoTools
{
    public static async Task ReSlidTpAndDebug(string role, Vector3 pos,int delay, int slidTime = 700 )
    {
        if (delay > 0)
        {
            await Coroutine.Instance.WaitAsync(delay);
        }
        Share.TrustDebugPoint.Add(pos);
        if (RemoteControlHelper.GetRoleByPlayerName(Core.Me.Name.TextValue) != "")
        {
            if(slidTime > 0)
                RemoteControlHelper.SlideTp(role, pos, slidTime);
            else
                RemoteControlHelper.SetPos(role, pos);
        }
        
    }

    public static async Task ReMoveToAndDebug(string role, Vector3 pos, int delay)
    {
        if (delay > 0)
        {
            await Coroutine.Instance.WaitAsync(delay);
        }
        Share.TrustDebugPoint.Add(pos);
        if (RemoteControlHelper.GetRoleByPlayerName(Core.Me.Name.TextValue) != "")
        {
            RemoteControlHelper.SetPos(role, pos);
        }
    }
    public static Vector3 RotatePoint(Vector3 point, Vector3 centre, float radian)
    {

        Vector2 v2 = new(point.X - centre.X, point.Z - centre.Z);

        var rot = (MathF.PI - MathF.Atan2(v2.X, v2.Y) + radian);
        var lenth = v2.Length();
        return new(centre.X + MathF.Sin(rot) * lenth, centre.Y, centre.Z - MathF.Cos(rot) * lenth);
    }
    public static int PositionTo8Dir(Vector3 point, Vector3 centre)
    {
        // Dirs: N = 0, NE = 1, ..., NW = 7
        var r = Math.Round(4 - 4 * Math.Atan2(point.X - centre.X, point.Z - centre.Z) / Math.PI) % 8;
        return (int)r;

    }
}