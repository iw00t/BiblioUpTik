using BiblioUpTik.Mobi.Metadata;
using eBdb.EpubReader;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace BiblioUpTik
{
  internal class Book : ListViewItem
  {
    public static ListViewItem GetBook(FileSystemInfo file)
    {
      switch (file.Extension)
      {
        case ".mobi":
          return Book.AddMobi(file);
        case ".azw3":
          return Book.AddMobi(file);
        case ".epub":
          return Book.AddEpub(file);
        case ".pdf":
          return Book.AddPDF(file);
        default:
          return (ListViewItem) null;
      }
    }

    private static ListViewItem AddPDF(FileSystemInfo file)
    {
      PdfReader pdf = new PdfReader(file.FullName);
      ListViewItem listViewItem1 = new ListViewItem()
      {
        Text = file.Name,
        Tag = (object) (file.FullName + "| non-retail")
      };
      listViewItem1.SubItems.Add("None");
      string text1 = pdf.Info.ContainsKey("Author") ? pdf.Info["Author"] : "";
      string text2 = pdf.Info.ContainsKey("Title") ? pdf.Info["Title"] : "";
      string str;
      if (!pdf.Info.ContainsKey("EBX_PUBLISHER"))
        str = "";
      else
        str = pdf.Info["EBX_PUBLISHER"].Split(',')[0];
      string text3 = str;
      string text4 = pdf.Info.ContainsKey("CreationDate") ? pdf.Info["CreationDate"].Substring(pdf.Info["CreationDate"].IndexOf(':') + 1, 4) : "0";
      string isbn = Book.DigitsOnly(Book.FindISBN(pdf));
      string text5 = Book.IsISBNValid(isbn) ? isbn : "";
      string text6 = "";
      int numberOfPages = pdf.NumberOfPages;
      listViewItem1.SubItems.Add(text1);
      listViewItem1.SubItems.Add(text2);
      listViewItem1.SubItems.Add(text3);
      listViewItem1.SubItems.Add(text4);
      listViewItem1.SubItems.Add(text5);
      listViewItem1.SubItems.Add(text6);
      listViewItem1.SubItems.Add(numberOfPages.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      listViewItem1.SubItems.Add("");
      listViewItem1.SubItems.Add("");
      ListViewItem listViewItem2 = BiblioUpTik.Properties.Settings.Default.OnRun ? Book.ScrapeData(listViewItem1) : listViewItem1;
      listViewItem2.SubItems[8].Text = Book.DigitsOnly(listViewItem2.SubItems[8].Text);
      if (string.IsNullOrWhiteSpace(listViewItem2.SubItems[8].Text))
        listViewItem2.SubItems[8].Text = "0";
      for (int index = 2; index < listViewItem2.SubItems.Count; ++index)
        listViewItem2.SubItems[index].Text = HttpUtility.HtmlDecode(listViewItem2.SubItems[index].Text);
      return listViewItem2;
    }

    private static ListViewItem AddEpub(FileSystemInfo file)
    {
      try
      {
        Epub epub = new Epub(file.FullName);
        ListViewItem listViewItem1 = new ListViewItem()
        {
          Text = file.Name,
          Tag = (object) (file.FullName + "| non-retail")
        };
        listViewItem1.SubItems.Add("None");
        string str1 = epub.Creator.Count > 0 ? string.Join(", ", (IEnumerable<string>) epub.Creator) : "";
        if (str1.Contains<char>(',') && epub.Creator.Count == 1)
        {
          string[] strArray = str1.Split(',');
          str1 = strArray[1].Trim() + " " + strArray[0];
        }
        string text1 = epub.Title.Count > 0 ? string.Join(": ", (IEnumerable<string>) epub.Title) : "";
        string text2 = epub.Publisher.Count > 0 ? string.Join(", ", (IEnumerable<string>) epub.Publisher) : "";
        string str2;
        if (epub.Date.Count <= 0)
          str2 = "0";
        else
          str2 = epub.Date[0].Split(' ')[0].Split('-')[0];
        string input = str2;
        if (input.Contains("/"))
          input = input.Split('/')[2];
        string text3 = Book.DigitsOnly(input);
        if (string.IsNullOrWhiteSpace(text3) || Convert.ToInt32(text3) > DateTime.Now.Year)
          text3 = "0";
        string text4 = epub.ID.Count <= 0 ? Book.FindISBN(epub) : (Book.IsISBNValid(Book.DigitsOnly(epub.ID[0])) ? Book.DigitsOnly(epub.ID[0]) : Book.FindISBN(epub));
        string text5 = epub.Description.Count > 0 ? epub.Description[0] : "";
        listViewItem1.SubItems.Add(str1);
        listViewItem1.SubItems.Add(text1);
        listViewItem1.SubItems.Add(text2);
        listViewItem1.SubItems.Add(text3);
        listViewItem1.SubItems.Add(text4);
        listViewItem1.SubItems.Add(text5);
        listViewItem1.SubItems.Add("0");
        listViewItem1.SubItems.Add("");
        listViewItem1.SubItems.Add("");
        ListViewItem listViewItem2 = BiblioUpTik.Properties.Settings.Default.OnRun ? Book.ScrapeData(listViewItem1) : listViewItem1;
        listViewItem2.SubItems[8].Text = Book.DigitsOnly(listViewItem2.SubItems[8].Text);
        if (string.IsNullOrWhiteSpace(listViewItem2.SubItems[8].Text))
          listViewItem2.SubItems[8].Text = "0";
        for (int index = 2; index < listViewItem2.SubItems.Count; ++index)
          listViewItem2.SubItems[index].Text = HttpUtility.HtmlDecode(listViewItem2.SubItems[index].Text);
        return listViewItem2;
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
      }
      return (ListViewItem) null;
    }

    private static ListViewItem AddMobi(FileSystemInfo file)
    {
      MobiMetadata mobiMetadata = new MobiMetadata(file.FullName);
      ListViewItem listViewItem1 = new ListViewItem()
      {
        Text = file.Name,
        Tag = (object) (file.FullName + "| non-retail | " + mobiMetadata.MobiHeader.EXTHHeader.ASIN)
      };
      string str = mobiMetadata.MobiHeader.EXTHHeader.Author;
      if (str.Contains<char>(','))
      {
        string[] strArray = str.Split(',');
        str = strArray[1].Trim() + " " + strArray[0];
      }
      string updatedTitle = mobiMetadata.MobiHeader.EXTHHeader.UpdatedTitle;
      string text1 = mobiMetadata.MobiHeader.EXTHHeader.Publisher.Split(',')[0];
      string text2 = Book.DigitsOnly(mobiMetadata.MobiHeader.EXTHHeader.PublishedDate.Split('-')[0]);
      if (string.IsNullOrWhiteSpace(text2) || Convert.ToInt32(text2) > DateTime.Now.Year)
        text2 = "0";
      string text3 = "";
      if (!string.IsNullOrEmpty(mobiMetadata.MobiHeader.EXTHHeader.IBSN))
        text3 = Book.IsISBNValid(Book.DigitsOnly(mobiMetadata.MobiHeader.EXTHHeader.IBSN)) ? Book.DigitsOnly(mobiMetadata.MobiHeader.EXTHHeader.IBSN) : "";
      else if (mobiMetadata.MobiHeader.EXTHHeader.Source.Contains("ISBN"))
        text3 = Book.IsISBNValid(Book.DigitsOnly(mobiMetadata.MobiHeader.EXTHHeader.Source)) ? Book.DigitsOnly(mobiMetadata.MobiHeader.EXTHHeader.Source) : "";
      string description = mobiMetadata.MobiHeader.EXTHHeader.Description;
      listViewItem1.SubItems.Add("None");
      listViewItem1.SubItems.Add(str);
      listViewItem1.SubItems.Add(updatedTitle);
      listViewItem1.SubItems.Add(text1);
      listViewItem1.SubItems.Add(text2);
      listViewItem1.SubItems.Add(text3);
      listViewItem1.SubItems.Add(description);
      listViewItem1.SubItems.Add("0");
      listViewItem1.SubItems.Add("");
      listViewItem1.SubItems.Add("");
      ListViewItem listViewItem2 = BiblioUpTik.Properties.Settings.Default.OnRun ? Book.ScrapeMobiData(listViewItem1) : listViewItem1;
      listViewItem2.SubItems[8].Text = Book.DigitsOnly(listViewItem2.SubItems[8].Text);
      if (string.IsNullOrWhiteSpace(listViewItem2.SubItems[8].Text))
        listViewItem2.SubItems[8].Text = "0";
      for (int index = 2; index < listViewItem2.SubItems.Count; ++index)
        listViewItem2.SubItems[index].Text = HttpUtility.HtmlDecode(listViewItem2.SubItems[index].Text);
      return listViewItem2;
    }

    private static string CheckISBNPage(string page)
    {
      string str = "";
      if (page.Contains("Copyright") && page.Contains("ISBN"))
      {
        if (page.Contains("ISBN-10") || page.Contains("ISBN-13") || page.Contains("ISBN 10") || page.Contains("ISBN 13"))
        {
          page = page.Replace("ISBN-10", "ISBN");
          page = page.Replace("ISBN-13", "ISBN");
          page = page.Replace("ISBN 10", "ISBN");
          page = page.Replace("ISBN 13", "ISBN");
        }
        page = page.Replace(":", "");
        page = page.Replace("-", "");
        str = Book.DigitsOnly(page.Substring(page.IndexOf("ISBN", StringComparison.Ordinal), 19).Replace("ISBN ", "").Split(' ')[0]);
      }
      return str;
    }

    private static string FindISBN(PdfReader pdf)
    {
      string isbn = "";
      for (int pageNum = 1; pageNum <= 30; ++pageNum)
      {
        isbn = Book.CheckISBNPage(PDFParser.ExtractTextFromPDFBytes(pdf.GetPageContent(pageNum)));
        if (!string.IsNullOrWhiteSpace(isbn))
          break;
      }
      return Book.IsISBNValid(isbn) ? isbn : "";
    }

    private static string FindISBN(Epub epub)
    {
      string isbn = Book.CheckISBNPage(epub.GetContentAsPlainText());
      return Book.IsISBNValid(isbn) ? isbn : "";
    }

    public static string DigitsOnly(string input)
    {
      if (string.IsNullOrEmpty(input))
        return input;
      input = new string(Array.FindAll<char>(input.ToCharArray(), (Predicate<char>) (c => char.IsDigit(c))));
      return input;
    }

    public static string ISBN(string isbn)
    {
      if (string.IsNullOrEmpty(isbn))
        return isbn;
      isbn = new string(Array.FindAll<char>(isbn.ToCharArray(), (Predicate<char>) (c => char.IsDigit(c) || c == 'X')));
      return isbn;
    }

    public static bool IsISBNValid(string isbn)
    {
      int num1 = 0;
      int num2 = 0;
      if (string.IsNullOrEmpty(isbn))
        return false;
      char[] charArray = isbn.ToCharArray();
      if (charArray.Length != 10 && charArray.Length != 13)
        return false;
      switch (charArray.Length)
      {
        case 10:
          for (int index = 0; index < 9; ++index)
          {
            if (charArray[index] < '0' || charArray[index] > '9')
              return false;
          }
          if (charArray[9] != 'X' && (charArray[9] < '0' || charArray[9] > '9'))
            return false;
          for (int index = 0; index < 10; ++index)
          {
            if (charArray[index] == 'X')
              num1 += 10;
            else
              num1 += (int) charArray[index] - 48;
            num2 += num1;
          }
          return num2 % 11 == 0;
        case 13:
          int num3 = 0;
          int index1 = 0;
          while (index1 < 13)
          {
            num3 += (int) charArray[index1];
            index1 += 2;
          }
          int index2 = 1;
          while (index2 < 12)
          {
            num3 += 3 * (int) charArray[index2];
            index2 += 2;
          }
          return num3 % 10 == 0;
        default:
          return false;
      }
    }

    public static string ConvertToISBN10(string isbn)
    {
      if (!Book.IsISBNValid(isbn) || isbn.Length != 13)
        return isbn;
      isbn = isbn.Substring(3, 9);
      char[] charArray = isbn.ToCharArray();
      int start = 11;
      int num = 11 - ((IEnumerable<char>) charArray).Select<char, int>((Func<char, int>) (t => (int) char.GetNumericValue(t) * --start)).Sum() % 11;
      string str1;
      switch (num)
      {
        case 10:
          str1 = "X";
          break;
        case 11:
          str1 = "0";
          break;
        default:
          str1 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          break;
      }
      string str2 = str1;
      isbn += str2;
      return isbn;
    }

    public static ListViewItem ScrapeMobiData(ListViewItem item)
    {
      try
      {
        string query = item.Tag.ToString().Split('|')[2].Trim();
        BiblioUpTik.Web web = new BiblioUpTik.Web();
        try
        {
          Uri url = new Uri(new Uri("http://www.amazon.com/dp/").ToString() + query);
          if (BiblioUpTik.Properties.Settings.Default.GetPublisher || BiblioUpTik.Properties.Settings.Default.GetPages)
          {
            string strSource = web.BookRequest(url);
            if (BiblioUpTik.Properties.Settings.Default.GetPublisher)
              item.SubItems[4].Text = web.GetStringBetween(strSource, "<b>Publisher:</b>", ";", 0).Replace(",", "").Trim();
            if (BiblioUpTik.Properties.Settings.Default.GetPages && int.Parse(item.SubItems[8].Text) <= 0)
              item.SubItems[8].Text = web.GetStringBetween(strSource, "<b>Print Length:</b>", " pages", 0).Trim();
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine((object) ex);
        }
        if (BiblioUpTik.Properties.Settings.Default.GetCover || BiblioUpTik.Properties.Settings.Default.GetDescription)
        {
          GoogleBook googleBook = string.IsNullOrWhiteSpace(item.SubItems[6].Text) ? new GoogleBook(item, false) : new GoogleBook(item, true);
          bool flag = googleBook.VolumeInfo() != null;
          if (BiblioUpTik.Properties.Settings.Default.GetDescription && flag)
            item.SubItems[7].Text = googleBook.Description() ?? item.SubItems[7].Text;
          if (BiblioUpTik.Properties.Settings.Default.GetCover)
          {
            if (query != "")
              item.SubItems[9].Text = Book.AzCoverLink(query);
            else if (!string.IsNullOrWhiteSpace(item.SubItems[6].Text))
              item.SubItems[9].Text = Book.AzCoverLink(Book.ConvertToISBN10(item.SubItems[6].Text));
            if (string.IsNullOrWhiteSpace(item.SubItems[9].Text) && query != "")
              item.SubItems[9].Text = Book.GrCoverLink(query);
          }
          if (!string.IsNullOrWhiteSpace(item.SubItems[6].Text) && flag)
          {
            if (string.IsNullOrWhiteSpace(item.SubItems[5].Text))
              item.SubItems[5].Text = googleBook.PublishDate() ?? "";
            if (string.IsNullOrWhiteSpace(item.SubItems[2].Text))
              item.SubItems[2].Text = googleBook.Author() ?? "";
            if (string.IsNullOrWhiteSpace(item.SubItems[3].Text))
              item.SubItems[3].Text = googleBook.Title() ?? "";
          }
        }
        return item;
      }
      catch (Exception ex)
      {
        return item;
      }
    }

    public static ListViewItem ScrapeData(ListViewItem item)
    {
      try
      {
        if (BiblioUpTik.Properties.Settings.Default.GetCover || BiblioUpTik.Properties.Settings.Default.GetDescription || BiblioUpTik.Properties.Settings.Default.GetPublisher || BiblioUpTik.Properties.Settings.Default.GetPages)
        {
          GoogleBook googleBook = string.IsNullOrWhiteSpace(item.SubItems[6].Text) ? new GoogleBook(item, false) : new GoogleBook(item, true);
          bool flag = googleBook.VolumeInfo() != null;
          if (flag)
          {
            if (BiblioUpTik.Properties.Settings.Default.GetDescription)
              item.SubItems[7].Text = googleBook.Description() ?? item.SubItems[7].Text;
            if (BiblioUpTik.Properties.Settings.Default.GetPublisher)
              item.SubItems[4].Text = googleBook.Publisher() ?? item.SubItems[4].Text;
            if (BiblioUpTik.Properties.Settings.Default.GetPages)
              item.SubItems[8].Text = googleBook.PageCount() ?? item.SubItems[8].Text;
          }
          if (BiblioUpTik.Properties.Settings.Default.GetCover)
          {
            if (!string.IsNullOrWhiteSpace(item.SubItems[6].Text))
            {
              item.SubItems[9].Text = Book.AzCoverLink(Book.ConvertToISBN10(item.SubItems[6].Text));
              if (string.IsNullOrWhiteSpace(item.SubItems[9].Text))
                item.SubItems[9].Text = Book.GrCoverLink(item.SubItems[6].Text);
            }
            if (string.IsNullOrWhiteSpace(item.SubItems[9].Text))
              item.SubItems[9].Text = Book.GrCoverLink(item.SubItems[3].Text.Split('(')[0].Trim().Replace(' ', '+'));
          }
          if (!string.IsNullOrWhiteSpace(item.SubItems[6].Text) && flag)
          {
            if (string.IsNullOrWhiteSpace(item.SubItems[5].Text))
              item.SubItems[5].Text = googleBook.PublishDate() ?? "";
            if (string.IsNullOrWhiteSpace(item.SubItems[2].Text))
              item.SubItems[2].Text = googleBook.Author() ?? "";
            if (string.IsNullOrWhiteSpace(item.SubItems[3].Text))
              item.SubItems[3].Text = googleBook.Title() ?? "";
          }
        }
        return item;
      }
      catch (Exception ex)
      {
        return item;
      }
    }

    private static string GrCoverLink(string query)
    {
      try
      {
        Uri uri = new Uri("http://www.goodreads.com/");
        Uri url = new Uri(uri.ToString() + "search?utf8=✓&query=" + query);
        BiblioUpTik.Web web = new BiblioUpTik.Web();
        string strSource1 = web.BookRequest(url);
        string str = new Uri(new Uri(uri.ToString() + "/book/show/").ToString() + web.GetStringBetween(strSource1, "<img id=\"coverImage\"", "/>", 0)).ToString();
        int startIndex = str.IndexOf("src=\"") + 5;
        int length = str.LastIndexOf('"') - startIndex;
        try
        {
          return str.Substring(startIndex, length);
        }
        catch (Exception ex)
        {
          string strSource2 = new BiblioUpTik.Web().BookRequest(new Uri(new Uri("http://www.overdrive.com/").ToString() + "search?q=" + query));
          return new Uri(web.GetStringBetween(strSource2, "<img not-data-src=\"/Content/img/load.gif\" src=\"", "\"", 0)).ToString();
        }
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    public static string AzCoverLink(string query)
    {
      try
      {
        Uri url = new Uri(new Uri("http://www.amazon.com/gp/product/images/").ToString() + query);
        BiblioUpTik.Web web = new BiblioUpTik.Web();
        string strSource = web.BookRequest(url);
        string stringBetween1 = web.GetStringBetween(strSource, "<noscript><div id=\"imageViewerDiv\">", "</div></noscript>", 0);
        string stringBetween2 = web.GetStringBetween(stringBetween1, "<img src=\"", "\" id=", 0);
        int startIndex = stringBetween2.IndexOf('_') - 1;
        int count = stringBetween2.LastIndexOf('_') + 1 - startIndex;
        return stringBetween2.Remove(startIndex, count);
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    public string GetSubstringByString(string a, string b, string c)
    {
      return c.Substring(c.IndexOf(a) + a.Length, c.IndexOf(b) - c.IndexOf(a) - a.Length);
    }
  }
}
