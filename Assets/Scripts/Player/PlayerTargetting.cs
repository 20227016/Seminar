
using UnityEngine;
using System.Collections;
using Cinemachine;

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

    [SerializeField, Tooltip("通常時のカメラ")]
    private CinemachineVirtualCamera _normalCamera = default;

    [SerializeField, Tooltip("ターゲッティング時のカメラ")]
    private CinemachineVirtualCamera _targettingCamera = default;

    // 通常時カメラのPOVコンポーネント取得用
    private CinemachinePOV _normalCameraPOV = default;

    // ターゲッティングフラグ
    private bool _isTargetting = default;


    private void Awake()
    {
        // キャッシュ
        _normalCameraPOV = _normalCamera.GetCinemachineComponent<CinemachinePOV>();

        // 初期化
        _targettingCamera.gameObject.SetActive(false);
        _isTargetting = false;
    }

    public void Targetting()
    {

        if (_isTargetting)
        {

            // ターゲッティングカメラの回転角をノーマルカメラのPOVに反映
            _normalCameraPOV.m_VerticalAxis.Value = Mathf.Repeat(_targettingCamera.transform.eulerAngles.x + HalfCircleDegrees, FullCircleDegrees) - HalfCircleDegrees;
            _normalCameraPOV.m_HorizontalAxis.Value = _targettingCamera.transform.eulerAngles.y;
        }

        // カメラの切り替え
        _isTargetting = !_isTargetting;
        _normalCamera.gameObject.SetActive(!_isTargetting);
        _targettingCamera.gameObject.SetActive(_isTargetting);
    }
}