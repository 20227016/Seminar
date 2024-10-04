
using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;

/// <summary>
/// GameLauncher.cs
/// クラス説明
///
///
/// 作成日: 9/3
/// 作成者: 山田智哉
/// </summary>
public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private NetworkRunner networkRunnerPrefab;

    [SerializeField]
    private NetworkPrefabRef playerAvatarPrefab;

    private NetworkRunner networkRunner;

    [SerializeField, Tooltip("プレイヤーのスポーン位置")]
    private Vector3 _playerSpawnPos = default;

    private PlayerInput _playerInput = default;

    private async void Start()
    {
        networkRunner = Instantiate(networkRunnerPrefab);
        // NetworkRunnerのコールバック対象に、このスクリプト（GameLauncher）を登録する
        networkRunner.AddCallbacks(this);
        _playerInput = GetComponent<PlayerInput>();
        var result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });
    }




    // セッションへプレイヤーが参加した時に呼ばれるコールバック
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // ホスト（サーバー兼クライアント）かどうかはIsServerで判定できる
        if (!runner.IsServer) 
        { 
            return; 
        }

        // ランダムな生成位置（半径5の円の内部）を取得する
        var posValue = _playerSpawnPos;

        var spawnPosition = new Vector3(posValue.x, posValue.y, posValue.z);

        // 参加したプレイヤーのアバターを生成する
        var avatar = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);
        
        // プレイヤー（PlayerRef）とアバター（NetworkObject）を関連付ける
        runner.SetPlayerObject(player, avatar);
    }

    // セッションからプレイヤーが退出した時に呼ばれるコールバック
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer)
        { 
            return;
        }

        // 退出したプレイヤーのアバターを破棄する
        if (runner.TryGetPlayerObject(player, out var avatar))
        {
            runner.Despawn(avatar);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        PlayerNetworkInput networkInput = new PlayerNetworkInput();

        // Move
        InputAction moveAction = _playerInput.actions["Move"];
        if (moveAction != null)
        {
            networkInput.MoveDirection = moveAction.ReadValue<Vector2>();
        }

        // Dash (Run)
        InputAction dashAction = _playerInput.actions["Dash"];
        if (dashAction != null)
        {
            networkInput.IsRunning = dashAction.ReadValue<float>() > 0;
        }

        // Attack Light
        InputAction attackLightAction = _playerInput.actions["AttackLight"];
        if (attackLightAction != null)
        {
            networkInput.IsAttackLight = attackLightAction.triggered;
        }

        // Attack Strong
        InputAction attackStrongAction = _playerInput.actions["AttackStrong"];
        if (attackStrongAction != null)
        {
            networkInput.IsAttackStrong = attackStrongAction.triggered;
        }

        // Avoidance
        InputAction avoidanceAction = _playerInput.actions["Avoidance"];
        if (avoidanceAction != null)
        {
            networkInput.IsAvoidance = avoidanceAction.triggered;
        }

        // Targetting
        InputAction targetAction = _playerInput.actions["Target"];
        if (targetAction != null)
        {
            networkInput.IsTargetting = targetAction.triggered;
        }

        // Skill
        InputAction skillAction = _playerInput.actions["Skill"];
        if (skillAction != null)
        {
            networkInput.IsSkill = skillAction.triggered;
        }

        // Resurrection
        InputAction resurrectionAction = _playerInput.actions["Resurrection"];
        if (resurrectionAction != null)
        {
            networkInput.IsResurrection = resurrectionAction.triggered;
        }

        input.Set(networkInput);
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