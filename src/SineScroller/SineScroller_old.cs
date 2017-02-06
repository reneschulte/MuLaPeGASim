using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

// TODO: usercontrol kommentare suchen für eigenschaften
// TODO: frequency wirkl. Frequenz?

namespace SineScroller
{
	/// <summary>
	/// Description: A Usercontrol which represents a sine curve Scrolltext.
	/// Uses double buffering and a xy-Value-Table, which is calculated before scrolling.
	/// The scrollling could be paused if a mousebutton is pressed over the Scrolltext.
	/// Author: Rene Schulte
	/// Version: 1.0
	/// Last recent Update: 17.10.2004
	/// </summary>
	public class SineScroller : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// The text which should be scrolled; default -> "Hello World!"
		/// </summary>
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
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				fontWidth = value.Size + 3.5f;
				computeScrollTable();
			}
		}

		/// <summary>
		/// The width of one charcter of the current font -> fontWidth = font.Size + 3.5;
		/// </summary>
		protected float fontWidth;

		/// <summary>
		/// The Brush for drawing; default -> SolidBrush(Color.Black); 
		/// this property could only be set programmatically
		/// </summary>
		public Brush brushProp
		{
			get { return brush; }
			set { if(value!=null) brush = value; }
		}
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
		/// How many pixels the text will be moved to the left each tick; default -> 5; 
		/// dx have to be >= 0
		/// </summary>
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
		protected float dx;

		/// <summary>
		/// The frequency of the sine curve; default -> 0.5; 
		/// </summary>
		public float frequencyProp
		{
			get { return freq; }
			set 
			{
				freq = value; 
				computeScrollTable();
			}
		}
		protected float freq;

		/// <summary>
		/// The amplitude scale factor of the sine curve; default -> 4.5; 
		/// </summary>
		public float amplitudeProp
		{
			get { return amp; }
			set 
			{
				amp = value; 
				computeScrollTable();
			}
		}
		protected float amp;

		/// <summary>
		/// The offset for the y-Values of the sine curve; default -> 0; 
		/// </summary>
		public float yOffsetProp
		{
			get { return yOff; }
			set 
			{
				yOff = value; 
				computeScrollTable();
			}
		}
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
		/// Stores the the value og the ticks variable if a mousebutton is pressed over the text
		/// </summary>
		protected int oldTick;

		/// <summary>
		/// Set to true fs the user pressed a mousebutton down, set to false if it was released
		/// </summary>
		protected bool pauseScrolling;

		/// <summary>
		/// The Timerevent for drawing the text
		/// </summary>
		protected Timer timer;

		/// <summary>
		/// The interval for the Timer; default -> 40 ms == 25fps
		/// </summary>
		public int intervalProp
		{
			get { return timer.Interval; }
			set { timer.Interval = value; }
		}

		/// <summary>
		/// The Image which is used for double buffering
		/// </summary>
		protected Image bmp;

		/// <summary>
		/// The Graphics Object for the Bitmap -> double buffering
		/// </summary>
		protected Graphics gfx;

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

			bmp = new Bitmap(this.Width, this.Height);
			gfx = Graphics.FromImage(bmp);
			dx = 5;
			freq = 0.5f;
			amp = 4.5f;
			yOff = 0;
			textProp = "Hello World!";
			Font = new Font("Sans serif", 10f);
			brush = new SolidBrush(Color.Black);
			ForeColor = Color.Black;		
			pauseScrolling = false;	
			timer = new Timer();
			timer.Interval = 40;
			timer.Tick += new EventHandler(timer_Tick);
			setTranslation();
			computeScrollTable();
			timer.Start();
		}		

		/// <summary>
		/// The Paint-method does the BitBlt
		/// </summary>
		/// <param name="pea">the PaintEventArgs ;-)</param>
		protected override void OnPaint(PaintEventArgs pea)
		{
			base.OnPaint(pea);
            pea.Graphics.DrawImage(bmp, 0, 0);
		}

		/// <summary>
		/// clear everything
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
		/// The Tick event handler method fills the DoubleBuffer and calls the paint method 
		/// </summary>
		/// <param name="sender">Sender ;-)</param>
		/// <param name="e">EventArgs ;-)</param>
		protected void timer_Tick(object sender, EventArgs e)
		{
			gfx.Clear(this.BackColor);
			int indexX = 0, indexY = 0;
			for(int i=0; i<chars.Length; i++)
			{
				indexX = indexY = ticks - i;
				if(pauseScrolling)
					indexX = oldTick - i;
				if(indexX >= scrollTab.Length || indexY >= scrollTab.Length)
				{
					if(pauseScrolling)
						break;
					continue;
				}
				else if(indexX < 0 || indexY < 0)
					break;

				gfx.DrawString(chars[i], Font, brush, scrollTab[indexX].X+i*fontWidth, scrollTab[indexY].Y);
			}
			ticks++;
			if(indexY >= scrollTab.Length)
			{
				ticks = 0;
				if(pauseScrolling)
					ticks = oldTick;
			}
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
		/// Translates the coordinate-origin for the Doublebuffer
		/// </summary>
		protected void setTranslation()
		{
			if(gfx != null && bmp != null)
			{
				bmp = new Bitmap(this.Width, this.Height);
				gfx = Graphics.FromImage(bmp);	
				gfx.TranslateTransform(0, (this.Height>>1)-(Font.Height*0.625f));
			}
		}

		/// <summary>
		/// If the size has been changed, the Translation is calculated and the ScrollTable is computed with the new values
		/// </summary>
		/// <param name="sender">Sender ;-)</param>
		/// <param name="e">EventArgs ;-)</param>
		protected void SineScroller_Resize(object sender, System.EventArgs e)
		{
			setTranslation();		
			computeScrollTable();
		}

		private void SineScroller_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pauseScrolling = true;
			oldTick = ticks;
		}

		private void SineScroller_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pauseScrolling = false;
			ticks = oldTick;
		}
	}
}