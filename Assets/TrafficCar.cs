using UnityEngine;

public class TrafficCar : MonoBehaviour
{
    public Transform[] waypoints; // Danh sách các điểm cần đi qua
    public float speed = 5.0f;    // Tốc độ xe
    public float rotationSpeed = 2.0f; // Tốc độ xoay đầu xe
    public float reachDist = 1.0f; // Khoảng cách coi như "đã đến nơi"

    private int currentPoint = 0; // Đang đi đến điểm số mấy

    void Update()
    {
        // Nếu chưa gán đường đi thì không chạy
        if (waypoints.Length == 0) return;

        // 1. Tính toán hướng và di chuyển
        Transform target = waypoints[currentPoint];
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Giữ xe không bị chúi đầu xuống đất

        // Quay đầu xe hướng về mục tiêu
        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        }

        // Di chuyển xe
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // 2. Kiểm tra xem đã đến nơi chưa
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= reachDist)
        {
            currentPoint++; // Chuyển sang điểm tiếp theo

            // Nếu đi hết danh sách thì quay về điểm 0 (Vòng lặp)
            if (currentPoint >= waypoints.Length)
            {
                currentPoint = 0;
            }
        }
    }
}