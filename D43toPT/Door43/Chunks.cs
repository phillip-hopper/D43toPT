using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace D43toPT.Door43
{
	class Chunks
	{
		public List<ChunkChapter> Chapters = new List<ChunkChapter>();

		public Chunks(JArray jsonBook)
		{
			Chapters = jsonBook.ToObject<List<ChunkChapter>>();
		}

		public string ApplyChunksToChapter(int chapterNumber, string usfm)
		{
			var previousLine = string.Empty;
			var marker = Environment.NewLine + @"\s5";
			var oldLines = usfm.Split(new [] { "\r\n", "\n" }, StringSplitOptions.None);
			var newLines = new List<string>();
			var chunkIdx = 0;
			var q_alone_re = new Regex(@"^\\q[0-9a-z]*\s*$");
			var firstVerses = Chapters.First(c => c.chapter == chapterNumber).first_verses;

			// insert the first marker
			newLines.Add(Environment.NewLine + marker);
			chunkIdx++;

			for (var i = 0; i < oldLines.Length; i++)
			{
				var line = oldLines[i].Trim();
				if (string.IsNullOrEmpty(line))
					continue;

				if (chunkIdx < firstVerses.Length)
				{
					var pattern = string.Format(@"\\v[\u00A0\s]{0}[\s-]", firstVerses[chunkIdx]);
					if (Regex.IsMatch(line, pattern))
					{
						if (previousLine == @"\p")
						{
							newLines.Insert(newLines.Count - 1, marker);
						}
						else if (q_alone_re.IsMatch(previousLine))
						{
							newLines.Insert(newLines.Count - 1, marker);
						}
						else
						{
							newLines.Add(marker);
						}

						chunkIdx++;
					}
				}

				newLines.Add(line);
				previousLine = line;
			}

			return string.Join(Environment.NewLine, newLines);
		}
	}

	class ChunkChapter
	{
		public int chapter;
		public int[] first_verses;
	}
}
