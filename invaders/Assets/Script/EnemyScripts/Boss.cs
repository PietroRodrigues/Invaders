

public class Boss : BossStatos
{
    void Start()
    {
        statos.target = FindAnyObjectByType<Player>().gameObject;
    }

    void Update()
    {
        Diferencial();
    }

    public override void Diferencial()
    {
        if (bossActive)
        {

            base.Diferencial();

        }
    }

}
