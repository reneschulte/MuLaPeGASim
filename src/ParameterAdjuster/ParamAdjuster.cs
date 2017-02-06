using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ParamAdjuster
{
	/// <summary>
	/// The delegate for the event when the slider has changed
	/// </summary>
	public delegate void valueHandler(Object sender, float newValue);

	/// <summary>
	/// Description: A set of control elements. Used for simulating an adjusting unit for float variables.
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	[ToolboxBitmap(typeof(ParamAdjuster), "toolboxBmp.bmp")]
	public class ParamAdjuster : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// The event which is thrown when the scrollbar value has been changed
		/// </summary>
		[Description("The event which is thrown when the scrollbar value has been changed")]
		public event valueHandler onNewValue;	
		
		/// <summary>
		/// The step in which the Adjuster could be adjusted -> default: 0.1
		/// </summary>
		[Description("The step of a value change"), Category("Data")]
		public float stepProp
		{
			get{ return step; }
			set
			{ 
				if(value>0) 
				{
					step = value;
					maxProp = valueMax;
					minProp = valueMin;
					valueProp = valueProp;
					paramNameProp = paramName;
				}
			}
		}
		/// <summary>
		/// The step in which the Adjuster could be adjusted -> default: 0.1
		/// </summary>
		protected float step = 0.1f;

		/// <summary>
		/// The minimum value for the adjuster -> default: 0.0
		/// </summary>
		[Description("The minimum value"), Category("Data")]
		public float minProp
		{
			get{ return valueMin; }
			set
			{ 
				if(value<valueMax) 
				{
					valueMin = value; 
					paramScrollBar.Minimum = (int)(Math.Round(valueMin/stepProp,0));
				}
			}
		}
		/// <summary>
		/// The minimum value for the adjuster -> default: 0.0
		/// </summary>
		protected float valueMin = 0.0f;

		/// <summary>
		/// The maximum value for the adjuster -> default: 1.0
		/// </summary>
		[Description("The maximum value"), Category("Data")]
		public float maxProp
		{
			get{ return valueMax; }
			set
			{ 
				if(value>valueMin) 
				{
					valueMax = value;
					paramScrollBar.Maximum = (int)Math.Round((valueMax - stepProp)/stepProp, 0) + paramScrollBar.LargeChange;	
				}
			}
		}
		/// <summary>
		/// The maximum value for the adjuster -> default: 1.0
		/// </summary>
		protected float valueMax = 1.0f;

/*		/// <summary>
		/// the initial value for the adjuster -> default: 0.5
		/// </summary>
		public float valueInitProp
		{
			get{ return valueInit; }
			set
			{ 
				if(value>=valueMin && value<=valueMax) 
				{
					valueInit = value;
					paramLabel.Text = this.paramName + ": " + Math.Round(valueInit, rounding).ToString();
					paramScrollBar.Value = (int)Math.Round(valueInit / valueStep, 0);
				}
			}
		}
		
*/
		/// <summary>
		/// The current value of the adjuster
		/// </summary>
		[Description("The current value"), Category("Data")]
		public float valueProp
		{
			get{ return (paramScrollBar.Value * step); }
			set
			{ 
				paramScrollBar.Value = (int)Math.Round(value / step, 0);
				paramLabel.Text = this.paramName + ": " + value.ToString();
			}
		}

		/// <summary>
		/// The name of the parameter which the adjuster represents -> default: "Paramter"
		/// </summary>
		[Description("The name of the parameter"), Category("Appearance")]
		public string paramNameProp
		{
			get{ return paramName; }
			set
			{ 
				paramName = value; 
				paramLabel.Text = paramName + ": " + Math.Round(paramScrollBar.Value*step, rounding).ToString();
			}
		}
		/// <summary>
		/// The name of the parameter which the adjuster represents -> default: "Paramter"
		/// </summary>
		protected string paramName = "Parameter";

		/// <summary>
		/// The number of digits the value is rounded -> default: 4
		/// </summary>
		[Description("The number of digits the value is rounded"), Category("Data")]
		public int roundingProp
		{
			get{ return rounding; }
			set{ if(value >= 0) rounding = value; }
		}
		/// <summary>
		/// The number of digits the value is rounded -> default: 4
		/// </summary>
		protected int rounding = 4;

		/// <summary>
		/// The Label :o)
		/// </summary>
		protected System.Windows.Forms.Label paramLabel;

		/// <summary>
		/// The Scrollbar :o)
		/// </summary>
		protected System.Windows.Forms.HScrollBar paramScrollBar;

		/// <summary>
		/// Needed Visual Studio Designervariable
		/// </summary>
		protected System.ComponentModel.Container components = null;

		/// <summary>
		/// Defaultconstructor
		/// </summary>
		public ParamAdjuster()
		{
			// Dieser Aufruf ist für den Windows Form-Designer erforderlich.
			InitializeComponent();
		}

		/// <summary>
		/// Constructor init. the Adjuster with the values
		/// </summary>
		/// <param name="val">the init. value</param>
		/// <param name="valMin">the minimum value for the adjuster</param>
		/// <param name="valMax">the maximum value for the adjuster</param>
		/// <param name="valStep">the step for the adjuster</param>
		/// <param name="paramName">the name of the parameter which the adjuster represents</param>
		public ParamAdjuster(float val, float valMin, float valMax, float valStep, string paramName) : this()
		{
			setParamAndCtrls(val, valMin, valMax, valStep, paramName);
		}

		/// <summary>
		/// Sets the values of the controls
		/// </summary>
		/// <param name="val">the init. value</param>
		/// <param name="valMin">the minimum value for the adjuster</param>
		/// <param name="valMax">the maximum value for the adjuster</param>
		/// <param name="valStep">the step for the adjuster</param>
		/// <param name="paramName">the name of the parameter which the adjuster represents</param>
		public void setParamAndCtrls(float val, float valMin, float valMax, float valStep, string paramName)
		{
			this.stepProp = valStep;			
			this.maxProp = valMax;
			this.minProp = valMin;
			this.valueProp = val;			
			this.paramNameProp = paramName;
		}

		/// <summary>
		/// Clear everything
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component-Designer generated code

		/// <summary>
		/// needed Visual Studio method to init. the controls / components
		/// don't change the method stub !!!
		/// </summary>
		private void InitializeComponent()
		{
			this.paramLabel = new System.Windows.Forms.Label();
			this.paramScrollBar = new System.Windows.Forms.HScrollBar();
			this.SuspendLayout();
			// 
			// paramLabel
			// 
			this.paramLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.paramLabel.AutoSize = true;
			this.paramLabel.Location = new System.Drawing.Point(0, 0);
			this.paramLabel.Name = "paramLabel";
			this.paramLabel.Size = new System.Drawing.Size(60, 16);
			this.paramLabel.TabIndex = 24;
			this.paramLabel.Text = "Parameter:";
			this.paramLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// paramScrollBar
			// 
			this.paramScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.paramScrollBar.Cursor = System.Windows.Forms.Cursors.NoMoveHoriz;
			this.paramScrollBar.Location = new System.Drawing.Point(120, 4);
			this.paramScrollBar.Name = "paramScrollBar";
			this.paramScrollBar.Size = new System.Drawing.Size(320, 8);
			this.paramScrollBar.TabIndex = 23;
			this.paramScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.paramScrollBar_Scroll);
			// 
			// ParamAdjuster
			// 
			this.Controls.Add(this.paramScrollBar);
			this.Controls.Add(this.paramLabel);
			this.Name = "ParamAdjuster";
			this.Size = new System.Drawing.Size(440, 16);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Handles changes of the scrollbar and fires a onNewValue event
		/// </summary>
		protected void paramScrollBar_Scroll(object sender, System.Windows.Forms.ScrollEventArgs sea)
		{
			float newValue = (float)Math.Round(sea.NewValue*step, rounding);
			paramLabel.Text = this.paramName + ": " + newValue.ToString();
			if(onNewValue != null)
				onNewValue(this, newValue);
		}
	}
}
