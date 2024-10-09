using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class GameLauncher : NetworkBehaviour, INetworkRunnerCallbacks
{
    public static GameLauncher _instance;

    [SerializeField]
    private NetworkRunner networkRunnerPrefab;

    [SerializeField]
    private NetworkPrefabRef playerAvatarPrefab;

    private NetworkRunner networkRunner;

    [SerializeField, Tooltip("プレイヤーのスポーン位置")]
    private Vector3 _playerSpawnPos = default;

    private PlayerInput _playerInput;

    private PlayerNetworkInput playerNetworkInput;

    private Subject<GameObject> _onPlayerJoin = new Subject<GameObject>();

    public IObservable<GameObject> OnPlayerJoin => _onPlayerJoin;

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

        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.AddCallbacks(this);
        _playerInput = GetComponent<PlayerInput>();

        var result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        RegisterInputActions(true);
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

    /// <summary>
    /// 入力アクション管理
    /// </summary>
    private void HandleInput(InputAction.CallbackContext context)
    {
        // 定義されているアクションを取得
        InputActionTypeEnum? actionType = InputActionManager.GetActionType(context.action.name);

        // 定義にないアクションはリターン
        if (actionType == null)
        {
            return;
        }

        switch (actionType.Value)
        {
            case InputActionTypeEnum.Move:
                playerNetworkInput.MoveDirection = context.ReadValue<Vector2>();
                break;

            case InputActionTypeEnum.Dash:
                playerNetworkInput.IsRunning = context.ReadValueAsButton();
                break;

            case InputActionTypeEnum.AttackLight:
                if (context.canceled) return;
                playerNetworkInput.IsAttackLight = true; // 発動時にtrue
                break;

            case InputActionTypeEnum.AttackStrong:
                if (context.canceled) return;
                playerNetworkInput.IsAttackStrong = true; // 発動時にtrue
                break;

            case InputActionTypeEnum.Avoidance:
                if (context.canceled) return;
                playerNetworkInput.IsAvoidance = true; // 発動時にtrue
                break;

            case InputActionTypeEnum.Targetting:
                if (context.canceled) return;
                playerNetworkInput.IsTargetting = true; // 発動時にtrue
                break;

            case InputActionTypeEnum.Skill:
                if (context.canceled) return;
                playerNetworkInput.IsSkill = true; // 発動時にtrue
                break;

            case InputActionTypeEnum.Resurrection:
                if (context.canceled) return;
                playerNetworkInput.IsResurrection = true; // 発動時にtrue
                break;
        }
    }

    // 入力処理
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // 現在の入力データをNetworkInputにセット
        input.Set(playerNetworkInput);

        // 入力情報をリセット（必要に応じて）
        ResetInput();
    }

    private void ResetInput()
    {
        // 各フラグをリセット
        playerNetworkInput.IsAttackLight = false;
        playerNetworkInput.IsAttackStrong = false;
        playerNetworkInput.IsAvoidance = false;
        playerNetworkInput.IsTargetting = false;
        playerNetworkInput.IsSkill = false;
        playerNetworkInput.IsResurrection = false;
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

        _onPlayerJoin.OnNext(avatar.gameObject);
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
