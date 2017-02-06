using System;
using System.Collections;
using System.Drawing;

namespace OCRPreProcessing
{
	/// <summary>
	/// Description: The interface for an Image Segmenter/Separator
	/// <para>Author: Torsten Baer</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 11-03-2004</para>
	/// </summary>
	public interface IImageSeparator
	{
		/// <summary>
		/// The event which is thrown to indicate the progress of computing
		/// </summary>
		event progressHandler onComputing;

		/// <summary>
		/// Separates the given image into n new images which contains the found letters
		/// </summary>
		/// <param name="bmp">The image which should be separated</param>
		/// <returns>The found letters as a Bitmap array</returns>
		Bitmap[] separateToBitmaps(Bitmap bmp);
	}

	/// <summary>
	/// Description: Separates an image into n images using the horizontal and vertical 
	/// histograms of a Bitmap
	/// <para>Author: Torsten Baer</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-27-2004</para>
	/// </summary>
	public class HistogramSeparator : IImageSeparator
	{
		private const float LOOP_STEP = 100f/8;
		private int progressCounter;

		/// <summary>
		/// The event which is thrown to indicate the progress of computing
		/// </summary>
		public event progressHandler onComputing;

		/// <summary>
		/// The thresholdfactor for the columns
		/// </summary>
		private float colThreshold;

		/// <summary>
		/// The thresholdfactor for the lines
		/// </summary>
		private float lineThreshold;

		/// <summary>
		/// The Rectangles for separation
		/// </summary>
		private Rectangle[] rects;

		/// <summary>
		/// Property to set the thresholdfactor for columns. 
		/// The factor will be multiplicated with the average of the column histogram
		/// </summary>
		public float ColumnThreshold
		{
			get { return this.colThreshold; }
			set
			{
				this.colThreshold = value;
			}
		}

		/// <summary>
		/// Property to set the threshold factor for the lines. 
		/// The factor will be multiplicated with the avarage of the line histogram
		/// </summary>
		public float LineThreshold
		{
			get { return this.lineThreshold; }
			set
			{
				this.lineThreshold = value;
			}
		}

		/// <summary>
		/// The rectangles for separation
		/// </summary>
		public Rectangle[] Rectangles
		{
			get { return rects; }
		}

		/// <summary>
		/// Defaultconstructor, initializes members
		/// </summary>
		public HistogramSeparator()
		{
			this.colThreshold = 0.05f;
			this.lineThreshold = 0.2f;
			this.rects = null;
		}

		/// <summary>
		/// This method separates the given Bitmap and returns the areas, where letters where found
		/// </summary>
		/// <param name="bmp">The image which should be separated</param>
		/// <returns>An array of rectangles</returns>
		private Rectangle[] getRectangles(Bitmap bmp)
		{
			float maxValue;

			// get line histogram
			float[] lineHisto = computeLineOrientedHistogram(bmp, out maxValue);

			float step = LOOP_STEP/lineHisto.Length;

			for(int i=0; i<lineHisto.Length; i++)
			{
				lineHisto[i] /= maxValue;

				if(onComputing != null)
					onComputing((int)(progressCounter*LOOP_STEP+i*step), 100);
			}
			progressCounter++;

			ArrayList lines = new ArrayList();
			int upper = -1, lower = -1;
			bool upperBorderFound = false;
			float lastVal = -1f;

			step = LOOP_STEP/(lineHisto.Length-2);

			// determine upper and lower line edges
			for(int i=1; i<lineHisto.Length-1; i++)
			{
				if(!upperBorderFound && lineHisto[i] >= this.lineThreshold && lineHisto[i] > lastVal)
				{
					lines.Add(i-1);
					upperBorderFound = true;
				}
				if(upperBorderFound && lineHisto[i] <= this.lineThreshold && lineHisto[i] < lastVal)
				{
					lines.Add(i+1);
					upperBorderFound = false;
				}

				lastVal = lineHisto[i];

				if(onComputing != null)
					onComputing((int)(progressCounter*LOOP_STEP+(i-1)*step), 100);
			}
			progressCounter++;

			// if nothing was found, select all
			if(lines.Count == 0)
			{
				lines.Add(0);
				lines.Add(bmp.Height);
			}
			// if an odd number was found
			else if(lines.Count%2 != 0)
				lines.Add(bmp.Height);

			ArrayList points = new ArrayList();
			float[] colHisto = null;

			step = LOOP_STEP/(lines.Count/2);

			// iterate through the lines to determine the letters
			for(int i=0; i<=lines.Count/2; i+=2)
			{
				upper = (int)lines[0+i]; lower = (int)lines[1+i];
				// get the columns histogram for the line
				colHisto = computeColumnOrientedHistogram(bmp, upper, lower, out maxValue);

				// calculate the avarage value
				for(int j=0; j<colHisto.Length; j++)
					colHisto[j] /= maxValue;

				lastVal = -1f;
				upperBorderFound = false;

				// find the points for the rectangles which will frame the letters
				for(int j=1; j<colHisto.Length; j++)
				{
					if(!upperBorderFound && colHisto[j] >= this.colThreshold && colHisto[j] > lastVal)
					{
						points.Add(new Point(j-1, upper));
						upperBorderFound = true;
					}
					if(upperBorderFound && colHisto[j] <= this.colThreshold && colHisto[j] < lastVal)
					{
						points.Add(new Point(j, lower));
						upperBorderFound = false;
					}
					lastVal = colHisto[j];
				}

				// if nothing was found, select all
				if(points.Count == 0)
				{
					points.Add(new Point(0,upper));
					points.Add(new Point(bmp.Width, lower));
				}
				// if an odd number of points was found
				else if(points.Count%2 != 0)
				{
					if(upperBorderFound)
						points.Add(new Point(colHisto.Length-1, lower));
					else if(!upperBorderFound)
						points.Add(new Point(colHisto.Length-1, upper));
				}

				if(onComputing != null)
					onComputing((int)(progressCounter*LOOP_STEP+i*step), 100);
			}
			progressCounter++;

			// convert found points into rectangles
			Rectangle[] rect = new Rectangle[points.Count/2];

			step = LOOP_STEP/rect.Length;

			for(int i=0, j=0; j<rect.Length; i+=2, j++)
			{
				rect[j] = new Rectangle(((Point)points[i]).X, ((Point)points[i]).Y,
					((Point)points[i+1]).X-((Point)points[i]).X, ((Point)points[i+1]).Y-((Point)points[i]).Y);

				if(onComputing != null)
					onComputing((int)(progressCounter*LOOP_STEP+i*step), 100);
			}
			progressCounter++;
			return rect;
		}

		/// <summary>
		/// Separates an image into n images using histograms
		/// </summary>
		/// <param name="bmp">the image which should be separated</param>
		/// <returns>the separated images</returns>
		public Bitmap[] separateToBitmaps(Bitmap bmp)
		{
			progressCounter = 0;

			// get the rectangles for separation
			this.rects = this.getRectangles(bmp);
			Bitmap[] bmps = new Bitmap[rects.Length];

			float step = LOOP_STEP/rects.Length;

			// separate the image into single bitmaps
			Graphics grfx;
			for(int i=0; i<bmps.Length; i++)
			{
				bmps[i] = new Bitmap(rects[i].Width, rects[i].Height);
				grfx = Graphics.FromImage(bmps[i]);

				grfx.DrawImage(bmp, 0, 0, rects[i], GraphicsUnit.Pixel);

				if(onComputing != null)
					onComputing((int)(progressCounter*LOOP_STEP+i*step), 100);
			}

			// all done
			if(onComputing != null)
				onComputing(100,100);

			return bmps;
		}

		/// <summary>
		/// Builds the vertical histogram of a given image
		/// </summary>
		/// <param name="bmp">The Image, which should be separated</param>
		/// <param name="maxValue">The max Value of the determined histogram</param>
		/// <returns>The histogram, represented by a float array</returns>
		protected float[] computeLineOrientedHistogram(Bitmap bmp, out float maxValue)
		{
			maxValue = 0f;

			float[] histogram = new float[bmp.Height];

			float step = LOOP_STEP/histogram.Length;

			for(int i=0; i<bmp.Height; i++)
			{
				for(int j=0; j<bmp.Width; j++)
					histogram[i] += 1-bmp.GetPixel(j,i).GetBrightness();

				if(histogram[i] > maxValue)
					maxValue = histogram[i];

				if(onComputing != null)
					onComputing((int)(progressCounter*LOOP_STEP+i*step), 100);
			}
			this.progressCounter++;
			return histogram;
		}

		/// <summary>
		/// Builds the horizontal histogram of a given image
		/// </summary>
		/// <param name="bmp">The Image, which should be separated</param>
		/// <param name="upper">Marks the upper edge for the Calculation</param>
		/// <param name="lower">Marks the lower edge for the Calculation</param>
		/// <param name="maxValue">The max Value of the determined histogram</param>
		/// <returns>The histogram, represented by a float array</returns>
		protected float[] computeColumnOrientedHistogram(Bitmap bmp, int upper, int lower, out float maxValue)
		{
			maxValue = 0f;

			if(upper < 0)
				upper = 0;
			if(lower > bmp.Height)
				lower = bmp.Height;

			float[] histogram = new float[bmp.Width];
			float step = LOOP_STEP/histogram.Length;

			for(int i=0; i<bmp.Width; i++)
			{
				for(int j=upper; j<lower; j++)
					histogram[i] += 1-bmp.GetPixel(i,j).GetBrightness();

				if(histogram[i] > maxValue)
					maxValue = histogram[i];

				if(onComputing != null)
					onComputing((int)(progressCounter*LOOP_STEP+i*step), 100);
			}

			this.progressCounter++;
			return histogram;
		}

	}
}