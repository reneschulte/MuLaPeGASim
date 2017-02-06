using System;
using System.Collections;

namespace MultilayerNet
{
	/// <summary>
	/// The delegate for the event when the error is calculated
	/// </summary>
	public delegate void errorHandler(float error);

	/// <summary>
	/// The delegate for handling the output
	/// </summary>
	public delegate void outputHandler(NeuralNet nn, int currentPattern);

	/// <summary>
	/// Description: The abstract class for various child class Multilayer Perceptron learning algorithms.
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public abstract class LearningAlgo
	{
		/// <summary>
		/// The event which is thrown, after the summed global error was calculated
		/// </summary>
		public abstract event errorHandler onErrorCalculated;

		/// <summary>
		/// The event which is thrown, after the output was calculated
		/// </summary>
		public abstract event outputHandler onOutCalculated;

		/// <summary>
		/// The NeuralNet reference which should be trained
		/// </summary>
		public NeuralNet nnProp { get{return nn;} set{if(value!=null) nn=value;} }
		/// <summary>
		/// The NeuralNet reference which should be trained
		/// </summary>
		protected NeuralNet nn;

		/// <summary>
		/// Returns the current iteration of the algorithm
		/// </summary>
		public int iterationProp{ get {return iter;} }
		/// <summary>
		/// The current iteration of the algorithm
		/// </summary>
		protected int iter;

		/// <summary>
		/// Returns the current global summed error of the Network
		/// </summary>
		public float errorProp{ get {return error;} }
		/// <summary>
		/// The current global summed error of the Network
		/// </summary>
		protected float error;

		/// <summary>
		/// The smallest tolerable global summed error. The learning stops if(minTolerableError >= error) -> default 0.000
		/// </summary>
		public float minTolerableErrorProp
		{
			get {return minTolerableError;}
			set{if(value>0) minTolerableError = value;}
		}
		/// <summary>
		/// The smallest tolerable global summed error. The learning stops if(minTolerableError >= error) -> default 0.000
		/// </summary>
		protected float minTolerableError;

		/// <summary>
		/// The maximum iterations which will be done by the algorithm. Values must be greater than 0 -> default 1000
		/// </summary>
		public int maxIterationProp { get{return maxIter;} set{if(value>0) maxIter=value;} }
		/// <summary>
		/// The maximum iterations which will be done by the algorithm. Values must be greater than 0 -> default 1000
		/// </summary>
		protected int maxIter;

		/// <summary>
		/// The Requested output patterns (teach output) for the input
		/// <seealso cref="MultilayerNet.NeuralNet"/>
		/// </summary>
		public float[][] teachOutputProp { get {return teachOutput;} set{ teachOutput = value;} }
		/// <summary>
		/// The Requested output patterns (teach output) for the input
		/// </summary>
		protected float[][] teachOutput;

		/// <summary>
		/// The input patterns corresponding to the teachOutput
		/// <seealso cref="MultilayerNet.NeuralNet"/>
		/// </summary>
		public float[][] inputProp { get {return input;} set{input = value;} }
		/// <summary>
		/// The input patterns corresponding to the teachOutput
		/// </summary>
		protected float[][] input;

		/// <summary>
		/// Contains the String(s) with the specific properties for the learning algorithm
		/// </summary>
		/// <example>
		/// The Property must have the Name 'parameterProp' and the String in activationProps 'parameter',
		/// in order to use the FileManagers read methods properly.
		/// <para>Example for the Backpropagation learning algorithm:
		/// <code>
		/// public float learningRateProp { get{...} set{...} }
		/// public float momentumParamProp { get{...} set{...} }
		/// public float daFlatFactorProp { get{...} set{...} }
		/// public override string[] algoProps{ get {return new string[]{"learningRate", "momentumParam", "daFlatFactor"};} }
		/// </code>
		/// </para>
		/// </example>
		public abstract string[] algoProps{ get; }

		/// <summary>
		/// Contains the String(s) with the description(s) for the specific properties for the extended algorithms
		/// </summary>
		public abstract string[] algoPropsDescription{ get; }

		/// <summary>
		/// Defaultconstructor -> init the Network with null
		/// </summary>
		public LearningAlgo() : this(null) {}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="nn">the MultilayerNet.NeuralNet which should be trained</param>
		public LearningAlgo(NeuralNet nn)
		{
			this.nn = nn;
			maxIterationProp = 1000;
			minTolerableError = 0.0f;
		}

		/// <summary>
		/// Train one pattern
		/// </summary>
		/// <param name="teachOutput">the requested output for the current input of the net (NeuralNet.inputProp)</param>
		/// <returns>the trained Network which learned one iteration</returns>
		public abstract NeuralNet learnOnePattern(float[] teachOutput);

		/// <summary>
		/// Train the Network with the patterns (teachOutputProp, inputProp)
		/// <seealso cref="MultilayerNet.NeuralNet"/>
		/// </summary>
		/// <param name="teachOutputs">the requested output</param>
		/// <param name="inputs">the input patterns for the net</param>
		/// <returns>the trained Network</returns>
		public abstract NeuralNet learnPatterns(float[][] teachOutputs, float[][] inputs);

		/// <summary>
		/// Train the Network with patterns that where already init. before
		/// <seealso cref="MultilayerNet.NeuralNet"/>
		/// </summary>
		/// <returns>the trained Network</returns>
		public abstract NeuralNet learnPatterns();
	}
}