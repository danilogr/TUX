using System;
using System.IO;
using bmlTUX.Scripts.ExperimentParts;
using bmlTUX.Scripts.Utilities;
using UnityEngine;

namespace bmlTUX.Scripts.Managers {
	[Serializable]
	public class OutputFile {
		public string OutputFolder;
		public string OutputFileName;
		public string FullPath;

		public OutputFile(string outputFolder, string outputFileName) {
			OutputFolder = outputFolder;
			OutputFileName = outputFileName;
			FullPath = Path.Combine(outputFolder, outputFileName);
			FullPath += ".csv";
		}

		public static OutputFile DebugFile(FileLocationSettings fileLocationSettings) {
			if (fileLocationSettings == null) Debug.Log($"{TuxLog.Prefix} File location settings null");
			return new OutputFile(fileLocationSettings.DebugFolder, fileLocationSettings.DebugFileName);
		}

		public void OutputToFile(Outputtable output) {
			
			Directory.CreateDirectory(OutputFolder ?? throw new NullReferenceException("Folder could not be created"));

			using (StreamWriter streamWriter = new StreamWriter(FullPath)) {
				streamWriter.Write(output.AsString);
			}

			ExperimentEvents.OutputSuccessfullyUpdated(FullPath);

		}
		
		
    
        
	}


}