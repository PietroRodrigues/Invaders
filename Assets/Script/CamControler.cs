using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControler : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    
    [SerializeField] Transform playerTarget;
    [SerializeField] Vector3 focoPos;

    [Range(1,100)][SerializeField]
    float SpeedCamSwith = 5;

    [SerializeField]
    float mouseSensivity = 8;

    [Range(10,80)][SerializeField]
    float fieldOfView = 40f;

    [Header("Position Cam")]
    [SerializeField] float distance = 25;
    [SerializeField] float altura = 6;
    [SerializeField] float pan = 0;

    float camDistance = 0; 
    float camAltura = 0;
    float camPan = 0;

    Vector2 yMinMax = new Vector2(-75, 75);

    //Hud hud;
    Vector3 foco;
    Vector3 rotationSmoothSpeed;
    RaycastHit hit;
    Vector3 rotationAtual;

    bool NotOrbit;

    float x;
    float y;

    void Start(){       
        playerTarget.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    void Update(){
        
        if(Input.GetKeyDown(KeyCode.LeftAlt)) NotOrbit = !NotOrbit;

        foco = focoPos + playerTarget.position;
    }

    // Update is called once per frame
   void LateUpdate()
    {
        transform.localEulerAngles = CamRotationOrbital(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));

        transform.position = CamColider();  
    }

    Vector3 CamRotationOrbital(float eixoX, float eixoY){

        if(NotOrbit)
            return rotationAtual;

        x += eixoX * mouseSensivity ;
        y -= eixoY * mouseSensivity ;
        
        y = Mathf.Clamp(y , yMinMax.x , yMinMax.y);
        
        rotationAtual = Vector3.SmoothDamp(rotationAtual, new Vector3(y,x),ref rotationSmoothSpeed, 0.12f * Time.deltaTime);

        return rotationAtual;
    }

    float CamDistance(){

        camDistance = Mathf.Lerp(camDistance,distance,SpeedCamSwith * 0.12f);

        return camDistance;
       
    }

    float CamAltura(){

        camAltura = Mathf.Lerp(camAltura,altura,SpeedCamSwith * 0.12f);

        return camAltura;

    }

    float CamPan(){

        camPan = Mathf.Lerp(camPan,0,(SpeedCamSwith/4) * 0.12f);

        return camPan;
    }

    Vector3 CamPos(){

        Vector3 distance =  transform.forward * CamDistance();

        Vector3 altura =  transform.up * CamAltura();

        Vector3 pam =  transform.right * CamPan();

        Vector3 camPos = (foco + altura) - (distance + pam);

        return camPos;

    }

    Vector3 CamColider(){
        
        Vector3 pos = Vector3.zero;
        Vector3 camPos = CamPos();  
        
        bool see = Physics.Linecast(foco,camPos, out hit,1,QueryTriggerInteraction.Ignore);

        if(see){
            pos = hit.point + (foco - camPos) * 0.12f;
        }else{
            pos = camPos;
        }
        
        return pos;

    }

    public void CamFoco(bool inputPress){

        if(inputPress){
            mainCam.fieldOfView = Mathf.Lerp( mainCam.fieldOfView, 20, 0.12f);
            altura = Mathf.Lerp(altura,2.5f,0.12f);
        }else{
            altura = Mathf.Lerp(altura,5,0.12f);;
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, fieldOfView, 0.12f);
        }
        
    }

}
