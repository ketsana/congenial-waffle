using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour, IMoveVelocity
{
    [SerializeField]
    private float m_speed = 1f;

    public void SetVelocity(Vector3 p_moveDirection)
    {
        p_moveDirection.z = 0f;
        this.transform.Translate(p_moveDirection * m_speed);
    }
}
