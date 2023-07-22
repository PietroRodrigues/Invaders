using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUDInventario
{
    RectTransform[] inventario = new RectTransform[3];

    public void LoadInventario (HudComponents components){

        for (int i = 0; i < components.inventario.childCount; i++)
        {
            inventario[i] = components.inventario.GetChild(i).GetComponent<RectTransform>();
        }

    }

    public int UpdateIndicWelpon(int componentIndc){

        int indc = componentIndc;

        if(Input.mouseScrollDelta.y != 0){
        
            indc += (Input.mouseScrollDelta.y > 0) ? -1 : 1;

        }

        if(indc > inventario.Length - 1)
            indc = 0;

        if(indc < 0)
            indc = inventario.Length - 1;

        return indc;

    }

    public void FlipWelpom(HudComponents components){        

        for (int i = 0; i < inventario.Length; i++)
        {
            inventario[i].localScale = Vector2.MoveTowards(inventario[i].localScale, (i == components.indcWelpom)? new Vector3(1.4f,1.4f,1.4f) : Vector3.one ,components.speedTranslation * Time.deltaTime);
        }

    }

}
