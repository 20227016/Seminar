
using UnityEngine;
using System.Collections;

public class B : MonoBehaviour,IMove
{

    private CharacterStatusStruct _characterStatusStruct;

    private IMove _move;
    private IAvoidance _avoidance;
    private void Start()
    {
        // �ŏ��͕���
        _move = GetComponent<PlayerWalk>();
        _avoidance = GetComponent<PlayerAvoidance>();

        // null �`�F�b�N
        if (_move == null)
        {
            Debug.LogError("Run�R���|�[�l���g���A�^�b�`����Ă��܂���");
        }

        if (_avoidance == null)
        {
            Debug.LogError("PlayerAvoidance�R���|�[�l���g���A�^�b�`����Ă��܂���");
        }
    }

    private void Update()
    {
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // "Shift"�L�[�������Ƒ���A����ȊO�͕���
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _move = GetComponent<PlayerRun>();
        }
        else
        {
            _move = GetComponent<PlayerWalk>();
        }

        // null �`�F�b�N
        if (_move != null)
        {
            // �ړ������̎��s
            _move.Move(moveDirection);
        }
        else
        {
            Debug.LogError("Move�R���|�[�l���g�����݂��܂���B");
        }
    }

    public void Move(Vector2 moveDirection)
    {
        _move?.Move(moveDirection); // �ړ��̌Ăяo��
    }
}