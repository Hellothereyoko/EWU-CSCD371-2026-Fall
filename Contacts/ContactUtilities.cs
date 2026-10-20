namespace Contacts;

public static class ContactUtilities
{
    /// <summary>
    /// Utility method used for validating a name entry for a contact.
    /// Returns true if string meets standard for proper name, else
    /// errorMessage will contain what is wrong
    /// </summary>
    /// <param name="name"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static bool ValidateName(string? name, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            errorMessage = "Name cannot be empty";
            return false;
        }

        for (int i = 0; i < name.Length; i++)
        {
            char current = name[i];

            // letters are always allowed
            if (char.IsLetter(current))
                continue;

            // allow ' and - characters if next to letters on both sides
            if (current is '\'' or '-')
            {
                bool hasLeftLetter = i > 0 && char.IsLetter(name[i - 1]);
                
                bool hasRightLetter = i < name.Length - 1 && char.IsLetter(name[i + 1]);
                
                if (hasLeftLetter && hasRightLetter)
                    continue;
                
                // immediately fail and give reasoning
                errorMessage = $"The Character: ({current}) must have a letter on both sides.";
                return false;
            }
            
            errorMessage = "Names may only contain letters. Apostrophes (') and hyphens (-) are allowed between letters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Utility method for validating a string meets email classifications.
    /// Return true if the passed string is a valid email, else
    /// errorMessage contains what is determined invalid
    /// </summary>
    /// <param name="email"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static bool ValidateEmail(string? email, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            errorMessage = "Email cannot be empty";
            return false;
        }
        
        int atCount = email.Count(c => c == '@');
        if (atCount != 1)
        {
            errorMessage = "Email must contain exactly one '@' character.";
            return false;
        }
        
        string[] parts = email.Split('@');
        
        string localPart = parts[0];
        string domainFullPart = parts[1];

        if (string.IsNullOrWhiteSpace(localPart))
        {
            errorMessage = "Email address must contain at least one character prior to '@'.";
            return false;
        }
        
        int dotIndex = domainFullPart.LastIndexOf('.');

        // no dot or no characters precede dot or not more than 2 character post dot
        if (dotIndex < 1 || dotIndex > domainFullPart.Length - 3)
        {
            errorMessage = "Email Domain must follow domain.extension format, with at least 1 character prior to '.' and 2 post.";
            return false;
        }
        
        string extension = domainFullPart[(dotIndex + 1)..];

        if (!extension.All(char.IsLetter))
        {
            errorMessage = "Email domain extension may only contain letters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}

// Validation API Rules

// Name Rules:
// 1. Only contain letters, apostrophe, and hyphen
// 2. If containing apostrophe or hyphen, they must be surrounded with letters

// Email Rules:
// 1. Not null or white space
// 2. Exactly one '@'
// 3. At least one character prior to @ as local portion
// 4. domain full portion must contain a domain prior and domain extension
// 5. domain prior to contain at least one character
// 6. domain extension to contain at least two letters (and only letters)
// Note some weird emails can still be valid, but let's be honest, it's more work than needed here.