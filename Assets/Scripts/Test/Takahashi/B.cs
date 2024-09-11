
using UnityEngine;
using System.Collections;

public class B : MonoBehaviour,IMove
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

    public void Move(Vector2 moveDirection)
    {
        _move?.Move(moveDirection); // 移動の呼び出し
    }
}