using UnityEngine;

public class SubFracture : MonoBehaviour
{
    public bool Suporte;
    [HideInInspector] public Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.Sleep();
    }
}