using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;


public class GetItemObject : MonoBehaviour
{

    // 制作者(田内)
    // フラグが変わった時のUI表示（吉田）

    //=================================================
    // 現在採取可能かどうか


    private BoolReactiveProperty m_isGet = new();


    public bool IsGet { get { return m_isGet.Value; } }


    //========================================================
    // フラグが変わった時のUI表示

    [Header("DOスピード")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_doSpead = 0.2f;

    [SerializeField]
    private AppearanceItemName m_appearName = null;

    [SerializeField]
    private AppearanceItemOutline m_appearOutline = null;

    //========================================================
    //========================================================
    //========================================================

    private void Start()
    {
        /*a.OnTriggerEnterAsObservable()
            .Where(_ => enabled)
            .Subscribe(collider =>
            {
                GameObject otherObj = collider.gameObject;
            }

        a.OnTriggerExitAsObservable()
      .Where(_ => enabled)
      .Subscribe(collider =>
      {
          GameObject otherObj = collider.gameObject;
      }*/

                m_isGet.Value = false;// 初期化

        if (m_appearName == null)
        {
            Debug.LogError("AppearanceItemNameコンポーネントが登録されていません");
        }
        else
        {
            // 名前表示
            SwitchItemName();
        }

        if (m_appearOutline == null)
        {
            Debug.LogError("AppearanceItemOutlineコンポーネントが登録されていません");
        }
        else
        {
            // アウトライン表示
            SwitchOutline();
        }

        

    }


    private void OnTriggerEnter(Collider other)
    {
        // 所持できれば
        if (!InventoryManager.instance.IsInList()) return;

        // 範囲内にいるのがPlayerであれば採取可能に
        if (other.gameObject.CompareTag("Player"))
        {
            m_isGet.Value = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // 範囲内から出たのがPlayerであれば採取不可能に
        if (other.gameObject.CompareTag("Player"))
        {
            m_isGet.Value = false;
        }
    }


    private void Update()
    {
        // 取得する処理 PlayerStateに移動したのでコメント(吉田)
        // Get();
    }


    public void GetItem()
    {
        var component = gameObject.GetComponentInParent<AssignItemID>();
        if (component == null)
        {
            Debug.LogError("AssignIngredientIDが存在しないのでIDを取得できません");
            return;
        }


        // インベントリに加える
        if (InventoryManager.instance.AddItem(component.ItemTypeID, component.ItemID))
        {
            // オブジェクトを削除する
            Destroy(gameObject);
        }
    }


    // 名前表示・非表示切り替え（吉田）
    private void SwitchItemName()
    {
        m_isGet.Subscribe(x =>
        {
            if (x)
            {
                m_appearName.OnShow(m_doSpead);
            }
            else
            {
                m_appearName.OnHide(m_doSpead);
            }
        });
    }

    // アウトライン表示（吉田）
    private void SwitchOutline()
    {
        m_isGet.Subscribe(x =>
        {
            if (x)
            {
                m_appearOutline.OnShow(m_doSpead);
            }
            else
            {
                m_appearOutline.OnHide(m_doSpead);
            }
        });
    }


    // 取得する
    private void Get()
    {

        DebugGet();

        if (!m_isGet.Value) return;


        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {

            if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "Gathering"))
            {

                var component = gameObject.GetComponentInParent<AssignItemID>();
                if (component == null)
                {
                    Debug.LogError("AssignIngredientIDが存在しないのでIDを取得できません");
                    return;
                }

                // インベントリに加える
                if (InventoryManager.instance.AddItem(component.ItemTypeID, component.ItemID))
                {

                    // オブジェクトを削除する
                    Destroy(gameObject);

                }

            }
        }
    }


    private void DebugGet()
    {

#if DEBUG

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {

            if (Keyboard.current.pKey.wasPressedThisFrame)
            {

                var component = gameObject.GetComponentInParent<AssignItemID>();
                if (component == null)
                {
                    Debug.LogError("AssignIngredientIDが存在しないのでIDを取得できません");
                    return;
                }

                // 手持ちに加える
                if (InventoryManager.instance.AddItem(component.ItemTypeID, component.ItemID))
                {
                    // オブジェクトを削除
                    Destroy(gameObject);

                }

            }
        }


#endif

    }

}
