using System;

namespace BiblioUpTik.Mobi.Metadata
{
  public static class Converter
  {
    public static short ToInt16(byte[] bytes)
    {
      return BitConverter.ToInt16(Converter.CheckBytes(bytes), 0);
    }

    public static int ToInt32(byte[] bytes)
    {
      return BitConverter.ToInt32(Converter.CheckBytes(bytes), 0);
    }

    public static long ToInt64(byte[] bytes)
    {
      return BitConverter.ToInt64(Converter.CheckBytes(bytes), 0);
    }

    public static ushort ToUInt16(byte[] bytes)
    {
      return BitConverter.ToUInt16(Converter.CheckBytes(bytes), 0);
    }

    public static uint ToUInt32(byte[] bytes)
    {
      return BitConverter.ToUInt32(Converter.CheckBytes(bytes), 0);
    }

    public static ulong ToUInt64(byte[] bytes)
    {
      return BitConverter.ToUInt64(Converter.CheckBytes(bytes), 0);
    }

    private static byte[] CheckBytes(byte[] bytesToCheck)
    {
      byte[] numArray = (byte[]) bytesToCheck.Clone();
      if (BitConverter.IsLittleEndian)
        Array.Reverse((Array) numArray);
      return numArray;
    }
  }
}
