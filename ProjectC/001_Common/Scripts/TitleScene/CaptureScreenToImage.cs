using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class CaptureScreenToImage : MonoBehaviour
{
    [SerializeField] private Image m_image = null;


    [ContextMenu("Capture")]
    public void Button()
    {
        Capture().Forget();
    }

    public async UniTask Capture()
    {
        // ReadPixelsがこの後でないと使えないので必ず書く
        var runner = new GameObject("runner").AddComponent<CoroutineRunner>();
        await UniTask.WaitForEndOfFrame(runner);
        // スクリーンの大きさのSpriteを作る
        var texture = new Texture2D(Screen.width, Screen.height);

        // スクリーンを取得する
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        // 適応する
        texture.Apply();

        // 取得した画像をSpriteに入るように変換する
        byte[] pngdata = texture.EncodeToPNG();
        texture.LoadImage(pngdata);
        texture.name = "CapturedImage";

        // 先ほど作ったSpriteに画像をいれる
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        Debug.Log("Captured");

        // Spriteを使用するオブジェクトに指定する
        // 今回はUIのImage
        SceneCaptureManager.m_capturedImage = sprite;

        // サイズ変更
        SceneCaptureManager.m_sizeDelta = new Vector2(texture.width, texture.height);

        if (m_image != null)
        {
            m_image.sprite = sprite;
        }

        Destroy(runner.gameObject);
    }
}
