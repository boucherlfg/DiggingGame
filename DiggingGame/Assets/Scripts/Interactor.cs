using UnityEngine;

public class Interactor : Usable
{
    [SerializeField]
    private Targetter targetter;

    public override void Use(PlayerInteraction player)
    {
        if (!targetter.Targetted) return;
        targetter.Targetted.Performed(player);
    }

    public override void StopUsing(PlayerInteraction player)
    {
        if(!targetter.Targetted) return;
        targetter.Targetted.Cancelled(player);
    }
}