﻿using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace D43toPT.Paratext
{
	class Paratext8ProjectSettings
	{
		private XmlDocument m_xmlDocument;
		private string m_fileName;
		private string m_usfmFileFormat;
		private Regex m_nameAndID;

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

				m_usfmFileFormat = preForm + bookFmt + UsfmFileSuffix;
			}

			return string.Format(m_usfmFileFormat, bookNumber, bookId);
		}

		public string UsfmFileSuffix
		{
			get { return GetValue("FileNamePostPart").Trim(); }
		}

		public KeyValuePair<string, string> GetNumberAndID(string fileName)
		{
			if (m_nameAndID == null)
			{
				var bookPattern = GetValue("FileNameBookNameForm").Trim();
				bookPattern = bookPattern.Replace("41", @"(\d{2})").Replace("MAT", @"([\dA-Z]{3})");
				m_nameAndID = new Regex(bookPattern);
			}

			var match = m_nameAndID.Match(fileName);
			if (!match.Success)
				return new KeyValuePair<string, string>(string.Empty, string.Empty);

			return new KeyValuePair<string, string>(match.Groups[1].Value, match.Groups[2].Value);
		}
	}
}
