using UnityEngine;
using TMPro; 

public class GarageDoor : MonoBehaviour
{
    public bool isClosed = true;     
    public float openHeight = 3.0f;  
    public float speed = 2.0f;        
    public GameObject textHienThi;   

    private Vector3 viTriDong;
    private Vector3 viTriMo;   
    private bool nguoiChoiOgan = false;

    void Start()
    {
        viTriDong = transform.localPosition; 
        viTriMo = viTriDong + new Vector3(0, openHeight, 0); 

        if (textHienThi != null) textHienThi.SetActive(false); 
    }

    void Update()
    {
        Vector3 dichDen = isClosed ? viTriDong : viTriMo;
        transform.localPosition = Vector3.Lerp(transform.localPosition, dichDen, Time.deltaTime * speed);

        if (nguoiChoiOgan && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }
    }

    public void ToggleDoor()
    {
        isClosed = !isClosed; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            nguoiChoiOgan = true;
            if (textHienThi != null) textHienThi.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nguoiChoiOgan = false;
            if (textHienThi != null) textHienThi.SetActive(false);
        }
    }
}