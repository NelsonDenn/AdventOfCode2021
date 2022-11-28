using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day16
	{
		public static long Run()
		{
			var bits = GetData();

			//var result = RunPart1(bits); // 913
			var result = RunPart2(bits); // 1510977819698

			return result;
		}

		private static long RunPart2(string bits)
		{
			var packet = new Packet(bits);
			return packet.GetValue();
		}

		private static int RunPart1(string bits)
		{
			var packet = new Packet(bits);
			return packet.PacketVersionSum;
		}

		private static string GetData()
		{
			// Example data
			//string data = "D2FE28"; // literal value packet
			//string data = "38006F45291200"; // operator packet with length type ID 0
			//string data = "EE00D40C823060"; // operator packet with length type ID 1
			//string data = "8A004A801A8002F478"; // 16
			//string data = "620080001611562C8802118E34"; // 12
			//string data = "C0015000016115A2E0802F182340"; // 23
			//string data = "A0016C880162017C3686B18A3D4780"; // 31

			// Input data
			string data = "E058F79802FA00A4C1C496E5C738D860094BDF5F3ED004277DD87BB36C8EA800BDC3891D4AFA212012B64FE21801AB80021712E3CC771006A3E47B8811E4C01900043A1D41686E200DC4B8DB06C001098411C22B30085B2D6B743A6277CF719B28C9EA11AEABB6D200C9E6C6F801F493C7FE13278FFC26467C869BC802839E489C19934D935C984B88460085002F931F7D978740668A8C0139279C00D40401E8D1082318002111CE0F460500BE462F3350CD20AF339A7BB4599DA7B755B9E6B6007D25E87F3D2977543F00016A2DCB029009193D6842A754015CCAF652D6609D2F1EE27B28200C0A4B1DFCC9AC0109F82C4FC17880485E00D4C0010F8D110E118803F0DA1845A932B82E200D41E94AD7977699FED38C0169DD53B986BEE7E00A49A2CE554A73D5A6ED2F64B4804419508B00584019877142180803715224C613009E795E58FA45EA7C04C012D004E7E3FE64C27E3FE64C24FA5D331CFB024E0064DEEB49D0CC401A2004363AC6C8344008641B8351B08010882917E3D1801D2C7CA0124AE32DD3DDE86CF52BBFAAC2420099AC01496269FD65FA583A5A9ECD781A20094CE10A73F5F4EB450200D326D270021A9F8A349F7F897E85A4020CF802F238AEAA8D22D1397BF27A97FD220898600C4926CBAFCD1180087738FD353ECB7FDE94A6FBCAA0C3794875708032D8A1A0084AE378B994AE378B9A8007CD370A6F36C17C9BFCAEF18A73B2028C0A004CBC7D695773FAF1006E52539D2CFD800D24B577E1398C259802D3D23AB00540010A8611260D0002130D23645D3004A6791F22D802931FA4E46B31FA4E4686004A8014805AE0801AC050C38010600580109EC03CC200DD40031F100B166005200898A00690061860072801CE007B001573B5493004248EA553E462EC401A64EE2F6C7E23740094C952AFF031401A95A7192475CACF5E3F988E29627600E724DBA14CBE710C2C4E72302C91D12B0063F2BBFFC6A586A763B89C4DC9A0";

			return ParseData(data);
		}

		//private static string ParseData(string data)
		//{
		//	// We don't want to lose any starting zeroes
		//	string leftPad = "";
		//	if (int.TryParse(data.Substring(0, 1), out int digit))
		//	{
		//		leftPad = digit <= 3 ? "00" : digit <= 8 ? "0" : "";
		//	}

		//	return leftPad + Convert.ToString(Convert.ToInt64(data, 16), 2);
		//}

		private static string ParseData(string data)
		{
			string binary = "";
			foreach (char hexCharacter in data.ToCharArray())
			{
				binary += Convert.ToString(Convert.ToInt64(hexCharacter.ToString(), 16), 2).PadLeft(4, '0');
			}

			return binary;
		}
	}

	public class Packet
	{
		public int PacketVersion { get; private set; }
		public int PacketTypeId { get; private set; }
		public long LiteralValue { get; private set; }
		public bool IsLiteralValuePacket => LiteralValue > 0;
		private int _literalValuePacketLength;
		private int _operatorPacketHeaderLength;
		public int PacketLength => IsLiteralValuePacket ? _literalValuePacketLength : _operatorPacketHeaderLength + SubPackets.Sum(p => p.PacketLength);
		public List<Packet> SubPackets { get; } = new List<Packet>();

		public int PacketVersionSum => PacketVersion + SubPackets.Sum(p => p.PacketVersionSum);

		public Packet(string bits)
		{
			// The first three bits encode the packet version
			PacketVersion = (int)ConvertBinaryToDecimal(bits.Substring(0, 3));

			// The next three bits encode the packet type ID
			PacketTypeId = (int)ConvertBinaryToDecimal(bits.Substring(3, 3));

			int currentBit = 6;

			if (PacketTypeId == 4)
			{
				// This packet represents a literal value
				string literalValueBits = "";
				bool keepReading = true;
				while (keepReading)
				{
					string fiveBits = bits.Substring(currentBit, 5);

					// If the first bit is a 0, then this is the last group to read
					keepReading = fiveBits[0] == '1';

					// Add the last four bits of the five-bit group to our literal value bits collection
					literalValueBits += fiveBits.Substring(1, 4);

					currentBit += 5;
				}

				LiteralValue = ConvertBinaryToDecimal(literalValueBits);
				_literalValuePacketLength = currentBit;
			}
			else
			{
				// This packet represents an operator
				int lengthTypeId = bits[currentBit++] - '0'; // Indicates how many bits to read next

				if (lengthTypeId == 0)
				{
					// The next 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet
					int totalLengthOfSubPackets = (int)ConvertBinaryToDecimal(bits.Substring(currentBit, 15));
					currentBit += 15;
					_operatorPacketHeaderLength = currentBit;

					// Loop over the remaining bits within this operator packet and get all subpackets
					while (currentBit < _operatorPacketHeaderLength + totalLengthOfSubPackets)
					{
						string remainingBits = bits.Substring(currentBit, totalLengthOfSubPackets - SubPackets.Sum(p => p.PacketLength));

						// Get the next subpacket from the remaining bits
						var subpacket = new Packet(remainingBits);
						currentBit += subpacket.PacketLength;
						SubPackets.Add(subpacket);
					}
				}
				else
				{
					// The next 11 bits are a number that represents the number of sub-packets immediately contained by this packet
					int numberSubPackets = (int)ConvertBinaryToDecimal(bits.Substring(currentBit, 11));
					currentBit += 11;
					_operatorPacketHeaderLength = currentBit;

					// Loop over the remaining bits within this operator packet and get all subpackets
					for (int i = 0; i < numberSubPackets; i++)
					{
						string remainingBits = bits.Substring(currentBit);

						// Get the next subpacket from the remaining bits
						var subpacket = new Packet(remainingBits);
						currentBit += subpacket.PacketLength;
						SubPackets.Add(subpacket);
					}
				}
			}
		}

		public long GetValue()
		{
			switch (PacketTypeId)
			{
				case 0:
					// Sum packet
					return SubPackets.Sum(p => p.GetValue());
				case 1:
					// Product packet
					return SubPackets.Select(p => p.GetValue()).Aggregate((product, next) => product * next);
				case 2:
					// Minimum packet
					return SubPackets.Min(p => p.GetValue());
				case 3:
					// Maximum packet
					return SubPackets.Max(p => p.GetValue());
				case 4:
					// Literal value packet
					return LiteralValue;
				case 5:
					// Greater than packet
					return SubPackets[0].GetValue() > SubPackets[1].GetValue() ? 1 : 0;
				case 6:
					// Less than packet
					return SubPackets[0].GetValue() < SubPackets[1].GetValue() ? 1 : 0;
				case 7:
					// Equal to packet
					return SubPackets[0].GetValue() == SubPackets[1].GetValue() ? 1 : 0;
				default:
					throw new Exception("This shouldn't happen");
			}
		}

		private static long ConvertBinaryToDecimal(string bits)
		{
			return Convert.ToInt64(Convert.ToString(Convert.ToInt64(bits, 2), 10));
		}
	}
}
