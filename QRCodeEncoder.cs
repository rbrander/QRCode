/*
QRCode message encoder: takes in a message (string) and returns a bitmap

*/
using System;
using System.Text.RegularExpressions;

/*
Input: Message (string)
Validation: length, character set
Output: object: {width, data: 2darray}

*/

namespace QRCode {
	public class QRCodeEncoder {
		public static readonly int MAX_VERSION_SUPPORTED = 10; // QRCode limit is 40
	
		public static void Main(string[] args)
		{
			Console.WriteLine(" QRCode Encoder ");
			Console.WriteLine("~~~~~~~~~~~~~~~~\n");
			
			// GetMessageAndErrorCorrectionLevel();
			//string numericMessage = "1234567";
			//string alphaNumericMessage = "a1234567";
			string byteMessage = "#####~~~~~#####~~~~~";
			QRCodeEncoder.Message = byteMessage;
			QRCodeEncoder.ErrorCorrectionLevel = "H";
			
			EncodeData();
			
			GenerateErrorCorrectionCode();
			
			StructureFinalMessage();
			
			BuildBitMap();
		}
		
		private static void BuildBitMap()
		{
			int?[,] matrix = BitmapGenerator.CreateNew(QRCodeEncoder.Version);
			BitmapGenerator.Draw(matrix);
		}
		
		private static string FinalMessage;
		private static void StructureFinalMessage()
		{
			// Check if interleaving is needed (need more than 1 block to interleave)
			var info = ErrorCorrectionCodeWordBlockInfo.Lookup(QRCodeEncoder.Version, QRCodeEncoder.ErrorCorrectionLevel);
			bool needsInterleaving = (info.NumGroup2Blocks > 0);
		
			if (needsInterleaving) {
				// TODO:Interleave Data CodeWords
				string InterleavedDataCodewords = QRCodeEncoder.EncodedData;
				// TODO:Interleave Error Correction CodeWords
				string InterleavedErrorCorrectionCodeWords = QRCodeEncoder.ErrorCorrectionCodeWordData;
				QRCodeEncoder.FinalMessage = InterleavedDataCodewords + InterleavedErrorCorrectionCodeWords;
			} else
				QRCodeEncoder.FinalMessage = QRCodeEncoder.EncodedData + QRCodeEncoder.ErrorCorrectionCodeWordData;
			
			// add remainder its if necessary
			int numRemainderBits = Data.RequiredRemainderBits[QRCodeEncoder.Version];
			if (numRemainderBits > 0) {
				while (numRemainderBits-- > 0)
					QRCodeEncoder.FinalMessage += "0";
			}
		}
		
		private static string ErrorCorrectionCodeWordData;
		private static void GenerateErrorCorrectionCode()
		{
			// TODO
			QRCodeEncoder.ErrorCorrectionCodeWordData = string.Empty;
		}
		
		private static int Version;
		private static string DataMode;
		private static string ModeIndicator;
		private static string CharacterCountIndicator;
		private static string EncodedData;
		private static void EncodeData()
		{
			// Determine the data mode from the message character set
			QRCodeEncoder.DataMode = GetDataMode(QRCodeEncoder.Message);
			Console.WriteLine("Data Mode: " + QRCodeEncoder.DataMode);
			
			// Ensure the message is all uppercase if alphanumeric
			if (QRCodeEncoder.DataMode == "AlphaNumeric")
				QRCodeEncoder.Message = QRCodeEncoder.Message.ToUpper();
			
			// Get the mode indicator
			QRCodeEncoder.ModeIndicator = QRCodeEncoder.GetModeIndicator(QRCodeEncoder.DataMode);
			Console.WriteLine("ModeIndicator: " + QRCodeEncoder.ModeIndicator);
			
			// Use lookup table to determine version
			QRCodeEncoder.Version = Capacity.GetVersion(QRCodeEncoder.ErrorCorrectionLevel, QRCodeEncoder.DataMode, QRCodeEncoder.Message.Length);
			Console.WriteLine("Version: " + QRCodeEncoder.Version);
			
			// Get the char count indicator
			QRCodeEncoder.CharacterCountIndicator = QRCodeEncoder.GetCharacterCountIndicator(
				QRCodeEncoder.Version, QRCodeEncoder.Message.Length, QRCodeEncoder.DataMode);
			Console.WriteLine("CharacterCountIndicator: " + QRCodeEncoder.CharacterCountIndicator);
			
			// Encode the message based on data mode
			if (QRCodeEncoder.DataMode == "Numeric")
				QRCodeEncoder.EncodedData = QRCodeEncoder.EncodeNumeric(QRCodeEncoder.Message);
			else if (QRCodeEncoder.DataMode == "AlphaNumeric")
				QRCodeEncoder.EncodedData = QRCodeEncoder.EncodeAlphaNumeric(QRCodeEncoder.Message);
			else if (QRCodeEncoder.DataMode == "byte")
				QRCodeEncoder.EncodedData = QRCodeEncoder.EncodeBytes(QRCodeEncoder.Message);
			else if (QRCodeEncoder.DataMode == "Kanji" || QRCodeEncoder.DataMode == "ECI")
				throw new Exception("Unsupported data mode!");
			else
				throw new Exception("Unknown data mode!");
			Console.WriteLine("Encoded: " + QRCodeEncoder.EncodedData);
		}
		
		private static string GetDataMode(string msg)
		{
			string dataMode;
			
			if (ContainsNumericOnly(msg))
				dataMode = "Numeric";
			else if (ContainsAlphaNumericOnly(msg))
				dataMode = "AlphaNumeric";
			else
				dataMode = "byte";
			
			return dataMode;
		}
		
		public static string EncodeNumeric(string msg)
		{
			string encoded = string.Empty;
			
			if (string.IsNullOrWhiteSpace(msg)) return string.Empty;
			if (!ContainsNumericOnly(msg)) throw new Exception("Message is not numeric!");
			
			// break up into chunks of 3 numbers, convert number to binary, concat
			int numChunks = (int)Math.Ceiling(msg.Length / 3m);
			int remainderChunk = msg.Length % 3;
			for (int chunk = 0; chunk < numChunks; chunk++) {
				bool isLastChunk = ((chunk == numChunks - 1) && remainderChunk > 0);
				string msgChunk = msg.Substring(chunk * 3, (isLastChunk ? remainderChunk : 3));
				encoded += Convert.ToString(int.Parse(msgChunk), 2);
			}
			
			return encoded;
		}
		
		public static string EncodeAlphaNumeric(string msg)
		{
			string encoded = string.Empty;
			
			if (string.IsNullOrWhiteSpace(msg)) return string.Empty;
			if (!ContainsAlphaNumericOnly(msg)) throw new Exception("Message is not alphanumeric!");
			
			// break up into chunks of 2 chars, convert number to decimal, concat (num1 * 45) + num2
			int numChunks = (int)Math.Ceiling(msg.Length / 2m);
			int remainderChunk = msg.Length % 2;
			for (int chunk = 0; chunk < numChunks; chunk++) {
				bool isLastChunk = ((chunk == numChunks - 1) && remainderChunk > 0);
				string msgChunk = msg.Substring(chunk * 2, (isLastChunk ? remainderChunk : 2));
				
				int chunkValue = 0;
				if (msgChunk.Length == 2) {
					int firstCharValue = QRCodeEncoder.GetAlphaNumericValue(msgChunk[0]);
					int secondCharValue = QRCodeEncoder.GetAlphaNumericValue(msgChunk[1]);
					chunkValue = (firstCharValue * 45) + secondCharValue;
					// convert to 11 bit binary string
					string bitString = Convert.ToString(chunkValue, 2);
					if (bitString.Length < 11)
						while (bitString.Length < 11)
							bitString = "0" + bitString;
					encoded += bitString;
				} else if (msgChunk.Length == 1) {
					chunkValue = QRCodeEncoder.GetAlphaNumericValue(msgChunk[0]);
					// convert to 6 bit binary string
					string bitString = Convert.ToString(chunkValue, 2);
					if (bitString.Length < 6)
						while (bitString.Length < 6)
							bitString = "0" + bitString;
					encoded += bitString;
				}
			}
			
			return encoded;
		}
		
		private static string EncodeBytes(string msg)
		{
			string encoded = string.Empty;
			foreach (char c in msg) {
				string bitString = Convert.ToString((int)c, 2);
				if (bitString.Length < 8)
					while (bitString.Length < 8)
						bitString = "0" + bitString;
				encoded += bitString;
			}
			return encoded;
		}
		
		private static string GetModeIndicator(string dataMode)
		{
			switch (dataMode) {
				case "Numeric": return "0001";
				case "AlphaNumeric": return "0010";
				case "byte": return "0100";
				case "Kanji": return "1000";
				case "ECI": return "0111";
				default: throw new Exception("Invalid dataMode \""+dataMode+"\"");
			}
		}
		
		private static bool ContainsNumericOnly(string msg)
		{
			return Regex.IsMatch(msg, "^[0-9]*$");
		}
		
		private static bool ContainsAlphaNumericOnly(string msg)
		{
			return Regex.IsMatch(msg, @"^[0-9A-Za-z\ \$\%\*\+\-\.\/\:]*$");
		}
		
		public static int GetAlphaNumericValue(char c)
		{
			switch (c) {
				case '0': return 0;
				case '1': return 1;
				case '2': return 2;
				case '3': return 3;
				case '4': return 4;
				case '5': return 5;
				case '6': return 6;
				case '7': return 7;
				case '8': return 8;
				case '9': return 9;
				case 'A': return 10;
				case 'B': return 11;
				case 'C': return 12;
				case 'D': return 13;
				case 'E': return 14;
				case 'F': return 15;
				case 'G': return 16;
				case 'H': return 17;
				case 'I': return 18;
				case 'J': return 19;
				case 'K': return 20;
				case 'L': return 21;
				case 'M': return 22;
				case 'N': return 23;
				case 'O': return 24;
				case 'P': return 25;
				case 'Q': return 26;
				case 'R': return 27;
				case 'S': return 28;
				case 'T': return 29;
				case 'U': return 30;
				case 'V': return 31;
				case 'W': return 32;
				case 'X': return 33;
				case 'Y': return 34;
				case 'Z': return 35;
				case ' ': return 36;
				case '$': return 37;
				case '%': return 38;
				case '*': return 39;
				case '+': return 40;
				case '-': return 41;
				case '.': return 42;
				case '/': return 43;
				case ':': return 44;
				default: throw new Exception("Invalid AlphaNumeric character '"+c+"'");
			}
		}
		
		public static string GetCharacterCountIndicator(int version, int msgLength, string dataMode)
		{
			if (version < 1 || version > MAX_VERSION_SUPPORTED)
				throw new Exception("Invalid version! Must be between 1 and " + MAX_VERSION_SUPPORTED);
		
			string bitStr = Convert.ToString(msgLength, 2);
			
			int numBits = bitStr.Length;
			// Set the bit string requirement based on QR Code rules
			if (version >= 1 && version <= 9) {	
				switch (dataMode) {
					case "Numeric": numBits = 10; break;
					case "AlphaNumeric": numBits = 9; break;
					case "byte": numBits = 8; break;
					case "Kanji": case "ECI": numBits = 8; break;
				}
			} else if (version >= 10 && version <= 26) {
				switch (dataMode) {
					case "Numeric": numBits = 12; break;
					case "AlphaNumeric": numBits = 11; break;
					case "byte": numBits = 16; break;
					case "Kanji": case "ECI": numBits = 10; break;
				}
			} else if (version >= 27 && version <= 40) {
				switch (dataMode) {
					case "Numeric": numBits = 14; break;
					case "AlphaNumeric": numBits = 13; break;
					case "byte": numBits = 16; break;
					case "Kanji": case "ECI": numBits = 12; break;
				}
			}
			
			// add padding if needed
			if (numBits > bitStr.Length)
				while (bitStr.Length < numBits)
					bitStr = "0" + bitStr;	// pad left with zeros
			
			return bitStr;
		}
		
		private static void GetMessageAndErrorCorrectionLevel()
		{
			QRCodeEncoder.Message = GetInputMessage();
			Console.WriteLine("Message: \"" + QRCodeEncoder.Message + "\"");
			Console.WriteLine("Length: "+QRCodeEncoder.Message.Length);
			
			QRCodeEncoder.ErrorCorrectionLevel = GetErrorCorrectionLevel();
			Console.WriteLine("Error Correction Level: " + QRCodeEncoder.ErrorCorrectionLevel);
		}
		
		private static string ErrorCorrectionLevel;
		private static string GetErrorCorrectionLevel()
		{
			string errorCorrectionLevel;
			do {
				Console.WriteLine("\nError Correction Levels");
				Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
				Console.WriteLine("L - Low     ( 7%)");
				Console.WriteLine("M - Med     (15%)");
				Console.WriteLine("H - High    (30%)");
				Console.WriteLine("Q - Quarter (25%)");
				Console.Write("\nSelect an Error Correction Level: ");
				errorCorrectionLevel = Console.ReadLine().ToUpper();
			} while (!isValidErrorCorrectionLevel(errorCorrectionLevel));
			return errorCorrectionLevel;
		}
		
		private static bool isValidErrorCorrectionLevel(string errorCorrectionLevel)
		{
			if (string.IsNullOrWhiteSpace(errorCorrectionLevel) || errorCorrectionLevel.Length != 1) {
				Console.WriteLine("Length must be 1");
				return false;
			}
			switch (errorCorrectionLevel.ToUpper()) {
				case "L":
				case "M":
				case "H":
				case "Q":
					break;
				default:
					Console.WriteLine("Must be one of (L, M, H, or Q)");
					return false;
			}
			return true;
		}
		
		
		private static string Message;
		private static readonly int MAX_MESSAGE_LENGTH = 254;
		private static string GetInputMessage()
		{
			string msg;
			do {
				Console.Write("\nEnter Message to Encode: ");
				msg = Console.ReadLine();
			} while(!isValidMessage(msg));
			return msg;
		}
		
		private static bool isValidMessage(string msg)
		{
			if (String.IsNullOrWhiteSpace(msg) || (msg.Length > MAX_MESSAGE_LENGTH)) {
				Console.WriteLine("Invalid message (must be betwen 1 and " + MAX_MESSAGE_LENGTH + " chars)");
				return false;
			}
			return true;
		}
	}
}