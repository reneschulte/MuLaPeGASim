using System;
using System.Collections;

namespace MultilayerNet
{

	/// <summary>
	/// Description: Backpropagation Multilayer Perceptron learning algorithm. 
	/// Using the Momentum method, Flat Spot Elimination and offline (batch) or online training.
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class Backpropagation : LearningAlgo
	{
		/// <summary>
		/// The event which is thrown, after the summed global error was calculated
		/// </summary>
		public override event errorHandler onErrorCalculated;

		/// <summary>
		/// The event which is thrown, after the output was calculated
		/// </summary>
		public override event outputHandler onOutCalculated;

		/// <summary>
		/// The learning rate for the network, default = 0.1
		/// </summary>
		public float learningRateProp { get{return learningRate;} set{ learningRate=value;} }
		/// <summary>
		/// The learning rate for the network, default = 0.1
		/// </summary>
		protected float learningRate = 0.1f;

		/// <summary>
		/// Returns the minimum value which could be used to set the minimum for adjusting the learning rate
		/// </summary>
		public float learningRateMinProp{ get{return 0.0f;} }

		/// <summary>
		/// Returns the maximum value which could be used to set the minimum for adjusting the learning rate
		/// </summary>
		public float learningRateMaxProp{ get{return 10.0f;} }

		/// <summary>
		/// Returns the step range which could be used to set the step for adjusting the learning rate value
		/// </summary>
		public float learningRateStepProp{ get{return 0.01f;} }

		/// <summary>
		/// The Momentum factor:
		/// <para>The inertia rate for the network, default = 0.2.
		/// Values for the momentumParam must be between 0.0f and 1.0f</para>
		/// </summary>
		public float momentumParamProp
		{
			get{return momentumParam;}
			set{ if(value >= momentumParamMinProp && value <= momentumParamMaxProp) momentumParam=value; }
		}
		/// <summary>
		/// The Momentum factor:
		/// <para>The inertia rate for the network, default = 0.2.
		/// Values for the momentumParam must be between 0.0f and 1.0f</para>
		/// </summary>
		protected float momentumParam = 0.2f;

		/// <summary>
		/// Returns the minimum value which could be used to set the minimum for adjusting the Momentum factor
		/// </summary>
		public float momentumParamMinProp{ get{return 0.0f;} }

		/// <summary>
		/// Returns the maximum value which could be used to set the minimum for adjusting the Momentum factor
		/// </summary>
		public float momentumParamMaxProp{ get{return 0.9f;} }

		/// <summary>
		/// Returns the step range which could be used to set the step for adjusting the Momentum factor value
		/// </summary>
		public float momentumParamStepProp{ get{return 0.01f;} }

		/// <summary>
		/// The factor for Flat Spot Elimination:
		/// <para>added to the derivation of the activation function -> default = 0.
		/// Values must be between 0 and 1</para>
		/// </summary>
		public float daFlatFactorProp
		{
			get{return daFlatFactor;}
			set{ if(value >= daFlatFactorMinProp && value <= daFlatFactorMaxProp) daFlatFactor=value; }
		}

		/// <summary>
		/// The factor for Flat Spot Elimination:
		/// <para>added to the derivation of the activation function -> default = 0.
		/// Values must be between 0 and 1</para>
		/// </summary>
		protected float daFlatFactor = 0.0f;

		/// <summary>
		/// Returns the minimum value which could be used to set the minimum for adjusting the Flat Spot factor
		/// </summary>
		public float daFlatFactorMinProp{ get{return 0.0f;} }

		/// <summary>
		/// Returns the maximum value which could be used to set the minimum for adjusting the Flat Spot factor
		/// </summary>
		public float daFlatFactorMaxProp{ get{return 1.0f;} }

		/// <summary>
		/// Returns the step range which could be used to set the step for adjusting the Flat Spot factor value
		/// </summary>
		public float daFlatFactorStepProp{ get{return 0.01f;} }

		/// <summary>
		/// The weight change of the pattern before(t-1) the current pattern(t). Needed for momentum method
		/// </summary>
		protected float[][][] dwOld;

		/// <summary>
		/// The weight change of the pattern
		/// </summary>
		protected float[][][] weightChange;

		/// <summary>
		/// Contains the String(s) with the specific properties for the Backpropagation algorithm
		/// <see cref="MultilayerNet.LearningAlgo.algoProps"/> for an example
		/// </summary>
		public override string[] algoProps{ get {return new string[]{"learningRate", "momentumParam", "daFlatFactor"};} }

		/// <summary>
		/// Contains the String(s) with the description(s) for the specific properties of the Backpropagation algorithm
		/// </summary>
		public override string[] algoPropsDescription{ get {return new string[]{"Learning rate", "Momentum factor", "Flat Spot"};} }

		/// <summary>
		/// Defaultconstructor -> init the nn with null
		/// </summary>
		public Backpropagation() : base() {}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="nn">the MultilayerNet.NeuralNet which should be trained</param>
		public Backpropagation(NeuralNet nn) : base(nn) {}

		/// <summary>
		/// Constructor for 1 of 3 specific algorithm parameters
		/// </summary>
		/// <param name="nn">the MultilayerNet.NeuralNet which should be trained</param>
		/// <param name="learningRate">the learning rate for Backpropagation</param>
		public Backpropagation(NeuralNet nn, float learningRate) : this(nn)
		{
			this.learningRateProp = learningRate;
		}

		/// <summary>
		/// Constructor for 2 of 3 specific algorithm parameters
		/// </summary>
		/// <param name="nn">the MultilayerNet.NeuralNet which should be trained</param>
		/// <param name="learningRate">the learning rate for Backpropagation</param>
		/// <param name="momentumParam">the Momentum factor for the Momentum Backprop. method</param>
		public Backpropagation(NeuralNet nn, float learningRate, float momentumParam)
			: this(nn, learningRate)
		{
			this.momentumParamProp = momentumParam;
		}

		/// <summary>
		/// Constructor for 3 of 3 specific algorithm parameters
		/// </summary>
		/// <param name="nn">the MultilayerNet.NeuralNet which should be trained</param>
		/// <param name="learningRate">the learning rate for Backpropagation</param>
		/// <param name="momentumParam">the Momentum factor for the Momentum Backprop. method</param>
		/// <param name="flatSpotParam">factor for Backprop. Flat Spot Elimination method</param>
		public Backpropagation(NeuralNet nn, float learningRate, float momentumParam, float flatSpotParam)
			: this(nn, learningRate, momentumParam)
		{
			this.daFlatFactorProp = flatSpotParam;
		}


		/// <summary>
		/// Train one pattern
		/// </summary>
		/// <param name="teachOut">the requested output for the current input of the net (NeuralNet.inputProp)</param>
		/// <returns>the trained Network which learned one iteration</returns>
		public override NeuralNet learnOnePattern(float[] teachOut)
		{
			if(this.teachOutputProp == null)
			{
				this.teachOutputProp = new float[1][];
				this.teachOutputProp[0] = teachOut;
			}
			if(this.inputProp == null)
			{
				this.inputProp = new float[1][];
				this.inputProp[0] = this.nn.inputProp;
			}

			if(teachOut.Length != nn.outputProp.Length)
				throw new Exception("teachOutput vector must have the same length as the number of neurons in the output layer");

			float o = 0.0f, i = 0.0f, fj, deriv, dw = 0.0f;
			int nNeuronsPrevLayer = 0;
			float[] delta, fsum1 = null, fsum2;
			// calculate the error for the output neurons
			nn.propagate();
			delta = new float[nn.outputLayerProp.Length];
			for(int j=0; j<nn.outputLayerProp.Length; j++)
			{
				o = nn.outputLayerProp.neuronsProp[j].outputProp;
				deriv = nn.outputLayerProp.neuronsProp[j].actFuncProp.derivOutputProp + daFlatFactorProp;
				fj = teachOut[j] - o;
				delta[j] = deriv * fj;
				error += 0.5f * (fj * fj);
			}
			int la = 0;
			for(Layer l=nn.outputLayerProp; l != null; l = l.prevLayerProp, la++)
			{
				fsum2 = new float[l.neuronsProp[0].weightWithBiasProp.Length];
				if(dwOld[la] == null)
					dwOld[la] = new float[l.neuronsProp.Length][];
				if(weightChange[la] == null)
					weightChange[la] = new float[l.neuronsProp.Length][];
				for(int k=0; k<l.neuronsProp.Length; k++)
				{
					nNeuronsPrevLayer = l.neuronsProp[k].weightWithBiasProp.Length;
					if(dwOld[la][k] == null)
						dwOld[la][k] = new float[nNeuronsPrevLayer];
					if(weightChange[la][k] == null)
						weightChange[la][k] = new float[nNeuronsPrevLayer];
					deriv = l.neuronsProp[k].actFuncProp.derivOutputProp + daFlatFactorProp;

					// Falls innere Schicht, bestimme delta mit 1. Ableitung * summe der fehler * gewichten,
					// der vorangegangenen Schicht -> fsum2
					if(l != nn.outputLayerProp)
						delta[k] = deriv * fsum1[k];

					for(int m=0; m<nNeuronsPrevLayer; m++)
					{
						// Falls Neuron nicht 1. hiddenlayer, nimm für gewichtsanpassung den output der vorhergehenden Schicht
						if(l.prevLayerProp != null)
							i = l.neuronsProp[k].inputWithBiasProp[m];
							// Falls Neuron 1. hiddenlayer, nimm für gewichtsanpassung den input
						else
							for(int n=0; n<l.neuronsProp[k].inputWithBiasProp.Length; n++)
								i += l.neuronsProp[k].inputWithBiasProp[n] * l.neuronsProp[k].weightWithBiasProp[n];
						// Erst Summe bestimmen und dann neue Gewichte ändern
						////////////////////////////////////////////////////////
						// Fehler des k-ten Neurons mit den Gewichten zu Vorgängerneuronen summieren -> fsum für
						// nächste iteration speichern
						fsum2[m] += l.neuronsProp[k].weightWithBiasProp[m] * delta[k];
						// Neue gewichte für k-tes neuron berechnen
						dw = this.learningRateProp * i * delta[k];
						//		l.neuronsProp[k].weightWithBiasProp[m] += dw + momentumParam * dwOld[la][k][m];
						weightChange[la][k][m] += dw + momentumParam * dwOld[la][k][m];
						// In Vektor alte delta wij speichern für jedes Neuron speichern,
						// ab 2.ter Iteration bekommen diese Bedeutung
						dwOld[la][k][m] = dw;
					}
				}
				// delta für nächste Schicht neu anlegen
				delta = new float[nNeuronsPrevLayer];
				// für neues delta fsum1 anlegen, welches in der vorherigen Schicht bestimmt wurde (fsum2)
				fsum1 = new float[fsum2.Length];
				// zum speichern der Fehlersumme für nächste Schicht
				fsum1 = fsum2;
			}
			return nn;
		}

		/// <summary>
		/// Train the Network with patterns that where already init. before, using the online method
		/// </summary>
		/// <returns>the trained Network</returns>
		public override NeuralNet learnPatterns()
		{
			return learnPatterns(teachOutput, input);
		}

		/// <summary>
		/// Train the Network with the patterns (teachOutputProp, inputProp), using the online method
		/// </summary>
		/// <param name="teachOuts">the requested output</param>
		/// <param name="inputs">the input patterns for the net</param>
		/// <returns>the trained Network</returns>
		public override NeuralNet learnPatterns(float[][] teachOuts, float[][] inputs)
		{
			if(teachOuts.Length != inputs.Length)
				throw new Exception("number of input patterns != number of teach patterns");
	//		teachOutputProp = new float[teachOuts.Length][];
	//		inputProp = new float[inputs.Length][];
			teachOutputProp = teachOuts;
			inputProp = inputs;
			iter = 0;
			dwOld = new float[nn.layersProp.Length][][];

			for(error = 0.0f; iter < maxIter; iter++)
			{
				error = 0.0f;
				// Mix the pattern array
				mixPatternSet();
				for(int i=0; i<teachOuts.Length/*0.65*/;i++)
				{
					if(inputProp[i].Length != nn.inputProp.Length)
						throw new Exception("inputs[i] must have the same length as the input vector of the net");
					weightChange = new float[nn.layersProp.Length][][];
					this.nn.inputProp = inputProp[i];
					this.learnOnePattern(teachOutputProp[i]);

					ArrayList t = new ArrayList(weightChange);
					t.Reverse();
					for(int index=0; index<weightChange.Length; index++)
					{
						weightChange[index] = (float[][])t[index];
						for(int j=0; j<weightChange[index].Length; j++)
							for(int k=0; k<weightChange[index][j].Length; k++)
								weightChange[index][j][k] += nn.weightNetProp[index][j][k];
					}
					nn.weightNetProp = weightChange;

					if(this.onOutCalculated != null)
						this.onOutCalculated(nn, i);
				}
				if(this.onErrorCalculated != null)
					this.onErrorCalculated(error);
				// Wenn Fehler nicht mehr siknifikant abnimmt -> Abbruch
				if(error <= minTolerableError)
				{
					Console.WriteLine("error <= {0}", minTolerableError);
					return nn;
				}
			}
			return nn;
		}

		/// <summary>
		/// Mix the set of patterns randomly for learning
		/// </summary>
		protected void mixPatternSet()
		{
			if(teachOutputProp == null || inputProp == null)
				throw new NullReferenceException("No patterns init.! inputProp or teachOutputProp == null!");
			ArrayList teachAL = new ArrayList(teachOutputProp);
			ArrayList inpAL = new ArrayList(inputProp);
			teachOutputProp = new float[teachOutputProp.Length][];
			inputProp = new float[inputProp.Length][];
			Random rand;
			int index;
			for(int i=0; i<teachOutputProp.Length; i++)
			{
				rand = new Random(unchecked((int)DateTime.Now.Ticks) * (i+1));
				index = rand.Next(teachAL.Count);

				teachOutputProp[i] = (float[])teachAL[index];
				inputProp[i] = (float[])inpAL[index];
				teachAL.RemoveAt(index);
				inpAL.RemoveAt(index);
			}
		}

		/// <summary>
		/// Train the Network with patterns that where already init. before, using the batch/offline method
		/// </summary>
		/// <returns>the trained Network</returns>
		public NeuralNet learnPatternsBatch()
		{
			return learnPatternsBatch(teachOutput, input);
		}

		/// <summary>
		/// Train the Network with the patterns (teachOutputProp, inputProp), using the batch/offline method
		/// </summary>
		/// <param name="teachOuts">the requested output</param>
		/// <param name="inputs">the input patterns for the net</param>
		/// <returns>the trained Network</returns>
		public NeuralNet learnPatternsBatch(float[][] teachOuts, float[][] inputs)
		{
			if(teachOuts.Length != inputs.Length)
				throw new Exception("number of input patterns != number of teach patterns");
			teachOutputProp = new float[teachOuts.Length][];
			inputProp = new float[inputs.Length][];
			teachOutputProp = teachOuts;
			inputProp = inputs;
			iter = 0;
			dwOld = new float[nn.layersProp.Length][][];

			for(error = 0.0f; iter < maxIter; iter++)
			{
				error = 0.0f;
				// Mix the pattern array
				//	mixPatternSet();
				weightChange = new float[nn.layersProp.Length][][];
				for(int i=0; i<teachOuts.Length;i++)
				{
					if(inputProp[i].Length != nn.inputProp.Length)
						throw new Exception("inputs[i] must have the same length as the input vector of the net");
					this.nn.inputProp = inputProp[i];
					this.learnOnePattern(teachOutputProp[i]);
					if(this.onOutCalculated != null)
						this.onOutCalculated(nn, i);
				}

				ArrayList t = new ArrayList(weightChange);
				t.Reverse();
				for(int index=0; index<weightChange.Length; index++)
				{
					weightChange[index] = (float[][])t[index];
					for(int j=0; j<weightChange[index].Length; j++)
						for(int k=0; k<weightChange[index][j].Length; k++)
							weightChange[index][j][k] += nn.weightNetProp[index][j][k];
				}
				nn.weightNetProp = weightChange;

				if(this.onErrorCalculated != null)
					this.onErrorCalculated(error);
				// Wenn Fehler nicht mehr siknifikant abnimmt -> Abbruch
				if(error <= minTolerableError)
				{
					Console.WriteLine("error <= {0}", minTolerableError);
					return nn;
				}
			}
			return nn;
		}
	}
}