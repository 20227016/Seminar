using UnityEngine;
using Cinemachine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections.Generic;

/// <summary>
/// PlayerTargetting.cs
/// クラス説明
/// プレイヤーのターゲッティングクラス
/// 
/// 作成日: 9/18
/// 作成者: 山田智哉
/// </summary>
public class PlayerTargetting : MonoBehaviour, ITargetting
{
    private const float FullCircleDegrees = 360f;
    private const float HalfCircleDegrees = 180f;
    private const float TargettingDistance = 100f;
    private const float TargettingFactor = 0.3f;
    private const float TargettingThreshold = 0.5f;
    private const float DistanceNormalizationFactor = 500f;

    [SerializeField, Tooltip("通常時のカメラ")]
    private CinemachineVirtualCamera _normalCamera = default;

    [SerializeField, Tooltip("ターゲッティング時のカメラ")]
    private CinemachineVirtualCamera _targettingCamera = default;

    [SerializeField, Tooltip("ターゲットのレイヤー")]
    private LayerMask _targetLayer = default;

    [SerializeField, Tooltip("無視するレイヤー")]
    private LayerMask _ignoreLayer = default;

    // 通常時カメラのPOVコンポーネント取得用
    private CinemachinePOV _normalCameraPOV = default;

    // メインカメラ
    private Camera _mainCamera = default;

    // ターゲッティングフラグ
    private bool _isTargetting = default;

    // 現在のターゲットオブジェクト
    private GameObject _currentTarget = default;

    // ロックオンイベント
    private Subject<Transform> _lockonEvent = new Subject<Transform>();

    public IObservable<Transform> LockOnEvent => _lockonEvent;

    /// <summary>
    /// 起動時処理
    /// </summary>
    private void Awake()
    {
        // キャッシュ
        _mainCamera = Camera.main;
        _normalCameraPOV = _normalCamera.GetCinemachineComponent<CinemachinePOV>();
        _targettingCamera.gameObject.SetActive(false);
        _isTargetting = false;
        // 更新処理
        this.UpdateAsObservable()
            .Where(_ => _isTargetting && !IsTargetVisible(_currentTarget))
            .Subscribe(_ => Targetting());

    }

    public void Targetting()
    {
        // ターゲッティングしてないとき
        if (!_isTargetting)
        {
            // 現在のターゲットにターゲット検索結果を格納
            _currentTarget = SearchTarget();

            if (_currentTarget == null) return;

            // ターゲットとの射線が通っているとき
            if (IsTargetVisible(_currentTarget))
            {
                _targettingCamera.LookAt = _currentTarget.transform;
            }
        }
        else
        {
            // ターゲッティングカメラの回転角をノーマルカメラのPOVに反映
            _normalCameraPOV.m_VerticalAxis.Value = Mathf.Repeat(_targettingCamera.transform.eulerAngles.x + HalfCircleDegrees, FullCircleDegrees) - HalfCircleDegrees;
            _normalCameraPOV.m_HorizontalAxis.Value = _targettingCamera.transform.eulerAngles.y;
        }

        // カメラを切り替える
        _isTargetting = !_isTargetting;
        _normalCamera.gameObject.SetActive(!_isTargetting);
        _targettingCamera.gameObject.SetActive(_isTargetting);
        _lockonEvent.OnNext(_currentTarget.transform);
    }

    /// <summary>
    /// ターゲット視認フラグ
    /// </summary>
    /// <param name="target">ターゲットオブジェクト</param>
    /// <returns>ターゲットが視認できているか</returns>
    private bool IsTargetVisible(GameObject target)
    {
        // ターゲットと自身の距離を計算
        Vector3 direction = target.transform.position - _mainCamera.transform.position;

        // 射線が通っているときはtrue
        if (Physics.Raycast(_mainCamera.transform.position, direction, out RaycastHit hit, TargettingDistance, _ignoreLayer))
        {
            return hit.collider.gameObject.layer != _targetLayer;
        }

        return false;
    }

    /// <summary>
    /// ターゲット検索
    /// </summary>
    /// <returns>ターゲット</returns>
    private GameObject SearchTarget()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, TargettingDistance, Vector3.up, 0, _targetLayer);
        if (hits.Length == 0)
        {
            return null;
        }

        List<GameObject> hitObjects = new();
        RaycastHit hit;

        for (int i = 0; i < hits.Length; i++)
        {

            Vector3 targetDirection = hits[i].collider.gameObject.transform.position - transform.position;
            if (Physics.Raycast(transform.position, targetDirection, out hit, TargettingDistance))
            {

                if (hit.collider.gameObject == hits[i].collider.gameObject)
                {

                    hitObjects.Add(hit.collider.gameObject);

                }
            }
        }

        if (hitObjects.Count == 0)
        {
            return null;
        }

        float degreep = Mathf.Atan2(_mainCamera.transform.forward.x, _mainCamera.transform.forward.z);
        float degreemum = Mathf.PI * 2;
        GameObject target = null;

        foreach (GameObject enemy in hitObjects)
        {
            Vector3 pos = _mainCamera.transform.position - enemy.transform.position;
            Vector3 pos2 = enemy.transform.position - _mainCamera.transform.position;
            pos2.y = 0.0f;
            pos2.Normalize();

            float degree = Mathf.Atan2(pos2.x, pos2.z);

            if (Mathf.PI <= (degreep - degree))
            {
                degree = degreep - degree - Mathf.PI * 2;
            }
            else if (-Mathf.PI >= (degreep - degree))
            {
                degree = degreep - degree + Mathf.PI * 2;
            }
            else
            {
                degree = degreep - degree;
            }

            // DistanceNormalizationFactorを使用
            degree = degree + degree * (pos.magnitude / DistanceNormalizationFactor) * TargettingFactor;

            if (Mathf.Abs(degreemum) >= Mathf.Abs(degree))
            {
                degreemum = degree;
                target = enemy;
            }
        }

        // 求めた一番小さい値が一定値より小さい場合、ターゲッティングをオンにします
        if (Mathf.Abs(degreemum) <= TargettingThreshold)
        {
            return target;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        // Gizmosの色を設定
        Gizmos.color = Color.red;

        // SphereCastの可視化
        // transform.positionからTargettingDistanceの範囲にSphereを表示
        Gizmos.DrawWireSphere(transform.position, TargettingDistance);

        // ターゲット候補が存在する場合、その位置を線で結ぶ
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, TargettingDistance, Vector3.up, 0, _targetLayer);
        foreach (RaycastHit hit in hits)
        {
            // ターゲットまでの線を引く
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, hit.collider.transform.position);

            // ターゲットの位置に小さな球を表示
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hit.collider.transform.position, 0.5f); // ターゲットを示す小さな球
        }
    }
}