using System;
using System.Drawing;

namespace OCRPreProcessing
{
	/// <summary>
	/// Description: Abstract class for various algorithms to extract the necessary features for a Neural Network, 
	/// from an image containing a character 
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-30-2004</para>
	/// </summary>
	public abstract class OCRImageExtractor
	{
		/// <summary>
		/// The threshold for extracting bitmap values b/w. Default 0.5 
		/// <para>if(pixel.brightness > threshold) pat = 1; else pat = 0</para>
		/// </summary>
		public float thresholdProp
		{
			get { return threshold; }
			set { if(value >= 0.0f && value <= 1.0f) threshold = value; }
		}
		/// <summary>
		/// The threshold for extracting bitmap values b/w. Default 0.5 
		/// <para>if(pixel.brightness > threshold) pat = 1; else pat = 0</para>
		/// </summary>
		protected float threshold = 0.5f;

		/// <summary>
		/// Defaultconstructor, does nothin'
		/// </summary>
		public OCRImageExtractor() {}

		/// <summary>
		/// Computes the pattern vector
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <returns>the pattern vector</returns>
		public float[] computePatternVec(Bitmap srcBmp)
		{
			return computePatternVec(srcBmp, new Rectangle(0, 0, srcBmp.Width, srcBmp.Height));
		}

		/// <summary>
		/// Computes the pattern vector
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <param name="window">the window which should be extracted</param>
		/// <returns>the pattern vector</returns>
		public float[] computePatternVec(Bitmap srcBmp, Rectangle window)
		{
			if(srcBmp == null)
				throw new Exception("No input image to extract!");
			if(window.Width + window.X > srcBmp.Width)
				window.Width = srcBmp.Width-window.X;
			if(window.Height + window.Y > srcBmp.Height)
				window.Height = srcBmp.Height-window.Y;

			float[] patternVector = new float[window.Height * window.Width];
			for(int y=0; y<window.Height; y++)
			{
//				Console.Write("\n{0,2}: ", y);
				for(int x=0; x<window.Width; x++)	
				{
					if(srcBmp.GetPixel(x+window.Left, y+window.Top).GetBrightness() > threshold)
						patternVector[y*window.Width+x] = 0.0f;
					else
						patternVector[y*window.Width+x] = 1.0f;
//					Console.Write(patternVector[y*window.Width+x]);
				} 
			}
			return patternVector;
		}

		/// <summary>
		/// Computes the feature vector
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <returns>the feature vector</returns>
		public abstract float[] computeFeatureVec(Bitmap srcBmp);

		/// <summary>
		/// Computes the feature vector
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <param name="window">the window which should be extracted</param>
		/// <returns>the feature vector</returns>
		public abstract float[] computeFeatureVec(Bitmap srcBmp, Rectangle window);
	}


	/// <summary>
	/// Description: Just the raw negative brightness data.featureVector == patternVector, 
	/// each element of the vector is the negative brightness of one pixel
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-30-2004</para>
	/// </summary>
	public class RawExtractor : OCRImageExtractor
	{
		/// <summary>
		/// The Defaultconstructor, does nothin'
		/// </summary>
		public RawExtractor() {}

		/// <summary>
		/// Computes the feature vector: featureVector == patternVector, 
		/// each element of the vector is the negative brightness of one pixel
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <returns>the feature vector</returns>
		public override float[] computeFeatureVec(Bitmap srcBmp)
		{
			return computeFeatureVec(srcBmp, new Rectangle(0, 0, srcBmp.Width, srcBmp.Height));
		}

		/// <summary>
		/// Computes the feature vector: featureVector == patternVector, 
		/// each element of the vector is the negative brightness of one pixel
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <param name="window">the window which should be extracted</param>
		/// <returns>the feature vector</returns>
		public override float[] computeFeatureVec(Bitmap srcBmp, Rectangle window)
		{
			return computePatternVec(srcBmp, window);
		}
	}


	/// <summary>
	/// Description: abstract class for various counting methods
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-30-2004</para>
	/// </summary>
	public abstract class OCRImgExtCounter : OCRImageExtractor
	{
					
		/// <summary>
		/// Describes how often a column should be extracted. Default == 1 => every column. 
		/// A value of 2 means that every 2nd column would be used for extraction
		/// </summary>
		public int dxProp
		{
			get { return dx; }
			set { dx = value; }
		}
		/// <summary>
		/// Describes how often a column should be extracted. Default == 1 => every column. 
		/// A value of 2 means that every 2nd column would be used for extraction
		/// </summary>
		protected int dx = 1;

		/// <summary>
		/// Describes how often a row should be extracted. Default == 1 => every row.
		/// A value of 2 means that every 2nd row would be used for extraction
		/// </summary>
		public int dyProp
		{
			get { return dy; }
			set { dy = value; }
		}
		/// <summary>
		/// Describes how often a row should be extracted. Default == 1 => every row.
		/// A value of 2 means that every 2nd row would be used for extraction
		/// </summary>
		protected int dy = 1;

		/// <summary>
		/// The Defaultconstructor, does nothin'
		/// </summary>
		public OCRImgExtCounter() {}

		/// <summary>
		/// Initialize the size of the feature vector
		/// </summary>
		/// <param name="window">the window for extracting</param>
		/// <returns>the feature vector</returns>
		protected float[] init(Rectangle window)
		{
			int w = window.Width/dx + window.Width%dx;
			int h = window.Height/dy + window.Height%dy;
			return new float[w + h];
		}
	}

	/// <summary>
	/// Description: Counts the black pixel in each row and each column to compute the feature vector
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-30-2004</para>
	/// </summary>
	public class BlackPxCounter : OCRImgExtCounter
	{
		/// <summary>
		/// The Defaultconstructor, does nothin'
		/// </summary>
		public BlackPxCounter() {}

		/// <summary>
		/// Counts the black pixel in each row and each column to compute the feature vector
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <returns>the feature vector</returns>
		public override float[] computeFeatureVec(Bitmap srcBmp)
		{
			return computeFeatureVec(srcBmp, new Rectangle(0, 0, srcBmp.Width, srcBmp.Height));
		}

		/// <summary>
		/// Counts the black pixel in each row and each column to compute the feature vector
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <param name="window">the window which should be extracted</param>
		/// <returns>the feature vector</returns>
		public override float[] computeFeatureVec(Bitmap srcBmp, Rectangle window)
		{
			float[] patternVector = computePatternVec(srcBmp, window);
			float[] featureVector = init(window);
			int nBlackPx = 0, offsetFeat = 0;
			int offPat = 0;

			int y;
			for(y=0; y<window.Height; y+=dy, nBlackPx=0)
			{
				for(int x=offPat; x<(window.Width + offPat); x++)	
					if(patternVector[x] == 1)
						nBlackPx++;
				featureVector[y/dy] = (float)nBlackPx / window.Width;
				offPat += window.Width;
			}

			offsetFeat = y/dy;
			offPat = window.Width*window.Height - window.Height;
			nBlackPx = 0;
			for(int x=0; x<window.Width; x+=dx, nBlackPx=0)
			{
				for(y=x; y<(offPat + x); y+=window.Width)
					if(patternVector[y] == 1)
						nBlackPx++;
				featureVector[offsetFeat + x/dx] = (float)nBlackPx / window.Height;
			}
			return featureVector;
		}
	}

	/// <summary>
	/// Description: Counts the black pixel in each row and each column and adds them to compute the feature vector
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-30-2004</para>
	/// </summary>
	public class BlackPxAddRowAndColCounter : OCRImgExtCounter
	{
		/// <summary>
		/// The Defaultconstructor, does nothin'
		/// </summary>
		public BlackPxAddRowAndColCounter() {}

		/// <summary>
		/// Initialize the size of the feature vector
		/// </summary>
		/// <param name="window">the window for extracting</param>
		/// <returns>the feature vector</returns>
		protected new float[] init(Rectangle window)
		{
			int w = window.Width/dx + window.Width%dx;
			int h = window.Height/dy + window.Height%dy;
			return new float[Math.Max(w, h)];
		}

		/// <summary>
		/// Counts the black pixel in each row and each column and adds them to compute the feature vector
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <returns>the feature vector</returns>
		public override float[] computeFeatureVec(Bitmap srcBmp)
		{
			return computeFeatureVec(srcBmp, new Rectangle(0, 0, srcBmp.Width, srcBmp.Height));
		}

		/// <summary>
		/// Counts the black pixel in each row and each column and adds them to compute the feature vector
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <param name="window">the window which should be extracted</param>
		/// <returns>the feature vector</returns>
		public override float[] computeFeatureVec(Bitmap srcBmp, Rectangle window)
		{
			float[] patternVector = computePatternVec(srcBmp, window);
			float[] featureVector = init(window);
			int nBlackPx = 0;
			int offPat = 0;

			int y;
			for(y=0; y<window.Height; y+=dy, nBlackPx=0)
			{
				for(int x=offPat; x<(window.Width + offPat); x++)	
					if(patternVector[x] == 1)
						nBlackPx++;
				featureVector[y/dy] = (float)nBlackPx;
				offPat += window.Width;
			}

			offPat = window.Width*window.Height - window.Height;
			nBlackPx = 0;
			for(int x=0; x<window.Width; x+=dx, nBlackPx=0)
			{
				for(y=x; y<(offPat + x); y+=window.Width)
					if(patternVector[y] == 1)
						nBlackPx++;
				featureVector[x/dx] += (float)nBlackPx;
				featureVector[x/dx] /= (window.Height + window.Width);
			}
			return featureVector;
		}
	}

	/// <summary>
	/// Description: Counts the black and white pixel changes in each row and each column
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-30-2004</para>
	/// </summary>
	public class BlackPxWhitePxChangesCounter : OCRImgExtCounter
	{
		/// <summary>
		/// The Defaultconstructor, does nothin'
		/// </summary>
		public BlackPxWhitePxChangesCounter() {}

		/// <summary>
		/// Counts the black and white pixel changes in each row and each column
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <returns>the feature vector</returns>
		public override float[] computeFeatureVec(Bitmap srcBmp)
		{
			return computeFeatureVec(srcBmp, new Rectangle(0, 0, srcBmp.Width, srcBmp.Height));
		}

		/// <summary>
		/// Counts the black and white pixel changes in each row and each column
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <param name="window">the window which should be extracted</param>
		/// <returns>the feature vector</returns>
		public override float[] computeFeatureVec(Bitmap srcBmp, Rectangle window)
		{
			float[] patternVector = computePatternVec(srcBmp, window);
			float[] featureVector = init(window);
			int changes = 0, offsetFeat = 0;
			int offPat = 0;

			int y;
			for(y=0; y<window.Height; y+=dy, changes=0)
			{
				for(int x=offPat; x<(window.Width + offPat - 1); x++)	
					if(patternVector[x] != patternVector[x+1])
						changes++;
				featureVector[y/dy] = (float)changes / window.Width;
				offPat += window.Width;
			}

			offsetFeat = y/dy;
			offPat = window.Width*window.Height - window.Height;
			changes = 0;
			for(int x=0; x<window.Width; x+=dx, changes=0)
			{
				for(y=x; y<(offPat + x - window.Width); y+=window.Width)
					if(patternVector[y] != patternVector[y+window.Width])
						changes++;
				featureVector[offsetFeat + x/dx] = (float)changes / window.Height;
			}
			return featureVector;
		}
	}

	/// <summary>
	/// Description: Segments the image in nSegments rows and nSegments columns. 
	/// Counts the black pixels in each segment, the sum of each segment is normalized. 
	/// One item of the the feature vector == normalized sum of black pixels in a segement
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-30-2004</para>
	/// </summary>
	public class ImgSegmenter : OCRImageExtractor
	{
		/// <summary>
		/// The image would be segmented in nSegments rows and nSegments columns; default -> 4
		/// </summary>
		public int nSegmentsProp
		{
			get { return nSegments; }
			set { nSegments = value; }
		}
		/// <summary>
		/// The image would be segmented in nSegments rows and nSegments columns; default -> 4
		/// </summary>
		protected int nSegments = 4;

		/// <summary>
		/// The Defaultconstructor, does nothin'
		/// </summary>
		public ImgSegmenter() {}

		/// <summary>
		/// Segments the image in nSegments rows and nSegments columns. 
		/// Counts the black pixels in each segment, the sum of each segment is normalized. 
		/// One item of the the feature vector == normalized sum of black pixels in a segement
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <returns>the feature vector</returns>
		public override float[] computeFeatureVec(Bitmap srcBmp)
		{
			return computeFeatureVec(srcBmp, new Rectangle(0, 0, srcBmp.Width, srcBmp.Height));
		}

		/// <summary>
		/// Segments the image in nSegments rows and nSegments columns. 
		/// Counts the black pixels in each segment, the sum of each segment is normalized. 
		/// One item of the the feature vector == normalized sum of black pixels in a segement
		/// </summary>
		/// <param name="srcBmp">the source image which should be extracted</param>
		/// <param name="window">the window which should be extracted</param>
		/// <returns>the feature vector</returns>
		public override float[] computeFeatureVec(Bitmap srcBmp, Rectangle window)
		{
			if(srcBmp == null)
				throw new Exception("No input image to extract!");
			
			Bitmap tBmp = new Bitmap(window.Width, window.Height);
			Graphics gfx = Graphics.FromImage(tBmp);
			gfx.DrawImage(srcBmp, 0, 0, window, GraphicsUnit.Pixel);
			srcBmp = tBmp;

			if(nSegments == 0)
				nSegments = 1;
			if(nSegments > srcBmp.Width)
				nSegments = srcBmp.Width;
			if(nSegments > srcBmp.Height)
				nSegments = srcBmp.Height;

			Size segmSize = new Size(srcBmp.Width/nSegments, srcBmp.Height/nSegments);
			if(segmSize.Height == 0)
				segmSize.Height = 1;
			if(segmSize.Width == 0)
				segmSize.Width = 1;

			int restPxX =  srcBmp.Width - segmSize.Width*nSegments;
			int restPxY =  srcBmp.Height - segmSize.Height*nSegments;
			int restRows = 0, restCols = 0;
			if(restPxX > 0)
			{
				restCols += restPxX/segmSize.Width;
				if(restPxX%segmSize.Width != 0)
					restCols++;
			}
			if(restPxY > 0)
			{
				restRows += restPxY/segmSize.Height;
				if(restPxY%segmSize.Height != 0)
					restRows++;
			}
			int nSegmTotal = (nSegments+restCols) * (nSegments+restRows);

			float[] featureVector = new float[nSegmTotal];			
			float blackSum;
			int index = 0;

			for(int yOff=0; yOff<srcBmp.Height; yOff+=segmSize.Height)
			{
				for(int xOff=0; xOff<srcBmp.Width; xOff+=segmSize.Width)	
				{
					blackSum = 0.0f;
					int y=0, x=0;
					for(y=yOff; y<(yOff+segmSize.Height) && y<srcBmp.Height; y++)
					{						
						for(x=xOff; x<(xOff+segmSize.Width) && x<srcBmp.Width; x++)	
						{
							if(srcBmp.GetPixel(x, y).GetBrightness() < threshold)
								blackSum += 1.0f;
						}
					}
					featureVector[index++] = blackSum/((x-xOff)*(y-yOff));
				} 
			}
			return featureVector;
		}
	}
}
