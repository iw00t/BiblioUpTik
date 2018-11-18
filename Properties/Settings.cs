using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BiblioUpTik.Properties
{
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
  [CompilerGenerated]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings defaultInstance = Settings.defaultInstance;
        return defaultInstance;
      }
    }

    [DebuggerNonUserCode]
    [SettingsManageability(SettingsManageability.Roaming)]
    [DefaultSettingValue("")]
    [UserScopedSetting]
    public string Username
    {
      get
      {
        return (string) this[nameof (Username)];
      }
      set
      {
        this[nameof (Username)] = (object) value;
      }
    }

    [DefaultSettingValue("")]
    [SettingsManageability(SettingsManageability.Roaming)]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public string Password
    {
      get
      {
        return (string) this[nameof (Password)];
      }
      set
      {
        this[nameof (Password)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    [SettingsManageability(SettingsManageability.Roaming)]
    public int Delay
    {
      get
      {
        return (int) this[nameof (Delay)];
      }
      set
      {
        this[nameof (Delay)] = (object) value;
      }
    }

    [DebuggerNonUserCode]
    [UserScopedSetting]
    [SettingsManageability(SettingsManageability.Roaming)]
    [DefaultSettingValue("%author% - %title% (%subtitle%)")]
    public string Template
    {
      get
      {
        return (string) this[nameof (Template)];
      }
      set
      {
        this[nameof (Template)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    [SettingsManageability(SettingsManageability.Roaming)]
    public bool OnRun
    {
      get
      {
        return (bool) this[nameof (OnRun)];
      }
      set
      {
        this[nameof (OnRun)] = (object) value;
      }
    }

    [DefaultSettingValue("False")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    [SettingsManageability(SettingsManageability.Roaming)]
    public bool Secure
    {
      get
      {
        return (bool) this[nameof (Secure)];
      }
      set
      {
        this[nameof (Secure)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    [SettingsManageability(SettingsManageability.Roaming)]
    public bool GetCover
    {
      get
      {
        return (bool) this[nameof (GetCover)];
      }
      set
      {
        this[nameof (GetCover)] = (object) value;
      }
    }

    [SettingsManageability(SettingsManageability.Roaming)]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool GetPublisher
    {
      get
      {
        return (bool) this[nameof (GetPublisher)];
      }
      set
      {
        this[nameof (GetPublisher)] = (object) value;
      }
    }

    [SettingsManageability(SettingsManageability.Roaming)]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    [DefaultSettingValue("False")]
    public bool GetPages
    {
      get
      {
        return (bool) this[nameof (GetPages)];
      }
      set
      {
        this[nameof (GetPages)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    [SettingsManageability(SettingsManageability.Roaming)]
    public bool GetDescription
    {
      get
      {
        return (bool) this[nameof (GetDescription)];
      }
      set
      {
        this[nameof (GetDescription)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DefaultSettingValue("True")]
    [DebuggerNonUserCode]
    [SettingsManageability(SettingsManageability.Roaming)]
    public bool GetEpub
    {
      get
      {
        return (bool) this[nameof (GetEpub)];
      }
      set
      {
        this[nameof (GetEpub)] = (object) value;
      }
    }

    [UserScopedSetting]
    [SettingsManageability(SettingsManageability.Roaming)]
    [DefaultSettingValue("True")]
    [DebuggerNonUserCode]
    public bool GetMobi
    {
      get
      {
        return (bool) this[nameof (GetMobi)];
      }
      set
      {
        this[nameof (GetMobi)] = (object) value;
      }
    }

    [DefaultSettingValue("True")]
    [SettingsManageability(SettingsManageability.Roaming)]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public bool GetPDF
    {
      get
      {
        return (bool) this[nameof (GetPDF)];
      }
      set
      {
        this[nameof (GetPDF)] = (object) value;
      }
    }

    [UserScopedSetting]
    [SettingsManageability(SettingsManageability.Roaming)]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool RecursiveDir
    {
      get
      {
        return (bool) this[nameof (RecursiveDir)];
      }
      set
      {
        this[nameof (RecursiveDir)] = (object) value;
      }
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("128")]
    [SettingsManageability(SettingsManageability.Roaming)]
    [UserScopedSetting]
    public int Width
    {
      get
      {
        return (int) this[nameof (Width)];
      }
      set
      {
        this[nameof (Width)] = (object) value;
      }
    }

    [SettingsManageability(SettingsManageability.Roaming)]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    [DefaultSettingValue("192")]
    public int Height
    {
      get
      {
        return (int) this[nameof (Height)];
      }
      set
      {
        this[nameof (Height)] = (object) value;
      }
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    [UserScopedSetting]
    [SettingsManageability(SettingsManageability.Roaming)]
    public bool ShowCover
    {
      get
      {
        return (bool) this[nameof (ShowCover)];
      }
      set
      {
        this[nameof (ShowCover)] = (object) value;
      }
    }

    [DefaultSettingValue("")]
    [UserScopedSetting]
    [SettingsManageability(SettingsManageability.Roaming)]
    [DebuggerNonUserCode]
    public string LastFolder
    {
      get
      {
        return (string) this[nameof (LastFolder)];
      }
      set
      {
        this[nameof (LastFolder)] = (object) value;
      }
    }

    [DefaultSettingValue("0")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public int FormX
    {
      get
      {
        return (int) this[nameof (FormX)];
      }
      set
      {
        this[nameof (FormX)] = (object) value;
      }
    }

    [DebuggerNonUserCode]
    [UserScopedSetting]
    [DefaultSettingValue("0")]
    public int FormY
    {
      get
      {
        return (int) this[nameof (FormY)];
      }
      set
      {
        this[nameof (FormY)] = (object) value;
      }
    }

    [UserScopedSetting]
    [SettingsManageability(SettingsManageability.Roaming)]
    [DefaultSettingValue("")]
    [DebuggerNonUserCode]
    public string AnnounceURL
    {
      get
      {
        return (string) this[nameof (AnnounceURL)];
      }
      set
      {
        this[nameof (AnnounceURL)] = (object) value;
      }
    }

    [DebuggerNonUserCode]
    [SettingsManageability(SettingsManageability.Roaming)]
    [DefaultSettingValue("True")]
    [UserScopedSetting]
    public bool GetAZW3
    {
      get
      {
        return (bool) this[nameof (GetAZW3)];
      }
      set
      {
        this[nameof (GetAZW3)] = (object) value;
      }
    }
  }
}
