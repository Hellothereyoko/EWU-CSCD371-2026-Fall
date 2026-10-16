namespace Contacts;

public static class Program
{
    public static void Main()
    {
        ContactBook contactBook = new();
        
        while (true)
        {
            Console.WriteLine("**********Main Menu:**********");
            Console.WriteLine("1. Add a new contact.");
            Console.WriteLine("2. List all contacts.");
            Console.WriteLine("3. Remove a contact.");
            Console.WriteLine("4. Remove a contact.");
            Console.WriteLine("5. Exit the application.");
            
            Console.Write("Please select an option (1-4): ");
            string? input = Console.ReadLine();

            switch (input?.Trim())
            {
                case "1":
                    ShowAddContactMenu();
                    break;
                
                case "2":
                    Console.WriteLine("Feature Not Implemented.");
                    // ListContacts
                    break;
                
                case "3":
                    Console.WriteLine("Feature Not Implemented.");
                    // RemoveContactsMenu
                    break;
                
                case "4":
                    Console.WriteLine("Feature Not Implemented.");
                    // EditContacts
                    break;
                
                case "5":
                    Console.WriteLine("Goodbye!");
                    return;
                
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
        }
        // "return;"

        void ShowAddContactMenu()
        {
            while (true)
            {
                Console.WriteLine("********** Add Contact Menu **********");
                Console.WriteLine("1. Add a new Employee.");
                Console.WriteLine("2. Add a new Student.");
                Console.WriteLine("3. Return to Main Menu.");

                Console.Write("Please select an option (1-2): ");
                string? input = Console.ReadLine();
                
                switch (input?.Trim())
                {
                    case "1":
                        contactBook.Add(new Employee(new FullName("John", "Smith"), "email"));
                        break;
                
                    case "2":
                        contactBook.Add(new Student(new FullName("John", "Smith"), "email"));
                        break;
                
                    case "3":
                        return;
                
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
        }
    }
}
