using UnityEngine;

// 画面外の敵の位置をUI表示（伊波）
// https://qiita.com/o8que/items/46e486f62bdf05c29559

[RequireComponent(typeof(RectTransform))]
public class TargetIndicator : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image m_arrow = default;

    private Transform m_target = default;
    public Transform Target { get { return m_target; } set { m_target = value; ShowArrow(true); } }

    [Tooltip("一秒分の加算値")]
    [SerializeField, Range(0.0f, 5.0f)]
    private float m_addAlpha = 0.5f;

    private Vector2 m_drawSize = new(200f, 100f);
    private Vector2 m_drawCenterPos = new(0f, -300f);

    public void SetDrawData(Vector2 _drawSize, Vector2 _drawCenterPos)
    {
        m_drawSize = _drawSize;
        m_drawCenterPos = _drawCenterPos;
    }

    private Camera mainCamera;
    private RectTransform rectTransform;

    private void ShowArrow(bool _isShow)
    {
        if (_isShow) m_addAlpha = Mathf.Abs(m_addAlpha);
        else m_addAlpha = -Mathf.Abs(m_addAlpha);
    }

    private void Start()
    {
        if (!m_arrow)
        {
            Debug.LogError("Arrowが指定されていません", gameObject);
        }

        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        m_arrow.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    private void LateUpdate()
    {
        Color color = m_arrow.color;
        color.a += m_addAlpha * Time.deltaTime;
        color.a = Mathf.Clamp(color.a, 0.0f, 1.0f);
        m_arrow.color = color;

        // ターゲットがいなければ画面外に
        if (!m_target)
        {
            //rectTransform.anchoredPosition = new Vector2(Screen.width, Screen.height) + rectTransform.sizeDelta;
            ShowArrow(false);
            return;
        }

        float canvasScale = transform.root.localScale.z;
        var center = 0.5f * new Vector3(Screen.width, Screen.height);

        // ターゲット座標を２Ｄ変換
        var pos = mainCamera.WorldToScreenPoint(m_target.position) - center;
        if (pos.z < 0f)
        {
            pos.x = -pos.x;
            pos.y = -pos.y;

            if (Mathf.Approximately(pos.y, 0f))
            {
                pos.y = -center.y;
            }
        }

        // カメラから一定距離かつ、カーソルが楕円の中に入ってたら非表示
        if (Mathf.Abs(pos.x - m_drawCenterPos.x) < m_drawSize.x && Mathf.Abs(pos.y - m_drawCenterPos.y) < m_drawSize.y)
        {
            ShowArrow(false);
        }
        else
        {
            ShowArrow(true);
        }

        // UIが見切れないよう座標補正
        var halfSize = 0.5f * canvasScale * rectTransform.sizeDelta;
        float d = Mathf.Max(
            Mathf.Abs(pos.x / (center.x - halfSize.x)),
            Mathf.Abs(pos.y / (center.y - halfSize.y))
        );
        //bool isOffscreen = (pos.z < 0f || d > 1f);
        //if (isOffscreen)
        {
            pos.x /= d;
            pos.y /= d;
            pos -= new Vector3(m_drawCenterPos.x, m_drawCenterPos.y);

            // 座標を楕円形に補正
            var deg = Vector2.SignedAngle(Vector2.up, pos);
            var rad = -deg * Mathf.Deg2Rad;
            pos.x = Mathf.Sin(rad) * m_drawSize.x;
            pos.y = Mathf.Cos(rad) * m_drawSize.y;
        }
        rectTransform.anchoredPosition = pos / canvasScale;
        rectTransform.anchoredPosition += m_drawCenterPos;


        // 矢印の角度調整
        //if (isOffscreen)
        {
            m_arrow.rectTransform.eulerAngles = new Vector3(
                0f, 0f,
                Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg
            );
        }
    }
}
