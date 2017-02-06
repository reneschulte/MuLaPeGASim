using System;

namespace MultilayerNet
{
	/// <summary>
	/// Description: The artificial neuron
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-30-2004</para>
	/// </summary>
	public class Neuron
	{
/*		/// <summary>
		/// counts up each time the getRandomizedVector()-method is called, 
		/// needed for randomization
		/// </summary>	
		protected static int randSeed;
*/
		/// <summary>
		/// Returns the output of the neuron, which depends on the current activation function
		/// </summary>	
		public float outputProp { get{return actFuncProp.outputProp;} }

		/// <summary>
		/// Returns the input without the bias on-state,
		/// sets the input without overwriting the bias on-state
		/// </summary>	
		public float[] inputProp 
		{ 
			get
			{
				float[] t = new float[input.Length-1];
				for(int i=0; i<t.Length; i++)
					t[i] = input[i];
				return t;
			} 
			set
			{
				if(weight != null)
					if(value.Length != weightProp.Length)
						throw new Exception("Weight vector must have the same length as the input vector!");
				input = new float[value.Length+1];
				int i;
				for(i=0; i<value.Length; i++)
					input[i] = value[i];
				input[i] = 1.0f;		// Bias neuron is always active
			} 
		}
		/// <summary>
		/// The input of the neuron, with the bias on-state (last value in the vector)
		/// </summary>	
		protected float[] input;
		

		/// <summary>
		/// Returns the input with the bias and 
		/// sets the input including the bias on-state (last value in the vector)
		/// </summary>	
		public float[] inputWithBiasProp 
		{ 
			get{ return input; } 
			set
			{
				if(weight != null)
					if(value.Length != weightWithBiasProp.Length)
						throw new Exception("Weight vector must have the same length as the input vector!");
				input = value;
			} 
		}

		/// <summary>
		/// Returns the weight without the bias and 
		/// sets the weight without overwriting the bias
		/// </summary>	
		public float[] weightProp		
		{ 
			get
			{
				float[] t = new float[weight.Length-1];
				for(int i=0; i<t.Length; i++)
					t[i] = weight[i];
				return t;
			} 
			set
			{
				if(input != null)
					if(value.Length != inputProp.Length)
						throw new Exception("Weight vector must have the same length as the input vector!");
				weight = new float[value.Length+1];
				int i;
				for(i=0; i<value.Length; i++)
					weight[i] = value[i];
			} 
		}
		/// <summary>
		/// The weight of the neuron including the bias (last value in the vector)
		/// </summary>
		protected float[] weight;		

		/// <summary>
		/// Returns the weight with the bias and 
		/// sets the weight including the bias (last value in the vector)
		/// </summary>	
		public float[] weightWithBiasProp		
		{ 
			get{ return weight;	} 
			set
			{
				if(input != null)
					if(value.Length != inputWithBiasProp.Length)
						throw new ArgumentException("Weight vector must have the same length as the input vector!");
				weight = value;
			} 
		}

		/// <summary>
		/// The threshold of the neuron. The negative of the last value in the vector 
		/// -> -weightWithBiasProp[weightWithBiasProp.Length-1]
		/// </summary>
		public float thresholdProp 
		{
			get
			{
				if (weight != null)
					return -weightWithBiasProp[weightWithBiasProp.Length-1];
				else
					return 0.0f;
			} 
			set
			{
				if (weight != null)
					weightWithBiasProp[weightWithBiasProp.Length-1] = -value;			
			} 
		}

		/// <summary>
		/// Returns the net output of the neuron. 
		/// </summary>	
		public float netProp
		{ 
			get
			{
		/*		if(weight == null)
				{
					randSeed++;
					weightWithBiasProp = getRandomizedVector(inputWithBiasProp.Length);
				}
		*/		float net = 0.0f;
				for(int i=0; i<weight.Length; i++)
					net += weight[i] * input[i];
				return net;
			}
		}

		/// <summary>
		/// The activation function of the neuron
		/// </summary>	
		public IActivationFunction actFuncProp { get{return actFunc;} set{actFunc = (value!=null)?value:actFunc;} }
		/// <summary>
		/// The activation function of the neuron
		/// </summary>
		protected IActivationFunction actFunc;
		
		/// <summary>
		/// The Defaultconstructor, actFunc = LogisticActFunc. 
		/// </summary>
		public Neuron()
		{
			this.actFuncProp = new LogisticActFunc(this);
		}	

		/// <summary>
		/// The constructor for activationFunc as parameter;
		/// </summary>
		/// <param name="actF">the activation function for this neuron</param>
		public Neuron(IActivationFunction actF)
		{
			this.actFuncProp = actF;
		}


/*		/// <summary>
		/// returns a randomized vector with the length = param. vectorLength;
		/// the values are between 0.0f and 1.0f
		/// </summary>
		public float[] getRandomizedVector(int vectorLength)
		{
			float[] temp = new float[vectorLength];
			Random rand;
			int i;
			for(i=0; i<temp.Length-1; i++)
			{
				rand = new Random(unchecked((int)DateTime.Now.Ticks) * i * Neuron.randSeed);
				temp[i] = 0.5f;//((float)rand.NextDouble());
			}
			temp[i] = -0.5f;//-((float)rand.NextDouble());
			return temp;
		}
*/	}
}
