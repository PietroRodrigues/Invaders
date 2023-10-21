using UnityEngine;
using UnityEngine.VFX;

public class Estrutura : MonoBehaviour
{
      [HideInInspector] public enum TipoEstrutura {Base, TanquePropano, Sucata, Arvore, Ruina, Pedra}

      [SerializeField] public ComponentsEstrutures components;   

      [SerializeField] Breakable breakable;


      private void Awake()
      {
            components.hp = components.hpMax;
            components.boxCollider = GetComponent<BoxCollider>();
            breakable = new Breakable(components);
        
            switch (components.tipo)
            {     
                  case TipoEstrutura.Base:
                        //components.estruturaSetada = new BasePlayer();
                  break;
                  case TipoEstrutura.Sucata:
                        //components.estruturaSetada = new Suata();
                  break;
                  case TipoEstrutura.Arvore:
                        //components.estruturaSetada = new Arvore();
                  break;
                  case TipoEstrutura.TanquePropano:
                        components.estruturaSetada = new TanqueCombustivel(components);
                  break;
                  case TipoEstrutura.Ruina:
                        //components.estruturaSetada = new Ruina();
                  break;
                  default:
                  break;
            }
            
            
      }

      private void OnCollisionEnter(Collision other)
      {
            breakable.Conlision(other,components.estruturaSetada,other);
      }

}

[System.Serializable]
public struct ComponentsEstrutures
{     
      [HideInInspector]public IEstrutura estruturaSetada;
      [HideInInspector] public BoxCollider boxCollider;
      [SerializeField]public Estrutura.TipoEstrutura tipo;
      [HideInInspector] public float hp ;
      [SerializeField] public float hpMax ;
      public GameObject EsturaInteira;
      public GameObject EsturaQuebrada;
      public Transform particulas;
      public VisualEffect explosion;
}