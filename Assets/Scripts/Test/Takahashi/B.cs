
using UnityEngine;
using System.Collections;

public class B : MonoBehaviour,IMove,IAvoidance
{

    private CharacterStatusStruct _characterStatusStruct;

    private IMove _move;
    private IAvoidance _avoidance;
    private void Start()
    {
        _move = GetComponent<IMove>();
        _avoidance = GetComponent<IAvoidance>();
    }

    private void Update()
    {
        
    }

    public void Avoidance(Vector2 avoidanceDirection) { }

    public void Move(Vector2 moveDirection) { }

}