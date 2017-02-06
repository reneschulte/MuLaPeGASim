using System;

namespace MultilayerNet
{
	/// <summary>
	/// Description: A simple class containing the input and the depending teach output for a set of patterns
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class Patterns
	{
		/// <summary>
		/// The requested output (teach output) patterns corresponding to the input set
		/// </summary>		
		public float[][] teachOutputsProp
		{ 
			get {return teachOutputs;} 
			set
			{
				if(value == null)
					return;
				else if (inputsProp != null)
					if(inputsProp.Length != value.Length)
						throw new Exception("the input set must have the same length as the teach output set!");	 
				teachOutputs = value;
			} 
		}
		/// <summary>
		/// The requested output (teach output) set corresponding to the input set
		/// </summary>	
		protected float[][] teachOutputs;

		/// <summary>
		/// The input patterns corresponding to the teachOutput
		/// </summary>		
		public float[][] inputsProp 
		{ 
			get {return inputs;} 
			set
			{
				if(value == null)
					return;
				else if (teachOutputsProp != null)
					if(teachOutputsProp.Length != value.Length)
						throw new Exception("the input set must have the same length as the teach output set!");	 
				inputs = value;
			} 
		}
		/// <summary>
		/// The input patterns corresponding to the teachOutput
		/// </summary>	
		protected float[][] inputs;

		/// <summary>
		/// The number of input / teach output patterns, which this object contains
		/// </summary>
		public int Length { get {return inputs==null?0:inputs.Length;} }

		/// <summary>
		/// Defaultconstructor -> does nothing
		/// </summary>
		public Patterns() {}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="inputs">the input set</param>
		/// <param name="teachOutputs">the corresponding teach output set</param>
		public Patterns(float[][] inputs, float[][] teachOutputs)
		{
			this.inputsProp = inputs;
			this.teachOutputsProp = teachOutputs;
		}
	}
}
