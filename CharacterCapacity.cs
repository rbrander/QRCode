using System;

namespace QRCode 
{
	public static class Capacity 
	{
		public class Unit {
			public int Version;	// number between 1 and 10
			public string ErrorCorrectionLevel;	// One of: L, M, H, or Q
			public string DataMode;	// One of: Numeric, AlphaNumeric, or byte;
			public int NumBytes;
		}

		public static int GetBytes(int version, string errorCorrectionLevel, string dataMode)
		{
			// todo add validation for inputs
			foreach (Unit u in Units)
				if (u.Version == version && u.ErrorCorrectionLevel == errorCorrectionLevel && u.DataMode == dataMode)
					return u.NumBytes;
			return -1; // return negative if not found
		}
		
		public static int GetVersion(string errCorrLevel, string dataMode, int minBytes)
		{
			for (int version = 1; version <= 10; version++) {
				int numBytes = GetBytes(version, errCorrLevel, dataMode);
				if (numBytes >= minBytes) {
					return version;
				}
			}
			return -1;	// return negative if not found
		}
	
		public static Unit[] Units = new Unit[] {
			// Version 1
			new Unit {Version=1, ErrorCorrectionLevel="L", DataMode="Numeric", NumBytes=41},
			new Unit {Version=1, ErrorCorrectionLevel="M", DataMode="Numeric", NumBytes=34},
			new Unit {Version=1, ErrorCorrectionLevel="Q", DataMode="Numeric", NumBytes=27},
			new Unit {Version=1, ErrorCorrectionLevel="H", DataMode="Numeric", NumBytes=17},
			new Unit {Version=1, ErrorCorrectionLevel="L", DataMode="AlphaNumeric", NumBytes=25},
			new Unit {Version=1, ErrorCorrectionLevel="M", DataMode="AlphaNumeric", NumBytes=20},
			new Unit {Version=1, ErrorCorrectionLevel="Q", DataMode="AlphaNumeric", NumBytes=16},
			new Unit {Version=1, ErrorCorrectionLevel="H", DataMode="AlphaNumeric", NumBytes=10},
			new Unit {Version=1, ErrorCorrectionLevel="L", DataMode="byte", NumBytes=17},
			new Unit {Version=1, ErrorCorrectionLevel="M", DataMode="byte", NumBytes=14},
			new Unit {Version=1, ErrorCorrectionLevel="Q", DataMode="byte", NumBytes=11},
			new Unit {Version=1, ErrorCorrectionLevel="H", DataMode="byte", NumBytes=7},
			new Unit {Version=2, ErrorCorrectionLevel="L", DataMode="Kanji", NumBytes=10},
			new Unit {Version=2, ErrorCorrectionLevel="M", DataMode="Kanji", NumBytes=8},
			new Unit {Version=2, ErrorCorrectionLevel="Q", DataMode="Kanji", NumBytes=7},
			new Unit {Version=2, ErrorCorrectionLevel="H", DataMode="Kanji", NumBytes=4},
			// Version 2
			new Unit {Version=2, ErrorCorrectionLevel="L", DataMode="Numeric", NumBytes=77},
			new Unit {Version=2, ErrorCorrectionLevel="M", DataMode="Numeric", NumBytes=63},
			new Unit {Version=2, ErrorCorrectionLevel="Q", DataMode="Numeric", NumBytes=48},
			new Unit {Version=2, ErrorCorrectionLevel="H", DataMode="Numeric", NumBytes=34},
			new Unit {Version=2, ErrorCorrectionLevel="L", DataMode="AlphaNumeric", NumBytes=47},
			new Unit {Version=2, ErrorCorrectionLevel="M", DataMode="AlphaNumeric", NumBytes=38},
			new Unit {Version=2, ErrorCorrectionLevel="Q", DataMode="AlphaNumeric", NumBytes=29},
			new Unit {Version=2, ErrorCorrectionLevel="H", DataMode="AlphaNumeric", NumBytes=20},
			new Unit {Version=2, ErrorCorrectionLevel="L", DataMode="byte", NumBytes=32},
			new Unit {Version=2, ErrorCorrectionLevel="M", DataMode="byte", NumBytes=26},
			new Unit {Version=2, ErrorCorrectionLevel="Q", DataMode="byte", NumBytes=20},
			new Unit {Version=2, ErrorCorrectionLevel="H", DataMode="byte", NumBytes=14},
			new Unit {Version=2, ErrorCorrectionLevel="L", DataMode="Kanji", NumBytes=20},
			new Unit {Version=2, ErrorCorrectionLevel="M", DataMode="Kanji", NumBytes=16},
			new Unit {Version=2, ErrorCorrectionLevel="Q", DataMode="Kanji", NumBytes=12},
			new Unit {Version=2, ErrorCorrectionLevel="H", DataMode="Kanji", NumBytes=8},
			// Version 3
			new Unit {Version=3, ErrorCorrectionLevel="L", DataMode="Numeric", NumBytes=127},
			new Unit {Version=3, ErrorCorrectionLevel="M", DataMode="Numeric", NumBytes=101},
			new Unit {Version=3, ErrorCorrectionLevel="Q", DataMode="Numeric", NumBytes=77},
			new Unit {Version=3, ErrorCorrectionLevel="H", DataMode="Numeric", NumBytes=58},
			new Unit {Version=3, ErrorCorrectionLevel="L", DataMode="AlphaNumeric", NumBytes=77},
			new Unit {Version=3, ErrorCorrectionLevel="M", DataMode="AlphaNumeric", NumBytes=61},
			new Unit {Version=3, ErrorCorrectionLevel="Q", DataMode="AlphaNumeric", NumBytes=47},
			new Unit {Version=3, ErrorCorrectionLevel="H", DataMode="AlphaNumeric", NumBytes=35},
			new Unit {Version=3, ErrorCorrectionLevel="L", DataMode="byte", NumBytes=53},
			new Unit {Version=3, ErrorCorrectionLevel="M", DataMode="byte", NumBytes=42},
			new Unit {Version=3, ErrorCorrectionLevel="Q", DataMode="byte", NumBytes=32},
			new Unit {Version=3, ErrorCorrectionLevel="H", DataMode="byte", NumBytes=24},
			new Unit {Version=3, ErrorCorrectionLevel="L", DataMode="Kanji", NumBytes=32},
			new Unit {Version=3, ErrorCorrectionLevel="M", DataMode="Kanji", NumBytes=26},
			new Unit {Version=3, ErrorCorrectionLevel="Q", DataMode="Kanji", NumBytes=20},
			new Unit {Version=3, ErrorCorrectionLevel="H", DataMode="Kanji", NumBytes=15},
			// Version 4
			new Unit {Version=4, ErrorCorrectionLevel="L", DataMode="Numeric", NumBytes=187},
			new Unit {Version=4, ErrorCorrectionLevel="M", DataMode="Numeric", NumBytes=149},
			new Unit {Version=4, ErrorCorrectionLevel="Q", DataMode="Numeric", NumBytes=111},
			new Unit {Version=4, ErrorCorrectionLevel="H", DataMode="Numeric", NumBytes=82},
			new Unit {Version=4, ErrorCorrectionLevel="L", DataMode="AlphaNumeric", NumBytes=114},
			new Unit {Version=4, ErrorCorrectionLevel="M", DataMode="AlphaNumeric", NumBytes=90},
			new Unit {Version=4, ErrorCorrectionLevel="Q", DataMode="AlphaNumeric", NumBytes=67},
			new Unit {Version=4, ErrorCorrectionLevel="H", DataMode="AlphaNumeric", NumBytes=50},
			new Unit {Version=4, ErrorCorrectionLevel="L", DataMode="byte", NumBytes=78},
			new Unit {Version=4, ErrorCorrectionLevel="M", DataMode="byte", NumBytes=62},
			new Unit {Version=4, ErrorCorrectionLevel="Q", DataMode="byte", NumBytes=46},
			new Unit {Version=4, ErrorCorrectionLevel="H", DataMode="byte", NumBytes=34},
			new Unit {Version=4, ErrorCorrectionLevel="L", DataMode="Kanji", NumBytes=48},
			new Unit {Version=4, ErrorCorrectionLevel="M", DataMode="Kanji", NumBytes=38},
			new Unit {Version=4, ErrorCorrectionLevel="Q", DataMode="Kanji", NumBytes=28},
			new Unit {Version=4, ErrorCorrectionLevel="H", DataMode="Kanji", NumBytes=21},
			// Version 5
			new Unit {Version=5, ErrorCorrectionLevel="L", DataMode="Numeric", NumBytes=255},
			new Unit {Version=5, ErrorCorrectionLevel="M", DataMode="Numeric", NumBytes=202},
			new Unit {Version=5, ErrorCorrectionLevel="Q", DataMode="Numeric", NumBytes=144},
			new Unit {Version=5, ErrorCorrectionLevel="H", DataMode="Numeric", NumBytes=106},
			new Unit {Version=5, ErrorCorrectionLevel="L", DataMode="AlphaNumeric", NumBytes=154},
			new Unit {Version=5, ErrorCorrectionLevel="M", DataMode="AlphaNumeric", NumBytes=122},
			new Unit {Version=5, ErrorCorrectionLevel="Q", DataMode="AlphaNumeric", NumBytes=87},
			new Unit {Version=5, ErrorCorrectionLevel="H", DataMode="AlphaNumeric", NumBytes=64},
			new Unit {Version=5, ErrorCorrectionLevel="L", DataMode="byte", NumBytes=106},
			new Unit {Version=5, ErrorCorrectionLevel="M", DataMode="byte", NumBytes=84},
			new Unit {Version=5, ErrorCorrectionLevel="Q", DataMode="byte", NumBytes=60},
			new Unit {Version=5, ErrorCorrectionLevel="H", DataMode="byte", NumBytes=44},
			new Unit {Version=5, ErrorCorrectionLevel="L", DataMode="Kanji", NumBytes=65},
			new Unit {Version=5, ErrorCorrectionLevel="M", DataMode="Kanji", NumBytes=52},
			new Unit {Version=5, ErrorCorrectionLevel="Q", DataMode="Kanji", NumBytes=37},
			new Unit {Version=5, ErrorCorrectionLevel="H", DataMode="Kanji", NumBytes=27},
			// Version 6
			new Unit {Version=6, ErrorCorrectionLevel="L", DataMode="Numeric", NumBytes=322},
			new Unit {Version=6, ErrorCorrectionLevel="M", DataMode="Numeric", NumBytes=255},
			new Unit {Version=6, ErrorCorrectionLevel="Q", DataMode="Numeric", NumBytes=178},
			new Unit {Version=6, ErrorCorrectionLevel="H", DataMode="Numeric", NumBytes=139},
			new Unit {Version=6, ErrorCorrectionLevel="L", DataMode="AlphaNumeric", NumBytes=195},
			new Unit {Version=6, ErrorCorrectionLevel="M", DataMode="AlphaNumeric", NumBytes=154},
			new Unit {Version=6, ErrorCorrectionLevel="Q", DataMode="AlphaNumeric", NumBytes=108},
			new Unit {Version=6, ErrorCorrectionLevel="H", DataMode="AlphaNumeric", NumBytes=84},
			new Unit {Version=6, ErrorCorrectionLevel="L", DataMode="byte", NumBytes=134},
			new Unit {Version=6, ErrorCorrectionLevel="M", DataMode="byte", NumBytes=106},
			new Unit {Version=6, ErrorCorrectionLevel="Q", DataMode="byte", NumBytes=74},
			new Unit {Version=6, ErrorCorrectionLevel="H", DataMode="byte", NumBytes=58},
			new Unit {Version=6, ErrorCorrectionLevel="L", DataMode="Kanji", NumBytes=82},
			new Unit {Version=6, ErrorCorrectionLevel="M", DataMode="Kanji", NumBytes=65},
			new Unit {Version=6, ErrorCorrectionLevel="Q", DataMode="Kanji", NumBytes=45},
			new Unit {Version=6, ErrorCorrectionLevel="H", DataMode="Kanji", NumBytes=36},
			// Version 7
			new Unit {Version=7, ErrorCorrectionLevel="L", DataMode="Numeric", NumBytes=370},
			new Unit {Version=7, ErrorCorrectionLevel="M", DataMode="Numeric", NumBytes=293},
			new Unit {Version=7, ErrorCorrectionLevel="Q", DataMode="Numeric", NumBytes=207},
			new Unit {Version=7, ErrorCorrectionLevel="H", DataMode="Numeric", NumBytes=154},
			new Unit {Version=7, ErrorCorrectionLevel="L", DataMode="AlphaNumeric", NumBytes=224},
			new Unit {Version=7, ErrorCorrectionLevel="M", DataMode="AlphaNumeric", NumBytes=178},
			new Unit {Version=7, ErrorCorrectionLevel="Q", DataMode="AlphaNumeric", NumBytes=125},
			new Unit {Version=7, ErrorCorrectionLevel="H", DataMode="AlphaNumeric", NumBytes=93},
			new Unit {Version=7, ErrorCorrectionLevel="L", DataMode="byte", NumBytes=154},
			new Unit {Version=7, ErrorCorrectionLevel="M", DataMode="byte", NumBytes=122},
			new Unit {Version=7, ErrorCorrectionLevel="Q", DataMode="byte", NumBytes=86},
			new Unit {Version=7, ErrorCorrectionLevel="H", DataMode="byte", NumBytes=64},
			new Unit {Version=7, ErrorCorrectionLevel="L", DataMode="Kanji", NumBytes=95},
			new Unit {Version=7, ErrorCorrectionLevel="M", DataMode="Kanji", NumBytes=75},
			new Unit {Version=7, ErrorCorrectionLevel="Q", DataMode="Kanji", NumBytes=53},
			new Unit {Version=7, ErrorCorrectionLevel="H", DataMode="Kanji", NumBytes=39},
			// Version 8
			new Unit {Version=8, ErrorCorrectionLevel="L", DataMode="Numeric", NumBytes=461},
			new Unit {Version=8, ErrorCorrectionLevel="M", DataMode="Numeric", NumBytes=365},
			new Unit {Version=8, ErrorCorrectionLevel="Q", DataMode="Numeric", NumBytes=259},
			new Unit {Version=8, ErrorCorrectionLevel="H", DataMode="Numeric", NumBytes=202},
			new Unit {Version=8, ErrorCorrectionLevel="L", DataMode="AlphaNumeric", NumBytes=279},
			new Unit {Version=8, ErrorCorrectionLevel="M", DataMode="AlphaNumeric", NumBytes=221},
			new Unit {Version=8, ErrorCorrectionLevel="Q", DataMode="AlphaNumeric", NumBytes=157},
			new Unit {Version=8, ErrorCorrectionLevel="H", DataMode="AlphaNumeric", NumBytes=122},
			new Unit {Version=8, ErrorCorrectionLevel="L", DataMode="byte", NumBytes=192},
			new Unit {Version=8, ErrorCorrectionLevel="M", DataMode="byte", NumBytes=152},
			new Unit {Version=8, ErrorCorrectionLevel="Q", DataMode="byte", NumBytes=108},
			new Unit {Version=8, ErrorCorrectionLevel="H", DataMode="byte", NumBytes=84},
			new Unit {Version=8, ErrorCorrectionLevel="L", DataMode="Kanji", NumBytes=118},
			new Unit {Version=8, ErrorCorrectionLevel="M", DataMode="Kanji", NumBytes=93},
			new Unit {Version=8, ErrorCorrectionLevel="Q", DataMode="Kanji", NumBytes=66},
			new Unit {Version=8, ErrorCorrectionLevel="H", DataMode="Kanji", NumBytes=52},
			// Version 9
			new Unit {Version=9, ErrorCorrectionLevel="L", DataMode="Numeric", NumBytes=552},
			new Unit {Version=9, ErrorCorrectionLevel="M", DataMode="Numeric", NumBytes=432},
			new Unit {Version=9, ErrorCorrectionLevel="Q", DataMode="Numeric", NumBytes=312},
			new Unit {Version=9, ErrorCorrectionLevel="H", DataMode="Numeric", NumBytes=235},
			new Unit {Version=9, ErrorCorrectionLevel="L", DataMode="AlphaNumeric", NumBytes=335},
			new Unit {Version=9, ErrorCorrectionLevel="M", DataMode="AlphaNumeric", NumBytes=262},
			new Unit {Version=9, ErrorCorrectionLevel="Q", DataMode="AlphaNumeric", NumBytes=189},
			new Unit {Version=9, ErrorCorrectionLevel="H", DataMode="AlphaNumeric", NumBytes=143},
			new Unit {Version=9, ErrorCorrectionLevel="L", DataMode="byte", NumBytes=230},
			new Unit {Version=9, ErrorCorrectionLevel="M", DataMode="byte", NumBytes=180},
			new Unit {Version=9, ErrorCorrectionLevel="Q", DataMode="byte", NumBytes=130},
			new Unit {Version=9, ErrorCorrectionLevel="H", DataMode="byte", NumBytes=98},
			new Unit {Version=9, ErrorCorrectionLevel="L", DataMode="Kanji", NumBytes=141},
			new Unit {Version=9, ErrorCorrectionLevel="M", DataMode="Kanji", NumBytes=111},
			new Unit {Version=9, ErrorCorrectionLevel="Q", DataMode="Kanji", NumBytes=80},
			new Unit {Version=9, ErrorCorrectionLevel="H", DataMode="Kanji", NumBytes=60},
			// Version 10
			new Unit {Version=10, ErrorCorrectionLevel="L", DataMode="Numeric", NumBytes=652},
			new Unit {Version=10, ErrorCorrectionLevel="M", DataMode="Numeric", NumBytes=513},
			new Unit {Version=10, ErrorCorrectionLevel="Q", DataMode="Numeric", NumBytes=364},
			new Unit {Version=10, ErrorCorrectionLevel="H", DataMode="Numeric", NumBytes=288},
			new Unit {Version=10, ErrorCorrectionLevel="L", DataMode="AlphaNumeric", NumBytes=395},
			new Unit {Version=10, ErrorCorrectionLevel="M", DataMode="AlphaNumeric", NumBytes=311},
			new Unit {Version=10, ErrorCorrectionLevel="Q", DataMode="AlphaNumeric", NumBytes=221},
			new Unit {Version=10, ErrorCorrectionLevel="H", DataMode="AlphaNumeric", NumBytes=174},
			new Unit {Version=10, ErrorCorrectionLevel="L", DataMode="byte", NumBytes=271},
			new Unit {Version=10, ErrorCorrectionLevel="M", DataMode="byte", NumBytes=213},
			new Unit {Version=10, ErrorCorrectionLevel="Q", DataMode="byte", NumBytes=151},
			new Unit {Version=10, ErrorCorrectionLevel="H", DataMode="byte", NumBytes=119},
			new Unit {Version=10, ErrorCorrectionLevel="L", DataMode="Kanji", NumBytes=167},
			new Unit {Version=10, ErrorCorrectionLevel="M", DataMode="Kanji", NumBytes=131},
			new Unit {Version=10, ErrorCorrectionLevel="Q", DataMode="Kanji", NumBytes=93},
			new Unit {Version=10, ErrorCorrectionLevel="H", DataMode="Kanji", NumBytes=74}
		};
	}
}