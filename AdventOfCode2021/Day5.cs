﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day5
	{
		public static int Run()
		{
			var lines = GetLines();

			var result = RunPart1(lines);
			//var result = RunPart2(lines);

			return result;
			// 21101
		}

		// Get the bingo board that would win last
		private static int RunPart2(List<Line> lines)
		{
			

			return 0;
		}

		// Get the bingo board that would win first
		private static int RunPart1(List<Line> lines)
		{
			int overallMaxX = lines.Max(p => Math.Max(p.StartingPoint.X, p.EndingPoint.X));
			int overallMaxY = lines.Max(p => Math.Max(p.StartingPoint.Y, p.EndingPoint.Y));
			int[,] ventCounts = new int[overallMaxX + 1, overallMaxY + 1];

			foreach (var line in lines)
			{
				if (line.IsHorizontal)
				{
					int minY = Math.Min(line.StartingPoint.Y, line.EndingPoint.Y);
					int maxY = Math.Max(line.StartingPoint.Y, line.EndingPoint.Y);

					for (int y = minY; y <= maxY; y++)
					{
						ventCounts[line.StartingPoint.X, y]++;
					}
				}
				else if (line.IsVertical)
				{
					int minX = Math.Min(line.StartingPoint.X, line.EndingPoint.X);
					int maxX = Math.Max(line.StartingPoint.X, line.EndingPoint.X);

					for (int x = minX; x <= maxX; x++)
					{
						ventCounts[x, line.StartingPoint.Y]++;
					}
				}
				// Part 2
				else if (line.IsDiagonal)
				{
					int length = Math.Abs(line.StartingPoint.X - line.EndingPoint.X) + 1;
					int deltaX = line.StartingPoint.X < line.EndingPoint.X ? 1 : -1;
					int deltaY = line.StartingPoint.Y < line.EndingPoint.Y ? 1 : -1;

					for (int i = 0; i < length; i++)
					{
						int currentX = line.StartingPoint.X + (i * deltaX);
						int currentY = line.StartingPoint.Y + (i * deltaY);
						ventCounts[currentX, currentY]++;
					}
				}
			}

			int ventCountsWithTwoOrMore = 0;

			for (int x = 0; x < overallMaxX; x++)
			{
				for (int y = 0; y < overallMaxY; y++)
				{
					if (ventCounts[x, y] > 1)
					{
						ventCountsWithTwoOrMore++;
					}
				}
			}

			return ventCountsWithTwoOrMore;
		}

		private static List<Line> GetLinesTest()
		{
			List<Line> lines = new List<Line>();

			string data =
@"1,1 -> 3,3
9,7 -> 7,9";

			var sets = data.Split("\r\n");

			foreach (var set in sets)
			{
				var points = set.Split(" -> ");

				var startingCoordinates = points[0].Split(",");
				var endingCoordinates = points[1].Split(",");

				lines.Add(new Line
				{
					StartingPoint = new Point
					{
						X = int.Parse(startingCoordinates[0]),
						Y = int.Parse(startingCoordinates[1])
					},
					EndingPoint = new Point
					{
						X = int.Parse(endingCoordinates[0]),
						Y = int.Parse(endingCoordinates[1])
					}
				});
			}

			return lines;
		}

		private static List<Line> GetLines()
		{
			List<Line> lines = new List<Line>();

			string data =
@"242,601 -> 242,18
938,357 -> 938,128
920,574 -> 750,574
804,978 -> 804,813
955,932 -> 68,45
232,604 -> 232,843
69,570 -> 467,968
355,432 -> 611,688
945,19 -> 700,19
904,932 -> 904,918
455,65 -> 516,65
571,485 -> 588,485
717,142 -> 217,142
377,344 -> 66,344
510,818 -> 132,818
848,709 -> 848,950
785,50 -> 785,857
23,981 -> 971,33
938,45 -> 938,327
212,402 -> 601,13
749,142 -> 651,240
94,930 -> 22,930
436,467 -> 820,851
544,265 -> 458,265
517,708 -> 517,785
957,893 -> 957,22
684,610 -> 526,452
713,687 -> 526,687
220,781 -> 988,13
12,45 -> 912,945
854,677 -> 646,677
382,498 -> 382,64
676,879 -> 148,351
809,52 -> 336,525
959,951 -> 41,33
943,162 -> 132,973
897,732 -> 897,308
21,196 -> 702,877
938,972 -> 656,972
798,139 -> 90,847
213,597 -> 582,966
248,955 -> 973,230
985,606 -> 985,885
166,693 -> 804,693
807,897 -> 28,118
433,306 -> 433,447
899,61 -> 60,900
984,582 -> 691,582
803,583 -> 910,583
348,142 -> 348,244
352,775 -> 352,430
240,285 -> 240,406
394,541 -> 394,655
887,622 -> 298,33
62,37 -> 861,836
819,136 -> 29,926
717,332 -> 717,408
709,63 -> 276,496
384,441 -> 150,441
292,251 -> 557,516
518,311 -> 52,777
50,735 -> 479,306
932,865 -> 139,72
43,21 -> 982,960
63,927 -> 796,194
958,351 -> 958,623
643,451 -> 35,451
534,14 -> 459,14
20,649 -> 924,649
983,18 -> 35,966
84,668 -> 203,668
40,654 -> 748,654
474,760 -> 85,371
512,431 -> 272,431
588,93 -> 112,569
648,687 -> 832,687
988,867 -> 116,867
979,46 -> 94,931
242,307 -> 800,865
100,204 -> 807,911
890,962 -> 88,962
273,510 -> 273,201
184,748 -> 813,119
214,915 -> 950,179
960,975 -> 89,104
853,347 -> 853,79
853,308 -> 884,308
245,394 -> 245,640
850,554 -> 604,800
141,159 -> 141,378
635,632 -> 897,894
352,182 -> 550,182
748,613 -> 748,887
531,664 -> 255,388
785,414 -> 432,767
374,457 -> 653,736
451,535 -> 444,535
600,179 -> 434,13
489,605 -> 845,961
658,786 -> 658,196
305,556 -> 305,914
820,368 -> 204,984
903,70 -> 548,425
840,450 -> 796,494
289,183 -> 768,662
21,54 -> 950,983
765,294 -> 209,850
467,511 -> 703,747
354,645 -> 730,645
176,30 -> 964,818
290,259 -> 345,259
868,945 -> 96,173
536,884 -> 536,94
415,177 -> 415,99
250,140 -> 466,140
900,107 -> 900,249
74,394 -> 137,394
364,957 -> 364,81
718,477 -> 718,227
27,14 -> 988,975
491,956 -> 154,956
289,283 -> 289,225
479,583 -> 604,583
581,406 -> 23,964
837,526 -> 732,526
417,435 -> 417,206
502,184 -> 20,666
903,754 -> 817,668
381,284 -> 967,870
31,17 -> 176,17
225,377 -> 179,377
316,932 -> 358,890
605,841 -> 559,841
865,193 -> 865,827
836,834 -> 142,140
229,610 -> 232,610
26,13 -> 872,859
26,444 -> 26,79
272,690 -> 531,949
964,954 -> 331,954
545,91 -> 335,91
906,942 -> 906,301
608,778 -> 608,364
475,723 -> 475,710
454,207 -> 614,207
200,180 -> 917,897
966,44 -> 71,939
288,252 -> 288,413
795,791 -> 66,62
81,39 -> 588,546
249,244 -> 892,244
483,579 -> 853,579
220,921 -> 220,286
917,834 -> 675,834
569,692 -> 569,521
344,586 -> 835,95
116,153 -> 888,925
681,52 -> 871,242
980,976 -> 27,23
828,567 -> 482,567
660,432 -> 660,441
826,379 -> 280,379
42,839 -> 259,622
743,23 -> 91,23
318,400 -> 318,528
539,745 -> 734,940
831,194 -> 831,210
582,630 -> 361,851
284,900 -> 213,829
52,855 -> 763,855
215,753 -> 452,753
290,187 -> 417,187
69,48 -> 69,126
76,628 -> 76,365
257,694 -> 54,694
755,713 -> 556,912
519,265 -> 342,265
193,319 -> 193,651
496,231 -> 900,231
83,942 -> 83,524
524,59 -> 989,524
288,800 -> 907,181
458,138 -> 586,138
338,244 -> 934,840
843,728 -> 843,476
42,634 -> 657,19
827,634 -> 369,176
779,900 -> 779,503
20,20 -> 870,870
467,241 -> 467,142
677,483 -> 501,483
10,989 -> 989,10
11,989 -> 989,11
244,750 -> 244,607
479,497 -> 48,928
372,341 -> 615,341
817,941 -> 339,941
352,67 -> 352,581
590,747 -> 590,405
524,26 -> 524,37
501,300 -> 117,300
265,194 -> 491,420
397,891 -> 983,305
423,717 -> 423,922
197,863 -> 197,217
12,91 -> 379,91
364,426 -> 364,185
649,835 -> 649,309
517,380 -> 485,380
328,469 -> 568,469
781,298 -> 781,264
25,794 -> 25,197
570,744 -> 570,544
664,352 -> 632,320
528,944 -> 528,696
242,44 -> 900,702
486,775 -> 486,556
608,245 -> 788,245
114,11 -> 114,508
751,560 -> 751,884
211,513 -> 448,513
389,219 -> 308,300
638,200 -> 105,200
258,243 -> 365,243
120,558 -> 556,122
787,166 -> 274,166
617,666 -> 185,234
537,172 -> 808,172
633,980 -> 282,980
270,150 -> 270,225
925,32 -> 48,909
979,14 -> 891,102
98,278 -> 98,485
333,771 -> 119,771
132,673 -> 132,189
416,470 -> 482,404
762,151 -> 925,151
148,721 -> 378,491
255,576 -> 255,474
21,48 -> 938,965
876,615 -> 777,615
713,209 -> 209,209
250,474 -> 271,453
684,71 -> 451,71
406,614 -> 519,501
479,252 -> 112,252
721,768 -> 284,331
290,344 -> 290,111
359,934 -> 544,934
754,976 -> 726,976
358,544 -> 358,904
597,344 -> 597,581
915,222 -> 915,255
931,160 -> 135,956
160,657 -> 348,657
35,942 -> 949,28
298,837 -> 298,356
540,195 -> 540,119
29,140 -> 29,955
118,117 -> 980,979
240,384 -> 464,608
677,667 -> 361,351
982,987 -> 11,16
638,770 -> 95,227
135,285 -> 135,349
843,313 -> 843,529
208,220 -> 945,957
450,889 -> 977,362
876,69 -> 283,69
57,586 -> 57,231
602,78 -> 602,564
708,704 -> 267,704
697,336 -> 697,264
564,522 -> 519,567
195,217 -> 274,138
35,885 -> 116,804
680,28 -> 148,28
736,34 -> 736,616
918,454 -> 52,454
143,40 -> 415,40
985,469 -> 985,282
804,703 -> 107,703
707,59 -> 296,470
37,935 -> 931,41
45,723 -> 45,531
897,959 -> 165,227
691,948 -> 523,948
545,560 -> 545,45
251,24 -> 748,521
625,506 -> 625,626
302,702 -> 989,15
489,926 -> 489,507
405,830 -> 405,871
736,851 -> 19,134
712,848 -> 48,184
925,914 -> 33,22
593,254 -> 369,478
965,691 -> 155,691
758,931 -> 349,522
64,135 -> 820,891
79,933 -> 79,683
609,454 -> 233,454
617,853 -> 309,545
695,130 -> 695,578
508,198 -> 363,198
184,414 -> 275,505
627,901 -> 519,901
765,715 -> 213,715
445,134 -> 669,134
785,33 -> 302,516
563,218 -> 470,125
136,461 -> 264,461
523,643 -> 674,643
473,695 -> 473,235
616,835 -> 757,976
406,763 -> 406,224
483,111 -> 203,111
70,863 -> 922,11
738,141 -> 738,54
146,697 -> 332,883
939,16 -> 23,932
836,15 -> 317,534
853,586 -> 853,596
733,377 -> 733,461
378,597 -> 378,640
522,225 -> 522,78
875,886 -> 875,130
302,83 -> 771,83
969,588 -> 419,38
268,159 -> 585,476
658,955 -> 33,330
940,149 -> 492,149
157,254 -> 962,254
265,778 -> 265,365
414,494 -> 608,494
27,959 -> 948,38
220,160 -> 220,891
836,316 -> 836,179
843,727 -> 163,47
225,695 -> 598,695
678,249 -> 892,249
938,36 -> 938,170
190,486 -> 40,336
815,256 -> 815,866
961,200 -> 961,89
67,895 -> 67,853
480,727 -> 852,727
334,94 -> 334,452
67,622 -> 987,622
48,29 -> 982,963
90,29 -> 963,902
859,739 -> 338,739
869,254 -> 474,649
196,43 -> 69,43
336,439 -> 336,837
248,387 -> 587,48
378,729 -> 162,513
699,658 -> 513,844
447,410 -> 670,410
739,593 -> 889,443
83,970 -> 964,89
276,406 -> 276,191
860,75 -> 247,688
435,858 -> 435,905
691,893 -> 691,757
136,896 -> 611,421
693,211 -> 477,427
181,793 -> 181,717
674,326 -> 664,336
938,826 -> 164,52
833,380 -> 833,753
833,349 -> 230,952
662,870 -> 662,23
974,511 -> 145,511
38,579 -> 57,579
966,965 -> 966,498
641,217 -> 240,618
418,986 -> 834,986
971,716 -> 971,263
254,313 -> 254,823
61,790 -> 61,834
262,439 -> 262,864
345,856 -> 894,307
736,862 -> 281,862
814,636 -> 814,240
853,865 -> 853,22
792,106 -> 207,106
647,303 -> 531,303
506,706 -> 337,706
402,140 -> 402,958
899,796 -> 669,796
806,619 -> 463,276
340,347 -> 340,363
18,21 -> 979,982
395,214 -> 395,862
228,330 -> 333,330
723,950 -> 723,150
392,298 -> 36,298
916,118 -> 114,920
210,854 -> 80,724
212,206 -> 513,507
44,659 -> 161,659
771,44 -> 198,617
485,706 -> 169,706
385,455 -> 308,455
390,317 -> 390,385
492,532 -> 56,968
237,674 -> 712,674
988,909 -> 254,175
86,276 -> 448,276
688,418 -> 927,179
667,773 -> 504,610
968,974 -> 109,115
843,54 -> 843,265
19,249 -> 19,437
307,326 -> 341,360
531,891 -> 531,202
281,535 -> 270,546
503,305 -> 164,644
170,971 -> 30,971
763,247 -> 946,247
795,920 -> 623,920
673,16 -> 899,16
785,845 -> 290,845
68,614 -> 68,711
284,984 -> 67,984
787,942 -> 120,942
953,369 -> 773,549
927,727 -> 315,115
884,686 -> 254,56
432,276 -> 432,287
658,99 -> 81,676
622,917 -> 679,917
938,978 -> 938,793
945,15 -> 369,15
603,709 -> 603,74
670,422 -> 222,870
190,702 -> 190,362
354,349 -> 369,334
26,880 -> 876,30
636,31 -> 636,731
778,628 -> 778,25
23,483 -> 170,483
23,972 -> 963,32
725,308 -> 384,308
97,962 -> 620,962
136,929 -> 136,768
656,295 -> 851,295
125,801 -> 755,171
120,32 -> 553,32
698,196 -> 286,608
66,721 -> 66,836
931,680 -> 931,499
862,449 -> 862,743
71,143 -> 180,252
510,327 -> 612,225
932,874 -> 352,874
599,372 -> 583,372
821,770 -> 126,75
317,186 -> 495,186
557,710 -> 56,209
895,866 -> 306,277
571,948 -> 571,738
287,864 -> 243,864
802,728 -> 802,198
711,642 -> 983,642
969,922 -> 969,645
89,417 -> 57,385
567,967 -> 567,781
350,498 -> 142,498
92,931 -> 988,35
980,940 -> 152,112
55,944 -> 679,320
669,410 -> 669,679
151,431 -> 241,431
984,882 -> 80,882
431,374 -> 431,39
30,91 -> 765,826
730,228 -> 80,878
379,570 -> 705,570
67,398 -> 67,136
491,515 -> 491,344
396,453 -> 749,453
203,660 -> 203,579
912,900 -> 912,280
909,88 -> 367,88
41,942 -> 825,158
724,417 -> 17,417
463,536 -> 170,536
715,737 -> 715,134
627,453 -> 805,453
934,795 -> 695,556
404,729 -> 738,729
973,685 -> 973,310
563,348 -> 771,556
716,232 -> 983,232
975,183 -> 975,759
934,958 -> 117,958
538,806 -> 538,84
695,677 -> 629,677";

			var sets = data.Split("\r\n");

			foreach (var set in sets)
			{
				var points = set.Split(" -> ");

				var startingCoordinates = points[0].Split(",");
				var endingCoordinates = points[1].Split(",");

				lines.Add(new Line
				{
					StartingPoint = new Point
					{
						X = int.Parse(startingCoordinates[0]),
						Y = int.Parse(startingCoordinates[1])
					},
					EndingPoint = new Point
					{
						X = int.Parse(endingCoordinates[0]),
						Y = int.Parse(endingCoordinates[1])
					}
				});
			}

			return lines;
		}
	}

	internal class Line
	{
		public Point StartingPoint { get; set; }
		public Point EndingPoint { get; set; }

		public bool IsHorizontal => StartingPoint.X == EndingPoint.X;
		public bool IsVertical => StartingPoint.Y == EndingPoint.Y;
		public bool IsDiagonal => !IsHorizontal && !IsVertical
			&& Math.Abs(StartingPoint.X - EndingPoint.X) == Math.Abs(StartingPoint.Y - EndingPoint.Y);

		public override string ToString()
		{
			return $"{StartingPoint} -> {EndingPoint}";
		}
	}

	internal class Point
	{
		public int X { get; set; }
		public int Y { get; set; }

		public override string ToString()
		{
			return $"{X},{Y}";
		}
	}
}
