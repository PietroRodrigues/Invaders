using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class TanqueCombustivel : IEstrutura
{
   ComponentsEstrutures componentes;

   public TanqueCombustivel(ComponentsEstrutures conponentes){
      
      this.componentes = conponentes;

   }
   
   public bool AplicaModification(float hp, float hpMax)
   {
      bool destruido = false;
   
      float hpPorcentagen =  hp / hpMax * 100;

      if(hpPorcentagen <= 0){
         destruido = true;
         Explosion();
      }

      FurosPlay(hpPorcentagen,destruido);

      return destruido;
   }

   void FurosPlay(float hpPorcentagen,bool destruido){

      int cont = 0;

      int furosAtivos = Mathf.CeilToInt(componentes.particulas.childCount - ((hpPorcentagen + 10) / 100 * componentes.particulas.childCount));
     
      foreach (Transform furo in componentes.particulas)
      {   
         if(destruido){
            furo.GetComponent<VisualEffect>().Stop();
         }else if(cont < furosAtivos){
            furo.GetComponent<VisualEffect>().Play();
            cont++;
         }          
      }      
   }

   void Explosion(){
      componentes.explosion.Play();
   }

}