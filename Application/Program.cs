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
            
            Console.Write("Please select an option (1-5): ");
            string? input = Console.ReadLine();

            switch (input?.Trim())
            {
                case "1":
                    PromptCreateContact();
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

        // Responsibility : Selection of type of contact to create
        void PromptCreateContact()
        {
            Console.WriteLine();
            Console.WriteLine("********** Add Contact **********");
            while (true)
            {
                Console.WriteLine("1. Add a new Employee.");
                Console.WriteLine("2. Add a new Student.");
                Console.WriteLine();

                Console.Write("Please select a contact type to add (1-2): ");
                string? input = Console.ReadLine();
                
                switch (input?.Trim())
                {
                    case "1":
                        CreateEmployee();
                        return;
                
                    case "2":
                        CreateStudent();
                        return;
                
                    default:
                        Console.WriteLine("Invalid input.");
                        Console.WriteLine();
                        break;
                }
            }
        }

        void CreateEmployee()
        {
            Employee employeeToAdd = new(
                PromptFullName(), 
                PromptEmail()
            );
            
            // While loop for editing?
            if (contactBook.Contains(employeeToAdd))
            {
                Console.WriteLine("Contact already exists.");
                // TODO : EditContact(IContact) here
                return;
            }
            contactBook.Add(employeeToAdd);
        }
        
        void CreateStudent()
        {
            Student studentToAdd = new(
                PromptFullName(), 
                PromptEmail()
            );

            // While loop for editing?
            if (contactBook.Contains(studentToAdd))
            {
                Console.WriteLine("Contact already exists.");
                // TODO : EditContact(IContact) here
                return;
            }
            contactBook.Add(studentToAdd);
        }

        // Responsibility : Get a FullName from user
        FullName PromptFullName()
        {
            string firstName;
            string lastName;
            string? middleName;

            while (true)
            {
                Console.WriteLine("Please Provide First Name: ");
                string? input = Console.ReadLine();

                if (ContactUtilities.ValidateName(input, out string error)
                    && input is not null)
                {
                    firstName = input;
                    break;
                }
                Console.WriteLine(error);
            }

            while (true)
            {
                Console.WriteLine("Please Provide Last Name: ");
                string? input = Console.ReadLine();

                if (ContactUtilities.ValidateName(input, out string error)
                    && input is not null)
                {
                    lastName = input;
                    break;
                }
                Console.WriteLine(error);
            }

            while (true)
            {
                Console.WriteLine("Please Provide Middle Name (Optional): ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    middleName = null;
                    break;
                }

                if (ContactUtilities.ValidateName(input, out string error))
                {
                    middleName = input;
                    break;
                }
                Console.WriteLine(error);
            }
            
            return new FullName(firstName, lastName, middleName);
        }

        // Responsibility : Get a single email input from user
        string PromptEmail()
        {
            while (true)
            {
                Console.WriteLine("Please Provide Email: ");
                string? input = Console.ReadLine();

                if (ContactUtilities.ValidateEmail(input, out string error)
                    && input is not null)
                {
                    return input;
                }
                Console.WriteLine(error);
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
