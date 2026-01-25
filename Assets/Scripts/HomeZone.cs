using UnityEngine;

public class HomeZone : MonoBehaviour
{
    public bool xeDaVaoNha = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cart")) 
        {
            xeDaVaoNha = true;
            Debug.Log("Xe đã vào nhà an toàn!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cart"))
        {
            xeDaVaoNha = false;
            Debug.Log("Xe lại bị đẩy ra ngoài rồi!");
        }
    }
}