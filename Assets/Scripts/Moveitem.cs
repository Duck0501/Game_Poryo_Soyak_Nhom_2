using UnityEngine;

public class Moveitem : MonoBehaviour
{
    private Vector3 startPos;
    private bool isDragging = false;

    private Transform validDropPoint;

    void Start()
    {
        startPos = transform.position;
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        Transform nearestPoint = FindNearestDropPoint();
        if (nearestPoint != null && Vector2.Distance(transform.position, nearestPoint.position) < 0.5f)
        {
            // Snap item to point
            transform.position = nearestPoint.position;
        }
        else
        {
            // Return to original position
            transform.position = startPos;
        }
    }

    Transform FindNearestDropPoint()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("DropPoint");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject point in points)
        {
            float dist = Vector2.Distance(transform.position, point.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = point.transform;
            }
        }

        return nearest;
    }
}
