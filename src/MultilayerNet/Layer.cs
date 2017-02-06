using System;

namespace MultilayerNet
{
	/// <summary>
	/// Description: A layer in a neural net. The layers are connected with the variable prevLayer -> linked list
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-30-2004</para>
	/// </summary>
	public class Layer
	{	
		/// <summary>
		/// The array of neurons in the layer
		/// </summary>	
		public Neuron[] neuronsProp{ get{return neuronsInLayer;} set{neuronsInLayer = value;} }
		/// <summary>
		/// The array of neurons in the layer
		/// </summary>	
		protected Neuron[] neuronsInLayer;
		
		/// <summary>
		/// The previous layer of the current layer (this)
		/// </summary>	
		public Layer prevLayerProp{ get{return prevLayer;} set{prevLayer = value;} }
		/// <summary>
		/// The previous layer of the current layer (this)
		/// </summary>	
		protected Layer prevLayer;
		
	/*	/// <summary>
		/// the next layer from the current (this) layer
		/// </summary>	
		protected Layer nextLayer;
		public Layer nextLayerProp{ get{return nextLayer;} set{nextLayer = value;} }
	*/	
		/// <summary>
		/// Returns the number of neurons in the layer
		/// </summary>	
		public int Length{ get{return this.neuronsProp.Length;} }

		/// <summary>
		/// The weight matrix of the neurons in the layer (gets incl. the bias)
		/// <para>weightLayerProp[index neuron in layer, from left to right][weight presynapt. neuron]</para>
		/// </summary>		
		public float[][] weightLayerProp
		{
			get
			{
				float[][] result = new float[neuronsProp.Length][];
				for(int i=0; i<neuronsProp.Length; i++)
					result[i] = neuronsProp[i].weightWithBiasProp;
				return result;
			}
			set
			{
				if(value != null)
					if(value.Length != neuronsProp.Length)
						throw new ArgumentException("Weight vector must have the same length as the input vector!");
				for(int i=0; i<neuronsProp.Length; i++)
					neuronsProp[i].weightWithBiasProp = value[i];
			}
		}

		/// <summary>
		/// The threshold vector of the neurons in the layer;
		/// <para>thresholdLayerProp[index neuron in layer, from left to right]</para>
		/// </summary>		
		public float[] thresholdLayerProp
		{
			get
			{
				float[] result = new float[neuronsProp.Length];
				for(int i=0; i<neuronsProp.Length; i++)
					result[i] = neuronsProp[i].thresholdProp;
				return result;
			}
			set
			{
				for(int i=0; i<neuronsProp.Length; i++)
					neuronsProp[i].thresholdProp = value[i];
			}
		}

		/// <summary>
		/// The output of the layer, which is calculated just in time.
		/// </summary>		
		public float[] outputProp
		{
			get
			{
				float[] result = new float[neuronsProp.Length];
				for(int i=0; i<neuronsProp.Length; i++)
					result[i] = neuronsProp[i].outputProp;
				return result;
			}
		}

		/// <summary>
		/// The input of the layer, which is calculated just in time.
		/// </summary>
		public float[] inputProp
		{
			get{ return this.neuronsProp[0].inputProp; }
			set
			{
				for(int i=0; i<neuronsProp.Length; i++)
					neuronsProp[i].inputProp = value;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="nNeurons">creates nNeurons neurons in the layer</param>
		public Layer(int nNeurons)
		{
			this.neuronsProp = new Neuron[nNeurons];
			for(int i=0; i<nNeurons; i++)
				this.neuronsProp[i] = new Neuron();
		}

		/// <summary>
		/// Defaultconstructor -> creates 3 neurons in the layer
		/// </summary>
		public Layer() : this(3) {}
	}
}

