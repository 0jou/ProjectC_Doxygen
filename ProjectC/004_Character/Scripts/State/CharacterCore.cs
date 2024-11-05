using Arbor;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.VFX;
using SaintsField;


// キャラクターの基盤クラス
public partial class CharacterCore : MonoBehaviour, IDamageable
{
    // グループナンバー
    [SerializeField] private CharacterGroupNumber m_groupNo;
    public CharacterGroupNumber GroupNo => m_groupNo;

    private bool m_doFriendlyFire = false;
    public bool DoFriendlyFire { get { return m_doFriendlyFire; } set { m_doFriendlyFire = value; } }

    [SerializeField] private MyCharacterController m_charaCtrl;
    public MyCharacterController CharaCtrl { get { return m_charaCtrl; } }
    [SerializeField] public Animator m_animator;

    // 入力インターフェース
    private IInputProvider m_inputProvider = NullCharacterIP.NullInstance;

    // どの入力インターフェースを使うか
    public enum InputTypes
    {
        None,
        [InspectorName("プレイヤー")] Player,
        [InspectorName("敵")] Enemy,
        [InspectorName("スキルキャラクター")] SkillChara,
        [InspectorName("経営")] Management,
    }
    [SerializeField] private InputTypes m_inputType;
    public InputTypes InputType { get { return m_inputType; } }

    // 移動の種類
    public enum MoveTypes
    {
        Loose,  // 緩やかに移動
        Sharp,  // びたびたに移動
    }

    [SerializeField]
    private MoveTypes m_moveType = MoveTypes.Loose;

    public MoveTypes MoveType { get { return m_moveType; } }


    // ヒットストップ用プロパティ
    public float HitStopRemainingTime
    {
        get; set;
    }

    //ヒットストップするためのプレイヤーの攻撃エフェクト保存用（山本）
    public VisualEffect TempPlayerAttackEffect
    {
        get; set;
    }


    //=====================
    // ヒット時演出用
    // ====================
    // マテリアルの色パラメーターのID 文字列は、使用シェーダーの詳細のPropertyから
    private static readonly int PROPERTY_COLOR = Shader.PropertyToID("_BaseColor");
    // モデルのRenderer
    [SerializeField] private Renderer m_renderer;
    // モデルのマテリアル複製
    private Material m_material;
    // DOTween用
    private DG.Tweening.Sequence m_seq;

    // 移動用変数
    // 移動速度
    [SerializeField] private VisualEffect m_dashEffect; // ダッシュ用エフェクト

    private bool m_isRun = false;
    //ダッシュキー押しっぱなしを防ぐ用（山本）
    private bool m_isPush = false;
    private bool m_isNoKnockBack = false;
    private float m_knockBackMultiplier = 1;
    private Vector3 m_knockBackVec;
    bool m_isNoDamage = false;
    private bool m_isNoStamina = false; // スタミナなくて動けない状態

    [ShowIf(nameof(m_groupNo) + "!=", CharacterGroupNumber.enemy)]
    [SerializeField] private CharacterStatus m_characterStatus;
    public CharacterStatus Status { get { return m_characterStatus; } }

    [Header("各キャラクター専用のパラメータ")]
    [Tooltip("プレイヤーだった場合のみ参照をいれる")]
    public PlayerParameters PlayerParameters;

    [Tooltip("敵だった場合のみ参照をいれる")]
    public EnemyParameters EnemyParameters;

    [Tooltip("スキルだった場合のみ参照をいれる")]
    public PlayerSkillsParameters PlayerSkillsParameters;


    // TODO:リスタート時の初期化関連　伊波
    // 展示会終わったら消す
    private Vector3 m_initPos;
    public void EnemyResetPos()
    {
        if (m_inputType != InputTypes.Enemy) return;
        m_charaCtrl.SetPositionMotor(m_initPos);
    }


    private void Awake()
    {
        if (m_animator)
        {
            // 非アクティブにした時に、アニメーターステートが初期化されるのを防ぐ
            m_animator.keepAnimatorStateOnDisable = true;
        }

        if (m_renderer)
        {
            // materialにアクセスして、自動生成されるマテリアルを保持
            m_material = m_renderer.material;
        }

        //装備しているスキルから消費BPを取得する
        SetStorySkillBP();

        m_characterStatus.MaxHP = m_characterStatus.m_hp.Value;

        //キャラクターを正面へ回転(山本)
        SetRotateToTarget(transform.forward);
    }

    void Start()
    {
        m_initPos = transform.position;

        IMetaAI<CharacterCore>.Instance.RegisterObject(this);

        // インスペクターで選択した入力インターフェースに応じて、選択
        if (m_inputType == InputTypes.Player)
        {
            m_inputProvider = new PlayerInputProvider();

            //スタート時にリスタート位置,向きを記録する
            if (gameObject.TryGetComponent(out PlayerParameters parameters))
            {
                parameters.PlayerRestartPosition = gameObject.transform.root.position;
                parameters.PlayerRestartForward = gameObject.transform.root.forward;
            }
        }
        else if (m_inputType == InputTypes.Enemy || m_inputType == InputTypes.SkillChara || m_inputType == InputTypes.Management)
        {
            m_inputProvider = GetComponentInChildren<EnemyInputProvider>();
        }

        // ダッシュエフェクトがセットされていれば、最初は止めておく
        if (m_dashEffect != null)
        {
            m_dashEffect.Stop();
        }

        if (m_groupNo == CharacterGroupNumber.enemy && EnemyParameters)
        {
            m_characterStatus = new(EnemyParameters.GetEnemyData().CharacterStatus);
        }
        m_characterStatus.MaxHP = m_characterStatus.m_hp.Value;

        // アップデート処理
        this.UpdateAsObservable()
            .Where(_ => enabled)
            .Subscribe(_ =>
            {
                // ヒットストップ処理 Animatorのスピードで再現
                if (HitStopRemainingTime <= 0)
                {
                    if (m_animator)
                    {
                        m_animator.speed = 1.0f;
                    }

                    //攻撃エフェクトが保存されていて、HitStop時間外なら（山本）
                    if (TempPlayerAttackEffect)
                    {
                        //if (TempPlayerAttackEffect.HasFloat("AddAngle"))
                        //{
                        //    if (TempPlayerAttackEffect.transform.TryGetComponent(out AttackEffectRegistration effct))
                        //    {
                        //        //AddAngleがあったら元に戻す
                        //        TempPlayerAttackEffect.SetFloat("AddAngle", effct.OriginAddAngle);
                        //    }
                        //}

                        if (TempPlayerAttackEffect.transform.TryGetComponent(out AttackEffectRegistration effct))
                        {
                            //変更したVFXのプロパティを元の値へと戻す
                            effct.ReturnVFXPropertiesValue();
                        }

                        //AttackEffectのスピードを元に戻す
                        TempPlayerAttackEffect.playRate = 1.0f;
                    }

                }
                else
                {
                    if (m_animator)
                    {
                        m_animator.speed = 0.0f;
                    }

                    //攻撃エフェクトが保存されていて、HitStop時間内なら（山本）
                    if (TempPlayerAttackEffect)
                    {
                        //if (TempPlayerAttackEffect.HasFloat("AddAngle"))
                        //{
                        //    //AddAngleがあったら0.0fに変更
                        //    TempPlayerAttackEffect.SetFloat("AddAngle", 0.0f);
                        //}

                        if (TempPlayerAttackEffect.transform.TryGetComponent(out AttackEffectRegistration effct))
                        {
                            //変更したVFXのプロパティを指定した値へと変更
                            effct.SetVFXPropertiesValue(0.0f);
                        }

                        //AttackEffectのスピードを止める
                        TempPlayerAttackEffect.playRate = 0.0f;

                    }

                }

                HitStopRemainingTime -= Time.deltaTime;
                if (HitStopRemainingTime < 0) HitStopRemainingTime = 0;
            }
            );
    }

    // ダメージ処理
    void IDamageable.Damaged(DamageNotification _dmgData, Collider _hitCol, float _knockBackMultiplier, bool _isStrongAttack)
    {
        //ノーダメージフラグ中に早期リターン（山本）
        if (m_isNoDamage) { return; }


        //死亡判定とスキル等による怯み判定がバッティングした際に強制的に死亡状態へと移行させる処理（山本）
        if (m_characterStatus.m_hp.Value <= 0.0f)
        {
            //死亡トリガーを逃れてしまったら強制的に死亡状態へと移行する（山本）
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            {
                m_characterStatus.m_hp.Value = 0.0f;
                m_animator.SetTrigger("IsDead");
                //HP0でもアタックエフェクトが表示されるようにする（山本）
                _dmgData.m_replyIsHit = true;
            }
            return;
        }

        if (m_characterStatus.m_hp.Value > 0)
        {
            // ダメージ処理
            Transform managerObj = transform.root.Find("ConditionManager");
            if (managerObj != null && managerObj.TryGetComponent(out ConditionManager manager))
            {
                m_characterStatus.m_hp.Value -= _dmgData.m_damage * manager.DamageMulti();
            }
            else
            {
                m_characterStatus.m_hp.Value -= _dmgData.m_damage;
            }

            //!@todo :ヒットエフェクトが出来たらここは消す
            if (_hitCol)
            {
                SoundManager.Instance.Start3DPlayback("HitSoundBlow", _hitCol.transform.position);
            }

            // ヒットストップ処理
            HitStopRemainingTime = _dmgData.m_hitStopTime;
            // ヒット時演出
            HitFadeBlink(Color.red);
        }


        // 0以下で、倒れる
        if (m_characterStatus.m_hp.Value <= 0.0f)
        {
            m_characterStatus.m_hp.Value = 0.0f;
            m_animator.SetBool("IsDead", true);
            //HP0でもアタックエフェクトが表示されるようにする（山本）
            _dmgData.m_replyIsHit = true;
            return;
        }


        if (m_characterStatus.m_hp.Value > 0)
        {
            // ノックバック処理
            if (_hitCol)
            {
                if (_isStrongAttack)
                {
                    m_animator.SetTrigger("BlowAway");
                }
                else if (!m_isNoKnockBack)
                {
                    m_knockBackVec = _hitCol.transform.forward;
                    m_knockBackVec.y = 0.0f;
                    m_knockBackVec.Normalize();

                    if (m_characterStatus.m_knockBackDamage <= _dmgData.m_damage)
                    {
                        if (_knockBackMultiplier > 0.0f)
                        {
                            m_animator.SetTrigger("KnockBack");
                            m_knockBackMultiplier = _knockBackMultiplier;

                            // 子オブジェクトまでたどるのはさすがに重いのでシーン制限 上甲 経営シーン払い戻し処理用
                            if (SceneNameManager.instance.CurrentSceneName == "ManagementScene")
                            {
                                var arbors = gameObject.GetComponentsInChildren<ArborFSM>();
                                foreach (var arv in arbors)
                                {
                                    arv.SendTrigger("Damage");
                                }
                            }
                        }
                    }

                    // 敵であれば索敵指示をだす
                    if (gameObject.TryGetComponent(out ArborFSM arborFSM))
                    {
                        arborFSM.SendTrigger("Damage");

                        if (EnemyParameters)
                        {
                            EnemyParameters.AttackedVec = -m_knockBackVec;
                        }
                    }
                }
            }

            // 返信用データ用意
            _dmgData.m_replyIsHit = true;
        }
    }


    async private void HitStop(DamageNotification _dmgData)
    {
        if (m_animator == null) { return; }

        m_animator.speed = 0.0f;

        // ヒットストップを、指定の時間実行
        await UniTask.Delay((int)(_dmgData.m_hitStopTime * 1000));  //ミリ秒単位のため、1000倍

        m_animator.speed = 1.0f;
    }

    // カラー乗算によるダメージ演出再生
    void HitFadeBlink(Color color)
    {
        m_seq.Kill();
        m_seq = DOTween.Sequence();
        m_seq.Append(DOTween.To(() => Color.white, c => m_material.SetColor(PROPERTY_COLOR, c), color, 0.1f));
        m_seq.Append(DOTween.To(() => color, c => m_material.SetColor(PROPERTY_COLOR, c), Color.white, 0.1f));
        m_seq.Play();

    }




    public void Move(float _targetSpeed)
    {
        switch (m_moveType)
        {
            // 緩やか
            case MoveTypes.Loose:
                {
                    m_charaCtrl.MoveSpeed += (_targetSpeed - m_charaCtrl.MoveSpeed) * 0.5f;
                    m_animator.SetFloat("Speed", m_charaCtrl.MoveSpeed / Status.DushSpeed, 0.15f, Time.fixedDeltaTime);
                    break;
                }

            // びたびた
            case MoveTypes.Sharp:
                {
                    m_charaCtrl.MoveSpeed = _targetSpeed;
                    m_animator.SetFloat("Speed", m_charaCtrl.MoveSpeed / Status.DushSpeed, 0.15f, Time.fixedDeltaTime);
                    break;
                }
        }
    }

    // セットしたフレームのみこの値は反映される
    public void SetMoveVec(Vector3 _moveVec)
    {
        m_charaCtrl.MoveVec = _moveVec;
    }

    public void SetRotateToTarget(Vector3 nextVec)
    {
        m_charaCtrl.LookVector = nextVec;
    }

    //童話スキルのBPをセットする関数
    public void SetStorySkillBP()
    {
        //格スキルIDから使用BPを取得する
        if (PlayerParameters)
        {
            PlayerStatus status = PlayerParameters.PlayerStatus;

            var bp1 = StorySkillDataBaseManager.instance.GetStorySkillData(PlayerParameters.StorySkill1_ID);
            status.m_bpSkill_1.Value = bp1.PayBP;


            var bp2 = StorySkillDataBaseManager.instance.GetStorySkillData(PlayerParameters.StorySkill2_ID);
            status.m_bpSkill_2.Value = bp2.PayBP;

            //最大値を代入する
            status.MaxBPSkill_1 = status.m_bpSkill_1.Value;
            status.MaxBPSkill_2 = status.m_bpSkill_2.Value;
        }
    }


    [System.Serializable]
    public class ActionState_Base : AnimatorStateMachine.ActionStateBase
    {
        [SerializeField] private bool m_isRootMotion = false;

        protected CharacterCore Core { get; private set; }
        public override void Initialize(AnimatorStateMachine stateMachine)
        {
            base.Initialize(stateMachine);

            Core = stateMachine.transform.parent.GetComponent<CharacterCore>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            //死亡判定チェック（山本）
            //if (Core.m_characterStatus.m_hp.Value <= 0.0f)
            //{
            //    //死亡トリガーを逃れてしまったら強制的に死亡状態へと移行する（山本）
            //    if (!Core.m_animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            //    {
            //        Core.m_characterStatus.m_hp.Value = 0.0f;
            //        Core.m_animator.SetTrigger("IsDead");
            //    }
            //}

        }

        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_charaCtrl.MoveSpeed = 0.0f;
            Core.m_charaCtrl.IsRootMotion = m_isRootMotion;
        }
    }
}

