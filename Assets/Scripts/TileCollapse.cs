using UnityEngine;
using System.Collections;

public class TileCollapse : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float delayTime = 1f; // seconds before collapse

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CollapseAfterDelay());
        }
    }

    IEnumerator CollapseAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        Collapse();
    }

    void Collapse()
    {
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}




