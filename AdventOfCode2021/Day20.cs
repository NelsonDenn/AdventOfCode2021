﻿using System;

namespace AdventOfCode2021
{
	public class Day20
	{
		public static int Run()
		{
			bool isTest = false;
			var inputImage = GetInputImage(isTest);
			var enhancementAlgorithm = GetEnhancementAlgorithm(isTest);

			//var result = RunPart1(inputImage, enhancementAlgorithm); // test: 35, 5231
			var result = RunPart2(inputImage, enhancementAlgorithm); // test: 3351, 14279

			return result;
		}

		private static int RunPart2(char[,] inputImage, string enhancementAlgorithm)
		{
			// Convert the image 50 times
			return GetOutputImageNumLitPixels(inputImage, enhancementAlgorithm, 50);
		}

		private static int RunPart1(char[,] inputImage, string enhancementAlgorithm)
		{
			// Convert the image 2 times
			return GetOutputImageNumLitPixels(inputImage, enhancementAlgorithm, 2);
		}

		private static int GetOutputImageNumLitPixels(char[,] inputImage, string enhancementAlgorithm, int n)
		{
			// Write out the image to the console
			Console.WriteLine("Input image: ");
			for (int y = 0; y < inputImage.GetLength(1); y++)
			{
				for (int x = 0; x < inputImage.GetLength(0); x++)
				{
					Console.Write(inputImage[x, y]);
				}
				Console.WriteLine();
			}

			for (int i = 0; i < n; i++)
			{
				// The character that will be seen infinitely beyond the edge of the visible output image.
				// This character is '.' the first time. After, it may change depending on the image enhancement algorithm.
				char infiniteCharacter = i % 2 == 0 && enhancementAlgorithm[0] == '#' ? enhancementAlgorithm[enhancementAlgorithm.Length - 1] : enhancementAlgorithm[0];
				var outputImage = ConvertInputImageToOutputImage(inputImage, enhancementAlgorithm, infiniteCharacter);

				// Write out the image to the console
				Console.WriteLine("\r\n\r\nOutput image: ");
				for (int y = 0; y < outputImage.GetLength(1); y++)
				{
					for (int x = 0; x < outputImage.GetLength(0); x++)
					{
						Console.Write(outputImage[x, y]);
					}
					Console.WriteLine();
				}

				inputImage = outputImage;
			}

			// Count the number of lit pixels
			int numLitPixels = 0;
			for (int y = 0; y < inputImage.GetLength(1); y++)
			{
				for (int x = 0; x < inputImage.GetLength(0); x++)
				{
					if (inputImage[x, y] == '#')
					{
						numLitPixels++;
					}
				}
			}

			return numLitPixels;
		}

		private static char[,] ConvertInputImageToOutputImage(char[,] inputImage, string enhancementAlgorithm, char infiniteCharacter)
		{
			// The output image will grow one pixel in every direction
			var outputImage = new char[inputImage.GetLength(0) + 2, inputImage.GetLength(1) + 2];

			for (int y = 0; y < outputImage.GetLength(1); y++)
			{
				for (int x = 0; x < outputImage.GetLength(0); x++)
				{
					// Determine the binary string for this output pixel by looking at the
					// surrounding 9 pixels of the input image
					string binary = "";

					for (int yOutput = y - 1; yOutput <= y + 1; yOutput++)
					{
						for (int xOutput = x - 1; xOutput <= x + 1; xOutput++)
						{
							// Translate: input image [0, 0] == output image [1, 1]
							int xInput = xOutput - 1;
							int yInput = yOutput - 1;

							// If this point isn't in our input image, then it's an unlit pixel
							if (xInput < 0 || xInput >= inputImage.GetLength(0) ||
								yInput < 0 || yInput >= inputImage.GetLength(1))
							{
								binary += infiniteCharacter == '#' ? "1" : "0";
							}
							else
							{
								binary += inputImage[xInput, yInput] == '#' ? "1" : "0";
							}
						}
					}

					int decimalValue = ConvertBinaryToDecimal(binary);
					outputImage[x, y] = enhancementAlgorithm[decimalValue];
				}
			}

			return outputImage;
		}

		private static string GetEnhancementAlgorithm(bool isTest)
		{
			return isTest
				? "..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#"
				: "#.#.####.####.#.#.#.....##.#..#####...##..#.#..#.####.#.#...#.#.#.#...#########.#.....#.#...##.##.#.####...##.#..##..##.###.##...#.#.#.##.##.#.#.#..#..#..#..##..##..##..#.#...#.#...#..#....#....#.##.##..###.....####.#.####...########.#.##.#.#.#.....#..##..##..###....#.###..###......#..#####..##..#..#.##..#..#..##.#.###.#.....#....#..####..####....#..##.#..####.#...##.###..#.....###..#..#..##...#####.#.....#..##..##..####.###.#.##..####.###.##...##..#...###.####...###....###.#..#.#.####.#...##.......##...#..";
		}

		private static char[,] GetInputImage(bool isTest)
		{
			string data = isTest ?
@"#..#.
#....
##..#
..#..
..###"
:
@".#..##....##.....##....##...##.###.####..##.#.#.#.#....#.#..#..###.#....##..##..#######..#.##.#.##.#
......#.#.##...#.#..#..##.##.###..#.#.#....#..#...#..##...#####.####..####.###...##...##.##....###..
#.##.#..#.##.#.#.###.#.....##..##.#...##....#......#....#.#.###.....#.###..#.#..##...##..##.#.##....
......##.#.#.#.......#####.#....##.##...#.##.##....#..##..##..##.#..#..##..###..#.#.#.#...##..#..#..
.#.....####..##..#.##.....####....#.....##..#.#.#.#...#...##.#.##.#.#..###...##..#.#.##......#..#.##
...#..#..##...#.#...#....##..####..##.###.###..####.##.##..##.###..#.#.##.#......#.#.#.#..####..#...
....#...#...###.##.####.#....#..##.#.##..####..##.....#......###...#.####...#####.#.#......####.#...
....#.....######.....##.##...#.#.#.##..#...####...##...##...#.#.#...###..###.##..#.#.#....#.##...###
.##.######.##.##.######.#....###.#...###.###.#.#..#..##..###.#..#..#.#.######.##.##...#.#.####.#.###
..#..###..#..####.#.#.#...###...####....#....###.#.#.###.##.#.######.##..#.#....##.#.#.....##.####..
.###.###.......#.##.#....#..##.##.##.....##.#..##.#..##.##..#...###...#..#...##.#..#.#.####..##.....
..##.###.##........##.#.##...##.####..##.#...####..###..#..##########.#...###.#...##.##.#...#...##.#
##........#..#####..#.####...##..#..###...#.##.###......#..###.#.#.....###.#.....##...####...#....#.
#.###..#####.#.#..##...###..######...###..##..#.##..##.##.##...##.###..###..##..#.###....##....#..#.
...##.#.###..#..######..#..#..##.#...#...####.####.#...####..#.###..###.##.#####.....##.###.#.###...
..#...#....###....#.#....##.#.##.#####.###.#....##.##.##.#.#.##.####.#..##.#..#.##.##..##...####..##
#.######.##....#..####....##...#####.##...#.##..#.#..##.....###.##...##.######.#..#..#.####.#.##.#.#
#.######....##.....#..##....####.#.#...#..#...#.#.#.#.#.#..#.#..###.....#..###.##.....#######..###.#
..##.#..##..###.#.###...#.......#.###..###.#.###.#######.#..###..#..##.#.#...####..#..#.....#..####.
##.#.#.#....#.....#....#.#.##.#..##.####......####....#..##...#..####.#####.###.#....#####..###.#.#.
.#.##...#...#.##..##.###...###.##.#...#.#.#....###.#..#...#.#..###..#.#.#.#.###..####..########.#.#.
....###.#.#.#.....#..##.#.#.#.#..######.#.#.##..#.###.####.#..#.#######.##.#...#.##.####.#.#..#....#
##.#.##...########.#.#.#...#.#.#.#..##..#...##.#...####..#...#.##...##...#..#...#.###....#.#.#.#.#..
#.......#..###.###..###.##.###..###..#......##.##.#...#####...#...##.#..##.#.###..#.###.####.###.##.
..#....##.....##..#....#..##.#.#..#.##.#.##.##.##########.....####..##.###..#..#.#....##...#..#....#
..##.#..##..#.#####...####..#.##.###.....####..##..##...####.#..#.##..#...#...#.....#..###..##...##.
....#..###...#...#.###.#..#..##.####..#..#.###.....##...#########.#...###..#.#.#.....##..###...#..#.
###.#.##.#..#.###.#.#..###.##..###...###....#.#..#.............#..#.##.####.....###.###.###....#....
..#.###.#....###..#.###.##.#..#.###..##.#...##...#.#.###..#.#.##....#.##.#.#..#..#......###.##.##.#.
##..####..#.#.#...##....#.##..#.......##...#.......###.####..####.#..####.#...###...#...#........###
#.#...###.#.#.###....#.#..##.##....##.#..#.#..#.##.#.#####..###..#...#..######....##.##....###.###..
#.#.#..####..#.##..##.##.######.####.#.##...##..##..####...##.#.###...##.###...#.##...##...####.#..#
.##.#..##..##...#.##.#.####.....#.#..#.#####.###..###.#.#..##....#..#..###.#.....##....###.#########
#####.##.#.########.#..###.###.#.#.##..#.###.#..#...#.#.#.#.##..#....###.#...###..#.#..##...##.#....
.#.#..#.#######.###..##...##..#..###.#..#####.#..#...#####...#.......#..#.######..##.....##.#.#...##
#.....#..###.##.#.###.#.........#.#.#.#####.##.###########.#...###.#..#..#..#.##...#....#..#....#.#.
#.###...#.#..#.##.###...####.##.##.##..#..#.####.##.#####.....###...##.###.#..#..#..#.##.####.####..
##...##..#..#######..#####......#..#..##..#..#..#.###.##.##.....#####.#.###..###..#..#.#######...#.#
.#...###...#..##.......#####.###.#...##...##.##..#.##.####.......#.#...#...#..###.##.....##..#..#..#
#..#.##..#..##...#.#######....#...#.###.#.##...##..#..#.#.#.#...#.###.###.##......#..#.####.#..#####
........##...###..#.#...###.###.###.#..#..##..#.##..##..##...#...#.##.#.#.####.....####....##.##...#
#.##.###.##.#.#.#.#.#..#......###.######.#..#..#.#.#...#.#.##..#....##.##.##.#.#....#.......##..#.#.
#####.#####.#.#..#.#......##.####...##..##.#.#.#..#....####..######.###..##.##...##.######.##....#..
.##.#..###...#########...##....###.##.#####....###.##.####..##..#.....#.#......##....###.#####..##.#
.#...##.##.#..###..###......####.###........###.#.####..#..####..#.#...######.##.####..#.###.#......
#.#.##.##.####.#.##.#..##...####..#.#.##.#..#.#.....##.......#####..#####......#.......#....##.##.##
####.#.#.##...#.####.##.#...#.###..##.##....###.#####..#######..##.##.##....###.######..#......#.###
#.##...##.#.##.####.####.##.#..#########...##.#..##...#.#.#.####....####.#..#....###...##..####.#...
..###..###..###...#..##...##.#.#.#..##....#.###.#####.........#.#....#.#####.##....#.##...#..##..#.#
.#.#.#.#..#...#..##.#...######.##.###....#..#...##.#..#.##.#.#.#..#######..##.#..#....##..#....##.#.
##.#.###..##...#..##.##...###.##.#.##....#..#...#..##...##..####.....###.##.##.....#..##..##..##.##.
##.##..#.#..#..#####..#.#...#....#.##..#...########.#######..#.#.###.#...#..####.#..##.#.###.##..#..
#.#.#...#.#.###...##.###.##.#..#..#..#..#.##.###.#..#######...#..#.##.#...#.##.#...##.#.##..#..#.###
#.#.#####.###...#..#..########..##..#.####.###...#.#.#..#..#.#...#.....##.##.#####.#..####..####...#
.#..##....##.#.##.#.....#..#..####.#.##.###.....#.#..##...#..####..#####...#.....#.##..###.##.###.#.
####..#..###..###.#.####..####..#.##.###..#.###...####.#....######...#.#...#.#...####...##.####.#.##
.###..######......#...##.###.###.###..##.......#..###..##.##.##....#..#..#.##.#.##..#..#..#..#..#.#.
##..##.##.####.####.#..#.....#.#......##.#####.#...##...####.#####.###.#...###.###..##.###..#..##.#.
.#..###.....##.###.#....##.##.....#..#.#....#.####..####.###..#..##..###.....##.##.#.######.###.#..#
#.....##....#..#.#.#..#..###...#..#..#..####.##.#..#.##....##.#.###..#..#.....#.....#.##...##.....##
###...#.#...#.#..#####.###.#.........#.###.....#....#####....#.#....##.#.#.##...#...#.#...#.##....##
..##..#..#.#####.##.##.#######...#.#####.##..##.....#####.####.#.####..#.#.#..#...#.#.#.#....#.#....
###.#..##.####....#..#.##.#.####..#..##...#..#..##.#....#...#####..##...#.#######.#.####......######
.#..##.....##..#..#.##..#.#########..##.#######...#.##.#.##...#.###..##..#..#.###.#..#...##.#.#.###.
.##...######...####.#.#####.#.#..##.#...##.#...####..####...#..###...###.....#.##.#.#....###..##....
#.##...#..#..#.#.#.#....#....####.#.#.#.####..####...#.#.##..#....##.....#..#....###.###.#.#####....
#.#.##.##..#.#...#....##..#...#.#.##..#.#....##...#..##.######.....#..###.#####.##..#.#.####.#...#.#
#.###..##.#.#..#.#..#.###.#....#..##...#.#.#..#.#..#####.##.##.....#.#.###..######...#.#.##...###..#
.##.#...###...#...##.####..###.....####.#.####..##.##..#.###.#..####...#.###.#.##...#.#..##..#...#..
.###.#.#..#....##.##..##..###.####.##..##...#####.#.#...##.#..#...#...##.###...##..#......#...#.####
.......##.#..#.#.###.####..#.#.##.########..#.###..#.#......#....##..#.###..#..#....##..##..#......#
#.#..#...####.##..#..#.#..###.......##..#..#....#.#....#.#.#..##.##..#....#...#####.####.##.##...#.#
..#.....#####..#...###.####.##.#..##.#..#..########.#..#..##.###.#.##..#.######....##....##.###.#.##
#.#.#.#..#..#.#.#.#.#...#####.##.#.#.####..##..###.###.##.#.#..###..#.#.#....#...##.#...###...#.#.#.
#.#.#..#.#.#..###.#.#....#.....#.#.###.......##...##....#....##.#.####.#.###...#.##.#..####.#...###.
#...##.#####.##.#..#..###..###.#.#...##..##...#...#...##..##...#......#...#..##.#.##.....#...#..#.##
#..##.#...#..#.#.####...##.######...#..##.##.##...#####.....#.#.#.#.##.####....#.#...####.##.#.##...
##..#.##..#.####.#.####..#..##.###.....##.#...##.#..#.#..#.#...#...#..#..###.##..##..#.###..###.##.#
.##.#..##.#########.##...........##..#..##...##.#.....##.###...###.####.#.#.##...##....##.#.#.#.#.##
#...#...###.###....#####....#.#...##.####.##..#.###..#.#.#..##...###.##..####..###.###.##......#####
.#..#.##.##.##...#.##...#.....#.##.##.#.#.#..#...#.#.##....###.###.#.#.#.##.#..##########....####.#.
..#..#.###..###.#..#.#.#..#.#.###....####.#####.##.###..#..####...#......#####..###........#..##...#
#.#.#.#.##..######.....##..#.##.#####...##......#.#.#..####.#..##.#####.....#.##.##..####.##.#.##...
...##...#..##...##...##..#....#..#####.#####.#.##.#......##.##...####.#####..##...#...#..####..##..#
.##.#.###########...###...##..#....#...#...###.##.###...######..##.....#..#.######.#####.#..#.#.####
..#....##.......##.##.....##...##....#.###.#..#...##..#...#....###.##.##..###.####..###..##.##..##.#
###....#.#.....#...#####.#.#....#.##..##########.#.######....###..###.##.#####.####..###.#.#.#.###.#
#.##.##..#..#####.....#..#.####.###.#...##..#.....###..#...######.##..##.###.##..#####....##.#####..
#...#..###.#....##..##.###.###..#.#...##..#.###....######..#..###.#..###.##....#.#..#...#.#..###.#.#
#..##..#.#.....#.....###.##.#...#...###..#.#####..##......#..#.##..###.#..##.###...####...##..#....#
###...######..#.#...#...#..#..#..####.##.##.##.##.##..##.###.#.#.##...#.#..#..#...####...#.###.#.###
..#.#.##..#.###.....#...#..##..##...#..#.....####...#.#..........##.....###.#..##.#.##...##.#..#..#.
#.##....###.#.##...###.#...#.#.#..#.###.#####.#######..#....##..##.####.##..#..#..#####...####..#.##
#...#.####..##.###.#...##......#.####...#..##.....#.##..##.#.##...#...##.#.##.#.##.......##..###.###
....###.##..#.#..###..#....#..#.#.#####...#....#.##...##..#.#..#..########.#.#...##....#.#.#..##.##.
..#......#.#.#.#.#...#.#.####..##.#####.##.......###....#.#.#.#....#....#######...#.#...#....#.##..#
.##.#.#...#...##...#...########.##.#.#.#.##...#..#.##..##.#.###..##..#####.#.#.....#...#..###.....#.
#...#....####..#..#####..#.##.....#.#.#...######......#.#.###...#.####.#......##...####.#.#.##.###.#
.###.....###...##.###..#.####.###.#...###.#....#..#..#.#.##.##....###.##..##..#....#####..###..##.##
#...#.##.##.#...#.....#..####..#.#..#.#.#.###..#.......###...#.....#...##.##...#.#####.#.#....#...##";

			return ParseData(data);
		}

		private static char[,] ParseData(string data)
		{
			var rows = data.Split("\r\n");

			var inputImage = new char[rows[0].Length, rows.Length];

			for (int y = 0; y < rows.Length; y++)
			{
				var row = rows[y].ToCharArray();

				for (int x = 0; x < row.Length; x++)
				{
					inputImage[x, y] = row[x];
				}
			}

			return inputImage;
		}

		private static int ConvertBinaryToDecimal(string bits)
		{
			return Convert.ToInt32(Convert.ToString(Convert.ToInt32(bits, 2), 10));
		}
	}
}
