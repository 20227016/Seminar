
using UnityEngine;
using System.Collections;

public class B : MonoBehaviour
{

    private CharacterStatusStruct _characterStatusStruct;

    private IMove _move;
    private IAvoidance _avoidance;
    private void Start()
    {
        // �ŏ��͕���
        _move = GetComponent<PlayerMove>();
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


    }
}