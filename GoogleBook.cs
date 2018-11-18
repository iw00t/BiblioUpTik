using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BiblioUpTik
{
  internal class GoogleBook
  {
    private static readonly Web Web = new Web();
    private const string Key = "AIzaSyA-WBA24-RLKI6QT2K9uj0S3pk2Py80FHo";
    private static JToken _volumeInfo;

    public GoogleBook(ListViewItem item, bool isbnSearch)
    {
      try
      {
        string text1 = item.SubItems[2].Text;
        string text2 = item.SubItems[3].Text;
        string text3 = item.SubItems[6].Text;
        GoogleBook._volumeInfo = isbnSearch ? GoogleBook.GetVolumeInfo(GoogleBook.GetGoogleUrl(text3)) : GoogleBook.GetVolumeInfo(GoogleBook.GetGoogleUrl(text1, text2));
        if (!isbnSearch || GoogleBook._volumeInfo != null || (string.IsNullOrWhiteSpace(text1) || string.IsNullOrWhiteSpace(text2)))
          return;
        GoogleBook._volumeInfo = GoogleBook.GetVolumeInfo(GoogleBook.GetGoogleUrl(text1, text2));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    public JToken VolumeInfo()
    {
      return GoogleBook._volumeInfo;
    }

    public string Author()
    {
      return GoogleBook._volumeInfo[(object) "authors"].ToString().Trim('"').Replace("\"", "").Trim('[', ']').Replace("  ", " ").Trim();
    }

    public string Title()
    {
      return GoogleBook._volumeInfo[(object) "title"].ToString().Trim('"');
    }

    public string PublishDate()
    {
      return GoogleBook._volumeInfo[(object) "publishedDate"].ToString().Trim('"');
    }

    public string Cover()
    {
      return GoogleBook._volumeInfo[(object) "imageLinks"][(object) "small"].ToString().Trim('"');
    }

    public string Description()
    {
      return GoogleBook._volumeInfo[(object) "description"].ToString().Replace("\\", "").Trim('"');
    }

    public string Publisher()
    {
      return GoogleBook._volumeInfo[(object) "publisher"].ToString().Replace(",", "").Trim('"');
    }

    public string PageCount()
    {
      return GoogleBook._volumeInfo[(object) "pageCount"].ToString();
    }

    public string Tags()
    {
      return string.Join(" ", ((IEnumerable<string>) GoogleBook._volumeInfo[(object) "categories"].ToList<JToken>().Aggregate<JToken, string>((string) null, (Func<string, JToken, string>) ((current1, objectResult) => ((IEnumerable<string>) objectResult.ToString().Split('/')).Aggregate<string, string>(current1, (Func<string, string, string>) ((current, tag) => current + tag.Trim() + ", ")))).Replace("\"", " / ").Trim(' ', ',', '/').Replace(" / ,  / ", ", ").Split(' ')).Distinct<string>());
    }

    private static Uri GetGoogleUrl(string isbn)
    {
      Uri url = new Uri(new Uri("https://www.googleapis.com/books/v1/volumes?q=").ToString() + "isbn:" + isbn + "&key=AIzaSyA-WBA24-RLKI6QT2K9uj0S3pk2Py80FHo");
      JObject jobject = JObject.Parse(GoogleBook.Web.BookRequest(url));
      if (jobject.Count > 2)
        return jobject["items"].Children().ToList<JToken>().Where<JToken>((Func<JToken, bool>) (objectResult => objectResult.ToString().Contains(isbn))).Select<JToken, Uri>((Func<JToken, Uri>) (objectResult => new Uri(objectResult[(object) "selfLink"].ToString().Replace("\"", "")))).FirstOrDefault<Uri>();
      return (Uri) null;
    }

    private static Uri GetGoogleUrl(string author, string title)
    {
      Uri uri = new Uri("https://www.googleapis.com/books/v1/volumes?q=intitle:");
      title = title.Split('(')[0].Split(':')[0].Trim();
      author = author.Replace(" ", "+").Split(',')[0];
      Uri url = new Uri(uri.ToString() + title.Replace(" ", "+") + "+inauthor:" + author + "&filter=ebooks&key=AIzaSyA-WBA24-RLKI6QT2K9uj0S3pk2Py80FHo");
      JObject jobject = JObject.Parse(GoogleBook.Web.BookRequest(url));
      if (jobject.Count > 2)
        return jobject["items"].Children().ToList<JToken>().Where<JToken>((Func<JToken, bool>) (objectResult => objectResult.ToString().Contains(title))).Select<JToken, Uri>((Func<JToken, Uri>) (objectResult => new Uri(objectResult[(object) "selfLink"].ToString().Replace("\"", "")))).FirstOrDefault<Uri>();
      return (Uri) null;
    }

    private static JToken GetVolumeInfo(Uri bookLink)
    {
      if (bookLink == (Uri) null)
        return (JToken) null;
      return JObject.Parse(GoogleBook.Web.BookRequest(new Uri(bookLink.ToString() + "?key=AIzaSyA-WBA24-RLKI6QT2K9uj0S3pk2Py80FHo")))["volumeInfo"];
    }
  }
}
