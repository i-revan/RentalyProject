using System.Text.RegularExpressions;

namespace RentalyProject.Utilities.Extensions
{
    public static class RegisterExtension
    {
        public static string Capitalize(this string identity)
        {
            identity = identity.Substring(0, 1).ToUpper() + identity.Substring(1).ToLower();
            return identity;
        }
        public static bool CheckMail(this string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (regex.IsMatch(email))
            {
                return true;
            }
            return false;
        }
        public static bool HasDigit(this string identity)
        {
            foreach (var letter in identity)
            {
                if (Char.IsDigit(letter))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
