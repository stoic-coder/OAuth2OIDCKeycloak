// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="StringHelper.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Security.Cryptography;
using System.Text;

namespace KC.Security.Helpers;


/// <summary>
/// Class StringHelper.
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// Encrypts the specified encrypt string.
    /// </summary>
    /// <param name="encryptString">The encrypt string.</param>
    /// <returns>System.String.</returns>
    public static string Encrypt(string encryptString)   
    {  
        const string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";  
        var clearBytes = Encoding.Unicode.GetBytes(encryptString);
        using var encryptor = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {  
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76  
        });  
        encryptor.Key = pdb.GetBytes(32);  
        encryptor.IV = pdb.GetBytes(16);
        using var ms = new MemoryStream();
        using(var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)) {  
            cs.Write(clearBytes, 0, clearBytes.Length);  
            cs.Close();  
        }  
        encryptString = Convert.ToBase64String(ms.ToArray());

        return encryptString;  
    }

    /// <summary>
    /// Decrypts the specified cipher text.
    /// </summary>
    /// <param name="cipherText">The cipher text.</param>
    /// <returns>System.String.</returns>
    public static string Decrypt(string cipherText)   
    {  
        var EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";  
        cipherText = cipherText.Replace(" ", "+");  
        var cipherBytes = Convert.FromBase64String(cipherText);
        using var encryptor = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {  
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76  
        });  
        encryptor.Key = pdb.GetBytes(32);  
        encryptor.IV = pdb.GetBytes(16);
        using var ms = new MemoryStream();
        using(var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)) {  
            cs.Write(cipherBytes, 0, cipherBytes.Length);  
            cs.Close();  
        }  
        cipherText = Encoding.Unicode.GetString(ms.ToArray());

        return cipherText;  
    }  
}