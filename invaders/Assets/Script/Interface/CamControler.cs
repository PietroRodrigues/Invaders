using UnityEngine;
using UnityEngine.ProBuilder;

public class CamControler : MonoBehaviour
{
    [SerializeField] bool showDebugInfo;
    [SerializeField] bool cameraFixa;
    
    [SerializeField] Camera mainCam;
    [SerializeField] Transform mouseAim;
    [SerializeField] Player player;

    [SerializeField] Vector3 focoPos;
    [SerializeField] float aimDistance;

    [Range(1,100)][SerializeField] float camSmoothSpeed = 5;
    [Range(1,100)][SerializeField] float RootCamSmoothSpeed = 5;
    [Range(1,100)][SerializeField] float camRotateSmoothSpeed = 5;

    [SerializeField]
    public float mouseSensitivity = 3;
    
    [Header("Position Cam")]
    [SerializeField] float distance = 8;
    [SerializeField] float altura = 0;
    [SerializeField] float pan = 0;

    float camDistance = 0; 
    float camAltura = 0;
    float camPan = 0;

    Vector3 camPos;

    [SerializeField] Vector2 yMinMax = new Vector2(-75, 75);

    [SerializeField] HUD hud;
    Vector3 target;
    Vector3 rotationSmoothSpeed;
    Vector3 velocity;
    Vector3 velocityRoot;
    RaycastHit hit;
    Vector3 rotationAtual;

    [HideInInspector] public bool cursorVisible = false;

    [HideInInspector] public PlayerControler playerInputs;

    float x;
    float y;
    float mouseX;
    float mouseY;

    Vector3 initialCamPosition;

    private void Awake()
    {
        transform.parent = null;

    }

    void Start(){

        player = FindObjectOfType<Player>();

        if(player != null){
            player.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");        
            playerInputs = new PlayerControler();
            initialCamPosition = mainCam.transform.position;
        }
        
    }

    void Update(){
        
        if(Input.GetKeyDown(KeyCode.LeftAlt))
            cursorVisible = !cursorVisible;
        
        Cursor.visible = cursorVisible;
        if(!cursorVisible)
           Cursor.lockState = CursorLockMode.Locked;
        else
           Cursor.lockState = CursorLockMode.None;
        
        
        if(player != null){
            
            playerInputs.GameInputs(player);
            
            mouseX = playerInputs.inputsControl.xMause * mouseSensitivity;
            mouseY = playerInputs.inputsControl.yMause * mouseSensitivity;

            target = focoPos + (cameraFixa? player.transform.position : transform.position);

            mouseAim.localEulerAngles = MouseAimRotation(mouseX,mouseY);

            Vector3 upVec = (Mathf.Abs(mouseAim.forward.y) > 0.9f) ? transform.up : Vector3.up;

            mainCam.transform.localRotation = Damp(mainCam.transform.localRotation,Quaternion.LookRotation(mouseAim.forward,upVec),camRotateSmoothSpeed,Time.deltaTime);      

            mouseAim.localPosition = mainCam.transform.localPosition;

            camPos = CamPos();

        }
         
    }

    private void FixedUpdate()
    {
        if(player != null){

            transform.position = Vector3.SmoothDamp(transform.position , player.transform.position, ref velocityRoot,  RootCamSmoothSpeed * Time.deltaTime);
        }
        
    }

    void LateUpdate()
    {
        if(player != null){
        
            Vector3 targetCamPos = Vector3.Lerp(mainCam.transform.position, camPos, camSmoothSpeed * 10 * Time.deltaTime);

            mainCam.transform.position = Vector3.SmoothDamp(mainCam.transform.position, targetCamPos, ref velocity, camSmoothSpeed * Time.deltaTime);
        
        }
    }

    private Quaternion Damp(Quaternion a, Quaternion b, float lambda, float dt)
    {
        return Quaternion.Slerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    Vector3 MouseAimRotation(float eixoX, float eixoY){

        if(cursorVisible)
            return rotationAtual;

        x += eixoX;
        y -= eixoY;
        
        y = Mathf.Clamp(y , yMinMax.x , yMinMax.y);
        
        rotationAtual = Vector3.SmoothDamp(rotationAtual, new Vector3(y,x),ref rotationSmoothSpeed, 0.12f * Time.deltaTime);

        return rotationAtual;
    }
   
    Vector3 CamPos(){

        Vector3 distanceCam = mainCam.transform.forward * CamDistance();
        Vector3 alturaCam = mainCam.transform.up * CamAltura();
        Vector3 panCam = mainCam.transform.right * CamPan();

        Vector3 posCam = target + (alturaCam - distanceCam + panCam);

        bool see = Physics.Linecast(target,posCam, out hit,1,QueryTriggerInteraction.Ignore);

        Debug.DrawLine(target,mainCam.transform.position,Color.yellow);

        return see? hit.point + (target - posCam) * 0.12f : posCam;

    }

    float CamDistance(){

        camDistance = Mathf.MoveTowards(camDistance,distance,camSmoothSpeed * Time.deltaTime);

        return camDistance;
       
    }

    float CamAltura(){

        camAltura = Mathf.MoveTowards(camAltura,altura,camSmoothSpeed * Time.deltaTime);       

        return camAltura;

    }

    float CamPan(){

        camPan = Mathf.MoveTowards(camPan,pan,camSmoothSpeed * Time.deltaTime);

        return camPan;
    }

}
