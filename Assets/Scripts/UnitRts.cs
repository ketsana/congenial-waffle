using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRts : MonoBehaviour
{
    private GameObject m_selectedGameObject;
    private IMovePosition m_movePosition;

    private void Awake()
    {
        m_selectedGameObject = this.transform.Find("Selected").gameObject;
        m_movePosition = this.GetComponent<IMovePosition>();
        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool p_visible)
    {
        m_selectedGameObject.SetActive(p_visible);
    }

    public void MoveTo(Vector3 p_targetPosition)
    {
        m_movePosition.SetMovePosition(p_targetPosition);
    }
}
