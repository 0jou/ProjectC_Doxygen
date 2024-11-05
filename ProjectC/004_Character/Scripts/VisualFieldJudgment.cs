using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 視野角判定処理共通用クラス（伊波）

public class VisualFieldJudgment
{
    public bool SearchTarget(GameObject _target, ChaseData _chaseParameters, Collider _myCollider, ref float _nearestDist)
    {
        if (_myCollider.transform.root.name == _target.name) return false;

        // if (Vector3.Distance(_target.transform.position, m_startPos) >= m_chaseParameters.value.DistAwayFromSpawnPos) return false;

        float dist;

        dist = Vector3.Distance(_myCollider.transform.position, _target.transform.position);
        if (dist <= _chaseParameters.NoticeDist)
        {
            _nearestDist = dist;
            return true;
        }
        if (dist > _chaseParameters.SearchCharacterDist) return false;

        float angle;
        angle = Vector3.Angle(_myCollider.transform.forward, _target.transform.position - _myCollider.transform.position);
        if (angle > _chaseParameters.ViewAngle * 0.5f) return false;

        if (dist >= _nearestDist) return false;

        if (!_target.TryGetComponent(out Collider targetCollider))
        {
            return false;
        }
        Vector3 dir = targetCollider.bounds.center - _myCollider.bounds.center;
        dist = Vector3.Distance(targetCollider.bounds.center, _myCollider.bounds.center);
        var ray = new Ray(_myCollider.bounds.center, dir);
        Debug.DrawRay(ray.origin, ray.direction * dist, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, dist))
        {
            if (hit.collider.gameObject.name == targetCollider.gameObject.name)
            {
                _nearestDist = dist;
                return true;
            }
        }
        return false;
    }
    public bool ChaseTarget(GameObject _target, ChaseData _chaseParameters, Collider _myCollider)
    {
        if (Vector3.Distance(_myCollider.transform.position, _target.transform.position) <= _chaseParameters.NoticeDist)
        {
            return true;
        }

        float angle;
        angle = Vector3.Angle(_myCollider.transform.forward, _target.transform.position - _myCollider.transform.position);
        if (angle > _chaseParameters.ViewAngle * 0.5f) return false;

        if (!_target.TryGetComponent(out Collider targetCollider))
        {
            return false;
        }

        Vector3 dir = targetCollider.bounds.center - _myCollider.bounds.center;
        float dist = Vector3.Distance(targetCollider.bounds.center, _myCollider.bounds.center);
        Ray ray = new (_myCollider.bounds.center, dir);
        Debug.DrawRay(ray.origin, ray.direction * dist, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, dist))
        {
            if (hit.collider.gameObject.name == targetCollider.gameObject.name)
            {
                return true;
            }
        }

        return false;
    }

    public bool SearchTargetNearSpawn(GameObject _target, ChaseData _chaseParameters, Collider _myCollider, Vector3 _startPos, ref float _nearestDist, float _searchDist)
    {
        if (_myCollider.transform.root.name == _target.name) return false;

        if (Vector3.Distance(_target.transform.position, _startPos) >= _chaseParameters.DistAwayFromSpawnPos) return false;

        float dist;

        dist = Vector3.Distance(_myCollider.transform.position, _target.transform.position);
        if (dist <= _chaseParameters.NoticeDist)
        {
            _nearestDist = dist;
            return true;
        }
        if (dist > _searchDist) return false;

        float angle;
        angle = Vector3.Angle(_myCollider.transform.forward, _target.transform.position - _myCollider.transform.position);
        if (angle > _chaseParameters.ViewAngle * 0.5f) return false;

        if (dist >= _nearestDist) return false;

        if (!_target.TryGetComponent(out Collider targetCollider))
        {
            return false;
        }
        Vector3 dir = targetCollider.bounds.center - _myCollider.bounds.center;
        dist = Vector3.Distance(targetCollider.bounds.center, _myCollider.bounds.center);
        var ray = new Ray(_myCollider.bounds.center, dir);
        Debug.DrawRay(ray.origin, ray.direction * dist, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, dist))
        {
            if (hit.collider.gameObject.name == targetCollider.gameObject.name)
            {
                _nearestDist = dist;
                return true;
            }
        }

        return false;
    }
}
