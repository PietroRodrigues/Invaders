using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevador : MonoBehaviour
{

    [SerializeField] float altura;
    [SerializeField] float speed;
    [SerializeField] Transform plataforma;

    Vector3 tgPos;

    bool danger = false;

    private void FixedUpdate()
    {
        if(plataforma.transform.localPosition.y <= 0 || danger){
            tgPos = new Vector3(0,altura,0);        
        }else if(plataforma.transform.localPosition.y >= altura){
            tgPos = new Vector3(0,0,0);
            danger = false;
        }

        plataforma.transform.localPosition = Vector3.MoveTowards(plataforma.transform.localPosition,tgPos,speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.CompareTag("Player"))
        {
            if(other.transform.root.transform.position.y < plataforma.transform.position.y)
                danger = true;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.root.CompareTag("Player"))
        {
            danger = false;
        }        
    }

}
