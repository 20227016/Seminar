
using UnityEngine;
using System.Collections;

public class B : MonoBehaviour,IMove,IAvoidance
{

    private CharacterStatusStruct _characterStatusStruct;

    private IMove _move;
    private IAvoidance _avoidance;
    private void Start()
    {
        // 最初は歩く
        _move = GetComponent<PlayerWalk>();
        _avoidance = GetComponent<PlayerAvoidance>();

        // null チェック
        if (_move == null)
        {
            Debug.LogError("Runコンポーネントがアタッチされていません");
        }

        if (_avoidance == null)
        {
            Debug.LogError("PlayerAvoidanceコンポーネントがアタッチされていません");
        }
    }

    private void Update()
    {
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // "Shift"キーを押すと走る、それ以外は歩く
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _move = GetComponent<PlayerRun>();
        }
        else
        {
            _move = GetComponent<PlayerWalk>();
        }

        // スペースキーで回避
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 avoidanceDirection = moveDirection.normalized; // 回避方向は移動方向と同じ
            _avoidance.Avoidance(avoidanceDirection);
        }

        // null チェック
        if (_move != null)
        {
            // 移動処理の実行
            _move.Move(moveDirection);
        }
        else
        {
            Debug.LogError("Moveコンポーネントが存在しません。");
        }
    }

    public void Avoidance(Vector2 avoidanceDirection)
    {
        _avoidance?.Avoidance(avoidanceDirection); // 回避の呼び出し
    }

    public void Move(Vector2 moveDirection)
    {
        _move?.Move(moveDirection); // 移動の呼び出し
    }
}