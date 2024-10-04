using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;

public class GameLauncher : SimulationBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private NetworkRunner networkRunnerPrefab;

    [SerializeField]
    private NetworkPrefabRef playerAvatarPrefab;

    private NetworkRunner networkRunner;

    [SerializeField, Tooltip("プレイヤーのスポーン位置")]
    private Vector3 _playerSpawnPos = default;

    // PlayerActionMapのインスタンスを作成
    private PlayerInput _playerInput = default;

    private async void Start()
    {
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.AddCallbacks(this);
        _playerInput = GetComponent<PlayerInput>();
        var result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });
    }


    // プレイヤーが参加した時の処理
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer)
        {
            return;
        }

        var spawnPosition = new Vector3(_playerSpawnPos.x, _playerSpawnPos.y, _playerSpawnPos.z);
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

    // 入力処理
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        PlayerNetworkInput playerNetworkInput = new PlayerNetworkInput();

        playerNetworkInput.MoveDirection = _playerInput.actions["Move"].ReadValue<Vector2>();
        playerNetworkInput.IsRunning = _playerInput.actions["Dash"].IsPressed();
        playerNetworkInput.IsAttackLight = _playerInput.actions["AttackLight"].IsPressed();
        playerNetworkInput.IsAttackStrong = _playerInput.actions["AttackStrong"].IsPressed();
        playerNetworkInput.IsAvoidance = _playerInput.actions["Avoidance"].IsPressed();
        playerNetworkInput.IsTargetting = _playerInput.actions["Targetting"].IsPressed();
        playerNetworkInput.IsSkill = _playerInput.actions["Skill"].IsPressed();
        playerNetworkInput.IsResurrection = _playerInput.actions["Resurrection"].IsPressed();

        // 入力をNetworkInputにセット
        input.Set(playerNetworkInput);
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
