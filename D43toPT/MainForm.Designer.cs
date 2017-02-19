namespace D43toPT
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.label1 = new System.Windows.Forms.Label();
			this.txtParatextDirectory = new System.Windows.Forms.TextBox();
			this.btnSelectParatext = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtDoor43Directory = new System.Windows.Forms.TextBox();
			this.btnSelectDoor43 = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnConvertToParatext = new System.Windows.Forms.Button();
			this.btnConvertToDoor43 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(48, 107);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(172, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "Paratext project directory";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtParatextDirectory
			// 
			this.txtParatextDirectory.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtParatextDirectory.Location = new System.Drawing.Point(226, 104);
			this.txtParatextDirectory.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.txtParatextDirectory.Name = "txtParatextDirectory";
			this.txtParatextDirectory.Size = new System.Drawing.Size(372, 24);
			this.txtParatextDirectory.TabIndex = 1;
			// 
			// btnSelectParatext
			// 
			this.btnSelectParatext.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnSelectParatext.Location = new System.Drawing.Point(604, 102);
			this.btnSelectParatext.Name = "btnSelectParatext";
			this.btnSelectParatext.Size = new System.Drawing.Size(75, 27);
			this.btnSelectParatext.TabIndex = 2;
			this.btnSelectParatext.Text = "Select";
			this.btnSelectParatext.UseVisualStyleBackColor = true;
			this.btnSelectParatext.Click += new System.EventHandler(this.btnSelectParatext_Click);
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(48, 143);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(168, 18);
			this.label2.TabIndex = 3;
			this.label2.Text = "Local Door43 repository";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtDoor43Directory
			// 
			this.txtDoor43Directory.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtDoor43Directory.Location = new System.Drawing.Point(226, 140);
			this.txtDoor43Directory.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.txtDoor43Directory.Name = "txtDoor43Directory";
			this.txtDoor43Directory.Size = new System.Drawing.Size(372, 24);
			this.txtDoor43Directory.TabIndex = 4;
			// 
			// btnSelectDoor43
			// 
			this.btnSelectDoor43.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnSelectDoor43.Location = new System.Drawing.Point(604, 138);
			this.btnSelectDoor43.Name = "btnSelectDoor43";
			this.btnSelectDoor43.Size = new System.Drawing.Size(75, 27);
			this.btnSelectDoor43.TabIndex = 5;
			this.btnSelectDoor43.Text = "Select";
			this.btnSelectDoor43.UseVisualStyleBackColor = true;
			this.btnSelectDoor43.Click += new System.EventHandler(this.btnSelectDoor43_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnSelectDoor43, 3, 3);
			this.tableLayoutPanel1.Controls.Add(this.label2, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.btnSelectParatext, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtDoor43Directory, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtParatextDirectory, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnConvertToParatext, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.btnConvertToDoor43, 2, 5);
			this.tableLayoutPanel1.Controls.Add(this.label3, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label4, 1, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(727, 516);
			this.tableLayoutPanel1.TabIndex = 6;
			// 
			// btnConvertToParatext
			// 
			this.btnConvertToParatext.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnConvertToParatext.AutoSize = true;
			this.btnConvertToParatext.Location = new System.Drawing.Point(226, 182);
			this.btnConvertToParatext.Margin = new System.Windows.Forms.Padding(3, 12, 3, 6);
			this.btnConvertToParatext.Name = "btnConvertToParatext";
			this.btnConvertToParatext.Size = new System.Drawing.Size(145, 28);
			this.btnConvertToParatext.TabIndex = 6;
			this.btnConvertToParatext.Text = "Convert to Paratext";
			this.btnConvertToParatext.UseVisualStyleBackColor = true;
			this.btnConvertToParatext.Click += new System.EventHandler(this.btnConvertToParatext_Click);
			// 
			// btnConvertToDoor43
			// 
			this.btnConvertToDoor43.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnConvertToDoor43.AutoSize = true;
			this.btnConvertToDoor43.Location = new System.Drawing.Point(226, 228);
			this.btnConvertToDoor43.Margin = new System.Windows.Forms.Padding(3, 12, 3, 6);
			this.btnConvertToDoor43.Name = "btnConvertToDoor43";
			this.btnConvertToDoor43.Size = new System.Drawing.Size(141, 28);
			this.btnConvertToDoor43.TabIndex = 7;
			this.btnConvertToDoor43.Text = "Convert to Door43";
			this.btnConvertToDoor43.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.label3, 3);
			this.label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.label3.Location = new System.Drawing.Point(48, 42);
			this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(631, 43);
			this.label3.TabIndex = 8;
			this.label3.Text = "Before you begin, make sure you have a Paratext project setup for this procedure." +
    " If you don\'t, you should just create an empty project, the default settings are" +
    " fine.";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label4, 3);
			this.label4.Dock = System.Windows.Forms.DockStyle.Top;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(48, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(631, 18);
			this.label4.TabIndex = 9;
			this.label4.Text = "* Currently, this has only been tested with Paratext 8";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(751, 540);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "Door43 to Paratext";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtParatextDirectory;
		private System.Windows.Forms.Button btnSelectParatext;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtDoor43Directory;
		private System.Windows.Forms.Button btnSelectDoor43;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnConvertToParatext;
		private System.Windows.Forms.Button btnConvertToDoor43;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
	}
}

