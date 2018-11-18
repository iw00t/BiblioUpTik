using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace BiblioUpTik
{
  internal class Web
  {
    public string GetStringBetween(string strSource, string strBegin, string strEnd, int Start)
    {
      string[] strArray = new string[2]{ "", "" };
      for (int index = 0; index <= Start; ++index)
      {
        strSource = index >= 1 ? strArray[1] : strSource;
        int num = strSource.IndexOf(strBegin);
        if (num != -1)
        {
          strSource = strSource.Substring(num + strBegin.Length);
          int length = strSource.IndexOf(strEnd);
          if (length != -1)
          {
            strArray[0] = strSource.Substring(0, length);
            if (length + strEnd.Length < strSource.Length)
              strArray[1] = strSource.Substring(length + strEnd.Length);
          }
        }
        else
          strArray[1] = strSource;
      }
      return strArray[0];
    }

    public string Strip(string str, bool html = true, bool newLine = true)
    {
      string input = (string) null;
      try
      {
        if (html)
          input = Regex.Replace(str, "<.*?>", string.Empty);
        if (newLine)
          input = input != null ? Regex.Replace(input, "(\n|\r)+", string.Empty) : Regex.Replace(str, "(\n|\r)+", string.Empty);
      }
      catch
      {
        input = string.Empty;
      }
      return input;
    }

    public string BookRequest(Uri url)
    {
      StringBuilder stringBuilder = new StringBuilder();
      byte[] numArray = new byte[8192];
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; WOW64; Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618; InfoPath.2; OfficeLiveConnector.1.3; OfficeLivePatch.0.0)";
      WebResponse response = httpWebRequest.GetResponse();
      Stream responseStream = response.GetResponseStream();
      int count;
      do
      {
        count = responseStream.Read(numArray, 0, numArray.Length);
        if (count != 0)
        {
          string str = Encoding.ASCII.GetString(numArray, 0, count);
          stringBuilder.Append(str);
        }
      }
      while (count > 0);
      responseStream.Close();
      response.Close();
      return stringBuilder.ToString();
    }
  }
}
