using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Text;

namespace BiblioUpTik
{
  public class PDFParser
  {
    private static int _numberOfCharsToKeep = 15;

    public bool ExtractText(string inFileName, string outFileName)
    {
      StreamWriter streamWriter = (StreamWriter) null;
      try
      {
        PdfReader pdfReader = new PdfReader(inFileName);
        streamWriter = new StreamWriter(outFileName, false, Encoding.UTF8);
        Console.Write("Processing: ");
        int num1 = 68;
        float num2 = (float) num1 / (float) pdfReader.NumberOfPages;
        int num3 = 0;
        float num4 = 0.0f;
        for (int pageNum = 1; pageNum <= pdfReader.NumberOfPages; ++pageNum)
        {
          streamWriter.Write(PDFParser.ExtractTextFromPDFBytes(pdfReader.GetPageContent(pageNum)) + " ");
          if ((double) num2 >= 1.0)
          {
            for (int index = 0; index < (int) num2; ++index)
            {
              Console.Write("#");
              ++num3;
            }
          }
          else
          {
            num4 += num2;
            if ((double) num4 >= 1.0)
            {
              for (int index = 0; index < (int) num4; ++index)
              {
                Console.Write("#");
                ++num3;
              }
              num4 = 0.0f;
            }
          }
        }
        if (num3 < num1)
        {
          for (int index = 0; index < num1 - num3; ++index)
            Console.Write("#");
        }
        return true;
      }
      catch
      {
        return false;
      }
      finally
      {
        streamWriter?.Close();
      }
    }

    public static string ExtractTextFromPDFBytes(byte[] input)
    {
      if (input == null || input.Length == 0)
        return "";
      try
      {
        string str = "";
        bool flag1 = false;
        bool flag2 = false;
        int num1 = 0;
        char[] recent = new char[PDFParser._numberOfCharsToKeep];
        for (int index = 0; index < PDFParser._numberOfCharsToKeep; ++index)
          recent[index] = ' ';
        for (int index1 = 0; index1 < input.Length; ++index1)
        {
          char ch = (char) input[index1];
          if (flag1)
          {
            if (num1 == 0)
            {
              if (PDFParser.CheckToken(new string[2]
              {
                "TD",
                "Td"
              }, recent))
                str += "\n\r";
              else if (PDFParser.CheckToken(new string[3]
              {
                "'",
                "T*",
                "\""
              }, recent))
                str += "\n";
              else if (PDFParser.CheckToken(new string[1]
              {
                "Tj"
              }, recent))
                str += " ";
            }
            int num2;
            if (num1 == 0)
              num2 = !PDFParser.CheckToken(new string[1]
              {
                "ET"
              }, recent) ? 1 : 0;
            else
              num2 = 1;
            if (num2 == 0)
            {
              flag1 = false;
              str += " ";
            }
            else if (ch == '(' && num1 == 0 && !flag2)
              num1 = 1;
            else if (ch == ')' && num1 == 1 && !flag2)
              num1 = 0;
            else if (num1 == 1)
            {
              if (ch == '\\' && !flag2)
              {
                flag2 = true;
              }
              else
              {
                if (ch >= ' ' && ch <= '~' || ch >= '\x0080' && ch < 'ÿ')
                  str += ch.ToString();
                flag2 = false;
              }
            }
          }
          for (int index2 = 0; index2 < PDFParser._numberOfCharsToKeep - 1; ++index2)
            recent[index2] = recent[index2 + 1];
          recent[PDFParser._numberOfCharsToKeep - 1] = ch;
          int num3;
          if (!flag1)
            num3 = !PDFParser.CheckToken(new string[1]
            {
              "BT"
            }, recent) ? 1 : 0;
          else
            num3 = 1;
          if (num3 == 0)
            flag1 = true;
        }
        return str;
      }
      catch
      {
        return "";
      }
    }

    private static bool CheckToken(string[] tokens, char[] recent)
    {
      foreach (string token in tokens)
      {
        if ((int) recent[PDFParser._numberOfCharsToKeep - 3] == (int) token[0] && (int) recent[PDFParser._numberOfCharsToKeep - 2] == (int) token[1] && (recent[PDFParser._numberOfCharsToKeep - 1] == ' ' || recent[PDFParser._numberOfCharsToKeep - 1] == '\r' || recent[PDFParser._numberOfCharsToKeep - 1] == '\n') && (recent[PDFParser._numberOfCharsToKeep - 4] == ' ' || recent[PDFParser._numberOfCharsToKeep - 4] == '\r' || recent[PDFParser._numberOfCharsToKeep - 4] == '\n'))
          return true;
      }
      return false;
    }
  }
}
