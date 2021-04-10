using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRtsController : MonoBehaviour
{
    [SerializeField] 
    private Transform m_selectionAreaTransform;

    private Vector3 m_startPosition;
    private List<UnitRts> m_selectedUnitRtsList;

    private void Awake()
    {
        m_selectedUnitRtsList = new List<UnitRts>();
        m_selectionAreaTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Left Mouse Button Pressed
        if (Input.GetMouseButtonDown(0))
        {
            m_selectionAreaTransform.gameObject.SetActive(true);
            m_startPosition = GetMouseWorldPosition();
        }

        // Left Mouse Button Held Down
        if (Input.GetMouseButton(0))
        {
            Vector3 _currentMousePosition = GetMouseWorldPosition();
            Vector3 _lowerLeft = new Vector3(
                    Mathf.Min(m_startPosition.x, _currentMousePosition.x),
                    Mathf.Min(m_startPosition.y, _currentMousePosition.y)
                );
            Vector3 _upperRight = new Vector3(
                    Mathf.Max(m_startPosition.x, _currentMousePosition.x),
                    Mathf.Max(m_startPosition.y, _currentMousePosition.y)
                );
            m_selectionAreaTransform.position = _lowerLeft;
            m_selectionAreaTransform.localScale = _upperRight - _lowerLeft;
        }

        // Left Mouse Button Released
        if (Input.GetMouseButtonUp(0))
        {
            m_selectionAreaTransform.gameObject.SetActive(false);

            Debug.Log("Box Detected: " + m_startPosition + " | " + GetMouseWorldPosition());
            Collider2D[] _collider2dArray = Physics2D.OverlapAreaAll(m_startPosition, GetMouseWorldPosition());

            // Deselect all Units
            foreach (UnitRts unitRts in m_selectedUnitRtsList)
            {
                unitRts.SetSelectedVisible(false);
            }

            m_selectedUnitRtsList.Clear();

            // Select Units within Selection Area
            foreach (Collider2D collider2D in _collider2dArray)
            {
                UnitRts _unitRts = collider2D.GetComponent<UnitRts>();
                if (_unitRts != null)
                {
                    _unitRts.SetSelectedVisible(true);
                    m_selectedUnitRtsList.Add(_unitRts);
                }
            }

            Debug.Log(m_selectedUnitRtsList.Count);
        }

        // Right Mouse Button Pressed
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 _moveToPosition = GetMouseWorldPosition();

            List<Vector3> _targetPositionList = GetPositionListAround(_moveToPosition, new float[] { 1f, 2f, 3f}, new int[] { 5, 10, 20 });

            int _targetPositionListIndex = 0;
            foreach (UnitRts unitRts in m_selectedUnitRtsList)
            {
                unitRts.MoveTo(_targetPositionList[_targetPositionListIndex]);
                _targetPositionListIndex = (_targetPositionListIndex + 1) % _targetPositionList.Count;
            }
        }
    }

    private List<Vector3> GetPositionListAround(Vector3 p_startPosition, float[] p_ringDistanceArray, int[] p_ringPositionCountArray)
    {
        List<Vector3> _positionList = new List<Vector3>();

        _positionList.Add(p_startPosition);

        for (int i = 0; i < p_ringDistanceArray.Length; i++)
        {
            _positionList.AddRange(GetPositionListAround(p_startPosition, p_ringDistanceArray[i], p_ringPositionCountArray[i]));
        }

        return _positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 p_startPosition, float p_distance, int p_positionCount)
    {
        List<Vector3> _positionList = new List<Vector3>();

        for (int i = 0; i < p_positionCount; i++)
        {
            float _angle = i * (360f / p_positionCount);
            Vector3 _direction = ApplyRotationToVector(new Vector3(1, 0), _angle);
            Vector3 _position = p_startPosition + _direction * p_distance;
            _positionList.Add(_position);
        }

        return _positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 p_vector, float p_angle)
    {
        return Quaternion.Euler(0, 0, p_angle) * p_vector;
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
