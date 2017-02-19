using System;
using System.IO;
using System.Xml;

namespace D43toPT
{
	public class ProgramSettings : IDisposable
	{
		private bool m_Disposed;
		private string m_FileName;
		private bool m_Changed;
		private XmlDocument m_Doc = null;
		private string m_sectionName = "programSettings";

		public enum SettingsType { User, Machine };

		public ProgramSettings(SettingsType pType, string pCompanyName, string pApplicationName)
		{
			// name of settings file
			m_FileName = getFileName(pType, pCompanyName, pApplicationName);

			// create if it does not exist yet
			if (!File.Exists(m_FileName)) createNewFile();

			// now load the file
			m_Doc = new XmlDocument();
			m_Doc.Load(m_FileName);
		}

		public string ParatextProjectDirectory
		{
			get { return getStringSetting("paratextProjectDirectory", string.Empty); }
			set { setStringSetting("paratextProjectDirectory", value); }
		}

		public string Door43RepositoryDirectory
		{
			get { return getStringSetting("door43RepositoryDirectory", string.Empty); }
			set { setStringSetting("door43RepositoryDirectory", value); }
		}

		private string getFileName(SettingsType pType, string pCompanyName, string pApplicationName)
		{
			string dirName;
			if (pType == SettingsType.User)
			{
				dirName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			}
			else
			{
				dirName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			}

			if ((pCompanyName != null) && (pCompanyName.Length > 0))
			{
				dirName = Path.Combine(dirName, pCompanyName);
			}

			if ((pApplicationName != null) && (pApplicationName.Length > 0))
			{
				dirName = Path.Combine(dirName, pApplicationName);
			}

			if (pType == SettingsType.User)
			{
				return Path.Combine(dirName, "UserSettings.config");
			}
			else
			{
				return Path.Combine(dirName, "AppSettings.config");
			}
		}

		private void createNewFile()
		{
			XmlDocument doc = new XmlDocument();

			// root element
			doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
				"<configuration>" +
				"</configuration>");

			// sections
			doc.DocumentElement.AppendChild(doc.CreateNode(XmlNodeType.Element, "connectionStrings", ""));

			// check for directory
			FileInfo fi = new FileInfo(m_FileName);
			if (!Directory.Exists(fi.DirectoryName))
			{
				Directory.CreateDirectory(fi.DirectoryName);
			}

			// write to disk
			doc.Save(m_FileName);
		}

		private XmlNode getSection(string pSectionName)
		{
			foreach (XmlNode node in m_Doc.DocumentElement.ChildNodes)
			{
				if (node.Name.CompareTo(pSectionName) == 0)
				{
					return node;
				}
			}

			return null;
		}

		private string getStringSetting(string pSettingName)
		{
			XmlNode section = getSection(m_sectionName);

			// check for missing section
			if (section == null) return null;

			foreach (XmlNode node in section.ChildNodes)
			{
				if (node.Name.CompareTo(pSettingName) == 0)
				{
					return node.Attributes.GetNamedItem("value").Value;
				}
			}

			// if you are here, it was not found
			return null;
		}

		private string getStringSetting(string pSettingName, string pDefault)
		{
			string s = getStringSetting(pSettingName);

			if (s == null) return pDefault; else return s;
		}

		private decimal getNumericSetting(string pSettingName)
		{
			string tempVal = getStringSetting(pSettingName);

			if (tempVal == null)
				return (decimal)0;
			else
				return decimal.Parse(tempVal);
		}

		private void setStringSetting(string pSettingName, string pValue)
		{
			XmlNode section = getSection(m_sectionName);
			bool exists = false;

			// check for missing section
			if (section == null)
			{
				section = m_Doc.DocumentElement.AppendChild(m_Doc.CreateNode(XmlNodeType.Element, m_sectionName, ""));
			}

			// set the value, if it exists
			foreach (XmlNode node in section.ChildNodes)
			{
				if (node.Name.CompareTo(pSettingName) == 0)
				{
					exists = true;
					m_Changed = true;

					bool nodeExists = false;
					foreach (XmlNode attr in node.Attributes)
					{
						if (attr.Name.CompareTo("value") == 0)
						{
							nodeExists = true;
							attr.Value = pValue;
						}
					}

					if (!nodeExists)
					{
						XmlNode attr = m_Doc.CreateNode(XmlNodeType.Attribute, "value", "");
						attr.Value = pValue;
						node.Attributes.SetNamedItem(attr);
					}
				}
			}

			// add the setting if not found
			if (!exists)
			{
				XmlNode node = section.AppendChild(m_Doc.CreateNode(XmlNodeType.Element, pSettingName, ""));
				XmlNode attr = m_Doc.CreateNode(XmlNodeType.Attribute, "value", "");
				attr.Value = pValue;
				node.Attributes.SetNamedItem(attr);

				m_Changed = true;
			}

		}

		public void setNumericSetting(string pSettingName, decimal pValue)
		{
			setStringSetting(pSettingName, pValue.ToString());
		}

		public void Save()
		{
			if (m_Changed)
			{
				m_Doc.Save(m_FileName);

				m_Changed = false;
			}
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool pDisposing)
		{
			if (!m_Disposed)
			{
				if (pDisposing)
				{
					// code for managed objects
				}

				Save();
			}
			m_Disposed = true;
		}
	}
}
