using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    public static GameLauncher _instance;

    [SerializeField]
    private NetworkRunner networkRunnerPrefab;

    [SerializeField]
    private NetworkPrefabRef playerAvatarPrefab;

    [SerializeField, Tooltip("プレイヤーのスポーン位置")]
    private Vector3 _playerSpawnPos = default;

    private PlayerInput _playerInput = default;

    private Vector2 _moveInput = default;

    private Camera _mainCamera = default;

    public static GameLauncher Instance
    {
        get
        {
            return _instance;
        }
    }

    private async void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        _mainCamera = Camera.main;
        _playerInput = GetComponent<PlayerInput>();
        RegisterInputActions(true);

        NetworkRunner networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.AddCallbacks(this);

        StartGameResult result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });
        
    }

    private void RegisterInputActions(bool isRegister)
    {
        // 一括登録
        if (isRegister)
        {
            foreach (InputAction action in _playerInput.actions)
            {
                action.performed += HandleInput;
                action.canceled += HandleInput;
            }
        }
        // 一括解除
        else
        {
            foreach (InputAction action in _playerInput.actions)
            {
                action.performed -= HandleInput;
                action.canceled -= HandleInput;
            }
        }
    }

    private readonly Dictionary<string, bool> InputStates = new()
    {
        { "Run", false },
        { "AttackLight", false },
        { "AttackStrong", false },
        { "Targetting", false },
        { "Skill", false },
        { "Resurrection", false },
        { "Avoidance", false }
    };

    /// <summary>
    /// 入力管理メソッド
    /// </summary>
    /// <param name="context">入力アクション</param>
    private void HandleInput(InputAction.CallbackContext context)
    {

        switch (context.action.name)
        {

            case "Move":
                _moveInput = context.ReadValue<Vector2>();
                break;

            case "Run":
                InputStates["Run"] = true;
                break;

            case "AttackLight":
                InputStates["AttackLight"] = !context.canceled;
                break;

            case "AttackStrong":
                InputStates["AttackStrong"] = !context.canceled;
                break;

            case "Targetting":
                InputStates["Targetting"] = !context.canceled;
                break;

            case "Skill":
                InputStates["Skill"] = !context.canceled;
                break;

            case "Resurrection":
                InputStates["Resurrection"] = !context.canceled;
                break;

            case "Avoidance":
                InputStates["Avoidance"] = !context.canceled;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// カメラの向きを基準にした移動方向算出メソッド
    /// </summary>
    /// <returns>カメラの向きを基準にした移動方向</returns>
    private Vector3 GetMoveDirectionFromCamera()
    {
        // カメラの正面方向と右方向を取得
        Vector3 cameraForward = _mainCamera.transform.forward;
        Vector3 cameraRight = _mainCamera.transform.right;

        // 高さ（Y軸）の影響を排除
        cameraForward.y = 0;
        cameraRight.y = 0;

        // 正規化
        cameraForward.Normalize();
        cameraRight.Normalize();

        // カメラ基準での移動方向を計算
        Vector3 direction = cameraForward * _moveInput.y + cameraRight * _moveInput.x;
        return new Vector2(direction.x, direction.z);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // 入力をインスタンス化
        PlayerNetworkInput data = new()
        {
            MoveDirection = GetMoveDirectionFromCamera(),
            IsRunning = InputStates["Run"],
            IsAttackLight = InputStates["AttackLight"],
            IsAttackStrong = InputStates["AttackStrong"],
            IsTargetting = InputStates["Targetting"],
            IsSkill = InputStates["Skill"],
            IsResurrection = InputStates["Resurrection"],
            IsAvoidance = InputStates["Avoidance"],
        };

        // 入力を収集
        input.Set(data);

        // 入力を初期化
        ResetInput();
    }

    /// <summary>
    /// 入力初期化メソッド
    /// </summary>
    private void ResetInput()
    {
        // trueの入力を初期化する
        foreach (string key in new List<string>(InputStates.Keys))
        {

            if (InputStates[key])
            {
                InputStates[key] = false; // 値をリセット
            }
        }
    }

    // プレイヤーが参加した時の処理
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer)
        {
            return;
        }

        var spawnPosition = new Vector3(_playerSpawnPos.x + UnityEngine.Random.Range(0,10), _playerSpawnPos.y, _playerSpawnPos.z);
        var avatar = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);
        runner.SetPlayerObject(player, avatar);
    }

    // プレイヤーが退出した時の処理
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer)
        {
            return;
        }

        if (runner.TryGetPlayerObject(player, out var avatar))
        {
            runner.Despawn(avatar);
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}
