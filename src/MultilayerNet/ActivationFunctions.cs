using System;
using System.Collections;

namespace MultilayerNet
{
	/// <summary>
	/// Description: Interface for an activation function
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public interface IActivationFunction
	{
		/// <summary>
		/// Returns the human readable name of the activation function
		/// </summary>
		String Name{ get; }

		/// <summary>
		/// Returns the output of the activation function; calculated just in time
		/// </summary>
		float outputProp{get;}

		/// <summary>
		/// Returns the first derivation output of the activation function; calculated just in time
		/// </summary>
		float derivOutputProp{get;}

		/// <summary>
		/// The reference to the neuron which belongs to this activation function
		/// </summary>
		Neuron neuronProp{get; set;}

		/// <summary>
		/// Returns the minimum value for the activation function
		/// </summary>
		float minProp{ get; }

		/// <summary>
		/// Returns the maximum value for the activation function
		/// </summary>
		float maxProp{ get; }

		/// <summary>
		/// Contains the String(s) with the specific properties for the activation function
		/// </summary>
		/// <example>
		/// The Property must have the Name 'parameterProp' and the String in activationProps 'parameter',
		/// in order to use the FileManagers read methods properly.
		/// <para>Example for the Sigmoid activation function:
		/// <code>
		/// public float steepnessProp{ get{...} set{...} }
		/// public string[] activationProps{ get{return new string[]{"steepness"};} }
		/// </code>
		/// </para>
		/// </example>
		string[] activationProps{get;}
	}

	/// <summary>
	/// Description: Abstract class for extending Sigmoid activation functions.
	/// The parameter steepness stands for the steepness of the function.
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public abstract class SigmoidActFunc : IActivationFunction
	{
		/// <summary>
		/// Returns the name of the Sigmoid Activation Function
		/// </summary>
		public abstract String Name{ get; }

		/// <summary>
		/// The steepness of the Sigmoid function; default 1.0f
		/// </summary>
		public float steepnessProp{ get{return steepness;} set{steepness = (value>0)?value:steepness;} }
		/// <summary>
		/// The steepness of the Sigmoid function; default 1.0f
		/// </summary>
		protected float steepness = 1.0f;

		/// <summary>
		/// Returns the minimum value which could be used to set the minimum for adjusting the steepness
		/// </summary>
		public static float steepnessMinProp{ get{return 0.0f;} }

		/// <summary>
		/// Returns the maximum value which could be used to set the maximum for adjusting the steepness
		/// </summary>
		public static float steepnessMaxProp{ get{return 20.0f;} }

		/// <summary>
		/// Returns the step range which could be used to set the step for adjusting the steepness values
		/// </summary>
		public static float steepnessStepProp{ get{return 0.1f;} }

		/// <summary>
		/// The reference to the neuron which belongs to this Sigmoid activation function
		/// </summary>
		public Neuron neuronProp{ get{return neuron;} set{neuron = (value!=null)?value:neuron;} }
		/// <summary>
		/// The reference to the neuron which belongs to this Sigmoid activation function
		/// </summary>
		protected Neuron neuron;

		/// <summary>
		/// Returns the output of the Sigmoid activation function; calculated just in time
		/// </summary>
		public abstract float outputProp{ get; }

		/// <summary>
		/// Returns the first derivation output of the Sigmoid activation function; calculated just in time
		/// </summary>
		public abstract float derivOutputProp{ get; }

		/// <summary>
		/// Returns the minimum value for the Sigmoid activation function
		/// </summary>
		public abstract float minProp{ get; }

		/// <summary>
		/// Returns the maximum value for the Sigmoid activation function
		/// </summary>
		public abstract float maxProp{ get; }

		/// <summary>
		/// Contains the String(s) with the specific properties for the Sigmoid activation function.
		/// <see cref="MultilayerNet.IActivationFunction.activationProps"/> for an example
		/// </summary>
		public string[] activationProps{ get{return new string[]{"steepness"};} }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="n">the associated neuron</param>
		/// <param name="steepness">the steepness of the Sigmoid function</param>
		public SigmoidActFunc(Neuron n, float steepness)
		{
			this.neuronProp= n;
			this.steepnessProp = steepness;
		}

		/// <summary>
		/// Constructor, steepness default = 1.0f
		/// </summary>
		/// <param name="n">the associated neuron</param>
		public SigmoidActFunc(Neuron n)
		{
			this.neuronProp= n;
		}

		/// <summary>
		/// The Defaultconstructor, inits the belonging neuron with null
		/// </summary>
		public SigmoidActFunc() : this(null) {}
	}


	/// <summary>
	/// Description: The Logistic activation function. (min=0; max=1)
	/// The parameter steepness stands for the steepness of the function.
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class LogisticActFunc : SigmoidActFunc
	{
		/// <summary>
		/// Returns the name of the Logistic activation function
		/// </summary>
		public override String Name
		{
			get { return "Logistic activation function"; }
		}

		/// <summary>
		/// Returns the output of the Logistic activation function, returns: 1/(1 + Exp(-steepness * net))
		/// </summary>
		public override float outputProp{ get{return (float)(1/(1 + Math.Exp(-steepnessProp * neuronProp.netProp)));} }

		/// <summary>
		/// Returns the first derivation output of the Logistic activation function, returns: (steepness * outputProp * (1 - outputProp))
		/// </summary>
		public override float derivOutputProp{ get{return (steepness * (outputProp * (1 - outputProp)));} }

		/// <summary>
		/// Returns the minimum value for the Logistic activation function
		/// </summary>
		public override float minProp{ get{ return 0.0f;} }

		/// <summary>
		/// Returns the maximum value for the Logistic activation function
		/// </summary>
		public override float maxProp{ get{ return 1.0f;} }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="n">the associated neuron</param>
		/// <param name="steepness">the steepness of the Sigmoid function</param>
		public LogisticActFunc(Neuron n, float steepness) : base(n, steepness) {}

		/// <summary>
		/// Constructor, steepness default = 1.0f
		/// </summary>
		/// <param name="n">the associated neuron</param>
		public LogisticActFunc(Neuron n) : base(n) {}

		/// <summary>
		/// The Defaultconstructor, inits the belonging neuron with null
		/// </summary>
		public LogisticActFunc() : base(null) {}
	}

	/// <summary>
	/// Description: The Tangens Hyperbolicus activation function. (min=-1; max=1)
	/// The parameter steepness stands for the steepness of the function.
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class TanhActFunc : SigmoidActFunc
	{
		/// <summary>
		/// Returns the name of the Tangens Hyperbolicus activation function
		/// </summary>
		public override String Name
		{
			get { return "Tangens Hyperbolicus activation function"; }
		}

		/// <summary>
		/// Returns the output of the Tangens Hyperbolicus activation function, returns: tanh(steepness * net)
		/// </summary>
		public override float outputProp{ get{return (float)(Math.Tanh(steepnessProp * neuronProp.netProp));} }

		/// <summary>
		/// Returns the first derivation output of the Tangens Hyperbolicus activation function, returns: (1 - output^2)*steepness
		/// </summary>
		public override float derivOutputProp{ get {return ((1 - (outputProp*outputProp))*steepnessProp);} }

		/// <summary>
		/// Returns the minimum value for the Tangens Hyperbolicus activation function
		/// </summary>
		public override float minProp{ get{ return -1.0f;} }

		/// <summary>
		/// Returns the maximum value for the Tangens Hyperbolicus activation function
		/// </summary>
		public override float maxProp{ get{ return 1.0f;} }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="n">the associated neuron</param>
		/// <param name="steepness">the steepness of the Sigmoid function</param>
		public TanhActFunc(Neuron n, float steepness) : base(n, steepness) {}

		/// <summary>
		/// Constructor, steepness default = 1.0f
		/// </summary>
		/// <param name="n">the associated neuron</param>
		public TanhActFunc(Neuron n) : base(n) {}

		/// <summary>
		/// The Defaultconstructor, inits the belonging neuron with null
		/// </summary>
		public TanhActFunc() : base(null) {}
	}

	/// <summary>
	/// Description: The Linear Step activation function. Also called Binary Threshold function
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class StepActFunc : IActivationFunction
	{
		/// <summary>
		/// Returns the name of the Linear Step activation function
		/// </summary>
		public String Name
		{
			get { return "Step activation function"; }
		}

		/// <summary>
		/// The reference to the neuron which belongs to this Linear Step activation function
		/// </summary>
		public Neuron neuronProp{ get{return neuron;} set{neuron = (value!=null)?value:neuron;} }
		/// <summary>
		/// The reference to the neuron which belongs to this Linear Step activation function
		/// </summary>
		protected Neuron neuron;

		/// <summary>
		/// Returns the output of the Linear Step activation function; if(net>threshold) return 1; else return 0;
		/// </summary>
		public float outputProp
		{
			get
			{
				if(neuronProp.netProp > neuronProp.thresholdProp)
					return  1.0f;
				else
					return 0.0f;
			}
		}

		/// <summary>
		/// Returns the first derivation output of the Linear Step activation function; if(0.0001>Abs(net)) float.MaxValue; else return 0;
		/// </summary>
		public float derivOutputProp
		{
			get
			{
				if (Math.Abs(neuronProp.netProp) < 0.0001)
					return float.MaxValue;
				else
					return 0;
			}
		}

		/// <summary>
		/// Returns the minimum value for the Linear Step activation function
		/// </summary>
		public float minProp{ get{ return 0.0f;} }

		/// <summary>
		/// Returns the maximum value for the Linear Step activation function
		/// </summary>
		public float maxProp{ get{ return 1.0f;} }

		/// <summary>
		/// Contains the String(s) with the specific properties for the Linear Step activation function.
		/// <see cref="MultilayerNet.IActivationFunction.activationProps"/> for an example
		/// </summary>
		public string[] activationProps{ get{return new string[]{};} }

		/// <summary>
		/// Constructor, init the belonging neuron
		/// </summary>
		public StepActFunc(Neuron n)
		{
			this.neuronProp = n;
		}

		/// <summary>
		/// The Defaultconstructor, inits the belonging neuron with null
		/// </summary>
		public StepActFunc() : this(null) {}
	}
}