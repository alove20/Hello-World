using ColonyShipExodus.Models;

namespace ColonyShipExodus;

/// <summary>
/// A utility class for handling all console input and output.
/// </summary>
public static class ConsoleUI
{
    public static void DisplayHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n--- {title} ---");
        Console.ResetColor();
    }

    public static void DisplaySubHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"* {title}");
        Console.ResetColor();
    }

    public static void DisplayMessage(string message)
    {
        Console.WriteLine($"> {message}");
    }
    
    public static void DisplayWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"! {message}");
        Console.ResetColor();
    }

    public static void DisplayIntro()
    {
        Clear();
        DisplayHeader("Welcome to Colony Ship Exodus");
        DisplayMessage("Your colony ship has crash-landed on an unknown alien world.");
        DisplayMessage("As the commander, you must lead the few survivors to establish a new home.");
        DisplayMessage("Manage your resources, assign tasks, and brave the dangers of this new frontier.");
        Pause();
    }
    
    public static void DisplayGameOver(int survivorCount)
    {
        Clear();
        DisplayHeader("Game Over");
        if (survivorCount <= 0)
        {
            DisplayWarning("All survivors have perished. The hope for humanity on this world dies with them.");
        }
        else
        {
            DisplayMessage("The colony has collapsed.");
        }
        DisplayMessage("Thank you for playing.");
        Pause();
    }

    public static void DisplaySurvivorTaskMenu(Survivor survivor)
    {
        Console.WriteLine($"\nSelect task for {survivor.Name} (Health: {survivor.Health}):");
        Console.WriteLine("  1. Gather Food");
        Console.WriteLine("  2. Gather Water");
        Console.WriteLine("  3. Scavenge for Building Materials");
        Console.WriteLine("  4. Rest and Recover");
    }

    public static int GetPlayerInput(int maxChoice)
    {
        int choice = 0;
        bool isValid = false;
        while (!isValid)
        {
            Console.Write("Enter your choice: ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out choice) && choice >= 1 && choice <= maxChoice)
            {
                isValid = true;
            }
            else
            {
                DisplayWarning($"Invalid input. Please enter a number between 1 and {maxChoice}.");
            }
        }
        return choice;
    }

    public static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    
    public static void Clear()
    {
        Console.Clear();
    }
}