using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// RoomManager.cs
/// クラス説明
/// ネットワーク管理
///
/// 作成日: 10/9
/// 作成者: 高橋光栄
/// </summary>
public class RoomManager : NetworkBehaviour, INetworkRunnerCallbacks
{

    [SerializeField, Tooltip("ネットワークランナー")]
    //private NetworkRunner _networkRunner = default; 
    private static NetworkRunner _networkRunner; // 静的なインスタンスとして宣言

    [SerializeField]
    private GameObject _roomButtonPrefab; // 部屋ボタンのプレハブ

    [SerializeField]
    private Transform roomListContent; // ScrollView の Content にアタッチ

    private TextMeshProUGUI _buttonText = default;

    private StartGameResult result;

    [SerializeField]
    private TextMeshProUGUI cardNameText;
    [SerializeField]
    private TextMeshProUGUI _cardNameText;

    private int currentPlayerCount = 0;
    

    // 部屋情報を格納するためのシンプルなリスト
    private List<RoomInfo> preDefinedRooms = new List<RoomInfo>
    {
         new RoomInfo("Room 1", 0, 4),
         new RoomInfo("Room 2", 0, 4),
         new RoomInfo("Room 3", 0, 4),
         new RoomInfo("Room 4", 0, 4),
    };

    private void Start()
    {
        // UIを初期化
        InitializeRoomListUI();
        if (_networkRunner == null)
        {
            print("ランナーを作成しました");
            _networkRunner = gameObject.AddComponent<NetworkRunner>();
        }
    }

    private void Update()
    {
        print(result);
        cardNameText.text = "ランナー " + _networkRunner.IsRunning;
        _cardNameText.text = "現在の人数 " + currentPlayerCount;
    }

    private void InitializeRoomListUI()
    {

        // リストにあるストリング型(roomNames)を取得し、それをroomNameに代入する(リストにある命名数分、ループし、ボタンを生成する)
        foreach (RoomInfo room in preDefinedRooms)
        {

            // ボタンのプレハブをインスタンス化
            GameObject roomButtonInstance = Instantiate(_roomButtonPrefab, roomListContent);

            // ボタンにルーム名を表示
            _buttonText = roomButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (_buttonText != null)
            {
                _buttonText.text = room.RoomName;
            }

            // ボタンをクリックしたら、JoinRoom(メソッド)を呼ぶ
            Button button = roomButtonInstance.GetComponent<Button>();
            if (button != null)
            {
                // クリックしたルームの名前(roomName)を取得し、JoinRoom(string roomName)に送る
                button.onClick.AddListener(() => JoinRoom(room.RoomName));
            }
        }
    }

    // 部屋に入室するための処理
    private async void JoinRoom(string roomName)
    {
        Debug.Log($"{roomName} に入室を試みます。");

        // 現在のプレイヤー数を取得
        currentPlayerCount = GetCurrentPlayerCount(roomName);

        //if (_networkRunner.IsServer)
        //{
        //    // ホストとしてプレイヤー数をカウント
        //    currentPlayerCount = GetCurrentPlayerCount(roomName);
        //    Debug.Log("ホストがプレイヤー数をカウント: " + currentPlayerCount);
        //}
        //else
        //{
        //    // クライアントはホストから情報を受け取る（例えば、RPCを使用）
        //    currentPlayerCount = await GetPlayerCountFromHost(roomName);
        //    Debug.Log("クライアントがホストからプレイヤー数を受け取りました: " + currentPlayerCount);
        //}



        GameMode gameMode;
        if (currentPlayerCount == 0)
        {
            Debug.Log("最初のプレイヤーとしてホストになります。");
            gameMode = GameMode.Host; // 最初のプレイヤーはホスト
        }
        else
        {
            Debug.Log("クライアントとして入室します。");
            gameMode = GameMode.Client; // それ以降はクライアント
        }

        // ゲーム開始の引数設定
        var startGameArgs = new StartGameArgs()
        {
            GameMode = gameMode,   // 選択されたゲームモードを設定
            SessionName = roomName,   // 入室する部屋の名前
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()  // シーンマネージャー設定
        };


        //if (_networkRunner != null && _networkRunner.IsRunning)
        //{
        //    Debug.LogWarning("既に部屋に入室しています。再接続を試みる前にランナーを停止します。");
        //    await _networkRunner.Shutdown();
        //}

        if (_networkRunner == null)
        {
            Debug.LogError("ランナーがありません。");
            _networkRunner = gameObject.AddComponent<NetworkRunner>();
            return;
        }

        Debug.Log($"ランナーは現在実行中か: {_networkRunner.IsRunning}");
        result = await _networkRunner.StartGame(startGameArgs);


        if (result.Ok)
        {

            print("部屋の入室に成功しました！");

            // なぜか二人目でここで止まる
            RoomInfo room = preDefinedRooms.Find(r => r.RoomName == roomName);

            Debug.Log($"ランナーは現在実行中か（再確認）: {_networkRunner.IsRunning}");

            if (room != null)
            {
                print("部屋の入室を確認致しました。部屋の情報を更新いたします。");

                // プレイヤー数を更新
                room.UpdatePlayerCount(room.CurrentPlayerCount + 1);

                // ローカルのプレイヤー数を更新
                UpdateRoomInfo(roomName, currentPlayerCount);

                // 全クライアントにプレイヤーが入室したことを通知
                RPC_NotifyPlayerJoined(roomName, currentPlayerCount);

            }

        }
        else
        {
            Debug.LogError($"部屋 {roomName} に入室できませんでした: {result.ShutdownReason}");
        }
    }


    // -------------------------------------------------------------------------------クライアントがホストからプレイヤー数を取得するための総合処理

    private async Task<int> GetPlayerCountFromHost(string roomName)
    {
        // RPCでホストからプレイヤー数を取得する処理
        return await Task.FromResult(0); // 仮の処理。実際にはRPCを実装
    }

    // プレイヤー数を取得するRPC
    public void RPC_GetPlayerCount(NetworkRunner runner, string roomName)
    {
        // ここでルーム名に基づいてプレイヤー数を取得
        int playerCount = GetPlayerCount(roomName); // 実装に応じてプレイヤー数を取得

        // プレイヤー数をクライアントに返す
        RPC_SendPlayerCount(playerCount);
    }

    // クライアントにプレイヤー数を送信するRPC
    private void RPC_SendPlayerCount(int playerCount)
    {
        // プレイヤー数をクライアント側で処理する
        // 例: playerCountを保存またはUIを更新
        Debug.Log($"プレイヤー数: {playerCount}");
    }

    private int GetPlayerCount(string roomName)
    {
        // ここで実際のプレイヤー数を計算して返す
        // 仮にルーム名に基づいて固定の数を返す
        return 3; // 例: 3人がプレイ中
    }

    // ---------------------------------------------------------------------------------------終わり



    /// <summary>
    /// プレイヤーが入室したことを他のクライアントに通知するメソッド
    /// RPCの発信者を指定(RpcSources.StateAuthorit)し、
    　/// 全クライアント(RpcTargets.All)に通知を送る
    /// 〇このメソッドが呼ばれるタイミング
    /// ・プレイヤーが部屋に参加したとき
    /// </summary>
    /// <param name="roomName">部屋の変数名</param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_NotifyPlayerJoined(string roomName, int currentPlayerCount)
    {

        Debug.Log($"RPCで通知: 部屋 {roomName} にプレイヤーが参加しました");

        // 既存のUIを更新するためのメソッド
        // 入室した部屋のプレイヤー数を更新
        UpdateRoomInfo(roomName, currentPlayerCount);
    }

    /// <summary>
    /// 全てのクライアントで部屋の情報を更新する
    /// </summary>
    /// <param name="roomName"></param>
    /// <param name="currentPlayerCount"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_UpdateRoomInfo(string roomName, int currentPlayerCount)
    {
        // すべてのクライアントで部屋の情報を更新する
        UpdateRoomInfo(roomName, currentPlayerCount);
    }

    /// <summary>
    /// プレイヤーが入室した後にRPCで他のクライアントに通知する
    /// </summary>
    private void NotifyPlayerJoined(string roomName)
    {
        RoomInfo room = preDefinedRooms.Find(r => r.RoomName == roomName);
        if (room != null)
        {
            // 他のクライアントにRPCで更新を通知
            RPC_UpdateRoomInfo(roomName, room.CurrentPlayerCount);
        }
    }

    /// <summary>
    /// 部屋の情報を更新する
    /// </summary>
    private void UpdateRoomInfo(string roomName, int newPlayerCount)
    {
        foreach (var room in preDefinedRooms)
        {
            if (room.RoomName == roomName)
            {
                // プレイヤー数を更新
                room.CurrentPlayerCount = newPlayerCount;

                // ボタンのテキストを更新
                _buttonText = room.ButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
                if (_buttonText != null)
                {
                    _buttonText.text = $"{room.RoomName} ({room.CurrentPlayerCount}/{room.MaxPlayerCount})";
                }
                break;
            }
        }
    }

    // 部屋名を渡して、現在のプレイヤー数を取得するメソッド
    private int GetCurrentPlayerCount(string roomName)
    {
        // 指定された部屋名に基づいて RoomInfo を検索
        RoomInfo room = preDefinedRooms.Find(r => r.RoomName == roomName);
        if (room != null)
        {
            // 部屋が見つかった場合、その部屋の現在のプレイヤー数を返す
            return room.CurrentPlayerCount;
        }

        // 部屋が見つからなかった場合、0を返す（エラー処理も検討可能）
        return 0;
    }


    /// <summary>
    /// ネットワークで使用する変数の管理
    /// </summary>
    public class RoomInfo
    {

        public string RoomName { get; private set; }
        public int CurrentPlayerCount { get; set; } // 現在のプレイヤー数
        public int MaxPlayerCount { get; private set; } // 最大プレイヤー数
        public GameObject ButtonInstance { get; set; } // ボタンのインスタンス

        public RoomInfo(string roomName, int currentPlayerCount, int maxPlayerCount)
        {
            RoomName = roomName;
            CurrentPlayerCount = currentPlayerCount;
            MaxPlayerCount = maxPlayerCount;
        }

        public void UpdatePlayerCount(int newPlayerCount)
        {
            CurrentPlayerCount = newPlayerCount;
        }
    }



    // INetworkRunnerCallbacks を実装するために必要なメソッド群

    /// <summary>
    /// プレイヤーが接続をリクエストしたときに呼ばれる
    /// </summary>
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        // ここで接続リクエストに応じる処理を行う（必要であれば）
        Debug.Log("ConnectRequest received");
    }

    /// <summary>
    /// カスタム認証のレスポンスが来たときに呼ばれる
    /// </summary>
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        // カスタム認証の結果を処理する（認証が必要な場合）
        Debug.Log("CustomAuthenticationResponse received");
    }

    /// <summary>
    /// 信頼性のあるデータが受信されたときに呼ばれる
    /// </summary>
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        // データを処理する（信頼性のあるデータ受信時）
        Debug.Log("Reliable data received");
    }

    /// <summary>
    /// セッションリストが更新されたときに呼ばれる
    /// </summary>
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        // セッションリストが更新された際の処理（必要であればUIなどに反映）
        Debug.Log("Session list updated");
    }

    /// <summary>
    /// ユーザーシミュレーションメッセージが送られてきたときに呼ばれる
    /// </summary>
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        // シミュレーションメッセージを処理する
        Debug.Log("User simulation message received");
    }

    /// <summary>
    /// 接続が失敗したときに呼ばれる
    /// </summary>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player {player.PlayerId} has joined.");
    }

    /// <summary>
    /// プレイヤーが切断されたときに呼ばれる
    /// </summary>
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player {player.PlayerId} has left.");
    }

    /// <summary>
    /// ゲームがスタートしたときに呼ばれる
    /// </summary>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // プレイヤーからの入力を処理する
    }

    /// <summary>
    /// ネットワークの状態が変更されたときに呼ばれる
    /// </summary>
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"Runner shut down due to {shutdownReason}");
    }

    /// <summary>
    /// シーンがロードされたときに呼ばれる
    /// </summary>
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("Scene load done");
    }

    /// <summary>
    /// シーンロードが開始されたときに呼ばれる
    /// </summary>
    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("Scene load started");
    }

    /// <summary>
    /// ネットワークのイベントが処理されたときに呼ばれる
    /// </summary>
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.LogWarning($"Input missing for player {player.PlayerId}");
    }

    /// <summary>
    /// ネットワーク接続に問題が発生したときに呼ばれる
    /// </summary>
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("Disconnected from server");
    }

    /// <summary>
    /// 接続が成功したときに呼ばれる
    /// </summary>
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to server");
    }

    /// <summary>
    /// プレイヤーがドロップしたときに呼ばれる
    /// </summary>
    public void OnPlayerFailedToJoin(NetworkRunner runner, PlayerRef player, StartGameResult startGameResult)
    {
        Debug.LogError($"Player {player.PlayerId} failed to join: {startGameResult.ShutdownReason}");
    }

    /// <summary>
    /// ホストが移行されたときに呼ばれる
    /// </summary>
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("Host migration occurred");
    }

    /// <summary>
    /// 接続が失敗したときに呼ばれる
    /// </summary>
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.LogError($"Failed to connect to {remoteAddress} due to {reason}");
    }
}