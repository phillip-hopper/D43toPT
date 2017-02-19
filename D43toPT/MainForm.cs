using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

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
			// make sure a Paratext directory was selected
			if (string.IsNullOrEmpty(txtParatextDirectory.Text))
			{
				MessageBox.Show("The Paratext project directory was not specified.");
				return;
			}
			// make sure the Door43 directory exists
			if (string.IsNullOrEmpty(txtDoor43Directory.Text) || !Directory.Exists(txtDoor43Directory.Text))
			{
				MessageBox.Show("The selected local Door43 repository does not exist.");
				return;
			}

			// make sure the Paratext directory exits
			var paratextDir = txtParatextDirectory.Text;

			if (!Directory.Exists(paratextDir))
			{
				MessageBox.Show("The selected Paratext project directory does not exist.");
				return;
			}
			else
			{
				// warn that files in the Paratext directory will be removed and replaced
				var msgResult = MessageBox.Show(string.Format("Files will be deleted from {0}. Do you want to continue?", paratextDir), "Confirm Delete Files", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
				if (msgResult == DialogResult.No)
					return;
			}

			// make sure it contains USFM files
			// first, check for English ULB or UDB
			var door43Dir = Path.Combine(txtDoor43Directory.Text, "01-GEN");
			if (Directory.Exists(door43Dir))
			{
				if (File.Exists(Path.Combine(door43Dir, "01.usfm")))
				{
					importEnglishUlbUdb(txtDoor43Directory.Text, txtParatextDirectory.Text);
				}
			}
		}

		private void importEnglishUlbUdb(string sourceDirectory, string targetDirectory)
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

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// save settings
			if (!string.IsNullOrEmpty(txtParatextDirectory.Text))
				settings.ParatextProjectDirectory = txtParatextDirectory.Text;

			if (!string.IsNullOrEmpty(txtDoor43Directory.Text))
				settings.Door43RepositoryDirectory = txtDoor43Directory.Text;

			settings.Save();
		}
	}
}
