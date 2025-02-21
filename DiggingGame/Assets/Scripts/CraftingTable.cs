public class CraftingTable : Targettable
{
    private bool _clicked = false;

    public override void Performed(PlayerInteraction player)
    {
        if (_clicked) return;
        _clicked = true;
        
        UIEvents.MenuRequested.Invoke(MenuType.Crafting);
    }

    public override void Cancelled(PlayerInteraction player)
    {
        _clicked = false;
    }
}
