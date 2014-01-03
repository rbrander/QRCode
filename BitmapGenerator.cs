using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace QRCode 
{
	public static class BitmapGenerator
	{
		public static int GetSize(int Version)
		{
			return (((Version-1)*4)+21);
		}
		
		public static int?[,] CreateNew(int Version)
		{
			int size = GetSize(Version);
			int?[,] matrix = new int?[size, size];
			AddFinderPatterns(size, matrix);
			AddAlignmentPatterns(Version, matrix);
			AddTimingPatterns(size, matrix);
			AddDarkModule(size, matrix);
			AddReservedFormatInformationArea(Version, size, matrix);
			return matrix;
		}
		
		public static void SavePNG(string filename, int size, int?[,] matrix)
		{
			int multiplier = 2;
			Bitmap bmp = new Bitmap(size*multiplier, size*multiplier);
		
			for (int row = 0; row < size; row++) {
				for (int col = 0; col < size; col++) {
					Color color = (matrix[col, row] == 1) ? Color.Black : Color.White;
					for (int moduleX = 0; moduleX < multiplier; moduleX++)
						for (int moduleY = 0; moduleY < multiplier; moduleY++)
							bmp.SetPixel(col+moduleX, row+moduleY, color);
				}
			}
			
			bmp.Save(filename);
		}
		
		public static void AddDataBits(int size, int?[,] matrix, string dataBits)
		{
			// start in bottom right corner and go up then over two and down
			// when in upward movment, place bits in zig-zag from
			// bottom right to bottom left then up one and right and then left
			// the only exception is when on x = vertical timing row, skip it
			
			// zig-zag loop
			int currCharIndex = 0;
			for (int x = size-1; x > 0; x-=(x == 8 ? 3 : 2)) {
				bool directionUp = ((((size-1) - x) / 2) % 2) == 0;
				for (int y = (directionUp ? (size-1) : 0); directionUp ? y >= 0 : y < size; y+=(directionUp ? -1 : 1)) {
					for (int offset = 0; offset < 2; offset++) 
						if (matrix[x-offset, y] == null)
							matrix[x-offset, y] = int.Parse(dataBits[currCharIndex++].ToString());
				}
			}
		}
		
		public static void Draw(int?[,] matrix)
		{
			int size = (int)Math.Sqrt(matrix.Length);
			Console.WriteLine("\nMatrix");
			Console.WriteLine("~~~~~~");
			Console.WriteLine("Size: " + size);
			Console.WriteLine();
			
			for (var y = 0; y < size; y++) {
				for (var x = 0; x < size; x++) {
					char moduleChar = ' ';
					if (matrix[x,y] != null)
						switch (matrix[x,y]) {
							case 0:
								moduleChar = 'o';
								break;
							case 1:
								moduleChar = 'x';
								break;
							case -1:
								moduleChar = '~';
								break;
							default:
								break;
						}
					
					Console.Write(moduleChar);
				}
				Console.WriteLine();
			}
		}
		
		public static void AddDarkModule(int size, int?[,] matrix)
		{
			matrix[8, size-8] = 0;
		}
		
		public static void AddReservedFormatInformationArea(int version, int size, int?[,] matrix)
		{
			// reserved area will be marked by -1
			// Format Information Area is a set for four strips
			
			
			for (int y = 0; y < 9; y++) {
				matrix[8, size-1-Math.Min(y, 7)] = matrix[8, size-1-Math.Min(y, 7)] ?? -1;	// bottom left, small vertical strip touching Dark Module
				matrix[8, y] = matrix[8, y] ?? -1;	// top left, vertical strip 
			}
			
			for (int x = 0; x < 8; x++) {
				matrix[size-1-x, 8] = -1;	// top right, horizontal strip
				if (matrix[x, 8] == null)
					matrix[x, 8] = -1;	// top left, horizontal strip
			}
			
			// version >= 7 have addtional Version Information Area
			if (version >= 7) {
				// add a 3x6 box on both opposing finder corners
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 6; j++) {
						matrix[j, size-9-i] = -1;
						matrix[size-9-i, j] = -1;
					}
				}
			}
		}
		
		public static void AddTimingPatterns(int size, int?[,] matrix)
		{
			// draw dotted line connecting finder patterns
			for (int i = 6; i < size - 6; i++) {
				matrix[i, 6] = (i % 2);
				matrix[6, i] = (i % 2);
			}
		}
		
		public static void AddAlignmentPatterns(int Version, int?[,] matrix)
		{
			/*
				Draw alignment patterns in all combinations of alignment patterns for the given version
				Except for the corners where the location finders are (0, 0), (max, 0), (0, max)
				
			*/
			if (!Data.AlignmentPatternLocations.ContainsKey(Version)) return;
			
			int[] locations = Data.AlignmentPatternLocations[Version];
			int numLocations = locations.Length;
			
			for (int y = 0; y < numLocations; y++) {
				for (int x = 0; x < numLocations; x++) {
					bool isFinderCorner = 
						(x == 0 && y == 0) || 
						(x == 0 && y == numLocations-1) || 
						(x == numLocations-1 && y == 0);
					if (!isFinderCorner) {
						DrawAlignmentPattern(matrix, locations[x], locations[y]);
					}
				}
			}
		}
		
		public static void DrawAlignmentPattern(int?[,] matrix, int xCenter, int yCenter)
		{
			for (int y = yCenter - 2; y <= yCenter + 2; y++) {
				for (int x = xCenter - 2; x <= xCenter + 2; x++) {
					bool drawDot = 
						((x == (xCenter-1) || x == (xCenter+1)) && (y >= yCenter-1 && y <= yCenter+1)) ||
						((x == xCenter) && (y == (yCenter-1) || y == (yCenter+1)));
					matrix[x, y] = drawDot ? 1 : 0;
				}
			}
		}
		
		public static void AddFinderPatterns(int size, int?[,] matrix)
		{
			// a finder patter is a set of three nested boxes
			// 7x7 black with 5x5 white with 3x3 black
			
			// finder patterns belong in three corners: top-left, top-right, bottom-left
			
/*

The top-left finder pattern's top left corner is always placed at (0,0).
The top-right finder pattern's top LEFT corner is always placed at ([(((V-1)*4)+21) - 7], 0)
The bottom-left finder pattern's top LEFT corner is always placed at (0,[(((V-1)*4)+21) - 7])

*/
			int OppositeCornerOffset = size - 1 - 7;

			for (int y = 0; y < 7; y++) {
				// Vertical Separators
				matrix[7, y] = 1;
				matrix[OppositeCornerOffset, y] = 1;
				matrix[7, OppositeCornerOffset + y + 1] = 1;
				
				for (int x = 0; x < 7; x++) {
					// Horizontal Separators
					matrix[OppositeCornerOffset + x + 1, 7] = 1;
					matrix[x, 7] = 1;
					matrix[x, OppositeCornerOffset] = 1;
					
					bool drawDot = (
						(((x == 1) || (x == 5)) && ((y >= 1) && (y <= 5))) ||	// vertical lines
						(((y == 1) || (y == 5)) && ((x >= 1) && (x <= 5))) ||	// horiztonal lines
						(((x == 1) && (y == 1)) || ((x == 5) && (y == 5)) || 	// corners
						 ((x == 1) && (y == 5)) || ((x == 5) && (y == 1)))
					);
					matrix[x, y] = drawDot ? 1 : 0;
					matrix[x, size - 1 - y] = drawDot ? 1 : 0;
					matrix[size - 1 - x, y] = drawDot ? 1 : 0;
				}
			}
			// Separator corners
			matrix[7, 7] = 1;
			matrix[7, OppositeCornerOffset] = 1;
			matrix[OppositeCornerOffset, 7] = 1;
		}
	}
}