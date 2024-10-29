using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManagementEventUI : MonoBehaviour
{
    // 制作者 田内
    // 経営イベントUIの作成

    [Header("作成するUI")]
    [SerializeField]
    private ManagementEventUI m_managementEventUI = null;

    private List<ManagementEventUI> m_createManagementEventUIList = new();

    //==================================================
    //                  実行処理
    //==================================================

    public void OnUpdate()
    {
        Create();

        CheckNull();
    }


    private void Create()
    {
        foreach (var data in ManagementEventManager.instance.EventList)
        {
            bool existence = false;

            // 作成済みのイベントに存在するか
            foreach (var createEvent in m_createManagementEventUIList)
            {
                // 存在すれば
                if (createEvent.TargetManagementEvent == data)
                {
                    existence = true;
                    break;
                }
            }

            // 存在しなければ作成する
            if (existence == false)
            {
                var ui = Instantiate(m_managementEventUI, transform);
                ui.SetUI(data);
                m_createManagementEventUIList.Add(ui);
            }

        }
    }


    private void CheckNull()
    {
        m_createManagementEventUIList.RemoveAll(_ => _ == null);
    }


}
