using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstaculos : MonoBehaviour
{
    [SerializeField] public GameObject plataforma;
    [SerializeField] public GameObject lava;
    [SerializeField] public List<ObjectTrainerStatos> objectTargets;
    
    [HideInInspector] public int defeated = 0;

    private void Start()
    {

        foreach (ObjectTrainerStatos obj in objectTargets)
        {
            obj.initialPos = obj.transformObj.localPosition;
        }
    }

    private void FixedUpdate()
    {
        foreach (ObjectTrainerStatos obj in objectTargets)
        {
            Vector3 tgPos = Vector3.zero;

            float distanceInitialPoit = Vector3.Distance(obj.transformObj.localPosition ,obj.initialPos);

            if(distanceInitialPoit <= obj.distancia && !obj.volta){

                tgPos = new Vector3(obj.x ? obj.transformObj.localPosition.x + obj.distancia : obj.transformObj.localPosition.x,
                                    obj.y ? obj.transformObj.localPosition.y + obj.distancia :obj.transformObj.localPosition.y,
                                    obj.z ? obj.transformObj.localPosition.z + obj.distancia : obj.transformObj.localPosition.z);
            }else{
                if(distanceInitialPoit >= obj.distancia)
                    obj.volta = true;

                if(obj.volta)
                    tgPos = obj.initialPos;

                if(distanceInitialPoit <= 0.1)
                    obj.volta = false;
            }
            
            obj.transformObj.localPosition = Vector3.MoveTowards(obj.transformObj.localPosition,tgPos,obj.speed * Time.deltaTime);

        }

    }
}

[System.Serializable]
public class ObjectTrainerStatos{

    public Transform transformObj;
    public bool x, y, z;
    public float distancia;
    public float speed;
    [HideInInspector] public Vector3 initialPos;
    [HideInInspector] public bool volta;

}
