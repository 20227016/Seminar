
using UnityEngine;

public interface IAvoidance
{
    /// <summary>
    /// ������\�b�h
    /// </summary>
    /// <param name="avoidanceDirection">������</param>
    /// <param name="avoidanceDistance">�������</param>
    /// <param name="avoidanceDuration">�������</param>
    void Avoidance(Vector2 avoidanceDirection, float avoidanceDistance, float avoidanceDuration);
}