using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public static class SceneCaptureManager
{
    public static Sprite m_capturedImage { get; set; } = null;

    public static Vector2 m_sizeDelta { get; set; } = new Vector2();
}