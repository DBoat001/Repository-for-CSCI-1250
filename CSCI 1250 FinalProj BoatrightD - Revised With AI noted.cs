
//Ai (chat gpt mostly) was used to help me structure some sections (mostly spacing because i kept getting spacing errors where things were not on the right lines or tabbeed out properly), i understood the basics of what i wanted 
//things to say and do as well as the functions i wanted but actually typing them out with proper symbols and things proved to be a struggle for me. 
// Important AI Note: I did throw chunks of my own code into ai at times when i ran into spacing problems and asked "(*Copy/Pasted error *) Can you space this properly for me, and then explain what the issue was?". 
// Spacing has consistently been an issue for me throughout this course and has yet to fully click into my brain still (among other things T-T )
// Notes shown throughout code.

using System;                   // Gives access to Console and basic system functions
using System.Collections.Generic; // Allows use of List
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
//AI Note: For creating both CheckOutItem and program Classes, i used AI to help me structure these better as i kept encountering errors, for some reason i still cannot quite figure out, for program using static was not giving me an error later on while public was,
//Through some research i think it had something to do with being able to access static item without needing to create an object and i think i was goofing up somewhere that was making it not able to reference back but i couldnt figure out where so i changed to static 
// which worked so i went with it.
// I dont remeber the exact prompts i used for AI but the consistent style was "im creating a class x, i want a public list that holds y, and the ability to read and change the value, how would i do so and why" <- along those lines
// ==========================================
class CheckOutItem
{
    public List<LibraryItem> Items { get; set; } // All checked-out items   //Ai prompt used: "im creating a class called CheckOutItem, i want a public list that holds LibraryItem, and the ability to read and change the value of Items from outside the class, how would i do that and why"

    public CheckOutItem()
    {
        Items = new List<LibraryItem>();  // Start with empty checkout list
        }

    public void AddItem(LibraryItem item)  // Void was used as i didnt want anything to return
    {
        Items.Add(item);  // Add a LibraryItem to the checkout list
        }

    public bool RemoveItem(int id)  // bool was used to T or F if the item was removed correctly
    {
        // Find the item with matching ID    // Ai promt: "I want to be able to search for an item in the Items list where the ID mathces the ID given, how would i structure that and why?"
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
    static List<LibraryItem> Catalog = new List<LibraryItem>();  // Holds all library items // Catalog is used to hold the list of Library items
    static CheckOutItem Checkout = new CheckOutItem();           // Holds items the user checked out

    static void Main()
    {
        LoadCatalog();  // Load items from catalog.txt when program starts

        bool running = true;  // Controls menu loop

        while (running)
        {
            // Menu options shown to the user // I wrote out all of the console menu lists, but i asked Ai to help me with the title line to try and get it to stay even. "I want a console.write line command that will display "Library Checkout System" as 
                                            // if on a reciept but keep it evenly in the center."
            Console.WriteLine("\n===== LIBRARY CHECKOUT SYSTEM =====");
            Console.WriteLine("1. Add a Library Item");
            Console.WriteLine("2. View Catalog Items");
            Console.WriteLine("3. Check Out an Item");
            Console.WriteLine("4. Return an Item");
            Console.WriteLine("5. View Checkout Receipt");
            Console.WriteLine("6. Save Checkout List");
            Console.WriteLine("7. Load Checkout List");
            Console.WriteLine("8. Exit");
            Console.WriteLine("Choose an option: ");

            string choice = Console.ReadLine(); // Read user input

            // Decide what menu option the user picked  // I found the switch statement on youtube as well, its an alternative to else-if statments that helped drop the number of code lines as well as being a little easier for my brain to understand so i opted for it.
            // Ai Note: I put the below cases 1-8 in ai to ask it how to space it as i was getting an error and found i was simply using break wrong. iI was trying to put it a line below each option instead of at the end 
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
    static void LoadCatalog()  //Method to load catalog  // Again sticking with static because I didn't want to run into the same issues as before and static looks like a much more fun word than public :)
    {
        if (!File.Exists("catalog.txt"))  // Ai note: "I want a way to check if a file named catalog.txt exsists in my C# code what line of code wsould do that?"
        {
            // If catalog file doesn't exist, program still works with empty catalog
            Console.WriteLine("catalog.txt not found, starting with empty catalog.");
            return;
            }

        // Read all lines from the file
        string[] lines = File.ReadAllLines("catalog.txt");

        // Process each line into a LibraryItem   // This is another one I know I had trouble structuring. I was able to ge the string lines figured out.  //Ai Note: for int and double i explained what i was trying to do for both and asked what line of code would help do that and why.
        //Ex. what single line of code would help me convert a text id to a number.  Ex2. What single line of code would help me create a late fee. (it originally returned code with a float but i asked for a double because through research a double is usually more accurate)
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
        // Ask user for item info  // //no Ai note for most of this as i referenced previous code and notes and followed the structure i'd already seen/used
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

        // Also save permanently to file  // AI Note: I asked ai if there was a better way to append multiple things instead of going line by line, then researched a little more specifically to see if what i got was true, which led to this line
        File.AppendAllText("catalog.txt", $"{id},{title},{type},{fee}\n");

        Console.WriteLine("Item added and saved to catalog.");
        }

    // ==========================================
    // VIEW ALL CATALOG ITEMS
    // ==========================================
    static void ViewItems()    //no Ai note here as i referenced previous code, notes, and videos and followed the structure
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
    static void CheckoutMenu()   //no Ai note here as i referenced previous code, notes, and videos and followed the structure
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
    static void ReturnMenu()  //no Ai note here as i referenced previous code, notes, and videos and followed the structure
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
    static void ViewReceipt()  // Ai Note: For this section i wrote MOST of it out on my own however i put in what i had up to before print the line and asked how i would write out a console.writeline to print out the Id, title, late days, and fees in an evenly displayed format. 
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
            Console.WriteLine($"{item.Id} - {item.Title} | {lateDays} days late | Fee: ${fee:0.00}");  //Asked for evely displayed format

            total += fee; // Add to total
        }

        Console.WriteLine($"TOTAL LATE FEES: ${total:0.00}");  
        Console.WriteLine("================================");  
    }

    // ==========================================
    // SAVE CHECKOUT LIST TO FILE
    // ==========================================
    static void SaveCheckoutList()  //Through youtube tutorials I found Streamwriter (as well as a lot of things within System.io), it was explained as a way to write in and handle files in C# properly // AI note: i wanted to use streamwrite cause it seemed cool
        // So i asked AI to walk me thtough how to use a simple version of streamwriter to save a checkout list to myCheckouts.txt file
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
    static void LoadCheckoutList()  //no Ai note here as i referenced previous code, notes, and videos and followed the structure
    {
        if (!File.Exists("myCheckouts.txt"))
        {
            Console.WriteLine("No saved checkout file found.");
            return;
        }

        Checkout.Items.Clear(); // Remove current checkout items

        // Read each line to rebuild the list
        string[] lines = File.ReadAllLines("myCheckouts.txt");  //no Ai note here as i referenced previous code, notes, and videos and followed the structure

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
