using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Statos
{   
    public GameObject target;
    [SerializeField] GameObject bullet;
    [HideInInspector] public float coodalShoting;

    public float speed = 5f;
    [SerializeField] float AlturaMax;
    [SerializeField] float AlturaMin; 
    [SerializeField] float raioDistance;
    [SerializeField] float checkRadius;
    public LayerMask obstacleLayer;
    [SerializeField] Vector3 newPosition;

    Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Define a nova posição aleatória do objeto
        if(Vector3.Distance(transform.position, newPosition) < 1){
            newPosition = NewPositionGenerator();
        }

        if(newPosition.y > AlturaMax || newPosition.y < AlturaMin)
            newPosition = NewPositionGenerator();
        
        Debug.DrawLine(transform.position,newPosition, Color.red);
        
        // Move o objeto em direção à nova posição com a velocidade especificada
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

        transform.LookAt(newPosition, Vector3.up);
    }

Vector3 NewPositionGenerator(){
    Vector3 randomPoint;
    do
    {
        randomPoint = transform.position + Random.insideUnitSphere * raioDistance;

    } while (Physics.CheckSphere(randomPoint, checkRadius, obstacleLayer, QueryTriggerInteraction.Ignore));

    return randomPoint;
}

private void OnDrawGizmos() {

    // Desenha uma esfera na nova posição gerada
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(newPosition, checkRadius);

    // Desenha uma esfera na posição atual do objeto
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(transform.position, raioDistance);
}
}
