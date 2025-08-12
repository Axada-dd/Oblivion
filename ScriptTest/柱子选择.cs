using AEAssist.CombatRoutine.Trigger;
using AEAssist.CombatRoutine.Trigger.Node;
using Dalamud.Game.ClientState.Objects.Types;
using AEAssist;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using System.Numerics;
using AEAssist.CombatRoutine.Module;
using System.Collections.Generic;
using System.Linq;
using AEAssist.Extension;

namespace Oblivion.ScriptTest;

/// <summary>
/// 柱子选择脚本 - 根据角色职责和柱子状态自动选择目标
/// 处理12柱觉醒和34柱觉醒的情况，为不同职责分配不同的攻击目标
/// </summary>
public class 柱子选择 : ITriggerScript
{
    /// <summary>
    /// 检查并执行柱子选择逻辑
    /// </summary>
    /// <param name="scriptEnv">脚本环境，包含共享变量</param>
    /// <param name="condParams">触发条件参数</param>
    /// <returns>是否完成处理</returns>
    public bool Check(ScriptEnv scriptEnv, ITriggerCondParams condParams)
    {
        // 获取已标记的柱子列表，如果不存在或数量不正确则退出
        if (!scriptEnv.KV.TryGetValue("已标记柱子", out var 已标记柱子)) return false;
        List<uint>? 柱子列表 = 已标记柱子 as List<uint>;
        if (柱子列表 == null) return false;
        if (柱子列表.Count != 4) return false;
        // 获取Boss和四个柱子的引用
        var boss = TargetMgr.Instance.Units.Values.FirstOrDefault(e=>e.DataId == 8730 && e.IsTargetable);
        var 一柱 = TargetMgr.Instance.Enemys.Values.FirstOrDefault(e => e.EntityId == 柱子列表[2]);
        var 二柱 = TargetMgr.Instance.Enemys.Values.FirstOrDefault(e => e.EntityId == 柱子列表[1]);
        var 三柱 = TargetMgr.Instance.Enemys.Values.FirstOrDefault(e => e.EntityId == 柱子列表[3]);
        var 四柱 = TargetMgr.Instance.Enemys.Values.FirstOrDefault(e => e.EntityId == 柱子列表[0]);
        Share.TrustDebugPoint.Clear();
        //柱子列表顺序是右上开始顺时针1234
        
        // 如果所有柱子都不存在，则完成处理
        if (一柱 == null && 二柱 == null && 三柱 == null && 四柱 == null) return true;
        // 处理1、2号柱子觉醒的情况
        if (scriptEnv.KV.TryGetValue("12柱觉醒",out var p))
        {
            // D1和D3职责攻击1号柱子，如果不存在则攻击Boss
            if (AI.Instance.PartyRole is "D1" or "D3")
            {
                if (一柱 != null)
                {
                    if (!SetTarget(一柱)) return false;
                }
                else
                {
                    if (!SetTarget(boss)) return false;
                }
            }

            // D2和D4职责攻击2号柱子，如果不存在则攻击Boss
            if (AI.Instance.PartyRole is "D2" or "D4")
            {
                if (二柱 != null)
                {
                    if (!SetTarget(二柱)) return false;
                }
                else
                {
                    if (!SetTarget(boss)) return false;
                }
            }
            // 如果1、2号柱子都不存在，移除觉醒标记
            if (一柱 == null && 二柱 == null)
                scriptEnv.KV.Remove("12柱觉醒");
            return false;
        }

        // 处理3、4号柱子觉醒的情况
        if (scriptEnv.KV.TryGetValue("34柱觉醒", out var p1))
        {
            if (三柱 != null)
            {
                if (!SetTarget(三柱)) return false;
            }
            if (四柱 != null)
            {
                if (!SetTarget(四柱)) return false;
            }
            // 如果3、4号柱子都不存在，移除觉醒标记
            if (三柱 == null && 四柱 == null)
                scriptEnv.KV.Remove("34柱觉醒");
            return false;
        }
        // 坦克和治疗职责始终攻击Boss
        if (AI.Instance.PartyRole is "MT" or "ST" or "H1" or "H2")
        {
            if (!SetTarget(boss)) return false;
        }

        // 正常情况下，D1和D3职责攻击1号柱子（血量大于40%时），否则攻击Boss
        if (AI.Instance.PartyRole is "D1" or "D3")
        {
            if (一柱 != null && 一柱.CurrentHpPercent() > 0.4)
            {
                if (!SetTarget(一柱)) return false;
            }
            else
            {
                if (!SetTarget(boss)) return false;
            }
        }

        // 正常情况下，D2和D4职责攻击2号柱子（血量大于40%时），否则攻击Boss
        if (AI.Instance.PartyRole is "D2" or "D4")
        {
            if (二柱 != null && 二柱.CurrentHpPercent() > 0.4)
            {
                if (!SetTarget(二柱)) return false;
            }
            else
            {
                if (!SetTarget(boss)) return false;
            }
        }
        return false;
    }

    /// <summary>
    /// 设置目标，如果目标与当前目标不同
    /// </summary>
    /// <param name="target">要设置的目标</param>
    /// <returns>是否成功设置目标</returns>
    private bool SetTarget(IBattleChara? target)
    {
        if (target == null) return false;
        if (target == Core.Me.GetCurrTarget()) return false;
        Core.Resolve<MemApiTarget>().SetTarget(target);
        return true;
    }
}