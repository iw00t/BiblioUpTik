using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace BiblioUpTik.Properties
{
  [CompilerGenerated]
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) BiblioUpTik.Properties.Resources.resourceMan, (object) null))
          BiblioUpTik.Properties.Resources.resourceMan = new ResourceManager("BiblioUpTik.Properties.Resources", typeof (BiblioUpTik.Properties.Resources).Assembly);
        return BiblioUpTik.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return BiblioUpTik.Properties.Resources.resourceCulture;
      }
      set
      {
        BiblioUpTik.Properties.Resources.resourceCulture = value;
      }
    }
  }
}
