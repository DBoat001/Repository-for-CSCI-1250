using System;                   // Gives access to Console and basic system functions
using System.Collections.Generic; // Allows use of List<T>
using System.IO;                // Allows reading and writing files

// ==========================================
// CLASS: LibraryItem
// Represents one item in the library catalog
// ==========================================
class LibraryItem
{
    public int Id { get; set; }              // Unique ID for the item
    public string Title { get; set; }        // Item title
    public string Type { get; set; }         // Book / DVD / etc.
    public double DailyLateFee { get; set; } // Late fee charged per day

    public LibraryItem(int id, string title, string type, double dailyLateFee)
    {
        Id = id;                    // Store the ID
        Title = title;              // Store the title
        Type = type;                // Store the type
        DailyLateFee = dailyLateFee;// Store the daily fee
    }

    public override string ToString()
    {
        // Returns a friendly display version of the item
        return $"{Id} - {Title} ({Type}) | Late Fee: ${DailyLateFee}/day";
    }
}

// ==========================================
// CLASS: CheckOutItem
// Holds a list of items the student has checked out
// ==========================================
class CheckOutItem
{
    public List<LibraryItem> Items { get; set; } // All checked-out items

    public CheckOutItem()
    {
        Items = new List<LibraryItem>();  // Start with empty checkout list
    }

    public void AddItem(LibraryItem item)
    {
        Items.Add(item);  // Add a LibraryItem to the checkout list
    }

    public bool RemoveItem(int id)
    {
        // Find the item with matching ID
        LibraryItem found = Items.Find(i => i.Id == id);

        if (found != null)
        {
            Items.Remove(found);  // Remove if found
            return true;          // Tell program the remove worked
        }
        return false;             // Removal failed (not found)
    }
}

class Program
{
    static List<LibraryItem> Catalog = new List<LibraryItem>();  // Holds all library items
    static CheckOutItem Checkout = new CheckOutItem();           // Holds items the user checked out

    static void Main()
    {
        LoadCatalog();  // Load items from catalog.txt when program starts

        bool running = true;  // Controls menu loop

        while (running)
        {
            // Menu options shown to the user
            Console.WriteLine("\n===== LIBRARY CHECKOUT SYSTEM =====");
            Console.WriteLine("1. Add a Library Item");
            Console.WriteLine("2. View Catalog Items");
            Console.WriteLine("3. Check Out an Item");
            Console.WriteLine("4. Return an Item");
            Console.WriteLine("5. View Checkout Receipt");
            Console.WriteLine("6. Save Checkout List");
            Console.WriteLine("7. Load Checkout List");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine(); // Read user input

            // Decide what menu option the user picked
            switch (choice)
            {
                case "1": AddLibraryItem(); break;   // Add new item
                case "2": ViewItems(); break;        // Show catalog
                case "3": CheckoutMenu(); break;     // Check out an item
                case "4": ReturnMenu(); break;       // Return an item
                case "5": ViewReceipt(); break;      // Show late fees receipt
                case "6": SaveCheckoutList(); break; // Save checkout list to file
                case "7": LoadCheckoutList(); break; // Load saved checkout list
                case "8": running = false; break;    // Exit loop (and program)
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }

        Console.WriteLine("Goodbye!"); // Program ends
    }

    // ==========================================
    // LOAD CATALOG FROM FILE
    // ==========================================
    static void LoadCatalog()
    {
        if (!File.Exists("catalog.txt"))
        {
            // If catalog file doesn't exist, program still works with empty catalog
            Console.WriteLine("catalog.txt not found, starting with empty catalog.");
            return;
        }

        // Read all lines from the file
        string[] lines = File.ReadAllLines("catalog.txt");

        // Process each line into a LibraryItem
        foreach (string line in lines)
        {
            string[] p = line.Split(','); // Separate each part by comma

            int id = int.Parse(p[0]);     // Convert ID text to number
            string title = p[1];          // Title
            string type = p[2];           // Type
            double fee = double.Parse(p[3]); // Late fee

            // Add new LibraryItem object to catalog list
            Catalog.Add(new LibraryItem(id, title, type, fee));
        }
    }

    // ==========================================
    // ADD NEW ITEM TO CATALOG
    // ==========================================
    static void AddLibraryItem()
    {
        // Ask user for item info
        Console.Write("Enter new item ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Enter title: ");
        string title = Console.ReadLine();

        Console.Write("Enter type (Book/DVD): ");
        string type = Console.ReadLine();

        Console.Write("Enter daily late fee: ");
        double fee = double.Parse(Console.ReadLine());

        // Create the item object
        LibraryItem item = new LibraryItem(id, title, type, fee);

        // Add to catalog list
        Catalog.Add(item);

        // Also save permanently to file
        File.AppendAllText("catalog.txt", $"{id},{title},{type},{fee}\n");

        Console.WriteLine("Item added and saved to catalog.");
    }

    // ==========================================
    // VIEW ALL CATALOG ITEMS
    // ==========================================
    static void ViewItems()
    {
        Console.WriteLine("\n===== CATALOG ITEMS =====");

        // Loop through all items and print them
        foreach (var item in Catalog)
        {
            Console.WriteLine(item.ToString());
        }
    }

    // ==========================================
    // CHECK OUT AN ITEM
    // ==========================================
    static void CheckoutMenu()
    {
        Console.Write("Enter ID to check out: ");
        int id = int.Parse(Console.ReadLine());

        // Look for item with matching ID
        LibraryItem found = Catalog.Find(i => i.Id == id);

        if (found == null)
        {
            Console.WriteLine("Item not found.");
            return;
        }

        // Prevent double-checkout of the same item
        if (Checkout.Items.Exists(i => i.Id == id))
        {
            Console.WriteLine("You already checked out this item.");
            return;
        }

        // Add item to checkout list
        Checkout.AddItem(found);

        Console.WriteLine("Item checked out!");
    }

    // ==========================================
    // RETURN AN ITEM
    // ==========================================
    static void ReturnMenu()
    {
        Console.Write("Enter ID to return: ");
        int id = int.Parse(Console.ReadLine());

        // Attempt to remove
        if (Checkout.RemoveItem(id))
            Console.WriteLine("Item returned.");
        else
            Console.WriteLine("Item not found in your checkout list.");
    }

    // ==========================================
    // VIEW RECEIPT WITH LATE FEES
    // ==========================================
    static void ViewReceipt()
    {
        Console.WriteLine("\n===== CHECKOUT RECEIPT =====");

        if (Checkout.Items.Count == 0)
        {
            // No items to show
            Console.WriteLine("You have no items checked out.");
            return;
        }

        Console.Write("Enter days late (same for all items): ");
        int lateDays = int.Parse(Console.ReadLine());

        double total = 0; // total late fees

        // Loop through each checked-out item
        foreach (var item in Checkout.Items)
        {
            // Calculate fee = days late * daily rate
            double fee = lateDays > 0 ? lateDays * item.DailyLateFee : 0;

            // Print the line
            Console.WriteLine($"{item.Id} - {item.Title} | {lateDays} days late | Fee: ${fee:0.00}");

            total += fee; // Add to total
        }

        Console.WriteLine($"TOTAL LATE FEES: ${total:0.00}");
        Console.WriteLine("================================");
    }

    // ==========================================
    // SAVE CHECKOUT LIST TO FILE
    // ==========================================
    static void SaveCheckoutList()
    {
        // Open or create the file
        using (StreamWriter writer = new StreamWriter("myCheckouts.txt"))
        {
            // Write each checked-out item to file
            foreach (var item in Checkout.Items)
            {
                writer.WriteLine($"{item.Id},{item.Title},{item.Type},{item.DailyLateFee}");
            }
        }

        Console.WriteLine("Checkout list saved.");
    }

    // ==========================================
    // LOAD CHECKOUT LIST FROM FILE
    // ==========================================
    static void LoadCheckoutList()
    {
        if (!File.Exists("myCheckouts.txt"))
        {
            Console.WriteLine("No saved checkout file found.");
            return;
        }

        Checkout.Items.Clear(); // Remove current checkout items

        // Read each line to rebuild the list
        string[] lines = File.ReadAllLines("myCheckouts.txt");

        foreach (string line in lines)
        {
            string[] p = line.Split(',');

            int id = int.Parse(p[0]);     // ID
            string title = p[1];          // Title
            string type = p[2];           // Type
            double fee = double.Parse(p[3]); // Daily fee

            // Add item back to checkout list
            Checkout.AddItem(new LibraryItem(id, title, type, fee));
        }

        Console.WriteLine("Checkout list loaded.");
    }
}
