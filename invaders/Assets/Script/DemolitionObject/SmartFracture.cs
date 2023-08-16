using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartFracture : MonoBehaviour
{
   bool quebrado = false;

   public float breakRadius = .2f;
   [HideInInspector] public List<SubFracture> cells;

   [SerializeField]private float durability = 0;
   [Range(1, 100)]
   [SerializeField]private float collapseLevel = 40;
   [SerializeField]private int suportMin = 20;

   [SerializeField]float activationDelay = 0.1f;

   void Start()
   {
      InitSubFractures();
   }

   void InitSubFractures()
   {
      for (int i = 0; i < transform.childCount; i++)
      {
         SubFracture cell = transform.GetChild(i).GetComponent<SubFracture>();
         cell.parent = GetComponent<SmartFracture>();
         cell.rb = transform.GetChild(i).GetComponent<Rigidbody>();
         cell.rb.isKinematic = true;
         cells.Add(cell);
      }

   }

   private void Update()
   {
      if(!quebrado){
         int cellInIntacts = 0;
         int cellSuport = 0;

         foreach (SubFracture cell in cells)
         {
            if (cell.rb.isKinematic)
               cellInIntacts++;

            if (cell.Suporte)
            {
               cellSuport++;
            }
         }

         durability = (float)cellInIntacts / cells.Count * 100;

            if (cellSuport <= suportMin || durability <= collapseLevel)
            {
               StartCoroutine(Collapse());
               quebrado = true;
            }
      }
   }

  IEnumerator Collapse()
    {
        foreach (SubFracture cell in cells)
        {
            yield return new WaitForSeconds(activationDelay); // Espera o tempo definido
            
            Rigidbody rb = cell.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }

   public void Fracture(Vector3 point, Vector3 force)
   {
      foreach (SubFracture cell in cells)
      {
         Rigidbody cellRigidbody = cell.GetComponent<Rigidbody>();

         if (cellRigidbody.isKinematic)
         {
            Vector3 globalCellPosition = transform.TransformPoint(cell.transform.position);
            float distanceToCell = Vector3.Distance(point, globalCellPosition);

            if (distanceToCell < breakRadius)
            {
               cell.Suporte = false;
               cellRigidbody.isKinematic = false;
            }
         }
      }
   }

}