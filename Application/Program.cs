using Contacts;

namespace Application;

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
                    if (HasContacts(true))
                        DisplayContacts();
                    WaitForUser();
                    break;
                
                case "3":
                    if (HasContacts(true))
                        PromptRemoveContact();
                    break;
                
                case "4":
                    // TODO : Edit Contact Options
                    Console.WriteLine("Feature Not Implemented.");
                    // EditContacts
                    break;
                
                case "5":
                    Console.WriteLine();
                    Console.WriteLine("Goodbye!");
                    return;
                
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
        }
        // "return;"

        bool HasContacts(bool printIfFalse)
        {
            if (contactBook.GetAll().Length > 0)
                return true;

            if (!printIfFalse) return false;
            
            Console.WriteLine();
            Console.WriteLine("No contacts");
            Console.WriteLine();
            return false;
        }

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

        IContact EditContact(IContact contactToEdit)
        {
            string firstName = contactToEdit.Name.FirstName;
            string lastName = contactToEdit.Name.LastName;
            string? middleName = contactToEdit.Name.MiddleName;
            string email = contactToEdit.Email;
            Guid id = contactToEdit.Id;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("********** Edit Contact **********");
                Console.WriteLine($"1. First Name : {firstName}");
                Console.WriteLine($"2. Last Name  : {lastName}");

                Console.WriteLine(
                    $"3. Middle Name: " +
                    $"{middleName ?? "Add Middle Name"}");

                Console.WriteLine($"4. Email      : {email}");
                Console.WriteLine("5. Save Changes");
                
                Console.Write("Select field to edit: ");
                string? input = Console.ReadLine();

                switch (input?.Trim())
                {
                    case "1":
                        //firstName = PromptName("First Name");
                        break;

                    case "2":
                        //lastName = PromptName("Last Name");
                        break;

                    case "3":
                        //middleName = PromptOptionalName("Middle Name");
                        break;

                    case "4":
                        email = PromptEmail();
                        break;

                    case "5":
                        return RebuildContact();

                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
            
            IContact RebuildContact()
            {
                FullName fullName = new(
                    firstName,
                    lastName,
                    middleName
                );

                return contactToEdit switch
                {
                    Student => new Student(fullName, email) { Id = id },
                    Employee => new Employee(fullName, email) { Id = id },
                    _ => throw new InvalidOperationException()
                };
            }
        }

        #region Contact Removal

        void PromptRemoveContact()
        {
            while (true)
            {
                IContact selectedContact = PromptContactSelection();
                
                // TODO : Verify the selection?
                
                contactBook.Remove(selectedContact);
                Console.WriteLine("Contact Selected was Removed.");
                Console.WriteLine();

                // removed last contact must end loop
                if (!HasContacts(false))
                {
                    Console.WriteLine("Contact List is now Empty.");
                    Console.WriteLine();
                    WaitForUser();
                    return;
                }
                
                Console.Write("Remove Another? (y/n): ");
                string? input = Console.ReadLine();
                
                // FIXME ? : ends loop on anything other than 'y' 
                if (!string.Equals(input?.Trim(), "y", StringComparison.OrdinalIgnoreCase))
                    return;
            }
        }

        #endregion

        IContact PromptContactSelection()
        {
            IContact[] contacts = DisplayContacts();

            while (true)
            {
                Console.Write("Select Contact by number: ");
                string? input = Console.ReadLine();
                
                if (int.TryParse(input, out int selection) 
                    && selection >= 1 && selection <= contacts.Length)
                    return contacts[selection - 1];
                
                Console.WriteLine("Invalid selection.");
            }
        }

        // Responsibility : Display contact list to user
        IContact[] DisplayContacts()
        {
            IContact[] contacts = contactBook.GetAll();
            
            Console.WriteLine();
            Console.WriteLine("********** Contact List **********");

            for (int i = 0; i < contacts.Length; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {ContactDisplayText(contacts[i])}");
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

        void WaitForUser()
        {
            Console.Write("Press enter to continue...");
            Console.ReadLine();
            Console.WriteLine();
        }
    }
}
