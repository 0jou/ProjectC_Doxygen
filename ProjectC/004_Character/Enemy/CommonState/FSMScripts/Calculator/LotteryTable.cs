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
    [SerializeField] private OutputSlotAttackData _UseAttackData = new();

    public FlexibleTransform _targetTrans = new();
    public FlexibleAttackData _firstAttackData = new();
    public FlexibleAttackData _secondAttackData = new();

    private CharacterCore _characterCore;

    public override void OnCalculate()
    {
        if (_targetTrans.value == null)
        {
            _Output.SetValue(1);
            _UseAttackData.SetValue(_firstAttackData.value);
            return;
        }


        if (_secondAttackData.value == null || _secondAttackData.value.LotteryData.value.Count == 0)
        {
            LotteryAttack(_firstAttackData);
        }
        else
        {
            if (!_characterCore)
            {
                if (!transform.root.TryGetComponent(out _characterCore))
                {
                    _Output.SetValue(1);
                    _UseAttackData.SetValue(_firstAttackData.value);
                    return;
                }
            }

            if (_characterCore.Status.m_hp.Value / _characterCore.Status.MaxHP >= 0.5f)
            {
                LotteryAttack(_firstAttackData);
            }
            else
            {
                LotteryAttack(_secondAttackData);
            }
        }
    }

    private void LotteryAttack(FlexibleAttackData useData)
    {
        if (useData.value.LotteryData.value.Count == 0)
        {
            Debug.LogError("攻撃の抽選データが渡されていません" + transform.root.name);
        }
        _UseAttackData.SetValue(useData.value);

        float sumWeight = 0;
        float weight;
        float distAttackPoint;
        float toTargetDist = Vector3.Distance(transform.position, _targetTrans.value.position);

        // 各攻撃の抽選値を決定
        foreach (var attackData in useData.value.LotteryData.value)
        {
            distAttackPoint = Mathf.Abs(toTargetDist - attackData.m_bestAttackDist.value);
            distAttackPoint = Mathf.Clamp(distAttackPoint, 0.5f, 2.0f);
            weight = attackData.m_defaultWeight.value
                + attackData.m_weightInclude.value * attackData.LoseCount;
            weight /= distAttackPoint;
            sumWeight += weight;
            attackData.m_lotteryWeight = weight;
        }

        AttackLotteryData data;
        float rand = Random.Range(0.0f, sumWeight);
        for (int i = 0; i < useData.value.LotteryData.value.Count; ++i)
        {
            data = useData.value.LotteryData.value[i];
            data.LoseCount++;
            if (rand >= 0.0f && rand < data.m_lotteryWeight)
            {
                _Output.SetValue(i + 1);
                data.LoseCount = 0;
            }
            rand -= data.m_lotteryWeight;
            useData.value.LotteryData.value[i] = data;
        }
    }
}
