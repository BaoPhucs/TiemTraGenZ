using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HouseAudio : MonoBehaviour
{
    [Header("--- BỎ 2-3 FILE NHẠC VÀO ĐÂY ---")]
    public AudioClip[] danhSachAmThanh; // Mảng chứa các bài nhạc

    [Header("--- CẤU HÌNH KHOẢNG CÁCH ---")]
    public float khoangCachNgheRo = 2.0f; // Đứng cách 2m nghe to nhất
    public float khoangCachTatTieng = 15.0f; // Đi xa quá 15m là im bặt

    private AudioSource loa;

    void Start()
    {
        loa = GetComponent<AudioSource>();

        // 1. Cấu hình Âm thanh 3D (Quan trọng để đi xa không nghe thấy)
        loa.spatialBlend = 1.0f; // 1.0 là 3D hoàn toàn
        loa.rolloffMode = AudioRolloffMode.Linear; // Giảm âm lượng đều đặn
        loa.minDistance = khoangCachNgheRo;
        loa.maxDistance = khoangCachTatTieng;
        loa.loop = true; // Phát lặp lại liên tục
        loa.playOnAwake = true;

        // 2. Chọn bài ngẫu nhiên
        PhatNhacNgauNhien();
    }

    void PhatNhacNgauNhien()
    {
        if (danhSachAmThanh != null && danhSachAmThanh.Length > 0)
        {
            // Random.Range(0, Length) sẽ lấy số từ 0 đến Length-1
            int baiNgauNhien = Random.Range(0, danhSachAmThanh.Length);

            // Gán bài nhạc vào loa
            loa.clip = danhSachAmThanh[baiNgauNhien];
            loa.Play();
        }
        else
        {
            Debug.LogWarning("Nhà này chưa có băng đĩa nhạc nào cả!");
        }
    }
}