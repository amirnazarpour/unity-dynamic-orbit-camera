using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float radius = 5f;
    private float angle = 0f;
    public float rotationSpeed = 30f;

    [Header("Vertical movement")] public int verticalLimitHeightMax;
    public int verticalLimitHeightMin;
    public float verticalSpeed;

    private List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        points = new List<Vector3>();
        transform.position = new Vector3(transform.position.x, verticalLimitHeightMax, transform.position.z);
        points.Add(transform.position);
    }

    void Update()
    {
        if (target == null) return;
        angle += rotationSpeed * Time.deltaTime;
        float radian = angle * Mathf.Deg2Rad;
        float x = target.position.x + radius * Mathf.Cos(radian);
        float z = target.position.z + radius * Mathf.Sin(radian);
        transform.position = new Vector3(x, transform.position.y - verticalSpeed * Time.deltaTime, z);
        if (transform.position.y < verticalLimitHeightMin)
        {
            points.Clear();
            transform.position = new Vector3(transform.position.x, verticalLimitHeightMax, transform.position.z);
            points.Add(transform.position);
        }


        points.Add(transform.position);
        for (int i = 0; i < points.Count - 2; i++)
        {
            Debug.DrawLine(points[i], points[i + 1], Color.green);
        }

        Vector3 direction = (target.position - transform.position).normalized;
        float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float pitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(-pitch, yaw, 0);
        transform.rotation = targetRotation;
    }
}