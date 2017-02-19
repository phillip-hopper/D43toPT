using System.IO;
using System.Xml;

namespace D43toPT
{
	class Paratext8ProjectSettings
	{
		private XmlDocument m_xmlDocument;
		private string m_fileName;
		private string m_usfmFileFormat;

		public Paratext8ProjectSettings(string projectDirectory)
		{
			m_fileName = Path.Combine(projectDirectory, "Settings.xml");
			m_xmlDocument = new XmlDocument();
			m_xmlDocument.PreserveWhitespace = true;
			m_xmlDocument.LoadXml(File.ReadAllText(m_fileName));
		}

		public string GetValue(string nodeName)
		{
			var scriptureText = m_xmlDocument.DocumentElement;
			var node = scriptureText.SelectSingleNode(nodeName);
			return node.InnerXml;
		}

		public void SetValue(string nodeName, string value)
		{
			var scriptureText = m_xmlDocument.DocumentElement;
			var node = scriptureText.SelectSingleNode(nodeName);
			node.InnerXml = value;
		}

		public void Save()
		{
			m_xmlDocument.Save(m_fileName);
		}

		public string UsfmFileName(string bookNumber, string bookId)
		{
			if (string.IsNullOrEmpty(m_usfmFileFormat))
			{
				var bookFmt = GetValue("FileNameBookNameForm").Trim();
				bookFmt = bookFmt.Replace("41", "{0}").Replace("MAT", "{1}");
				var preForm = GetValue("FileNamePrePart").Trim();
				var postForm = GetValue("FileNamePostPart").Trim();
				m_usfmFileFormat = preForm + bookFmt + postForm;
			}

			return string.Format(m_usfmFileFormat, bookNumber, bookId);
		}
	}
}
