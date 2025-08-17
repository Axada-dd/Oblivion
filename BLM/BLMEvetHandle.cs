using AEAssist.CombatRoutine.Module.AILoop;
using AEAssist.Function;
using AEAssist.IO;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM;

/// <summary>
/// 黑魔法师(BLM)职业的事件处理类，实现了IRotationEventHandler接口
/// 负责处理黑魔法师战斗中的各种事件和状态更新
/// </summary>
public class BLMEvetHandle : IRotationEventHandler
{
    private int 释放技能时状态 = 0;
    /// <summary>
    /// 黑魔法师GCD技能ID集合，用于识别和处理GCD技能的使用
    /// 包含所有冰系、火系、雷系以及其他GCD技能
    /// </summary>
    private readonly HashSet<uint> _gcdSpellIds = new HashSet<uint>
    {
        Skill.冰一,Skill.冰三,Skill.冰冻.GetActionChange(),Skill.冰澈,Skill.玄冰,
        Skill.火一,Skill.火三,Skill.火二.GetActionChange(),Skill.火四,Skill.核爆,Skill.绝望,Skill.耀星,
        Skill.异言,Skill.悖论,Skill.秽浊,Skill.雷一.GetActionChange(),Skill.雷二.GetActionChange(),Skill.崩溃,Skill.灵极魂
    };

    /// <summary>
    /// 黑魔法师能力技(oGCD)技能ID集合，用于识别和处理非GCD技能的使用
    /// 包含黑魔纹、三连咏唱、墨泉等辅助技能
    /// </summary>
    private readonly HashSet<uint> _ogcdSpellIds = new HashSet<uint>
    {
        Skill.黑魔纹,Skill.三连,Skill.墨泉,Skill.即刻,Skill.星灵移位,Skill.醒梦,Skill.详述
    };

    private readonly uint[] _fireSpellIds = new[]
    {
        Skill.核爆, Skill.火三, Skill.火四, Skill.绝望, Skill.火二.GetActionChange(), Skill.耀星, Skill.悖论
    };

    private readonly uint[] _bSpellIds = new[]
    {
        Skill.悖论, Skill.冰三, Skill.冰澈, Skill.冰冻.GetActionChange(), Skill.玄冰, Skill.灵极魂
    };

    public async Task OnPreCombat()
    {
        
        // 火状态下使用星灵移位
        if (BLMHelper.火状态)
        {
            await Skill.星灵移位.GetSpell(SpellTargetType.Self).Cast();
        }

        // 冰状态且条件满足时使用灵极魂
        if (BLMHelper.冰状态 && (BLMHelper.冰层数 < 3 || BLMHelper.冰针 < 3 || Core.Me.CurrentMp < 10000))
        {
            await Skill.灵极魂.GetSpell(SpellTargetType.Self).Cast();
        }
        await Task.CompletedTask;
    }


    public void OnResetBattle()
    {
        // 创建新的战斗数据实例并重置所有状态标志
        BattleData.Instance = new BattleData();
        BattleData.Instance.IsInnerOpener = false;
        BattleData.Instance.需要即刻 = false;
        BattleData.Instance.需要瞬发gcd = false;
        BattleData.Instance.正在特殊循环中 = false;
        // 重置QT状态
        QT.Reset();
    }


    public async Task OnNoTarget()
    {
        // 战斗时间小于10秒时不处理
        if (AI.Instance.BattleData.CurrBattleTimeInMs < 10 * 1000) return;
        
        // 重置技能状态标志
        BattleData.Instance.需要即刻 = false;
        BattleData.Instance.需要瞬发gcd = false;
        BattleData.Instance.正在特殊循环中 = false;
        
        // 处理Boss上天特殊情况
        if (QT.Instance.GetQt("Boss上天"))
        {
            if (BLMHelper.火状态)
            {
                await Skill.灵极魂.GetSpell(SpellTargetType.Self).Cast();
            }

            // 冰状态且条件满足时使用灵极魂
            if (BLMHelper.冰状态 && (BLMHelper.冰层数 < 3 || BLMHelper.冰针 < 3 || Core.Me.CurrentMp < 10000))
            {
                await Skill.灵极魂.GetSpell(SpellTargetType.Self).Cast();
            }
        }
        await Task.CompletedTask;
    }


    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
        if (_gcdSpellIds.Contains(spell.Id))
            BattleData.Instance.已使用瞬发 = GCDHelper.GetGCDCooldown() >= (Core.Me.HasAura(Buffs.咏速Buff) ? 1500 : 1700);
    }


    public void AfterSpell(Slot slot, Spell spell)
    {
        if (释放技能时状态 == 1)
        {
            if (spell.Id == Skill.火二.GetActionChange() || spell.Id == Skill.火三 || spell.Id == Skill.星灵移位)
            {

                BattleData.Instance.上一轮循环 = new List<uint>(BattleData.Instance.冰状态gcd);
                BattleData.Instance.冰状态gcd.Clear();
                if (spell.Id == Skill.火二.GetActionChange() || spell.Id == Skill.火三)
                {
                    BattleData.Instance.火状态gcd.Add(spell.Id);
                }
            }
            else
            {
                BattleData.Instance.冰状态gcd.Add(spell.Id);
            }
        }else if (释放技能时状态 == 2)
        {
            if (spell.Id == Skill.冰冻.GetActionChange() || spell.Id == Skill.冰三 || spell.Id == Skill.星灵移位)
            {
                BattleData.Instance.上一轮循环 = new List<uint>(BattleData.Instance.火状态gcd);
                BattleData.Instance.火状态gcd.Clear();
                if (spell.Id == Skill.冰冻.GetActionChange() || spell.Id == Skill.冰三)
                {
                    BattleData.Instance.冰状态gcd.Add(spell.Id);
                }
            }
            else
            {
                BattleData.Instance.火状态gcd.Add(spell.Id);
            }
        }else if (释放技能时状态 == 0)
        {
            BattleData.Instance.冰状态gcd.Clear();
            BattleData.Instance.火状态gcd.Clear();
            if (_fireSpellIds.Contains(spell.Id))
            {
                BattleData.Instance.火状态gcd.Add(spell.Id);
            }else if (_bSpellIds.Contains(spell.Id))
            {
                BattleData.Instance.冰状态gcd.Add(spell.Id);
            }
        }
        if (spell.Id == Skill.星灵移位)
        {
            if (BLMHelper.冰状态) 释放技能时状态 = 1;
            else if (BLMHelper.火状态) 释放技能时状态 = 2;
            else 释放技能时状态 = 0;
            if (BLMHelper.冰状态 && BLMHelper.冰针 == 3)
            {
                BattleData.Instance.三冰针进冰 = true;
            }
        }
        // 处理GCD技能施放后的状态
        if (_gcdSpellIds.Contains(spell.Id))
        {
            // 记录前一个GCD技能ID
            BattleData.Instance.前一gcd = spell.Id;
            // 重置GCD技能计数
            AI.Instance.BattleData.CurrGcdAbilityCount = 1;
            // 根据GCD冷却时间判断是否使用了瞬发技能
            // 有咏速buff时阈值为1500ms，否则为1700ms
            BattleData.Instance.已使用瞬发 = GCDHelper.GetGCDCooldown() >= (Core.Me.HasAura(Buffs.咏速Buff) ? 1500 : 1700);
            if (BLMHelper.冰状态) 释放技能时状态 = 1;
            else if (BLMHelper.火状态) 释放技能时状态 = 2;
            else 释放技能时状态 = 0;
            
        }
        if (BattleData.Instance.三冰针进冰)
        {
            if (spell.Id == Skill.冰澈 || spell.Id == Skill.玄冰)
            {
                BattleData.Instance.三冰针进冰 = false;
            }
        }

        
        // 处理能力技(oGCD)施放后的状态
        if (_ogcdSpellIds.Contains(spell.Id))
        {
            // 记录前一个能力技ID
            BattleData.Instance.前一能力技 = spell.Id;
        }
        
        // 处理瞬发技能使用后的状态
        if (BattleData.Instance.已使用瞬发)
        {
            // 重置瞬发GCD需求标志
            BattleData.Instance.需要瞬发gcd = false;
            // 更新GCD技能计数
            AI.Instance.BattleData.CurrGcdAbilityCount = 2;

        }
    }


    public void OnBattleUpdate(int currTimeInMs)
    {
        
        // 可瞬发状态下不需要即刻
        if (Helper.可瞬发()) BattleData.Instance.需要即刻 = false;
        
        // 处理角色发呆状态
        if (BLMHelper.在发呆())
        {
            // 有可用瞬发技能时设置需要瞬发GCD标志
            if (BLMHelper.可用瞬发() != 0)
                BattleData.Instance.需要瞬发gcd = true;
            else
            {
                BattleData.Instance.需要即刻 = true;
            }
        }
        
        // 正在施法时重置相关状态
        if (Core.Me.IsCasting)
        {
            BattleData.Instance.需要即刻 = false;
            BattleData.Instance.已使用瞬发 = false;
            BattleData.Instance.需要瞬发gcd = false;
        }
        


        // 处理特殊循环状态
        if (!BattleData.Instance.特供循环) BattleData.Instance.正在特殊循环中 = false;

        // 更新各种战斗状态数据
        /*BattleData.Instance.已存在黑魔纹 = Helper.有buff(737);
        BattleData.Instance.能使用耀星 = BLMHelper.能使用耀星();
        BattleData.Instance.能使用的火四个数 = BLMHelper.能使用的火四个数();
        BattleData.Instance.火循环剩余gcd = BLMHelper.火循环gcd();
        BattleData.Instance.冰循环剩余gcd = BLMHelper.冰循环gcd();
        BattleData.Instance.能星灵转冰 = BLMHelper.能星灵转冰();*/
    }


    public void OnEnterRotation()
    {
        // 输出欢迎信息
        LogHelper.Print(
            "欢迎使用OVO的黑魔acr，反馈请到：");
        LogHelper.Print("建议设置提前使用gcd时间为50，使用fuckanime三插，DR能力技动画减少");

        // 检查全局设置并给出建议
        if (Helper.GlobalSettings.NoClipGCD3)
            LogHelper.PrintError("建议不要在acr全局设置中勾选【全局能力技不卡GCD】选项");
        
        // 重置开场标志
        BattleData.Instance.IsInnerOpener = false;

    }


    public void OnExitRotation()
    {
        // 保存黑魔法师设置
        BLMSetting.Instance.Save();
        // 保存QT状态
        BLMSetting.Instance.SaveQtStates(QT.Instance);
    }

    public void OnTerritoryChanged()
    {
        // 重置QT状态
        QT.Reset();
    }
}