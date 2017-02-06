using System;
using System.Drawing;

namespace OCRPreProcessing
{
	/// <summary>
	/// Description: Class for creating an image which contains a character
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class OCRImageCreator
	{
		/// <summary>
		/// The bitmap containing the character
		/// </summary>		
		public Bitmap bmpProp
		{
			get { return bmp; }
			set { bmp = value; }
		}
		/// <summary>
		/// The bitmap containing the character
		/// </summary>	
		protected Bitmap bmp;

		/// <summary>
		/// The char character corresponding to the image -> default: 'W'
		/// </summary>
		public char characterProp
		{
			get { return character; }
			set { character = value; }
		}
		/// <summary>
		/// The char character corresponding to the image -> default: 'W'
		/// </summary>
		protected char character = 'W';

		/// <summary>
		/// The Font which will be used for drawing the image -> default: sans serif with size 6.5
		/// </summary>
		public Font fontProp 
		{
			get { return font; }
		}
		/// <summary>
		/// The Font which will be used for drawing the image -> default: sans serif with size 6.5
		/// </summary>
		protected Font font = new Font("Sans serif", 6.5f);

		/// <summary>
		/// The Defaultconstructor creates an image with the character W in the size 
		/// of 6.5f and the sans serif font
		/// </summary>
		public OCRImageCreator()
		{
			this.bmp = createCharBmp(this.character, this.font);
		}

		/// <summary>
		/// Constructor creates an image in the size of 6.5f and the sans serif font
		/// </summary>
		/// <param name="c">the character for drawing the image</param>
		public OCRImageCreator(char c)
		{
			this.bmp = createCharBmp(c, this.font);
		}

		/// <summary>
		/// Constructor for all parameters
		/// </summary>
		/// <param name="c">the character for drawing the image</param>
		/// <param name="font">the font used for drawing the character in the image</param>
		public OCRImageCreator(char c, Font font)
		{
			this.bmp = createCharBmp(c, font);
		}

		/// <summary>
		/// Creates an image with a character
		/// </summary>
		/// <param name="c">the character for drawing the image</param>
		/// <param name="font">the font used for drawing the character in the image</param>
		/// <returns>the bitmap containing the drawn character</returns>
		public Bitmap createCharBmp(char c, Font font)
		{
			this.character = c;
			this.font = font;
			int width = (int)(this.font.Size-1)*2;
			int height = this.font.Height;
			Bitmap bmp = new Bitmap(width, height);
			Graphics gfx = Graphics.FromImage(bmp);
			gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
			gfx.Clear(Color.White);
			gfx.DrawString(character.ToString(), this.font, new SolidBrush(Color.Black), 0, -1);
			return bmp;
		}

		/// <summary>
		/// Add randomly black and white pixels to the image
		/// </summary>
		/// <param name="percent">how much percent of the pixels should be noised</param>
		public void addNoiseToImg(float percent)
		{
			if(percent == 0)
				return;
			double par = percent * 0.01;
			Random rand = new Random();
			Random rand2 = new Random();
			System.Threading.Thread.Sleep(1);
			int bright;
			for(int x=0; x<bmp.Width; x++)	
			{
				for(int y=0; y<bmp.Height; y++)
				{
					if(rand.NextDouble() < par)
					{
						// Pixel umdrehen
				//		bright = (int)((1 - bmp.GetPixel(x, y).GetBrightness()) * 255);
						if(rand2.NextDouble() >= 0.5)
							bright = 255;
						else
							bright = 0;
						bmp.SetPixel(x, y, Color.FromArgb(bright, bright, bright));
					}
				}
			}
		}
	}
}
