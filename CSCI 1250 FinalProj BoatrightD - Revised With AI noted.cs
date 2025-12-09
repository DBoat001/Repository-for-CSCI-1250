
//Ai (chat gpt mostly) was used to help me structure some sections (mostly spacing because I kept getting spacing errors where things were not on the right lines or tabbeed out properly), I understood the basics of what I wanted 
//things to say and do as well as the functions I wanted but actually typing them out with proper symbols and things proved to be a struggle for me. 
// important Ai Note: I did throw chunks of my own code into ai at times when I ran into spacing problems and asked "(*Copy/Pasted error *) Can you space this properly for me, and then explain what the issue was?". 
// Spacing has consistently been an issue for me throughout this course and has yet to fully click into my brain still (among other things T-T )
// Notes shown throughout code.

using System;                   // Gives access to Console and basic system functions
using System.Collections.Generic; // Allows use of List
using System.iO;                // Allows reading and writing files

// ==========================================
// CLASS: Libraryitem
// Represents one item in the library catalog
// ==========================================  //no Ai note for most of this as I referenced previous code and notes and followed the structure I'd already seen/used
class Libraryitem
{
    public int id { get; set; }              // Unique iD for the item          // Found get; Set; on youtube tutorials as a way to shorten Get id/Return id
    public string Title { get; set; }        // item title
    public string Type { get; set; }         // Book / DVD / etc.
    public double DailyLateFee { get; set; } // Late fee charged per day

    public Libraryitem(int id, string title, string type, double dailyLateFee)
    {
        id = id;                    // Store the iD   
        Title = title;              // Store the title
        Type = type;                // Store the type
        DailyLateFee = dailyLateFee;// Store the daily fee
        }

    public override string ToString()  // Found Override in Youtube tutorial to easily return my version of ToString instead of the default to make it more display friendly value
    {
        // Returns a friendly display version of the item
        return $"{id} - {Title} ({Type}) | Late Fee: ${DailyLateFee}/day";
           }
}

// ==========================================
// CLASS: CheckOutitem
// Holds a list of items the student has checked out
// ==========================================
class CheckOutitem
{
    public List<Libraryitem> items { get; set; } // All checked-out items   //Ai prompt used: "im creating a class called CheckOutitem, I want a public list that holds Libraryitem, and the ability to read and change the value of items from outside the class,
                                                                                                                                                                                                                             // using get set, how would i do that in a single line?"

    public CheckOutitem()
    {
        items = new List<Libraryitem>();  // Start with empty checkout list
        }

    public void Additem(Libraryitem item)  // Void was used as I didnt want anything to return
    {
        items.Add(item);  // Add a Libraryitem to the checkout list
        }

    public bool Removeitem(int id)  // bool was used to T or F if the item was removed correctly
    {
        // Find the item with matching iD    // Ai prompt: "I want to be able to search for an item in the items list where the iD mathces the iD given, how would I structure that and why?"
        Libraryitem found = items.Find(I => I.id == id);

        if (found != null)
        {
            items.Remove(found);  // Remove if found
            return true;          // Tell program the remove worked
        }
        return false;             // Removal failed (not found)
        }  
}

//Ai Note: For creating Program, I used Ai to help me structure better as I kept encountering errors, for some reason I still cannot quite figure out, for program using static was not giving me an error later on while public was,
//Through some research I think it had something to do with being able to access static item without needing to create an object and I think I was goofing up somewhere that was making it not able to reference back but I couldnt figure out where so I changed to static 
// which worked so I continued with it in place of public.

class Program  
{
    static List<Libraryitem> Catalog = new List<Libraryitem>();  // Holds all library items // Catalog is used to hold the list of Library items
    static CheckOutitem Checkout = new CheckOutitem();           // Holds items the user checked out

    static void Main()
    {
        LoadCatalog();  // Load items from catalog.txt when program starts

        bool running = true;  // Controls menu loop

        while (running)
        {
            // Menu options shown to the user   // AI Note: I wrote out all of the console menu lists, but asked Ai to help me with the title line to try and get it to stay even. 
            Console.WriteLine("\n===== LiBRARY CHECKOUT SYSTEM =====");   // "I want a console.write line command that will display "Library Checkout System" as if on a reciept but keep it evenly in the center."
            Console.WriteLine("1. Add a Library item");
            Console.WriteLine("2. View Catalog items");
            Console.WriteLine("3. Check Out an item");
            Console.WriteLine("4. Return an item");
            Console.WriteLine("5. View Checkout Receipt");
            Console.WriteLine("6. Save Checkout List");
            Console.WriteLine("7. Load Checkout List");
            Console.WriteLine("8. Exit");
            Console.WriteLine("Choose an option: ");

            string choice = Console.ReadLine(); // Read user input

            // Decide what menu option the user picked  // I found the switch statement on youtube as well, its an alternative to else-if statments that helped drop the number of code lines as well as being a little easier for my brain to understand so I opted for it.
            // Ai Note: I put the below cases 1-8 in ai to ask it how to space it as I was getting an error and found I was simply using break wrong. I was trying to put it a line below each option instead of at the end 
            switch (choice)
            {
                case "1": AddLibraryitem(); break;   // Add new item
                case "2": Viewitems(); break;        // Show catalog
                case "3": CheckoutMenu(); break;     // Check out an item
                case "4": ReturnMenu(); break;       // Return an item
                case "5": ViewReceipt(); break;      // Show late fees receipt
                case "6": SaveCheckoutList(); break; // Save checkout list to file
                case "7": LoadCheckoutList(); break; // Load saved checkout list
                case "8": running = false; break;    // Exit loop (and program)
                default:
                    Console.WriteLine("invalid option. Try again.");
                    break;
            }
        }

        Console.WriteLine("Goodbye!"); // Program ends
        }

    // ==========================================
    // LOAD CATALOG FROM FiLE
    // ==========================================
    static void LoadCatalog()  //Method to load catalog  // Again sticking with static because I didn't want to run into the same issues as before and static looks like a much more fun word than public :)
    {
        if (!File.Exists("catalog.txt"))  // Ai note: "I want a way to check if a file named catalog.txt exsists in my C# code what line of code wsould do that?"
        {
            // if catalog file doesn't exist, program still works with empty catalog
            Console.WriteLine("catalog.txt not found, starting with empty catalog.");
            return;
            }

        // Read all lines from the file
        string[] lines = File.ReadAllLines("catalog.txt");

        // Process each line into a Libraryitem   // This is another one I know I had trouble structuring. I was able to get the string lines figured out.  //Ai Note: for int and double I explained what I was trying to do for both and asked what line of code would help do that and why.
        //Ex. what single line of code would help me convert a text id to a number.  Ex2. What single line of code would help me create a late fee. (it originally returned code with a float but I asked for a double because through research a double is usually more accurate)
        foreach (string line in lines)
        {
            string[] p = line.Split(','); // Separate each part by comma

            int id = int.Parse(p[0]);     // Convert iD text to number
            string title = p[1];          // Title
            string type = p[2];           // Type
            double fee = double.Parse(p[3]); // Late fee  

            // Add new Libraryitem object to catalog list
            Catalog.Add(new Libraryitem(id, title, type, fee));
            }
    }

    // ==========================================
    // ADD NEW iTEM TO CATALOG
    // ==========================================
    static void AddLibraryitem()
    {
        // Ask user for item info   //no Ai note for most of this as I referenced previous code and notes and followed the structure I'd already seen/used
        Console.Write("Enter new item iD: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Enter title: ");
        string title = Console.ReadLine();

        Console.Write("Enter type (Book/DVD): ");
        string type = Console.ReadLine();

        Console.Write("Enter daily late fee: ");
        double fee = double.Parse(Console.ReadLine());

        // Create the item object
        Libraryitem item = new Libraryitem(id, title, type, fee);

        // Add to catalog list
        Catalog.Add(item);

        // Also save permanently to file  // Ai Note: I asked ai if there was a better way to append multiple things instead of going line by line, then researched a little more specifically to see if what I got was true, which led to this line
        File.AppendAllText("catalog.txt", $"{id},{title},{type},{fee}\n");

        Console.WriteLine("item added and saved to catalog.");
        }

    // ==========================================
    // ViEW ALL CATALOG iTEMS
    // ==========================================
    static void Viewitems()    //no Ai note here as I referenced previous code, notes, and videos and followed the structure
    {
        Console.WriteLine("\n===== CATALOG iTEMS =====");

        // Loop through all items and print them
        foreach (var item in Catalog)
        {
            Console.WriteLine(item.ToString());
            }
    }
    // ==========================================
    // CHECK OUT AN iTEM
    // ==========================================
    static void CheckoutMenu()   //no Ai note here as I referenced previous code, notes, and videos and followed the structure
    {
        Console.Write("Enter iD to check out: ");
        int id = int.Parse(Console.ReadLine());

        // Look for item with matching iD
        Libraryitem found = Catalog.Find(I => I.id == id);

        if (found == null)
        {
            Console.WriteLine("item not found.");
            return;
            }

        // Prevent double-checkout of the same item
        if (Checkout.items.Exists(I => I.id == id))
        {
            Console.WriteLine("You already checked out this item.");
            return;
            }

        // Add item to checkout list
        Checkout.Additem(found);

        Console.WriteLine("item checked out!");
    }

    // ==========================================
    // RETURN AN iTEM
    // ==========================================
    static void ReturnMenu()  //no Ai note here as I referenced previous code, notes, and videos and followed the structure
    {
        Console.Write("Enter iD to return: ");
        int id = int.Parse(Console.ReadLine());

        // Attempt to remove
        if (Checkout.Removeitem(id))
            Console.WriteLine("item returned.");
        else
            Console.WriteLine("item not found in your checkout list.");
    }

    // ==========================================
    // ViEW RECEiPT WiTH LATE FEES
    // ==========================================
    static void ViewReceipt()  // Ai Note: For this section I wrote MOST of it out on my own however I put in what I had up to before print the line and asked how I would write out a console.writeline to print out the id, title, late days, and fees in an evenly displayed format. 
    {
        Console.WriteLine("\n===== CHECKOUT RECEiPT =====");

        if (Checkout.items.Count == 0)
        {
            // No items to show
            Console.WriteLine("You have no items checked out.");
            return;
            }

        Console.Write("Enter days late (same for all items): ");
        int lateDays = int.Parse(Console.ReadLine());

        double total = 0; // total late fees

        // Loop through each checked-out item
        foreach (var item in Checkout.items)
        {
            // Calculate fee = days late * daily rate
            double fee = lateDays > 0 ? lateDays * item.DailyLateFee : 0;

            // Print the line
            Console.WriteLine($"{item.id} - {item.Title} | {lateDays} days late | Fee: ${fee:0.00}");  //Asked for evely displayed format

            total += fee; // Add to total
            }

        Console.WriteLine($"TOTAL LATE FEES: ${total:0.00}");  
        Console.WriteLine("================================");  
    }

    // ==========================================
    // SAVE CHECKOUT LiST TO FiLE
    // ==========================================
    static void SaveCheckoutList()  //Through youtube tutorials I found Streamwriter (as well as a lot of things within System.io), it was explained as a way to write in and handle files in C# properly // Ai note: I wanted to use streamwrite cause it seemed cool
        // So I asked Ai to walk me thtough how to use a simple version of streamwriter to save a checkout list to myCheckouts.txt file
    {
        // Open or create the file
        using (StreamWriter writer = new StreamWriter("myCheckouts.txt"))
        {
            // Write each checked-out item to file
            foreach (var item in Checkout.items)
            {
                writer.WriteLine($"{item.id},{item.Title},{item.Type},{item.DailyLateFee}");
            }
        }

        Console.WriteLine("Checkout list saved.");
    }

    // ==========================================
    // LOAD CHECKOUT LiST FROM FiLE
    // ==========================================
    static void LoadCheckoutList()  //no Ai note here as I referenced previous code, notes, and videos and followed the structure
    {
        if (!File.Exists("myCheckouts.txt"))
        {
            Console.WriteLine("No saved checkout file found.");
            return;
        }

        Checkout.items.Clear(); // Remove current checkout items

        // Read each line to rebuild the list
        string[] lines = File.ReadAllLines("myCheckouts.txt");  //no Ai note here as I referenced previous code, notes, and videos and followed the structure

        foreach (string line in lines)
        {
            string[] p = line.Split(',');

            int id = int.Parse(p[0]);     // iD
            string title = p[1];          // Title
            string type = p[2];           // Type
            double fee = double.Parse(p[3]); // Daily fee

            // Add item back to checkout list
            Checkout.Additem(new Libraryitem(id, title, type, fee));
        }

        Console.WriteLine("Checkout list loaded.");
    }
}
