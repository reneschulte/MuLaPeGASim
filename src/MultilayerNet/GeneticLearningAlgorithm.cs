using System;
using System.Collections;

namespace MultilayerNet
{
	/// <summary>
	/// Description: Genetic Learning algorithm for neural network training, 
	/// uses the Real-Number-Crossover- and Mutation-Theory from the lecture script 
	/// of Prof. Dr. Heino Iwe ;-)
	/// <para>Author: Torsten Baer</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-27-2004</para>
	/// </summary>
	public class GeneticLearningAlgorithm : LearningAlgo
	{
		/// <summary>
		/// The random generator
		/// </summary>
		protected Random rand;

		/// <summary>
		/// The Mutation-Rate in percent for the Genetic Mutation Operator
		/// </summary>
		protected float mutationRate;
		/// <summary>
		/// The Mutation-Rate in percent for the Genetic Mutation Operator
		/// <seealso cref="MultilayerNet.NeuralNet" />
		/// </summary>
		public float mutationRateProp
		{
			get { return mutationRate*100; }
			set
			{
				if(value >= this.mutationRateMinProp && value <= this.mutationRateMaxProp)
					mutationRate = value/100;
				else
					throw new Exception(	"The Mutation-Rate has to be between" +
						this.mutationRateMinProp.ToString() + " and " +
						this.mutationRateMaxProp.ToString() + " %");
			}
		}

		/// <summary>
		/// Returns the minimum value in percent which could be used to set the minimum for adjusting the Mutation-Rate
		/// </summary>
		public float mutationRateMinProp { get { return  0f; } }

		/// <summary>
		/// Returns the maximum in percent value which could be used to set the minimum for adjusting the Mutation-Rate
		/// </summary>
		public float mutationRateMaxProp { get { return  6f; } }

		/// <summary>
		/// Returns the step range in percent which could be used to set the step for adjusting the Mutation-Rate
		/// </summary>
		public float mutationRateStepProp { get { return 0.1f; } }

		/// <summary>
		/// The number of the current generation
		/// </summary>
		protected int generationCount;
		/// <summary>
		/// Returns the number of the current generation
		/// </summary>
		public int gernerationCountProp
		{
			get { return generationCount+1; }
		}

		/// <summary>
		/// Saves the number of individuals in each generation for one calculation cycle. 
		/// The number of individuals can be changed by nIndividualsProp
		/// </summary>
		protected int nIndividualsForCalculation;

		/// <summary>
		/// The Number of individuals in each generation, only even values are allowed
		/// </summary>
		protected int nIndividuals;
		/// <summary>
		/// The Number of individuals in each generation, only even values are allowed
		/// <seealso cref="MultilayerNet.NeuralNet"/>
		/// </summary>
		public int nIndividualsProp
		{
			get { return nIndividuals; }
			set 
			{
				if(value >= this.nIndividualsMinProp && value%2==0)
					nIndividuals = value;
				else
					throw new Exception("There have to be " + this.nIndividualsMinProp + 
						" Individuals at minimum and " + this.nIndividualsMaxProp + 
						" at maximum and it must be a even Number!");
			}
		}

		/// <summary>
		/// Returns the minimum value which could be used to set the minimum for adjusting the individuals
		/// </summary>
		public int nIndividualsMinProp { get { return   10; } }

		/// <summary>
		/// Returns the maximum value which could be used to set the minimum for adjusting the individuals
		/// </summary>
		public int nIndividualsMaxProp { get { return 1000; } }

		/// <summary>
		/// Returns the step range which could be used to set the step for adjusting the individuals
		/// </summary>
		public int nIndividualsStepProp { get { return   2; } }

		/// <summary>
		/// The Number of individuals in each generation as a float value, only even values are allowed
		/// </summary>
		public float nIndividualsFloatProp
		{
			get { return (float)nIndividualsProp; }
			set { nIndividualsProp = (int)value; } 
		}

		/// <summary>
		/// Returns the minimum value as a float which could be used to set the minimum for adjusting the individuals
		/// </summary>
		public float nIndividualsFloatMinProp { get { return (float)nIndividualsMinProp; } }
		
		/// <summary>
		/// Returns the maximum value as a float which could be used to set the minimum for adjusting the individuals
		/// </summary>
		public float nIndividualsFloatMaxProp { get { return (float)nIndividualsMaxProp; } }

		/// <summary>
		/// Returns the step range as a float which could be used to set the step for adjusting the individuals
		/// </summary>
		public float nIndividualsFloatStepProp { get { return (float)nIndividualsStepProp; } }

		/// <summary>
		/// The Crossoverrate describes how many percent of the individuals will marry in each generation. 
		/// This variable is only used for internal calculation
		/// </summary>
		protected float crossOverRateForCalculation;

		/// <summary>
		/// The Crossoverrate describes how many percent of the individuals will marry in each generation
		/// </summary>
		protected float crossOverRate;
		/// <summary>
		/// The Crossoverrate describes how many percent of the individuals will marry in each generation
		/// </summary>
		public float crossOverRateProp
		{
			get { return this.crossOverRate*100; }
			set
			{
				if(value >= this.crossOverRateMinProp && value <= this.crossOverRateMaxProp)
					this.crossOverRate = value/100;
				else
					throw new Exception("The Crossover Rate have to be greater than " +
						this.crossOverRateMinProp.ToString() + " and smaller than " +
						this.crossOverRateMaxProp.ToString() + "!");
			}
		}

		/// <summary>
		/// Returns the minimum value which could be used to set the minimum for adjusting the Crossoverrate
		/// </summary>
		public float crossOverRateMinProp { get { return   0.0f; } }

		/// <summary>
		/// Returns the maximum value which could be used to set the minimum for adjusting the Crossoverrate
		/// </summary>
		public float crossOverRateMaxProp { get { return 100.0f; } }

		/// <summary>
		/// Returns the step range which could be used to set the step for adjusting the Crossoverrate
		/// </summary>
		public float crossOverRateStepProp { get { return  1.0f; } }

		/// <summary>
		/// The elitism rate, which will control the possibility of mutation for the individuals by marriage and elitism
		/// </summary>
		protected float elitismRate;
		/// <summary>
		/// The elitism rate, which will control the possibility of mutation for the individuals by marriage and elitism
		/// </summary>
		public float elitismRateProp
		{
			get { return this.elitismRate*100; }
			set 
			{
				if(value >= this.elitismRateMinProp && value <= this.elitismRateMaxProp)
					this.elitismRate = value/100;
				else
					throw new Exception("The Elitismusrate have to be bigger than " +
						this.elitismRateMinProp.ToString() + " and lower than " +
						this.elitismRateMaxProp.ToString() + "!");
			}
		}
		/// <summary>
		/// Returns the minimum value which could be used to set the minimum for adjusting the elitism rate
		/// </summary>
		public float elitismRateMinProp { get { return 0f; } }
		/// <summary>
		/// Returns the maximum value which could be used to set the minimum for adjusting the elitism rate
		/// </summary>
		public float elitismRateMaxProp { get { return 3f; } }
		/// <summary>
		/// Returns the step range which could be used to set the step for adjusting the elitism rate
		/// </summary>
		public float elitismRateStepProp { get{ return 0.1f; } }

		/// <summary>
		/// The event which is thrown, after the summed global error was calculated
		/// </summary>
		public override event errorHandler onErrorCalculated;
		
		/// <summary>
		/// The event which is thrown, after the output was calculated
		/// </summary>
		public override event outputHandler onOutCalculated;

		/// <summary>
		/// Contains the String(s) with the specific properties for the Genetic learning algorithm
		/// <see cref="MultilayerNet.LearningAlgo.algoProps"/> for an example
		/// </summary>
		public override string[] algoProps
		{
			get { return new string[] {"mutationRate", "nIndividualsFloat", "crossOverRate", "elitismRate"}; } 
		}

		/// <summary>
		/// Contains the String(s) with the description(s) for the specific properties of the Genetic learning algorithm
		/// </summary>
		public override string[] algoPropsDescription
		{
			get { return new string[] {"Mutation rate [%]", "Populationsize", "Crossover rate [%]", "Elitism rate [%]"}; } 
		}

		/// <summary>
		/// Defaultconstructor, init the Network with null and use 50 individuals
		/// <seealso cref="MultilayerNet.NeuralNet"/>
		/// </summary>
		public GeneticLearningAlgorithm() : this(null)	{}

		/// <summary>
		/// Constructor -> use 50 individuals
		/// </summary>
		/// <param name="nn">
		/// The Network that should be optimized
		/// </param>
		public GeneticLearningAlgorithm(NeuralNet nn) : this(nn,50)	{}

		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="nn">The Network that should be optimized</param>
		/// <param name="nIndividuals">The number of individuals for each generation, has to be > than 10</param>
		public GeneticLearningAlgorithm(NeuralNet nn, int nIndividuals) : base(nn)
		{
			if(nIndividuals%2 != 0)
				this.nIndividuals = nIndividuals+1;
			else
				this.nIndividuals = nIndividuals;

			this.mutationRate = this.mutationRate/100+(this.mutationRateMaxProp/100-this.mutationRateMinProp/100)/2;
			this.crossOverRate = this.crossOverRateMinProp/100+(this.crossOverRateMaxProp/100-this.crossOverRateMinProp/100)*0.60f;
			this.elitismRate = this.elitismRateMinProp/100+(this.elitismRateMaxProp/100-this.elitismRateMinProp/100)*0.6f;

			// defined random generator
			this.rand = new Random();

			onErrorCalculated = null;
			onOutCalculated = null;
		}

		/// <summary>
		/// Train one pattern
		/// </summary>
		/// <param name="teachOutput">the requested output for the current input of the net (NeuralNet.inputProp)</param>
		/// <returns>the trained Network which learned one iteration</returns>
		public override NeuralNet learnOnePattern(float[] teachOutput)
		{
			this.teachOutput = new float[][] { teachOutput };
			this.input = new float[][] {nn.inputProp};
			
			return learnPatterns();
		}
		
		/// <summary>
		/// Train the Network with patterns that where already init. before
		/// <seealso cref="MultilayerNet.NeuralNet"/>
		/// </summary>
		/// <returns>the trained Network</returns>
		public override NeuralNet learnPatterns()
		{
			return learnPatterns(this.teachOutput, this.input);
		}

		/// <summary>
		/// Train the Network with the patterns (teachOutputProp, inputProp)
		/// </summary>
		/// <param name="teachOutputs">the requested output</param>
		/// <param name="inputs">the input patterns for the net</param>
		/// <returns>the trained Network</returns>
		public override NeuralNet learnPatterns(float[][] teachOutputs, float[][] inputs)
		{
			this.teachOutput = teachOutputs;
			this.input = inputs;

			ArrayList[] generations = null;

			ArrayList currentGeneration = null;

			if(this.teachOutput == null || this.input == null)
				throw new Exception("Patterns not initialized -> teachOutput == null AND input == null\nor different item count between input andd output");

			initializeGenerations(out generations, out currentGeneration);

			for(generationCount=0; generationCount<this.maxIter; generationCount++)
			{
				this.iter = generationCount;
				float phiSum = 0.0f;
				float minError = float.PositiveInfinity;
				int minErrorAt = 0;

				// Number of Individuals changed
				if(this.nIndividuals > this.nIndividualsForCalculation)
				{
					int [][] dim = ((Individual)currentGeneration[0]).Dimensions;
					for(int i=0; i<this.nIndividuals-this.nIndividualsForCalculation; i++)
					{
						currentGeneration.Add(new Individual(dim));
					}
					this.nIndividualsForCalculation = this.nIndividuals;
				}
				else if(this.nIndividuals < this.nIndividualsForCalculation)
				{
					currentGeneration.RemoveRange(	this.nIndividuals, this.nIndividualsForCalculation-this.nIndividuals);
					this.nIndividualsForCalculation = this.nIndividuals;
				}

				if(this.crossOverRate != this.crossOverRateForCalculation)
					this.crossOverRateForCalculation = this.crossOverRate;

				// Test Individuals in phenotype-Area	
				foreach (Individual t in currentGeneration)
				{
					nn.weightNetProp = t.Parameter;
					t.Error = 0.0f;

					// Test the presented Patterns
					for(int i=0; i<teachOutputs.Length; i++)
					{
						if(this.input[i].Length != nn.inputProp.Length)
							throw new Exception("Inputvector must have the same length as the input of the neural Network!");
						if(teachOutput[i].Length != nn.outputProp.Length)
							throw new Exception("teachOutput vector must have the same length as the number of neurons in the output layer");
						
						nn.inputProp = this.input[i];

						float[] o = nn.propagate();

						if(this.onOutCalculated != null)
							onOutCalculated(nn,i);

						// sum up the pattern-error for using as Rare-Fitness and Standard-Fitness
						for(int j=0; j<o.Length; j++)
						{
							t.Error += 0.5f*(float)Math.Pow(this.teachOutput[i][j]-o[j],2);
						}
					}

					// save the minimal Error
					if(t.Error < minError)
					{
						minError = t.Error;
						minErrorAt = currentGeneration.IndexOf(t);
						this.error = error;
					}
					// calculate the adapted fitness
					t.Fitness = 1/(1+t.Error);
					// build the sum to normalize the fitness
					phiSum += t.Fitness;
				}
				// calculate the normalized fitness
				foreach (Individual t in currentGeneration)
				{
					t.Fitness = t.Fitness/phiSum;
				}

				if(this.onErrorCalculated != null)
					this.onErrorCalculated(minError);
				// if the tolerable error is reached -> break
				if(minError <= this.minTolerableError)
				{
					this.error = minError;
					nn.weightNetProp = (float[][][])((Individual)currentGeneration[minErrorAt]).Parameter;
					return nn;
				}

				// Select Marriage-Pool
				ArrayList marriagePool = rankBasedSelection(currentGeneration);
				
				// Marriage of the selected Individuals
				ArrayList childPool = new ArrayList();
				Individual[] childs;
				for(int j=0; j<=this.nIndividualsForCalculation-2; j+=2)
				{
					if(rand.NextDouble() < this.crossOverRateForCalculation)
						childs = marryIndividuals((Individual)marriagePool[j],(Individual)marriagePool[j+1]);
					else
						childs = new Individual[] { (Individual)marriagePool[j], (Individual)marriagePool[j+1] };

					childPool.AddRange(childs);
				}

				ArrayList nextGeneration = generations[(generationCount+1)%2];
				nextGeneration.Clear();
								
				// sort the Pools to select the Individuals
				childPool.Sort();

				// Elitismus-factor (how many good Individuals will be in the next Generation)
				float n = this.elitismRate;

				// Elitismus - n of N will be in the next Generation (maybe mutated)
				for(int j=0; j<Math.Floor(this.nIndividualsForCalculation*n); j++)
				{
					nextGeneration.Add(mutateIndividual((Individual)currentGeneration[j]));
				}
				for(int j=0; j<Math.Ceiling(this.nIndividualsForCalculation*(1-n)); j++)
				{
					nextGeneration.Add(mutateIndividual((Individual)childPool[j]));
				}

				currentGeneration = nextGeneration;
			}

			if(this.generationCount == this.maxIter)
			{
				currentGeneration.Sort();
				currentGeneration.Reverse();
			}

			this.error = ((Individual)currentGeneration[0]).Error;
			nn.weightNetProp = (float[][][])((Individual)currentGeneration[0]).Parameter;
			return nn;
		}

		/// <summary>
		/// Initializes the genetic Algotithm-Generations
		/// </summary>
		/// <param name="generations">a Reference on an Array of ArrayLists, which should hold the initialized generations</param>
		/// <param name="currentGeneration">a Reference on the current generation</param>
		protected void initializeGenerations(out ArrayList[] generations, out ArrayList currentGeneration)
		{
			if(this.nn == null)
				throw new Exception("No neural Network to train");

			this.nIndividualsForCalculation = this.nIndividuals;

			generations = new ArrayList[] {	new ArrayList(),
											  new ArrayList(0) };

			generationCount = 0;
			currentGeneration = generations[generationCount];

			// int-Vector with Dimensions for each Individual
			int [][] dimensions = new int[nn.weightNetProp.Length][];
			for(int i=0; i<nn.weightNetProp.Length; i++)
			{
				dimensions[i] = new int [nn.weightNetProp[i].Length];
				for(int j=0; j<nn.weightNetProp[i].Length; j++)
					dimensions[i][j] = nn.weightNetProp[i][j].Length;
			}
			
			// use the current Net also for training
			currentGeneration.Add(new Individual(this.nn.weightNetProp));

			// fill the current Generation with Individuals
			for(int i=1; i<this.nIndividualsForCalculation; i++)
			{
				currentGeneration.Add(new Individual(dimensions));
			}
		}

		#region Algorithms for Selecting, Mutating and the Marriage of Individuals
		/// <summary>
		/// Select the individuals for marriage rank based
		/// </summary>
		/// <param name="generation">the generation with the individuals</param>
		/// <returns>the marriage candidates</returns>
		protected ArrayList rankBasedSelection(ArrayList generation)
		{
			ArrayList marriagePool = new ArrayList();

			generation.Sort();
				
			float[] pi = new float[this.nIndividualsForCalculation];
			// selective pressure
			float q = (float)rand.Next(10000,20000)/100000;
			float c = 1/(1-((float)Math.Pow(1-q, this.nIndividualsForCalculation)));

			for(int j=0; j<this.nIndividualsForCalculation; j++)
				pi[j] = c*q*(float)Math.Pow(1-q, j);

			for(int j=0; j<this.nIndividualsForCalculation; j++)
			{
				float random = (float)rand.NextDouble();
				int k=0;
				do {} while (random < pi[k++] && k < pi.Length);
				marriagePool.Add(generation[k-1]);
			}

			return marriagePool;
		}

		/// <summary>
		/// Mutates the given individual
		/// </summary>
		/// <param name="p">the individual</param>
		/// <returns>a maybe mutated individual</returns>
		protected Individual mutateIndividual(Individual p)
		{
			float b = 2, ximin = Individual.xmin, ximax = Individual.xmax;
			int dim1 = rand.Next(p.Parameter.Length),
				dim2 = rand.Next(p.Parameter[dim1].Length),
				dim3 = rand.Next(p.Parameter[dim1][dim2].Length);
		
			if(rand.NextDouble() <= mutationRate)
			{
				float xi = p[dim1][dim2][dim3];
				if(rand.NextDouble() <= 0.5)
					p[dim1][dim2][dim3] = (float)(xi+(ximax-xi)*rand.NextDouble()*
						Math.Pow(1-this.generationCount/this.maxIter, b));
				else
					p[dim1][dim2][dim3] = (float)(xi-(xi-ximin)*rand.NextDouble()*
						Math.Pow(1-this.generationCount/this.maxIter, b));
			}

			return p;
		}
		
		/// <summary>
		/// Calculates the child individuals for the given parents
		/// </summary>
		/// <param name="mom">the 'Mother'</param>
		/// <param name="dad">the 'Father'</param>
		/// <returns>a vector with the childs</returns>
		protected Individual[] marryIndividuals(Individual mom, Individual dad)
		{
			float q = 0.15f;
			float alpha = (float)rand.NextDouble();
			float beta = 1-alpha;
			Individual[] childs = new Individual[] { new Individual(mom.Dimensions),
													 new Individual(mom.Dimensions) };

			for(int l=0; l < childs.Length; l++)
			{
				Individual child = childs[l];
				for(int i=0; i<mom.Dimensions.Length; i++)
					for(int j=0; j<mom.Dimensions[i].Length; j++)
						for(int k=0; k<mom.Dimensions[i][j]; k++)
						{
							if(l%2 == 0)
								child[i][j][k] = alpha*dad[i][j][k] + beta*mom[i][j][k];
							else
								child[i][j][k] = beta*dad[i][j][k] + alpha*mom[i][j][k];

							child[i][j][k] += 2*q*((float)rand.NextDouble())*
								((dad[i][j][k]*mom[i][j][k])/(dad[i][j][k]+mom[i][j][k]));
						}
			}

			return childs;
		}

		#endregion

		#region Individual for the Genetic Algorithm

		/// <summary>
		/// Individual for the Genetic algorithm
		/// </summary>
		protected class Individual : IComparable
		{
			/// <summary>
			/// Represents the 3-dimensional weight matrix of an neural network
			/// </summary>
			protected float[][][] parameter;

			/// <summary>
			/// Represents the fitness of the individual based on the error. 
			/// The fitness will be calculated during the propagation of the generations
			/// </summary>
			protected float fitness;

			/// <summary>
			/// Saves the error of the represented neural network
			/// </summary>
			protected float error;
		
			/// <summary>
			/// The static random generator
			/// </summary>
			protected static Random rand;

			/// <summary>
			/// Defines the minimal value of one weight at the initialization
			/// </summary>
			public static readonly float xmin = -0.75f;

			///<summary>
			/// Defines the maximal value of one weight at the initialization
			/// </summary>
			public static readonly float xmax =  0.75f;

			/// <summary>
			/// Represents the fitness of the individual based on the error. 
			/// The error is calculated during the test in the phenotype area
			/// </summary>
			public float Fitness
			{
				get { return fitness; }
				set { fitness = value; }
			}

			/// <summary>
			/// Saves the error of the represented neural network
			/// </summary>
			public float Error
			{
				get { return error; }
				set { error = value; }
			}

			/// <summary>
			/// Access the whole parameter vector
			/// </summary>
			public float[][][] Parameter
			{
				get { return parameter; }
				set { parameter = value; }
			}

			/// <summary>
			/// Indexer to access the first dimension of the 3-dimensional Parameter-vector directly
			/// </summary>
			public float[][] this[int index]
			{
				get { return parameter[index]; }
				set { parameter[index] = value; }
			}

			/// <summary>
			/// Indexer to access the second dimension of the 3-dimensional Parameter-vector directly
			/// </summary>
			public float[] this[int index1,int index2]
			{
				get { return parameter[index1][index2]; }
				set { parameter[index1][index2] = value; }
			}

			/// <summary>
			/// Indexer to access the third dimension of the 3-dimensional Parameter-vector directly
			/// </summary>
			public float this[int index1, int index2, int index3]
			{
				get { return parameter[index1][index2][index3]; }
				set { parameter[index1][index2][index3] = value; }
			}

			/// <summary>
			/// Returns the dimensions of the Parameter-matrix
			/// </summary>
			public int[][] Dimensions
			{
				get
				{
					int [][] dim = new int[this.parameter.Length][];
					for(int i=0; i<dim.Length; i++)
					{
						dim[i] = new int[this.parameter[i].Length];
						for(int j=0; j<dim[i].Length; j++)
							dim[i][j] = this.parameter[i][j].Length;
					}
					return dim;
				}
			}

			/// <summary>
			/// Constructs an individual with a parameter matrix, which will be defined by the dimensions 
			/// and values of the given two dimensional integer array
			/// </summary>
			/// <param name="dimensions">The dimensions of the parameter matrix</param>
			public Individual(int[][] dimensions)
			{
				// Initialize the parameter Vector
				parameter = new float[dimensions.Length][][];;
				for(int i=0; i<dimensions.Length; i++)
				{
					parameter[i] = new float[dimensions[i].Length][];
					for(int j=0; j<dimensions[i].Length; j++)
					{
						parameter[i][j] = new float[dimensions[i][j]];
					}
				}
				// set the parameter-values to a random number
				for(int i=0; i<parameter.Length; i++)
					for(int j=0; j<parameter[i].Length; j++)
						for(int k=0; k<parameter[i][j].Length; k++)
							parameter[i][j][k] = xmin+(float)rand.NextDouble()*(xmax-xmin);

				this.fitness = 0.0f;
			}

			/// <summary>
			/// Contrucs an individual with the given parameter matrix
			/// </summary>
			/// <param name="parameter">The weight matrix of a neural network</param>
			public Individual(float[][][] parameter)
			{
				this.parameter = parameter;
				this.fitness = fitness;
			}

			/// <summary>
			/// Static constructor for initializing the random generator
			/// </summary>
			static Individual()
			{
				rand = new Random();
			}

			/// <summary>
			/// Implementing the IComparer Interface to sort the individuals by the fitness of each -> 
			/// the best will be on top
			/// </summary>
			/// <param name="x">the Individual to which this Individual will be compared</param>
			/// <returns>
			///	1>	if this.Fitness > x.Fitness 
			///	0	if this.Fitness = x.Fitness 
			///	>1	if	  x.Fitness > this.Fitness
			/// </returns>
			public int CompareTo(object x)
			{
				try
				{
					return ((Individual)x).fitness.CompareTo(this.fitness);
				}
				catch(Exception)
				{
					throw new Exception("Argument is not a Genetic Individual");
				}
			}
		}
		#endregion Genetic Individual
	}	
}