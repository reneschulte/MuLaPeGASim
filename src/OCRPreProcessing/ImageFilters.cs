using System;
using System.Drawing;
using System.Collections;

namespace OCRPreProcessing
{
	/// <summary>
	/// Delegate for the progress of the image filter computing
	/// </summary>
	public delegate void progressHandler(int current, int total);
	
	/// <summary>
	/// Description: The interface of an Image Filter
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public interface IImageFilter
	{
		/// <summary>
		/// The event which is thrown to indicate the progress of computing
		/// </summary>
		event progressHandler onComputing;

		/// <summary>
		/// Compute the filtering
		/// </summary>
		/// <param name="srcBmp">the source image for filtering</param>
		/// <returns>the filtered image</returns>
		Bitmap compute(Bitmap srcBmp);
	}


	/// <summary>
	/// Description: Converts a RGB color image into a gray image
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class GrayFilter : IImageFilter
	{
		/// <summary>
		/// The event which is thrown to indicate the progress of computing
		/// </summary>
		public event progressHandler onComputing;

		/// <summary>
		/// Defaultconstructor, does nothin'
		/// </summary>
		public GrayFilter() {}

		/// <summary>
		/// Converts a RGB color image into a gray image
		/// </summary>
		/// <param name="srcBmp">the source image for filtering</param>
		/// <returns>the filtered image</returns>
		public Bitmap compute(Bitmap srcBmp)
		{
			Bitmap bmp = new Bitmap(srcBmp);
			// gray = 0.33 * r + 0.5 * g + 0.17 * b (Standard)
			// gray = 0.30 * r + 0.59 * g + 0.11 * b (NTSC)
			Color color;
			int gray;
			for (int x=0; x<srcBmp.Width; x++)
			{
				for (int y=0; y<srcBmp.Height; y++)
				{
					color = srcBmp.GetPixel(x, y);
					gray = (int)(0.33f * color.R + 0.5f * color.G + 0.17f* color.B) & 0xF0F0F0;
					bmp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
				}
				if(this.onComputing != null)
					onComputing(x, bmp.Width);
			}
			return bmp;
		}
	}

	/// <summary>
	/// Description: Converts an image into a binary black/white image
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 11-04-2004</para>
	/// </summary>
	public class BinarizeFilter : IImageFilter
	{
		/// <summary>
		/// The event which is thrown to indicate the progress of computing
		/// </summary>
		public event progressHandler onComputing;

		/// <summary>
		/// The threshold for converting values between 0 and 1 make sense ;-) -> default: 0.5 
		/// if(pixel.brightness > threshold) then pixel.newcolor=white else pixel.newcolor=black
		/// </summary>
		public float thresholdProp
		{
			get{ return threshold; }
			set{ threshold = value; }
		}
		/// <summary>
		/// The threshold for converting values between 0 and 1 make sense ;-) -> default: 0.5 
		/// if(pixel.brightness > threshold) then pixel.newcolor=white else pixel.newcolor=black
		/// </summary>
		protected float threshold;

		/// <summary>
		/// Defaultconstructor, does nothin'
		/// </summary>
		public BinarizeFilter() 
		{
			threshold = 0.5f;
		}

		/// <summary>
		/// Converts an image into a binary black/white image. 
		/// The pixel will be made black if the brightness of the pixel is lower than the threshold.
		/// </summary>
		/// <param name="srcBmp">the source image for filtering</param>
		/// <returns>the filtered image</returns>
		public Bitmap compute(Bitmap srcBmp)
		{
			Bitmap bmp = new Bitmap(srcBmp);
			Graphics.FromImage(bmp).Clear(Color.White);
			for (int x=0; x<srcBmp.Width; x++)
			{
				for (int y=0; y<srcBmp.Height; y++)
				{
					if(srcBmp.GetPixel(x, y).GetBrightness() < threshold)
						bmp.SetPixel(x, y, Color.Black);
				}
				if(this.onComputing != null)
					onComputing(x, bmp.Width);
			}
			return bmp;
		}
	}

	/// <summary>
	/// Description: Normalizes the brightness of a image using a best fit linear filter
	/// <para>Author: Rene Schulte</para>/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class BrightnNormalizer : IImageFilter
	{
		/// <summary>
		/// The event which is thrown to indicate the progress of computing
		/// </summary>
		public event progressHandler onComputing;

		/// <summary>
		/// Defaultconstructor, does nothin'
		/// </summary>
		public BrightnNormalizer() {}

		/// <summary>
		///  Normalizes the brightness of a image using a best fit linear filter
		/// </summary>
		/// <param name="srcBmp">the source image for filtering</param>
		/// <returns>the filtered image</returns>
		public Bitmap compute(Bitmap srcBmp)
		{
			Bitmap bmp = new Bitmap(srcBmp);
			float[] ra = new float[srcBmp.Width];
			float[] rb = new float[srcBmp.Width];
			float[] ca = new float[srcBmp.Height];
			float[] cb = new float[srcBmp.Height];
			float sx, sy, sxy, sxx;
			int n;
			float p;

			// rows:
			for(int x=0; x<srcBmp.Width; x++)  
			{
				n = 0; 
				sx = sy = sxy = sxx = 0;
				for(int y=0; y<srcBmp.Height; y++) 
				{
					sx += y; 
					sxx += y * y;
					p = srcBmp.GetPixel(x, y).GetBrightness()*255;
					sy += p; 
					sxy += p * y;
					n++;
				}
				ra[x] = (n*sxy - sx*sy) / (n*sxx - sx*sx);
				rb[x] = sy/n - ra[x] * sx/n;
				if(this.onComputing != null)
					onComputing(x, bmp.Width*3);
			}

			// cols:
			for(int y=0; y<srcBmp.Height; y++) 
			{
				n = 0; 
				sx = sy = sxy = sxx = 0;
				for(int x=0; x<srcBmp.Width; x++)  
				{
					sx += x; 
					sxx += x * x;
					p = ra[x] * y + rb[x];
					sy += p; 
					sxy += p * x;
					n++;
				}
				ca[y] = (n*sxy - sx*sy) / (n*sxx - sx*sx);
				cb[y] = sy/n - ca[y] * sx/n;
				if(this.onComputing != null)
					onComputing(y+bmp.Width, bmp.Width*3);
			}

			// apply:
			int gray;
			for(int x=0; x<srcBmp.Width; x++) 
			{
				for(int y=0; y<srcBmp.Height; y++) 
				{
					gray = (int)(srcBmp.GetPixel(x, y).GetBrightness()*255);
					gray = gray - (int)(ca[y] * x + cb[y])+128;
					if (gray < 0)
						gray = 0;
					else if (gray > 255)
						gray = 255; 
					bmp.SetPixel(x ,y, Color.FromArgb(gray, gray, gray));
				}
				if(this.onComputing != null)
					onComputing(x+2*bmp.Width, bmp.Width*3);
			}
			return bmp;
		}
	}


	/// <summary>
	/// Description: Equalizes the histogram of an image
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class HistEqualizer : IImageFilter
	{
		/// <summary>
		/// The event which is thrown to indicate the progress of computing
		/// </summary>
		public event progressHandler onComputing;

		/// <summary>
		/// Defaultconstructor, does nothin'
		/// </summary>
		public HistEqualizer() {}

		/// <summary>
		/// Equalizes the histogram of an image
		/// </summary>
		/// <param name="srcBmp">the source image for filtering</param>
		/// <returns>the filtered image</returns>
		public Bitmap compute(Bitmap srcBmp)
		{
			Bitmap bmp = new Bitmap(srcBmp);
			int[] h = new int[256];

			// read histogram
			for(int x=0; x<srcBmp.Width; x++) 
			{
				for(int y=0; y<srcBmp.Height; y++) 
					h[(int)(srcBmp.GetPixel(x, y).GetBrightness()*255)]++;
				if(this.onComputing != null)
					onComputing(x, bmp.Width*2);
			}

			// cumulative histogram
			for (int i=1; i<256; i++) 
				h[i] += h[i-1];

			// normalize
			for (int i=0; i<256; i++) 
				h[i] = (255 * h[i]) / (srcBmp.Width * srcBmp.Height);

			int gray;
			for(int x=0; x<srcBmp.Width; x++) 
			{
				for(int y=0; y<srcBmp.Height; y++) 
				{
					gray = (int)(srcBmp.GetPixel(x, y).GetBrightness()*255);
					gray = h[gray];						
					bmp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
				}
				if(this.onComputing != null)
					onComputing(x+bmp.Width, bmp.Width*2);
			}
			return bmp;
		}
	}

	/// <summary>
	/// Description: Smooths the image using a gaussian 5x5 convolution matrix
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class GaussFilter : IImageFilter
	{
		/// <summary>
		/// The event which is thrown to indicate the progress of computing
		/// </summary>
		public event progressHandler onComputing;

		/// <summary>
		/// The Sigma paramter for the gaussian smoothing. Higher values increase the 'smoothness' -> default: 1.6
		/// </summary>
		public double sigmaProp
		{
			get{ return sigma; }
			set{ sigma = value; }
		}
		/// <summary>
		/// The Sigma paramter for the gaussian smoothing. Higher values increase the 'smoothness' -> default: 1.6
		/// </summary>
		protected double sigma = 1.6;

		/// <summary>
		/// The gauss mask -> default: sigma = 1.6
		/// </summary>
		public double[,] gaussMaskProp
		{
			get{ return gaussMask; }
			set{ if(value != null) gaussMask = value; }
		}
		/// <summary>
		/// The gauss mask -> default: sigma = 1.6
		/// </summary>
		protected double[,] gaussMask;

		/// <summary>
		/// Defaultconstructor, initializes the gauss mask
		/// </summary>
		public GaussFilter()
		{
			gaussMask = new double[5,5];
			gaussMask[0,0] = 2; gaussMask[0,1] =  4; gaussMask[0,2] =  5; gaussMask[0,3] =  4; gaussMask[0,4] = 2;
			gaussMask[1,0] = 4; gaussMask[1,1] =  9; gaussMask[1,2] = 12; gaussMask[1,3] =  9; gaussMask[1,4] = 4;
			gaussMask[2,0] = 5; gaussMask[2,1] = 12; gaussMask[2,2] = 15; gaussMask[2,3] = 12; gaussMask[2,4] = 5;
			gaussMask[3,0] = 4; gaussMask[3,1] =  9; gaussMask[3,2] = 12; gaussMask[3,3] =  9; gaussMask[3,4] = 4;
			gaussMask[4,0] = 2; gaussMask[4,1] =  4; gaussMask[4,2] =  5; gaussMask[4,3] =  4; gaussMask[4,4] = 2;
			for(int j=0;j<5;++j)
				for(int i=0;i<5;++i)
					gaussMask[j,i] /= 115;
		}

		/// <summary>
		///  Smooths the image using a gaussian 5x5 convolution matrix
		/// </summary>
		/// <param name="srcBmp">the source image for filtering</param>
		/// <returns>the filtered image</returns>
		public Bitmap compute(Bitmap srcBmp)
		{
			if(srcBmp.Width <= gaussMask.GetUpperBound(0)+1 || srcBmp.Height <= gaussMask.GetUpperBound(1)+1)
				throw new FormatException("Gaussfilter could not be applied to Images smaller 6x6");

			Bitmap bmp = new Bitmap(srcBmp);
			float[,] gray = new float[srcBmp.Width, srcBmp.Height];
			for(int x=0; x<srcBmp.Width; x++)
				for(int y=0; y<srcBmp.Height; y++)
					gray[x,y] = srcBmp.GetPixel(x, y).GetBrightness()*255;

			int sum=0, tx, ty;
			for(int x=0; x<srcBmp.Width; x++)
			{
				for(int y=0; y<srcBmp.Height; y++)
				{
					if(x==0 || x==1)
						tx = x+2;
					else if(x==srcBmp.Width-1 || x==srcBmp.Width-2)
						tx = x-2;
					else
						tx = x;
					if(y==0 || y==1)
						ty = y+2;
					else if(y==srcBmp.Height-1 || y==srcBmp.Height-2)
						ty = y-2;
					else
						ty = y;

					sum = 0;
					for(int i=-2; i<=2; i++)
						for(int j=-2; j<=2; j++)
							sum += (int)(gray[tx+i,ty+j] * gaussMask[i+2,j+2]);

					if(sum > 255)
						sum = 255;
					else if(sum < 0)
						sum = 0;
					sum = sum%256;
					bmp.SetPixel(x, y, Color.FromArgb(sum, sum, sum));
				}
				if(this.onComputing != null)
					onComputing(x, srcBmp.Width);
			}
			return bmp;
		}

		/// <summary>
		/// Computes the convolution matrix for the gaussian smoothing, using the value of the sigma member
		/// </summary>
		public void computeGaussMask()
		{
			if(this.sigma == 1.6)
				return;

			gaussMask = new double[5,5];
			for(int j=0;j<5;++j)
				for(int i=0;i<5;++i)
				{
					for(double y = j-(5/2) - 0.5; y < j-(5/2) + 0.6; y += 0.1)
						for(double x = i-(5/2) - 0.5; x < i-(5/2) + 0.6; x += 0.1)
							gaussMask[i,j] += ((1/(2*Math.PI*sigma*sigma)) * Math.Pow(Math.E, -(x*x+y*y)/(2*sigma*sigma)) );
					gaussMask[i,j] /= 121;
				}
		}
	}


	/// <summary>
	/// Description: Filters the image for edge detection using the Sobel operator (two 3x3 convolution matrix)
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class SobelFilter : IImageFilter
	{
		/// <summary>
		/// The event which is thrown to indicate the progress of computing
		/// </summary>
		public virtual event progressHandler onComputing;

		/// <summary>
		/// The Sobel mask for x
		/// </summary>
		public readonly int[,] sobelMaskX;

		/// <summary>
		/// The Sobel mask for y
		/// </summary>
		public readonly int[,] sobelMaskY;

		/// <summary>
		/// Defaultconstructor, initialize the Sobel convolution mask
		/// </summary>
		public SobelFilter()
		{
			sobelMaskX = new int[3,3];
			sobelMaskX[0,0] = -1; sobelMaskX[0,1] = 0; sobelMaskX[0,2] = 1;
			sobelMaskX[1,0] = -2; sobelMaskX[1,1] = 0; sobelMaskX[1,2] = 2;
			sobelMaskX[2,0] = -1; sobelMaskX[2,1] = 0; sobelMaskX[2,2] = 1;

			sobelMaskY = new int[3,3];
			sobelMaskY[0,0] =  1; sobelMaskY[0,1] =  2; sobelMaskY[0,2] =  1;
			sobelMaskY[1,0] =  0; sobelMaskY[1,1] =  0; sobelMaskY[1,2] =  0;
			sobelMaskY[2,0] = -1; sobelMaskY[2,1] = -2; sobelMaskY[2,2] = -1;
		}

		/// <summary>
		/// Filters the image for edge detection using the Sobel operator (two 3x3 convolution matrix)
		/// </summary>
		/// <param name="srcBmp">the source image for filtering</param>
		/// <returns>the filtered image</returns>
		public virtual Bitmap compute(Bitmap srcBmp)
		{
			if(srcBmp.Width <= sobelMaskX.GetUpperBound(0)+1 || srcBmp.Height <= sobelMaskX.GetUpperBound(1)+1 || 
			   srcBmp.Width <= sobelMaskY.GetUpperBound(0)+1 || srcBmp.Height <= sobelMaskY.GetUpperBound(1)+1)
				throw new FormatException("Sobelfilter could not be applied to Images smaller 4x4");

			Bitmap bmp = new Bitmap(srcBmp);
			int[,] gray = new int[srcBmp.Width, srcBmp.Height];
			for(int x=0; x<srcBmp.Width; x++)
				for(int y=0; y<srcBmp.Height; y++)
					gray[x,y] = (int)(srcBmp.GetPixel(x, y).GetBrightness()*255);

			int sumX=0, sumY=0, sum, tx, ty;
			for(int x=0; x<srcBmp.Width; x++)
			{
				for(int y=0; y<srcBmp.Height; y++)
				{
					if(x==0)
						tx = x+1;
					else if(x==srcBmp.Width-1)
						tx = x-1;
					else
						tx = x;
					if(y==0)
						ty = y+1;
					else if(y==srcBmp.Height-1)
						ty = y-1;
					else
						ty = y;
					sumX = 0;
					sumY = 0;
					/*************** X+Y GRADIENT APPROXIMATION ***************/
					for(int i=-1; i<=1; i++)
						for(int j=-1; j<=1; j++)
						{
							sumX += gray[tx+i, ty+j] * sobelMaskX[i+1,j+1];
							sumY += gray[tx+i, ty+j] * sobelMaskY[i+1,j+1];
						}

					/******GRADIENT MAGNITUDE APPROXIMATION ******/
					sum = Math.Abs(sumX) + Math.Abs(sumY);

					if(sum > 255)
						sum = 255;
					else if(sum < 0)
						sum = 0;

					sum = 255-sum;
					bmp.SetPixel(x, y, Color.FromArgb(sum, sum, sum));
				}
				if(this.onComputing != null)
					onComputing(x, srcBmp.Width);
			}
			return bmp;
		}
	}


	/// <summary>
	/// Description: Detects the edges of an image using the Canny-Algorithm
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class CannyFilter : SobelFilter
	{
		/// <summary>
		/// The event which is thrown to indicate the progress of computing
		/// </summary>
		public override event progressHandler onComputing;
		
		/// <summary>
		/// The Sigma paramter for the gaussian smoothing higher values increase the 'smoothness' -> default: 1.6
		/// </summary>
		public double sigmaProp
		{
			get{ return sigma; }
			set{ sigma = value; }
		}
		/// <summary>
		/// The Sigma paramter for the gaussian smoothing higher values increase the 'smoothness' -> default: 1.6
		/// </summary>
		protected double sigma = 1.6;

		/// <summary>
		/// The low threshold for the hysteresis computing of the Canny algorithm -> default: 10
		/// </summary>
		public int lowThresholdProp
		{
			get{ return lowThreshold; }
			set{ lowThreshold = value; }
		}
		/// <summary>
		/// The low threshold for the hysteresis computing of the Canny algorithm -> default: 10
		/// </summary>
		protected int lowThreshold = 10;

		/// <summary>
		/// The high threshold for the hysteresis computing of the Canny algorithm -> default: 200
		/// </summary>
		public int highThresholdProp
		{
			get{ return highThreshold; }
			set{ highThreshold = value; }
		}
		/// <summary>
		/// The high threshold for the hysteresis computing of the canny algorithm -> default: 200
		/// </summary>
		protected int highThreshold = 200;

		/// <summary>
		/// If doSmoothing == true, the image will be smoothed using the gaussian smoothing,
		/// before the Canny algo is computed -> default: true
		/// </summary>
		public bool doSmoothingProp
		{
			get{ return doSmoothing; }
			set{ doSmoothing = value; }
		}
		/// <summary>
		/// If doSmoothing == true, the image will be smoothed using the gaussian smoothing,
		/// before the Canny algo is computed -> default: true
		/// </summary>
		protected bool doSmoothing = true;

		/// <summary>
		/// Defaultconstructor, just calls the Superconstructor
		/// <seealso cref="OCRPreProcessing.SobelFilter"/>
		/// </summary>
		public CannyFilter() : base() {}

		/// <summary>
		/// Detects the edges of an image using the Canny-Algorithm
		/// </summary>
		/// <param name="srcBmp">the source image for filtering</param>
		/// <returns>the filtered image</returns>
		public override Bitmap compute(Bitmap srcBmp)
		{
			
			if(srcBmp.Width <= sobelMaskX.GetUpperBound(0)+1 || srcBmp.Height <= sobelMaskX.GetUpperBound(1)+1 || 
				srcBmp.Width <= sobelMaskY.GetUpperBound(0)+1 || srcBmp.Height <= sobelMaskY.GetUpperBound(1)+1)
				throw new FormatException("Canny's Sobelfilter could not be applied to Images smaller 4x4");

			if(doSmoothing)
			{
				GaussFilter gf = new GaussFilter();
				gf.sigmaProp = this.sigma;
				gf.computeGaussMask();
				srcBmp = gf.compute(srcBmp);
			}
		
			Bitmap bmp = new Bitmap(srcBmp);
			int[,] gray = new int[srcBmp.Width, srcBmp.Height];
			for(int x=0; x<srcBmp.Width; x++)
				for(int y=0; y<srcBmp.Height; y++)
					gray[x,y] = (int)(srcBmp.GetPixel(x, y).GetBrightness()*255);

			int sumX=0, sumY=0, sum, tx, ty;
			float orient;
			int edgeDirection, leftPixel, rightPixel;
			int[] p = new int[8];

			for(int x=0; x<srcBmp.Width; x++)
			{
				for(int y=0; y<srcBmp.Height; y++)
				{
					/*++++++++++++++++++++++ Sobel Start +++++++++++++++++++++++++++*/
					if(x==0)
						tx = x+1;
					else if(x==srcBmp.Width-1)
						tx = x-1;
					else
						tx = x;
					if(y==0)
						ty = y+1;
					else if(y==srcBmp.Height-1)
						ty = y-1;
					else
						ty = y;
					sumX = 0;
					sumY = 0;
					/*************** X+Y GRADIENT APPROXIMATION ***************/
					for(int i=-1; i<=1; i++)
						for(int j=-1; j<=1; j++)
						{
							sumX += gray[tx+i, ty+j] * sobelMaskX[i+1,j+1];
							sumY += gray[tx+i, ty+j] * sobelMaskY[i+1,j+1];
						}

					/*********** GRADIENT MAGNITUDE APPROXIMATION ************/
					sum = Math.Abs(sumX) + Math.Abs(sumY);
					if(sum > 255)
						sum = 255;
					else if(sum < 0)
						sum = 0;

					/*+++++++++++++++++ Magnitude orientation +++++++++++++++++*/
					/**** edge direction*****/
					/* Cannot divide by zero*/
					if(sumX == 0)
					{
						if(sumY == 0)
							orient = 0.0f;
						else if (sumY < 0)
						{
							sumY = -sumY;
							orient = 90.0f;
						}
						else
							orient = 90.0f;
					}
						/* Can't take invtan of angle in 2nd Quad */
					else if(sumX<0 && sumY>0)
					{
						sumX = -sumX;
						orient = 180 - (float)(Math.Atan((float)sumY/sumX) * (180/Math.PI));
					}
						/* Can't take invtan of angle in 4th Quad */
					else if(sumX>0 && sumY<0)
					{
						sumY = -sumY;
						orient = 180 - (float)(Math.Atan((float)sumY/sumX) * (180/Math.PI));
					}
						/* else angle is in 1st or 3rd Quad */
					else
					{
						orient = (float)(Math.Atan((float)(sumY)/(float)(sumX)) * (180/Math.PI));
					}


					/*++++++++++++++++++ categorize edge direction +++++++++++++++++*/

					if(orient < 22.5f)
						edgeDirection = 0;
					else if(orient < 67.5f)
						edgeDirection = 45;
					else if(orient < 112.5f)
						edgeDirection = 90;
					else if(orient < 157.5f)
						edgeDirection = 135;
					else
						edgeDirection = 0;


					/*++++++++++++++++++ Nonmaximum supression +++++++++++++++++*/

					if(edgeDirection == 0)
					{
						leftPixel = gray[tx-1, ty];
						rightPixel = gray[tx+1, ty];
					}

					else if(edgeDirection == 45)
					{
						leftPixel = gray[tx-1, ty+1];
						rightPixel = gray[tx+1, ty-1];
					}

					else if(edgeDirection == 90)
					{
						leftPixel = gray[tx, ty-1];
						rightPixel = gray[tx, ty+1];
					}

					else
					{
						leftPixel = gray[tx-1, ty-1];
						rightPixel = gray[tx+1, ty+1];
					}

					/*++++++++++++++++++ Hysteresis +++++++++++++++++*/

					if(sum < leftPixel || sum < rightPixel)
						sum = 0;
					else
					{
						if(sum >= highThreshold)
							sum = 255; /* edge */
						else if(sum < lowThreshold)
							sum = 0;  /* nonedge */
							/* SUM is between T1 & T2 */
						else
						{
							/* Determine values of neighboring pixels */
							p[0] = gray[tx-1, ty-1];
							p[1] = gray[tx, ty-1];
							p[2] = gray[tx+1, ty-1];
							p[3] = gray[tx-1, ty];
							p[4] = gray[tx+1, ty];
							p[5] = gray[tx-1, ty+1];
							p[6] = gray[tx, ty+1];
							p[7] = gray[tx+1, ty+1];

							/* Check to see if neighboring pixel values are edges */
							if(p[0]  > highThreshold || p[1] > highThreshold || p[2]  > highThreshold || p[3]  > highThreshold ||
							   p[4]  > highThreshold || p[5] > highThreshold || p[6]  > highThreshold || p[7]  > highThreshold)
								sum = 255; /* make edge */
							else
								sum = 0; /* make nonedge */
						}
					}
					sum = 255-sum;
					bmp.SetPixel(x, y, Color.FromArgb(sum, sum, sum));
				}
				if(this.onComputing != null)
					onComputing(x, srcBmp.Width);
			}
			return bmp;
		}
	}

/*	public class EdgeBitmapGenerator:IImageFilter
	{
		private enum direction { Horizontal, Vertical, Diagonal1, Diagonal2 };
		public event OCRPreProcessing.progressHandler onComputing;

		public Bitmap compute(Bitmap srcBmp)
		{
			Bitmap targetBmp = new Bitmap(srcBmp);
			Graphics.FromImage(targetBmp).Clear(Color.White);
			int grey;

			for(int x=0; x<srcBmp.Width; x++)
			{
				for(int y=0; y<srcBmp.Height; y++)
				{
					grey = 255-this.computePixelLuminary(srcBmp, new Point(x,y));
					targetBmp.SetPixel(x,y, Color.FromArgb(grey, grey, grey));
				}

				if(this.onComputing != null)
					onComputing(x, srcBmp.Width);
			}
			return targetBmp;
		}

		private int computePixelLuminary(Bitmap bmp, Point p)
		{
			int xLeft, xRight, yUp, yDown;
			float pointLuminary = bmp.GetPixel(p.X, p.Y).GetBrightness();
			int[] luminary = new int[4];

			if((xLeft = p.X-1) < 0)
				xLeft = bmp.Width-1;
			if((xRight = p.X+1) > bmp.Width-1)
				xRight = 0;
			if((yUp = p.Y-1) < 0)
				yUp = bmp.Height-1;
			if((yDown = p.Y+1) > bmp.Height-1)
				yDown = 0;

			// horizontal
			luminary[(int)direction.Horizontal] = 255*(int)Math.Abs(2*pointLuminary - bmp.GetPixel(xLeft, p.Y).GetBrightness()- bmp.GetPixel(xRight, p.Y).GetBrightness());
			// vertikal
			luminary[(int)direction.Vertical] = 255*(int)Math.Abs(2*pointLuminary - bmp.GetPixel(p.X, yUp).GetBrightness() - bmp.GetPixel(p.X, yDown).GetBrightness());

			// diagonal
			luminary[(int)direction.Diagonal1] = 255*(int)Math.Abs(2*pointLuminary - bmp.GetPixel(xLeft, yUp).GetBrightness() - bmp.GetPixel(xRight, yDown).GetBrightness());
			luminary[(int)direction.Diagonal2] = 255*(int)Math.Abs(2*pointLuminary - bmp.GetPixel(xRight, yUp).GetBrightness() - bmp.GetPixel(xLeft, yDown).GetBrightness());

			int result = Math.Max(Math.Max(luminary[(int)direction.Horizontal], luminary[(int)direction.Vertical]),
							Math.Max(luminary[(int)direction.Diagonal1],luminary[(int)direction.Diagonal2]));
			if(result > 255)
				result = 255;
			else if(result < 0)
				result = 0;
			return result;
		}
	}

	public class LineEliminator:IImageFilter
	{
		public event OCRPreProcessing.progressHandler onComputing;

		private const int EDGE_GROWTH = 30;
		private const int NR_EDGE_PIXELS = 60;

		private const int NR_PIXEL_COUNT = 20;

		public Bitmap compute(Bitmap bmp)
		{
			float threshold = 0.2f;

			Bitmap result = new Bitmap(bmp);
			Graphics.FromImage(result).Clear(Color.White);

			for(int y=0, line=0, points; y<bmp.Height; y++)
			{
				points = 0;
				Color tmp;
				for(int x=0; x<bmp.Width; x++)
				{
					tmp = bmp.GetPixel(x,y);
					result.SetPixel(x, line, tmp);

					if(tmp.GetBrightness() < threshold)
						points++;
				}

				if(points >= NR_PIXEL_COUNT)
					line++;
			}
			return result;
		}
		// COMMENT OUT START
				public Bitmap compute(Bitmap bmp)
				{
					if(bmp == null)
						return null;

					Interval[] intervals = this.calculateLocalMax(this.countEdges(bmp));
					if(intervals != null)
					{
						Bitmap result = new Bitmap(bmp);
						Graphics.FromImage(result).Clear(Color.White);

						for(int i=0; i<intervals.Length; i++)
						{
							for(int y=intervals[i].Y0; y<intervals[i].Y1; y++)
							{
								for(int x=0; x<bmp.Width; x++)
									result.SetPixel(x, y, bmp.GetPixel(x, y));
							}
							if(this.onComputing != null)
								onComputing(i, intervals.Length);
						}
		//				return this.fillGaps(binarizeBitmap(result));
						return result;
					}
					return bmp;
				}
		// COMMENT OUT END
		
		private int[] countEdges(Bitmap bmp)
		{
			if(bmp == null)
				return null;

			int [] edges = new int[bmp.Height];

			for(int y=0; y<bmp.Height; y++)
			{
				for(int x=0; x<bmp.Width; x++)
				{
					if(bmp.GetPixel(x, y).GetBrightness() < 0.2)
						edges[y]++;
				}
				if(this.onComputing != null)
					onComputing(y, bmp.Height);
			}
			return edges;
		}

		private Interval[] calculateLocalMax(int[] histogram)
		{
			Interval[] result = null;
			ArrayList intervals = new ArrayList();
			bool insideTextArea = false;
			Interval si = null;
			int diff = 0, min=0, max=0;

			for(int i=1; i<histogram.Length; i++)
			{
				diff = histogram[i]-histogram[i-1];
				if(diff >= EDGE_GROWTH || histogram[i] > NR_EDGE_PIXELS)
				{
					if (!insideTextArea)
					{
						if (si == null || si.Y1 != i-2 || si.Y1-si.Y0<6)
						{
							si = new Interval();
							si.Y0 = i+1;
						}
						insideTextArea = true;
					}
				}
				else if (insideTextArea)
				{
					si.Y1 = i-1;
					if(si.Y1 - si.Y0 > 4 && !intervals.Contains(si))
					{
						intervals.Add(si);
						insideTextArea = false;
					}
					if (histogram[i]>histogram[max])
					{
						max = i;
						min = max;
					}
					else if (histogram[i]<histogram[min])
					{
						min = i;
						max = min;
					}
				}
				if(this.onComputing != null)
					onComputing(i, histogram.Length);
			}

			if (intervals.Count > 0)
			{
				result = new Interval[intervals.Count];
				for (int i=0; i<result.Length; i++)
				{
					result[i] = (Interval)intervals[i];;
				}
			}
			return result;
		}

		public Bitmap binarizeBitmap(Bitmap bmp)
		{
			if(bmp == null)
				return bmp;

			Bitmap result = new Bitmap(bmp);
			Graphics.FromImage(bmp).Clear(Color.White);

			for(int x=0; x<bmp.Width; x++)
			{
				for(int y=0; y<bmp.Height; y++)
				{
					float tmp = bmp.GetPixel(x,y).GetBrightness();
					if(tmp > 0.2)
						bmp.SetPixel(x, y, Color.Black);
				}
				if(this.onComputing != null)
					onComputing(x, bmp.Height);
			}
			return result;
		}

		public Bitmap fillGaps(Bitmap bmp)
		{
			if(bmp == null)
				return null;

			Bitmap result = new Bitmap(bmp);
			Graphics.FromImage(result).Clear(Color.White);

			float	upper, lower, left,	right, upperLeft, upperRight, lowerLeft, lowerRight;

			// fill
			for(int x=0; x<bmp.Width; x++)
			{
				for(int y=0; y<bmp.Height; y++)
				{
					upper = 1; lower = 1;
					left = 1; right = 1;
					upperLeft = 1; lowerRight = 1;
					upperRight = 1; lowerLeft = 1;


					if(x > 0)
						left = bmp.GetPixel(x, y).GetBrightness();
					if(x < bmp.Width-1)
						right = bmp.GetPixel(x, y).GetBrightness();
					if(y > 0)
						upper = bmp.GetPixel(x, y).GetBrightness();
					if(y < bmp.Height-1)
						lower = bmp.GetPixel(x, y).GetBrightness();

					if(x > 0 && y > 0)
						upperLeft = bmp.GetPixel(x-1, y-1).GetBrightness();
					if(x > 0 && y < bmp.Height-1)
						lowerLeft = bmp.GetPixel(x-1, y+1).GetBrightness();
					if(x < bmp.Width-1 && y > 0)
						upperRight = bmp.GetPixel(x+1, y-1).GetBrightness();
					if(x < bmp.Width-1 && y > bmp.Height-1)
						lowerRight = bmp.GetPixel(x+1, y+1).GetBrightness();

					float threshold = 0.2f;
					if(left < threshold && right < threshold
						|| upper < threshold && lower < threshold
						|| upperLeft < threshold && lowerRight < threshold
						|| upperRight < threshold && lowerLeft < threshold)
						bmp.SetPixel(x, y, Color.Black);
					else
						bmp.SetPixel(x, y, Color.White);
				}
				if(this.onComputing != null)
					onComputing(x, bmp.Width);
			}
			return bmp;
		}

		class Interval
		{
			private Point start;
			private Point end;

			public int X0
			{
				get { return this.start.X; }
				set { this.start.X = value; }
			}
			public int Y0
			{
				get { return this.start.Y; }
				set { this.start.Y = value; }
			}

			public int X1
			{
				get { return this.end.X; }
				set { this.end.X = value; }
			}
			public int Y1
			{
				get { return this.end.Y; }
				set { this.end.Y = value; }
			}

			public Interval()
			{
				this.start = new Point(0, 0);
				this.end = new Point(0, 0);
			}
		}
	}
*/
}