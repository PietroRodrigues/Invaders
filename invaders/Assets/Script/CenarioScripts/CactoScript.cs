using UnityEngine;
using UnityEngine.VFX;

public class CactoScript : MonoBehaviour
{
    [SerializeField] GameObject modelo;
    [SerializeField] VisualEffect effect;

    float reativeDeley = 10;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger){
            if(modelo.activeSelf){
                modelo.SetActive(false);
                effect.Play();
            }
        }
    }

    private void Update()
    {
        if(!modelo.activeSelf){
            reativeDeley -= Time.deltaTime;
            if(reativeDeley <= 0){
                modelo.SetActive(true);
                reativeDeley = 10;
            }
        }
    }
}
