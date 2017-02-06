using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MultilayerNet;

namespace MuLaPeGASim
{
	/// <summary>
	/// Zusammenfassung für ErrorMessageBox.
	/// </summary>
	public class ErrorMessageBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox errorTB;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel emailLL;
		private System.Windows.Forms.Label emailLab;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ErrorMessageBox(Exception exc)
		{
			try
			{
				// Erforderlich für die Windows Form-Designerunterstützung
				InitializeComponent();

				emailLab.Text = "If you would like to send an Email with the information from the"
					+ "\"" + FileManager.getInstance().LOGFILE_NAME_PROP + "\" file to the developers, click on the link below.";

				if(exc != null)
					errorTB.Text = exc.ToString();
				else
					errorTB.Text = "No Exception to handle -> Exception == null !";

				string mailLink = "mailto:s51122@informatik.htw-dresden.de,s51070@informatik.htw-dresden.de"
		/*			+ "?subject=Neural Network Simulator GUI - Error"
					+ "?body=Content of logfile: " + FileManager.LOGFILE_NAME_PROP + "%0A"
					+ "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + "%0A" + "%0A"
					+ FileManager.readErrorLogFile()  + "%0A"
					+ "$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" + "%0A"
					+ "Write your own failure description here:" + "%0A";

		*/			+ "?subject=" + Application.ProductName + " - Error"
					+ "&body=Please attach the logfile ('" + FileManager.getInstance().LOGFILE_NAME_PROP + "') to this Email." + "%0A"
					+ "You can find the logfile in the local program folder: " + Environment.CurrentDirectory + "%0A"
					+ "It is really important for the developers that you send the logfile with this Email. "
					+ "And to write a detailed failure description." + "%0A"
					+ "Thanks in advance for helping us to improve this program!";
				this.emailLL.Links.Add(0, emailLL.Text.Length, mailLink);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Problem initializing error MessageBox Controls!\n"
					+ "Exceptionmessage:\n"
					+ ex.Message);
			}
		}

		public ErrorMessageBox() : this(null) {}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.errorTB = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.emailLab = new System.Windows.Forms.Label();
			this.emailLL = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			//
			// errorTB
			//
			this.errorTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.errorTB.Location = new System.Drawing.Point(8, 32);
			this.errorTB.Multiline = true;
			this.errorTB.Name = "errorTB";
			this.errorTB.ReadOnly = true;
			this.errorTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.errorTB.Size = new System.Drawing.Size(862, 256);
			this.errorTB.TabIndex = 1;
			this.errorTB.Text = "";
			//
			// label1
			//
			this.label1.BackColor = System.Drawing.Color.White;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(368, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "An error with the following Exceptionmessage occured:";
			//
			// emailLab
			//
			this.emailLab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.emailLab.Location = new System.Drawing.Point(8, 296);
			this.emailLab.Name = "emailLab";
			this.emailLab.Size = new System.Drawing.Size(376, 32);
			this.emailLab.TabIndex = 3;
			//
			// emailLL
			//
			this.emailLL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.emailLL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.emailLL.Location = new System.Drawing.Point(8, 328);
			this.emailLL.Name = "emailLL";
			this.emailLL.Size = new System.Drawing.Size(528, 24);
			this.emailLL.TabIndex = 4;
			this.emailLL.TabStop = true;
			this.emailLL.Text = "Click here to send the error with your standard Emailclient";
			this.emailLL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.emailLL_LinkClicked);
			//
			// ErrorMessageBox
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(880, 350);
			this.Controls.Add(this.emailLL);
			this.Controls.Add(this.emailLab);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.errorTB);
			this.Name = "ErrorMessageBox";
			this.Text = ".:: E.R.R.O.R ::.";
			this.ResumeLayout(false);

		}
		#endregion

		private void emailLL_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start(e.Link.LinkData as string);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Problem starting Link (Emailclient?)! \n"
					+ "Exceptionmessage:\n"
					+ ex.Message);
			}
		}
	}
}