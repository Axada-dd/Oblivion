using AEAssist.CombatRoutine.Trigger;
using AEAssist.CombatRoutine.Trigger.Node;
using Dalamud.Game.ClientState.Objects.Types;

namespace Oblivion.ScriptTest;

public class M7SAutoSelecter:ITriggerScript
{
    private  float _maxDistance = 5;
    public bool Check(ScriptEnv scriptEnv, ITriggerCondParams condParams)
    {
        if (Core.Me.IsDead) return false;
        if(!Core.Me.IsMelee())
        { 
            _maxDistance = 25f;
        }
        Core.Me.SetTarget(AutoTarget());
        return false;
    }

    private IBattleChara AutoTarget()
    {
        var enemies = ECHelper.Objects
            .Where(obj => obj is IBattleChara)
            .Cast<IBattleChara>()
            .Where(c => c.DistanceToPlayer() < _maxDistance && c.IsEnemy())
            .ToArray()
            .OrderBy(c =>c.DistanceToPlayer());
        
        var 无目标鱼 = enemies.FirstOrDefault(c => c.DataId == 18346 && c.TargetObject == null);
        var 有目标鱼 = enemies.FirstOrDefault(c => c.DataId == 18346);
        var 所有鱼 = enemies.Where(c => c.DataId == 18346).ToArray();
        var 自己有鱼 = enemies.Any(c => c.DataId == 18346 && c.TargetObject?.IsMe() == true);
        var 非自己鱼 = enemies.Any(c => c.DataId == 18346 && !c.TargetObject?.IsMe() == true);
        var 治疗鱼 = enemies.FirstOrDefault(c => c.DataId == 18346 && c.GetCurrTarget()?.IsHealer() == true);
        var 哈基米 = enemies.FirstOrDefault(c => c.DataId == 18347);
        var 羊 = enemies.FirstOrDefault(c => c.DataId == 18344);
        var 马 = enemies.FirstOrDefault(c => c.DataId == 18345);
        var boss = enemies.FirstOrDefault(c => c.DataId == 18335);
        var battleTime = AI.Instance.BattleData.CurrBattleTimeInMs;
        if (AI.Instance.PartyRole == "ST")
        {
            if (羊 != null) return 羊;
            if (羊 == null&&有目标鱼 != null && 所有鱼.Length >= 2) return 有目标鱼;
            if (boss != null) return boss;
        }

        if (AI.Instance.PartyRole == "MT")
        {
            if (马 != null) return 马;
            if (有目标鱼 != null && 所有鱼.Length >= 2) return 有目标鱼;
            if (boss != null) return boss;
        }
        if (AI.Instance.PartyRole == "D1" || AI.Instance.PartyRole == "D2")
        {
            if (马 != null) return 马;
            if (有目标鱼 != null && 所有鱼.Length >= 2) return 有目标鱼;
            if (羊 != null) return 羊;
            if (boss != null) return boss;
        }

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
        if (AI.Instance.PartyRole == "D4")
        {
            if (!自己有鱼 && 无目标鱼 != null && 所有鱼.Length >= 2)
            {
                if(battleTime > 300*1000) return 无目标鱼;
            }
            if (马 != null) return 马;
            if (battleTime > 250 * 1000 && battleTime < 300 * 1000) return 治疗鱼;
            if (哈基米 != null) return 哈基米;
            if (有目标鱼 != null && 所有鱼.Length >= 2) return 有目标鱼;
            if (羊 != null) return 羊;
            if (boss != null) return boss;
        }
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

        return Core.Me.GetCurrTarget();
    }
}