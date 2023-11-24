using UnityEngine;

public class SubFracture : MonoBehaviour
{
  
    [HideInInspector] public Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.Sleep();
    }
}