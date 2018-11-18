using System.Collections;
using System.Windows.Forms;

namespace BiblioUpTik
{
  public class ListViewColumnSorter : IComparer
  {
    private int _columnToSort;
    private SortOrder _orderOfSort;
    private readonly CaseInsensitiveComparer _objectCompare;

    public ListViewColumnSorter()
    {
      this._columnToSort = 0;
      this._orderOfSort = SortOrder.None;
      this._objectCompare = new CaseInsensitiveComparer();
    }

    public int Compare(object x, object y)
    {
      int num = this._objectCompare.Compare((object) ((ListViewItem) x).SubItems[this._columnToSort].Text, (object) ((ListViewItem) y).SubItems[this._columnToSort].Text);
      switch (this._orderOfSort)
      {
        case SortOrder.Ascending:
          return num;
        case SortOrder.Descending:
          return -num;
        default:
          return 0;
      }
    }

    public int SortColumn
    {
      set
      {
        this._columnToSort = value;
      }
      get
      {
        return this._columnToSort;
      }
    }

    public SortOrder Order
    {
      set
      {
        this._orderOfSort = value;
      }
      get
      {
        return this._orderOfSort;
      }
    }
  }
}
