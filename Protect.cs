using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace BiblioUpTik
{
  public static class Protect
  {
    private static readonly byte[] entropy = Encoding.Unicode.GetBytes("rurityishbestponie");

    public static string EncryptString(SecureString input)
    {
      return Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(Protect.ToInsecureString(input)), Protect.entropy, DataProtectionScope.CurrentUser));
    }

    public static SecureString DecryptString(string encryptedData)
    {
      try
      {
        return Protect.ToSecureString(Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), Protect.entropy, DataProtectionScope.CurrentUser)));
      }
      catch
      {
        return new SecureString();
      }
    }

    public static SecureString ToSecureString(string input)
    {
      SecureString secureString = new SecureString();
      foreach (char c in input)
        secureString.AppendChar(c);
      secureString.MakeReadOnly();
      return secureString;
    }

    public static string ToInsecureString(SecureString input)
    {
      IntPtr bstr = Marshal.SecureStringToBSTR(input);
      string stringBstr;
      try
      {
        stringBstr = Marshal.PtrToStringBSTR(bstr);
      }
      finally
      {
        Marshal.ZeroFreeBSTR(bstr);
      }
      return stringBstr;
    }
  }
}
