using System;
using System.Drawing;
using System.Windows.Forms;

namespace MultilayerNet
{
	/// <summary>
	/// Just the testing class for the network
	/// </summary>
	class main
	{
		static void writeMSEToFile(float error)
		{
			FileManager.writeFloatToTextFile("error.txt", error);
		}
		static LearningAlgo.errorHandler writeErrorToFile = new LearningAlgo.errorHandler(writeMSEToFile);

		static void showOutput(NeuralNet nn, int currentPattern)
		{
			Console.Write("Learn: ");
			for(int j=0; j<nn.learnAlgoProp.teachOutputProp[currentPattern].Length; j++)
				Console.Write("{0,5:F3} ", nn.learnAlgoProp.teachOutputProp[currentPattern][j]);
			Console.Write("\n Real: ");
			for(int j=0; j<nn.outputProp.Length; j++)
				Console.Write("{0,5:F3} ", nn.outputProp[j]);
			Console.WriteLine("\n--- Cycle: {0}/{1} --- \n", nn.learnAlgoProp.iterationProp, nn.learnAlgoProp.maxIterationProp);
		}
		static LearningAlgo.outputHandler showOut = new LearningAlgo.outputHandler(showOutput);

		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			DateTime startTime = DateTime.Now;

			const int N_NUMBS = 8;
			const int N_NOISEPICS_EACH = 2;

			/////////////////////////////////////////////////////////////////////////////////////////////7
			/*	/// output für lernpattern an nn testen
					NeuralNet nn = FileManager.readNetworkFromXml("No.0-9_8-16-8_trained_black_px_counted.net");
					showOutputAfterLearning(nn, nn.learnAlgoProp.teachOutputProp.Length);
			*/
			///////////////////////////////////////////////////////////////////////////////////////////////
			/*	/// testnetz 4 x 4 Px Bilder
				///
					const int N_PAT = 7;
					PreProcImage[] numbs = new PreProcImage[N_PAT];
					float[][] input = new float[N_PAT][];
					float[][] teach = new float[N_PAT][];
					int inNeurons = 1;
					for(int i=0; i<N_PAT; i++)
					{
						numbs[i] = new PreProcImage("test_" + i + ".bmp", new Size(4, 4));
						numbs[i].scanBmpAndFillFeatureVector();
						input[i] = numbs[i].featureVecProp[0];
						//	input[i] = numbs[i].patternVecProp;

						//		input[i] = new float[inNeurons];
						//		for(int j=0; j<inNeurons; j++)
						//			input[i][j] = (i*0.1f);
						// Teachoutputvektor anlegen...
						teach[i] = new float[N_PAT];
						teach[i][i] = 1.0f;
					}

					NeuralNet nn = new NeuralNet(input[0], new int[]{input.Length}, N_PAT);
					nn.learnAlgoProp.maxIterationProp = 500;
					(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.6f;
					(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.5f;
					(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.01f;


					nn.learnAlgoProp.inputProp = input;
					nn.learnAlgoProp.teachOutputProp = teach;

					string filename = "test_4x4_" + input[0].Length.ToString();
					for(int i=0; i<nn.layersProp.Length; i++)
						filename += "-" + nn.layersProp[i].Length;
					filename += ".net";
					FileManager.writeNetworkToXml(filename, nn);

			*/
			/*	///////////////////////////////////////////////////////////////////////////////////////////////
				/// testnetz anlegen
					const int N_PATTERNS = 14;
					float[][] input = new float[N_PATTERNS][];
					float[][] teach = new float[N_PATTERNS][];

					for(int i=0; i<N_PATTERNS; i++)
					{
						input[i] = new float[N_PATTERNS];
						teach[i] = new float[N_PATTERNS];
						input[i][i] = teach[i][i] = 1.0f;
					}

					int nNeuronsInHidden = 3*input[0].Length;
					NeuralNet nn = new NeuralNet(input[0], new int[]{nNeuronsInHidden}, teach[0].Length);
					nn.learnAlgoProp.maxIterationProp = 1000;
					(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.6f;
					(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.6f;
					(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.01f;

					nn.learnAlgoProp.inputProp = input;
					nn.learnAlgoProp.teachOutputProp = teach;

					string filename = input[0].Length.ToString();
					for(int i=0; i<nn.layersProp.Length; i++)
						filename += "-" + nn.layersProp[i].Length;
					filename += ".net";
					FileManager.writeNetworkToXml(filename, nn);
		*/

	/*		//////////////////////////////////////////////////////////////////////////////////////////////////////////
			//  Zahlen 0-9 (5x7) erzeugen und in XML speichern
			
			float[][] input = new float[][]{   new float[] {   0,1,1,1,0,
															   1,0,0,0,1,
															   1,0,0,0,1,
															   1,0,0,0,1,
															   1,0,0,0,1,
															   1,0,0,0,1,
															   0,1,1,1,0},

											   new float[] {   0,0,1,0,0,
															   0,1,1,0,0,
															   1,0,1,0,0,
															   0,0,1,0,0,
															   0,0,1,0,0,
															   0,0,1,0,0,
															   0,0,1,0,0},

											   new float[] {   0,1,1,1,0,
															   1,0,0,0,1,
															   0,0,0,0,1,
															   0,0,0,1,0,
															   0,0,1,0,0,
															   0,1,0,0,0,
															   1,1,1,1,1},

											   new float[] {   0,1,1,1,0,
															   1,0,0,0,1,
															   0,0,0,0,1,
															   0,1,1,1,0,
															   0,0,0,0,1,
															   1,0,0,0,1,
															   0,1,1,1,0},

											   new float[] {   0,0,0,1,0,
															   0,0,1,1,0,
															   0,1,0,1,0,
															   1,0,0,1,0,
															   1,1,1,1,1,
															   0,0,0,1,0,
															   0,0,0,1,0},

											   new float[] {   1,1,1,1,1,
															   1,0,0,0,0,
															   1,0,0,0,0,
															   1,1,1,1,0,
															   0,0,0,0,1,
															   1,0,0,0,1,
															   0,1,1,1,0},

											   new float[] {   0,1,1,1,0,
															   1,0,0,0,1,
															   1,0,0,0,0,
															   1,1,1,1,0,
															   1,0,0,0,1,
															   1,0,0,0,1,
															   0,1,1,1,0},

											   new float[] {   1,1,1,1,1,
															   0,0,0,0,1,
															   0,0,0,0,1,
															   0,0,0,1,0,
															   0,0,1,0,0,
															   0,1,0,0,0,
															   1,0,0,0,0},

											   new float[] {   0,1,1,1,0,
															   1,0,0,0,1,
															   1,0,0,0,1,
															   0,1,1,1,0,
															   1,0,0,0,1,
															   1,0,0,0,1,
															   0,1,1,1,0},

											   new float[] {   0,1,1,1,0,
															   1,0,0,0,1,
															   1,0,0,0,1,
															   0,1,1,1,1,
															   0,0,0,0,1,
															   1,0,0,0,1,
															   0,1,1,1,0}};

			float[][] teach = new float[10][];
			for(int i=0; i<teach.Length; i++)
			{
				teach[i] = new float[teach.Length];
				teach[i][i] = 1;
			}

			NeuralNet nn = new NeuralNet(input[0], new int[]{10}, teach.Length);
			nn.learnAlgoProp.maxIterationProp = 5000;
			(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.3f;
			(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.6f;
			(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.0f;

			nn.learnAlgoProp.inputProp = input;
			nn.learnAlgoProp.teachOutputProp = teach;

			string filename = input[0].Length.ToString();
			for(int i=0; i<nn.layersProp.Length; i++)
				filename += "-" + nn.layersProp[i].Length;
			filename += "_Digits_0-9_5x7.net";
			FileManager.writeNetworkToXml(filename, nn, "Digits_0-9_5x7");
*/


	/*		//////////////////////////////////////////////////////////////////////////////////////////////////////////
			//  Characters erzeugen und in XML speichern
			byte firstChar =  (byte)'A';
			byte lastChar = (byte)'D';
			int charsCount = lastChar - firstChar + 1;
			float[][] input = new float[charsCount*N_NOISEPICS_EACH][];
			float[][] teach = new float[charsCount*N_NOISEPICS_EACH][];
			OCRImageCreator chara;
			OCRImgExtCounter ext;
			for (int i= 0; i<charsCount; i++)
			{
				for(int j=0; j<N_NOISEPICS_EACH; j++)
				{
					chara = new OCRImageCreator(Convert.ToChar(firstChar + i));
					//Noise
				//	chara.addNoiseToImg(2);
					//speichern
				//	chara.bmpProp.Save(Convert.ToChar(firstChar + i) + "_noise" + j + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);

					// Inputvektor anlegen
					ext = new BlackPxAddRowAndColCounter(chara.bmpProp);
					input[i*N_NOISEPICS_EACH+j] = ext.featureVecProp;
					// Teachoutputvektor anlegen...
					teach[i*N_NOISEPICS_EACH+j] = new float[charsCount];
					teach[i*N_NOISEPICS_EACH+j][i] = 1.0f;
				}
			}

			NeuralNet nn = new NeuralNet(input[0], new int[]{charsCount}, charsCount);
			nn.learnAlgoProp.maxIterationProp = 500;
			(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.3f;
			(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.6f;
			(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.01f;

			nn.learnAlgoProp.inputProp = input;
			nn.learnAlgoProp.teachOutputProp = teach;

			string filename = input[0].Length.ToString();
			for(int i=0; i<nn.layersProp.Length; i++)
				filename += "-" + nn.layersProp[i].Length;
			filename += "_" + Convert.ToChar(firstChar).ToString() + "-" + Convert.ToChar(lastChar).ToString();
			filename += "_with_each_" + N_NOISEPICS_EACH + "_noisyPatterns_untrained_black_px_counted_10pxChars.net";
			FileManager.writeNetworkToXml(filename, nn, "A-Z_untrained");

		/////////////////////////////////////////////////////////////////////////////////////
	*/	//	Compute and save some noisy test images
	/*
			ImageFilter img;
			for(int i=0; i<N_NUMBS; i++)
			{
				img = new ImageFilter(i + ".bmp");
				for(int j=0; j<N_NOISEPICS_EACH; j++)
				{
					img.computeNoiseImg(5);
					img.noiseBmpProp.Save("NoisyNumb_" + i + "_pattern_" + j + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
				}
			}

		//  verrauchte Zahlen 0-9 (16x24) lernen
			float[][] teachPats = new float[][]{new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
												new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
												new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
												new float[] {0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
												new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
												new float[] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
												new float[] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
												new float[] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
												new float[] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
												new float[] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f}};
			float[][] teach = new float[N_NUMBS*N_NOISEPICS_EACH][];
			float[][] input = new float[N_NUMBS*N_NOISEPICS_EACH][];
			PreProcImage[] numbs = new PreProcImage[N_NUMBS*N_NOISEPICS_EACH];

			for(int i=0; i<N_NUMBS; i++)
				for(int j=0; j<N_NOISEPICS_EACH; j++)
				{
					numbs[i*N_NOISEPICS_EACH+j] = new PreProcImage("NoisyNumb_" + i + "_pattern_" + j + ".bmp");
					numbs[i*N_NOISEPICS_EACH+j].scanBmpAndFillFeatureVector();
					input[i*N_NOISEPICS_EACH+j] = numbs[i*N_NOISEPICS_EACH+j].featureVecProp[0];
					teach[i*N_NOISEPICS_EACH+j] = teachPats[i];
				}

			NeuralNet nn = new NeuralNet(numbs[0].featureVecProp[0], new int[]{10, 10}, 10);
			nn.learnAlgoProp.maxIterationProp = 10000;
			(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.5f;
			(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.0f;
			(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.0f;
			(nn.learnAlgoProp as Backpropagation).onErrorCalculated += writeErrorToFile;
			(nn.learnAlgoProp as Backpropagation).onOutCalculated += showOut;

			Console.WriteLine("<-------- LEARNING -------->\n");
			nn.learnAlgoProp.learnPatterns(teach, input);
			Console.WriteLine("<--- LEARNING FINISHED ---->");
			FileManager.writeNetworkToXml("0-9_trained_all_patterns.net", nn, "0-9_trained_all_patterns");

			Console.WriteLine("\n ---------------- Output after learning");
			for(int i=0; i<teach.GetUpperBound(0)+1;i++)
			{
				nn.inputProp = input[i];
				Console.Write("Learned: ");
				for(int j=0; j<teach[i].Length; j++)
					Console.Write("{0,5:F3} ", teach[i][j]);
				Console.Write("\nRealOut: ");
				for(int j=0; j<nn.outputProp.Length; j++)
					Console.Write("{0,5:F3} ", nn.outputProp[j]);
				Console.WriteLine("\n");
			}
			Console.WriteLine("\n< = - FIN - = >");

	*//*
			PreProcImage two = new PreProcImage("2.bmp");

			NeuralNet nn = new NeuralNet(two.featureVecProp[0], new int[]{10}, 10);
			(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.3f;
			(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.6f;
			(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.1f;

			float[] teach = new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};
			nn.learnAlgoProp.learnOnePattern(teach);
			Console.Write("Learned: ");
			for(int j=0; j<teach.Length; j++)
				Console.Write("{0,5:F3} ", teach[j]);
			Console.Write("\nRealOut: ");
			for(int j=0; j<nn.outputProp.Length; j++)
				Console.Write("{0,5:F3} ", nn.outputProp[j]);
			Console.WriteLine("\n");

			Console.WriteLine("\n\n+++++++++++++++++++++++++++ Doing Pic Work ++++++++++++++++++++++++++++++");
			PreProcImage img = new PreProcImage("2_transl.bmp");
			Console.WriteLine("\n\n+++++++++++++++++++++++++++ scan finished! ++++++++++++++++++++++++++++++\n");
			for(int i=0; i<30;i++) //img.featureVecProp.GetUpperBound(0)+1
			{
				nn.inputProp = img.featureVecProp[i];
				Console.Write("\nRealOut: ");
				for(int j=0; j<nn.outputProp.Length; j++)
					Console.Write("{0,5:F9} ", nn.outputProp[j]);
				Console.WriteLine("\n");
			}

			Console.WriteLine("\n< = - FIN - = >");

	*/	//////////////////////////////////////////////////////////////////////////////////////////////////////////
/*	//  saubere Zahlen 0-9 (16x24) lernen
			PreProcImage[] numbs = new PreProcImage[N_NUMBS];
			float[][] input = new float[N_NUMBS][];
			float[][] teach = new float[N_NUMBS][];
			int inNeurons = 1;
			for(int i=0; i<N_NUMBS; i++)
			{
				numbs[i] = new PreProcImage(i + ".bmp", new Size(8, 8));
				numbs[i].scanBmpAndFillFeatureVector();
				input[i] = numbs[i].featureVecProp[0];
			//	input[i] = numbs[i].patternVecProp;

		//		input[i] = new float[inNeurons];
		//		for(int j=0; j<inNeurons; j++)
		//			input[i][j] = (i*0.1f);
			 // Teachoutputvektor anlegen...
				teach[i] = new float[N_NUMBS];
				teach[i][i] = 1.0f;
			}

			NeuralNet nn = new NeuralNet(input[0], new int[]{3*input.Length}, teach.Length);
			nn.learnAlgoProp.maxIterationProp = 500;
			(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.6f;
			(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.6f;
			(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.01f;
		//	(nn.learnAlgoProp as Backpropagation).onErrorCalculated += writeErrorToFile;
		//	(nn.learnAlgoProp as Backpropagation).onOutCalculated += showOut;

			nn.learnAlgoProp.inputProp = input;
			nn.learnAlgoProp.teachOutputProp = teach;

			string filename = "No.0-9_" + input[0].Length.ToString();
			for(int i=0; i<nn.layersProp.Length; i++)
				filename += "-" + nn.layersProp[i].Length;
			filename += "_untrained_black_px_counted.net";
			FileManager.writeNetworkToXml(filename, nn, "0-9_untrained");

			nn.learnAlgoProp.learnPatterns();

				FileManager.writeNetworkToXml("0-9_trained_" + nn.learnAlgoProp.maxIterationProp.ToString() + "_cycles.net", nn, "0-9_trained");
			Console.WriteLine("\n<-------- LEARNING -------->\n");
			for(float i=0; i<0.5f; i+=0.1f)
			{
				nn = new NeuralNet(input[0], new int[]{10}, N_NUMBS);
				(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.1f + i;
				(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.1f;
				(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.0f;
				(nn.learnAlgoProp as Backpropagation).errorCalculated += writeErrorToFile;
				nn.learnAlgoProp.learnPatterns(teach, input);
			}
			for(float i=0; i<0.5f; i+=0.1f)
			{
				nn = new NeuralNet(input[0], new int[]{10}, N_NUMBS);
				(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.01f + i;
				(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.01f + i;
				(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.0f;
				(nn.learnAlgoProp as Backpropagation).errorCalculated += writeErrorToFile;
				nn.learnAlgoProp.learnPatterns(teach, input);
			}
			for(float i=0; i<0.5f; i+=0.1f)
			{
				nn = new NeuralNet(input[0], new int[]{10}, N_NUMBS);
				(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.1f;
				(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.0f + i;
				(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.0f;
				(nn.learnAlgoProp as Backpropagation).errorCalculated += writeErrorToFile;
				nn.learnAlgoProp.learnPatterns(teach, input);
			}
			Console.WriteLine("<--- LEARNING FINISHED ---->");
			showOutputAfterLearning(nn);
			Console.WriteLine("\n< = - FIN - = >");

*/		////////////////////////////////////////////////////////////////////////////////////////////////////7
/*		// Iwes Klassifizierer
			float[][] input = new float[][]{	new float[] {0.05f, 0.02f},
												new float[] {0.09f, 0.11f},
												new float[] {0.12f, 0.20f},
												new float[] {0.15f, 0.22f},
												new float[] {0.20f, 0.25f},
												new float[] {0.75f, 0.75f},
												new float[] {0.80f, 0.83f},
												new float[] {0.82f, 0.80f},
												new float[] {0.90f, 0.89f},
												new float[] {0.95f, 0.89f},
												new float[] {0.09f, 0.04f},
												new float[] {0.10f, 0.10f},
												new float[] {0.14f, 0.21f},
												new float[] {0.18f, 0.24f},
												new float[] {0.22f, 0.28f},
												new float[] {0.77f, 0.78f},
												new float[] {0.79f, 0.81f},
												new float[] {0.84f, 0.82f},
												new float[] {0.94f, 0.93f},
												new float[] {0.98f, 0.99f}};

			float[][] teach = new float[][]{	new float[] {1.0f, 0.0f},
												new float[] {1.0f, 0.0f},
												new float[] {1.0f, 0.0f},
												new float[] {1.0f, 0.0f},
												new float[] {1.0f, 0.0f},
												new float[] {0.0f, 1.0f},
												new float[] {0.0f, 1.0f},
												new float[] {0.0f, 1.0f},
												new float[] {0.0f, 1.0f},
												new float[] {0.0f, 1.0f},
												new float[] {1.0f, 0.0f},
												new float[] {1.0f, 0.0f},
												new float[] {1.0f, 0.0f},
												new float[] {1.0f, 0.0f},
												new float[] {1.0f, 0.0f},
												new float[] {0.0f, 1.0f},
												new float[] {0.0f, 1.0f},
												new float[] {0.0f, 1.0f},
												new float[] {0.0f, 1.0f},
												new float[] {0.0f, 1.0f}};

			NeuralNet nn = new NeuralNet(input[0], new int[]{3, 3}, teach[0].Length);
		//	nn.weightNetProp = FileManager.readNetWeightFromXml("Simple_2-3-3-2_classification_net_untrained_containing_20_patterns.net");
			(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.3f;
			(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.6f;
			(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.0f;
		//	(nn.learnAlgoProp as Backpropagation).onErrorCalculated += writeErrorToFile;
		//	(nn.learnAlgoProp as Backpropagation).onOutCalculated += showOut;

			nn.learnAlgoProp.inputProp = input;
			nn.learnAlgoProp.teachOutputProp = teach;

			FileManager.writeNetworkToXml("10-3-3-10.net", nn);
			Console.WriteLine("<-------- LEARNING -------->\n");
			nn.learnAlgoProp.learnPatterns(teach, input);
			Console.WriteLine("<--- LEARNING FINISHED ---->");

			showOutputAfterLearning(nn);

			Console.WriteLine("\n< = - FIN - = >");
	*/

	/*		////////////////// ONLINE /////////////////////////
			// Waagerechten und senkrechten Strich lernen (5x7)
			Bitmap bmp = new Bitmap("waagerecht.bmp");
			float[] brightnessVecW = new float[bmp.Height * bmp.Width];
			for(int y=0; y<bmp.Height; y++)
			{
				for(int x=0; x<bmp.Width; x++)
				{
					brightnessVecW[y*bmp.Width+x] = bmp.GetPixel(x, y).GetBrightness();
					Console.Write(brightnessVecW[y*bmp.Width+x]);
				}
				Console.WriteLine();
			}
			bmp = new Bitmap("senkrecht.bmp");
			float[] brightnessVecS = new float[bmp.Height * bmp.Width];
			for(int y=0; y<bmp.Height; y++)
				for(int x=0; x<bmp.Width; x++)
					brightnessVecS[y*bmp.Width+x] = bmp.GetPixel(x, y).GetBrightness();

			NeuralNet nn = new NeuralNet(brightnessVecW, new int[]{10}, 2);
			nn.learnAlgoProp.maxIterationProp = 5000;
		//	(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.8f;
		//	(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.6f;
		//	(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.1f;
			(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.1f;
			(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.1f;
			(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.0f;
			(nn.learnAlgoProp as Backpropagation).errorCalculated += writeErrorToFile;

			FileManager.writeNetworkToXml("StricheVormLernen.net", nn, "stricher");

			float[] W =  new float[]{1.0f, 0.0f};
			float[] S = new float[]{0.0f, 1.0f};
			float[][] teach = new float[][]{W, S};
			float[][] input = new float[][]{brightnessVecW, brightnessVecS};

			nn.learnAlgoProp.learnPatterns(teach, input);
			Console.WriteLine("\n ---------------- Output after learning");
			for(int i=0; i<teach.GetUpperBound(0)+1;i++)
			{
				nn.inputProp = input[i];
				Console.Write("Learned: ");
				for(int j=0; j<teach[i].Length; j++)
					Console.Write("{0,5:F3} ", teach[i][j]);
				Console.Write("\nRealOut: ");
				for(int j=0; j<nn.outputProp.Length; j++)
					Console.Write("{0,5:F3} ", nn.outputProp[j]);
				Console.WriteLine("\n");
			}

			Console.WriteLine("\n< = - FIN - = >");
			(nn.learnAlgoProp as Backpropagation).errorCalculated -= writeErrorToFile;
	*/
			// Buchstaben A + B lernen
	/*		// Bitmaps laden, lernen lassen, XML Net speichern
			Bitmap bmp = new Bitmap("A.bmp");
			float[] brightnessVecA = new float[bmp.Height * bmp.Width];
			for(int y=0; y<bmp.Height; y++)
			{
				for(int x=0; x<bmp.Width; x++)
				{
					brightnessVecA[y*bmp.Width+x] = bmp.GetPixel(x, y).GetBrightness();
					Console.Write(brightnessVecA[y*bmp.Width+x]);
				}
				Console.WriteLine();
			}

			NeuralNet nn = new NeuralNet(brightnessVecA, new int[]{20}, 2);
			nn.learnAlgoProp.maxIterationProp = 100;
			(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.2f;
			(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.6f;
			(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.1f;
			(nn.learnAlgoProp as Backpropagation).errorCalculated += writeErrorToFile;

			float[] A =  new float[]{1.0f, 0.0f};
			nn.learnAlgoProp.learnOnePattern(A);
			Console.WriteLine("finished! learning A=1.0f {0} * {1}- Output: {2}", bmp.Height, bmp.Width, nn.outputProp[0]);

			FileManager.writeNetworkToXml("AB_after_learning_1A.net", nn, "after_learning");

			bmp = new Bitmap("B.bmp");
			float[] brightnessVecB = new float[bmp.Height * bmp.Width];
			for(int y=0; y<bmp.Height; y++)
				for(int x=0; x<bmp.Width; x++)
						brightnessVecB[y*bmp.Width+x] = bmp.GetPixel(x, y).GetBrightness();
			nn.inputProp = brightnessVecB;
			float[] B = new float[]{0.0f, 1.0f};
			nn.learnAlgoProp.learnOnePattern(B);
			Console.WriteLine("finished! learning B=0.0f {0} * {1}- Output: {2}", bmp.Height, bmp.Width, nn.outputProp[1]);

			nn.inputProp = brightnessVecA;
			Console.WriteLine("Output for A=1.0f: {0}", nn.outputProp[0]);
			nn.inputProp = brightnessVecB;
			Console.WriteLine("Output for B=0.0f: {0}", nn.outputProp[1]);

			//nn.learnAlgoProp.maxIterationProp = 1000;
			nn.inputProp = brightnessVecA;
			nn.learnAlgoProp.learnOnePattern(A);
			Console.WriteLine("2nd finished! learning A=1.0f: {0}", nn.outputProp[0]);
			nn.inputProp = brightnessVecB;
			Console.WriteLine("Output for B=0.0f: {0}", nn.outputProp[0]);
			nn.learnAlgoProp.learnOnePattern(B);
			Console.WriteLine("\n2nd finished! learning B=0.0f: {0}", nn.outputProp[1]);
			nn.inputProp = brightnessVecA;
			Console.WriteLine("Output for A=1.0f: {0}", nn.outputProp[0]);

			Console.WriteLine("\n< = - FIN - = >");
			(nn.learnAlgoProp as Backpropagation).errorCalculated -= writeErrorToFile;
 */

	/*		////////////// XOR //////////////////////////////////////////////////
			/// OPTIMALE PARAMETER FEHLEN !!!!!!!!

			float[][] input = new float[][]{new float[] {0.0f, 0.0f},
											new float[] {0.0f, 1.0f},
											new float[] {1.0f, 0.0f},
											new float[] {1.0f, 1.0f}};

			float[][] teach = new float[][]{new float[] {0.0f},
											new float[] {1.0f},
											new float[] {1.0f},
											new float[] {0.0f}};

			NeuralNet nn = new NeuralNet(input[0], new int[]{2}, 1);
			nn.learnAlgoProp.maxIterationProp = 5000;
			(nn.learnAlgoProp as Backpropagation).learningRateProp = 0.3f;
			(nn.learnAlgoProp as Backpropagation).momentumParamProp = 0.0f;
			(nn.learnAlgoProp as Backpropagation).daFlatFactorProp = 0.0f;
		//	(nn.learnAlgoProp as Backpropagation).onErrorCalculated += writeErrorToFile;
		//	(nn.learnAlgoProp as Backpropagation).onOutCalculated += showOut;

			nn.learnAlgoProp.inputProp = input;
			nn.learnAlgoProp.teachOutputProp = teach;
			FileManager.writeNetworkToXml("XOR_2-2-1_untrained.net", nn, "XOR_2-2-1_untrained");
			nn.learnAlgoProp.learnPatterns();
		//	FileManager.writeNetworkToXml("XOR_2-2-1_trained.net", nn, "XOR_2-2-1_trained");
			showOutputAfterLearning(nn);
			Console.WriteLine("-------------------->ITERS: " + nn.learnAlgoProp.iterationProp);
			printNN(nn);
	*/
	/*		///////////////////////////
			// Little Red Riding Hood
			NeuralNet nn = new NeuralNet(new float[]{1.0f, 0.0f, 0.0f}, new int[]{}, 3);

			nn.learnAlgoProp.learnOnePattern(new float[]{1.0f, 0.0f, 0.0f});
			printNN(nn);

			nn.inputProp = new float[]{0.0f, 1.0f, 1.0f};
			nn.learnAlgoProp.learnOnePattern(new float[]{0.0f, 1.0f, 1.0f});
			printNN(nn);

			nn.inputProp = new float[]{1.0f, 1.0f, 1.0f};
			nn.learnAlgoProp.learnOnePattern(new float[]{1.0f, 1.0f, 1.0f});
			printNN(nn);

			FileManager.writeNetworkToXml("mlp.net", nn, "Little Red Riding Hood");

		*/	///////////////////////////
		/*	NeuralNet nn = new NeuralNet(new float[]{1.0f, 0.0f, 0.0f}, new int[]{}, 3);

			float[][] input = new float[][]{new float[] {1.0f, 0.0f, 0.0f},
											new float[] {0.0f, 1.0f, 1.0f},
											new float[] {1.0f, 1.0f, 1.0f}};

			nn.learnAlgoProp.learnPatterns(input, input);
			// nn.learnAlgoProp.learnPatterns(input, input);
			printNN(nn);
			FileManager.writeNetworkToXml("LRRH_Online.net", nn, "Little Red Riding Hood Online");

			*//*
			 * runtimetest for an Array of neurons and an System.Collections.ArrayList of neurons
			 * result for 5000000 Neuronobjects / iterations
			 * Array time elapsed: 00:00:04:343
			 * ArrayList time elapsed: 00:00:40:906
			 */
			/*
			TimeSpan t;
			DateTime startTime;
			const int iters = 4000000;

			startTime = DateTime.Now;
			Neuron[] na = new Neuron[iters];
			for(int i =0; i < iters; i++)
				na[i] = new Neuron();
			float[] result = new float[iters];
			for(int i =0; i < iters; i++)
				result[i] = na[i].thresholdProp;
			t = DateTime.Now - startTime;
			Console.WriteLine("Array time elapsed: {0:00}:{1:00}:{2:00}:{3:000}\n", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);

			startTime = DateTime.Now;
			System.Collections.ArrayList nal = new System.Collections.ArrayList();
			for(int i =0; i < iters; i++)
				nal.Add(new Neuron());
			float[] result2 = new float[iters];
			for(int i =0; i < iters; i++)
				result[i] = (nal[i] as Neuron).thresholdProp;

			t = DateTime.Now - startTime;
			Console.WriteLine("ArrayList time elapsed: {0:00}:{1:00}:{2:00}:{3:000}\n", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
			*/

			TimeSpan ts = DateTime.Now - startTime;
			Console.WriteLine("Time elapsed: {0:00}:{1:00}:{2:00}:{3:000}\n", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
		//	Console.WriteLine("-------------------->ITERS: " + nn.learnAlgoProp.iterationProp);

		//	Console.ReadLine();

			Application.Run(new GUI());
		}

		public static void 	showOutputAfterLearning(NeuralNet nn, int howManyPatterns)
		{
			Console.WriteLine("\n ---------------- Output after learning --------------------------------");
			for(int i=0; i<nn.learnAlgoProp.teachOutputProp.GetUpperBound(0)+1 && i<howManyPatterns;i++)
			{
				nn.inputProp = nn.learnAlgoProp.inputProp[i];
				Console.Write("Learned: ");
				for(int j=0; j<nn.learnAlgoProp.teachOutputProp[i].Length; j++)
					Console.Write("{0,5:F3} ", nn.learnAlgoProp.teachOutputProp[i][j]);
				Console.Write("\nRealOut: ");
				for(int j=0; j<nn.outputProp.Length; j++)
					Console.Write("{0,5:F3} ", nn.outputProp[j]);
				Console.WriteLine("\n");
			}
		}


		public static void printNN(NeuralNet nn)
		{
			nn.propagate();
			Console.WriteLine("-----------------------------------------\n");
			for(int i=nn.layersProp.Length-1; i>=0; i--)
			{
				if(i<nn.layersProp.Length-1)
					Console.WriteLine("Hiddenlayer: {0}", i+1);
				else
					Console.WriteLine("Outputlayer:");
				Console.WriteLine("~~~~~~~~~~~~~~~");
				Console.WriteLine("Output:");
				for(int j=0; j<nn.layersProp[i].neuronsProp.Length; j++)
					Console.Write("{0,8:F3}", nn.layersProp[i].neuronsProp[j].outputProp);
				Console.WriteLine("");
				for(int j=0; j<nn.layersProp[i].neuronsProp.Length; j++)
					Console.Write("{0,8:F3}", "^|^ ");
				Console.WriteLine("");
				Console.WriteLine("Net:");
				for(int j=0; j<nn.layersProp[i].neuronsProp.Length; j++)
					Console.Write("{0,8:F3}", nn.layersProp[i].neuronsProp[j].netProp);
				Console.WriteLine("");
				Console.WriteLine("Input without Bias:");
				for(int k=0; k<nn.layersProp[i].inputProp.Length; k++)
				{
					for(int j=0; j<nn.layersProp[i].neuronsProp.Length; j++)
						Console.Write("{0,8:F3}", nn.layersProp[i].inputProp[k]);
					Console.WriteLine("");
				}
				Console.WriteLine("Weight with Bias:");
				for(int k=0; k<nn.weightNetProp[i][0].Length; k++)
				{
					for(int j=0; j<nn.weightNetProp[i].Length; j++)
						Console.Write("{0,8:F3}", nn.weightNetProp[i][j][k]);
					Console.WriteLine("");
				}
				Console.WriteLine("");
			}
			Console.WriteLine("Inputlayer:");
			Console.WriteLine("~~~~~~~~~~~~~~~");
			Console.WriteLine("Output with bias:");
			for(int j=0; j<nn.inputProp.Length; j++)
				Console.Write("{0,8:F3}", nn.inputProp[j]);
			Console.WriteLine("");
			Console.WriteLine("-----------------------------------------\n");

			Console.WriteLine("\nNetwork output:");
			for(int i=0; i<nn.outputProp.Length; i++)
				Console.WriteLine("{0,4:F3}", nn.outputProp[i]);
			Console.WriteLine("Network input without bias:");
			for(int i=0; i<nn.inputProp.Length; i++)
				Console.WriteLine("{0,4:F3}", nn.inputProp[i]);

			Console.WriteLine("");
			Console.WriteLine("Anzahl Layer (ohne Input): {0}", nn.layersProp.Length);
			int nNeurons = 0;
			for(int i=0; i<nn.layersProp.Length;i++)
				nNeurons += nn.layersProp[i].neuronsProp.Length;
			Console.WriteLine("Anzahl Neuronen insgesamt (ohne Input): {0}", nNeurons);
		}
	}
}