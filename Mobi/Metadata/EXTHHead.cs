using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BiblioUpTik.Mobi.Metadata
{
  public class EXTHHead : BaseHeader
  {
    private byte[] identifier = new byte[4];
    private byte[] headerLength = new byte[4];
    private byte[] recordCount = new byte[4];
    private List<EXTHRecord> recordList = new List<EXTHRecord>();

    public EXTHHead()
    {
      this.PopulateFieldList(true);
    }

    public EXTHHead(FileStream fs)
    {
      fs.Read(this.identifier, 0, this.identifier.Length);
      if (this.IdentifierAsString != "EXTH")
        throw new IOException("Expected to find EXTH header identifier EXTH but got something else instead");
      fs.Read(this.headerLength, 0, this.headerLength.Length);
      fs.Read(this.recordCount, 0, this.recordCount.Length);
      for (int index = 0; (long) index < (long) this.RecordCount; ++index)
        this.recordList.Add(new EXTHRecord(fs));
      this.PopulateFieldList();
    }

    protected int DataSize
    {
      get
      {
        int num = 0;
        foreach (EXTHRecord record in this.recordList)
          num += record.Size;
        return num;
      }
    }

    public int Size
    {
      get
      {
        int dataSize = this.DataSize;
        return 12 + dataSize + this.GetPaddingSize(dataSize);
      }
    }

    protected int GetPaddingSize(int dataSize)
    {
      int num = dataSize % 4;
      if (num != 0)
        num = 4 - num;
      return num;
    }

    public string IdentifierAsString
    {
      get
      {
        return Encoding.UTF8.GetString(this.identifier).Replace("\0", string.Empty);
      }
    }

    public uint HeaderLength
    {
      get
      {
        return Converter.ToUInt32(this.headerLength);
      }
    }

    public uint RecordCount
    {
      get
      {
        return Converter.ToUInt32(this.recordCount);
      }
    }

    public string Author
    {
      get
      {
        return this.GetRecordByType(100);
      }
    }

    public string Publisher
    {
      get
      {
        return this.GetRecordByType(101);
      }
    }

    public string Imprint
    {
      get
      {
        return this.GetRecordByType(102);
      }
    }

    public string Description
    {
      get
      {
        return this.GetRecordByType(103);
      }
    }

    public string IBSN
    {
      get
      {
        return this.GetRecordByType(104);
      }
    }

    public string Subject
    {
      get
      {
        return this.GetRecordByType(105);
      }
    }

    public string PublishedDate
    {
      get
      {
        return this.GetRecordByType(106);
      }
    }

    public string Review
    {
      get
      {
        return this.GetRecordByType(107);
      }
    }

    public string Contributor
    {
      get
      {
        return this.GetRecordByType(108);
      }
    }

    public string Rights
    {
      get
      {
        return this.GetRecordByType(109);
      }
    }

    public string SubjectCode
    {
      get
      {
        return this.GetRecordByType(110);
      }
    }

    public string Type
    {
      get
      {
        return this.GetRecordByType(111);
      }
    }

    public string Source
    {
      get
      {
        return this.GetRecordByType(112);
      }
    }

    public string ASIN
    {
      get
      {
        return this.GetRecordByType(113);
      }
    }

    public string VersionNumber
    {
      get
      {
        return this.GetRecordByType(114);
      }
    }

    public string RetailPrice
    {
      get
      {
        return this.GetRecordByType(118);
      }
    }

    public string RetailPriceCurrency
    {
      get
      {
        return this.GetRecordByType(119);
      }
    }

    public string DictionaryShortName
    {
      get
      {
        return this.GetRecordByType(200);
      }
    }

    public string CDEType
    {
      get
      {
        return this.GetRecordByType(501);
      }
    }

    public string UpdatedTitle
    {
      get
      {
        return this.GetRecordByType(503);
      }
    }

    public string ASIN2
    {
      get
      {
        return this.GetRecordByType(504);
      }
    }

    private string GetRecordByType(int recType)
    {
      string empty = string.Empty;
      foreach (EXTHRecord record in this.recordList)
      {
        if ((long) record.RecordType == (long) recType)
          empty = Encoding.UTF8.GetString(record.RecordData);
      }
      return empty;
    }
  }
}
