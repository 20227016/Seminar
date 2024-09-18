
using UnityEngine;
using System.Collections;
using Cinemachine;

/// <summary>
/// PlayerTargetting.cs
/// クラス説明
/// 
///
/// 作成日: 9/18
/// 作成者: 山田智哉
/// </summary>
public class PlayerTargetting : MonoBehaviour, ITargetting
{
    [SerializeField]
    private CinemachineVirtualCamera _normalCamera = default;

    [SerializeField]
    private CinemachineVirtualCamera _targettingCamera = default;

    private bool _isTargetting = default;

    private void Awake()
    {
        _targettingCamera.gameObject.SetActive(false);
        _isTargetting = false;
    }

    public void Targetting()
    {
        if (_isTargetting)
        {
           
        }

        // カメラの切り替え
        _isTargetting = !_isTargetting;
        _normalCamera.gameObject.SetActive(!_isTargetting);
        _targettingCamera.gameObject.SetActive(_isTargetting);
    }
}