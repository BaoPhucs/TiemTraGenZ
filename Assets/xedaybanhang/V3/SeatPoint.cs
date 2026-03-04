using UnityEngine;

public class SeatPoint : MonoBehaviour
{
    [Header("Trạng thái ghế")]
    public bool isOccupied = false; 

    [Header("Vị trí khách ngồi (Tạo 1 object con trống làm tọa độ)")]
    public Transform sitPosition;


    void OnEnable()
    {
        isOccupied = false;
    }
}