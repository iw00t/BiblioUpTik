using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace BiblioUpTik
{
  public class PortableSettingsProvider : SettingsProvider
  {
    private const string Settingsroot = "Settings";
    private XmlDocument _settingsXML;

    public override void Initialize(string name, NameValueCollection col)
    {
      base.Initialize(this.ApplicationName, col);
    }

    public override string ApplicationName
    {
      get
      {
        if (Application.ProductName.Trim().Length > 0)
          return Application.ProductName;
        FileInfo fileInfo = new FileInfo(Application.ExecutablePath);
        return fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
      }
      set
      {
      }
    }

    public override string Name
    {
      get
      {
        return nameof (PortableSettingsProvider);
      }
    }

    public virtual string GetAppSettingsPath()
    {
      return new FileInfo(Application.ExecutablePath).DirectoryName;
    }

    public virtual string GetAppSettingsFilename()
    {
      return this.ApplicationName + ".settings";
    }

    public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection propvals)
    {
      foreach (SettingsPropertyValue propval in propvals)
        this.SetValue(propval);
      try
      {
        this.SettingsXML.Save(Path.Combine(this.GetAppSettingsPath(), this.GetAppSettingsFilename()));
      }
      catch (Exception ex)
      {
      }
    }

    public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
    {
      SettingsPropertyValueCollection propertyValueCollection = new SettingsPropertyValueCollection();
      foreach (SettingsProperty prop in props)
      {
        SettingsPropertyValue property = new SettingsPropertyValue(prop)
        {
          IsDirty = false,
          SerializedValue = (object) this.GetValue(prop)
        };
        propertyValueCollection.Add(property);
      }
      return propertyValueCollection;
    }

    private XmlDocument SettingsXML
    {
      get
      {
        if (this._settingsXML == null)
        {
          this._settingsXML = new XmlDocument();
          try
          {
            this._settingsXML.Load(Path.Combine(this.GetAppSettingsPath(), this.GetAppSettingsFilename()));
          }
          catch (Exception ex)
          {
            this._settingsXML.AppendChild((XmlNode) this._settingsXML.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
            this._settingsXML.AppendChild(this._settingsXML.CreateNode(XmlNodeType.Element, "Settings", ""));
          }
        }
        return this._settingsXML;
      }
    }

    private string GetValue(SettingsProperty setting)
    {
      string str;
      try
      {
        str = PortableSettingsProvider.IsRoaming(setting) ? this.SettingsXML.SelectSingleNode("Settings/" + setting.Name).InnerText : this.SettingsXML.SelectSingleNode("Settings/" + Environment.MachineName + "/" + setting.Name).InnerText;
      }
      catch (Exception ex)
      {
        str = setting.DefaultValue != null ? setting.DefaultValue.ToString() : "";
      }
      return str;
    }

    private void SetValue(SettingsPropertyValue propVal)
    {
      XmlElement xmlElement1;
      try
      {
        xmlElement1 = !PortableSettingsProvider.IsRoaming(propVal.Property) ? (XmlElement) this.SettingsXML.SelectSingleNode("Settings/" + Environment.MachineName + "/" + propVal.Name) : (XmlElement) this.SettingsXML.SelectSingleNode("Settings/" + propVal.Name);
      }
      catch (Exception ex)
      {
        xmlElement1 = (XmlElement) null;
      }
      if (xmlElement1 != null)
        xmlElement1.InnerText = propVal.SerializedValue.ToString();
      else if (PortableSettingsProvider.IsRoaming(propVal.Property))
      {
        XmlElement element = this.SettingsXML.CreateElement(propVal.Name);
        element.InnerText = propVal.SerializedValue.ToString();
        this.SettingsXML.SelectSingleNode("Settings").AppendChild((XmlNode) element);
      }
      else
      {
        XmlElement xmlElement2;
        try
        {
          xmlElement2 = (XmlElement) this.SettingsXML.SelectSingleNode("Settings/" + Environment.MachineName);
        }
        catch (Exception ex)
        {
          xmlElement2 = this.SettingsXML.CreateElement(Environment.MachineName);
          this.SettingsXML.SelectSingleNode("Settings").AppendChild((XmlNode) xmlElement2);
        }
        if (xmlElement2 == null)
        {
          xmlElement2 = this.SettingsXML.CreateElement(Environment.MachineName);
          this.SettingsXML.SelectSingleNode("Settings").AppendChild((XmlNode) xmlElement2);
        }
        XmlElement element = this.SettingsXML.CreateElement(propVal.Name);
        element.InnerText = propVal.SerializedValue.ToString();
        xmlElement2.AppendChild((XmlNode) element);
      }
    }

    private static bool IsRoaming(SettingsProperty prop)
    {
      return prop.Attributes.Cast<DictionaryEntry>().Select<DictionaryEntry, Attribute>((Func<DictionaryEntry, Attribute>) (d => (Attribute) d.Value)).OfType<SettingsManageabilityAttribute>().Any<SettingsManageabilityAttribute>();
    }
  }
}
