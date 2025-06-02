using System;
using System.Text;
using System.Security.Cryptography;
// Implementation of encryption not completed in this version, code remains for potential future use 
public class EncryptionManager {

	//AES - 256 bits key, Electronic Codebook (ECB) mode, padding mode PKCS #7
	public static string Encrypt(string input)
	{
		//TODO change this once decrypt functional
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes ("12345678901234567890123456789012");
		byte[] inputArray = UTF8Encoding.UTF8.GetBytes (input);
		RijndaelManaged rijndael = new RijndaelManaged ();
		rijndael.Key = keyArray;
		rijndael.Mode = CipherMode.ECB;
		rijndael.Padding = PaddingMode.PKCS7;
		ICryptoTransform cryptoTransform = rijndael.CreateEncryptor ();
		byte[] outputArray = cryptoTransform.TransformFinalBlock (inputArray, 0, inputArray.Length);
		return Convert.ToBase64String (outputArray, 0, outputArray.Length);
	}

}
