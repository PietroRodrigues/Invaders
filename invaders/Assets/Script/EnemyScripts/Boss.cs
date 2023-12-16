

public class Boss : BossStatos
{   

    void Start(){
        
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
