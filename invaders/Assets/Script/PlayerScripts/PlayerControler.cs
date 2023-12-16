using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler
{
    public InputsVar inputsControl;

    public void GameInputs(){
        
        if(!Cursor.visible){
        
            inputsControl.xInput = Input.GetAxisRaw("Horizontal");
            inputsControl.zInput = Input.GetAxisRaw("Vertical");
            inputsControl.xMause = Input.GetAxis("Mouse X");
            inputsControl.yMause = Input.GetAxis("Mouse Y");
            inputsControl.jumpInput =  Input.GetKey(KeyCode.Space);
            inputsControl.Dash =  Input.GetKey(KeyCode.LeftShift);
            inputsControl.special = Input.GetKey(KeyCode.R);
            inputsControl.switchTarget = Input.GetKeyDown(KeyCode.Tab);
            inputsControl.disparar = Input.GetMouseButton(0);
            inputsControl.mirar = Input.GetMouseButton(1);

        }else{

            inputsControl.xInput = 0;
            inputsControl.zInput = 0;
            inputsControl.jumpInput =  false;
            inputsControl.Dash = false;         
            inputsControl.mirar = false;
            inputsControl.switchTarget = false;
        }
    
    }

    public struct InputsVar{
        public float xInput;
        public float zInput;
        public float xMause;
        public float yMause;
        public bool jumpInput;
        public bool Dash;
        public bool mirar;
        public bool disparar;
        public bool special;
        public bool switchTarget;
    }

}
