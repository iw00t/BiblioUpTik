using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BiblioUpTik.Mobi.Metadata
{
  public class BaseHeader
  {
    protected SortedDictionary<string, object> fieldList = new SortedDictionary<string, object>();
    protected SortedDictionary<string, object> fieldListNoBlankRows = new SortedDictionary<string, object>();
    protected SortedDictionary<string, object> emptyFieldList = new SortedDictionary<string, object>();
    private List<string> fieldListExclude = new List<string>()
    {
      nameof (FieldList),
      nameof (FieldListNoBlankRows),
      nameof (EmptyFieldList),
      "EXTHHeader"
    };

    public SortedDictionary<string, object> FieldList
    {
      get
      {
        return this.fieldList;
      }
    }

    public SortedDictionary<string, object> FieldListNoBlankRows
    {
      get
      {
        return this.fieldListNoBlankRows;
      }
    }

    public SortedDictionary<string, object> EmptyFieldList
    {
      get
      {
        return this.emptyFieldList;
      }
    }

    public override string ToString()
    {
      return this.ToString(false);
    }

    public string ToString(bool showBlankRows)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (showBlankRows)
      {
        foreach (KeyValuePair<string, object> field in this.fieldList)
          stringBuilder.AppendLine(string.Format("{0}: {1}", (object) field.Key, field.Value));
      }
      else
      {
        foreach (KeyValuePair<string, object> fieldListNoBlankRow in this.fieldListNoBlankRows)
          stringBuilder.AppendLine(string.Format("{0}: {1}", (object) fieldListNoBlankRow.Key, fieldListNoBlankRow.Value));
      }
      return stringBuilder.ToString();
    }

    protected void PopulateFieldList()
    {
      this.PopulateFieldList(false);
    }

    protected void PopulateFieldList(bool blankOnly)
    {
      this.fieldList.Clear();
      this.emptyFieldList.Clear();
      foreach (PropertyInfo property in this.GetType().GetProperties())
      {
        if (!this.fieldListExclude.Contains(property.Name))
        {
          if (!blankOnly)
          {
            this.fieldList.Add(property.Name, property.GetValue((object) this, (object[]) null));
            if (property.GetValue((object) this, (object[]) null).ToString() != string.Empty)
              this.fieldListNoBlankRows.Add(property.Name, property.GetValue((object) this, (object[]) null));
          }
          this.emptyFieldList.Add(property.Name, (object) null);
        }
      }
    }
  }
}
