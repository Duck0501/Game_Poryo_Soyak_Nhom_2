using System.Collections;
using UnityEngine;

public class Arrow_Control : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 originalPosition;
    private bool isDragging = false;

    [Header("Thả hợp lệ nếu nằm trong khoảng cách này")]
    public float validDropDistance = 0.5f;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Kiểm tra có điểm hợp lệ gần đó không
        GameObject[] dropPoints = GameObject.FindGameObjectsWithTag("DropPoint");
        GameObject nearestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject point in dropPoints)
        {
            float distance = Vector3.Distance(transform.position, point.transform.position);
            if (distance < closestDistance && distance <= validDropDistance)
            {
                closestDistance = distance;
                nearestPoint = point;
            }
        }

        if (nearestPoint != null)
        {
            // Thả vào đúng vị trí
            transform.position = nearestPoint.transform.position;
        }
        else
        {
            // Trả về vị trí ban đầu
            StartCoroutine(SmoothReturnToOriginalPosition());
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0; // 2D
        return mousePoint;
    }

    IEnumerator SmoothReturnToOriginalPosition()
    {
        float elapsed = 0f;
        float duration = 0.2f;
        Vector3 start = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, originalPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
}
