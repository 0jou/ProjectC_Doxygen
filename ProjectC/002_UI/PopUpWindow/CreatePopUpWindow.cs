using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CreatePopUpWindow : MonoBehaviour
{
    // ポップアップのウィンドウを作成する
    // 制作者　田内

    [Header("作成するポップアップウィンドウコントローラ")]
    [SerializeField]
    private GameObject m_popUpWindow = null;


    // 作成したウィンドウ保持
    private GameObject m_createWindow = null;

    private async UniTask Create()
    {
        if (m_popUpWindow == null)
        {
            Debug.LogError("ウィンドウがシリアライズされていません");
            return;
        }

        m_createWindow = Instantiate(m_popUpWindow);

        var controller = m_createWindow.GetComponent<WindowController>();
        if (controller == null)
        {
            Debug.LogError("WindowControllerコンポーネントがアタッチされていません");
        }

        await controller.CreateWindow<BaseWindow>();

        // 削除する
        if (m_createWindow != null)
        {
            Destroy(m_createWindow);
        }

    }


    void Start()
    {
        Create().Forget();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
