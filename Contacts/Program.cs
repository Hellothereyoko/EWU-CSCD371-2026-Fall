namespace Contacts;


/// <summary>
/// This is the main entry point for the Contacts application.
/// </summary>
public static class Program 
{
    public static void Main()
    {
        // Define vars inherited from IContact interface for Student and Employee classes.
            // def 1st name
            // def middle name (opt)
            // def last name
            // def record type (Student or Employee)
        bool menuLoop = true;


        // Pgrm Start
        System.Console.WriteLine("Contacts Application Started!"); 
    

        // Create a new instance of the ContactBook class.
        ContactBook contactBook = new ContactBook();


        while (menuLoop){
        // Main Menu Interface (switch case)
            // 1. Add a new contact (Student or Employee)
            // 2. List all contacts
            // 3. Remove a contact
            // 4. Exit the application
        System.Console.WriteLine("**********Main Menu:**********");
        System.Console.WriteLine("1. Add a new contact (Student or Employee)");
        System.Console.WriteLine("2. List all contacts");
        System.Console.WriteLine("3. Remove a contact");
        System.Console.WriteLine("4. Exit the application");


        System.Console.Write("Please select an option (1-4): ");
        string? input = System.Console.ReadLine();

        if (input is null || input.Trim() == "4")
        {
            menuLoop = false;
        }

        }


        // Add Functionality
            // Prompt the user to enter the contact details (first name, middle name, last name, record type).
            // Create a new instance of the appropriate class (Student or Employee) based on the record type.
            // Add the new contact to the ContactBook using the Add method.(Ensure no dupes)


        // List Functionality
            // Retrieve all contacts from the ContactBook using the GetAll method.
            // Display the list of contacts to the user.


        // Remove Functionality
            // Prompt the user to enter the contact details of the contact they wish to remove.
            // Create a temporary instance of the appropriate class (Student or Employee) based on the provided details.
            // Remove the contact from the ContactBook using the Remove method. (Ensure contact exists)

    }
}



/// Notes:
/// --------------------------------------------------
/// 
/// - Let the user add students and employees, list contacts, and remove a contact. ❌✔
/// - Adding a duplicate contact is reported to the user, not silently ignored. ❌✔
/// - Bad input must not crash the program. ❌✔
/// - Keep your code testable. ❌✔
/// 
/// 
/// 
/// 
/// Build a simple command-line contacts list that stores both **student** and **employee** records. This assignment is about **records and equality**: two contacts with the same data must be treated as the same contact.