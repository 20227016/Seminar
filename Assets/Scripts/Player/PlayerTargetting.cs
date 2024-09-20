
using UnityEngine;
using Cinemachine;
using UniRx;
using UniRx.Triggers;

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
    private const float BoxCastScaleMultiplier = 10f;
    private const float BoxCastDistance = 100f;
    private const int EnemyLayer = 7;

    [SerializeField, Tooltip("通常時のカメラ")]
    private CinemachineVirtualCamera _normalCamera = default;

    [SerializeField, Tooltip("ターゲッティング時のカメラ")]
    private CinemachineVirtualCamera _targettingCamera = default;

    [SerializeField, Tooltip("障害物のレイヤー")]
    private LayerMask _layerMask = default;

    // 通常時カメラのPOVコンポーネント取得用
    private CinemachinePOV _normalCameraPOV = default;

    // メインカメラ
    private Camera _camera = default;

    // ターゲッティングフラグ
    private bool _isTargetting = default;

    // 現在のターゲット
    private GameObject _currentTarget = default;

    /// <summary>
    /// ボックスキャスト設定
    /// </summary>
    /// <returns>BoxCastStruct</returns>
    private BoxCastStruct CreateBoxCastStruct()
    {
        return new BoxCastStruct
        {
            _originPos = transform.position,
            _size = transform.localScale * BoxCastScaleMultiplier,
            _direction = _camera.transform.forward,
            _quaternion = Quaternion.identity,
            _distance = BoxCastDistance
        };
    }

    /// <summary>
    /// 起動時処理
    /// </summary>
    private void Awake()
    {
        // キャッシュ
        _camera = Camera.main;
        _normalCameraPOV = _normalCamera.GetCinemachineComponent<CinemachinePOV>();
        _targettingCamera.gameObject.SetActive(false);
        _isTargetting = false;

        // 更新処理
        this.UpdateAsObservable()
            .Where(_ => _isTargetting && !IsEnemyVisible(_currentTarget))
            .Subscribe(_ => Targetting());
    }

    public void Targetting()
    {
        // ターゲッティングしてないとき
        if (!_isTargetting)
        {

            BoxCastStruct boxCastStruct = CreateBoxCastStruct();
            RaycastHit[] hits = Search.Sort(Search.BoxCastAll(boxCastStruct));
            bool hasTarget = false;

            foreach (RaycastHit hit in hits)
            {

                // エネミーを発見した時
                if (hit.collider.gameObject.layer == EnemyLayer &&
                    IsEnemyVisible(hit.collider.gameObject))
                {

                    // ターゲットに設定
                    _currentTarget = hit.collider.gameObject;
                    _targettingCamera.LookAt = hit.collider.transform;
                    hasTarget = true;
                    break;
                }
            }
            if (!hasTarget)
            {

                return;

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
    }

    /// <summary>
    /// 敵視認フラグ
    /// </summary>
    /// <param name="enemy">敵オブジェクト</param>
    /// <returns>敵が視認できているか</returns>
    private bool IsEnemyVisible(GameObject enemy)
    {
        Vector3 direction = enemy.transform.position - _camera.transform.position;

        if (Physics.Raycast(_camera.transform.position, direction, out RaycastHit hit, BoxCastDistance, _layerMask))
        {
            return hit.collider.gameObject.layer == EnemyLayer;
        }

        return false;
    }
}