using System;

namespace QRCode 
{
	public static class ErrorCorrectionCodeWordBlockInfo
	{
		public class Info 
		{
			// Lookup fields
			public int Version;
			public string ECLevel;	// Error Correction Level
			// Data fields
			public int NumECCodeWords;
			public int NumGroup1Blocks;
			public int NumGroup1DataCodeWords;
			public int NumGroup2Blocks;
			public int NumGroup2DataCodeWords;
			// Calculated fields
			public int NumTotalDataCodeWords { get { return NumECCodeWords + NumDataCodeWords; } }
			public int NumDataCodeWords { get { return (NumGroup1Blocks * NumGroup1DataCodeWords) + (NumGroup2Blocks * NumGroup2DataCodeWords); } }
			
			public Info(int version, string ecLevel, int numECCodeWords, int numGroup1Blocks, 
				int numGroup1DataCodeWords, int numGroup2Blocks, int numGroup2DataCodeWords)
			{
				this.Version = version;
				this.ECLevel = ecLevel;
				this.NumECCodeWords = numECCodeWords;
				this.NumGroup1Blocks = numGroup1Blocks;
				this.NumGroup1DataCodeWords = numGroup1DataCodeWords;
				this.NumGroup2Blocks = numGroup2Blocks;
				this.NumGroup2DataCodeWords = numGroup2DataCodeWords;
			}
		}
		
		public static Info Lookup(int version, string ecLevel)
		{
			foreach (Info i in Infos)
				if (i.Version == version && i.ECLevel == ecLevel)
					return i;
			throw new Exception("Unable to find data!");
		}
		
		public static Info[] Infos = new Info[] {
			// Info(Version, ECLevel, NumECCodeWords, NumGroup1Blocks, NumGroup1DataCodeWords, NumGroup2Blocks, NumGroup2DataCodeWords),
			new Info(1, "L", 7, 1, 19, 0, 0),
			new Info(1, "M", 10, 1, 16, 0, 0),
			new Info(1, "Q", 13, 1, 13, 0, 0),
			new Info(1, "H", 17, 1, 9, 0, 0),
			new Info(2, "L", 10, 1, 34, 0, 0),
			new Info(2, "M", 16, 1, 28, 0, 0),
			new Info(2, "Q", 22, 1, 22, 0, 0),
			new Info(2, "H", 28, 1, 16, 0, 0),
			new Info(3, "L", 15, 1, 55, 0, 0),
			new Info(3, "M", 26, 1, 44, 0, 0),
			new Info(3, "Q", 18, 2, 17, 0, 0),
			new Info(3, "H", 22, 2, 13, 0, 0),
			new Info(4, "L", 20, 1, 80, 0, 0),
			new Info(4, "M", 18, 2, 32, 0, 0),
			new Info(4, "Q", 26, 2, 24, 0, 0),
			new Info(4, "H", 16, 4, 9, 0, 0),
			new Info(5, "L", 26, 1, 108, 0, 0),
			new Info(5, "M", 24, 2, 43, 0, 0),
			new Info(5, "Q", 18, 2, 15, 2, 16),
			new Info(5, "H", 22, 2, 11, 2, 12),
			new Info(6, "L", 18, 2, 68, 0, 0),
			new Info(6, "M", 16, 4, 27, 0, 0),
			new Info(6, "Q", 24, 4, 19, 0, 0),
			new Info(6, "H", 28, 4, 15, 0, 0),
			new Info(7, "L", 20, 2, 78, 0, 0),
			new Info(7, "M", 18, 4, 31, 0, 0),
			new Info(7, "Q", 18, 2, 14, 4, 15),
			new Info(7, "H", 26, 4, 13, 1, 14),
			new Info(8, "L", 24, 2, 97, 0, 0),
			new Info(8, "M", 22, 2, 38, 2, 39),
			new Info(8, "Q", 22, 4, 18, 2, 19),
			new Info(8, "H", 26, 4, 14, 2, 15),
			new Info(9, "L", 30, 2, 116, 0, 0),
			new Info(9, "M", 22, 3, 36, 2, 37),
			new Info(9, "Q", 20, 4, 16, 4, 17),
			new Info(9, "H", 24, 4, 12, 4, 13),
			new Info(10, "L", 18, 2, 68, 2, 69),
			new Info(10, "M", 26, 4, 43, 1, 44),
			new Info(10, "Q", 24, 6, 19, 2, 20),
			new Info(10, "H", 28, 6, 15, 2, 16)
			// TODO: fill in up to max version
		};
	}
}