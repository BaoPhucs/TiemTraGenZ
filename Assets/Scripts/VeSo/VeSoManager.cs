using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // Đã thêm thư viện này để dùng chức năng chờ

public class VeSoManager : MonoBehaviour
{
    [Header("--- UI CỬA HÀNG ---")]
    public GameObject panelVeSo;
    public TextMeshProUGUI txtKetQua;
    public GameObject[] danhSachVeSo;
    public int giaVeSo = 10000;

    [Header("--- UI GỢI Ý ---")]
    public GameObject txtGoiY;

    [Header("--- HAPPY ENDING ---")]
    public GameObject videoHappyEnding;
    public VideoPlayer videoPlayerHappy;
    public GameObject panelHaiNutBam;

    [Header("--- GIAO DIỆN CẦN ẨN KHI ENDING ---")]
    public GameObject txtViral;
    public GameObject txtTinhLang;

    [Header("--- ÂM THANH ---")]
    public AudioSource audioSource;
    public AudioClip amThanhTrungGio;
    public AudioClip amThanhCoLamMoiCoAn;
    public AudioClip amThanhTinhDay;

    private bool minhDangOGan = false;
    private int veTrungThuong = -1;
    private bool hackTrungThuong = false;

    void Start()
    {
        if (panelVeSo != null) panelVeSo.SetActive(false);
        if (videoHappyEnding != null) videoHappyEnding.SetActive(false);
        if (panelHaiNutBam != null) panelHaiNutBam.SetActive(false);
        if (txtGoiY != null) txtGoiY.SetActive(false);
    }

    void Update()
    {
        if (minhDangOGan && Input.GetKeyDown(KeyCode.E))
        {
            MoCuaHang();
        }

        // BẤM F5 ĐỂ KÍCH HOẠT HACK TRÚNG ĐỘC ĐẮC 100%
        if (Input.GetKeyDown(KeyCode.F5))
        {
            hackTrungThuong = true;
            Debug.Log("🚨 BẬT HACK F5: Tờ vé số tiếp theo chắc chắn trúng!");
        }
    }

    void MoCuaHang()
    {
        if (txtGoiY != null) txtGoiY.SetActive(false);

        panelVeSo.SetActive(true);

        txtKetQua.text = "";
        txtKetQua.color = Color.white;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        foreach (var btn in danhSachVeSo) { btn.SetActive(true); }

        veTrungThuong = Random.Range(0, 10);
    }

    public void ChonVeSo(int viTriCuaVe)
    {
        if (QuanLyKho.Instance.MuaVeSo(giaVeSo))
        {
            foreach (var btn in danhSachVeSo) { btn.SetActive(false); }

            if (hackTrungThuong)
            {
                veTrungThuong = viTriCuaVe;
                hackTrungThuong = false;
            }

            if (viTriCuaVe == veTrungThuong)
            {
                txtKetQua.text = "TRÚNG ĐỘC ĐẮC 2 TRIỆU ĐỒNG!!!";
                txtKetQua.color = Color.yellow;
                QuanLyKho.Instance.TrungDocDac(2000000);

                if (audioSource != null) audioSource.Stop();

                if (panelVeSo != null) panelVeSo.SetActive(false);
                if (txtViral != null) txtViral.SetActive(false);
                if (txtTinhLang != null) txtTinhLang.SetActive(false);

                Time.timeScale = 1f;

                if (videoHappyEnding != null)
                {
                    videoHappyEnding.SetActive(true);
                    if (videoPlayerHappy != null)
                    {
                        videoPlayerHappy.Stop();
                        videoPlayerHappy.Play();

                        // Gọi chức năng chờ video hát xong bằng Coroutine (Vượt lỗi của Unity)
                        StartCoroutine(ChoHienNutBaoDam());
                    }
                }

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                txtKetQua.color = Color.red;
                int randomChui = Random.Range(0, 3);

                if (randomChui == 0)
                {
                    txtKetQua.text = "Chúc mừng bạn đã TRÚNG GIÓ !!!!!!!! HẸ HẸ HẸ";
                    PlayNhac(amThanhTrungGio);
                }
                else if (randomChui == 1)
                {
                    txtKetQua.text = "Tỉnh dậy đi cháu ơi, không ai cứu được cháu đâu!";
                    PlayNhac(amThanhTinhDay);
                }
                else
                {
                    txtKetQua.text = "Có làm thì mới có ăn, ...";
                    PlayNhac(amThanhCoLamMoiCoAn);
                }
            }
        }
        else
        {
            txtKetQua.text = "Nghèo mà đòi chơi vé số! Cút!";
            txtKetQua.color = Color.red;
        }
    }

    // --- ĐỒNG HỒ THEO DÕI VIDEO (CHUẨN 100%) ---
    IEnumerator ChoHienNutBaoDam()
    {
        // Đợi 0.5 giây để video kịp khởi động
        yield return new WaitForSeconds(0.5f);

        // Kẹt ở vòng lặp này cho đến khi video dừng chạy
        while (videoPlayerHappy.isPlaying)
        {
            yield return null;
        }

        // Bùm! Video tắt thì hiện 2 nút lên
        if (panelHaiNutBam != null) panelHaiNutBam.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void PlayNhac(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }

    public void DongCuaHang()
    {
        panelVeSo.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (minhDangOGan && txtGoiY != null) txtGoiY.SetActive(true);
    }

    public void NhonTiepTucChoi()
    {
        if (videoHappyEnding != null) videoHappyEnding.SetActive(false);
        if (panelHaiNutBam != null) panelHaiNutBam.SetActive(false);
        DongCuaHang();
    }

    public void NhanChoiLaiTuDau()
    {
        Time.timeScale = 1f;
        QuanLyKho.Instance.ResetGameToZero();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            minhDangOGan = true;
            if (txtGoiY != null) txtGoiY.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            minhDangOGan = false;
            DongCuaHang();
            if (txtGoiY != null) txtGoiY.SetActive(false);
        }
    }
}