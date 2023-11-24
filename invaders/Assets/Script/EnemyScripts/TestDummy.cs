using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TestDummy : MonoBehaviour
{   
    [SerializeField] Rigidbody rb;
    [SerializeField] VisualEffect poeira;
    [SerializeField] Transform dashEffects;
    PlayerFisics fisica;
    [SerializeField] PartsTank partsTank;

    [SerializeField] float powerPropulsion = 15;
    [SerializeField] float distRaycast = 15;
    [SerializeField] float floatingHeight = 2;



    // Start is called before the first frame update
    void Start()
    {
        fisica = new PlayerFisics(rb,partsTank.cabine.GetComponent<BoxCollider>(),poeira,dashEffects);
    }

    // Update is called once per frame
    void Update()
    {
        fisica.Propulsor(distRaycast,floatingHeight);

        fisica.RotationDirection();
        
    }

    private void FixedUpdate()
    {
        fisica.AplicaFlutuadores(powerPropulsion);    
    }

}
