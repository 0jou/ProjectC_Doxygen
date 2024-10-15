using CriWare;
using UnityEngine;
using UnityEngine.UI;
using CI.QuickSave;

/// <summary>
/// @details各カテゴリ(Master,Music,SFX.UI)の音量を外部ファイルに保存しつつスライダーで調整するクラス
/// スライダー側での値の変更の適用はAudioPlayVolumeTest.csで行われている。
/// 各スライダーは設定しなくても動作する(セーブロードのみが可能。)
/// </summary>
public class VolumeSaveLoader : MonoBehaviour
{
    // 各カテゴリの音量を調整するスライダー群
    [Header("各スライダーは設定しなくとも動作はする")]

    [SerializeField] Slider m_masterVolSlider = null;
    [SerializeField] Slider m_musicVolSlider = null;
    [SerializeField] Slider m_sfxVolSlider = null;
    [SerializeField] Slider m_uiVolSlider = null;

    private VolumeSaveLoad m_volumeSaveLoad = new VolumeSaveLoad();

    /// <summary>
    /// @brief 各カテゴリの音量を外部ファイルから読み込む。
    /// @details セーブデータが存在しない場合はエラーを出力する。
    /// 保存先:Assets/ProjectC/998_DebugSaveData/QuickSave/Volume.json
    /// ビルド時:Application.persistentDataPath/Volume.json
    /// </summary>
    public void LoadVolume()
    {
        m_volumeSaveLoad.LoadVolume();
        SetCategoryVolume("Master", m_volumeSaveLoad.MasterVolume, m_masterVolSlider);
        SetCategoryVolume("Music", m_volumeSaveLoad.MusicVolume, m_musicVolSlider);
        SetCategoryVolume("SFX", m_volumeSaveLoad.SFXVolume, m_sfxVolSlider);
        SetCategoryVolume("UI", m_volumeSaveLoad.UIVolume, m_uiVolSlider);

        Debug.Log("Volume Loaded");
    }

    /// <summary>
    /// @brief 各カテゴリの音量を外部ファイルに保存する。
    /// @details セーブデータが存在しない場合は新規作成する。
    /// 保存先:Assets/ProjectC/998_DebugSaveData/QuickSave/Volume.json
    /// ビルド時:Application.persistentDataPath/Volume.json
    /// </summary>
    public void SaveVolume()
    {
        m_volumeSaveLoad.SaveVolumeCriCategory();

        Debug.Log("Volume Saved");
    }

    // 各音量をスライダーに反映する
    void Start()
    {
        LoadVolume();
    }


    /// <summary>
    /// @brief カテゴリの音量を設定する。
    /// @details スライダーが設定されている場合はスライダーにも適用する。
    /// </summary>
    /// <param name="a_categoryName"></param>
    /// <param name="a_vol"></param>
    /// <param name="a_slider"></param>
    private void SetCategoryVolume(string a_categoryName, float a_vol, Slider a_slider)
    {
        if (a_slider != null)
        {
            a_slider.value = a_vol;
        }
        CriAtom.SetCategoryVolume(a_categoryName, a_vol);
    }


    public struct VolumeSaveLoad
    {
        public float MasterVolume;
        public float MusicVolume;
        public float SFXVolume;
        public float UIVolume;
        /// <summary>
        /// @brief 各カテゴリの音量を外部ファイルから読み込む。
        /// @details セーブデータが存在しない場合はエラーを出力する。
        /// 保存先:Assets/ProjectC/998_DebugSaveData/QuickSave/Volume.json
        /// ビルド時:Application.persistentDataPath/Volume.json
        /// </summary>
        public void LoadVolume()
        {
            if (!QuickSaveReader.RootExists("Volume"))
            {
                Debug.LogError("セーブデータがありません:Volume"); return;
            }
            QuickSaveReader reader = QuickSaveReader.Create("Volume");

            MasterVolume = reader.Read<float>("Master");
            MusicVolume = reader.Read<float>("MusicVol");
            SFXVolume = reader.Read<float>("SFXVol");
            UIVolume = reader.Read<float>("UIVol");
        }

        public void LoadVolumeCriCategory()
        {
            MasterVolume = CriAtom.GetCategoryVolume("Master");
            MusicVolume = CriAtom.GetCategoryVolume("Music");
            SFXVolume = CriAtom.GetCategoryVolume("SFX");
            UIVolume = CriAtom.GetCategoryVolume("UI");
        }

        /// <summary>
        /// @brief 各カテゴリの音量を外部ファイルに保存する。
        /// @details セーブデータが存在しない場合は新規作成する。
        /// 保存先:Assets/ProjectC/998_DebugSaveData/QuickSave/Volume.json
        /// ビルド時:Application.persistentDataPath/Volume.json
        /// </summary>
        public void SaveVolume()
        {
            var writer = QuickSaveWriter.Create("Volume");

            writer.Write<float>("Master", MasterVolume);
            writer.Write<float>("MusicVol", MusicVolume);
            writer.Write<float>("SFXVol", SFXVolume);
            writer.Write<float>("UIVol", UIVolume);

            writer.Commit();
        }

        public void SetCriCategoryVolume()
        {
            CriAtom.SetCategoryVolume("Master",MasterVolume);
            CriAtom.SetCategoryVolume("Music", MusicVolume);
            CriAtom.SetCategoryVolume("SFX", SFXVolume);
            CriAtom.SetCategoryVolume("UI", UIVolume);
        }

        public void SaveVolumeCriCategory()
        {
            var writer = QuickSaveWriter.Create("Volume");

            writer.Write<float>("Master", CriAtom.GetCategoryVolume("Master"));
            writer.Write<float>("MusicVol", CriAtom.GetCategoryVolume("Music"));
            writer.Write<float>("SFXVol", CriAtom.GetCategoryVolume("SFX"));
            writer.Write<float>("UIVol", CriAtom.GetCategoryVolume("UI"));
            writer.Commit();
        }
    }
}