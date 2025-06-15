# ISlotResolver类Check方法返回值说明

## 耀星类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | Spells.耀星.GetSpell().IsReadyWithCanCast() 为 false                 |
| -6     | BLMHelper.火状态 为 false                                           |
| 1      | BLMHelper.耀星层数 == 6 且 Helper.IsMove 为 false                    |
| 2      | BLMHelper.耀星层数 == 6 且 Helper.IsMove 为 true 且 BattleData.Instance.可瞬发 |
| -99    | 以上条件均不满足                                                     |

## 悖论类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | Spells.悖论.GetSpell().IsReadyWithCanCast() 为 false                 |
| -6     | BLMHelper.悖论指示 为 false                                          |
| 1      | BLMHelper.冰状态 为 true 且 BLMHelper.冰层数 == 3 且 BLMHelper.冰针 == 3 |
| 2      | BLMHelper.冰状态 为 true 且 Helper.IsMove 为 true                    |
| 3      | BLMHelper.火状态 为 true 且 BLMHelper.火层数 == 3 且 BattleData.Instance.已使用耀星 且 Core.Me.CurrentMp >= 2400L |
| 4      | BLMHelper.火状态 为 true 且 BLMHelper.火层数 == 3 且 BLMHelper.耀星层数 >= 3 且 Helper.IsMove |
| 5      | BLMHelper.火状态 为 true 且 BLMHelper.火层数 == 3 且 Helper.IsMove    |
| -99    | 以上条件均不满足                                                     |

## 即刻类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | Spells.即刻.GetSpell().IsReadyWithCanCast() 为 false                 |
| 1      | BLMHelper.火状态 为 true 且 BattleData.Instance.已使用耀星 且 BattleData.Instance.已使用瞬发 且 Core.Me.CurrentMp < 800 且 GCDHelper.GetGCDCooldown() > 1500 |
| 2      | BLMHelper.冰状态 为 true 且 BLMHelper.冰层数 < 3 且 BattleData.Instance.已使用瞬发 |
| -99    | 以上条件均不满足                                                     |

## 详述类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | Spells.详述.GetSpell().IsReadyWithCanCast() 为 false                 |
| -2     | BLMHelper.通晓层数 >= 3 或 (BLMHelper.耀星层数 == 2 且 BLMHelper.通晓剩余时间 < 4000) |
| 1      | 以上条件均不满足                                                     |