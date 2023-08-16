using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubFracture : MonoBehaviour
{
    public bool Suporte;
    [HideInInspector] public SmartFracture parent;
    [HideInInspector] public Rigidbody rb;

    private void Start() {
        parent = GetComponentInParent<SmartFracture>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.isTrigger)
        {
            if (collision.collider.GetComponent<SubFracture>() == null)
            {
                Vector3 globalCollisionPoint = collision.contacts[0].point;

                parent.Fracture(globalCollisionPoint, collision.impulse);
                rb.isKinematic = false;
                Suporte = false;
            }
        }
    }
}