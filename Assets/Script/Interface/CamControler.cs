using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControler : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    
    [SerializeField] Transform playerTarget;
    [SerializeField] Vector3 focoPos;

    [Range(1,500)][SerializeField]
    float SpeedCamSwith = 5;

    [SerializeField]
    float mouseSensivity = 8;

    [Header("Config Foco")]
    [Range(10,80)][SerializeField]
    float fieldOfView = 0f;
    [Range(10,80)][SerializeField]
    float fieldView = 0;
    [SerializeField] float alturaFoco = 0;

    [Header("Position Cam")]
    [SerializeField] float distance = 25;
    [SerializeField] float altura = 0;
    //[SerializeField] float pan = 0;
    
    

    float camDistance = 0; 
    float camAltura = 0;
    float camPan = 0;

    [SerializeField] Vector2 yMinMax = new Vector2(-75, 75);

    //Hud hud;
    Vector3 LockAtTarget;
    [SerializeField] Vector3 rotationSmoothSpeed;
    [SerializeField] Vector3 suportPosSmoothSpeed;
    RaycastHit hit;
    Vector3 rotationAtual;

    [HideInInspector] public bool cursorVisible = true;

    PlayerControler playerControler;

    float x;
    float y;
    float xMause;
    float yMause;


    private void Awake() {
        Application.targetFrameRate = 60;
    }

    void Start(){    
        playerTarget.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        playerControler = playerTarget.GetComponent<Player>().playerControler;
       
    }

    void Update(){

        if(Input.GetKeyDown(KeyCode.LeftAlt))
            cursorVisible = !cursorVisible;        

        xMause = playerControler.inputsControl.xMause;
        yMause = playerControler.inputsControl.yMause;

        CamFoco(playerControler.inputsControl.foco);

        if(cursorVisible)
           Cursor.lockState = CursorLockMode.None;
        else
           Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = cursorVisible;        

        LockAtTarget = focoPos + playerTarget.position;

        transform.localEulerAngles = CamRotationOrbital(xMause,yMause);
    }

    // Update is called once per frame
   void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,  CamPos(),SpeedCamSwith * Time.deltaTime);
    }

    Vector3 CamRotationOrbital(float eixoX, float eixoY){

        if(cursorVisible)
            return rotationAtual;

        x += eixoX * mouseSensivity ;
        y -= eixoY * mouseSensivity ;
        
        y = Mathf.Clamp(y , yMinMax.x , yMinMax.y);
        
        rotationAtual = Vector3.SmoothDamp(rotationAtual, new Vector3(y,x),ref rotationSmoothSpeed, 0.12f * Time.deltaTime);

        return rotationAtual;
    }

    float CamDistance(){

        camDistance = Mathf.MoveTowards(camDistance,distance,SpeedCamSwith * Time.deltaTime);

        return camDistance;
       
    }

    float CamAltura(){

        camAltura = Mathf.MoveTowards(camAltura,altura,SpeedCamSwith * Time.deltaTime);

        return camAltura;

    }

    float CamPan(){

        camPan = Mathf.MoveTowards(camPan,0,(SpeedCamSwith/4) * Time.deltaTime);

        return camPan;
    }

    Vector3 CamPos(){

        Vector3 distance =  transform.forward * CamDistance();

        Vector3 altura =  transform.up * CamAltura();

        Vector3 pam =  transform.right * CamPan();

        Vector3 camPos = (LockAtTarget + altura) - (distance + pam);

        bool see = Physics.Linecast(LockAtTarget,camPos, out hit,1,QueryTriggerInteraction.Ignore);

        return (see)? hit.point + (LockAtTarget - camPos) * 0.12f : camPos;

    }

    Vector3 CamColider(){
        
        Vector3 pos = Vector3.zero;
        Vector3 camPos = CamPos();  
        
        bool see = Physics.Linecast(LockAtTarget,camPos, out hit,1,QueryTriggerInteraction.Ignore);

        if(see){
            pos = hit.point + (LockAtTarget - camPos) * 0.12f;
        }else{
            pos = camPos;
        }

        return pos;

    }

    void CamFoco(bool inputPress){

        if(inputPress){
            mainCam.fieldOfView = Mathf.Lerp( mainCam.fieldOfView, fieldView, 0.12f);
            camAltura = Mathf.Lerp(camAltura,alturaFoco,0.12f);
        }else{
            camAltura = Mathf.Lerp(camAltura, altura,0.12f);
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, fieldOfView, 0.12f);
        }
        
    }

}
