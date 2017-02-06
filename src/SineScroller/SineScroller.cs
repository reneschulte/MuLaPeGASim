using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SineScroller
{
	/// <summary>
	/// Description: A Usercontrol which represents a sine curve Scrolltext. 
	/// Uses double buffering and a xy-Value-Table, which is calculated before scrolling. 
	/// The scrollling could be paused if a mousebutton is pressed over the Scrolltext.
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	[ToolboxBitmap(typeof(SineScroller), "toolboxBmp.bmp")]
	public class SineScroller : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// The text which should be scrolled; default -> "Hello World!"
		/// </summary>
		[Description("The text which should be scrolled"), Category("Appearance")]
		public string textProp
		{
			get { return text; }
			set
			{
				if(value!=null)
				{
					text = value;
					chars = new string[text.Length];
					for(int i=0; i<text.Length; i++)
						chars[i] = Convert.ToString(text[i]);
					computeScrollTable();
				}
			}
		}
		/// <summary>
		/// The text which should be scrolled; default -> "Hello World!"
		/// </summary>
		protected string text;

		/// <summary>
		/// The string Array for the text -> faster drawing of each character
		/// </summary>
		protected string[] chars;

		/// <summary>
		/// The Width of the whole text in px; textwidth = font.Size * text.Length
		/// </summary>
		protected float textwidth;

		/// <summary>
		/// The Font for drawing; default -> Sans serif, Size 10
		/// </summary>
		[Description("The Font for drawing"), Category("Appearance")]
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				fontWidth = value.Size + charSpaceOffset;
				computeScrollTable();
			}
		}

		/// <summary>
		/// The space between two characters of the current font; fontWidth = font.Size + charSpaceOffset; default -> 3.5
		/// </summary>
		protected float fontWidth;

		/// <summary>
		/// The offset for the space between two characters of the current font; fontWidth = font.Size + charSpaceOffset; default -> 3.5
		/// </summary>
		[Description("The offset for the space between two characters of the current font; fontWidth = font.Size + charSpaceOffset"), Category("Appearance")]
		public float charSpaceOffsetProp
		{
			get { return charSpaceOffset; }
			set { charSpaceOffset = value; }
		}
		/// <summary>
		/// The offset for the space between two characters of the current font; default -> fontWidth = font.Size + charSpaceOffset;
		/// </summary>
		protected float charSpaceOffset;

		/// <summary>
		/// The Brush for drawing; default -> SolidBrush(Color.Black);
		/// this property could only be set programmatically
		/// </summary>
		[Description("The Brush for drawing. This property could only be set programmatically"), Category("Appearance"), Browsable(false)]
		public Brush brushProp
		{
			get { return brush; }
			set { if(value!=null) brush = value; }
		}
		/// <summary>
		/// The Brush for drawing; default -> SolidBrush(Color.Black);
		/// this property could only be set programmatically
		/// </summary>
		protected Brush brush;

		/// <summary>
		/// The Color of the current brush;
		/// if the brush is an instance of a LinearGradientBrush: LinearGradientBrush.LinearColors = new Color[]{value, value};
		/// if the brush is an instance of a PathGradientBrush: PathGradientBrush.CenterColor = value
		/// </summary>
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set
			{
				base.ForeColor = value;
				if(brush.GetType() == typeof(System.Drawing.SolidBrush))
					(brush as System.Drawing.SolidBrush).Color = value;
				else if(brush.GetType() == typeof(System.Drawing.Drawing2D.LinearGradientBrush))
					(brush as System.Drawing.Drawing2D.LinearGradientBrush).LinearColors = new Color[]{value, value};
				else if(brush.GetType() == typeof(System.Drawing.Drawing2D.PathGradientBrush))
					(brush as System.Drawing.Drawing2D.PathGradientBrush).CenterColor = value;
			}
		}


		/// <summary>
		/// The distance between the computed points; 
		/// How many pixels the text will be moved to the left each tick; default -> 5; 
		/// dx have to be >= 0 
		/// </summary>
		[Description("The distance between the computed points"), Category("Sine")]
		public float dxProp
		{
			get { return dx; }
			set
			{
				if(dx >= 0)
				{
					dx = value;
					computeScrollTable();
				}
			}
		}
		/// <summary>
		/// How many pixels the text will be moved to the left each tick; default -> 5;
		/// dx have to be >= 0
		/// </summary>
		protected float dx;

		/// <summary>
		/// The frequency of the sine curve; default -> 0.5;
		/// </summary>
		[Description("The frequency of the sine curve"), Category("Sine")]
		public float frequencyProp
		{
			get { return freq; }
			set
			{
				freq = value;
				computeScrollTable();
			}
		}
		/// <summary>
		/// The frequency of the sine curve; default -> 0.5;
		/// </summary>
		protected float freq;

		/// <summary>
		/// The amplitude of the sine curve; default -> 4.5;
		/// </summary>
		[Description("The amplitude of the sine curve"), Category("Sine")]
		public float amplitudeProp
		{
			get { return amp; }
			set
			{
				amp = value;
				computeScrollTable();
			}
		}
		/// <summary>
		/// The amplitude of the sine curve; default -> 4.5;
		/// </summary>
		protected float amp;

		/// <summary>
		/// The offset for the y-Values of the sine curve; default -> 0;
		/// </summary>
		[Description("The offset for the y-Values of the sine curve"), Category("Sine")]
		public float yOffsetProp
		{
			get { return yOff; }
			set
			{
				yOff = value;
				computeScrollTable();
			}
		}
		/// <summary>
		/// The offset for the y-Values of the sine curve; default -> 0;
		/// </summary>
		protected float yOff;

		/// <summary>
		/// The vector containing the absolute x and y values for the sine curve
		/// </summary>
		protected PointF[] scrollTab;

		/// <summary>
		/// Used as a Index for the Scrolltable -> Incremented each time the timer_Tick Event is fired;
		/// set to 0 if the text is completly outside the scrollarea => start again
		/// </summary>
		protected int ticks;

		/// <summary>
		/// Stores the the value of the ticks variable if a mousebutton is pressed over the scroller
		/// </summary>
		protected int oldTick;

		/// <summary>
		/// If true, the scrolling is paused, if false the scroller is scrolling :o)
		/// </summary>
		[Description("If true, the scrolling is paused, if false the scroller is scrolling :o)"), Category("Timer")]
		public bool pauseScrollingProp
		{
			get { return pauseScrolling; }
			set
			{
				if(value == true)
					SineScroller_MouseDown(this, null);
				else if(value == false)
					SineScroller_MouseUp(this, null);
			}
		}

		/// <summary>
		/// Set to true if the user pressed a mousebutton down, set to false if the button was released
		/// </summary>
		protected bool pauseScrolling;

		/// <summary>
		/// The Timerevent for drawing the text
		/// </summary>
		protected Timer timer;

		/// <summary>
		/// The interval for the Timer in ms; default -> 40 ms == 25fps
		/// </summary>
		[Description("The interval for the Timer in ms"), Category("Timer")]
		public int intervalProp
		{
			get { return timer.Interval; }
			set { timer.Interval = value; }
		}

		/// <summary>
		/// needed Visual Studio Designervariable
		/// </summary>
		protected System.ComponentModel.Container components = null;

		/// <summary>
		/// Defaultconstructor: Init. the variables with the default values and starts the Timer
		/// </summary>
		public SineScroller()
		{
			// Dieser Aufruf ist für den Windows Form-Designer erforderlich.
			InitializeComponent();

			this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);

			dx = 5;
			freq = 0.5f;
			amp = 4.5f;
			yOff = 0;
			charSpaceOffset = 3.5f;
			textProp = "Hello World!";
			Font = new Font("Sans serif", 10f);
			brush = new SolidBrush(Color.Black);
			ForeColor = Color.Black;
			pauseScrolling = false;
			timer = new Timer();
			timer.Interval = 40;
			timer.Tick += new EventHandler(timer_Tick);
			computeScrollTable();
			timer.Start();
		}

		/// <summary>
		/// The Paint-method does the Drawing
		/// </summary>
		/// <param name="pea">the PaintEventArgs ;-)</param>
		protected override void OnPaint(PaintEventArgs pea)
		{
			base.OnPaint(pea);
			Graphics gfx = pea.Graphics;
           	gfx.Clear(this.BackColor);
           	gfx.TranslateTransform(0, (this.Height>>1)-(Font.Height*0.625f));
			int indexX = 0, indexY = 0;
			for(int i=0; i<chars.Length; i++)
			{
				indexX = indexY = ticks - i;
				if(pauseScrolling)
					indexX = oldTick - i;
				if(indexX >= scrollTab.Length)
					continue;
				else if(indexX < 0 || indexY < 0)
					break;

				gfx.DrawString(chars[i], Font, brush, scrollTab[indexX].X+i*fontWidth, scrollTab[indexY%scrollTab.Length].Y);
			}
			ticks++;
			if(indexX >= scrollTab.Length)
			{
				ticks = 0;
				if(pauseScrolling)
					ticks = oldTick;
			}
		}

		/// <summary>
		/// clear everything
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(timer != null)
					timer.Stop();
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Vom Komponenten-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		protected void InitializeComponent()
		{
			//
			// SineScroller
			//
			this.Name = "SineScroller";
			this.Size = new System.Drawing.Size(408, 32);
			this.Resize += new System.EventHandler(this.SineScroller_Resize);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SineScroller_MouseUp);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SineScroller_MouseDown);

		}
		#endregion


 		/// <summary>
		/// The Tick event handler method calls the paint method
		/// </summary>
		/// <param name="sender">Sender ;-)</param>
		/// <param name="e">EventArgs ;-)</param>
		protected void timer_Tick(object sender, EventArgs e)
		{
			this.Refresh();
		}

		/// <summary>
		/// Computes the PointF-vector for the scrolling with the absolutes x,y-Values
		/// </summary>
		protected void computeScrollTable()
		{
			if(dx == 0)
				dx = 1;
			textwidth = fontWidth * text.Length;
			scrollTab = new PointF[(int)Math.Round((float)(this.Width+textwidth)/dx, 0)];
			for(int i=0; i<scrollTab.Length; i++)
			{
				scrollTab[i].X = this.Width - ((i+1)*dx);
				scrollTab[i].Y = (float)Math.Sin((i+1)*freq)*amp+yOff;
			}
			return;
		}

		/// <summary>
		/// If the size has been changed, the ScrollTable is computed with the new values
		/// </summary>
		/// <param name="sender">Sender ;-)</param>
		/// <param name="e">EventArgs ;-)</param>
		protected void SineScroller_Resize(object sender, System.EventArgs e)
		{
			computeScrollTable();
		}

		/// <summary>
		/// Pause the scrolling
		/// </summary>
		/// <param name="sender">Sender ;-)</param>
		/// <param name="e">EventArgs ;-)</param>
		protected void SineScroller_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pauseScrolling = true;
			oldTick = ticks;
		}

		/// <summary>
		/// Resume the scrolling
		/// </summary>
		/// <param name="sender">Sender ;-)</param>
		/// <param name="e">EventArgs ;-)</param>
		protected void SineScroller_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pauseScrolling = false;
			ticks = oldTick;
		}
	}
}