using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class BossCube : MonoBehaviour
{
   Boss boss;

   [SerializeField] GameObject layser;

   [SerializeField] bool isBoss;

   [SerializeField] Transform cubeRed;
   [SerializeField] Transform cubeBlue;

   bool recharger = false;
   bool attacked = false;
   bool fire = false;


   bool isMoving = false;
   [SerializeField] float rollSpeed = 5;
   float timer = 0;
   float timerRechard = 0;
   float deleyShot = 0;
   [SerializeField] float timerMax = 1;
   [SerializeField] float timerRechardMax = 2;
   float widthLayser = 0.5f;

   [SerializeField] Vector2 grid = new Vector2(0, 0);
   [SerializeField] int limitX = 3;
   [SerializeField] int limitY = 8;

   VisualEffect vfxLayser;
   LineRenderer lineLayser;

   void Start()
   {
      if (isBoss)
      {
         boss = GetComponent<Boss>();
         vfxLayser = layser.GetComponent<VisualEffect>();
         lineLayser = layser.GetComponent<LineRenderer>();
      }
   }

   void Update()
   {
      if (boss != null)
         if (!boss.bossActive)
            return;

      MoveCube();

      if (isBoss)
         DeleyVulnerabilit();
   }

   public void MoveCube()
   {
      if (isBoss)
      {
         if (recharger || boss.statos.hp <= 0) return;

         if (isMoving || cubeRed.localScale.x != 10)
         {
            timer = 0;
            return;
         }
      }
      else
      {
         if (isMoving)
         {
            timer = 0;
            return;
         }
      }

      MoveToGrid();

   }

   void MoveToGrid()
   {

      if (isBoss)
         timer = recharger ? 0 : timer + Time.deltaTime;
      else
         timer += Time.deltaTime;

      if (timer >= timerMax)
      {
         int dirrection = Random.Range(0, 4);

         if (dirrection == 0)
         {
            if (grid.x >= limitX)
            {
               dirrection = SortNewDirection();
            }
         }
         else if (dirrection == 1)
         {
            if (grid.x <= 0)
            {
               dirrection = SortNewDirection();
            }
         }

         if (dirrection == 2)
         {
            if (grid.y >= limitY)
            {
               dirrection = SortNewDirection();
            }
         }
         else if (dirrection == 3)
         {
            if (grid.y <= 0)
            {
               dirrection = SortNewDirection();
            }
         }

         if (dirrection == 0)
         {
            if (grid.x < limitX)
            {
               Assemble(Vector3.left);
               grid.x++;
            }
         }
         else if (dirrection == 1)
         {
            if (grid.x > 0)
            {
               Assemble(Vector3.right);
               grid.x--;
            }
         }
         else if (dirrection == 2)
         {
            if (grid.y < limitY)
            {
               Assemble(Vector3.forward);
               grid.y++;
            }
         }
         else if (dirrection == 3)
         {
            if (grid.y > 0)
            {
               Assemble(Vector3.back);
               grid.y--;
            }
         }

         GridLimits();
         timer = 0;
      }
   }

   void DeleyVulnerabilit()
   {

      if (isMoving) return;

      if (!recharger)
      {
         timerRechard += Time.deltaTime;

         if (timerRechard >= timerRechardMax)
         {
            recharger = true;
         }

      }
      else
      {
         Ray ray = new Ray(transform.position, layser.transform.forward);
         float rayDistance = 100;
         RaycastHit hit;

         bool see = Physics.Raycast(ray, out hit, rayDistance, 1, QueryTriggerInteraction.Ignore);

         lineLayser.SetPosition(1, see ? layser.transform.InverseTransformPoint(hit.point) : layser.transform.InverseTransformPoint(layser.transform.position + layser.transform.forward * rayDistance));

         if (!attacked)
         {
            Attack();
         }

         lineLayser.startWidth = Mathf.Lerp(lineLayser.startWidth, widthLayser, 2f * Time.deltaTime);

         if (lineLayser.startWidth < 0.1f)
            lineLayser.enabled = false;
         else
         {
            if (cubeBlue.localScale.x == 10)
               lineLayser.enabled = true;
         }

         if (lineLayser.enabled)
         {
            if (vfxLayser.aliveParticleCount == 0)
               vfxLayser.Play();
         }
         else
         {
            vfxLayser.Stop();
         }

         if (attacked && vfxLayser.aliveParticleCount == 0)
         {
            timerRechard = 0;
            recharger = false;
            attacked = false;
         }
      }

      if (boss.statos.hp <= 0)
      {
         cubeBlue.localScale = Vector3.MoveTowards(cubeBlue.localScale, Vector3.one * 10, 10f * Time.deltaTime);
         cubeRed.localScale = Vector3.MoveTowards(cubeRed.localScale, Vector3.one * 5, 10f * Time.deltaTime);
      }
      else
      {
         cubeBlue.localScale = Vector3.MoveTowards(cubeBlue.localScale, Vector3.one * (recharger ? 10 : 5), 10f * Time.deltaTime);
         cubeRed.localScale = Vector3.MoveTowards(cubeRed.localScale, Vector3.one * (recharger ? 5 : 10), 10f * Time.deltaTime);
      }
   }

   void Attack()
   {

      if (cubeBlue.localScale.x == 10)
      {

         if (!fire)
         {

            widthLayser = 0.2f;
            deleyShot += Time.deltaTime;

            if (deleyShot > 2)
            {
               fire = true;
            }

         }
         else
         {
            widthLayser = 20f;
            deleyShot -= Time.deltaTime;

            if (deleyShot <= 0)
            {
               attacked = true;
               fire = false;
               widthLayser = 0.01f;
               deleyShot = 0;
            }
         }
      }

   }

   int SortNewDirection()
   {
      int dir = Random.Range(0, 4);
      int otherDirection;

      do
      {
         otherDirection = Random.Range(0, 4);

      } while (dir == otherDirection);

      return otherDirection;
   }

   void GridLimits()
   {
      if (grid.x >= limitX)
      {
         grid.x = limitX;
      }
      else if (grid.x <= 0)
      {
         grid.x = 0;
      }
      else if (grid.y >= limitY)
      {
         grid.y = limitY;
      }
      else if (grid.y <= 0)
      {
         grid.y = 0;
      }

   }

   void Assemble(Vector3 dir)
   {
      var anchor = transform.position + (Vector3.down + dir) * (isBoss ? 5 : 2f);
      var axis = Vector3.Cross(Vector3.up, dir);
      StartCoroutine(Roll(anchor, axis));
   }

   IEnumerator Roll(Vector3 anchor, Vector3 axis)
   {
      isMoving = true;

      for (var i = 0; i < 90 / rollSpeed; i++)
      {
         transform.RotateAround(anchor, axis, rollSpeed);
         yield return new WaitForSecondsRealtime(0.01f);
      }

      isMoving = false;
   }

}