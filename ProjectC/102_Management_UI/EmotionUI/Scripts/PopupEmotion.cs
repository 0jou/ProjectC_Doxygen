using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;

using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class PopupEmotion : MonoBehaviour
{
    [SerializeField] float m_destoryTime = 3;

    //テスト用
    //public async void Start()
    //{
    //    await UniTask.Delay(1000);
    //    Popup("Emotion_Anger");
    //}

    public async void Popup(string objectKey)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>("Emotion_Balloon");
        await handle;

        var obj = Instantiate(handle.Result, this.gameObject.transform);

        obj.AddComponent<EmotionBase>().Emotion = objectKey;
        obj.GetComponent<EmotionBase>().Parent = obj;

        obj.OnDestroyAsObservable().Subscribe(_ =>
        {
            Addressables.Release(handle);
        });

        Destroy(obj, m_destoryTime);
    }
}


import os
import shutil
import subprocess

def copy_cs_files(src_folder, dest_folder):
    # 指定フォルダが存在しない場合は作成
    if not os.path.exists(dest_folder):
        os.makedirs(dest_folder)

    # .csファイルをコピー
    for root, dirs, files in os.walk(src_folder):
        for file in files:
            if file.endswith(".cs"):
                full_file_path = os.path.join(root, file)
                shutil.copy(full_file_path, dest_folder)
                print(f"Copied: {full_file_path} to {dest_folder}")

def run_doxygen(doxygen_path, doxyfile_path):
    try:
        subprocess.run([doxygen_path, doxyfile_path], check = True)
        print("Doxygen executed successfully.")
    except subprocess.CalledProcessError as e:
        print(f"Error while running Doxygen: {e}")

# メイン処理
src_folder = "C:/3D_prac/_ProjectC/Assets/ProjectC"  # ここにコピー元のフォルダパス
dest_folder = "C:/3D_prac/ProjectC_Doxygen/ProjectC"  # ここにコピー先のフォルダパス
doxygen_path = "C:/Program Files/doxygen/bin/doxygen.exe"  # Doxygenの実行ファイルのパス
doxyfile_path = "C:/3D_prac/ProjectC_Doxygen/Doxyfile"  # Doxyfileのパス

# ステップ1: .csファイルをコピー
copy_cs_files(src_folder, dest_folder)

# ステップ2: Doxygenを実行
run_doxygen(doxygen_path, doxyfile_path)