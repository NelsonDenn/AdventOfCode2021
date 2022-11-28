﻿using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day22
	{
		public static long Run()
		{
			bool isTest = false;
			var data = isTest ? GetTestData() : GetData();

			//var result = RunPart1(data); // test: 590784, test2: 474140, 647076
			var result = RunPart2(data); // test: , test2: 2758514936282235, 1233304599156793

			return result;
		}

		private static long RunPart2(List<ReactorRebootStep> steps)
		{
			// if a new ON step overlaps with an existing ON step, merge the steps (how?)
			// if a new OFF step overlaps with an existing ON step, split the existing ON step and reduce the ranges
			var finalSteps = new List<ReactorRebootStep>();

			foreach (var step in steps)
			{
				// For now, we are skipping values lower than -50 and higher than 50
				//if (step.MinX < -50 || step.MaxX > 50)
				//{
				//	continue;
				//}

				// Keep track of a list of steps to check, starting with this step, which may split into multiple steps to check
				var stepsToCheck = new List<ReactorRebootStep>() { step };

				while (stepsToCheck.Any())
				{
					var stepToCheck = stepsToCheck.First();

					// While there are any existing final steps that overlap with this step
					// Find an existing final step that overlaps this step
					var overlappingStep = finalSteps.FirstOrDefault(s => s.DoesOverlap(stepToCheck));

					if (overlappingStep == null)
					{
						// This step doesn't overlap with any existing final steps, so it's clear to be added to the final list of steps
						if (stepToCheck.On)
						{
							finalSteps.Add(stepToCheck);
						}
						stepsToCheck.Remove(stepToCheck);
						continue;
					}

					// Remove cubes from the current step that are already covered by the overlapping step
					// Note: the overlapping step must be ON by design
					if (stepToCheck.On)
					{
						var splitSteps = stepToCheck.SubtractOverlap(overlappingStep);
						stepsToCheck.AddRange(splitSteps);
						stepsToCheck.Remove(stepToCheck);
					}
					else
					{
						var splitSteps = overlappingStep.SubtractOverlap(step);
						finalSteps.Remove(overlappingStep);
						finalSteps.AddRange(splitSteps);
					}
				}
			}

			// Calculate all ON cubes
			long onCubeCount = 0;
			foreach (var step in finalSteps)
			{
				// Ranges include the end point, so +1
				long xRange = step.MaxX - step.MinX + 1;
				long yRange = step.MaxY - step.MinY + 1;
				long zRange = step.MaxZ - step.MinZ + 1;
				onCubeCount += xRange * yRange * zRange;
			}

			return onCubeCount;
		}

		private static long RunPart1(List<ReactorRebootStep> steps)
		{
			bool[,,] cubes = new bool[101, 101, 101];

			foreach (var step in steps)
			{
				// For now, we are skipping values lower than -50 and higher than 50
				if (step.MinX < -50 || step.MaxX > 50)
				{
					continue;
				}

				for (int x = step.MinX; x <= step.MaxX; x++)
				{
					for (int y = step.MinY; y <= step.MaxY; y++)
					{
						for (int z = step.MinZ; z <= step.MaxZ; z++)
						{
							// Translate -50 to 0, -49 to 1, ..., 50 to 100
							cubes[x + 50, y + 50, z + 50] = step.On;
						}
					}
				}
			}

			// Count all of the cubes that are on after following all of the steps
			long onCubeCount = 0;
			for (int x = 0; x < cubes.GetLength(0); x++)
			{
				for (int y = 0; y < cubes.GetLength(1); y++)
				{
					for (int z = 0; z < cubes.GetLength(2); z++)
					{
						if (cubes[x, y, z])
						{
							onCubeCount++;
						}
					}
				}
			}

			return onCubeCount;
		}

		private static List<ReactorRebootStep> GetTestData()
		{
			//			string data =
			//@"on x=-20..26,y=-36..17,z=-47..7
			//on x=-20..33,y=-21..23,z=-26..28
			//on x=-22..28,y=-29..23,z=-38..16
			//on x=-46..7,y=-6..46,z=-50..-1
			//on x=-49..1,y=-3..46,z=-24..28
			//on x=2..47,y=-22..22,z=-23..27
			//on x=-27..23,y=-28..26,z=-21..29
			//on x=-39..5,y=-6..47,z=-3..44
			//on x=-30..21,y=-8..43,z=-13..34
			//on x=-22..26,y=-27..20,z=-29..19
			//off x=-48..-32,y=26..41,z=-47..-37
			//on x=-12..35,y=6..50,z=-50..-2
			//off x=-48..-32,y=-32..-16,z=-15..-5
			//on x=-18..26,y=-33..15,z=-7..46
			//off x=-40..-22,y=-38..-28,z=23..41
			//on x=-16..35,y=-41..10,z=-47..6
			//off x=-32..-23,y=11..30,z=-14..3
			//on x=-49..-5,y=-3..45,z=-29..18
			//off x=18..30,y=-20..-8,z=-3..13
			//on x=-41..9,y=-7..43,z=-33..15
			//on x=-54112..-39298,y=-85059..-49293,z=-27449..7877
			//on x=967..23432,y=45373..81175,z=27513..53682";

			string data =
			@"on x=-5..47,y=-31..22,z=-19..33
on x=-44..5,y=-27..21,z=-14..35
on x=-49..-1,y=-11..42,z=-10..38
on x=-20..34,y=-40..6,z=-44..1
off x=26..39,y=40..50,z=-2..11
on x=-41..5,y=-41..6,z=-36..8
off x=-43..-33,y=-45..-28,z=7..25
on x=-33..15,y=-32..19,z=-34..11
off x=35..47,y=-46..-34,z=-11..5
on x=-14..36,y=-6..44,z=-16..29
on x=-57795..-6158,y=29564..72030,z=20435..90618
on x=36731..105352,y=-21140..28532,z=16094..90401
on x=30999..107136,y=-53464..15513,z=8553..71215
on x=13528..83982,y=-99403..-27377,z=-24141..23996
on x=-72682..-12347,y=18159..111354,z=7391..80950
on x=-1060..80757,y=-65301..-20884,z=-103788..-16709
on x=-83015..-9461,y=-72160..-8347,z=-81239..-26856
on x=-52752..22273,y=-49450..9096,z=54442..119054
on x=-29982..40483,y=-108474..-28371,z=-24328..38471
on x=-4958..62750,y=40422..118853,z=-7672..65583
on x=55694..108686,y=-43367..46958,z=-26781..48729
on x=-98497..-18186,y=-63569..3412,z=1232..88485
on x=-726..56291,y=-62629..13224,z=18033..85226
on x=-110886..-34664,y=-81338..-8658,z=8914..63723
on x=-55829..24974,y=-16897..54165,z=-121762..-28058
on x=-65152..-11147,y=22489..91432,z=-58782..1780
on x=-120100..-32970,y=-46592..27473,z=-11695..61039
on x=-18631..37533,y=-124565..-50804,z=-35667..28308
on x=-57817..18248,y=49321..117703,z=5745..55881
on x=14781..98692,y=-1341..70827,z=15753..70151
on x=-34419..55919,y=-19626..40991,z=39015..114138
on x=-60785..11593,y=-56135..2999,z=-95368..-26915
on x=-32178..58085,y=17647..101866,z=-91405..-8878
on x=-53655..12091,y=50097..105568,z=-75335..-4862
on x=-111166..-40997,y=-71714..2688,z=5609..50954
on x=-16602..70118,y=-98693..-44401,z=5197..76897
on x=16383..101554,y=4615..83635,z=-44907..18747
off x=-95822..-15171,y=-19987..48940,z=10804..104439
on x=-89813..-14614,y=16069..88491,z=-3297..45228
on x=41075..99376,y=-20427..49978,z=-52012..13762
on x=-21330..50085,y=-17944..62733,z=-112280..-30197
on x=-16478..35915,y=36008..118594,z=-7885..47086
off x=-98156..-27851,y=-49952..43171,z=-99005..-8456
off x=2032..69770,y=-71013..4824,z=7471..94418
on x=43670..120875,y=-42068..12382,z=-24787..38892
off x=37514..111226,y=-45862..25743,z=-16714..54663
off x=25699..97951,y=-30668..59918,z=-15349..69697
off x=-44271..17935,y=-9516..60759,z=49131..112598
on x=-61695..-5813,y=40978..94975,z=8655..80240
off x=-101086..-9439,y=-7088..67543,z=33935..83858
off x=18020..114017,y=-48931..32606,z=21474..89843
off x=-77139..10506,y=-89994..-18797,z=-80..59318
off x=8476..79288,y=-75520..11602,z=-96624..-24783
on x=-47488..-1262,y=24338..100707,z=16292..72967
off x=-84341..13987,y=2429..92914,z=-90671..-1318
off x=-37810..49457,y=-71013..-7894,z=-105357..-13188
off x=-27365..46395,y=31009..98017,z=15428..76570
off x=-70369..-16548,y=22648..78696,z=-1892..86821
on x=-53470..21291,y=-120233..-33476,z=-44150..38147
off x=-93533..-4276,y=-16170..68771,z=-104985..-24507";

			return ParseData(data);
		}

		private static List<ReactorRebootStep> GetData()
		{
			string data =
@"on x=4..48,y=-44..10,z=-45..4
on x=-41..3,y=-28..23,z=-4..44
on x=-11..36,y=-3..47,z=-6..41
on x=-38..14,y=-23..22,z=-33..18
on x=-37..13,y=-43..5,z=-20..26
on x=-44..4,y=-8..43,z=2..46
on x=-41..6,y=-39..5,z=-25..20
on x=-8..42,y=-19..26,z=-45..9
on x=-21..31,y=-23..31,z=-47..6
on x=-33..20,y=-49..0,z=-23..28
off x=29..47,y=26..44,z=21..36
on x=0..49,y=-43..9,z=-21..28
off x=34..48,y=36..45,z=-13..0
on x=-4..42,y=-4..42,z=-45..1
off x=5..23,y=18..33,z=12..28
on x=-9..37,y=-7..37,z=-38..14
off x=18..30,y=13..28,z=1..19
on x=-43..10,y=-27..25,z=-36..16
off x=20..30,y=-46..-37,z=13..23
on x=-46..4,y=-2..47,z=-20..32
on x=60835..73186,y=-51099..-20854,z=15096..33276
on x=-90516..-62783,y=-22014..2681,z=15971..34116
on x=32437..61116,y=55016..75955,z=-43220..-26114
on x=61663..78278,y=13402..23778,z=-58447..-27162
on x=-7203..27545,y=-63387..-39739,z=-84207..-58574
on x=60371..78784,y=-4929..24530,z=16635..35902
on x=2151..23649,y=-79023..-56347,z=23140..49303
on x=-15379..5313,y=9596..40708,z=55408..82133
on x=9762..44997,y=60848..63268,z=-56536..-39485
on x=-82016..-68432,y=5162..42218,z=-26489..-8810
on x=-9770..-5676,y=63112..84114,z=14156..35232
on x=-83053..-55898,y=-7298..7705,z=18872..56768
on x=21339..41341,y=12447..31347,z=55876..85179
on x=12940..21742,y=42024..62323,z=-62706..-43355
on x=7410..39215,y=-95457..-57028,z=-3529..14086
on x=-82573..-58796,y=11856..34753,z=25171..40965
on x=-55871..-31209,y=48584..56098,z=-52293..-28340
on x=1202..19255,y=4722..25599,z=71711..88378
on x=28465..47977,y=47963..70276,z=44808..50421
on x=-38182..-13536,y=16903..33739,z=-85612..-56731
on x=-58667..-26294,y=-79595..-56187,z=-189..21687
on x=27499..60232,y=-15440..5130,z=59339..70852
on x=49935..73736,y=-24802..-5748,z=28002..51493
on x=-63225..-40689,y=48124..60705,z=20004..34629
on x=-32620..-14556,y=16384..25285,z=-91210..-58108
on x=-13662..8481,y=4897..36713,z=56741..84383
on x=53694..78052,y=-45113..-19762,z=10232..48182
on x=64119..76755,y=20089..43753,z=19296..47109
on x=-88607..-69380,y=-30630..-7540,z=7737..28442
on x=-29273..5910,y=62070..91625,z=-32915..-20106
on x=-63284..-42785,y=-24688..9271,z=38367..65261
on x=1528..9579,y=-31225..-15385,z=56628..86970
on x=58311..92621,y=23712..42898,z=-12142..6017
on x=-48321..-23160,y=22699..45532,z=-77835..-49154
on x=-31363..-26330,y=-30082..5068,z=-74791..-67962
on x=-69064..-41915,y=-56798..-45525,z=-49815..-11356
on x=-39788..-12510,y=-82237..-56926,z=10396..32418
on x=8861..23482,y=58132..85515,z=-18587..13431
on x=54132..68601,y=-46275..-12721,z=38760..63426
on x=49664..62630,y=15647..39670,z=32401..62818
on x=33736..72117,y=44956..77168,z=-19589..-1417
on x=-83330..-60266,y=-21232..-2479,z=40518..63744
on x=-58458..-41716,y=-75352..-54666,z=26486..40905
on x=51338..74335,y=-72555..-46696,z=-6552..5995
on x=-55612..-32633,y=-63525..-57687,z=10466..32669
on x=-16805..-512,y=14779..20098,z=77824..82907
on x=-187..34308,y=-40503..-17920,z=-73303..-56995
on x=-5638..7072,y=-1912..175,z=-82007..-77035
on x=50282..85667,y=-52041..-27298,z=-14510..39
on x=555..37119,y=-56047..-33889,z=49135..79381
on x=57876..64249,y=9269..32776,z=34819..56768
on x=58399..87151,y=-34472..3222,z=5629..12240
on x=-75592..-54229,y=-57308..-32371,z=-885..28191
on x=-18928..-2887,y=-1111..4869,z=-99388..-60379
on x=-74467..-51287,y=-61337..-36868,z=-43992..-21241
on x=-63481..-27788,y=-76392..-40595,z=-35614..-16472
on x=40203..55533,y=-48322..-20543,z=49609..66612
on x=22431..52206,y=20856..49600,z=50769..75428
on x=53602..77039,y=4364..21980,z=-26779..-23973
on x=50554..63701,y=-50154..-46418,z=7688..18144
on x=35216..61059,y=-61010..-45902,z=-31915..-8430
on x=30990..63968,y=-62139..-47822,z=-49640..-20751
on x=5872..28880,y=-36616..-20786,z=-77949..-62367
on x=58617..83360,y=-56669..-25514,z=-15084..1578
on x=-7917..19331,y=-38974..-10249,z=-93808..-73674
on x=-36015..-18100,y=-1910..30814,z=70164..77253
on x=-72211..-49171,y=30136..48243,z=-40074..-33976
on x=-60065..-39911,y=-32108..-12300,z=54895..79287
on x=-33572..-15818,y=59008..95505,z=-4201..26094
on x=19947..53161,y=48551..66292,z=28364..49575
on x=-79473..-65327,y=-34675..-4541,z=-17253..-1835
on x=61635..88533,y=-1393..19404,z=-26987..4451
on x=-52601..-28147,y=-33544..-10722,z=49146..78809
on x=59666..65526,y=7986..30264,z=44731..68629
on x=52264..69389,y=37807..58455,z=-43991..-32762
on x=20955..56341,y=-73986..-48987,z=21543..28657
on x=-8671..6881,y=57248..82076,z=49296..69551
on x=-5135..17503,y=-9335..7170,z=74682..84677
on x=26539..47446,y=-58750..-46145,z=53149..74272
on x=11381..46321,y=45328..67962,z=33794..44833
on x=-14211..4060,y=46036..80743,z=42654..61674
on x=-47778..-31535,y=39379..64139,z=-49433..-31399
on x=59211..75959,y=-630..29887,z=-51603..-40235
on x=21675..43339,y=52319..65852,z=20891..53427
on x=-40053..-35405,y=-61562..-34657,z=40884..61693
on x=3257..33496,y=61187..85311,z=-38192..-16710
on x=15443..24692,y=-48099..-44606,z=56527..65214
on x=26251..44234,y=-35085..-27152,z=52157..60516
on x=-30170..-7365,y=54037..84578,z=-35102..-19823
on x=-86696..-58695,y=10805..33353,z=-16616..-8352
on x=63831..87945,y=6621..28600,z=27828..55060
on x=39842..44174,y=50368..68154,z=19535..38291
on x=-29465..-12948,y=53320..81486,z=19329..37286
on x=-4564..17219,y=-52721..-34660,z=-77236..-67462
on x=65087..78228,y=-45988..-33118,z=-30718..-11072
on x=20903..42619,y=5873..24545,z=54214..79235
on x=-24821..-15165,y=-80257..-61078,z=27299..43240
on x=-20023..4360,y=-36657..-18515,z=-85225..-65593
on x=28839..43494,y=51246..78277,z=-58569..-27228
on x=-73669..-66349,y=35350..57001,z=-12503..-8203
on x=-65258..-46527,y=-27933..1033,z=37054..64315
on x=20126..42562,y=69014..88764,z=15230..36367
on x=70388..82554,y=-40763..-15967,z=-18714..1939
on x=-29977..-10088,y=-42308..-26095,z=64849..68866
on x=-51627..-19831,y=51548..75538,z=-31505..-16333
on x=-77951..-46955,y=-35793..-26067,z=42894..46869
on x=-8289..20279,y=69634..84807,z=20812..42326
on x=66333..85391,y=-33554..-30303,z=2933..19330
on x=56382..70342,y=-20514..10814,z=-48719..-33107
on x=2834..13379,y=75435..80904,z=-406..9617
on x=21125..39200,y=51389..82714,z=-28353..-16548
on x=-94216..-79666,y=-1162..17392,z=289..11727
on x=25058..44136,y=34421..59252,z=43525..63614
on x=-27518..-22911,y=-90808..-74345,z=-21224..-12030
on x=-42005..-8514,y=-32029..-10077,z=-74901..-70082
on x=-58135..-50002,y=55190..62697,z=-28403..-19505
on x=-19512..-9896,y=-67577..-51832,z=41077..53109
on x=48434..68799,y=25169..51350,z=-47485..-24200
on x=4120..36124,y=-85971..-49306,z=-41209..-35595
on x=-21814..5022,y=42713..71330,z=-65183..-35079
on x=33632..55149,y=17457..46822,z=-71736..-54449
on x=-76287..-65789,y=31217..45468,z=6957..27127
on x=5586..21623,y=24560..53301,z=-81523..-50254
on x=11567..20183,y=-68030..-38381,z=41983..71056
on x=-54389..-37045,y=-23920..3502,z=58649..68194
on x=34163..50089,y=43190..62084,z=-44470..-28275
on x=-81593..-50075,y=-14979..-3394,z=45887..61817
on x=32558..50667,y=-35014..-22732,z=-76705..-54828
on x=30944..55649,y=-3388..31546,z=-62149..-53109
on x=-32635..-974,y=57055..77777,z=-18345..-9721
on x=-19148..4435,y=-53241..-41569,z=43074..61642
on x=-68837..-52527,y=-54717..-46535,z=-45363..-9242
on x=-66803..-44131,y=-80890..-47222,z=-3955..12323
on x=-13059..609,y=72630..78732,z=18471..35230
on x=-62378..-35048,y=-4345..12636,z=-78723..-41810
on x=49301..69347,y=31026..36812,z=-61676..-41853
on x=46239..60216,y=-20974..-696,z=41770..61392
on x=74988..81836,y=-42307..-11308,z=-1964..30781
on x=-52831..-35841,y=29838..50263,z=-72934..-48323
on x=3681..18057,y=-79944..-53136,z=27259..35521
on x=-66964..-42663,y=22163..52213,z=-53212..-36189
on x=-83809..-47894,y=16011..42572,z=18024..39361
on x=-40282..-29731,y=-17295..21579,z=-86459..-54504
on x=48893..64872,y=38417..52574,z=-37382..-25980
on x=-69692..-60954,y=39755..46667,z=-28153..-16056
on x=-28880..-10995,y=71734..94832,z=-2361..25984
on x=-46985..-15187,y=70064..78319,z=-12610..-80
on x=24283..53071,y=58428..67306,z=14453..34421
on x=-96063..-72408,y=-495..16687,z=-17372..6980
on x=-30851..-16257,y=56654..72961,z=31089..50736
on x=-24070..-14668,y=67304..79775,z=2389..14543
on x=56513..67794,y=-54200..-35096,z=-3787..26846
on x=-9416..17713,y=29175..60120,z=67357..81417
on x=13752..23055,y=-93939..-60962,z=-10099..17785
on x=45017..75963,y=590..30771,z=47801..71503
on x=-40633..-25984,y=64169..82991,z=2094..12822
on x=33894..58265,y=55012..82293,z=-15068..5207
on x=-36034..-32610,y=58363..76409,z=23385..33193
on x=-40558..-23989,y=72913..88300,z=-16226..9017
on x=-26533..-12328,y=58430..95092,z=-21505..-116
on x=-65817..-44975,y=-24580..763,z=53685..68196
on x=20936..56294,y=63263..83904,z=7828..39061
on x=-78635..-52470,y=20905..54121,z=11457..33833
on x=-31285..-20124,y=45212..61511,z=37614..59108
on x=18502..40345,y=-24843..-3490,z=51589..78837
on x=44832..66764,y=-37334..-14962,z=33170..56493
on x=-83057..-76951,y=-36457..-3472,z=-9972..25078
on x=-34241..-7569,y=-87897..-62323,z=-34250..-25090
on x=62121..81021,y=-14787..-4125,z=16386..26387
on x=-2765..16700,y=57652..82448,z=19074..44764
on x=-46987..-36685,y=46792..61546,z=28810..57702
on x=55518..83446,y=16731..35055,z=34128..57271
on x=-80810..-58185,y=13322..28564,z=-21854..8081
on x=32283..61111,y=32767..50716,z=-51005..-30277
on x=-55084..-35082,y=-76528..-64236,z=-34372..-4542
on x=36600..61712,y=-65169..-40631,z=-36750..-12694
on x=32895..35664,y=-4273..12843,z=-89420..-68506
on x=71100..92216,y=-10390..-1345,z=12148..28098
on x=-79894..-60882,y=16186..47889,z=831..24847
on x=-81811..-60988,y=-35646..-3119,z=-49235..-35613
on x=-52224..-18260,y=57829..74464,z=-4946..21374
on x=30872..55612,y=8739..20578,z=-75733..-59655
on x=-73902..-48026,y=-14917..-9451,z=-62619..-41481
on x=2152..13100,y=-22702..-1904,z=-87446..-73597
on x=28139..58369,y=-8268..8326,z=64575..82773
on x=45082..60299,y=18330..54784,z=-63038..-38821
on x=37126..40498,y=16698..37263,z=56572..79015
on x=7713..24819,y=-67857..-47588,z=27143..52661
on x=8399..31900,y=-95815..-64121,z=-1134..8030
on x=9165..34746,y=51733..71711,z=-35238..-22844
on x=-35255..-11806,y=-75977..-71182,z=-40155..-7408
on x=-13155..-1814,y=24957..32732,z=-86363..-60667
on x=-78803..-47904,y=-64683..-40849,z=-85..19170
on x=-58849..-39154,y=52361..68595,z=-18722..1953
on x=23768..54659,y=-54893..-49316,z=26131..62976
on x=-52089..-20504,y=-4273..4735,z=-87038..-62851
on x=43353..71574,y=49659..71621,z=-20967..-3847
on x=-80015..-54035,y=10893..38696,z=12831..39483
on x=-94027..-72391,y=4629..31086,z=-4535..15388
on x=-66837..-56566,y=20392..53159,z=-37753..-18075
off x=-49441..-39746,y=-55256..-46711,z=-55653..-40977
on x=35237..48951,y=13214..41053,z=-68984..-43398
on x=-3823..14468,y=72433..84497,z=22476..40602
off x=-61525..-40710,y=-45149..-21675,z=56408..76559
on x=-22998..13592,y=54719..82854,z=-57840..-25482
off x=22335..44880,y=-18975..10974,z=-84170..-61280
off x=10692..32345,y=-42853..-13970,z=-72971..-57189
on x=-17856..12135,y=-46544..-20507,z=-90612..-69259
on x=-1376..26277,y=73713..90253,z=8401..35734
off x=-40041..-16084,y=-80576..-57176,z=-21081..7685
on x=-55180..-35741,y=-64015..-43758,z=-54741..-38944
on x=-4927..8192,y=68718..94200,z=-18031..-8243
off x=-28123..-19562,y=-81850..-64173,z=-22367..11456
off x=52415..83558,y=-54643..-27615,z=-34569..-7526
off x=-64969..-42817,y=48831..65076,z=24862..46711
off x=-47038..-20196,y=61669..80279,z=24557..37576
off x=19573..29018,y=69713..85754,z=-16519..18124
on x=-24427..-14600,y=56205..78659,z=9031..33521
on x=-4298..9954,y=-81578..-50149,z=22848..49948
on x=31492..61096,y=-65934..-33187,z=24071..39074
on x=5335..13199,y=75241..94850,z=17014..28915
off x=40639..62322,y=44416..50577,z=-36730..-19968
off x=9165..28764,y=-84835..-47728,z=-38187..-34103
off x=-58724..-31435,y=-42662..-22106,z=-63199..-55199
off x=37039..50903,y=-26573..-11837,z=-68143..-56769
on x=-29011..-5964,y=-82622..-74338,z=-15874..2161
off x=-31316..-16599,y=14358..37299,z=-87670..-59803
on x=59747..74786,y=-32964..-16894,z=-29512..-23051
off x=16398..53660,y=36500..49449,z=-65103..-41176
off x=-35842..-2427,y=66638..88997,z=-15053..-8898
on x=-53640..-18531,y=-13712..21501,z=-77231..-63885
on x=57568..90163,y=-11188..8803,z=29711..33998
on x=-67102..-31404,y=-57792..-48711,z=16204..52029
on x=4388..18256,y=30552..41291,z=57004..80636
on x=-74168..-65523,y=14745..26141,z=27851..50260
off x=-6897..15664,y=2274..15513,z=69526..94210
off x=63828..95467,y=-2880..16178,z=-12680..6879
off x=-2877..26613,y=-49708..-28450,z=-80303..-55775
on x=68257..83446,y=6162..32470,z=-7027..28698
off x=-56387..-33419,y=27800..61664,z=-54978..-39757
off x=47586..65994,y=-73268..-58241,z=-14785..8577
on x=-24606..1967,y=-19355..-7409,z=-77859..-74466
off x=-33418..-25228,y=-31668..1490,z=54853..87579
off x=-27850..-15271,y=-78941..-67184,z=-14383..9267
on x=60846..81657,y=5581..14240,z=-33297..-5213
on x=-33786..-20789,y=47804..60888,z=27562..63100
off x=72507..80732,y=15511..25873,z=-27950..-10251
off x=-14204..9970,y=9139..33097,z=59536..77693
on x=10885..42182,y=-35887..-29137,z=-77032..-53352
off x=-65613..-41382,y=-51266..-44097,z=11980..42642
off x=-45428..-23414,y=-61707..-49335,z=31263..64413
on x=-55160..-36729,y=-65006..-50506,z=-26473..-17078
off x=-72370..-62069,y=-21561..-66,z=-45714..-35951
off x=-88298..-62749,y=-13860..-929,z=32119..39606
off x=-29087..-6394,y=-25358..-11518,z=-94168..-65753
on x=-55782..-33441,y=39622..74674,z=8000..42855
on x=-29914..-5414,y=-45562..-24620,z=54345..80183
on x=26229..50139,y=-86414..-67916,z=-25238..-6197
on x=-87601..-59655,y=12933..33422,z=16957..23160
on x=-56924..-39567,y=-23701..4430,z=-71723..-42914
on x=-5451..1726,y=75330..80127,z=23070..34596
off x=-28066..-5518,y=62987..94295,z=-22274..-2285
off x=-86003..-75242,y=-35769..-7609,z=-7269..3230
off x=-24188..-5924,y=-80853..-58982,z=-47217..-26911
on x=-16543..2497,y=-41628..-33260,z=-72329..-68247
off x=-27806..-4614,y=-12792..5030,z=71014..84235
on x=-45894..-34652,y=-19621..4069,z=-73195..-66201
on x=-64695..-47979,y=-40007..-26826,z=-59252..-36278
off x=-58749..-44740,y=-35768..-7079,z=53856..77197
on x=-78605..-63264,y=7597..27425,z=-60392..-29347
off x=37772..45871,y=-67944..-42187,z=-29542..-8862
off x=-31442..-19867,y=-65165..-44647,z=-55703..-32008
on x=-71094..-49369,y=53790..67639,z=-8403..25308
on x=50297..77493,y=47392..49973,z=-24332..5227
on x=-65267..-59046,y=-48934..-43993,z=-27752..-9900
off x=27673..63036,y=-56600..-43411,z=38647..46170
on x=-60129..-40486,y=52569..78053,z=-28411..1249
on x=35074..46681,y=-75731..-52818,z=-35239..-15924
off x=-8157..3674,y=-95241..-68034,z=8443..39745
on x=-40372..-11589,y=-45956..-25718,z=62531..72724
off x=-62176..-34224,y=36206..56527,z=-61236..-28268
off x=10125..28982,y=22423..49501,z=52891..79890
on x=61143..96118,y=-26331..510,z=-24321..4979
off x=-7827..5237,y=-3848..17790,z=71527..98020
on x=-16410..-507,y=-910..15054,z=72523..83857
off x=-89645..-52495,y=-43455..-16691,z=-24430..-7112
on x=-20950..6006,y=15326..33921,z=66037..86096
on x=-53876..-49139,y=39256..57480,z=23164..37463
on x=7517..17572,y=77015..95522,z=5479..25023
off x=36886..60478,y=14805..32755,z=-77379..-46308
off x=42017..77405,y=-15788..11251,z=-66525..-37982
off x=46275..68710,y=-55755..-37862,z=-44756..-34174
off x=5245..29015,y=61451..84774,z=-10747..11869
on x=-29977..-10847,y=2857..19826,z=76646..79411
on x=34852..66149,y=-54315..-39950,z=23043..51805
on x=-69203..-42855,y=48755..63772,z=-15307..3623
off x=-90486..-56805,y=-7433..-3716,z=-37511..-25610
on x=-49424..-19219,y=57316..70290,z=31002..44840
off x=1256..34370,y=-39909..-25878,z=-73606..-54011
on x=-37556..-21765,y=-86404..-64506,z=-16265..-10738
off x=51849..72798,y=-50674..-28291,z=-841..33961
on x=35161..55446,y=-33710..-19792,z=-58344..-52620
off x=-92426..-65316,y=-28518..7282,z=516..23668
off x=41150..47274,y=19331..35774,z=-64012..-48895
off x=-15557..12011,y=24510..43286,z=-79068..-53258
off x=43304..58140,y=33990..66907,z=12551..37658
off x=-66383..-43305,y=46925..58833,z=-47255..-18377
off x=-60614..-28864,y=64072..75729,z=-22112..5534
on x=22763..45396,y=-50193..-20915,z=-67972..-55181
on x=-14988..16966,y=46393..63463,z=40030..64371
on x=67557..80545,y=20905..41054,z=6894..25288
off x=-41766..-39148,y=52019..89131,z=-13345..1820
on x=-77491..-58259,y=-34602..-7675,z=40700..60505
on x=-58432..-35902,y=-83855..-61680,z=-39446..-8525
off x=-68770..-36943,y=-59305..-33343,z=25019..47867
off x=11672..16600,y=14569..32933,z=61723..93190
on x=35811..61208,y=15975..34958,z=45880..72310
on x=-38438..-21563,y=42278..61863,z=-79132..-50724
off x=-72897..-57102,y=-32916..-19418,z=36512..55438
off x=16873..22754,y=-67507..-39173,z=-69085..-54633
on x=8127..32592,y=-40433..-37859,z=-70632..-54966
off x=-53040..-26020,y=27625..43868,z=-81230..-57009
on x=50068..80578,y=-15765..-11571,z=32956..51823
off x=-19647..12336,y=6891..36092,z=-88749..-69313
off x=-22926..-12009,y=-38024..-19389,z=60358..85818
on x=-8789..-2261,y=38124..74252,z=52997..73289
off x=20501..35766,y=-75199..-56235,z=-32368..-11278
on x=-89166..-55505,y=-1123..16557,z=-49091..-20554
on x=-86663..-53304,y=-11886..9600,z=16377..47222
off x=-43587..-14141,y=-78221..-71117,z=-26019..4273
off x=51529..74793,y=-13678..-9601,z=-57077..-43218
off x=16080..20138,y=-8496..26481,z=-94025..-57409
on x=-9280..821,y=18376..34142,z=-91246..-70157
on x=-30164..-24105,y=39459..55516,z=35752..66409
on x=-67253..-60281,y=17603..50898,z=-54330..-18889
off x=-96754..-59391,y=14640..30672,z=-18557..16053
off x=48528..61498,y=28181..55273,z=-58418..-39733
on x=-22623..642,y=-72097..-61211,z=23328..42882
off x=27074..47909,y=-69471..-42714,z=-70066..-34256
off x=41036..60174,y=54672..66432,z=-1693..10819
off x=-53198..-27280,y=28939..57330,z=60357..78142
off x=38226..62175,y=41283..70704,z=-5541..6706
on x=-23034..-12636,y=-88604..-58763,z=-22203..-5526
off x=-45227..-37661,y=24242..52054,z=-79449..-45391
off x=-50372..-35901,y=-65142..-42735,z=30200..37913
on x=-70694..-59634,y=3799..17232,z=-42310..-32861
off x=21705..34702,y=32894..51603,z=-69219..-55416
on x=50003..77194,y=8823..40616,z=31934..50041
on x=28521..30241,y=33036..55341,z=52066..73340
off x=40460..72096,y=-45628..-40418,z=-47520..-29755
off x=-71992..-41516,y=-48622..-29739,z=-49193..-27687
on x=-6784..9435,y=-73585..-48730,z=36607..69977
off x=-59735..-42545,y=967..27091,z=51230..74732
on x=-75352..-55195,y=25349..47893,z=34027..63618
on x=12802..15858,y=-60006..-39917,z=56336..85690
on x=-49697..-29545,y=-70999..-61800,z=-10200..21176
off x=48297..74261,y=-54143..-37583,z=11684..23632
off x=-40821..-19943,y=-24830..-16739,z=-94285..-57573
off x=-73261..-66064,y=-47062..-20830,z=-14427..3045
on x=45282..70195,y=-60910..-36635,z=-5986..20409
off x=34881..68440,y=42279..74979,z=-36647..-20312
on x=-54825..-32057,y=-6594..15868,z=55325..65789
off x=-24247..-22418,y=50082..61687,z=-64615..-30368
on x=-66403..-55155,y=-51035..-46507,z=433..22893
on x=50850..77752,y=-42441..-38195,z=16462..42927
off x=53492..84696,y=-52904..-38923,z=3874..26590
off x=-17681..-820,y=-54879..-42429,z=51149..72606
off x=-1365..25131,y=49316..81774,z=-46069..-29062
off x=-77722..-63998,y=-54059..-27405,z=-5858..28662
on x=20472..42660,y=51877..69524,z=9121..30121
off x=31328..46094,y=-12100..-1178,z=56893..66464
off x=-47555..-33328,y=-74522..-48085,z=29616..40372
off x=39702..53151,y=-67758..-49166,z=-31133..-9963
off x=16966..38557,y=-29204..-1018,z=-82268..-55135
on x=25727..52915,y=-71129..-61303,z=-29621..-11100
off x=57766..80643,y=-23173..-9077,z=6173..32585
on x=6907..34334,y=-16166..-1149,z=-84013..-66109
on x=-51859..-46381,y=-67669..-51000,z=-1720..10534
on x=-62102..-45256,y=26373..58587,z=17662..48424
on x=-43442..-9706,y=73336..76533,z=-2302..26485
off x=29646..43052,y=-88030..-60328,z=4486..33616
on x=22232..40583,y=-80283..-62094,z=3020..19434
off x=42570..61205,y=55367..63121,z=16987..33619
on x=-70409..-52917,y=5132..27846,z=32582..55307
on x=27282..51198,y=-28431..-9502,z=58642..61960
off x=-39669..-24319,y=26942..43876,z=-71505..-51701
off x=38231..71961,y=-30494..1149,z=55892..64212
on x=-35194..-28383,y=-75841..-54355,z=13505..33535
off x=-51048..-37443,y=-23528..-11705,z=-80336..-56252
on x=64400..79685,y=-44408..-25349,z=12201..16321
off x=44375..56297,y=-69268..-33121,z=19593..48549
on x=-50815..-21236,y=21054..29528,z=-69546..-58420
on x=51188..85166,y=-26208..-351,z=29662..48742
on x=-69558..-59026,y=-16899..-6106,z=-58955..-29683
on x=48770..86736,y=-13749..12854,z=-59825..-21626
on x=18137..28863,y=-30392..-15399,z=58327..76134
off x=-13481..6976,y=67381..84761,z=-45009..-16182
off x=43006..58725,y=58369..70578,z=-8916..2639
off x=587..5222,y=53820..72007,z=-59152..-47989
on x=45216..79608,y=-37805..-9849,z=17201..53406";

			return ParseData(data);
		}

		private static List<ReactorRebootStep> ParseData(string data)
		{
			var rows = data.Split("\r\n");

			var steps = new List<ReactorRebootStep>();

			foreach (string row in rows)
			{
				bool on = row.StartsWith("on");

				int xPosition = row.IndexOf('x') + 2;
				int yPosition = row.IndexOf('y') + 2;
				int zPosition = row.IndexOf('z') + 2;

				string x = row.Substring(xPosition, yPosition - xPosition - 3);
				string y = row.Substring(yPosition, zPosition - yPosition - 3);
				string z = row.Substring(zPosition);

				var xParts = x.Split("..");
				var yParts = y.Split("..");
				var zParts = z.Split("..");

				var step = new ReactorRebootStep
				{
					On = on,
					MinX = int.Parse(xParts[0]),
					MaxX = int.Parse(xParts[1]),
					MinY = int.Parse(yParts[0]),
					MaxY = int.Parse(yParts[1]),
					MinZ = int.Parse(zParts[0]),
					MaxZ = int.Parse(zParts[1]),
				};

				steps.Add(step);
			}

			return steps;
		}
	}

	public class ReactorRebootStep
	{
		public bool On { get; set; }
		public bool Off => !On;
		public int MinX { get; set; }
		public int MaxX { get; set; }
		public int MinY { get; set; }
		public int MaxY { get; set; }
		public int MinZ { get; set; }
		public int MaxZ { get; set; }

		public override string ToString()
		{
			return $"x={MinX}..{MaxX},y={MinY}..{MaxY},z={MinZ}..{MaxZ}";
		}

		public bool DoesOverlap(ReactorRebootStep step)
		{
			// Check whether the X ranges overlap
			if (step.MaxX < MinX || step.MinX > MaxX)
			{
				return false;
			}

			// Check whether the Y ranges overlap
			if (step.MaxY < MinY || step.MinY > MaxY)
			{
				return false;
			}

			// Check whether the Z ranges overlap
			if (step.MaxZ < MinZ || step.MinZ > MaxZ)
			{
				return false;
			}

			return true;
		}

		public List<ReactorRebootStep> SubtractOverlap(ReactorRebootStep step)
		{
			// The overlap between these x, y, and z ranges forms a box
			// Removing that box from current box (step) results in ~3 new smaller boxes
			var splitSteps = new List<ReactorRebootStep>();

			// Starting with x, get the three new boxes
			int newMinX;
			int newMaxX;
			if (MinX >= step.MinX && MinX <= step.MaxX)
			{
				newMinX = step.MaxX + 1;
			}
			else
			{
				newMinX = MinX;
			}

			if (MaxX >= step.MinX && MaxX <= step.MaxX)
			{
				newMaxX = step.MinX - 1;
			}
			else
			{
				newMaxX = MaxX;
			}

			// This step's x range is fully within the other step's x range
			bool xIsFullyContained = MinX != newMinX && MaxX != newMaxX;
			// The other step's x range is fully within this steps x range
			bool xFullyContains = MinX == newMinX && MaxX == newMaxX;

			// If this step's x range fully contains the other step's x range
			if (xFullyContains)
			{
				int leftMaxX = step.MinX - 1; // The new max for the left range
				int rightMinX = step.MaxX + 1; // The new min for the right range

				// Remove the middle - we need two new steps: one to the left of
				// the overlapping range, and one to the right
				var leftStep = new ReactorRebootStep
				{
					On = On,
					MinX = MinX,
					MaxX = leftMaxX,
					MinY = MinY,
					MaxY = MaxY,
					MinZ = MinZ,
					MaxZ = MaxZ
				};
				splitSteps.Add(leftStep);

				var rightStep = new ReactorRebootStep
				{
					On = On,
					MinX = rightMinX,
					MaxX = MaxX,
					MinY = MinY,
					MaxY = MaxY,
					MinZ = MinZ,
					MaxZ = MaxZ
				};
				splitSteps.Add(rightStep);

				// Update the new min x and new max x values for the next boxes
				newMinX = leftMaxX + 1;
				newMaxX = rightMinX - 1;
			}
			// If this step's x range is not fully contained by the other step's x range
			else if (!xIsFullyContained)
			{
				var newStep = new ReactorRebootStep
				{
					On = On,
					MinX = newMinX,
					MaxX = newMaxX,
					MinY = MinY,
					MaxY = MaxY,
					MinZ = MinZ,
					MaxZ = MaxZ
				};
				splitSteps.Add(newStep);

				// Update the new min x and new max x values for the next boxes
				int tempNewMinX = MaxX == newMaxX ? MinX : newMaxX + 1;
				newMaxX = MaxX == newMaxX ? newMinX - 1 : MaxX;
				newMinX = tempNewMinX;
			}
			else
			{
				// We don't need a new step for boxes that are fully contained by the other step
				// Also, reset the new min and new max values
				newMinX = MinX;
				newMaxX = MaxX;
			}

			// Moving on to y, get the next box
			int newMinY;
			int newMaxY;
			if (MinY >= step.MinY && MinY <= step.MaxY)
			{
				newMinY = step.MaxY + 1;
			}
			else
			{
				newMinY = MinY;
			}

			if (MaxY >= step.MinY && MaxY <= step.MaxY)
			{
				newMaxY = step.MinY - 1;
			}
			else
			{
				newMaxY = MaxY;
			}

			// This step's y range is fully within the other step's y range
			bool yIsFullyContained = MinY != newMinY && MaxY != newMaxY;
			// The other step's y range is fully within this steps y range
			bool yFullyContains = MinY == newMinY && MaxY == newMaxY;

			// If this step's y range fully contains the other step's y range
			if (yFullyContains)
			{
				int leftMaxY = step.MinY - 1; // The new max for the left range
				int rightMinY = step.MaxY + 1; // The new min for the right range

				// Remove the middle - we need two new steps: one to the left of
				// the overlapping range, and one to the right
				var leftStep = new ReactorRebootStep
				{
					On = On,
					MinX = newMinX,
					MaxX = newMaxX,
					MinY = MinY,
					MaxY = leftMaxY,
					MinZ = MinZ,
					MaxZ = MaxZ
				};
				splitSteps.Add(leftStep);

				var rightStep = new ReactorRebootStep
				{
					On = On,
					MinX = newMinX,
					MaxX = newMaxX,
					MinY = rightMinY,
					MaxY = MaxY,
					MinZ = MinZ,
					MaxZ = MaxZ
				};
				splitSteps.Add(rightStep);

				// Update the new min y and new max y values for the next boxes
				newMinY = leftMaxY + 1;
				newMaxY = rightMinY - 1;
			}
			// If this step's y range is not fully contained by the other step's y range
			else if (!yIsFullyContained)
			{
				var newStep = new ReactorRebootStep
				{
					On = On,
					MinX = newMinX,
					MaxX = newMaxX,
					MinY = newMinY,
					MaxY = newMaxY,
					MinZ = MinZ,
					MaxZ = MaxZ
				};
				splitSteps.Add(newStep);

				// Update the new min y and new max y values for the next boxes
				int tempNewMinY = MaxY == newMaxY ? MinY : newMaxY + 1;
				newMaxY = MaxY == newMaxY ? newMinY - 1 : MaxY;
				newMinY = tempNewMinY;
			}
			else
			{
				// We don't need a new step for boxes that are fully contained by the other step
				// Also, reset the new min and new max values
				newMinY = MinY;
				newMaxY = MaxY;
			}

			// Get the final box along the z axis
			int newMinZ;
			int newMaxZ;
			if (MinZ >= step.MinZ && MinZ <= step.MaxZ)
			{
				newMinZ = step.MaxZ + 1;
			}
			else
			{
				newMinZ = MinZ;
			}

			if (MaxZ >= step.MinZ && MaxZ <= step.MaxZ)
			{
				newMaxZ = step.MinZ - 1;
			}
			else
			{
				newMaxZ = MaxZ;
			}

			// This step's z range is fully within the other step's z range
			bool zIsFullyContained = MinZ != newMinZ && MaxZ != newMaxZ;
			// The other step's z range is fully within this steps z range
			bool zFullyContains = MinZ == newMinZ && MaxZ == newMaxZ;

			// If this step's z range fully contains the other step's z range
			if (zFullyContains)
			{
				int leftMaxZ = step.MinZ - 1; // The new max for the left range
				int rightMinZ = step.MaxZ + 1; // The new min for the right range

				// Remove the middle - we need two new steps: one to the left of
				// the overlapping range, and one to the right
				var leftStep = new ReactorRebootStep
				{
					On = On,
					MinX = newMinX,
					MaxX = newMaxX,
					MinY = newMinY,
					MaxY = newMaxY,
					MinZ = MinZ,
					MaxZ = leftMaxZ
				};
				splitSteps.Add(leftStep);

				var rightStep = new ReactorRebootStep
				{
					On = On,
					MinX = newMinX,
					MaxX = newMaxX,
					MinY = newMinY,
					MaxY = newMaxY,
					MinZ = rightMinZ,
					MaxZ = MaxZ
				};
				splitSteps.Add(rightStep);
			}
			// If this step's z range is not fully contained by the other step's z range
			else if (!zIsFullyContained)
			{
				var newStep = new ReactorRebootStep
				{
					On = On,
					MinX = newMinX,
					MaxX = newMaxX,
					MinY = newMinY,
					MaxY = newMaxY,
					MinZ = newMinZ,
					MaxZ = newMaxZ
				};
				splitSteps.Add(newStep);
			}
			else
			{
				// We don't need a new step for boxes that are fully contained by the other step
			}

			return splitSteps;
		}
	}
}
