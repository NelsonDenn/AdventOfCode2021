using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day1
{
	class Day1Part1
	{
		public static int Run()
		{
			var data = ImportData();
			//var result = RunPart1(data);
			var result = RunPart2(data);

			return result;
		}

		public static int RunPart2(List<int> data)
		{
			int count = 0;
			int[] previousThreeValues = new int[3];

			foreach (int dataPoint in data)
			{
				// Don't check if we don't have three previous values yet
				bool doCheck = !previousThreeValues.Any(p => p == 0);

				int previousThreeValuesSum = previousThreeValues.Sum();

				previousThreeValues[0] = previousThreeValues[1];
				previousThreeValues[1] = previousThreeValues[2];
				previousThreeValues[2] = dataPoint;

				int currentThreeValuesSum = previousThreeValues.Sum();

				if (doCheck && currentThreeValuesSum > previousThreeValuesSum)
				{
					count++;
				}
			}

			return count;
		}

		public static int RunPart1(List<int> data)
		{
			int count = 0;
			int previousDataPoint = -1;

			foreach (int dataPoint in data)
			{
				if (previousDataPoint != -1 && dataPoint > previousDataPoint)
				{
					count++;
				}

				previousDataPoint = dataPoint;
			}

			return count;
		}

		public static List<int> ImportData()
		{
			var lines = File.ReadAllLines("Day1/data.csv");
			List<int> data = lines.Select(s => int.Parse(s)).ToList();
			return data;
		}
	}
}
