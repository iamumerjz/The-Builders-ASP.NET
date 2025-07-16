using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryModel
{

    public class PasswordHashing
        {
            public static string HashPassword(string password)
            {
                // Generate a random salt
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                // Hash the password with the salt using bcrypt
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);

                // Combine the salt and hash
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                // Convert to Base64
                string hashedPassword = Convert.ToBase64String(hashBytes);

                return hashedPassword;
            }

            public static bool VerifyPassword(string password, string hashedPassword)
            {
                // Convert the hashed password from Base64
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                // Get the salt from the hashBytes
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);

                // Compute the hash of the entered password with the retrieved salt
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);

                // Compare the computed hash with the stored hash
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                        return false;
                }
                return true;
            }
        }

}
