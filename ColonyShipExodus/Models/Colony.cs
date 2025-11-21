namespace ColonyShipExodus.Models;

/// <summary>
/// Represents the player's colony, managing survivors, inventory, and buildings.
/// </summary>
public class Colony
{
    public List<Survivor> Survivors { get; set; }
    public Inventory Inventory { get; set; }
    public List<Building> Buildings { get; set; }

    public Colony()
    {
        Survivors = new List<Survivor>();
        Inventory = new Inventory();
        Buildings = new List<Building>();
    }

    public void DisplayStatus()
    {
        ConsoleUI.DisplaySubHeader("Colony Resources");
        Inventory.Display();
        
        Console.WriteLine();
        ConsoleUI.DisplaySubHeader("Survivor Roster");
        foreach (var survivor in Survivors)
        {
            survivor.DisplayStatus();
        }
    }
}