using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;


namespace CryptographicFailure
{
    class Program
    {
        static void Main(string[] args)
        {
            int SWV = 0;
            String cipher="";
            String encpted = "";
            byte[] encryptedData2 = { };
            byte[] iv2=new byte[16];
            while (true)
            {
                Console.WriteLine("*********************************************");
                Console.WriteLine("Please selecte one of these choeses:");
                Console.WriteLine("1- Caesar Algorithm ");
                Console.WriteLine("2- Caesar cracked ");
                Console.WriteLine("3- AES Algorithm ");
                Console.WriteLine("4- weak AES Algorithm ");
                Console.WriteLine("5- weak Algorithm ");
                Console.WriteLine("6- brute-force on the weak Algorithm ");
                Console.WriteLine("*********************************************");
                SWV =Convert.ToInt32( Console.ReadLine());
                switch (SWV)
                {
                    case 1:
                        {
                            int key = 6;

                            // The plaintext to be encrypted
                            string plaintext ;
                            Console.Write("plz enter the plantext: ");
                            plaintext =Console.ReadLine();

                            // Convert the plaintext to a byte array
                            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

                            // Encrypt the plaintext using the Caesar cipher
                            for (int i = 0; i < plaintextBytes.Length; i++)
                            {
                                // Shift the letter by the key
                                plaintextBytes[i] = (byte)(plaintextBytes[i] + key);
                            }

                            // Convert the encrypted bytes back to a string
                            string ciphertext = Encoding.UTF8.GetString(plaintextBytes);

                            cipher = ciphertext;
                            Console.WriteLine("*******************************");
                            Console.WriteLine(ciphertext);
                            Console.WriteLine("*******************************");
                            // The key is the number of positions to shift the letters

                            break;
                        }
                    case 2:
                        {
                            // Create a dictionary to store the frequency of each letter
                            if (cipher != "")
                            {
                                Dictionary<char, int> letterFrequency = new Dictionary<char, int>();

                                // Count the frequency of each letter in the ciphertext
                                foreach (char c in cipher)
                                {
                                    if (letterFrequency.ContainsKey(c))
                                    {
                                        letterFrequency[c]++;
                                    }
                                    else
                                    {
                                        letterFrequency[c] = 1;
                                    }
                                }
                               // Console.WriteLine("littral=" + letterFrequency.Count);
                                // Sort the dictionary by value in descending order
                                var sortedDictionary = from entry in letterFrequency orderby entry.Value descending select entry;
                                // The most common letter in the English language is 'e', so if the most common letter in the ciphertext is a
                                // letter that appears very frequently, it is likely that this letter corresponds to the letter 'e' in the plaintext.
                                // By analyzing the frequency of letters in the ciphertext, we can determine the key and decrypt the message.   
                                int key = sortedDictionary.First().Key - 'e';
                                int keytester=key;
                                // Decrypt the message using the key and show the posseble words
                                Console.WriteLine("*******************************");
                                for (int j = 0; j < key; j++) {
                                    
                                    byte[] ciphertextBytes = Encoding.UTF8.GetBytes(cipher);
                                    for (int i = 0; i < ciphertextBytes.Length; i++)
                                    {
                                        ciphertextBytes[i] = (byte)(ciphertextBytes[i] - keytester);
                                    }

                                    string decryptedMessage = Encoding.UTF8.GetString(ciphertextBytes);
                                    Console.WriteLine(decryptedMessage); // output: This is a secret message.
                                    keytester =keytester - 1;

                                }
                                Console.WriteLine("*******************************");

                            }
                            else
                            {
                                Console.WriteLine("'<please enter a Ciphertext from the first choese>'");
                            }

                            break;
                        }
                    case 3:
                        {
                            // The key and initialization vector (IV) used for AES encryption
                            /*byte[] key = Encoding.UTF8.GetBytes("mykey1234567890");
                            byte[] iv = Encoding.UTF8.GetBytes("myiv1234567890");*/

                            // Generate a random key and initialization vector (IV)
                            byte[] key = new byte[32];
                            byte[] iv = new byte[16];
                            RandomNumberGenerator.Fill(key);
                            RandomNumberGenerator.Fill(iv); //we can make the keys randmoly to make it stronger.

                            // Encrypt some data using the key and IV
                            string data = "sensitive data";
                            byte[] encryptedData = Encrypt(data, key, iv);
                            //this is the encrypted data
                            Console.WriteLine("***********************************************************");
                            Console.WriteLine("Encrypted data: " + BitConverter.ToString(encryptedData).Replace("-", ""));
                            Console.WriteLine("***********************************************************");
                            // Decrypt the data using the same key and IV
                            string decryptedData = Decrypt(encryptedData, key, iv);

                            // Output the decrypted data
                            Console.WriteLine("*******************************");
                            Console.WriteLine(decryptedData);
                            Console.WriteLine("*******************************");
                            break;
                        }
                    case 4:
                        {
                            // Generate a random key and initialization vector (IV)
                            byte[] key = new byte[32];
                            byte[] iv = new byte[16];
                            RandomNumberGenerator.Fill(key);
                            RandomNumberGenerator.Fill(iv);

                            // Encrypt some data using the key and IV
                            string data = "This is some sensitive data";
                            byte[] encryptedData = Encrypt(data, key, iv);

                            // Decrypt the data using a different key and IV
                            byte[] differentKey = new byte[32];
                            byte[] differentIv = new byte[16];
                            RandomNumberGenerator.Fill(differentKey);
                            RandomNumberGenerator.Fill(differentIv);
                            string decryptedData = Decrypt(encryptedData, differentKey, differentIv);

                            // Output the decrypted data
                            Console.WriteLine("decrbt=" + decryptedData);
                            break;
                        }
                    case 5:
                        {
                            byte[] key = new byte[16];
                            RandomNumberGenerator.Fill(key);

                            // Encrypt a message with the key
                            string message = "This is a secret message";
                            byte[] encryptedMessage = Encrypt(message, key);

                            // Decrypt the message with the key
                            string decryptedMessage = Decrypt(encryptedMessage, key);
                            Console.WriteLine(decryptedMessage);

                            break;
                        }
                    case 6:
                        {
                            byte[] encryptedMessage = GetEncryptedMessage();

                            // Try every possible key until we find one that successfully decrypts the message
                            for (int i = 0; i < int.MaxValue; i++)
                            {
                                // Convert the integer key to a byte array
                                byte[] key = BitConverter.GetBytes(i);

                                // Decrypt the message with the key
                                string decryptedMessage = Decrypt(encryptedMessage, key);

                                // Check if the decrypted message is the original message
                                if (decryptedMessage == "This is a secret message")
                                {
                                    // We found the correct key!
                                    Console.WriteLine("Key found: " + string.Join(", ", key.Select(b => b.ToString())));
                                    break;
                                }
                                Console.WriteLine(decryptedMessage);
                            }
                                break;
                        }
                    case 7:
                        {
                            byte[] key = new byte[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            byte[] iv = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                            iv2 = iv;
                            // Encrypt some data using the weak key
                            string data = "This is some sensitive data";
                            byte[] encryptedData = Encrypt(data, key, iv);


                            encpted = BitConverter.ToString(encryptedData).Replace("-", "");
                            encryptedData2 = encryptedData;
                            // Decrypt the data using the same key
                            string decryptedData = Decrypt(encryptedData, key, iv);

                            // Output the decrypted data
                            Console.WriteLine(decryptedData);

                            break;
                        }
                    case 8:
                        {
                            // brut force attck to get the possible word
                            byte[] encryptedData = GetEncryptedData();
                            byte[] iv = GetIV();

                            // Try all possible keys until the correct one is found
                            for (int i = 0; i < int.MaxValue; i++)
                            {
                                // Generate the current key
                                byte[] key = BitConverter.GetBytes(i);

                                // Try to decrypt the data with the current key
                                string decryptedData = Decrypt(encryptedData, key, iv);

                                // If the decryption is successful, output the key and the decrypted data
                                if (decryptedData != null)
                                {
                                    Console.WriteLine("Found key: " + BitConverter.ToInt32(key, 0));
                                    Console.WriteLine("Decrypted data: " + decryptedData);
                                    break;
                                }
                            }
                            break;
                        }

                }
            }
            
            
            
           
        }
        private static byte[] Encrypt(string data, byte[] key, byte[] iv)
        {
            // Use the Aes algorithm to encrypt the data
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // Encrypt the data and return the encrypted bytes
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    return encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                    
                }
            }
        }

        private static string Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            // Use the Aes algorithm to decrypt the data
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // Decrypt the data and return the decrypted string
                try
                {
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        byte[] decryptedBytes = decryptor.TransformFinalBlock(data, 0, data.Length);
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
                catch (CryptographicException)
                {
                    Console.WriteLine("you have an security error you are trying to do somthing wrong");
                    return null;
                }
            }
        }
     

         private static byte[] GetEncryptedData()
         {
                // Replace this with the actual encrypted data
                return GetEncryptedData();
         }

        static byte[] GetEncryptedMessage()
        {
            // Generate a random key
            byte[] key = new byte[16];
            RandomNumberGenerator.Fill(key);

            // Encrypt a message with the key
            string message = "This is a secret message";
            return Encrypt(message, key);
        }

        private static byte[] GetIV()
        {
            // Replace this with the actual IV
            return new byte[16];
        }


        static byte[] Encrypt(string message, byte[] key)
        {
            // Encrypt the message with the key using a simple XOR operation
            byte[] encryptedMessage = new byte[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                encryptedMessage[i] = (byte)(message[i] ^ key[i % key.Length]);
            }
            return encryptedMessage;
        }

        static string Decrypt(byte[] encryptedMessage, byte[] key)
        {
            // Decrypt the message with the key using a simple XOR operation
            char[] decryptedMessage = new char[encryptedMessage.Length];
            for (int i = 0; i < encryptedMessage.Length; i++)
            {
                decryptedMessage[i] = (char)(encryptedMessage[i] ^ key[i % key.Length]);
            }
            return new string(decryptedMessage);
        }
    }
}
