using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day24
	{
		public static int Run()
		{
			bool isTest = true;
			var data = isTest ? GetTestData() : GetData();

			var result = RunPart1(data);
			//var result = RunPart2(data);

			return result;
		}

		private static int RunPart2(List<string> data)
		{
			return 0;
		}

		private static int RunPart1(List<string> data)
		{
			return 0;
		}

		private static List<string> GetTestData()
		{
			string data =
@"2199943210
3987894921
9856789892
8767896789
9899965678";

			return ParseData(data);
		}

		private static List<string> GetData()
		{
			//var data = new List<int>
			//{
			//	1,4,3,3,1,3,1,1,1,2,1,1,1,4,4,1,5,5,3,1,3,5,2,1,5,2,4,1,4,5,4,1,5,1,5,5,1,1,1,4,1,5,1,1,1,1,1,4,1,2,5,1,4,1,2,1,1,5,1,1,1,1,4,1,5,1,1,2,1,4,5,1,2,1,2,2,1,1,1,1,1,5,5,3,1,1,1,1,1,4,2,4,1,2,1,4,2,3,1,4,5,3,3,2,1,1,5,4,1,1,1,2,1,1,5,4,5,1,3,1,1,1,1,1,1,2,1,3,1,2,1,1,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,4,5,1,3,1,4,4,2,3,4,1,1,1,5,1,1,1,4,1,5,4,3,1,5,1,1,1,1,1,5,4,1,1,1,4,3,1,3,3,1,3,2,1,1,3,1,1,4,5,1,1,1,1,1,3,1,4,1,3,1,5,4,5,1,1,5,1,1,4,1,1,1,3,1,1,4,2,3,1,1,1,1,2,4,1,1,1,1,1,2,3,1,5,5,1,4,1,1,1,1,3,3,1,4,1,2,1,3,1,1,1,3,2,2,1,5,1,1,3,2,1,1,5,1,1,1,1,1,1,1,1,1,1,2,5,1,1,1,1,3,1,1,1,1,1,1,1,1,5,5,1
			//};

			string data =
@"2199943210
3987894921
9856789892
8767896789
9899965678";

			return ParseData(data);
		}

		private static List<string> ParseData(string data)
		{
			var rows = data.Split("\r\n");

			var dataToReturn = new List<string>();

			foreach (string row in rows)
			{
				dataToReturn.Add(row);
			}

			return dataToReturn;
		}
	}
}
