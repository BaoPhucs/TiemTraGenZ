using UnityEngine;

public class CartController : MonoBehaviour
{
    public enum CartState
    {
        DiChuyen,
        BanHang
    }

    public CartState currentState;

    public GameObject khoBanGhe;
    public GameObject phaChe;

    void Start()
    {
        SetState(CartState.DiChuyen);
    }

    public void SetState(CartState newState)
    {
        currentState = newState;

        bool isBanHang = (newState == CartState.BanHang);

        khoBanGhe.SetActive(isBanHang);
        phaChe.SetActive(isBanHang);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            SetState(CartState.BanHang);

        if (Input.GetKeyDown(KeyCode.M))
            SetState(CartState.DiChuyen);
    }
}
