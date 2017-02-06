using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Security;

namespace MultilayerNet
{
	/// <summary>
	/// Description: The singleton class with methods to read and write xml containing a NeuralNet, patterns or txt files. 
	/// Errorlogging could also be done.
	/// <para>Author: Rene Schulte</para>
	/// <para>Version: 1.0</para>
	/// <para>Last recent Update: 10-24-2004</para>
	/// </summary>
	public class FileManager
	{
		/// <summary>
		/// The only static instance of this class
		/// </summary>
		private static FileManager instance = new FileManager();
	
		/// <summary>
		/// The hidden Defaultconstructor
		/// </summary>
		private FileManager() {}
	
		/// <summary>
		/// Get the only static instance
		/// </summary>
		/// <returns>the only instance of this class</returns>
		public static FileManager getInstance()
		{
			return instance;
		}

		/// <summary>
		/// Writes a float 3D Matrix into a formated text file. 
		/// Usually used for writing the network weight to a file.
		/// </summary>
		/// <param name="filename">the file name where to store</param>
		/// <param name="matrix">the matrix to write</param>
		public void write3DMatrixToTextFile(string filename, float[][][] matrix)
		{
			FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
			StreamWriter sw = new StreamWriter(fs);
			for(int i=matrix.Length-1; i >=0 ; i--)
			{
				sw.WriteLine("Layer: {0}", i);
				for(int j=0; j < matrix[i].Length; j++)
				{
					for(int k=0; k < matrix[i][j].Length; k++)
						sw.Write("{0,12:F6}", matrix[i][j][k]);
					sw.WriteLine(" => Neuron: {0}", j);
				}
				sw.WriteLine();
			}
			sw.Flush();
			sw.Close();
			fs.Close();
		}

		/// <summary>
		/// Appends a float value to a text file.
		/// <seealso cref="MultilayerNet.NeuralNet"/>
		/// </summary>
		/// <param name="filename">the file name where to store</param>
		/// <param name="val">the value to write</param>
		public void writeFloatToTextFile(string filename, float val)
		{
			FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write);
			StreamWriter sw = new StreamWriter(fs);
			sw.WriteLine(val);
			sw.Flush();
			sw.Close();
			fs.Close();
		}


		#region logging methods / attributes

		/// <summary>
		/// The filename in which the logging information is written -> default: error.log
		/// </summary>
		public string LOGFILE_NAME_PROP
		{
			get { return LOGFILE_NAME; }
			set { if(value != null && !value.Equals("")) LOGFILE_NAME = value; }
		}
		/// <summary>
		/// The filename in which the logging information is written -> default: error.log
		/// </summary>
		private string LOGFILE_NAME = "error.log";

		/// <summary>
		/// Writes a string to the logging file specified by LOGFILE_NAME
		/// </summary>
		/// <param name="msg">the message which should be written</param>
		public void writeStringToErrorLogFile(string msg)
		{
			FileStream fs = new FileStream(LOGFILE_NAME, FileMode.Append, FileAccess.Write);
			StreamWriter sw = new StreamWriter(fs);
			sw.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ MESSAGE +++++++++++++++++++++++++++++++++++++++++++++++++++++");
			sw.WriteLine(DateTime.Now);
			sw.WriteLine();
			sw.WriteLine(msg);
			sw.WriteLine();
			sw.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
			sw.WriteLine();
			sw.WriteLine();
			sw.Flush();
			sw.Close();
			fs.Close();
		}

		/// <summary>
		/// Writes the properties of the Exception to the logging file specified by LOGFILE_NAME
		/// </summary>
		/// <param name="exc">the Exception which should be written</param>
		public void writeExceptionToErrorLogFile(Exception exc)
		{
			FileStream fs = new FileStream(LOGFILE_NAME, FileMode.Append, FileAccess.Write);
			StreamWriter sw = new StreamWriter(fs);
			sw.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ EXCEPTION ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
			sw.WriteLine(DateTime.Now);
			sw.WriteLine();
			sw.WriteLine(exc.ToString());
			sw.WriteLine();
			sw.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
			sw.WriteLine();
			sw.WriteLine();
			sw.Flush();
			sw.Close();
			fs.Close();
		}

		/// <summary>
		/// Writes some system informations, .Net-, Assembly Version, ... to the logging file specified by LOGFILE_NAME
		/// </summary>
		public void writeSystemInfosToErrorLogFile()
		{
/*			string[] asmFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.dll");
			Assembly asm;
			FileInfo fileInfo;
			int index;
			for(int i=0; i<asmFiles.Length; i++)
			{
				fileInfo = new FileInfo(asmFiles[i]);
				index = fileInfo.Name.LastIndexOf(fileInfo.Extension);
				asm = Assembly.LoadWithPartialName(fileInfo.Name.Substring(0, index));
				if(asm != null)
					asmFiles[i] = asm.FullName;
				else
					asmFiles[i] += "   ...could not be reflected!";
			}
*/
			FileStream fs = new FileStream(LOGFILE_NAME, FileMode.Append, FileAccess.Write);
			StreamWriter sw = new StreamWriter(fs);

			sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------");
			sw.WriteLine("------------------------------------------------------------ SYSTEMINFOS ---------------------------------------------------");
			sw.WriteLine();
			sw.WriteLine("Date / Time: {0}", DateTime.Now);
			sw.WriteLine();
			sw.WriteLine("OS: {0} Version: {1}", Environment.OSVersion.Platform, Environment.OSVersion.Version);
			sw.WriteLine("System Dir.: ", Environment.SystemDirectory);
			sw.WriteLine("Bootmode: {0}", System.Windows.Forms.SystemInformation.BootMode);
			sw.WriteLine("Has shutdown started: {0}", Environment.HasShutdownStarted);
			sw.WriteLine(".NET Framework Version: {0} ", Environment.Version);
			sw.WriteLine();
			sw.WriteLine("Starting Application: {0}, Version: {1}", System.Windows.Forms.Application.ProductName,
																	System.Windows.Forms.Application.ProductVersion);
			sw.WriteLine("Physical memory for this process (bytes): {0}", Environment.WorkingSet);
			sw.WriteLine();
			sw.WriteLine("Attributes of the referenced Assemblies from Entry Assembly: ");
			AssemblyName[] asms = Assembly.GetEntryAssembly().GetReferencedAssemblies();
			for(int i=0; i<asms.Length; i++)
				sw.WriteLine(asms[i].FullName);
			sw.WriteLine();
			sw.WriteLine("Current Dir.: {0}", Environment.CurrentDirectory);
			sw.WriteLine(getContentOfDir(Environment.CurrentDirectory));
			sw.WriteLine();
			sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------");
			sw.WriteLine();
			sw.WriteLine();
			sw.Flush();
			sw.Close();
			fs.Close();
		}

		/// <summary>
		/// Reads recursive the content of a directory
		/// </summary>
		/// <param name="dir">the directory which content should be read</param>
		/// <returns>the content of the directory and subfolders -> formated</returns>
		public string getContentOfDir(string dir)
		{
			return getContentOfDir(Environment.CurrentDirectory, "");
		}

		/// <summary>
		/// Reads recursive the content of a directory
		/// </summary>
		/// <param name="dir">the directory which content should be read</param>
		/// <param name="whitespace">the whitespace for formating the directory tree</param>
		/// <returns>the content of the directory and subfolders -> formated</returns>
		protected string getContentOfDir(string dir, string whitespace)
		{
			StringBuilder sb = new StringBuilder();
			FileInfo fileInfo;
			string[] files = Directory.GetFiles(dir);
			string[] dirs = Directory.GetDirectories(dir);

			for(int i=0; i<dirs.Length; i++)
			{
				fileInfo = new FileInfo(dirs[i]);
				sb.Append(whitespace);
				sb.Append("|-- ");
				sb.Append(fileInfo.Name);
				sb.Append(Environment.NewLine);
				sb.Append(getContentOfDir(dirs[i], "|  " + whitespace));
			}
			for(int i=0; i<files.Length; i++)
			{
				fileInfo = new FileInfo(files[i]);
				sb.Append(whitespace);
				sb.Append("|-- ");
				sb.Append(fileInfo.Name);
				sb.Append(" (");
				sb.Append(fileInfo.Length);
				sb.Append(" Bytes, Last Write Time: ");
				sb.Append(fileInfo.LastWriteTime);
				sb.Append(")");
				sb.Append(Environment.NewLine);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Reads the content of the error log file specified by LOGFILE_NAME
		/// </summary>
		/// <returns>the content of the log file</returns>
		public string readErrorLogFile()
		{
			FileStream fs = new FileStream(LOGFILE_NAME, FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader(fs);
			StringBuilder sb = new StringBuilder((int)fs.Length);
			String str;
			while((str = sr.ReadLine())!= null)
			{
				sb.Append(str);
				sb.Append(Environment.NewLine);
			}
			sr.Close();
			fs.Close();
			return sb.ToString();
		}

		/// <summary>
		/// Deletes the logfile specified by LOGFILE_NAME
		/// </summary>
		public void deleteErrorLogFile()
		{
			File.Delete(LOGFILE_NAME);
		}

		#endregion

		# region .net file read / write

		/// <summary>
		/// Writes the whole network to Xml file
		/// <seealso cref="MultilayerNet.NeuralNet"/>
		/// </summary>
		/// <param name="fileName">the relative path</param>
		/// <param name="net">the MultilayerNet.NeuralNet instance</param>
		public void writeNetworkToXml(string fileName, NeuralNet net)
		{
			writeNetworkToXml(fileName, net, net.netNameProp);
		}

		/// <summary>
		/// Writes the whole network to Xml file
		/// <seealso cref="MultilayerNet.NeuralNet"/>
		/// </summary>
		/// <param name="fileName">the relative path</param>
		/// <param name="net">the MultilayerNet.NeuralNet instance</param>
		/// <param name="netName">the name for the net</param>
		public void writeNetworkToXml(string fileName, NeuralNet net, string netName)
		{
			XmlTextWriter writer = new XmlTextWriter(fileName, null);
			writer.Formatting = Formatting.Indented;
			writer.Indentation = 1;
			writer.IndentChar = '\t';
			Type t = null;
			float paramValue = 0;

			writer.WriteStartDocument();

				writer.WriteStartElement(net.ToString());
					writer.WriteAttributeString("number_of_layers", net.layersProp.Length.ToString());
					writer.WriteAttributeString("name", netName);

					writer.WriteStartElement("learning_algo");
						writer.WriteAttributeString("type", net.learnAlgoProp.ToString());
						writer.WriteAttributeString("max_iterations", net.learnAlgoProp.maxIterationProp.ToString());

						t = net.learnAlgoProp.GetType();
						for(int i=0; i<net.learnAlgoProp.algoProps.Length; i++)
						{
							paramValue = (float)t.GetProperty(net.learnAlgoProp.algoProps[i] + "Prop").GetValue(net.learnAlgoProp, null);
							writer.WriteAttributeString(net.learnAlgoProp.algoProps[i], paramValue.ToString());
						}

						if(net.learnAlgoProp.teachOutputProp != null)
						{
							writer.WriteStartElement("teach_outputs");
							writer.WriteAttributeString("number_of_teachpatterns", (net.learnAlgoProp.teachOutputProp.GetUpperBound(0)+1).ToString());
							for(int i=0; i<net.learnAlgoProp.teachOutputProp.GetUpperBound(0)+1; i++)
							{
								writer.WriteStartElement("teach_output_nr_" + i.ToString());
								for(int j=0; j<net.learnAlgoProp.teachOutputProp[i].Length; j++)
									writer.WriteAttributeString("t_" + j.ToString(), net.learnAlgoProp.teachOutputProp[i][j].ToString());
								writer.WriteEndElement();
							}
							writer.WriteEndElement();
						}

						if(net.learnAlgoProp.inputProp != null)
						{
							writer.WriteStartElement("input_patterns");
							writer.WriteAttributeString("number_of_inputpatterns", (net.learnAlgoProp.inputProp.GetUpperBound(0)+1).ToString());
							for(int i=0; i<net.learnAlgoProp.inputProp.GetUpperBound(0)+1; i++)
							{
								writer.WriteStartElement("input_pattern_nr_" + i.ToString());
								for(int j=0; j<net.learnAlgoProp.inputProp[i].Length; j++)
									writer.WriteAttributeString("i_" + j.ToString(), net.learnAlgoProp.inputProp[i][j].ToString());
								writer.WriteEndElement();
							}
							writer.WriteEndElement();
						}
					writer.WriteEndElement();

					writer.WriteStartElement("network_input");
					for(int i=0; i<net.inputProp.Length; i++)
						writer.WriteAttributeString("in_" + i.ToString(), net.inputProp[i].ToString());
					writer.WriteEndElement();

					writer.WriteStartElement("network_output");
					for(int i=0; i<net.outputProp.Length; i++)
						writer.WriteAttributeString("out_" + i.ToString(), net.outputProp[i].ToString());
					writer.WriteEndElement();

					writer.WriteStartElement("layers");
					for(int i=net.layersProp.Length-1; i>=0; i--)
					{
						if(i == net.layersProp.Length-1)
							writer.WriteStartElement("outputlayer");
						else
						{
							writer.WriteStartElement("hiddenlayer");
								writer.WriteAttributeString("nr", i.ToString());
						}
							writer.WriteAttributeString("number_of_neurons", net.layersProp[i].neuronsProp.Length.ToString());

						writer.WriteStartElement("layer_input_with_bias");
						for(int j=0; j<net.layersProp[i].inputProp.Length; j++)
							writer.WriteAttributeString("in_" + j.ToString(), net.layersProp[i].inputProp[j].ToString());
						writer.WriteEndElement();

						writer.WriteStartElement("layer_output");
						for(int j=0; j<net.layersProp[i].outputProp.Length; j++)
							writer.WriteAttributeString("out_" + j.ToString(), net.layersProp[i].outputProp[j].ToString());
						writer.WriteEndElement();

						for(int j=0; j<net.layersProp[i].neuronsProp.Length; j++)
						{
							writer.WriteStartElement("neuron");
								writer.WriteAttributeString("nr", j.ToString());
								writer.WriteAttributeString("output", net.layersProp[i].neuronsProp[j].outputProp.ToString());
								writer.WriteAttributeString("threshold",
															net.layersProp[i].neuronsProp[j].thresholdProp.ToString());
								writer.WriteAttributeString("activation_func",
															net.layersProp[i].neuronsProp[j].actFuncProp.ToString());

								t = net.layersProp[i].neuronsProp[j].actFuncProp.GetType();
								for(int k=0; k<net.layersProp[i].neuronsProp[j].actFuncProp.activationProps.Length; k++)
								{
									paramValue = (float)t.GetProperty(net.layersProp[i].neuronsProp[j].actFuncProp.activationProps[k]
																	  + "Prop").GetValue(net.layersProp[i].neuronsProp[j].actFuncProp, null);
									writer.WriteAttributeString(net.layersProp[i].neuronsProp[j].actFuncProp.activationProps[k], paramValue.ToString());
								}
								writer.WriteStartElement("input_with_bias");
								for(int k=0; k<net.layersProp[i].neuronsProp[j].inputWithBiasProp.Length; k++)
								{
										writer.WriteAttributeString("in_" + k.ToString(),
																	net.layersProp[i].neuronsProp[j].inputWithBiasProp[k].ToString());
								}
								writer.WriteEndElement();

								writer.WriteStartElement("weight_with_bias");
								for(int k=0; k<net.layersProp[i].neuronsProp[j].weightWithBiasProp.Length; k++)
								{
										writer.WriteAttributeString("w_" + k.ToString(),
																	net.layersProp[i].neuronsProp[j].weightWithBiasProp[k].ToString());
								}
								writer.WriteEndElement();
							writer.WriteEndElement();
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				writer.WriteEndElement();
			writer.WriteEndDocument();

			writer.Flush();
			writer.Close();
		}

		/// <summary>
		/// The name of the MultilayerNet Assembly. Needed for reflections
		/// </summary>
		public const string MULITLAYERNET_ASMNAME = "MultilayerNet";

		/// <summary>
		/// Reads a networks from a .net file
		/// </summary>
		/// <param name="fileName">the name of the file</param>
		/// <returns>the neural net</returns>
		public NeuralNet readNetworkFromXml(string fileName)
		{
			if(fileName.Substring(fileName.Length-3).ToUpper() != "NET")
				throw new ArgumentException("No .net file!" + "\nYou can only load networks from XML which were saved with this tool!");

			float[][][] netW = null;
			float[] netInput = null;
			float[][] input = null, teach = null;
			int patternIndex = 0, layerIndex = 0, neuronIndex = 0;
			int nPatterns = 0, nLayers = 0, nNeurons = 0;
			int[] hiddenLayers = null;
			string netName = null;
			LearningAlgo learnAlgo = null;
			IActivationFunction[][] actFuncs = null;
			XmlTextReader reader = new XmlTextReader(fileName);

			while(reader.Read())
			{
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "MultilayerNet.NeuralNet")
				{
					nLayers = int.Parse(reader["number_of_layers"]);
					netName = reader["name"];
					hiddenLayers = new int[nLayers-1];
					actFuncs = new IActivationFunction[nLayers][];
					netW = new float[nLayers][][];
					layerIndex = nLayers - 1;
				}
				else if(reader.LineNumber == 2 && reader.MoveToContent() == XmlNodeType.Element
						&& reader.Name != "MultilayerNet.NeuralNet")
				{
					Console.WriteLine(reader.LineNumber);
					throw new ArgumentException("XML file has not the required MultilayerNet.NeuralNet format!"
						+ "\nYou can only load networks from XML which were saved with this tool!");
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "learning_algo")
				{
					Assembly asm = Assembly.LoadWithPartialName(MULITLAYERNET_ASMNAME);
					Type t = asm.GetType(reader["type"], true, true);
					learnAlgo = (LearningAlgo)Activator.CreateInstance(t);
					learnAlgo.maxIterationProp = int.Parse(reader["max_iterations"]);
					reader.MoveToNextAttribute();
					reader.MoveToNextAttribute();
					for(int i=0; i<learnAlgo.algoProps.Length; i++)
					{
						reader.MoveToNextAttribute();
						if(reader.Name == "max_iterations")
							break;
						t.GetProperty(reader.Name + "Prop").SetValue(learnAlgo, float.Parse(reader[reader.Name]), null);
					}
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "teach_outputs")
				{
					nPatterns = int.Parse(reader["number_of_teachpatterns"]);
					teach = new float[nPatterns][];
					patternIndex = 0;
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "teach_output_nr_" + patternIndex)
				{
					teach[patternIndex] = new float[reader.AttributeCount];
					for(int j=0; j < reader.AttributeCount; j++)
						teach[patternIndex][j] = float.Parse(reader[j]);
					patternIndex++;
				}

				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "input_patterns")
				{
					nPatterns = int.Parse(reader["number_of_inputpatterns"]);
					input = new float[nPatterns][];
					patternIndex = 0;
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "input_pattern_nr_" + patternIndex)
				{
					input[patternIndex] = new float[reader.AttributeCount];
					for(int j=0; j < reader.AttributeCount; j++)
						input[patternIndex][j] = float.Parse(reader[j]);
					patternIndex++;
				}

				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "network_input")
				{
					netInput = new float[reader.AttributeCount];
					for(int j=0; j < reader.AttributeCount; j++)
						netInput[j] = float.Parse(reader[j]);
				}

				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "outputlayer")
				{
					nNeurons = int.Parse(reader["number_of_neurons"]);
					netW[layerIndex] = new float[nNeurons][];
					actFuncs[layerIndex] = new IActivationFunction[nNeurons];
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "neuron")
				{
					Assembly asm = Assembly.LoadWithPartialName(MULITLAYERNET_ASMNAME);
					Type t = asm.GetType(reader["activation_func"], true, true);
					actFuncs[layerIndex][neuronIndex] = (IActivationFunction)Activator.CreateInstance(t);
					reader.MoveToNextAttribute();
					reader.MoveToNextAttribute();
					reader.MoveToNextAttribute();
					reader.MoveToNextAttribute();
					for(int j=0; j<actFuncs[layerIndex][neuronIndex].activationProps.Length; j++)
					{
						reader.MoveToNextAttribute();
						if(reader.Name == "activation_func")
							break;
						t.GetProperty(reader.Name + "Prop").SetValue(actFuncs[layerIndex][neuronIndex], float.Parse(reader[reader.Name]), null);
					}
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "weight_with_bias")
				{
					netW[layerIndex][neuronIndex] = new float[reader.AttributeCount];
					for(int i=0; i < reader.AttributeCount; i++)
						netW[layerIndex][neuronIndex][i] = float.Parse(reader[i]);
				}
				if(reader.MoveToContent() == XmlNodeType.EndElement && reader.Name == "neuron")
					neuronIndex++;
				if(reader.MoveToContent() == XmlNodeType.EndElement && reader.Name == "outputlayer")
				{
					layerIndex--;
					break;
				}
			}

			while(reader.Read())
			{
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "hiddenlayer")
				{
					neuronIndex = 0;
					nNeurons = int.Parse(reader["number_of_neurons"]);
					netW[layerIndex] = new float[nNeurons][];
					actFuncs[layerIndex] = new IActivationFunction[nNeurons];
					hiddenLayers[layerIndex] = netW[layerIndex].Length;
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "neuron")
				{
					Assembly asm = Assembly.LoadWithPartialName(MULITLAYERNET_ASMNAME);
					Type t = asm.GetType(reader["activation_func"], true, true);
					actFuncs[layerIndex][neuronIndex] = (IActivationFunction)Activator.CreateInstance(t);
					reader.MoveToNextAttribute();
					reader.MoveToNextAttribute();
					reader.MoveToNextAttribute();
					reader.MoveToNextAttribute();
					for(int j=0; j<actFuncs[layerIndex][neuronIndex].activationProps.Length; j++)
					{
						reader.MoveToNextAttribute();
						if(reader.Name == "activation_func")
							break;
						t.GetProperty(reader.Name + "Prop").SetValue(actFuncs[layerIndex][neuronIndex], float.Parse(reader[reader.Name]), null);
					}
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "weight_with_bias")
				{
					netW[layerIndex][neuronIndex] = new float[reader.AttributeCount];
					for(int i=0; i < reader.AttributeCount; i++)
						netW[layerIndex][neuronIndex][i] = float.Parse(reader[i]);
				}
				if(reader.MoveToContent() == XmlNodeType.EndElement && reader.Name == "neuron")
					neuronIndex++;
				if(reader.MoveToContent() == XmlNodeType.EndElement && reader.Name == "hiddenlayer")
					layerIndex--;
			}
			reader.Close();


			// build Network from XML data
			NeuralNet nn = new NeuralNet(netInput, hiddenLayers, netW[nLayers-1].Length);
			nn.netNameProp = netName;
			nn.weightNetProp = netW;
			nn.actFuncForEachNeuronProp = actFuncs;
			nn.learnAlgoProp = learnAlgo;
			nn.learnAlgoProp.nnProp = nn;
			if(teach != null)
				nn.learnAlgoProp.teachOutputProp = teach;
			if(input != null)
				nn.learnAlgoProp.inputProp = input;

			return nn;
		}

		/// <summary>
		/// Reads only the weight from a .net file
		/// </summary>
		/// <param name="fileName">the name of the .net file</param>
		/// <returns>the network weights as a 3D float matrix</returns>
		public float[][][] readNetWeightFromXml(string fileName)
		{
			float[][][] netW = null;
			XmlTextReader reader = new XmlTextReader(fileName);
			int layerIndex=0, neuronIndex=0;

			while(reader.Read())
			{
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "MultilayerNet.NeuralNet")
				{
					layerIndex = int.Parse(reader["number_of_layers"]) - 1;
					netW = new float[layerIndex+1][][];
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "outputlayer")
					netW[layerIndex] = new float[int.Parse(reader["number_of_neurons"])][];
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "weight_with_bias")
				{
					netW[layerIndex][neuronIndex] = new float[reader.AttributeCount];
					for(int i=0; i < reader.AttributeCount; i++)
						netW[layerIndex][neuronIndex][i] = float.Parse(reader[i]);
				}
				if(reader.MoveToContent() == XmlNodeType.EndElement && reader.Name == "neuron")
					neuronIndex++;
				if(reader.MoveToContent() == XmlNodeType.EndElement && reader.Name == "outputlayer")
				{
					layerIndex--;
					break;
				}
			}

			while(reader.Read())
			{
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "hiddenlayer")
				{
					neuronIndex = 0;
					netW[layerIndex] = new float[int.Parse(reader["number_of_neurons"])][];
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "weight_with_bias")
				{
					netW[layerIndex][neuronIndex] = new float[reader.AttributeCount];
					for(int i=0; i < reader.AttributeCount; i++)
						netW[layerIndex][neuronIndex][i] = float.Parse(reader[i]);
				}
				if(reader.MoveToContent() == XmlNodeType.EndElement && reader.Name == "neuron")
					neuronIndex++;
				if(reader.MoveToContent() == XmlNodeType.EndElement && reader.Name == "hiddenlayer")
					layerIndex--;
			}

			reader.Close();
			return netW;
		}

		# endregion

		# region .pat file read / write

		/// <summary>
		/// Writes a set of patterns to a xml file
		/// </summary>
		/// <param name="fileName">the relative path</param>
		/// <param name="patterns">a set of input/teachOut patterns</param>
		public void writePatternsToXml(string fileName, Patterns patterns)
		{
			XmlTextWriter writer = new XmlTextWriter(fileName, null);
			writer.Formatting = Formatting.Indented;
			writer.Indentation = 1;
			writer.IndentChar = '\t';

			writer.WriteStartDocument();

			writer.WriteStartElement(patterns.ToString());
			writer.WriteAttributeString("number_of_patterns", patterns.Length.ToString());

			if(patterns.teachOutputsProp != null)
			{
				writer.WriteStartElement("teach_outputs");
				for(int i=0; i<patterns.teachOutputsProp.Length; i++)
				{
					writer.WriteStartElement("teach_output_nr_" + i.ToString());
					for(int j=0; j<patterns.teachOutputsProp[i].Length; j++)
						writer.WriteAttributeString("t_" + j.ToString(), patterns.teachOutputsProp[i][j].ToString());
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}

			if(patterns.inputsProp != null)
			{
				writer.WriteStartElement("input_patterns");
				for(int i=0; i<patterns.inputsProp.Length; i++)
				{
					writer.WriteStartElement("input_pattern_nr_" + i.ToString());
					for(int j=0; j<patterns.inputsProp[i].Length; j++)
						writer.WriteAttributeString("i_" + j.ToString(), patterns.inputsProp[i][j].ToString());
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();

			writer.Flush();
			writer.Close();
		}

		/// <summary>
		/// Writes a set of patterns to a xml file
		/// </summary>
		/// <param name="fileName">the relative path</param>
		/// <param name="inputs">the input set</param>
		/// <param name="teachOutputs">the corresponding teach output set for the input</param>
		public void writePatternsToXml(string fileName, float[][] inputs, float[][] teachOutputs)
		{
			writePatternsToXml(fileName, new Patterns(inputs, teachOutputs));
		}

		/// <summary>
		/// Reads a set of patterns from a xml file
		/// </summary>
		/// <param name="fileName">the name of the file</param>
		/// <returns>the patterns(input, teachOutput)</returns>
		public Patterns readPatternsFromXml(string fileName)
		{
			if(fileName.Substring(fileName.Length-3).ToUpper() != "PAT")
				throw new ArgumentException("No .pat file!" + "\nYou can only load patterns from XML which were saved with this tool!");

			float[][] input = null, teach = null;
			int patternIndex = 0;
			int nPatterns = 0;

			XmlTextReader reader = new XmlTextReader(fileName);

			while(reader.Read())
			{
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "MultilayerNet.Patterns")
				{
					nPatterns = int.Parse(reader["number_of_patterns"]);
				}
				else if(reader.LineNumber == 2 && reader.MoveToContent() == XmlNodeType.Element
					&& reader.Name != "MultilayerNet.Patterns")
				{
					throw new ArgumentException("XML file has not the required MultilayerNet.Patterns format!"
						+ "\nYou can only load patterns from XML which were saved with this tool!");
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "teach_outputs")
				{
					teach = new float[nPatterns][];
					patternIndex = 0;
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "teach_output_nr_" + patternIndex)
				{
					teach[patternIndex] = new float[reader.AttributeCount];
					for(int j=0; j < reader.AttributeCount; j++)
						teach[patternIndex][j] = float.Parse(reader[j]);
					patternIndex++;
				}

				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "input_patterns")
				{
					input = new float[nPatterns][];
					patternIndex = 0;
				}
				if(reader.MoveToContent() == XmlNodeType.Element && reader.Name == "input_pattern_nr_" + patternIndex)
				{
					input[patternIndex] = new float[reader.AttributeCount];
					for(int j=0; j < reader.AttributeCount; j++)
						input[patternIndex][j] = float.Parse(reader[j]);
					patternIndex++;
				}
			}
			reader.Close();

			return new Patterns(input, teach);
		}

		# endregion

	}
}