using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day14
	{
		public static long Run()
		{
			bool isTest = false;

			string template = isTest ? "NNCB" : "CKFFSCFSCBCKBPBCSPKP";
			var data = isTest ? GetTestData() : GetData();

			//var result = RunPart1(template, data); // test: 1588, 3247
			var result = RunPart2(template, data); // test: 2188189693529, 4110568157153

			return result;
		}

		private static long RunPart2(string template, Dictionary<string, char> data)
		{
			return RunAttempt2(template, data, 40);
		}

		private static long RunPart1(string template, Dictionary<string, char> data)
		{
			return RunAttempt2(template, data, 10);
		}

		// This did not scale well. See RunAttempt2
		private static int Run(string template, Dictionary<string, char> data, int numSteps)
		{
			char[] polymer = template.ToCharArray();

			// Update the polymer template *numSteps* times
			for (int step = 0; step < numSteps; step++)
			{
				// Create a new array to store the updated polymer in
				int newLength = polymer.Length * 2 - 1;
				char[] updatedPolymer = new char[newLength];

				// Keep track of the updatedPolymer's index
				int j = 0;

				// Loop over each pair of the current polymer's characters
				for (int i = 0; i < polymer.Length - 1; i++)
				{
					char firstCharInPair = polymer[i];
					char secondCharInPair = polymer[i + 1];
					string pair = firstCharInPair.ToString() + secondCharInPair;
					char charToInsertBetweenPair = data[pair];

					if (i == 0)
					{
						updatedPolymer[j++] = firstCharInPair;
					}

					updatedPolymer[j++] = charToInsertBetweenPair;
					updatedPolymer[j++] = secondCharInPair;
				}

				// Reassign the updated polymer to the original polymer variable
				polymer = updatedPolymer;
			}

			// Add up the number of times each element appears in the final polymer
			Dictionary<char, int> elementCounts = new Dictionary<char, int>();
			for (int i = 0; i < polymer.Length; i++)
			{
				char element = polymer[i];

				if (!elementCounts.ContainsKey(element))
				{
					elementCounts.Add(element, 1);
				}
				else
				{
					elementCounts[element]++;
				}
			}

			// Return the count of the most common element minus the count of the least common
			int max = elementCounts.Max(ec => ec.Value);
			int min = elementCounts.Min(ec => ec.Value);
			return max - min;
		}

		private static long RunAttempt2(string template, Dictionary<string, char> data, int numSteps)
		{
			char[] polymer = template.ToCharArray();
			Dictionary<string, long> pairCounts = new Dictionary<string, long>();

			// Loop over each pair of the current polymer's characters to get the starting pairs
			for (int i = 0; i < polymer.Length - 1; i++)
			{
				char firstCharInPair = polymer[i];
				char secondCharInPair = polymer[i + 1];
				string pair = firstCharInPair.ToString() + secondCharInPair;

				if (!pairCounts.ContainsKey(pair))
				{
					pairCounts.Add(pair, 1);
				}
				else
				{
					pairCounts[pair]++;
				}
			}

			// For each step, replace all current pairs with two new pairs
			for (int step = 0; step < numSteps; step++)
			{
				Dictionary<string, long> updatedPairCounts = new Dictionary<string, long>();
				
				// Get all pairs
				foreach (var pairCount in pairCounts)
				{
					string pair = pairCount.Key;
					char charToInsertBetweenPair = data[pair];
					string newPairOne = pair[0].ToString() + charToInsertBetweenPair;
					string newPairTwo = charToInsertBetweenPair + pair[1].ToString();
					
					if (!updatedPairCounts.ContainsKey(newPairOne))
					{
						updatedPairCounts.Add(newPairOne, pairCount.Value);
					}
					else
					{
						updatedPairCounts[newPairOne] += pairCount.Value;
					}

					if (!updatedPairCounts.ContainsKey(newPairTwo))
					{
						updatedPairCounts.Add(newPairTwo, pairCount.Value);
					}
					else
					{
						updatedPairCounts[newPairTwo] += pairCount.Value;
					}
				}

				// Reassign the updated pair counts to the original pair counts variable
				pairCounts = updatedPairCounts;
			}

			// Add up the number of times each element appears in the final polymer
			Dictionary<char, long> elementCounts = new Dictionary<char, long>();
			foreach (var pairCount in pairCounts)
			{
				// We only need the first character of each pair, as the second character will be counted as
				// the first character for another pair (except for the final character of the original polymer)
				char element = pairCount.Key[0];

				if (!elementCounts.ContainsKey(element))
				{
					elementCounts.Add(element, pairCount.Value);
				}
				else
				{
					elementCounts[element] += pairCount.Value;
				}
			}

			// Add the final character from the original polymer, as it hasn't been added yet
			char finalElement = polymer[polymer.Length - 1];
			elementCounts[finalElement]++;

			// Return the count of the most common element minus the count of the least common
			long max = elementCounts.Max(ec => ec.Value);
			long min = elementCounts.Min(ec => ec.Value);
			return max - min;
		}

		private static Dictionary<string, char> GetTestData()
		{
			string data =
@"CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";

			return ParseData(data);
		}

		private static Dictionary<string, char> GetData()
		{
			string data =
@"NS -> P
KV -> B
FV -> S
BB -> V
CF -> O
CK -> N
BC -> B
PV -> N
KO -> C
CO -> O
HP -> P
HO -> P
OV -> O
VO -> C
SP -> P
BV -> H
CB -> F
SF -> H
ON -> O
KK -> V
HC -> N
FH -> P
OO -> P
VC -> F
VP -> N
FO -> F
CP -> C
SV -> S
PF -> O
OF -> H
BN -> V
SC -> V
SB -> O
NC -> P
CN -> K
BP -> O
PC -> H
PS -> C
NB -> K
VB -> P
HS -> V
BO -> K
NV -> B
PK -> K
SN -> H
OB -> C
BK -> S
KH -> P
BS -> S
HV -> O
FN -> F
FS -> N
FP -> F
PO -> B
NP -> O
FF -> H
PN -> K
HF -> H
VK -> K
NF -> K
PP -> H
PH -> B
SK -> P
HN -> B
VS -> V
VN -> N
KB -> O
KC -> O
KP -> C
OS -> O
SO -> O
VH -> C
OK -> B
HH -> B
OC -> P
CV -> N
SH -> O
HK -> N
NO -> F
VF -> S
NN -> O
FK -> V
HB -> O
SS -> O
FB -> B
KS -> O
CC -> S
KF -> V
VV -> S
OP -> H
KN -> F
CS -> H
CH -> P
BF -> F
NH -> O
NK -> C
OH -> C
BH -> O
FC -> V
PB -> B";

			return ParseData(data);
		}

		private static Dictionary<string, char> ParseData(string data)
		{
			var rows = data.Split("\r\n");

			var dataToReturn = new Dictionary<string, char>();

			foreach (string row in rows)
			{
				string[] parts = row.Split(" -> ");
				dataToReturn.Add(parts[0], parts[1][0]);
			}

			return dataToReturn;
		}
	}
}
