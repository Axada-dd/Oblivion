using AEAssist.CombatRoutine.Trigger;
using AEAssist.CombatRoutine.Trigger.Node;
using System.Linq;
using AEAssist;
using Dalamud.Game.ClientState.Objects.Types;

namespace Oblivion.ScriptTest;

/// <summary>
/// M7S自动选择器 - 根据玩家角色和场上敌人情况自动选择目标
/// 针对不同职责(ST、MT、D1、D2等)设置不同的目标优先级
/// </summary>
public class M7SAutoSelecter : ITriggerScript
{
    private float _maxDistance = 5;
    /// <summary>
    /// 检查并执行自动目标选择逻辑
    /// </summary>
    /// <param name="scriptEnv">脚本环境，包含共享变量</param>
    /// <param name="condParams">触发条件参数</param>
    /// <returns>是否完成处理</returns>
    public bool Check(ScriptEnv scriptEnv, ITriggerCondParams condParams)
    {
        if (Core.Me.IsDead) return false;
        if (!Core.Me.IsMelee())
        {
            _maxDistance = 25f;
        }
        Core.Me.SetTarget(AutoTarget());
        return false;
    }

    /// <summary>
    /// 根据角色和战斗情况自动选择最佳目标
    /// </summary>
    /// <returns>选择的目标对象</returns>
    private IBattleChara AutoTarget()
    {
        // 获取指定距离内的所有敌人，并按照距离排序
        var enemies = ECHelper.Objects
            .Where(obj => obj is IBattleChara)
            .Cast<IBattleChara>()
            .Where(c => c.DistanceToPlayer() < _maxDistance && c.IsEnemy())
            .ToArray()
            .OrderBy(c => c.DistanceToPlayer());

        // 获取场上的各种敌人对象
        // 鱼类敌人 (DataId: 18346) - 按不同状态分类
        var 无目标鱼 = enemies.FirstOrDefault(c => c.DataId == 18346 && c.TargetObject == null);
        var 有目标鱼 = enemies.FirstOrDefault(c => c.DataId == 18346);
        var 所有鱼 = enemies.Where(c => c.DataId == 18346).ToArray();
        var 自己有鱼 = enemies.Any(c => c.DataId == 18346 && c.TargetObject?.IsMe() == true);
        var 非自己鱼 = enemies.Any(c => c.DataId == 18346 && !c.TargetObject?.IsMe() == true);
        var 治疗鱼 = enemies.FirstOrDefault(c => c.DataId == 18346 && c.GetCurrTarget()?.IsHealer() == true);
        // 其他敌人类型
        var 哈基米 = enemies.FirstOrDefault(c => c.DataId == 18347);
        var 羊 = enemies.FirstOrDefault(c => c.DataId == 18344);
        var 马 = enemies.FirstOrDefault(c => c.DataId == 18345);
        var boss = enemies.FirstOrDefault(c => c.DataId == 18335);
        // 获取当前战斗时间（毫秒）
        var battleTime = AI.Instance.BattleData.CurrBattleTimeInMs;
        // ST(副坦)的目标选择逻辑
        if (AI.Instance.PartyRole == "ST")
        {
            if (羊 != null) return 羊;
            if (羊 == null && 有目标鱼 != null && 所有鱼.Length >= 2) return 有目标鱼;
            if (boss != null) return boss;
        }

        // MT(主坦)的目标选择逻辑
        if (AI.Instance.PartyRole == "MT")
        {
            if (马 != null) return 马;
            if (有目标鱼 != null && 所有鱼.Length >= 2) return 有目标鱼;
            if (boss != null) return boss;
        }
        // D1和D2(输出1和2)的目标选择逻辑
        if (AI.Instance.PartyRole == "D1" || AI.Instance.PartyRole == "D2")
        {
            if (马 != null) return 马;
            if (有目标鱼 != null && 所有鱼.Length >= 2) return 有目标鱼;
            if (羊 != null) return 羊;
            if (boss != null) return boss;
        }

        // H2(治疗2)的目标选择逻辑
        if (AI.Instance.PartyRole == "H2")
        {
            if (!自己有鱼 && 无目标鱼 != null && 所有鱼.Length >= 2)
            {
                if (battleTime > 250 * 1000 && battleTime < 300 * 1000) return 无目标鱼;
            }
            if (马 != null) return 马;
            if (哈基米 != null) return 哈基米;
            if (有目标鱼 != null && 所有鱼.Length >= 2) return 有目标鱼;
            if (羊 != null) return 羊;
            if (boss != null) return boss;
        }
        // D4(输出4)的目标选择逻辑
        if (AI.Instance.PartyRole == "D4")
        {
            if (!自己有鱼 && 无目标鱼 != null && 所有鱼.Length >= 2)
            {
                if (battleTime > 300 * 1000) return 无目标鱼;
            }
            if (马 != null) return 马;
            if (battleTime > 250 * 1000 && battleTime < 300 * 1000) return 治疗鱼;
            if (哈基米 != null) return 哈基米;
            if (有目标鱼 != null && 所有鱼.Length >= 2) return 有目标鱼;
            if (羊 != null) return 羊;
            if (boss != null) return boss;
        }
        // D3(输出3)的目标选择逻辑
        if (AI.Instance.PartyRole == "D3")
        {
            if (!自己有鱼 && 无目标鱼 != null && 所有鱼.Length >= 2)
            {
                return 无目标鱼;
            }

            if (马 != null) return 马;
            if (哈基米 != null) return 哈基米;
            if (有目标鱼 != null && 所有鱼.Length >= 2) return 有目标鱼;
            if (羊 != null) return 羊;
            if (boss != null) return boss;
        }

        // 如果没有找到合适的目标，返回当前目标
        return Core.Me.GetCurrTarget();
    }
}