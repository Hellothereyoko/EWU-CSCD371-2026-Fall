namespace Contacts;

public static class Program
{
    public static void Main()
    {
        // TODO : Implement Persistence
        ContactBook contactBook = new();
        
        while (true)
        {
            Console.WriteLine("********** Main Menu **********");
            Console.WriteLine("1. Add a new contact.");
            Console.WriteLine("2. List all contacts.");
            Console.WriteLine("3. Remove a contact.");
            Console.WriteLine("4. Edit a contact.");
            Console.WriteLine("5. Exit the application.");
            
            Console.Write("Please select an option (1-4): ");
            string? input = Console.ReadLine();

            switch (input?.Trim())
            {
                case "1":
                    ShowAddContactMenu();
                    break;
                
                case "2":
                    DisplayContacts();
                    break;
                
                case "3":
                    Console.WriteLine("Feature Not Implemented.");
                    PromptRemoveContact();
                    break;
                
                case "4":
                    // TODO : Edit Contact Options
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
                        // TODO : Prompt for Creation Input
                        contactBook.Add(new Employee(new FullName("John", "Smith"), "email"));
                        break;
                
                    case "2":
                        // TODO: Prompt for Creation Input
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

        void PromptRemoveContact()
        {
            IContact[] contacts = DisplayContacts();

            if (contacts.Length == 0)
                return; // NOTE: display handles showing user no contacts exists
            
            Console.Write("Select Contact number to remove: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int selection) || selection < 1 || selection > contacts.Length)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }
            
            contactBook.Remove(contacts[selection - 1]);
            // TODO : Can we verify the selection was actual removed?
            Console.WriteLine("Contact Selected was Removed.");
            Console.WriteLine();
        }

        IContact[] DisplayContacts()
        {
            IContact[] contacts = contactBook.GetAll();
            
            Console.WriteLine();
            Console.WriteLine("********** Contact List **********");

            if (contacts.Length == 0)
            {
                Console.WriteLine("No contacts found.");
                Console.WriteLine("********** End Contact List **********");
                Console.WriteLine();
                return [];
            }

            for (int i = 0; i < contacts.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {ContactDisplayText(contacts[i])}");
            }
            
            Console.WriteLine("********** End Contact List **********");
            Console.WriteLine();
            
            return contacts;
            // you happy JetBrains? I made an actual return line here lmao

            string ContactDisplayText(IContact contact)
            {
                return
                    $"{contact.GetType().Name}: " +
                    $"{FullName(contact.Name)}: " +
                    $"({contact.Email})";
            }

            string FullName(FullName name)
            {
                return string.IsNullOrWhiteSpace(name.MiddleName)
                    ? $"{name.FirstName} {name.LastName}"
                    : $"{name.FirstName} {name.MiddleName} {name.LastName}";
            }
        }
    }
}
