using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day25
	{
		public static int Run()
		{
			bool isTest = false;
			var data = isTest ? GetTestData() : GetData();

			var result = RunPart1(data); // test: 58, 486
			//var result = RunPart2(data);

			return result;
		}

		private static int RunPart2(SeaCucumber[,] plot)
		{
			return 0;
		}

		private static int RunPart1(SeaCucumber[,] plot)
		{
			// per step
			// herd 1 moves east
			// herd 2 moves down
			Console.WriteLine("Initial state:");
			DrawPlot(plot);

			int width = plot.GetLength(0);
			int height = plot.GetLength(1);

			int numSteps = 0;
			while (true)
			{
				// Keep track of the number of sea cucumbers who move each turn
				int numMoved = 0;

				// First check each sea cucumber in the east-moving herd. Can they move forward?
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						var seaCucumber = plot[x, y];

						// Only concerned with east-moving sea cucumbers at this point
						if (seaCucumber != null && seaCucumber.IsMovingEast)
						{
							// Check if the space to the east is empty
							// If we're at the eastern edge of the ocean floor, wrap back around to the west
							int newX = x == width - 1 ? 0 : x + 1;
							if (plot[newX, y] == null)
							{
								seaCucumber.CanMove = true;
							}
						}
					}
				}

				// Now do the move, and reset their CanMove status
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						var seaCucumber = plot[x, y];

						// Only concerned with east-moving sea cucumbers at this point
						if (seaCucumber != null && seaCucumber.IsMovingEast)
						{
							if (seaCucumber.CanMove)
							{
								int newX = x == width - 1 ? 0 : x + 1;
								plot[newX, y] = seaCucumber;
								plot[x, y] = null;
								seaCucumber.CanMove = false;
								numMoved++;
							}
						}
					}
				}

				// Then check each sea cucumber in the south-moving herd. Can they move forward?
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						var seaCucumber = plot[x, y];

						// Only concerned with south-moving sea cucumbers at this point
						if (seaCucumber != null && seaCucumber.IsMovingSouth)
						{
							// Check if the space to the south is empty
							// If we're at the southern edge of the ocean floor, wrap back around to the north
							int newY = y == height - 1 ? 0 : y + 1;
							if (plot[x, newY] == null)
							{
								seaCucumber.CanMove = true;
							}
						}
					}
				}

				// Now do the move, and reset their CanMove status
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						var seaCucumber = plot[x, y];

						// Only concerned with south-moving sea cucumbers at this point
						if (seaCucumber != null && seaCucumber.IsMovingSouth)
						{
							if (seaCucumber.CanMove)
							{
								int newY = y == height - 1 ? 0 : y + 1;
								plot[x, newY] = seaCucumber;
								plot[x, y] = null;
								seaCucumber.CanMove = false;
								numMoved++;
							}
						}
					}
				}

				// Update the number of steps taken so far
				numSteps++;
				
				// Draw the plot
				//Console.WriteLine($"After {numSteps} steps:");
				//DrawPlot(plot);

				// If no sea cucumbers moved, then we're done
				if (numMoved == 0)
				{
					break;
				}
			}

			return numSteps;
		}

		private static void DrawPlot(SeaCucumber[,] plot)
		{
			for (int y = 0; y < plot.GetLength(1); y++)
			{
				for (int x = 0; x < plot.GetLength(0); x++)
				{
					var seaCucumber = plot[x, y];

					if (seaCucumber == null)
					{
						Console.Write('.');
					}
					else if (seaCucumber.IsMovingEast)
					{
						Console.Write('>');
					}
					else
					{
						Console.Write('v');
					}
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		private static SeaCucumber[,] GetTestData()
		{
			string data =
@"v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>";

			return ParseData(data);
		}

		private static SeaCucumber[,] GetData()
		{
			string data =
@".v>v...v>.vv.vv..>>......>.>vvvv...>.>v>v>.>.v>..v>v>.v.v>>v>vv..>v...vv..>.v..vvv...>..vv..vv.>..v...v...>..>..v>>..v.v......>.>vv.v...v..
>.v.v..>v..>..>>.>.v>>.>>..v..v>.>vvvv>.v...>..v.>..>..v..v.>..v.>v>>..>v......>.>..v>..v.>>vv..>...>v>.v.v>.....>v....v>.v.>.>.v.v>>.vvvv>
....vvvv..v...v.v.v...>vv.vv.v..v>v.>>.v....v.v.>>....v>>>>..v......v..v...vv.v.>..>>v>......v.>>>..v...>..>..vv.vv..v.v>.vvvvvv...v>>..vv>
...vv.>>.....>>>...vv...>.>vv>>>.>.v..vv...>.>>...>.>..v.v.v..vv>.v...>.>vv.v...vv..v...>.v..v...>.>.vv..vv......>>.>.v.v>...v>.>v.v.>.v>vv
v>>...>.>.>>.>.>>vv.>>.>>>.>.>.v.v.>v.>>v.>..v>.>.>..v>.v.v>.v>v..v>v.v>>>>.>..v.v..vv.>v>>>vvv>.v.vv..>v>.>.v..v.>>v.>..>.>v....>>v..>v.>>
>v....>>.vv..vv>>..>>..>.>vvvvv.>v.v.>..v....>>.v..>.....v....>.>v...>v>..>>>.v>...>...v>v..v.v.>>......>.v>v.>v.v.>.vv..vv>v>...v>..>v....
>v...v>>...v.v....>.>v>v.>..v>.>v>>.v.>.vvvv..v..v.v.vv.......v.>v..v.v>.v...>..>>>>.>>v..v..v...v>.>v.>.>>v.v.>v.>..>........v...vv.>vv...
.>v>>.>>>>..>>.>vv...>.>....>>>.>>..>v>.>.v.v..v>..>>>vv...v.v>.>vv>.>>..v....>vv>..>.....>>vv..>>.v>..>.vv.>.v..vv.v.....>.vv>....>.......
>.v.v....>>>.....v.>..>v>v..v..v>>.>.....>v>.>..>...v....>.>vv>.....>.v.v>v.......v.v>v>...>v..>v..>>v..v>.v.v>v.>v>vv.>v>.>vv>>vvvv.v.vv.>
>v..v>vv....>>v>>>v..>>.>...>.>v.>.>>.>vv..>vv..v>>>vvv.v>>.>>>...v.>.vv.>>.v..v.vv>>>>>....v.......>v>.>.>>v.>...>..v.>..vv>v.v...>>.>>>..
>.>v>.v.vvv>.>.vv..v>>vv..>v.....vv.>v...>v.v>...>.v.v.....vv>.>....v...v.>>>>.v.>...v>>>..v..vv>>..v>...>v..v...>>.>v>>.v.v>..v.v..v.v.v>.
>>..>v....v.v>...>v.>....>.>v..v..>>..v.v>>>...v.....v>.....v....vvv>v>>>..>v>.>..>.vv.vv.v...>.>..v.>>v...v>v...v...>v.v.>..v.....v.>.v.>.
.v>>v.>>v...vvvv.>v..>v>.>>.v...v>>.vv.vvv..v.v.v.vv..v.>v.vv...>v..>.>..>.vv.....v>v...v....>v..v..>..vv.....>.v....>v>>.v......v...>v.v>>
.v.v..v>vv>v.vvv..v>...v..v.>v..>>vv..vv.>v.v.v>v.....v...>..v..vvv...v>>.....>>>.v.vv.>.>>...>>........>vv.>.vv>v....v..>.v>v..>v>....>...
..>v.....vv...>v.v.v....v.....>.>>v....v.v.....>..>...>.....>..v.>..v.>.>.v......>>...>v.v>.vv..>..v>vvv...>...>vv.v>>.vvv>...>.v.v.>..v.vv
v>.>v.>>..v..vv.vvv>.vvv>>>v.>v>>v>vvv>v..>.v.>.>.>.>....v..v.v>...v.>.vv.vv>.>..v.v..>..>....v.>...v...v.>....vv>>.vvvvv.>.>>>..>v>>.>...v
....v.>vv>vv>.>.v.>v....>>.v>.>.v.v..>..>....v..>.v>v..>v>>.>>..>.>>>..v.v...>>v.v.>>..vv>.v.>.>..v>..>....v>v..>..>.>.>.>.v>.....>vvv..>..
v...>>>.....v...v.>.v.>.>.vv.>..>>>v.>v...v>>v>.....>..>>.>.v.>v.>.vv>>.v...>>v>>.>>vv>..>v.v..>.vv..>>..>.>vv.v.vv...>....v>..v..>vv.v>>>.
vvv>..v>..v>>...vvv>..vv.v.v.>v>>>v.>>>...>vv..v.>..>.v>...v.>v...>v..v.v.v...v>v.v.v.vv>>..vvv..v.vv...v.v...>..>>>>..v>.>v.>>...>.>>>v.v.
...>>>v.v...vv...>v>v..v.v......v...v.v>..>.....>.v>..v..>..v..>>>>..>....>.>....v..>.>..v>...>>..v>vv.>..>..v>...>.>v.>>>>.v>>v....v.>....
....v>v>>.vv.v....vv...>.>....v...>>>.vv...>>..vv......>v>v..v.>..>v.v>...vv>.>>.>v...>..>v>.v>.>v..v.v>.>....v.v>vv.>v>..vvv.vv>v..>v..>..
.vv..vv>..>.>.vv.v.>>....>>>..>>v.>>.>>>.v...>>.v.>v...v>.v.>v.>....>.v.>.v>v>..>v>.>v>.v...v>.vvv.>>>v...>>.....v...........v>v.>vv.>>>...
.v.>v..>>v>.vv.....vvvv.v..v.v...v.>..v.>.>..>...>>.>v..>.v..>>..v...>vv>...>....>vvv.vvv.v>.v.>....>>vv.v.>>v.....vv>..>....>>vv.v>..v...>
.>.>..v>v..>.>>v.vv.v.....>.>v>...>.v..vv.>..vv>v.v.v..>>>v....v>v.>.vv.v.>....>vv>vv..v>>..>.v>.v....>.>vvvvv.>v>..>v>.....v...v..v>.v....
.>..>......>.vv....>.vv.>..>>..v..v..v....>v>vv......>..>.vvv.>>vvv......>..v>.>..>>v....v>v>v...>v>.v..>..v>v>v>....>...v.>v>.>..v>....>.>
.v.>.v...v...v>...>....vv...>>..v...>>..v.>.....>v..v....>>.v>v>v.>v...>...v.>...>..vv.>v...>.>>......>.v>.v.vv..>........>v.vv>...vvv>...v
v>........>..>>v>v.vv.....>>.>.>..>.>>v>.vv>..vvvv>>>>..v.>..>..>v.>>..v..v.>...>vv.>..>.>.vv....>v>..>.v.v......v.>>>..v.v..>..vv.vv......
.>v..vv>.v.>>v>>vv.>.v...v..v..v.v.>v>.>.vv.v.>.>>.>v>..>..>.>.v.......v..v>>vvv..v..>.>.v..vvv.v>vv>.>>v>v...>.vv..>....>...vv..v..>....>.
...v>.vv.......v.>>vv...v....>v>.v>>...vv...v>v.>.>>>v..vv.>...v.v..v......v>.>>v..>v........v.v>.>..>v>>.....vv..>...v..v>v..v.v.v..>>>>>v
.vvv>..>>..>...v>.>..vv>.>..>>.>.>>v...>vv>.>>v>..>.vv>v>v..v....>v.v>>v.vvv.>.>.vvv.>..>>>>>...>v>v.v.v.>..v..>>.vv>v>>...v.v>>...v.v>.>.v
.v>v.....v...>>.....>>.>vv...v.>..>..v..>.>vv>....v..v>>.v>vv...v>v.>v....vv>>.>v>>.>..v.>..>.>.v.vvvv.vv>>.v>v..v..>>.v....>..v.....v>....
....>v>>v>...>....v......>>..vv.>v>v..vvv.vv..>v.v>v>v.v>.v..vv.....>.v.>..v>....vv.>.>>v...v>.>.v>...>.v>.v....>>v.>.v>.>.v.vv...>>vvv..v.
>..v>v.>v>.v..v..v....>.>v.>.>>.v>.>.>>v>vv...>>...vvv>....>..v>>v.>v.....>>>>>>.v>>>.v.>...vvv>.>...v>..v>.vv......>.v..>.>.v>....vv.vv.>.
..>vv...v>v>.v.>.>>v..>.v.v.v.>...>>>v.>.>>..v.v......v..vv..>.>>>>.vv...v>.v.>vv.>.>.v>v>.....>...>..v..v>..>.v>vvv..>....>vv>vvv.>..>>v.v
..>v>....>>.v....>vvvv...v...>v>...v.v.>.....vv...>.>.>>.v.>..v..>..v..>.>>>..>...>vv......>v>..>>v.>...>v>>>>v.v..v..........>.v>v.>>..v.v
.v.>>>.vv.v.....>..>..>>v.>..>..>..v..v....v.>>v>>..vv>>.v...>vv.v...v..vv>v>>..v>.v>..v.v.>v.>.>...>v..v>.>.v.>.v...>..>>...>>..>v..>.>v>.
v........>v.vv.v..vv..v....>.v.....v...>>v.v.v..v>.v>...v>v...vv>..>>vv.v..v..v.>..v..v...v..>v>v>>vvvv>.v...vv.>vv>..>...>.>>.>vv.vv>..>.v
>>....>v..>>..>v.v.v..v.>...v...>.v..v.>...>..>>..v>>>...vv.v.v>>v...v...v.>>>..>>..vv..........>>v>v.v>.....v.v.>.v.>v>v.>.v.>.vv....>v>>>
>...>v>.>.....v.>>..vv..>>>...vv.>>..>>..>..vv.vv.....>.>..>>.v.>v>v>v.v..>v.vv>.v.>.....>vv>....>>v.>>v..>v..v.>>..>>.>......vv.v..>..v>.v
.>vvv>v.....>>vv>....>.v>vvv.v>v......vv..v..v>..vv..v..>.v.>>v>>vvv..>>.vv.v>>.....>v..v.v...>>>....vv>..>v.>>>.>vv......>vv.vv>v>v.v...vv
.>.>v..v.>>.vv.v.>.v..vv.v..v.>v.v.vv>...>..vvvv>v>.vv..v...>...v.>>vv>..v.vv.v..>v...vv>.>...>........>..v.v.v....vv>.vvv.v.>v>>..>>....>.
...>...>...>>...>v.v.v>vv>.>.v>vvv>........v>v.v..>>>.>....v.>.>.vv.vv.>.>.v>v.>>>>.>>.>.>v.>.v.>>>vv>.v>.v.v.>..>>v>>.>>v>vv>v.v.vv.vv.v..
.....v.>>..v..>vv..vv>v>vv.......v..>.v...v.vv>.>.>v.v>.>...>v>v>.>.>.....vv>v>.v..v>.>>>....>>>>..>.>v..v.v>>....>.>...v.v.v..v>v..vv>>>vv
v...>vv..v>v>v...>v>v.>>.>....vv>..v..........>>v..v.v>v..vv...>>...vv.vvvv>.>..v.vv>.>..vv.>.>>>>vv.vv..>>....v.>>..>.v.v>..v..v..v..v.v>.
....>..>.>.>v......v>....>.v>>v.>>>.>....>v.v...v..>>...vv>v>>vvv......>.....vv.>>vv.v.>>..>.....>.v>>>.v...>>>.vvv...>..>>.....>........>v
..>>...>..v.vv>.>>>>..v...>..v>.....v.>..>>>v..v>.v.v..vv>.>v..>v.v...>.......>.vvv.>....>.v....v.v...>>>..>..v>>v>>v>.>>.v>>>...>.>....v>.
>>>v>..v>vv..v.vvvv>..v..v..>>..v.>v>vv>..v.v.v.vvv.....v>>v>..v.>..>>>v>..>......>v..v>>..>>>......>.>.>vvvv.>...>vvvv>.>vv.v.vvv..v>.v..>
>.>v..v>...>..v.>>.v...vv>.v...vv.....v>>>.>..>>.>>..v....>v.>....>v>>v>>>v>...>.v.......v>>>>..>vv>v...v........>v>v..>vv..v>>..>..vv..>..
v.vvv..v.>.....v.>.v..>...>...v...v>......v.....v..v.vv>>.>>v.>.>v..>>v..v.v.>v>.>.>.>>.>vv.v..v>>.v....v........v.v.v.vvvv>.>>..>.>......>
vv.v>v>vv.>>v.>..>v...v.vv..>v..v.>..v.>...>.....vv.v.>.>>v.v>.v...>v.vv>.v>v..>.>>>..v.vvv..v>..>....v...v>>>>v>vv>.....>>v.>>..vv>v.v>v..
.....v>.v..v.v.v.>>.>.>.>.>...>>...>>.v.v>...>>..v..>>.>...>.vv>.v>.>...v..v..v.....v.>.>.....v....>.....v>v.>.v>..>v.v..vv>..v.v.......>..
..vv.v.>v>..v>v>vv>.>vvv>.vv..v>..v.>.>....v..>.>.v..>v>v......>v.v>.......v.>.v>>.....vv..>...v..>......>>.v.v.>.v.>>>>.>.>.>...v...>>v..v
..vvv...v>.>.>......>.>>v......>..v......>>v>v.vvv>.....>...>v..v...>.>>v.v.>..v..v.>>>>.>>.v>...v..v.v.>.....vv...>.>vv.vv.v.>..>>..v..v>v
....v....v.>>v..>v.v>v>..>.>.v>..vvv..vv..vvv.v>v..v>>.v>>v.v.v.v>v>.>v..v..>...>v..v>.>vv..>v>..>.>...>.v.vv..>v>..>vv.v>.v.>...>.>.v.>..v
..vv>>v.>>>..v..vv.>>vv..>.v..>.>vv.v>v>..>vv>>...v....v.>v>..v>vv..>.vvv.v>...>>..v>...v>v..>....>...v>vvv>>.>v>..>v>...v>>.v>..>..v..>...
>>.>.v>>v...vv..v.....v.......>v...>>.....v>.v.>v..vv........>....>v....>>v...v.vvv.>.>.>.>v..v>>....v.v..>.......v.....vv.v>.v>v..>...vv..
...v.>..>v.>>..v.>.>v......>v.....v....>..>>v>.>vv.v..v>v>v>v.......v>.v>.>..>>.>vv>>v>>v.>.>.v.....vv..v..>vvv.>>v>..>vv>.vv..>vvv.>>..>>>
...>vvv>>.v>vv>>......v..>.>.v.>>>.v....>..>.v...>.v.>.>..>v>......>...vv.>v..>....vv...>..v...>>....>..>...v.vv.>>..>....v>vv..>vvv>.v.v.>
.v..vvvv..v....>v>>v.>>v..v....>>vvvv>v>vv>.v.v.>.>...>v..v...>>...v>>>..v.>v..v.v.v>>v.>.>>..v>>..>>vv...>.....v.>vv>.v....v.v.....>v.>>>.
>>...>v.>.>.>>.vv>..v..v>vvvvv.v.>.>..vv>v.>.>...>v..>>...>>.vv>v>...>v.v......>>....>.>>.>.>.>>v...vv.>...v>v>>.v...>>...v.>...>>.>...>.vv
>.>..>.>.>.vvv>..>vv>.>>.>v...v..v.>..v>...v..>............vv>v.>vvv>v....v.vv..>.v..>>>.......v.v...>...>....>..v.....>..>.vv.>.>.v.>.v>..
.>>...v...v..v>vvv>>.>.v>>.vv..v.v>..vv>>.vv.v.v.>.vv>>v.v.>vv>..............v>v>.>..>.v..>...>vv.>>.>.v.>.>...v.v.v...v>>.vv.vv.>.v..>.v..
..>.>>vv.v..>v>..v..>v.v>.vv.>v...>....v.v.>vv>.>>.>>v..>v....v>>>vv....vv>.>>.>v>.>>..v.....vvv>.>>>..>..v>>.>vv>>v>.>>v.>v.>v>.......v>.>
v....v.v>>>.>vvv>...v>v..>>>..>.>.>v.>....>.>.>..v..>v.vv..>.vv...v....>>.>v.>>>....v>v....v.....vvvv...v.>.>v..>vvv.vv....>>v..v.v.>..v>vv
>v>.v....v..>.v......>..vv>.v>..v..>..v>..vv.v.>.>...>....v..vv>..>.>>>.>.>...>>>v.v>>....vv>v>...v.>>>..>v...v.v>....v>.>.v>>...v>>..vvv.>
..v....v...>.....>.v.v....>..>v.v..v...>.>.v..v..>>..v...v..v..vvv........v.>....v...>v>>v>...v.v>>>>.v.vvvv...v...>>.v>..v>>.v......>>..v>
>vv>>>..>>v>.>>>>vvv.>>>.>....vvvv.v>v..>..>.v.v>>>.>.>>>v.>>>>.>.>....v.v.vv..>.v..>v..v..v..>...v...vv>>.v.vvv..vv>>>...>>.>>.vv..>v.v>v>
..vvvv...v...vv>>..>v...>vv..>>v.v...>...>>v>>.>v>..>v>.v.v...v..v..v>.>v.v...v>....vv....v...>....v...>.....vv>>.vv>>..>v.>>v...vv>v...vv.
vvv..v.vvv>.>.>>>...>..v...vv.>>vv.>..v.v...>v.>>....v.>>...>v>.vv...>..v.v...v..>v>>.>.v.>v.v.v..>.>.v.>>.v>>v>.v...vv>....vv.>.vv...>v...
v..v..>..v...v>>..>.>v...>v..vvv>...>.>.>.v...v>vv...>.>vv.>.v.>v..v>v..>..>v>>..vv>.>.>>..v.>>.v....>.>.....v>v....>v.v>>>.v.vv...........
>....v.v..>.>>..v>v...vv.....v.>..>.>..v>..v>....v>>.vv>>..v>...>..>v..>v>vvv.>.>v>v..vv.>.v.vv.>v>...>>.>.>...vv>.>..>>v>.>..v.v.>vv>>..v.
..v>>...>>.vv...vv>.>v.v....v>>v.>.......vv>v.v.v>v.>v.>...v>>.vv>>....v..>...v..v>vvv>..v..>v>..>>..v....>v>.....vv.>>.>..v>>v.v.>>v.>.>..
..v...vv>>.......>.v>.>vv>.vv>..v>.v...>..>v>.v.>>>v>v.vv.>>.>>.>v.v.v>..>..v>>v..>>vvv>v.vvvvv..v.>.v.....v.....vv.>v.v.>.>>v...v...>vvv>v
.v...vvv>....v...v.>v.v>.v>>>....vv>....v>.v>.vvvv.vv.v.v.>v.v..>>v.>.vvvv....v..>vv....>..vv>.vv..>.vv.>v.>v>..v.v...>>v.v...v.>.>v.>>..vv
..v>>v..>>>vv>.v...>>>.>.......vv>.v.>>...>>>.vv>..>.>.v>......>v....>>>v>.>.>>.v>..v>>.>.vv..v..>..>..>.>.vvv....v>..v.v>>..>>.....>>.>>..
v>v>>>>v...v>>.v>>v>>...>.>>..>v..>v.v.>>..>..>>...vv>.v...>.>.>...>.v......v>>.v..>..v>v>v....>v...v.v.>.....v......>v.v..v.>>>.v...>>>>..
>v..>.........>.v>.....v>..>.v>v..>v..v>v>>v>.>.vv..v...v>vvv..>.v..>.v.v>>>>...v>v.>>v>.....>..>>..v.>..>>..>>.v.>...vv.>...>v.>v>.v.v...>
>..>vvv..>...vvv>>v>.vvvv.vv.......>..>vvv.>>>v.v.>vv>vv.v.>.>.>...v.v.>>>..v.>....>.>v.>>v...>v..v.vvvvv>>>>v.vvvv>v.v>v.v.vv.>..>v..v.v>>
..v.....>>v..v>>v.v.vv.....v.>..>...>......>...>.v>..>..vv.vv...>vv>vvv..>v>v>>v.....>>..vv>>..>>>>......v..v.v...v.vv>.>vv..>v...>..>..>.>
...>>vvv.>.>>>v...v....v.>.v>.v.>......>....>v.>.v>v.vv.>>>v>>v...v.>.>vv.v.v>>v..vv>vv>>.......vv.>.v.>v..vv.vvv.v.v>...v..>>.v.v..vv.v.>.
v.vv>..v...v.>>..>v.>v.>>.>......>v..>>>v>....v.v..v>.v.v.v..v....v.v.>v>vv>.>>.v>v..v.v>....v.v>>v.v.>>..v...>v.>v>v.>..>>.>>vvv.>>..v.>>v
..vv..>...v...>>...>>.vv>>>v.v>.vvv.>vvv>v.v....>.v>....>>....>.v..>....v.>.>.>>.>...v.v.....>>>v.>vv>>>..v>>v....>>.>.v>.v>>..>>vv>...vv..
.v>v>..>>......>.>>v...>v.vvvv.vvv.v.....v>vv.v>vvv...v.>..>>....>>...>.v....>....v...vv..vv>...v>.>v.>v.>..>.>>......>v..>v>v..vvvv.v...v.
.v...v>.....v>v>.>>vv.......vvv.vv>.>....v...>...vv.>>>...>>...v>.vvv..>>v......>v...>...v.v>v...>.v..>>vvvv...v>>..>.>..v.v.v..v.v>>v.v.v.
...v>...>>vv>....v.>.v>.>....v......>vv....v>.>..vv.>...>....v>.......>v>>>v.>.....>..>>.>v.>>.>...v>.v.>vv>.v>vv.>v..v>>>....>...>..vv.>..
..>v.vv>>vvvvvv.>v.v....>.v.vvv..>v....vv..v>v.>v.>v.vv....vvvv....v>>..v>>>.>.>.>vv..>.>>vv>.>>>.v...v....>vv...vv.vv.v>v>........>.v>.vvv
.>..>...v..>>>..>...v>>v.vv.>>v.v..>..v>.>>.v>vv.>..>v..v.>vv..vv....v...>.v.>>>.>v..vvv.>v>.v.v..>.>..>v.>>>>.v>.v...>v.>..v.>vv.>..vvv.>.
>.v.......v...v.....v.v>v...vv>.>>vv.v>.vvvv.>v>vv..v>v>>..>.....v...v..........v>v>.v>v>.>..v..v.>.vv>.>>v>.vvv>.v.>vv...vv>v..v.>>.v..v..
>.v.v>..v.>v...>.v>>.>v..>....v.>.v>v.>>.v....v.>>vvv>..vv.>>>...>>>v.v>>..v.v.>.>vv...vv.v..v>>>.vv.>v.>...v..>>..v...>....>..>>.>..>>>.>v
>..v..>.>v>.>.>.....v.>..v>.v>>.v>..>.vv...v...v.>...v...v..vvv.>v>v..>>>.>>>>.v.>v..>..vv..v>...>v.>...vv..>>.>v..>..v.>.>.>vv.v..vv>.v..>
>.>vv...>v>..v>..>>..>.v.>.>..>>.v.>>...v>.v...>vvv>v..>.>.v.vv.v.vvv.vv.>...vvv..v.v.>.>>>....v...>...>..v>v>..>v>.>.>..>>.....vv..vvv.v.v
>..>.>v>..v>.>.>>v>.v.....>>..>....>>>>>.>>...v>.>..>.>..vvv>..>vvv.>.>..v.>>.>..vv...>...>..v..vv.>..v...v..>.>vvv.>>v.>.>..>..>...>>..v..
..v.>.v..>..v.v...vvvv.vv.>v.v.>v>>.v.vv.>........>....vv.>>....>v....v.v.>.>...vv....>..v.vv>.......v..v..vvv.>v...vv>>..vv...>v..v.v.>.v.
.>..v>.v>..>>.v..>.>..>...vvv..>.v..vv.>>v..v>>v.>.v.>>>......v.v>>v..v>...>.v..v.....v....>>.>v........v>vv>..v....>>.v.vv>.v...v.>.>>.v..
..>..v>...v.vv.>.v>>.v.v.vv...>...>.vv..>.>..v.v>v..v.v...v.>.>...v..>>v>.v>.v.>v>...v>>...>>..>.v>..vv>v>v.>v.v..>..v.v>v.v...vvvvv.v.v>.>
vv>>v..v..>>v.>..v..vv>>.>...>>...v..>.>>.>vv.....v......v...v>.vv>..>v..vvvvv.v.....>>v.v.>>.v..v.v>..>v.>v>>>........v.v>.....vv.>.v.>...
.>..vv>...vv>>v>..>vv.v...>.....vvv>..vv>.>..v.vv....>.....>..vv.>..v...vv.>..>....>v>vv>>>.>..>v>vvvv..>>>v..>..v........v...>>>vvv.v..v>.
.>....v>v.>.vv>>>v>....>.v>.v>.>.>.v>v.>>..v.>.....v.>>>>..vv>.v.vv..>v>>.>>.v>>.v>v......>.>v..v.v.>..>...v.>..>..v.....v>.....>.vv.>..>v>
.>.....v..v...vv>....>>>.v..>>>v..>.v.v.>.>>...vv>>v......>>.v..>>.v>.v..>.v>.....>v>>.>v.vv..>...v..v>.v..v>.v..v..>>>v>>..v>..v.vv>.>.v.>
.v......v.>vv>>v.......v..v.v...v>...v>v>.>>vvv.vvv>vvv.>.v>.>....v>v.v..>>..v>>>v......v....>v.v....v>.>.>>v.>.....v>v......v.>>.>.v.v>vvv
.vv.v..vv>...v.v..v.v....>....v.>..>>.v.>..vv..>>...v..>>.>>.v.>>.>>v>..v...>>.>..>..>vv>>>vv>..v>v..v.........v>v.>>.v.>>>.v>..>>v>.v..>.v
>.v>>>>>.vvv>>.v.vv>>>..v.v.v...>>>.>.v>...>.vvvv..>.>>.vv..v....v.>.>v>>..v.>vv...>.....>>>.>>...v..v..v>......v.>..vv.......v....v.....v.
.>.....v.>>.v..v..>..>v..>vv.v..v>v>>>.vvvv.>....v>v...>v.vv.>....>.vvvv>>v..>.>v.v>.v.>>...>..>.>>v.>..v>......>..v>>v..v..v>v.>.>.>v.>v>>
....vv..v.vv.v>..v.vv>>>.......vv....v>.vv>..vvv.vv.>>..v...>v>...>v.>..>.>.vv..>.>>.v.>.v>.>.>....v..>v.>...>v....v>>..v>vv..>>..>.>.....>
...>..v.vv>.vv.>..v>>v....vvv>.>.v.>.....>.v.>>>v>...vv...v>>......v.>.v.v.....v>.>>v>v.>.v.vv.>v>vv>..>>v..v...>>...v...vv.>>...>>.vv..>.>
>>.v.>v..v.>vv.>.>>...v.>.>.>.>>vvvv>>.>..v.v.....vv>v....>>..>.......v...v..>.v>..v....>>vv...vv>..v>v...v>vv....>>..>.>....v..>v>>v.....>
>.>.v>.vv..>.>>..v....v..>..v>v.v.>vv>..v.v.v>>>>..>vv>>>...v..v>..v..v.>..>..>.>>....v>vvvv...v>>.>v...vv.v.>.v>vv.>.vv.v....v.>..>>...>.v
v.>.>.vv>>v..>>..v..v..>.v..>v..>.v....>>..>...>.vvv....>v.v..v>>v.......v>vv>v>.v.v..v.v>.>>.v>.vv>.>v.vvv>>.>>v.vvvv.v.v>vvvvv.>>>vv>..>>
.vv..>>..>..v...>.vv...v.....v>>.v>>>....>v..vv.v..>..>....>.v.>..v..vvv>>v>....>..>.v>vvvv.>>>>vvv>>>v>v.>.v.v....>..>...v.v....v>>...>.>v
>.v..>.......v.>vv...>..v..v>>v...>..vv.>...v..vv>>.v>vv....v..vv..>v.....vv>...v.>v>....>v.v>..>.v.vvv>v>>>.>>>.>v...>>....vvv.v>.>.>v>>.>
>.v.>>....v......vv..>.>>>.>>...>>v..vv....>>.>v....v.>.>.>v.>>.v>....v...>>>..vv...>.v.vv.>..>v...>.>.>.....>....>>vv>>.v>>>...>.v.....>v.
..>>v...>>v.>.>>>.v..>>...>>.....vv.>>v.>..>>v>.vv..v..v.>.>.>v>.v>v>.vvvv>v>v..v>.v...>.v.v.vv>>v.v..v..vv.>vv>>.vv>...v>>.>.v>..v...v.>.>
>v..v.>..v...v....v.v.>>v.v>.>.>.>>..v...vv.v.vvv.vvv.v.v>.>..>.v..>v>.>.>...v.>>...>v>..>.vv.v.....>v...v..>..>.>...v.v.>..v.>.>vv...v.v>>
>.v..v.....>v>v.>.v....>...>....>v......>.v.>..v.v.....vv...v..>...v.>>.>..>...>.>..v>.>..>>v..>v.>>.>.vv.>v.>.>.>>..>>..v>.>>>.v..>v>>.v..
v>..>v>..v>.v>......v>v.v...>..>>..>.v>...v.>v.v..>..v>>.vv.>>..vv.>.>vv.>>>..>.>>v.vv....v>..>v>..>>vv>.vvv.v>vvvv..vv>.....>.>v>....>>>>.
..>v.>>>..vv.>>>...>......v...>..>vv>vv.>..>....vvvv...v.......vv.v>>>.>..vv>v>vvvvv>v.v...>..>.>......>....>v>..>.vv..>v.v...v>..v.>v.>>..
>.>>...v.>..vv.>.>..v.>>..>>.v...>.>v>>v.....vv.v>>>vv>..v>.>..vvv.vv..>..>.v.>v>.>.>>v..>.....>v.>v.>vv>.>.>>>.v..vv.......>.>v....vv.v.vv
v.>>vv>v>v.>.>>.v.v>vv>.v.>...>>....vvv.>v>..>..>...>>..>..>v>.>>.vvv.>...v>>.>..v.>v>..v.>.v..v.v..>..v.......v...vvv>>.>.vvv....>v.vv>v.>
>>>.v>.>v>>>>v>>..>...>v.>....>v.vv.>.>....>...v..v.>v........v>.>v>.>..v>>....vvv.vv.>vvv.v...vvv>.....v.......>..>>v.vv.>v...>.vv>>v.>vvv
.>....>.v.v...v..>v.v..v...v.v.>...>.v.v.v..v.vv..v..>.>......v>>v.v.v...>.vv.>>v.v.....>.v...>.v>>vv.>..>vv..>>.v....>.>.v..>>v.....>>>vv.
v..>v.v.v..>>....v....v.v>.v..>>.>..v>..vvvvvv>v>v...>....>.v>.>.v.>...>..v>>.>>>v.....>>>.v.>.>v>v>>..>..v..>v..>v>vv.v>v...v>..>v>>.v.>v.
....>v>...........v.>.v..>>v.>...>.>.vv..v>..v.v>...>.v>>v.>>>v>.v>..>..v.....v>.>>>.v..>>.>.v>v..v>....v>>v.>v.>v>vvv...v...v>.>..v.>..>>.
vv.>.v>v........vvv....>>.v.......>.>>..>>v.>v.v....>>v..>v..>>...>>..vv.>>>>.vv.....>>.v.>.v.>.>>.v..v.v..v.>.>v.v>>>vv.vv.>vv...v>>...vv>
>...vv....>>>.v>........v>vvv..v>>.>>>v.>v>>>..>>.>v.vv...vv.>v>v.>>.>v.>>.....vv.>.>.....>.>.........>v...>vv...>.v..>.v>.v>>..v....v.>v.v
>>vv>>>..>>vvv>v>..v>.>..>>>....v>>>.vv>.>..v...v..>.>v>>....v..v.v.v.vv.v>v..>vvv>>>v.>v..vv.>vvv..........>.vv..>>v>v.....vv>...>>.v..v..
v....>>..v....>v.>.>vvv.vvvv>..v.vv.....v.>.v>.>.>>v>..>.v.v...v>v>...v.>.>.v.v...>...vv.>vvv..v>....>v>>v....v>....v.>..>>.vv>v.v>.v>>>v.>
.>>.vv...vv.>v..>...v.>v>v>vv.>.>>...v..v...>>.v>.>.v..v.v>.v.>.v>v...vv>.>v....v>.>..v.v.>>.vv.>v.....>....>..v...v....>>.vv...>..>...v..>
v.>>.....>>>>..v.>>..v.....v>v.....v>.v.v>.v.......v.v.....v>v.vv..>..>v..>>.>..v>...>.>.v.v>>.v.>....>>....v.vv.v...v>...v>v.v..v...>v.>.>
.vv>>vvv...>>v..v>v..>>.v>v...>..v..v..>v.v..vvv....>...v.v.>.v>....>...>>v>..>...v.vv>..v>..>...>v>>>.v>........>.>.>.....>>v..v.vv>>...v.
..v>>.>.vv>>....v.v>v.v>.v...vvv.v...v....>.v....>..>v>>.vvv.v>.>.>>.v.v.>..>v.v.v>vvvv>v>v.v>>>v.>......>....v>..>v>..v..v....>v.>.v..v>.>
.>...v.>v>v.v>..>.v>>...>>.>.v.vv>..v.>.....vv>..>>>.>v....v..v..v...>...v.v.v.v>..v..v>.>>.v...>.>..v...>v..v>.>....v....>..v.vv.>..vv.>.v
..v.v>......>>v>v>.v>v>v>>v..>..vvv..v...>vv>.vvv.vv...vv...>.>>.>v....v.>v.v.v.v>vv.>>v.>>vv...v.>..vv>..vvv.>.....>..v..v.>vv.v....>.>.vv
v>>v..v>v..vvvvv.>v.>....v..v..>>>v....v>.>v>v......>...>>.v.vv..vv.>vvvv...>v>>.>..v>>.v>...>vv..vvv.>>v..v>..>v...>.>>..>>.>>.>v.v..>....
>>>.vvvv.>.v..v.>.v>>.v...>v>vv.v...v..v......>v...vv.v>.v..>..v.>vvv.>vv..>>>.v....v.v...v.vv.vv.v..>.>vvvv...vv..>....v..>...vv>..v>..v>.
..v...v.v>v>.>v>.>..v..>v>..v>..>>...v..>.>.v.>.>vv....>>>>>>..>>>vv..vv>vvv.vv>vvvv.>v...>...v...>v>.....v.v>..vv>..>v.v.vv.vv>>.vv..>>..>
.>.v..>v.v.v...v>>.>....vv.....vv.>...>..v>vv>v>..>..>v.vv>>.v>.v.>vv>>>..>>v.>..>.vv..>.>..vvv..v.>........>v>v>>>v.v.vvv...v..vv>.>>.v>..
v>v>vv>>>.v.v..v>.>..vv.v.>vv.>.......v>v..>>...v>...v.>vvv>vv.v....>>v.v>>>.v>.>v.>>.v>....v.v.>..>>..>>v..>vv...v..>.>>.>v>.>....v>vv.>v>";

			return ParseData(data);
		}

		private static SeaCucumber[,] ParseData(string data)
		{
			var rows = data.Split("\r\n");

			var dataToReturn = new SeaCucumber[rows[0].Length, rows.Length];

			for (int y = 0; y < rows.Length; y++)
			{
				var row = rows[y].ToCharArray();

				for (int x = 0; x < row.Length; x++)
				{
					char character = row[x];

					if (character == '>')
					{
						dataToReturn[x, y] = new SeaCucumber(true);
					}
					else if (character == 'v')
					{
						dataToReturn[x, y] = new SeaCucumber(false);
					}
				}
			}

			return dataToReturn;
		}
	}

	public class SeaCucumber
	{
		public bool IsMovingEast { get; private set; }
		public bool IsMovingSouth => !IsMovingEast;

		public bool CanMove { get; set; }

		public SeaCucumber(bool isMovingEast)
		{
			IsMovingEast = isMovingEast;
		}
	}
}
