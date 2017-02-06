using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MuLaPeGASim
{
	/// <summary>
	/// Description: Simple Dialog Box for showing information about the authors
	/// Author: Rene Schulte
	/// Version: 0.2
	/// Last recent Update: 23.10.2004
	/// </summary>
	public class AboutForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel emailReneLink;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label compNameLab;
		private System.Windows.Forms.Label userNameLab;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label osLab;
		private System.Windows.Forms.Label netLab;
		private System.Windows.Forms.LinkLabel emailTorstenLink;
		private System.Windows.Forms.Label ocrVersLab;
		private System.Windows.Forms.Label nnVersLab;
		private System.Windows.Forms.Label paVersLab;
		private System.Windows.Forms.Label sineVersLab;
        private ParamAdjuster.ParamAdjuster paramAdjuster1;
        private SineScroller.SineScroller sineScroller;
        private System.ComponentModel.IContainer components;

		public AboutForm()
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

			this.emailReneLink.Links.Add(0, emailReneLink.Text.Length, "mailto:s51122@informatik.htw-dresden.de?subject=Neural Network Simulator GUI");
			this.emailTorstenLink.Links.Add(0, emailTorstenLink.Text.Length, "mailto:s51070@informatik.htw-dresden.de?subject=Neural Network Simulator GUI");
			this.compNameLab.Text += Environment.MachineName;
			this.userNameLab.Text += Environment.UserName;
			this.osLab.Text += Environment.OSVersion.Platform.ToString() + " Version: " + Environment.OSVersion.Version.ToString();
			this.netLab.Text += Environment.Version.ToString();
			System.Reflection.AssemblyName asmn;
			try 
			{ 
				asmn = System.Reflection.AssemblyName.GetAssemblyName("MultilayerNet.dll"); 
				nnVersLab.Text = asmn.Name + ".dll Version: " + asmn.Version;
			}
			catch(System.IO.FileNotFoundException) {nnVersLab.Text = "MultilayerNet.dll Assembly not found!"; }
			try
			{
				asmn = System.Reflection.AssemblyName.GetAssemblyName("OCRPreProcessing.dll");
				ocrVersLab.Text = asmn.Name + ".dll Version: " + asmn.Version;
			}
			catch(System.IO.FileNotFoundException) {ocrVersLab.Text = "OCRPreProcessing.dll Assembly not found!"; }
			try
			{
				asmn = System.Reflection.AssemblyName.GetAssemblyName("ParamAdjuster.dll");
				paVersLab.Text = asmn.Name + ".dll Version: " + asmn.Version;
			}
			catch(System.IO.FileNotFoundException) {paVersLab.Text = "ParamAdjuster.dll Assembly not found!"; }
			try
			{
				asmn = System.Reflection.AssemblyName.GetAssemblyName("SineScroller.dll");
				sineVersLab.Text = asmn.Name + ".dll Version: " + asmn.Version;
			}
			catch(System.IO.FileNotFoundException) {sineVersLab.Text = "SineScroller.dll Assembly not found!"; }

            sineScroller.textProp = "Schulte & Baer Software Development proudly presents: --> " +
									Application.ProductName + " v" + Application.ProductVersion + " <--";
			PointF startPt = new PointF(sineScroller.Width*0.5f, 0);
			PointF endPt = new PointF(sineScroller.Width*0.5f, sineScroller.Height);
            sineScroller.brushProp = new System.Drawing.Drawing2D.LinearGradientBrush(startPt, endPt, Color.Black, Color.OrangeRed);
		}

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.emailReneLink = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.compNameLab = new System.Windows.Forms.Label();
            this.userNameLab = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.paramAdjuster1 = new ParamAdjuster.ParamAdjuster();
            this.emailTorstenLink = new System.Windows.Forms.LinkLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sineVersLab = new System.Windows.Forms.Label();
            this.paVersLab = new System.Windows.Forms.Label();
            this.ocrVersLab = new System.Windows.Forms.Label();
            this.nnVersLab = new System.Windows.Forms.Label();
            this.netLab = new System.Windows.Forms.Label();
            this.osLab = new System.Windows.Forms.Label();
            this.sineScroller = new SineScroller.SineScroller();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(38, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(218, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Rene Schulte";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(346, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(217, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "Torsten Bär";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 627);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "© 2004 Schulte, Bär";
            // 
            // emailReneLink
            // 
            this.emailReneLink.Location = new System.Drawing.Point(13, 249);
            this.emailReneLink.Name = "emailReneLink";
            this.emailReneLink.Size = new System.Drawing.Size(295, 35);
            this.emailReneLink.TabIndex = 8;
            this.emailReneLink.TabStop = true;
            this.emailReneLink.Text = "mulapegasim@rene-schulte.info";
            this.emailReneLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.emailReneLink_LinkClicked);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(27, 413);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(391, 214);
            this.label4.TabIndex = 10;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // compNameLab
            // 
            this.compNameLab.Location = new System.Drawing.Point(13, 23);
            this.compNameLab.Name = "compNameLab";
            this.compNameLab.Size = new System.Drawing.Size(576, 24);
            this.compNameLab.TabIndex = 11;
            this.compNameLab.Text = "Computername: ";
            // 
            // userNameLab
            // 
            this.userNameLab.Location = new System.Drawing.Point(13, 47);
            this.userNameLab.Name = "userNameLab";
            this.userNameLab.Size = new System.Drawing.Size(576, 23);
            this.userNameLab.TabIndex = 12;
            this.userNameLab.Text = "Username: ";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.paramAdjuster1);
            this.groupBox1.Controls.Add(this.emailReneLink);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.emailTorstenLink);
            this.groupBox1.Location = new System.Drawing.Point(26, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(392, 343);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "The authors";
            // 
            // paramAdjuster1
            // 
            this.paramAdjuster1.Location = new System.Drawing.Point(37, 22);
            this.paramAdjuster1.maxProp = 1F;
            this.paramAdjuster1.minProp = 0F;
            this.paramAdjuster1.Name = "paramAdjuster1";
            this.paramAdjuster1.paramNameProp = "Parameter";
            this.paramAdjuster1.roundingProp = 4;
            this.paramAdjuster1.Size = new System.Drawing.Size(8, 8);
            this.paramAdjuster1.stepProp = 0.1F;
            this.paramAdjuster1.TabIndex = 26;
            this.paramAdjuster1.valueProp = 0F;
            // 
            // emailTorstenLink
            // 
            this.emailTorstenLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.emailTorstenLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.emailTorstenLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailTorstenLink.Location = new System.Drawing.Point(13, 283);
            this.emailTorstenLink.Name = "emailTorstenLink";
            this.emailTorstenLink.Size = new System.Drawing.Size(294, 23);
            this.emailTorstenLink.TabIndex = 25;
            this.emailTorstenLink.TabStop = true;
            this.emailTorstenLink.Text = "mulapegasim@baer-torsten.de";
            this.emailTorstenLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.emailReneLink_LinkClicked);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.sineVersLab);
            this.groupBox2.Controls.Add(this.paVersLab);
            this.groupBox2.Controls.Add(this.ocrVersLab);
            this.groupBox2.Controls.Add(this.nnVersLab);
            this.groupBox2.Controls.Add(this.netLab);
            this.groupBox2.Controls.Add(this.osLab);
            this.groupBox2.Controls.Add(this.compNameLab);
            this.groupBox2.Controls.Add(this.userNameLab);
            this.groupBox2.Location = new System.Drawing.Point(26, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(652, 234);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Systeminformation";
            // 
            // sineVersLab
            // 
            this.sineVersLab.Location = new System.Drawing.Point(13, 199);
            this.sineVersLab.Name = "sineVersLab";
            this.sineVersLab.Size = new System.Drawing.Size(576, 23);
            this.sineVersLab.TabIndex = 18;
            this.sineVersLab.Text = "SineScroller.dll Version: ";
            // 
            // paVersLab
            // 
            this.paVersLab.Location = new System.Drawing.Point(13, 175);
            this.paVersLab.Name = "paVersLab";
            this.paVersLab.Size = new System.Drawing.Size(576, 24);
            this.paVersLab.TabIndex = 17;
            this.paVersLab.Text = "ParamAdjuster.dll Version: ";
            // 
            // ocrVersLab
            // 
            this.ocrVersLab.Location = new System.Drawing.Point(13, 152);
            this.ocrVersLab.Name = "ocrVersLab";
            this.ocrVersLab.Size = new System.Drawing.Size(576, 23);
            this.ocrVersLab.TabIndex = 16;
            this.ocrVersLab.Text = "OCRPreProcessing.dll Version: ";
            // 
            // nnVersLab
            // 
            this.nnVersLab.Location = new System.Drawing.Point(13, 129);
            this.nnVersLab.Name = "nnVersLab";
            this.nnVersLab.Size = new System.Drawing.Size(576, 23);
            this.nnVersLab.TabIndex = 15;
            this.nnVersLab.Text = "MultilayerNet.dll Version: ";
            // 
            // netLab
            // 
            this.netLab.Location = new System.Drawing.Point(13, 94);
            this.netLab.Name = "netLab";
            this.netLab.Size = new System.Drawing.Size(576, 23);
            this.netLab.TabIndex = 14;
            this.netLab.Text = ".NET Version: ";
            // 
            // osLab
            // 
            this.osLab.Location = new System.Drawing.Point(13, 70);
            this.osLab.Name = "osLab";
            this.osLab.Size = new System.Drawing.Size(576, 24);
            this.osLab.TabIndex = 13;
            this.osLab.Text = "Operating System: ";
            // 
            // sineScroller
            // 
            this.sineScroller.amplitudeProp = 4.5F;
            this.sineScroller.charSpaceOffsetProp = 3.5F;
            this.sineScroller.dxProp = 5F;
            this.sineScroller.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.sineScroller.ForeColor = System.Drawing.Color.Black;
            this.sineScroller.frequencyProp = 0.5F;
            this.sineScroller.intervalProp = 40;
            this.sineScroller.Location = new System.Drawing.Point(0, 0);
            this.sineScroller.Name = "sineScroller";
            this.sineScroller.pauseScrollingProp = false;
            this.sineScroller.Size = new System.Drawing.Size(408, 32);
            this.sineScroller.TabIndex = 0;
            this.sineScroller.textProp = "Hello World!";
            this.sineScroller.yOffsetProp = 0F;
            // 
            // AboutForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(434, 662);
            this.Controls.Add(this.sineScroller);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AboutForm";
            this.Text = ".:About:.";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		private void emailReneLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Link.LinkData as string);
		}
	}
}