using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//当たり判定用の球を破壊する関数
public class DestroySkillAtackCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerExit(Collider other)
    {
        // 攻撃コライダー同士なら消さない
        if (other.TryGetComponent(out AttackApplicant atk)) { return; }

        Destroy(this.gameObject);
    }

}
