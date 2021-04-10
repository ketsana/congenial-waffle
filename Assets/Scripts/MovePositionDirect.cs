using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionDirect : MonoBehaviour, IMovePosition
{
    private Vector3 m_movePosition;

    private void Awake()
    {
        m_movePosition = this.transform.position;
    }

    private void Update()
    {
        Vector3 _moveDirection = (m_movePosition - this.transform.position).normalized;

        if (Vector3.Distance(m_movePosition, this.transform.position) < 1f)
        {
            _moveDirection = Vector3.zero;
        }

        GetComponent<IMoveVelocity>().SetVelocity(_moveDirection);
    }

    public void SetMovePosition(Vector3 p_movePosition)
    {
        m_movePosition = p_movePosition;
    }
}
