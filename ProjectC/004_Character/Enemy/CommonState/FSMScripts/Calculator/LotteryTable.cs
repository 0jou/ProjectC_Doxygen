/**
* @file LotteryTable
* @brief 敵の攻撃抽選処理
*/
using UnityEngine;
using Arbor;
using Unity.VisualScripting.FullSerializer;

[AddComponentMenu("")]
[AddBehaviourMenu("Random/LotteryTable")]
[BehaviourTitle("Random.LotteryTable")]
[BuiltInBehaviour]
/**
* @brief 敵の攻撃抽選処理（伊波）
* @details ArborのCakculatorで抽選を行う　結果はint型で返す
*/
public class LotteryTable : Calculator
{
    [SerializeField] private OutputSlotInt _Output = new();

    [SerializeField]
    [SlotType(typeof(EnemyParameters))]
    private FlexibleComponent m_enemyParameters;

    private CharacterCore _characterCore;
    private EnemyParameters _enemyParameter;

    public override void OnCalculate()
    {
        if (_enemyParameter == null)
        {
            _enemyParameter = m_enemyParameters.value as EnemyParameters;
            if (!_enemyParameter)
            {
                Debug.LogError("EnemyParametersがセットされていません" + gameObject.name);
                return;
            }
        }

        if (_enemyParameter.Target == null)
        {
            _Output.SetValue(1);
            _enemyParameter.NowAttackData = _enemyParameter.GetFirstAttackData();
            return;
        }


        if (_enemyParameter.ValidSecondAttack)
        {
            if (!_characterCore)
            {
                if (!_enemyParameter.transform.TryGetComponent(out _characterCore))
                {
                    _Output.SetValue(1);
                    _enemyParameter.NowAttackData = _enemyParameter.GetFirstAttackData();
                    return;
                }
            }

            if(_enemyParameter.NowAttackDataType==1)
            {
                if (_characterCore.Status.m_hp.Value / _characterCore.Status.MaxHP >= 0.5f)
                {
                    LotteryAttack(_enemyParameter.NowAttackData);
                }
                else
                {
                    _enemyParameter.NowAttackDataType = 2;
                    _enemyParameter.NowAttackData= _enemyParameter.GetSecondAttackData();
                    LotteryAttack(_enemyParameter.NowAttackData);
                }
            }
            else
            {
                LotteryAttack(_enemyParameter.NowAttackData);
            }
          
        }
        else
        {
            LotteryAttack(_enemyParameter.NowAttackData);
        }
    }

    private void LotteryAttack(EnemyAttackData useData)
    {
        if (useData.LotteryData.Count == 0)
        {
            Debug.LogError("攻撃の抽選データが渡されていません" + transform.root.name);
        }

        float sumWeight = 0;
        float weight;
        float distAttackPoint;
        float toTargetDist = Vector3.Distance(transform.position, _enemyParameter.Target.position);

        // 各攻撃の抽選値を決定
        foreach (var attackData in useData.LotteryData)
        {
            distAttackPoint = Mathf.Abs(toTargetDist - attackData.m_bestAttackDist);
            distAttackPoint = Mathf.Clamp(distAttackPoint, 0.5f, 2.0f);
            weight = attackData.m_defaultWeight
                + attackData.m_weightInclude * attackData.LoseCount;
            weight /= distAttackPoint;
            sumWeight += weight;
            attackData.m_lotteryWeight = weight;
        }

        EnemyAttackData.AttackLotteryData data;
        float rand = Random.Range(0.0f, sumWeight);
        for (int i = 0; i < useData.LotteryData.Count; ++i)
        {
            data = useData.LotteryData[i];
            data.LoseCount++;
            if (rand >= 0.0f && rand < data.m_lotteryWeight)
            {
                _Output.SetValue(i + 1);
                data.LoseCount = 0;
            }
            rand -= data.m_lotteryWeight;
            useData.LotteryData[i] = data;
        }
    }
}
