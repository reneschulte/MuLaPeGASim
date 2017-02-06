using System;

namespace MultilayerNet
{	
	/// <summary>
	/// Description: The Multilayer Perceptron Neural Network
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-30-2004</para>
	/// </summary>
	public class NeuralNet
	{
		/// <summary>
		/// The name of the Network. Default -> 'no net name yet ;)'
		/// </summary>
		public string netNameProp
		{
			get{return netName;} 
			set{netName = (value!=null) ? value : netName; } 
		}
		/// <summary>
		/// The name of the Network. Default -> 'no net name yet ;)'
		/// </summary>
		protected string netName = "no net name yet ;)";

		/// <summary>
		/// The learning algorithm which is used to train the Network
		/// </summary>
		public LearningAlgo learnAlgoProp 
		{
			get{return learnAlgo;} 
			set
			{
				learnAlgo = value;
				learnAlgo.nnProp = this;
			}
		}
		/// <summary>
		/// The learning algorithm which is used to train the Network
		/// </summary>
		protected LearningAlgo learnAlgo;
		
		/// <summary>
		/// The layers in the Network
		/// </summary>
		public Layer[] layersProp {get{return layers;} set{layers = value;}}
		/// <summary>
		/// The layers in the Network
		/// </summary>
		protected Layer[] layers;
		
		/// <summary>
		/// Returns the outputlayer of the Network
		/// </summary>
		public Layer outputLayerProp {get{return layersProp[layersProp.Length-1];}}

		/// <summary>
		/// The weight 3D-matrix of the neurons in the whole Network.
		/// <para>weightNetProp[layer][index neuron in layer, from left to right][weight presynapt. neuron]; 
		/// layer index=0  == input layer</para>
		/// </summary>		
		public float[][][] weightNetProp
		{
			get
			{
				float[][][] result = new float[layersProp.Length][][];
				for(int i=0; i<layersProp.Length; i++)
					result[i] = layersProp[i].weightLayerProp;
				return result;
			}
			set
			{
				if(value != null)
					for(int i=0; i<layersProp.Length; i++)
						layersProp[i].weightLayerProp = value[i];
			}
		}

		/// <summary>
		/// The threshold matrix of the neurons in the whole network!
		/// <para>thresholdNetProp[layer][index neuron in layer, from left to right]</para>
		/// </summary>		
		public float[][] thresholdNetProp
		{
			get
			{
				float[][] result = new float[layersProp.Length][];
				for(int i=0; i<layersProp.Length; i++)
					result[i] = layersProp[i].thresholdLayerProp;
				return result;
			}
			set
			{
				if(value != null)
					for(int i=0; i<layersProp.Length; i++)
						layersProp[i].thresholdLayerProp = value[i];
			}
		}

		/// <summary>
		/// The activation function of each neuron
		/// <para>actFuncForEachNeuronProp[layer][index neuron in layer, from left to right]</para>
		/// </summary>		
		public IActivationFunction[][] actFuncForEachNeuronProp
		{
			get
			{
				IActivationFunction[][] result = new IActivationFunction[layersProp.Length][];
				for(int i=0; i<layersProp.Length; i++)
				{
					result[i] = new IActivationFunction[layersProp[i].neuronsProp.Length];
					for(int j=0; j<layersProp[i].neuronsProp.Length; j++)
						result[i][j] = layersProp[i].neuronsProp[j].actFuncProp;
				}
				return result;
			}
			set
			{
				if(value != null)
					for(int i=0; i<layersProp.Length; i++)
						for(int j=0; j<layersProp[i].neuronsProp.Length; j++)
						{
							layersProp[i].neuronsProp[j].actFuncProp = value[i][j];
							layersProp[i].neuronsProp[j].actFuncProp.neuronProp = layersProp[i].neuronsProp[j];
						}
			}
		}

/*		/// <summary>
		/// the steepness of each neurons with the logistic activation function
		/// steepnessForEachNeuronsProp[layer][index neuron in layer, from left to right]
		/// </summary>		
		public float[][] steepnessForEachNeuronsProp
		{
			get
			{
				float[][] result = new float[layersProp.Length][];
				for(int i=0; i<layersProp.Length; i++)
				{
					result[i] = new float[layersProp[i].neuronsProp.Length];
					for(int j=0; j<layersProp[i].neuronsProp.Length; j++)
						result[i][j] = (layersProp[i].neuronsProp[j].actFuncProp as LogisticActFunc).steepnessProp;
				}
				return result;
			}
			set
			{
				if(value != null)
					for(int i=0; i<layersProp.Length; i++)
						for(int j=0; j<layersProp[i].neuronsProp.Length; j++)
							(layersProp[i].neuronsProp[j].actFuncProp as LogisticActFunc).steepnessProp = value[i][j];
			}
		}
*/
		/// <summary>
		/// Just calls the propagate() method and returns the output vector of the net
		/// </summary>
		public float[] outputProp{ get{return propagate();} }

		/// <summary>
		/// Sets/gets the input vector of the net == input vector of all neurons in the first layer
		/// </summary>
		public float[] inputProp { get{return layersProp[0].inputProp;} set{layersProp[0].inputProp = value;} }

		
		/// <summary>
		/// Constructor, init. the weights randomly -> with an example !!!!
		/// <seealso cref="MultilayerNet.FileManager"/>
		/// <seealso cref="MultilayerNet.LearningAlgo"/>
		/// <seealso cref="MultilayerNet.GeneticLearningAlgorithm"/>
		/// </summary>
		/// <example>
		/// A simple example for a 2-3-3-2 network with input and teach output patterns, 
		/// and the usage of the FileManager and trainig algorithm.
		/// <code>
		/// float[][] input = new float[][]{	new float[] {0.05f, 0.02f},
		///										new float[] {0.09f, 0.11f},
		///										new float[] {0.12f, 0.20f},
		///										new float[] {0.15f, 0.22f},
		///										new float[] {0.20f, 0.25f},
		///										new float[] {0.75f, 0.75f},
		///										new float[] {0.80f, 0.83f},
		///										new float[] {0.82f, 0.80f},
		///										new float[] {0.90f, 0.89f},
		///										new float[] {0.95f, 0.89f}};
		///
		///	float[][] teach = new float[][]{	new float[] {1.0f, 0.0f},
		///										new float[] {1.0f, 0.0f},
		///										new float[] {1.0f, 0.0f},
		///										new float[] {1.0f, 0.0f},
		///										new float[] {1.0f, 0.0f},
		///										new float[] {0.0f, 1.0f},
		///										new float[] {0.0f, 1.0f},
		///										new float[] {0.0f, 1.0f},
		///										new float[] {0.0f, 1.0f},
		///										new float[] {0.0f, 1.0f}};
		///
		///	// Create the 2-3-3-2 network with the genetic learning algorithm:
		///	NeuralNet nn = new NeuralNet(input[0], new int[]{3, 3}, teach[0].Length, new GeneticLearningAlgorithm());
		///	(nn.learnAlgoProp as GeneticLearningAlgorithm).nIndividualsProp = 200;
		///	// Mutation-Rate in percent !!
		///	(nn.learnAlgoProp as GeneticLearningAlgorithm).mutationRateProp = 5f;
		///	// Append an eventhandler:
		///	nn.learnAlgoProp.onErrorCalculated += new MultilayerNet.errorHandler(writeGSEToFile);
		///
		///	nn.learnAlgoProp.inputProp = input;
		///	nn.learnAlgoProp.teachOutputProp = teach;
		///
		///	FileManager.getInstance().writeNetworkToXml(&quot;2-3-3-2_before_learning.net&quot;, nn, &quot;SuperNet untrained&quot;);
		///	Console.WriteLine(&quot;-------- LEARNING --------\n&quot;);
		///	nn.learnAlgoProp.learnPatterns();
		///	Console.WriteLine(&quot;--- LEARNING FINISHED AFTER {0} Generations ----&quot;, nn.learnAlgoProp.iterationProp);
		///	FileManager.getInstance().writeNetworkToXml(&quot;2-3-3-2_after_learning.net&quot;, nn, &quot;SuperNet trained&quot;);
		///	
		///	// The eventhandler method wich is used above:
		///	
		/// public void writeGSEToFile(float error)
		///	{
		///		FileManager.getInstance().writeFloatToTextFile(&quot;error.txt&quot;, error);
		///	}
		///	
		/// </code>
		/// </example>
		/// <param name="input">the input of the Network</param>
		/// <param name="hiddenLayers">describes how many hidden layers are created and how many neurons are in each layer</param>
		/// <param name="nOutputs">number of output neurons</param>
		/// <param name="la">the learning algorithm for the net</param>
		public NeuralNet(float[] input, int[] hiddenLayers, int nOutputs, LearningAlgo la)
		{
			layersProp = new Layer[hiddenLayers.Length+1];	
			layersProp[layersProp.Length-1] = new Layer(nOutputs);
	//		layersProp[layersProp.Length-1].nextLayerProp = null;			
			for(int i=0; i<layersProp.Length-1; i++)
				layersProp[i] = new Layer(hiddenLayers[i]);
			for(int i=0; i<layersProp.Length; i++)
			{
	//			if(i<layersProp.Length-1)
	//				layersProp[i].nextLayerProp = layersProp[i+1];
				if(i>0)
					layersProp[i].prevLayerProp = layersProp[i-1];
			}
			layersProp[0].prevLayerProp = null;
		
			this.inputProp = input; 

			if(la != null)
			{
				this.learnAlgoProp = la;
				this.learnAlgoProp.nnProp = this;
			}
			randomizeWeights();
			propagate();
		}

		/// <summary>
		/// Constructor for the default Backpropagation learn algo., init. the weights randomly 
		/// </summary>
		/// <param name="input">the input of the Network</param>
		/// <param name="hiddenLayers">describes how many hidden layers are created and how many neurons are in each layer</param>
		/// <param name="nOutputs">number of output neurons</param>
		public NeuralNet(float[] input, int[] hiddenLayers, int nOutputs)
				: this(input, hiddenLayers, nOutputs, null)
		{
			this.learnAlgoProp = new Backpropagation(this);
		}

		/// <summary>
		/// The Defaultconstructor, creates a simple Singlelayer Perceptron. Init input with 1, 1, 1. 
		/// No hidden layer and 3 outputs, learnAlgo = Backpropagation, init. the weights randomly 
		/// </summary>
		public NeuralNet() : this(new float[]{1.0f, 1.0f, 1.0f}, new int[]{}, 3) {}


		/// <summary>
		/// Computes the output of the network and returns the output vector
		/// </summary>
		public float[] propagate()
		{
			for(int i=1; i<layersProp.Length; i++)
			layersProp[i].inputProp = layersProp[i].prevLayerProp.outputProp;
	/*	//	Falls summe der outputs < 1.0 => randomize
	 * 		float t = 0.0f;
			for(int i=0; i<outputLayerProp.outputProp.Length; i++)
				t += outputLayerProp.outputProp[i];
			if(t <= 0.99f)
			{
				Console.WriteLine("------------------------> $~RESET~$ <------------------------");
				this.randomizeWeights();
			}
				Console.WriteLine("t= " + t);
	*/
			return outputLayerProp.outputProp;
		}

		/// <summary>
		/// Randomize the weights and the threshold of the network
		/// </summary>
		/// <param name="min">the minimum for the random init.</param>
		/// <param name="max">the maximum for the random init.</param>
		/// <param name="seed">the init. Seed for the random generator</param>
		public void randomizeWeights(float min, float max, int seed)
		{
			float mod = max - min;
			Random rand = new Random(seed);
			int weightVectorLength;
			for(int i=0; i<layersProp.Length; i++)
				for(int j=0; j<layersProp[i].neuronsProp.Length; j++)
				{
					if(i > 0)
						weightVectorLength = layersProp[i-1].neuronsProp.Length + 1;				
					else
						weightVectorLength = layersProp[0].neuronsProp[j].inputWithBiasProp.Length;
					layersProp[i].neuronsProp[j].weightWithBiasProp = new float[weightVectorLength];
					int k;
					for(k=0; k<weightVectorLength - 1; k++)
					{
						// changed, in den neuronen waren einige gewichte gleich!
//						rand = new Random(seed * (i+1) * (j+1) * (k+1));
						layersProp[i].neuronsProp[j].weightWithBiasProp[k] = ((float)(rand.NextDouble()%mod+min));
					}
					layersProp[i].neuronsProp[j].weightWithBiasProp[k] = -((float)(rand.NextDouble()%mod+min));
				}
		}

		/// <summary>
		/// Randomize the weights and the threshold of the network using the Systemtime as Seed
		/// </summary>
		/// <param name="min">the minimum for the random init.</param>
		/// <param name="max">the maximum for the random init.</param>
		public void randomizeWeights(float min, float max)
		{
			randomizeWeights(min, max, unchecked((int)DateTime.Now.Ticks));
		}

		/// <summary>
		/// Randomize the weights and the threshold in an optimal range 
		/// for the activation function of the first neuron in the first layer
		/// </summary>
		/// <param name="seed">the init. Seed for the random generator</param>
		public void randomizeWeights(int seed)
		{
			if(this.actFuncForEachNeuronProp == null)
				throw new Exception("Activation function vector for net is not init.!");
			if(this.actFuncForEachNeuronProp[0][0] == null)
				throw new Exception("Neuron 0-0 has activation function yet!");
			float range = this.actFuncForEachNeuronProp[0][0].maxProp - this.actFuncForEachNeuronProp[0][0].minProp;
			float randMin = this.actFuncForEachNeuronProp[0][0].minProp + range * 0.25f;
			float randMax = this.actFuncForEachNeuronProp[0][0].maxProp - range * 0.25f;
			randomizeWeights(randMin, randMax, seed);
		}

		/// <summary>
		/// Randomize the weights and the threshold in an optimal range using the Systemtime as Seed 
		/// for the activation function of the first neuron in the first layer
		/// </summary>
		public void randomizeWeights()
		{
			if(this.actFuncForEachNeuronProp == null)
				throw new Exception("Activation function vector for net is not init.!");
			if(this.actFuncForEachNeuronProp[0][0] == null)
				throw new Exception("Neuron 0-0 has activation function yet!");
			float range = this.actFuncForEachNeuronProp[0][0].maxProp - this.actFuncForEachNeuronProp[0][0].minProp;
			float randMin = this.actFuncForEachNeuronProp[0][0].minProp + range * 0.25f;
			float randMax = this.actFuncForEachNeuronProp[0][0].maxProp - range * 0.25f;
			randomizeWeights(randMin, randMax);
		}
	}
}
