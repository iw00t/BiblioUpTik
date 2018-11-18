using System.IO;

namespace BiblioUpTik.Mobi.Metadata
{
  public class MobiMetadata
  {
    private PDBHead pdbHeader;
    private PalmDOCHead palmDocHeader;
    private MobiHead mobiHeader;

    public PDBHead PDBHeader
    {
      get
      {
        return this.pdbHeader;
      }
    }

    public PalmDOCHead PalmDocHeader
    {
      get
      {
        return this.palmDocHeader;
      }
    }

    public MobiHead MobiHeader
    {
      get
      {
        return this.mobiHeader;
      }
    }

    public MobiMetadata(FileStream fs)
    {
      this.SetUpData(fs);
    }

    public MobiMetadata(string filePath)
    {
      FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
      this.SetUpData(fs);
      fs.Close();
    }

    private void SetUpData(FileStream fs)
    {
      this.pdbHeader = new PDBHead(fs);
      this.palmDocHeader = new PalmDOCHead(fs);
      this.mobiHeader = new MobiHead(fs, this.pdbHeader.MobiHeaderSize);
    }
  }
}
