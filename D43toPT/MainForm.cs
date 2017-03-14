using D43toPT.Door43;
using D43toPT.Paratext;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace D43toPT
{
	public partial class MainForm : Form
	{
		private ProgramSettings settings;

		public MainForm()
		{
			InitializeComponent();

			settings = new ProgramSettings(ProgramSettings.SettingsType.User, "", "D43toPT");

			// restore Paratext project directory
			restoreDirectorySetting(settings.ParatextProjectDirectory, txtParatextDirectory, @"C:\My Paratext 8 Projects");

			// restore Door43 repository directory
			restoreDirectorySetting(settings.Door43RepositoryDirectory, txtDoor43Directory, string.Empty);
		}

		private void btnSelectParatext_Click(object sender, System.EventArgs e)
		{
			using (var dlg = new FolderBrowserDialog())
			{
				var testDir = txtParatextDirectory.Text;
				if (Directory.Exists(testDir))
					dlg.SelectedPath = testDir;

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					txtParatextDirectory.Text = dlg.SelectedPath;
				}
			}
		}

		private void btnSelectDoor43_Click(object sender, System.EventArgs e)
		{
			using (var dlg = new FolderBrowserDialog())
			{
				var testDir = txtDoor43Directory.Text;
				if (Directory.Exists(testDir))
					dlg.SelectedPath = testDir;

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					txtDoor43Directory.Text = dlg.SelectedPath;
				}
			}
		}

		private void restoreDirectorySetting(string settingValue, TextBox control, string defaultValue)
		{
			if (Directory.Exists(settingValue))
			{
				control.Text = settingValue;
			}
			else if (Directory.Exists(defaultValue))
			{
				control.Text = defaultValue;
			}
		}

		private void btnConvertToParatext_Click(object sender, System.EventArgs e)
		{
			// make sure the Paratext and Door43 directories exists
			if (!checkDirectories())
				return;

			// warn that files in the Paratext directory will be removed and replaced
			var msgResult = MessageBox.Show(string.Format("Files will be deleted from {0}. Do you want to continue?", txtParatextDirectory.Text), "Confirm Delete Files", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			if (msgResult == DialogResult.No)
				return;

			// first, check for English ULB or UDB
			if (isEnglishFormat())
			{
				try
				{
					exportEnglishToParatext(txtDoor43Directory.Text, txtParatextDirectory.Text);
				}
				finally
				{
					lblStatus.Text = "Ready";
				}
			}
			else if (isPhilsFormat())
			{
				try
				{
					exportPhilToParatext(txtDoor43Directory.Text, txtParatextDirectory.Text);
				}
				finally
				{
					lblStatus.Text = "Ready";
				}
			}
            else
            {
                try
                {
                    exportDirectoryToParatext(txtDoor43Directory.Text, txtParatextDirectory.Text);
                }
                finally
                {
                    lblStatus.Text = "Ready";
                }
            }

		}

		private void exportEnglishToParatext(string sourceDirectory, string targetDirectory)
		{
			// get Paratext settings
			var paratextSettings = new Paratext8ProjectSettings(targetDirectory);
			var projectName = paratextSettings.GetValue("Name");
			var booksPresent = new int[123];
			

			// walk through the directories in the repository
			foreach (var sourceDir in Directory.GetDirectories(sourceDirectory))
			{
				var di = new DirectoryInfo(sourceDir);

				var bookDir = di.Name;

				// skip .git and .github
				if (bookDir.StartsWith("."))
					continue;

				// all usfm directories are in this format, "01-GEN"
				var pos = bookDir.IndexOf("-");
				if (pos < 1)
					continue;

				int bookNum;
				var success = int.TryParse(bookDir.Substring(0, pos), out bookNum);


				// check the book number
				if (!success || bookNum == 0 || bookNum == 40 || bookNum > 67)
					continue;

				// update BooksPresent
				if (bookNum > 40)
					bookNum--;

				bookNum--;
				booksPresent[bookNum] = 1;

				// book file name
				var bookParts = bookDir.Split('-');
				var bookName = paratextSettings.UsfmFileName(bookParts[0], bookParts[1]);
				lblStatus.Text = "Exporting " + bookName;
				Application.DoEvents();

				// loop through the usfm files
				var usfmFiles = Directory.GetFiles(sourceDir, "*.usfm");
				Array.Sort(usfmFiles, StringComparer.InvariantCulture);
				var usfm = string.Empty;
				foreach (var usfmFile in usfmFiles)
				{
					usfm += Environment.NewLine + File.ReadAllText(usfmFile);
				}

				// remove \s5 tags
				usfm = usfm.Replace(@"\s5", "");

				// remove extra white space
				usfm = usfm.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine);
				usfm = usfm.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine);
				usfm = usfm.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
				usfm = usfm.TrimStart();

				// write the usfm file
				if (string.IsNullOrEmpty(usfm))
					continue;

				File.WriteAllText(Path.Combine(targetDirectory, bookName), usfm);
			}

			// update Settings.xml
			paratextSettings.SetValue("BooksPresent", string.Join("", booksPresent));
			paratextSettings.Save();

			// finished
			MessageBox.Show("Finished.");
		}

		private void exportEnglishToDoor43(string sourceDirectory, string targetDirectory)
		{
			// get Paratext settings
			var paratextSettings = new Paratext8ProjectSettings(sourceDirectory);

			foreach (var usfmFile in Directory.GetFiles(sourceDirectory, "*" + paratextSettings.UsfmFileSuffix))
			{
				// get the book number and ID
				var fi = new FileInfo(usfmFile);
				var kvp = paratextSettings.GetNumberAndID(fi.Name);
				if (string.IsNullOrEmpty(kvp.Key))
					continue;
				var padding = kvp.Value == "PSA" ? 3 : 2;
				lblStatus.Text = "Exporting " + kvp.Key + "-" + kvp.Value;
				Application.DoEvents();

				Chunks chunks = chkChunkMarkers.Checked ? Resources.Resources.GetChunksV3(kvp.Value) : null;

				// door43 output directory
				var outDir = Path.Combine(targetDirectory, kvp.Key + "-" + kvp.Value);

				// split into chapters
				var chapterRegex = new Regex(@"(\\c[\u00A0\s][0-9]+(?:\s*[\r\n]+))");
				var chapters = chapterRegex.Split(File.ReadAllText(usfmFile));
				var chapterText = string.Empty;
				var chapterNum = 0;

				for (var i = 0; i < chapters.Length; i++)
				{
					var part = chapters[i];
					if (part.StartsWith(@"\c"))
					{
						// write the file
						writeDoor43File(chapterNum, chapterText, outDir, padding, chunks);

						// reset for next chapter
						chapterText = part;
						chapterNum = int.Parse(part.Substring(3));
					}
					else
					{
						chapterText += part;
					}
				}

				// write the last chapter
				if (!string.IsNullOrEmpty(chapterText))
					writeDoor43File(chapterNum, chapterText, outDir, padding, chunks);
			}

			// finished
			MessageBox.Show("Finished.");
		}

		private void exportPhilToParatext(string sourceDirectory, string targetDirectory)
		{
            exportDirectoryToParatext(Path.Combine(sourceDirectory, "usfm"), targetDirectory);
		}

        private void exportDirectoryToParatext(string sourceDirectory, string targetDirectory)
        {
            // get Paratext settings
            var paratextSettings = new Paratext8ProjectSettings(targetDirectory);
            var projectName = paratextSettings.GetValue("Name");
            var booksPresent = new int[123];

            // get book information
            var bookData = Resources.Resources.GetBookData();

            // copy source usfm files
            foreach (var fileName in Directory.EnumerateFiles(sourceDirectory).Where(filename => filename.ToLower().EndsWith("sfm")))
            {
                // read the source file
                var usfm = File.ReadAllText(fileName);

                // get the book id and number for the Paratext file name
                if (!usfm.StartsWith(@"\id "))
                {
                    MessageBox.Show(string.Format("Not a valid USFM file: {0}", fileName));
                    return;
                }

                var bookID = usfm.Substring(4, 3);

                // update books present 
                var bookNum = int.Parse(bookData[bookID][1]);

                if (bookNum > 40)
                    bookNum--;

                bookNum--;
                booksPresent[bookNum] = 1;

                // get paratext file name
                var bookName = paratextSettings.UsfmFileName(bookData[bookID][1], bookID);
                lblStatus.Text = "Exporting " + bookName;
                Application.DoEvents();

                // clean usfm
                usfm = cleanUsfm(usfm);

                // write the usfm file
                if (string.IsNullOrEmpty(usfm))
                    continue;

                File.WriteAllText(Path.Combine(targetDirectory, bookName), usfm);
            }

            // update Settings.xml
            paratextSettings.SetValue("BooksPresent", string.Join("", booksPresent));
            paratextSettings.Save();

            // finished
            MessageBox.Show("Finished.");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// save settings
			if (!string.IsNullOrEmpty(txtParatextDirectory.Text))
				settings.ParatextProjectDirectory = txtParatextDirectory.Text;

			if (!string.IsNullOrEmpty(txtDoor43Directory.Text))
				settings.Door43RepositoryDirectory = txtDoor43Directory.Text;

			settings.Save();
		}

		private void btnConvertToDoor43_Click(object sender, EventArgs e)
		{
			// make sure the Paratext and Door43 directories exists
			if (!checkDirectories())
				return;

			// warn that files in the Door43 directory will be removed and replaced
			var msgResult = MessageBox.Show(string.Format("Files will be deleted from {0}. Do you want to continue?", txtDoor43Directory.Text), "Confirm Delete Files", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			if (msgResult == DialogResult.No)
				return;

			// first, check for English ULB or UDB
			if (isEnglishFormat())
			{
				try
				{
					exportEnglishToDoor43(txtParatextDirectory.Text, txtDoor43Directory.Text);
				}
				finally
				{
					lblStatus.Text = "Ready";
				}
			}
		}

		private bool isEnglishFormat()
		{
			var door43Dir = Path.Combine(txtDoor43Directory.Text, "41-MAT");
			if (Directory.Exists(door43Dir))
			{
				if (File.Exists(Path.Combine(door43Dir, "00.usfm")) && File.Exists(Path.Combine(door43Dir, "01.usfm")))
				{
					return true;
				}
			}

			return false;
		}

		private bool isPhilsFormat()
		{
			var door43Dir = Path.Combine(txtDoor43Directory.Text, "usfm");
			if (Directory.Exists(door43Dir))
			{
				if (File.Exists(Path.Combine(txtDoor43Directory.Text, "meta.json")))
				{
					return true;
				}
			}

			return false;
		}

		private bool checkDirectories()
		{
			// make sure the Door43 directory exists
			var door43Dir = txtDoor43Directory.Text;
			if (string.IsNullOrEmpty(door43Dir))
			{
				MessageBox.Show("The Door43 repository was not specified.");
				return false;
			}
			else if (!Directory.Exists(door43Dir))
			{
				MessageBox.Show("The selected local Door43 repository does not exist.");
				return false;
			}

			// make sure the Paratext directory exits
			var paratextDir = txtParatextDirectory.Text;
			if (string.IsNullOrEmpty(paratextDir))
			{
				MessageBox.Show("The Paratext project directory was not specified.");
				return false;
			}
			else if (!Directory.Exists(paratextDir))
			{
				MessageBox.Show("The selected Paratext project directory does not exist.");
				return false;
			}

			return true;
		}

		private void writeDoor43File(int chapterNum, string chapterText, string outDir, int padding, Chunks chunks)
		{
			// build file name
			var outFilePath = Path.Combine(outDir, chapterNum.ToString().PadLeft(padding, '0') + ".usfm");

			if (!string.IsNullOrEmpty(chapterText))
			{
				// apply chunks
				if (chapterNum == 0)
					chapterText = chapterText.Trim();
				else if (chunks != null)
					chapterText = chunks.ApplyChunksToChapter(chapterNum, chapterText);
			}

			// make sure file ends with new line
			if ((chapterNum > 0) && !chapterText.EndsWith(Environment.NewLine))
				chapterText += Environment.NewLine;

			// write file
			File.WriteAllText(outFilePath, chapterText);
		}

		private string cleanUsfm(string usfm)
		{
			// remove \s5 tags
			usfm = usfm.Replace(@"\s5", "");

			// remove extra white space
			usfm = usfm.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine);
			usfm = usfm.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine);
			usfm = usfm.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
			return usfm.TrimStart();
		}
	}
}
