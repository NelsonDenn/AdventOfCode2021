﻿using System;
using System.Collections.Generic;

namespace AdventOfCode2021
{
	public class Day13
	{
		public static int Run()
		{
			//var data = GetTestData();
			var data = GetData();

			//var foldingInstructions = GetTestFoldingInstructions();
			var foldingInstructions = GetFoldingInstructions();

			var result = RunPart1(data, foldingInstructions); // 747
			//var result = RunPart2(data, foldingInstructions); // ARHZPCUH

			// 881 too high
			return result;
		}

		private static int RunPart2(char[,] plot, List<FoldingInstructions> instructions)
		{
			return 0;
		}

		private static int RunPart1(char[,] plot, List<FoldingInstructions> instructions)
		{
			//Console.WriteLine("Starting plot");
			//PrintPlot(plot);

			foreach (var instruction in instructions)
			{
				Console.WriteLine(instruction);

				char[,] foldedPlot;

				if (instruction.IsHorizontalFold)
				{
					int plotHeight = plot.GetLength(1);
					int foldedPlotWidth = plot.GetLength(0);
					int foldedPlotHeight = instruction.FoldingLine;
					foldedPlot = new char[foldedPlotWidth, foldedPlotHeight];

					for (int x = 0; x < foldedPlotWidth; x++)
					{
						for (int y = 0; y < instruction.FoldingLine; y++)
						{
							if (plot[x, y] == '#' || plot[x, plotHeight - y - 1] == '#')
							{
								foldedPlot[x, y] = '#';
							}
							else
							{
								foldedPlot[x, y] = '.';
							}
						}
					}
				}
				else
				{
					int plotWidth = plot.GetLength(0);
					int foldedPlotWidth = instruction.FoldingLine;
					int foldedPlotHeight = plot.GetLength(1);
					foldedPlot = new char[foldedPlotWidth, foldedPlotHeight];

					for (int x = 0; x < foldedPlotWidth; x++)
					{
						for (int y = 0; y < foldedPlotHeight; y++)
						{
							if (plot[x, y] == '#' || plot[plotWidth - x - 1, y] == '#')
							{
								foldedPlot[x, y] = '#';
							}
							else
							{
								foldedPlot[x, y] = '.';
							}
						}
					}
				}

				plot = foldedPlot;
				//PrintPlot(plot);
			}

			PrintPlot(plot);

			return 0;
		}

		private static void PrintPlot(char[,] plot)
		{
			Console.WriteLine();

			int numDots = 0; // '#' is a dot and '.' is an empty, unmarked position
			for (int y = 0; y < plot.GetLength(1); y++)
			{
				for (int x = 0; x < plot.GetLength(0); x++)
				{
					char value = plot[x, y];
					Console.Write(value);
					if (value == '#')
					{
						numDots++;
					}
				}

				Console.WriteLine();
			}

			Console.WriteLine("Number of dots: " + numDots);
			Console.WriteLine();
		}

		private static char[,] GetTestData()
		{
			string data =
@"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0";

			return ParseData(data);
		}

		private static List<FoldingInstructions> GetTestFoldingInstructions()
		{
			string data =
@"fold along y=7
fold along x=5";

			return ParseFoldingInstructions(data);
		}

		private static char[,] GetData()
		{
			string data =
@"899,112
477,249
462,60
49,299
1097,819
890,74
477,231
1242,464
57,641
833,471
1292,784
1232,717
898,227
442,749
893,116
395,423
1304,695
211,186
1097,243
1004,46
654,361
1044,784
745,284
376,637
634,737
509,369
242,395
199,442
397,170
634,94
954,15
1019,219
798,782
340,262
1299,598
1133,23
967,5
291,675
519,119
97,698
291,107
301,592
505,537
1280,780
340,632
919,588
733,884
166,577
530,840
1197,187
1004,605
244,668
38,368
1241,170
1161,199
597,701
594,140
1161,691
505,357
666,381
947,107
1268,179
984,585
418,775
805,471
1074,455
142,245
284,403
1064,557
207,766
485,759
236,775
38,309
329,59
1169,857
791,775
1310,809
530,715
571,401
525,673
1064,701
304,110
402,350
1237,172
646,287
959,156
62,320
674,364
959,738
723,535
177,423
798,85
497,859
825,99
473,593
575,45
505,215
764,133
109,684
884,457
356,879
920,238
310,233
79,266
177,663
691,868
1290,352
714,745
493,527
1173,65
1026,520
284,473
662,647
557,522
1190,667
181,180
1178,114
990,3
798,649
428,224
428,511
254,567
574,669
468,672
324,801
1290,542
120,702
271,312
145,817
1082,187
1014,493
691,420
718,544
412,560
420,368
428,432
326,585
686,108
1297,25
1290,782
689,893
1046,798
194,588
423,873
97,876
748,634
865,887
296,493
1046,350
488,311
1048,411
344,152
962,718
410,893
986,801
527,274
798,654
736,107
462,95
999,826
1119,595
604,652
261,124
20,352
581,50
417,778
820,260
310,651
95,21
20,21
629,32
1133,471
1082,885
82,450
1079,0
798,334
420,634
1243,87
290,262
477,295
226,511
915,670
364,632
248,625
1062,269
331,324
713,193
739,65
74,667
1248,574
903,490
65,868
1250,306
149,695
574,673
246,787
570,717
1225,105
214,36
904,336
991,726
1054,661
744,765
324,353
73,546
667,150
780,179
1136,381
920,14
57,253
825,404
638,427
170,297
231,465
792,140
979,773
669,8
1216,10
1020,262
758,158
879,516
621,893
1183,172
370,25
140,446
1034,820
301,289
325,835
167,794
1293,304
58,521
108,504
718,96
621,393
890,816
977,180
236,887
1277,309
197,53
999,397
1133,599
619,26
932,746
842,149
1236,765
16,651
1216,688
1149,367
743,819
706,674
1212,259
130,623
273,54
284,25
284,491
485,411
313,248
79,406
1046,96
13,663
35,710
306,166
560,206
1168,245
460,101
363,362
505,19
1248,47
264,432
1280,767
1031,100
1158,869
48,861
334,653
1135,822
1175,595
884,885
512,85
484,745
900,449
157,852
497,266
946,632
217,236
256,661
248,558
47,570
1103,766
1215,425
842,672
947,362
996,710
1033,75
552,812
62,798
862,702
756,803
3,466
934,677
1252,521
1220,103
570,499
997,438
1245,677
30,753
900,673
1119,593
716,754
264,350
420,484
1144,87
6,695
493,504
669,231
1051,695
97,522
97,565
954,463
674,595
63,292
961,409
348,848
166,359
686,767
192,575
848,319
925,477
735,182
326,107
470,117
1183,620
370,822
349,37
957,78
209,781
30,464
62,803
94,884
542,36
1307,793
398,681
604,450
376,329
175,520
495,820
764,504
768,858
241,341
502,869
1,726
252,675
157,490
672,427
1059,355
1151,820
319,168
1016,719
726,302
895,371
527,620
1151,436
805,647
540,705
1153,547
1304,199
817,367
88,126
1074,439
1289,140
1096,702
1243,359
739,401
448,702
858,85
810,291
1120,754
1190,702
239,523
805,215
920,350
1176,211
853,221
985,661
428,670
341,438
566,129
333,714
853,124
624,767
1240,368
641,250
1236,227
1218,484
1303,840
587,871
408,892
1004,838
109,210
716,140
1093,658
773,7
654,309
1049,677
674,530
318,831
1118,767
997,456
740,717
1238,791
848,127
541,259
830,8
363,219
584,526
672,651
353,816
457,322
112,529
1004,56
161,390
900,1
169,135
25,862
584,220
872,123
893,778
186,672
341,194
303,268
992,383
465,368
13,25
1168,240
311,516
713,701
646,607
1240,526
510,558
723,359
842,558
726,880
979,757
1202,257
495,773
70,686
753,522
94,688
753,428
273,532
197,501
485,722
840,848
420,816
186,149
698,403
363,676
1262,861
512,112
1201,658
129,98
1022,309
1263,324
569,660
74,675
80,409
177,232
455,4
818,527
480,397
186,222
1114,581
783,620
1201,236
197,893
1118,95
489,728
388,140
604,242
1216,240
797,754
756,539
797,37
1173,872
622,623
489,815
117,143
291,184
863,128
730,765
73,722
892,7
488,535
813,628
940,822
552,318
273,840
1086,289
1280,464
822,311
149,295
477,697
391,588
1237,348
262,483
6,199
584,592
485,539
390,126
490,738
1170,373
1140,297
934,637
396,436
1233,385
733,765
947,219
349,485
490,746
643,338
1084,511
979,122
408,555
820,290
669,25
1253,701
813,72
999,378
1176,855
1124,745
132,780
706,204
581,157
667,556
562,410
808,869
428,383
3,101
996,632
261,746
577,10
830,886
489,838
344,742
1136,845
398,661
398,238
1019,787
142,649
584,302
438,771
288,302
1161,295
92,634
606,145
970,632
1034,74
1079,894
142,240
1275,710
820,746
1173,22
74,577
855,890
689,501
497,347
358,688
636,364
567,819
1022,302
758,736
1253,253
428,501
821,815
887,21
1213,490
764,705
60,306
1153,255
1056,327
601,168
1099,634
880,718
1146,604
1213,18
1178,780
822,535
1161,471
952,127
1201,210
688,623
574,221
890,634
170,597
157,42
1084,393
310,661
666,8
375,548
597,193
390,544
65,677
263,180
621,449
1165,714
842,745
582,473
997,472
1145,815
25,32
820,156
612,520
957,302
770,705
113,187
1114,313
825,411
698,267
813,347
1236,667
723,807
169,815
805,194
1280,114
1223,815
820,820
790,659
850,101
1250,588
955,525
676,782
1113,221
224,289
1290,649
284,421
574,787
1020,632
1026,25
961,530
858,267
1237,157
1238,439
952,206
996,184
1292,364
969,707
546,133
619,555
575,182
584,880
979,137
30,268
477,214
797,857
1039,582
1144,359
211,17
790,235
1180,623
915,224
477,471
261,665
833,295
830,662
601,138
656,361
922,688
1262,145
1231,406
858,361
616,816
1290,270
764,189
320,689
18,530
813,266
363,107
577,436
609,439
560,464
229,490
480,886
281,333
202,267
1078,10
492,527
929,362
373,781
381,586
126,150
246,109
169,68
508,691
638,243
741,234
1081,404
1290,245
1034,298
621,841
821,728
23,44
1260,674
1044,140
217,628
750,688
1140,149
552,736
261,453
192,95
452,361
1140,597
726,368
798,688
519,147
1082,707
1294,19
70,357
840,693
691,362
1148,77
311,68
654,555
90,710
1049,770
1019,184
130,271
403,707
67,87
837,593
1213,404
1294,518
162,186
893,330
698,520
87,75
976,451
157,266
758,576
719,154
903,493
822,583
594,530
70,144
1153,628
463,327
1216,884
654,533
902,709
714,597
141,485
634,112
723,87
833,231
159,325
977,714
206,285
853,572
301,414
1004,400
750,884
358,127
1046,432
520,235
594,812
801,873
1169,37
986,353
1006,110
1099,260
915,695
1062,784
410,143
1300,8
1297,259
348,737
175,822
348,157
1037,54
1014,849
785,673
518,866
716,812
418,119
497,72
1093,236
1069,553
566,765
703,801
1230,409
1029,333
1190,227
94,206
641,886
1268,161
1181,221
417,116
47,324
1124,672
870,140
159,121
440,226
169,31
428,462
440,858
395,672
348,46
236,632
184,848
735,712
470,693
243,782
940,869
154,603
935,548
1039,841
490,143
149,471
920,880
243,112
706,652
817,504
465,884
581,844
914,884
711,536
90,343
952,530
1290,21
135,595
868,749
1238,473
898,667
174,397
1078,688
990,689
1272,368
438,123
400,262
102,812
1184,150
691,666
567,371
982,107
537,887
800,110
735,180
32,623
242,110
632,334
753,789
411,65
1290,737
313,438
900,751
333,180
426,457
261,677
440,140
714,297
527,172
729,546
673,142
457,501
1168,688
1124,149
664,383
848,799
480,381
159,436
867,821
213,243
378,148
947,676
72,473
1133,645
1150,653
653,840
912,233
769,259
1133,871
913,724
22,334";

			return ParseData(data);
		}

		private static List<FoldingInstructions> GetFoldingInstructions()
		{
			string data =
@"fold along x=655
fold along y=447
fold along x=327
fold along y=223
fold along x=163
fold along y=111
fold along x=81
fold along y=55
fold along x=40
fold along y=27
fold along y=13
fold along y=6";

			return ParseFoldingInstructions(data);
		}

		private static char[,] ParseData(string data)
		{
			var rows = data.Split("\r\n");

			int maxX = 0;
			int maxY = 0;

			List<KeyValuePair<int, int>> coordinatesList = new List<KeyValuePair<int, int>>();

			// Get all the coordinates in the list and keep track of the max x and y values
			for (int i = 0; i < rows.Length; i++)
			{
				var stringCoordinates = rows[i].Split(",");

				int x = int.Parse(stringCoordinates[0]);
				int y = int.Parse(stringCoordinates[1]);

				maxX = Math.Max(x, maxX);
				maxY = Math.Max(y, maxY);

				coordinatesList.Add(new KeyValuePair<int, int>(x, y));
			}

			// Create the plot with width and height to accomodate the max values
			var plot = new char[maxX + 1, maxY + 1];

			// Pre-fill the plot with dots
			for (int x = 0; x <= maxX; x++)
			{
				for (int y = 0; y <= maxY; y++)
				{
					plot[x, y] = '.';
				}
			}

			// Replace the dot with an octothorpe for each set of coordinates
			foreach (var coordinates in coordinatesList)
			{
				plot[coordinates.Key, coordinates.Value] = '#';
			}

			return plot;
		}

		private static List<FoldingInstructions> ParseFoldingInstructions(string data)
		{
			List<FoldingInstructions> instructions = new List<FoldingInstructions>();

			var rows = data.Split("\r\n");

			foreach (string row in rows)
			{
				bool isHorizontalFold = row[11] == 'y';
				int foldingLine = int.Parse(row.Substring(13));

				instructions.Add(new FoldingInstructions(isHorizontalFold, foldingLine));
			}

			return instructions;
		}
	}

	public class FoldingInstructions
	{
		public bool IsHorizontalFold { get; private set; }
		public bool IsVerticalFold => !IsHorizontalFold;
		public int FoldingLine { get; set; }

		public FoldingInstructions(bool isHorizontalFold, int foldingLine)
		{
			IsHorizontalFold = isHorizontalFold;
			FoldingLine = foldingLine;
		}

		public override string ToString()
		{
			return $"fold along {(IsHorizontalFold ? "y" : "x")}={FoldingLine}";
		}
	}
}
