

public class Boss : BossStatos
{   

    void Start(){
        statos.target = FindObjectOfType<Player>().gameObject;
    }

    void Update(){
        Diferencial();
    }

   public override void Diferencial()
   {
        if(bossActive){
           
            base.Diferencial();
    
        }
   }

}
