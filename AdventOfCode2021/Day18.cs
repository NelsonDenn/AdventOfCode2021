using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day18
	{
		public static int Run()
		{
			//var data = GetTestData();
			var data = GetData();

			//var result = RunPart1(data); // test: 4140, 4057
			var result = RunPart2(data); // test: 3993, 4683

			return result;
		}

		private static int RunPart2(List<SnailfishNumber> numbers)
		{
			int largestMagnitude = 0;

			foreach (var number in numbers)
			{
				foreach (var otherNumber in numbers)
				{
					if (number != otherNumber)
					{
						var sum = number.Clone(null).Add(otherNumber.Clone(null));
						largestMagnitude = Math.Max(largestMagnitude, sum.GetMagnitude());
					}
				}
			}

			return largestMagnitude;
		}

		private static int RunPart1(List<SnailfishNumber> numbers)
		{
			SnailfishNumber sum = null;

			foreach (var number in numbers)
			{
				sum = sum?.Add(number) ?? number;
				//sum = sum?.Add(number.Clone(null)) ?? number.Clone(null);
			}

			return sum.GetMagnitude();
		}

		private static List<SnailfishNumber> GetTestData()
		{
//			string data =
//@"[1,1]
//[2,2]
//[3,3]
//[4,4]
//[5,5]";

//			string data =
//@"[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]";

//			string data =
//@"[[[[4,3],4],4],[7,[[8,4],9]]]
//[1,1]";

			string data =
@"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";

			return ParseData(data);
		}

		private static List<SnailfishNumber> GetData()
		{
			string data =
@"[[[7,1],2],3]
[[1,7],7]
[[6,8],[[6,[3,6]],[0,5]]]
[[[[2,1],8],[[9,4],8]],[[6,5],4]]
[[1,[[3,8],[9,1]]],[[9,1],[[1,7],0]]]
[[[7,4],[8,[7,6]]],[9,[[6,3],[7,8]]]]
[[[[5,0],1],4],[[5,[6,9]],[[4,3],2]]]
[[[3,8],8],[[[3,2],8],[9,[0,5]]]]
[[[[5,8],[3,9]],[7,[1,4]]],[6,1]]
[3,[[[3,3],9],[0,7]]]
[[[6,9],1],[[0,[8,4]],[[2,2],9]]]
[[[[6,2],3],[0,4]],3]
[[[[3,8],7],[[7,4],0]],[2,[5,[2,8]]]]
[[4,[9,[8,0]]],[[1,5],[[9,3],8]]]
[[[8,5],[3,[1,4]]],[[6,[8,0]],[[2,7],[2,6]]]]
[4,7]
[[[[2,3],0],[[1,9],[4,1]]],[[1,[4,2]],3]]
[[[8,[5,3]],[[5,7],7]],[[5,6],[6,4]]]
[[[[2,4],1],[8,6]],[[6,5],[0,[9,1]]]]
[[[1,[5,7]],8],[[[9,1],9],[[1,2],4]]]
[[[[5,5],[4,0]],[4,[9,6]]],[[[2,1],1],7]]
[[[[1,9],[9,5]],[[5,0],[3,1]]],[[[6,7],[8,8]],[[7,3],0]]]
[[6,[[6,7],[9,0]]],[[7,7],[[0,3],0]]]
[[0,6],[5,2]]
[[[[5,8],3],[[9,0],8]],[7,4]]
[[0,[[9,9],[9,4]]],[[[1,1],2],[1,[6,7]]]]
[0,[[5,7],2]]
[[2,[[5,4],6]],[1,[8,[7,6]]]]
[[[1,7],[8,[5,8]]],[[[2,1],[9,1]],[[5,6],9]]]
[[1,8],[9,[4,3]]]
[5,[2,[[5,5],9]]]
[3,[8,[[2,8],[4,8]]]]
[[[4,9],[[5,5],0]],[9,[8,[3,0]]]]
[[[2,[6,4]],[8,[9,9]]],[[[0,4],8],[3,[9,7]]]]
[[[[8,1],[2,4]],3],[1,[[3,3],[6,3]]]]
[[[8,[7,3]],[1,8]],2]
[[8,[8,4]],[[6,[4,7]],[3,0]]]
[[[[4,6],[8,3]],9],[9,[[8,9],[0,9]]]]
[[3,[[2,7],[4,4]]],2]
[8,[[[8,6],2],[[8,9],6]]]
[[[[5,7],[2,0]],[[0,2],[5,5]]],[[[8,5],5],[[1,3],[2,3]]]]
[[1,6],[[9,8],[9,[4,9]]]]
[[[[1,4],5],9],[4,[6,8]]]
[[[[6,4],[9,0]],[[1,4],[6,6]]],[[9,[2,8]],2]]
[[[[5,9],2],[[0,0],5]],[2,1]]
[6,[[3,2],[[3,0],0]]]
[[[[7,4],1],[[4,1],1]],[[3,4],4]]
[3,[9,[9,7]]]
[[[3,[3,3]],[0,3]],[1,[1,8]]]
[[8,[8,7]],[[9,2],5]]
[[[1,[3,9]],[5,9]],[1,5]]
[[[[7,8],[9,7]],9],[[[9,2],[2,2]],[[9,6],8]]]
[4,[[3,5],[[1,3],[5,5]]]]
[7,[[[0,1],2],[[3,6],5]]]
[0,[[[2,4],[3,4]],[8,9]]]
[[1,[[6,8],1]],[8,0]]
[1,1]
[7,0]
[[1,2],[[0,[8,3]],[[4,5],[9,7]]]]
[[[[2,3],[5,9]],[7,[1,9]]],2]
[[3,5],[[9,7],9]]
[[[[6,9],[4,8]],6],0]
[[[[2,4],[3,9]],[2,[9,4]]],[[[8,9],[3,1]],7]]
[[5,[[0,2],4]],[[[9,9],[7,4]],[1,5]]]
[3,[6,[[5,4],1]]]
[[[2,[2,7]],2],[[4,[7,3]],5]]
[7,[[0,[2,0]],[[9,4],6]]]
[[4,[3,[6,2]]],9]
[[[0,[5,6]],[8,3]],[[7,9],[0,[9,6]]]]
[8,[[6,4],[4,8]]]
[[[8,[6,8]],[5,[7,3]]],[[[7,8],5],2]]
[[[[3,5],[4,7]],5],[[0,0],[9,[1,9]]]]
[[7,[[1,5],9]],[[[3,4],[1,7]],[1,[7,9]]]]
[[0,[3,[4,1]]],[[[2,9],3],[4,[0,8]]]]
[[[8,[1,6]],[[0,1],7]],[[[1,1],[0,2]],[[9,4],[9,6]]]]
[[[[6,7],0],[[6,8],9]],[[1,[6,6]],[[2,9],[4,7]]]]
[[[[5,0],[1,2]],[1,[5,1]]],[[0,4],1]]
[[9,1],6]
[[7,2],[[[5,5],[4,3]],6]]
[[9,[[0,6],9]],[[7,9],[7,1]]]
[[[[7,3],[6,4]],[[2,5],[7,2]]],[[[4,4],0],[[9,5],[8,5]]]]
[[[[8,8],[6,4]],[[0,2],[9,5]]],2]
[[[[3,0],7],[9,2]],[[0,[8,6]],[[7,2],[8,5]]]]
[[0,6],[1,[9,[4,3]]]]
[[0,8],[[[5,0],6],[5,[2,0]]]]
[[[[7,1],[0,3]],[[9,9],[3,5]]],[4,[8,4]]]
[7,[[1,[3,7]],[[3,4],[2,3]]]]
[[[[2,2],[4,8]],[[3,4],0]],[[[1,5],[2,8]],5]]
[6,[[[9,1],5],[9,9]]]
[[[2,[8,6]],[[9,9],[6,3]]],4]
[[[[3,2],[9,3]],8],9]
[[[[6,9],0],[[0,6],[1,3]]],[[5,[9,8]],[[1,5],[3,7]]]]
[[2,[4,[2,3]]],[[[6,0],[7,2]],3]]
[[[[8,3],4],[6,[8,8]]],4]
[[[9,8],5],[[[4,4],[6,3]],[8,6]]]
[9,2]
[[[3,4],[4,[7,0]]],[0,[4,[6,9]]]]
[[[0,8],[3,9]],[[[3,8],6],[[9,3],6]]]
[[[[5,6],[0,3]],1],[8,[2,9]]]
[[[[4,2],8],[[9,3],7]],0]";

			return ParseData(data);
		}

		private static List<SnailfishNumber> ParseData(string data)
		{
			var rows = data.Split("\r\n");

			var dataToReturn = new List<SnailfishNumber>();

			foreach (string row in rows)
			{
				SnailfishNumber current = new SnailfishNumber(null);

				for (int i = 0; i < row.Length; i++)
				{
					char character = row[i];
					if (character == '[')
					{
						// Start a new SnailfishNumber and begin tracking it
						current.Left = new SnailfishNumber(current);
						current = current.Left;
					}
					else if (character == ']')
					{
						// Don't try to track the parent of the outermost SnailfishNumber, because it doesn't have a parent!
						if (i != row.Length - 1)
						{
							// End of the current SnailfishNumber. Move up to tracking the parent again.
							current = current.Parent;
						}
					}
					else if (character == ',')
					{
						current.Right = new SnailfishNumber(current);
						current = current.Right;
					}
					else
					{
						// We're already on the right side of a pair
						// Assign this value to the current SnailfishNumber and begin tracking the parent SnailfishNumber again
						if (current.Parent != null && current == current.Parent.Right)
						{
							current.Value = character - '0';
							current = current.Parent;
						}
						else
						{
							// If we're not on the right side, then we must be on the left
							current.Value = character - '0';
							current = current.Parent;
						}
					}
				}

				dataToReturn.Add(current);
			}

			foreach (var snailfishNumber in dataToReturn)
			{
				Console.WriteLine(snailfishNumber);
			}

			Console.WriteLine();
			Console.WriteLine();

			return dataToReturn;
		}
	}

	public class SnailfishNumber
	{
		public SnailfishNumber Parent { get; set; }
		public SnailfishNumber Left { get; set; }
		public SnailfishNumber Right { get; set; }
		public int? Value { get; set; }

		public int ParentCount => Parent == null ? 0 : 1 + Parent.ParentCount;
		public bool HasBeenChecked { get; private set; }
		public bool IsParentsLeft => Parent?.Left == this;
		public bool IsParentsRight => Parent?.Right == this;

		public SnailfishNumber(SnailfishNumber parent)
		{
			Parent = parent;
		}

		public SnailfishNumber Clone(SnailfishNumber parent)
		{
			var clone = new SnailfishNumber(parent)
			{
				Value = Value
			};
			clone.Left = Left?.Clone(clone);
			clone.Right = Right?.Clone(clone);

			return clone;
		}

		public override string ToString()
		{
			return Value.HasValue ? Value.Value.ToString() : $"[{Left},{Right}]";
		}

		public SnailfishNumber Add(SnailfishNumber toAdd)
		{
			var sum = new SnailfishNumber(null)
			{
				Left = this,
				Right = toAdd
			};

			sum.Left.Parent = sum;
			sum.Right.Parent = sum;

			sum.Reduce();

			return sum;
		}

		private void Reduce()
		{
			// Repeatedly do the first action in this list that applies to the snailfish number:
			// If any pair is nested inside four pairs, the leftmost such pair explodes.
			// If any regular number is 10 or greater, the leftmost such regular number splits.
			while (true)
			{
				// First gather all pairs/numbers into a list so we can easily check them all
				var numbers = ToList();

				// If any pair is nested inside four pairs, the leftmost such pair explodes.
				var pairToExplode = numbers.Find(p => p.ParentCount > 3 && !p.Value.HasValue);
				if (pairToExplode != null)
				{
					Explode(pairToExplode);
					continue;
				}

				// If any regular number is 10 or greater, the leftmost such regular number splits.
				var numberToSplit = numbers.Find(p => p.Value.HasValue && p.Value > 9);
				if (numberToSplit != null)
				{
					// To split a regular number, replace it with a pair;
					// the left element of the pair should be the regular number divided
					// by two and rounded down, while the right element of the pair
					// should be the regular number divided by two and rounded up.
					numberToSplit.Left = new SnailfishNumber(numberToSplit)
					{
						Value = numberToSplit.Value.Value / 2 // ints automatically round down
					};
					numberToSplit.Right = new SnailfishNumber(numberToSplit)
					{
						Value = numberToSplit.Value.Value / 2 + (numberToSplit.Value.Value % 2) // Round up by one if value is odd
					};
					numberToSplit.Value = null;
					continue;
				}

				break;
			}
		}

		// Gather all SnailfishNumbers in a list from left to right
		private List<SnailfishNumber> ToList()
		{
			List<SnailfishNumber> numbers = new List<SnailfishNumber>();

			var current = this;
			while (current != null)
			{
				if (current.Left != null && !current.Left.HasBeenChecked)
				{
					current = current.Left;
				}
				else if (current.Right != null && !current.Right.HasBeenChecked)
				{
					current = current.Right;
				}
				else
				{
					// Done checking this pair and its children. Move back up to the parent
					current.HasBeenChecked = true;
					numbers.Add(current);
					current = current.Parent;
				}
			}

			// Reset the HasBeenChecked flag on each number
			numbers.ForEach(p => p.HasBeenChecked = false);

			return numbers;
		}

		// To explode a pair, the pair's left value is added to the first regular number
		// to the left of the exploding pair (if any), and the pair's right value is added
		// to the first regular number to the right of the exploding pair (if any).
		// Exploding pairs will always consist of two regular numbers.
		// Then, the entire exploding pair is replaced with the regular number 0.
		private void Explode(SnailfishNumber pairToExplode)
		{
			UpdateValueToLeft(pairToExplode);
			UpdateValueToRight(pairToExplode);

			pairToExplode.Left = null;
			pairToExplode.Right = null;
			pairToExplode.Value = 0;
		}

		// Finds the first regular number to the left of the pair to explode, and
		// increases its value by the pair to explode's left value
		private void UpdateValueToLeft(SnailfishNumber pairToExplode)
		{
			var parent = pairToExplode.Parent;
			var current = pairToExplode;

			// Find the first parent pair that the current pair is on the right of
			while (current.IsParentsLeft)
			{
				if (parent.Parent == null)
				{
					// No further up the tree to move. There are no values to the left of this pair
					return;
				}

				// Move up the tree
				current = parent;
				parent = parent.Parent;
			}

			// Start traversing down the parent and find the rightmost value
			current = parent.Left;

			while (!current.Value.HasValue)
			{
				current = current.Right;
			}

			// Make the update
			current.Value += pairToExplode.Left.Value;
		}

		// Finds the first regular number to the right of the pair to explode, and
		// increases its value by the pair to explode's right value
		private void UpdateValueToRight(SnailfishNumber pairToExplode)
		{
			var parent = pairToExplode.Parent;
			var current = pairToExplode;

			// Find the first parent pair that the current pair is on the left of
			while (current.IsParentsRight)
			{
				if (parent.Parent == null)
				{
					// No further up the tree to move. There are no values to the right of this pair
					return;
				}

				// Move up the tree
				current = parent;
				parent = parent.Parent;
			}

			// Start traversing down the parent and find the leftmost value
			current = parent.Right;

			while (!current.Value.HasValue)
			{
				current = current.Left;
			}

			// Make the update
			current.Value += pairToExplode.Right.Value;
		}

		// The magnitude of a pair is 3 times the magnitude of its left element plus
		// 2 times the magnitude of its right element. The magnitude of a regular number is just that number.
		public int GetMagnitude()
		{
			var numbers = ToList();

			// Continously combine pairs of regular numbers until we have just one regular number left
			while (!Value.HasValue)
			{
				var pairToCombine = numbers.Find(p => !p.Value.HasValue && p.Left.Value.HasValue && p.Right.Value.HasValue);
				pairToCombine.Value = (3 * pairToCombine.Left.Value) + (2 * pairToCombine.Right.Value);
				pairToCombine.Left = null;
				pairToCombine.Right = null;
			}

			return Value.Value;
		}
	}
}
