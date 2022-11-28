﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day19
	{
		public const int OverlappingBeaconCount = 12;

		public static int Run()
		{
			bool isTest = false;
			var data = isTest ? GetTestData() : GetData();

			var result = RunPart1(data); // test: 79, 419
			//var result = RunPart2(data);

			// 339 too low
			// 371 too low
			// 500 too high
			// 335
			// 383
			// 395
			// 407
			// 419 ding ding!!

			return result;
		}

		private static int RunPart2(List<Scanner> scanners)
		{
			return 0;
		}

		private static int RunPart1(List<Scanner> scanners)
		{
			int beaconCount = 0;
			int overlapCount = 0;

			for (int i = 0; i < scanners.Count; i++)
			{
				var scanner = scanners[i];
				beaconCount += scanner.Beacons.Count;

				for (int j = i + 1; j < scanners.Count; j++)
				{
					var otherScanner = scanners[j];

					// Attempt 1
					//var hashes = scanner.GetBeaconDifferenceHashes();
					//var otherHashes = otherScanner.GetBeaconDifferenceHashes();

					//var overlappingHashes = hashes.Intersect(otherHashes).ToList();

					// Attempt 2
					//var differences = scanner.GetBeaconDifferences();
					//var otherDifferences = otherScanner.GetBeaconDifferences();

					//var overlappingDifferencesCount = GetCommonDifferencesCount(differences, otherDifferences);

					// Attempt 3
					var squaredDistances = scanner.GetSquaredDistancesBetweenBeacons();
					var otherSquaredDistances = otherScanner.GetSquaredDistancesBetweenBeacons();

					var overlappingDistances = squaredDistances.Intersect(otherSquaredDistances).ToList();

					//beaconCount -= overlappingHashes.Count;

					//if (overlappingHashes.Count >= 12)
					//if (overlappingDifferencesCount >= 12)
					// If there are at least 12 beacons that are the same between both scanners,
					// then there should be at least 12 choose 2 squared distances in both lists.
					if (overlappingDistances.Count >= GetCombination(OverlappingBeaconCount, 2))
					{
						Console.WriteLine($"Scanner {scanner.Id} overlaps with scanner {otherScanner.Id}");
						//beaconCount -= overlappingHashes.Count;
						//beaconCount -= overlappingDifferencesCount;
						//beaconCount -= overlappingDistances.Count;
						//beaconCount -= 12;

						// Check whether there are only 12 overlapping beacons or more
						int overlappingBeaconCount = OverlappingBeaconCount;
						int overlappingDistancesCount = (int)GetCombination(overlappingBeaconCount, 2);
						while (overlappingDistances.Count > overlappingDistancesCount)
						{
							overlappingBeaconCount++;
							overlappingDistancesCount = (int)GetCombination(overlappingBeaconCount, 2);
						}

						beaconCount -= overlappingBeaconCount;
						overlapCount++;
					}
				}
			}

			Console.WriteLine("There are potentially up to " + scanners.Select(s => s.Beacons.Count).Aggregate((current, next) => current + next) + " beacons"); // 959
			Console.WriteLine("There were " + overlapCount + " overlaps"); // 335 with 52
			// but real answer is 419, so there must actually only be 45 overlaps (I have 7 false positives)


			//List<int> beaconhashes = new List<int>();
			//foreach (var scanner in scanners)
			//{
			//	foreach (var beacon in scanner.Beacons)
			//	{
			//		beaconhashes.Add(beacon.BeaconHash);
			//	}
			//}

			//Console.WriteLine("Count of beacon hashes: " + beaconhashes.Count);
			//Console.WriteLine("Count of unique beacon hashes: " + beaconhashes.Distinct().ToList().Count);

			//// Start by assuming scanner0 has the "correct" orientation
			//var scanner0 = scanners[0];

			//// Find a scanner whose dection cube overlaps with scanner0's detection cube (they both detect the same 12 beacons)
			//foreach (var scanner in scanners)
			//{
			//	if (scanner == scanner0)
			//	{
			//		continue;
			//	}

			//	// Check beacon by beacon for matches
			//	foreach (var scanner0Beacon in scanner0.Beacons)
			//	{
			//		// Start by assuming the first beacon of each match

			//		// If we aren't able to find at least 12 beacons that match,
			//		// then



			//		// Check each orientation of each of the scanner's beacons
			//		for (int i = 0; i < 24; i++)
			//		{
			//			foreach (var beacon in scanner.GetBeaconsByOrientation(i))
			//			{
			//				int xDiff = beacon.X - scanner0Beacon.X;
			//				int yDiff = beacon.Y - scanner0Beacon.Y;
			//				int zDiff = beacon.Z - scanner0Beacon.Z;
			//			}
			//		}
			//	}
			//}


			//// For each possible orientation for this scanner (should be 24)
			//for (int i = 0; i < scanner0.Beacons[0].GetOrientations().Count; i++)
			//{

			//}

			//scanners[0].Beacons[0].GetOrientations();
			return beaconCount;
		}

		// Returns n choose r
		public static long GetCombination(int n, int r)
		{
			long result = 1;
			for (int i = 1; i <= r; i++)
			{
				result *= n - (r - i);
				result /= i;
			}
			return result;
		}

		public static int GetCommonDifferencesCount(List<BeaconDifference> differences, List<BeaconDifference> otherDifferences)
		{
			int count = 0;

			// Compare each difference between two beacons seen from one scanner with
			// each difference between two beacons seen by the other scanner
			for (int i = 0; i < differences.Count; i++)
			{
				var difference = differences[i];

				for (int j = i + 1; j < otherDifferences.Count; j++)
				{
					var otherDifference = otherDifferences[j];

					if (difference.Equals(otherDifference))
					{
						count++;
					}
				}
			}

			return count;
		}

		private static List<Scanner> GetTestData()
		{
			string data =
@"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14";

			//			string data =
			//@"--- scanner 0 ---
			//3,2,1";

			return ParseData(data);
		}

		private static List<Scanner> GetData()
		{
			string data =
@"--- scanner 0 ---
-640,638,699
526,552,850
515,819,-585
416,-509,-431
-813,475,-525
-533,-621,504
579,482,859
606,435,862
-444,-581,-268
600,889,-528
514,747,-569
409,-497,-367
478,-646,471
-445,-498,-466
-532,-606,543
-118,3,33
-705,564,663
525,-637,586
-639,-535,609
-464,-397,-310
-864,575,-526
570,-663,589
-891,627,708
447,-367,-483
-732,513,-463

--- scanner 1 ---
902,878,-690
-606,551,-624
-553,551,-780
-292,410,805
-457,-795,549
886,646,719
751,660,776
-716,-303,-464
103,133,83
-9,37,-16
-422,-790,563
954,767,-617
-653,-330,-444
717,-659,595
560,-696,-682
-332,538,828
621,-746,491
799,548,709
-441,-708,436
515,-707,574
-641,-246,-486
-227,431,807
-587,459,-728
969,955,-580
497,-705,-710
449,-662,-799

--- scanner 2 ---
760,-747,-581
799,381,596
-633,323,841
805,351,849
478,-687,558
-144,-29,-85
-8,32,45
793,449,775
518,-828,529
-735,-657,-697
-421,721,-758
593,-804,574
375,728,-691
-601,531,870
773,-840,-584
-467,658,-881
-573,-648,321
-747,-722,-598
-601,477,893
-519,-819,365
435,579,-702
-669,-643,-531
-529,-621,346
377,692,-811
-442,817,-869
752,-785,-625

--- scanner 3 ---
642,-715,461
822,-784,-512
-690,675,-457
490,399,782
600,-800,334
400,726,-673
-699,903,274
-778,893,443
-365,-598,400
715,-745,-626
-505,-411,-495
556,726,-726
563,510,834
-301,-590,318
70,-73,-68
738,-649,-498
-404,-768,310
-720,566,-488
610,-698,276
-629,498,-498
600,322,849
-347,-313,-517
-512,-311,-417
-51,109,63
452,818,-707
-641,837,443

--- scanner 4 ---
-595,-753,286
-864,-859,-627
619,238,-868
-781,599,655
502,-571,-787
300,700,706
-705,734,711
399,-821,710
-95,-164,-43
-666,414,-815
-943,-856,-477
-788,385,-935
425,-876,724
430,665,729
503,729,716
441,-748,-828
-30,-3,-131
-788,547,-814
411,-639,-797
429,-928,844
-863,661,653
-696,-803,296
784,240,-768
626,338,-790
-537,-746,370
-916,-767,-618

--- scanner 5 ---
-691,-798,726
112,-55,96
-645,472,-662
-701,-834,-390
852,562,653
-417,654,713
784,-593,520
-517,495,-758
415,590,-577
557,631,-663
485,647,-662
865,520,500
-779,-812,-566
654,-538,484
-470,810,713
752,-788,-651
822,-797,-598
-796,-777,833
818,-514,428
-809,-778,-364
-436,758,805
-731,-775,854
852,575,405
-631,388,-797
869,-800,-571

--- scanner 6 ---
635,-690,347
-554,-698,885
-602,-754,862
440,720,-609
622,-743,-496
-664,-653,-423
-738,344,-754
741,-766,394
560,-813,-427
639,-758,502
429,619,-626
-711,-677,-487
515,-757,-419
-741,289,656
297,585,-606
-788,-784,-441
-722,371,783
-501,-775,789
766,263,503
808,337,515
-140,-89,-92
669,264,591
-747,299,-821
19,-140,76
-734,485,-875
-706,284,670

--- scanner 7 ---
493,599,478
-855,-753,557
421,-360,775
-451,605,657
-596,-261,-456
-748,721,-241
-580,-348,-343
555,629,-558
578,-397,-502
-9,-1,15
-792,-726,537
310,-394,892
-564,602,708
478,578,-631
-561,-266,-470
-84,95,150
376,-349,-481
482,-310,-398
358,-351,803
481,514,514
-780,-810,579
-700,777,-264
-801,798,-337
521,666,482
569,601,-797
-586,671,744

--- scanner 8 ---
-369,840,711
877,770,-612
-315,-575,744
-306,-478,-351
899,715,-432
166,112,159
30,89,14
-406,-371,-271
889,882,-514
487,-547,726
525,-608,690
-360,-503,-261
519,-534,473
-261,834,784
-536,758,-346
-560,912,-383
956,622,476
429,-546,-538
867,614,591
-504,-487,724
517,-733,-553
-7,-52,137
-418,844,881
898,662,594
-410,866,-386
451,-641,-671
-439,-677,716

--- scanner 9 ---
460,-778,662
-710,-610,-676
-564,339,497
-310,-826,567
-646,-622,-669
773,205,934
469,-942,-375
121,-198,15
699,333,-723
601,-749,622
640,-986,-429
-619,326,592
772,320,-659
803,339,827
-508,634,-571
667,-961,-385
18,-142,181
484,-733,737
-492,706,-570
-577,339,745
-594,-480,-638
-549,776,-485
902,343,-694
-264,-837,676
777,325,882
-277,-839,448

--- scanner 10 ---
873,690,-787
-521,-360,409
900,670,-850
541,-620,-420
-453,-758,-705
-633,-384,343
957,648,-775
473,-559,-390
-367,713,486
541,335,315
-455,519,-559
-444,-602,-645
498,424,363
-24,38,-24
-423,-383,364
-519,745,526
445,-595,-502
507,452,426
-524,483,-581
-391,-563,-703
577,-481,392
38,-90,-140
-458,570,-446
660,-601,384
-452,749,498
479,-537,409

--- scanner 11 ---
737,747,-445
-441,-754,-682
887,-526,-511
402,653,561
401,750,649
-471,705,-898
-620,623,650
24,-155,0
-616,696,-793
422,-722,482
-628,725,758
971,-538,-553
-552,619,805
-616,-618,290
914,-463,-531
509,-738,369
475,792,637
667,636,-350
-510,-732,-718
-444,-867,-631
-560,-570,267
152,-10,-36
-481,720,-786
794,794,-340
-637,-517,363
427,-757,496

--- scanner 12 ---
393,689,-866
-563,827,-867
-478,-639,527
590,-774,722
658,-790,-796
595,668,-818
-547,-710,-894
-113,-47,-89
-551,-831,-753
499,772,-905
766,-828,-729
-414,458,251
-544,700,-949
590,-786,-747
-349,456,272
764,544,487
513,-732,593
-538,-777,-939
-579,714,-954
677,554,389
629,487,501
-548,-558,508
-454,435,379
-646,-614,473
40,84,-35
520,-631,652

--- scanner 13 ---
851,779,473
-577,-836,-725
889,-776,-863
14,-23,30
-322,-965,370
924,640,549
-479,467,-834
-523,-668,-700
-429,-860,349
45,-160,-146
904,-735,628
-671,485,609
899,-770,614
569,400,-668
468,411,-651
-635,615,554
817,-751,741
703,-763,-917
603,268,-648
-727,649,633
925,737,461
-327,388,-843
-558,-766,-644
-532,417,-823
844,-767,-796
-301,-900,372

--- scanner 14 ---
790,415,-867
-602,757,-746
717,772,764
-468,544,536
-547,852,-733
146,141,53
778,595,805
919,-539,-700
-1,-35,47
794,467,-747
662,-669,856
-819,-430,-831
-560,604,-733
-606,-253,501
964,-643,-633
832,-556,-707
-364,452,555
637,-765,774
-648,-337,489
808,704,774
-312,544,589
820,522,-733
601,-737,886
-714,-487,-784
-809,-557,-673
-628,-310,426

--- scanner 15 ---
631,-685,-788
-733,-794,-800
-640,667,-534
520,-405,585
-684,761,-523
508,-598,673
-470,727,594
105,-2,-71
544,696,-618
-710,723,-651
508,-674,-705
-768,-612,668
-663,-628,-771
585,599,-563
573,724,-644
-30,-39,63
583,-483,635
-436,629,595
681,-703,-617
807,470,423
-761,-689,467
-410,695,451
-646,-811,-697
829,451,315
781,425,509
-833,-576,561

--- scanner 16 ---
732,498,-660
348,628,698
-660,-420,-471
-726,-402,-476
625,-473,-723
647,492,-532
-656,-460,-426
-781,589,707
436,-428,603
-680,-640,608
-550,603,-721
376,-427,706
-808,728,582
639,-511,-653
469,598,641
-495,805,-757
621,-277,-677
758,466,-564
-95,80,-6
430,535,716
-799,-654,461
-810,768,624
512,-517,693
-406,671,-787
-811,-647,695

--- scanner 17 ---
637,452,399
-794,570,-449
499,-804,497
-676,791,683
-708,-503,345
483,-876,498
543,-532,-830
540,-946,583
-617,-741,-607
-911,-513,374
758,340,-481
-123,-95,59
800,524,-461
542,-632,-881
-478,772,693
-549,-783,-557
-649,816,639
701,440,392
605,-570,-749
-498,-638,-585
-781,-490,409
602,441,-462
-750,500,-600
-108,27,-126
-856,415,-520
585,310,430

--- scanner 18 ---
-828,474,-453
617,664,823
840,-263,778
616,-655,-732
11,-15,15
-806,513,-423
-428,-757,439
-358,-643,-665
553,845,798
691,725,-400
703,-319,778
497,747,815
409,-696,-751
-403,-517,-769
-319,783,892
-501,-646,387
-392,-718,-723
-436,817,781
600,-673,-799
120,119,133
761,-253,947
725,727,-514
654,609,-522
-538,-834,372
-674,484,-356
-408,826,844

--- scanner 19 ---
-453,634,-387
-523,-689,859
795,-756,731
529,-352,-560
-80,98,145
799,-769,943
-411,-591,871
537,-340,-561
718,-788,809
-122,-62,17
-578,-509,881
-631,517,880
496,875,787
-535,-423,-554
-449,804,-436
638,718,-482
-427,-463,-428
688,660,-552
505,758,835
-671,399,938
670,-349,-569
-459,707,-279
-381,-460,-578
643,700,-548
487,780,923
-679,514,849

--- scanner 20 ---
801,-761,-969
107,-65,-68
-378,-403,522
-29,-119,-181
-598,-728,-752
-450,307,-695
-505,735,695
759,644,-524
818,433,671
666,-692,513
791,-858,-815
-404,444,-696
885,644,-600
-561,357,-698
783,351,505
762,-791,-947
-444,-547,590
550,-814,550
-601,761,727
-608,-608,-688
816,599,-609
-408,-492,388
-633,650,701
-583,-776,-605
548,-700,584
824,362,664

--- scanner 21 ---
278,-737,-669
480,727,642
-161,-60,-4
-965,417,357
389,737,574
-886,567,354
-430,-585,-594
-22,68,-97
535,819,-799
-604,439,-573
-448,-506,-590
-464,510,-614
-718,-356,410
-617,-346,460
407,-904,285
560,737,-895
-631,512,-536
-559,-430,467
-471,-431,-605
377,-770,392
-938,558,446
365,-744,-686
233,-670,-773
596,849,-867
403,-784,337
419,768,465

--- scanner 22 ---
655,436,709
664,319,661
-637,-680,552
-57,-47,25
833,-597,-928
522,476,-608
472,534,-678
529,-860,747
739,-861,726
-689,624,646
641,-884,681
803,-731,-839
-415,728,-535
517,494,-674
-41,81,-154
767,-529,-784
-536,-654,-526
-505,-733,519
-605,729,604
-628,593,497
673,414,797
-649,721,-523
-518,-584,492
-661,-515,-553
-562,714,-654
-578,-694,-524

--- scanner 23 ---
461,-708,682
339,-592,-827
451,-628,-855
-886,521,858
-480,-857,287
736,657,-427
689,477,454
-871,484,670
-435,-634,-640
724,615,-343
369,-653,-759
-99,-152,52
28,-34,-83
636,321,454
-311,-778,277
-443,-514,-723
-819,411,781
670,373,362
420,-769,505
-477,419,-793
451,-799,614
-414,391,-802
-366,302,-759
799,637,-400
-480,-720,343
-460,-570,-821

--- scanner 24 ---
-523,788,-642
-581,566,609
440,-592,639
510,-542,-433
0,2,135
-829,-552,477
-530,794,-713
428,-673,514
636,480,813
276,-518,-451
-493,-663,-425
399,-647,542
434,592,-326
684,286,787
-180,-45,48
-785,-563,571
472,-472,-447
-615,565,511
-555,422,543
286,490,-343
-467,-631,-314
303,695,-338
-499,-535,-340
662,394,768
-689,753,-724
-855,-680,515

--- scanner 25 ---
845,-399,526
-573,-842,-499
675,387,-313
914,-417,-609
-551,654,814
909,-412,599
643,335,-467
-641,-418,495
187,-53,3
643,358,-521
880,-422,396
-623,-832,-600
571,882,631
-436,670,697
779,-473,-685
65,115,109
-696,-404,521
561,863,829
841,-475,-767
-566,-758,-441
-751,453,-439
-476,699,838
-763,379,-341
-748,-284,525
615,907,658
-757,632,-371

--- scanner 26 ---
530,-663,789
517,-718,694
-318,-762,-485
-558,502,664
432,-935,-493
-646,-363,610
630,848,-706
-316,487,-633
-249,325,-650
556,716,-619
-635,-409,719
125,2,85
-227,-736,-409
-587,361,624
530,-676,541
530,704,459
516,678,404
51,-105,-37
396,674,352
590,-925,-620
479,-830,-550
563,803,-602
-542,385,549
-362,-726,-559
-648,-376,838
-237,503,-547

--- scanner 27 ---
-361,804,-629
959,711,-800
-758,404,620
840,-722,446
885,-564,-354
-373,888,-587
870,-554,-242
-445,-460,624
828,602,938
91,86,147
-728,472,681
23,-97,60
-443,-522,819
-646,376,656
801,584,-790
-589,-524,-458
-503,-513,663
-567,-593,-422
769,-674,552
761,498,983
-702,-527,-509
861,-699,-347
-298,831,-609
835,585,851
751,-557,453
912,654,-695

--- scanner 28 ---
-379,-558,668
421,762,670
-554,-463,641
422,-377,480
-835,731,632
736,955,-608
436,-496,627
-393,-537,-425
-832,841,-377
-67,163,-58
-813,975,-296
656,-516,-452
-800,763,-300
-96,29,102
-384,-529,-558
616,-520,-407
-533,-589,572
429,-365,714
-762,769,781
557,655,693
-396,-528,-566
831,819,-599
526,804,754
714,867,-590
674,-450,-457
-778,897,641

--- scanner 29 ---
474,-238,-657
698,-630,853
-473,-517,463
761,870,-677
714,-648,920
-623,720,-750
-744,-476,-371
89,32,154
697,689,-702
553,-228,-826
611,-294,-657
-741,680,-808
-506,898,517
905,-671,872
817,772,-694
660,634,643
-527,-550,-377
-433,892,409
-493,-429,652
-626,672,-719
159,185,37
589,752,635
623,750,521
-491,-489,-339
-336,902,566
-501,-619,587

--- scanner 30 ---
891,-409,-513
-336,-716,-416
780,366,-496
917,-377,-609
-497,549,-747
-544,-520,540
507,-599,775
-501,509,-702
518,-440,816
798,600,-520
-770,502,400
-391,-514,-452
893,393,717
806,-403,-641
589,-433,810
905,524,614
888,376,512
-567,491,-697
-536,-591,658
-247,-582,-386
-658,-554,542
-662,575,394
-613,405,390
784,562,-450
76,25,-32

--- scanner 31 ---
458,-655,504
925,-418,-428
547,692,-375
-762,646,-526
688,806,722
-690,367,770
680,815,589
-824,386,851
410,-553,437
697,695,-329
8,-81,24
673,732,-483
679,803,581
737,-429,-467
-740,466,-471
133,34,-66
-612,399,847
-345,-873,453
-314,-794,372
-752,564,-359
383,-725,408
-294,-813,466
-764,-716,-387
799,-430,-551
-767,-723,-316
-670,-591,-346

--- scanner 32 ---
477,-557,-581
64,-148,-59
-255,-529,361
-741,376,400
-260,-601,309
-34,-25,-150
657,646,-957
552,372,324
835,-622,472
539,-486,-736
-659,438,398
784,-594,617
-254,-568,328
-287,542,-462
583,-637,-666
-516,-618,-533
855,656,-938
422,337,347
716,-644,575
625,308,337
-441,574,-410
684,602,-982
-583,303,342
-495,574,-433
-656,-716,-477
-607,-531,-491

--- scanner 33 ---
-489,313,414
782,-641,460
820,-767,-836
796,543,704
-440,317,584
771,443,-739
-558,463,-511
-566,581,-582
-601,-779,518
-855,-463,-573
-711,-529,-505
779,526,-619
754,-655,-860
87,-27,112
827,495,701
799,-557,-837
-605,-824,558
885,535,819
-535,-774,500
734,-632,343
-332,323,511
-444,465,-545
754,345,-683
870,-623,438
-56,-111,29
-766,-493,-707

--- scanner 34 ---
717,-503,754
567,-695,-573
428,362,877
826,675,-390
-379,-559,876
-509,-495,-689
-533,343,831
25,-25,78
631,-685,-453
749,-655,714
-711,-531,-634
-597,524,856
694,704,-323
-356,-657,831
-671,463,-367
848,714,-420
-643,-465,-778
456,371,814
484,-670,-405
-380,-769,897
753,-588,742
-477,441,848
-634,579,-489
-108,-172,116
339,405,925
-705,461,-481

--- scanner 35 ---
-639,-333,647
749,-750,-732
-668,-338,-579
-681,702,-473
707,-872,-635
600,845,-679
618,611,-655
415,694,601
-128,-95,-57
-660,-347,-446
604,649,-725
556,-768,-676
491,605,691
-734,-508,620
488,-496,449
-701,482,606
355,695,704
-72,58,84
-820,468,459
-747,720,-478
501,-522,629
-677,662,-428
-608,-487,551
-832,475,693
-669,-350,-587
510,-659,534

--- scanner 36 ---
721,-680,490
134,180,57
521,-218,-432
-679,864,598
556,579,-570
-811,-395,-219
-670,-787,682
-713,639,-478
-653,-522,-229
-515,808,622
446,-275,-410
-767,-788,770
-645,772,677
-713,887,-491
740,-762,617
575,487,-674
-726,792,-436
-42,6,0
-764,-452,-324
509,-302,-390
705,-767,603
870,438,859
935,459,861
830,566,882
609,579,-726
-809,-714,758";

			return ParseData(data);
		}

		private static List<Scanner> ParseData(string data)
		{
			var rows = data.Split("\r\n");

			var scanners = new List<Scanner>();
			Scanner scanner = null;

			foreach (string row in rows)
			{
				// Start of a new scanner
				if (row.StartsWith("---"))
				{
					scanner = new Scanner
					{
						Id = int.Parse(row.Substring(12, 2).Trim())
					};
				}
				else if (row == "")
				{
					// Done with the previous scanner. Add it to the list
					scanners.Add(scanner);
				}
				else
				{
					var parts = row.Split(',');
					var beacon = new Beacon
					{
						X = int.Parse(parts[0]),
						Y = int.Parse(parts[1]),
						Z = int.Parse(parts[2])
					};
					scanner.Beacons.Add(beacon);
				}
			}

			// Add the final scanner
			scanners.Add(scanner);

			return scanners;
		}
	}

	public class Scanner
	{
		public int Id { get; set; }
		public List<Beacon> Beacons { get; set; } = new List<Beacon>();

		public override string ToString()
		{
			return $"Scanner {Id}";
		}

		public List<Beacon> GetBeaconsByOrientation(int index)
		{
			return Beacons.Select(b => b.GetOrientations()[index]).ToList();
		}

		// Compare each beacon's position to each other beacon's position and form a hash
		public List<long> GetBeaconDifferenceHashes()
		{
			var hashes = new List<long>();

			for (int i = 0; i < Beacons.Count; i++)
			{
				var beacon = Beacons[i];

				for (int j = i + 1; j < Beacons.Count; j++)
				{
					var otherBeacon = Beacons[j];

					//long hash = (beacon.X - otherBeacon.X) * (beacon.Y - otherBeacon.Y) * (beacon.Z - otherBeacon.Z);
					//long hash = (Math.Abs(beacon.X) + Math.Abs(otherBeacon.X)) * (Math.Abs(beacon.Y) + Math.Abs(otherBeacon.Y)) * (Math.Abs(beacon.Z) + Math.Abs(otherBeacon.Z));
					//hashes.Add(hash);

					// This one works
					//long hash = (Math.Abs(beacon.X) - Math.Abs(otherBeacon.X)) * (Math.Abs(beacon.Y) - Math.Abs(otherBeacon.Y)) * (Math.Abs(beacon.Z) - Math.Abs(otherBeacon.Z));
					//hashes.Add(Math.Abs(hash));

					long xDiff = Math.Max(Math.Abs(beacon.X), Math.Abs(otherBeacon.X)) - Math.Min(Math.Abs(beacon.X), Math.Abs(otherBeacon.X));
					long yDiff = Math.Max(Math.Abs(beacon.Y), Math.Abs(otherBeacon.Y)) - Math.Min(Math.Abs(beacon.Y), Math.Abs(otherBeacon.Y));
					long zDiff = Math.Max(Math.Abs(beacon.Z), Math.Abs(otherBeacon.Z)) - Math.Min(Math.Abs(beacon.Z), Math.Abs(otherBeacon.Z));
					long hash = xDiff * yDiff * zDiff;
					hashes.Add(hash);
				}
			}

			return hashes;
		}

		public List<BeaconDifference> GetBeaconDifferences()
		{
			var differences = new List<BeaconDifference>();

			for (int i = 0; i < Beacons.Count; i++)
			{
				var beacon = Beacons[i];

				for (int j = i + 1; j < Beacons.Count; j++)
				{
					var otherBeacon = Beacons[j];

					//var difference = new BeaconDifference
					//{
					//	XDiff = Math.Max(Math.Abs(beacon.X), Math.Abs(otherBeacon.X)) - Math.Min(Math.Abs(beacon.X), Math.Abs(otherBeacon.X)),
					//	YDiff = Math.Max(Math.Abs(beacon.Y), Math.Abs(otherBeacon.Y)) - Math.Min(Math.Abs(beacon.Y), Math.Abs(otherBeacon.Y)),
					//	ZDiff = Math.Max(Math.Abs(beacon.Z), Math.Abs(otherBeacon.Z)) - Math.Min(Math.Abs(beacon.Z), Math.Abs(otherBeacon.Z))
					//};

					var difference = new BeaconDifference
					{
						XDiff = beacon.X - otherBeacon.X,
						YDiff = beacon.Y - otherBeacon.Y,
						ZDiff = beacon.Z - otherBeacon.Z,
					};

					differences.Add(difference);
				}
			}

			return differences;
		}

		public List<int> GetSquaredDistancesBetweenBeacons()
		{
			var squaredDistances = new List<int>();

			// Consider two beacons seen by this scanner.
			// We can find the differences using their coordinates relative to the scanner.
			// First, find the difference between their x values - call this a.
			// Next, find the difference between their y values: b.
			// And the difference between their z values: d.
			// Use Pythagorean's theorem to get the straight-line distance from Beacon A to Beacon B
			// in one plane (i.e. ignore their z coordinates for now). Call this value c.
			// c^2 = a^2 + b^2
			// Now using c (or c^2), we can find the straight-line distance from Beacon A to Beacon B,
			// now including z coordinates. Call this value e.
			// e^2 = c^2 + d^2

			// Find the squared distance between each pair of beacons seen by this scanner
			for (int i = 0; i < Beacons.Count; i++)
			{
				var beacon = Beacons[i];

				for (int j = i + 1; j < Beacons.Count; j++)
				{
					var otherBeacon = Beacons[j];

					int a = beacon.X - otherBeacon.X;
					int b = beacon.Y - otherBeacon.Y;
					int d = beacon.Z - otherBeacon.Z;

					var cSquared = Math.Pow(a, 2) + Math.Pow(b, 2);
					var eSquared = cSquared + Math.Pow(d, 2);

					squaredDistances.Add((int)eSquared);
				}
			}

			return squaredDistances;
		}
	}

	public class Beacon
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		// This is really likely to be unique...
		public int BeaconHash => (Math.Abs(X) - Math.Abs(Y)) * (Math.Abs(X) - Math.Abs(Z)) * (Math.Abs(Y) - Math.Abs(Z));

		public override string ToString()
		{
			return $"{X},{Y},{Z}";
		}

		private List<Beacon> _orientations;
		//public List<Beacon> GetOrientations()
		//{
		//	if (_orientations == null)
		//	{
		//		// Each scanner is rotated some integer number of 90-degree turns around all of the x, y, and z axes
		//		_orientations = new List<Beacon>();

		//		// Rotate 90 degrees around each axis

		//		// Start with assuming the scanner is facing positive x. Rotate around the x axis.
		//		// At first, positive Z is up. Then positive Y is up. Then negative Z, then negative Y.
		//		_orientations.Add(new Beacon
		//		{
		//			X = X,
		//			Y = Y,
		//			Z = Z
		//		});

		//		_orientations.Add(new Beacon
		//		{
		//			X = X,
		//			Y = -Z,
		//			Z = Y
		//		});

		//		_orientations.Add(new Beacon
		//		{
		//			X = X,
		//			Y = -Y,
		//			Z = -Z
		//		});

		//		_orientations.Add(new Beacon
		//		{
		//			X = X,
		//			Y = Z,
		//			Z = -Y
		//		});

		//		// Next we rotate around the x axis again, but this time assuming we are facing negative x
		//		_orientations.Add(new Beacon
		//		{
		//			X = -X,
		//			Y = -Y,
		//			Z = Z
		//		});

		//		_orientations.Add(new Beacon
		//		{
		//			X = -X,
		//			Y = Z,
		//			Z = Y
		//		});

		//		_orientations.Add(new Beacon
		//		{
		//			X = -X,
		//			Y = Y,
		//			Z = -Z
		//		});

		//		_orientations.Add(new Beacon
		//		{
		//			X = -X,
		//			Y = -Z,
		//			Z = -Y
		//		});

		//		//for (int i = 0; i <= 4; i++)
		//		//{
		//		//	_orientations.Add(new Beacon
		//		//	{
		//		//		X = X,
		//		//		Y = Y,
		//		//		Z = Z
		//		//	});
		//		//}

		//		//_orientations.Add(new Beacon
		//		//{
		//		//	X = X,
		//		//	Y = Y,
		//		//	Z = Z
		//		//});
		//	}

		//	foreach (var orientation in _orientations)
		//	{
		//		Console.WriteLine(orientation);
		//	}
		//	return _orientations;
		//}
		public List<Beacon> GetOrientations()
		{
			if (_orientations == null)
			{
				// Each scanner is rotated some integer number of 90-degree turns around all of the x, y, and z axes
				_orientations = new List<Beacon>();

				// There's certainly a simpler way to do this...
				// X = X
				// Y = -Z
				// Z = Y

				int x = X;
				int y = Y;
				int z = Z;

				// Which axis are we rotating around?
				for (int i = 0; i < 3; i++)
				{
					//int x = X;
					//int y = Y;
					//int z = Z;

					// Facing positive or negative down the axis we're rotating about
					for (int j = 0; j < 2; j++)
					{
						// Which direction is up?
						for (int k = 0; k < 4; k++)
						{
							_orientations.Add(new Beacon
							{
								X = x,
								Y = y,
								Z = z
							});

							// Change which direction is up
							if (i == 0)
							{
								// Facing x or -x
								if (j == 0)
								{
									// Facing positive x
									int temp = y;
									y = -z;
									z = temp;
								}
								else
								{
									// Facing negative x
									int temp = y;
									y = z;
									z = -temp;
								}
							}
							else if (i == 1)
							{
								// Facing y or -y
								if (j == 0)
								{
									// Facing positive y
									int temp = z;
									z = -x;
									x = temp;
								}
								else
								{
									// Facing negative y
									int temp = z;
									z = x;
									x = -temp;
								}
							}
							else
							{
								// Facing z or -z
								if (j == 0)
								{
									// Facing positive z
									int temp = x;
									x = -y;
									y = temp;
								}
								else
								{
									// Facing negative z
									int temp = x;
									x = y;
									y = -temp;
								}
							}
						}

						// Face the other direction
						if (i == 0)
						{
							// Facing x
							x = -x;
							y = -y;
						}
						else if (i == 1)
						{
							// Facing y
							y = -y;
							z = -z;
						}
						else
						{
							// Facing z
							z = -z;
							x = -x;
						}
					}

					// Change which axis we think we're looking down
					if (i == 0)
					{
						x = Y;
						y = X;
						z = Z;
					}
					else if (i == 1)
					{
						x = Y;
						y = Z;
						z = X;
					}
				}
			}

			foreach (var orientation in _orientations)
			{
				Console.WriteLine(orientation);
			}
			return _orientations;
		}
	}

	public class BeaconDifference
	{
		public long XDiff { get; set; }
		public long YDiff { get; set; }
		public long ZDiff { get; set; }

		public override bool Equals(object obj)
		{
			var other = (BeaconDifference)obj;

			//return (Math.Abs(XDiff) == Math.Abs(other.XDiff) && Math.Abs(YDiff) == Math.Abs(other.YDiff) && Math.Abs(ZDiff) == Math.Abs(other.ZDiff))
			//	|| (Math.Abs(XDiff) == Math.Abs(other.XDiff) && Math.Abs(YDiff) == Math.Abs(other.ZDiff) && Math.Abs(ZDiff) == Math.Abs(other.YDiff))
			//	|| (Math.Abs(XDiff) == Math.Abs(other.YDiff) && Math.Abs(YDiff) == Math.Abs(other.XDiff) && Math.Abs(ZDiff) == Math.Abs(other.ZDiff))
			//	|| (Math.Abs(XDiff) == Math.Abs(other.YDiff) && Math.Abs(YDiff) == Math.Abs(other.ZDiff) && Math.Abs(ZDiff) == Math.Abs(other.XDiff))
			//	|| (Math.Abs(XDiff) == Math.Abs(other.ZDiff) && Math.Abs(YDiff) == Math.Abs(other.XDiff) && Math.Abs(ZDiff) == Math.Abs(other.YDiff))
			//	|| (Math.Abs(XDiff) == Math.Abs(other.ZDiff) && Math.Abs(YDiff) == Math.Abs(other.YDiff) && Math.Abs(ZDiff) == Math.Abs(other.XDiff));

			// All three differences must match, but they don't have to be the same axes.
			// If one difference matches negative the other difference, then exactly
			// one other different must match negative the other difference (two wrongs make a right?)

			return (XDiff == other.XDiff && YDiff == other.YDiff && ZDiff == other.ZDiff)
				|| (XDiff == other.XDiff && YDiff == other.ZDiff && ZDiff == other.YDiff)
				|| (XDiff == other.YDiff && YDiff == other.XDiff && ZDiff == other.ZDiff)
				|| (XDiff == other.YDiff && YDiff == other.ZDiff && ZDiff == other.XDiff)
				|| (XDiff == other.ZDiff && YDiff == other.XDiff && ZDiff == other.YDiff)
				|| (XDiff == other.ZDiff && YDiff == other.YDiff && ZDiff == other.XDiff)

				|| (XDiff == -other.XDiff && YDiff == -other.YDiff && ZDiff == other.ZDiff)
				|| (XDiff == -other.XDiff && YDiff == -other.ZDiff && ZDiff == other.YDiff)
				|| (XDiff == -other.YDiff && YDiff == -other.XDiff && ZDiff == other.ZDiff)
				|| (XDiff == -other.YDiff && YDiff == -other.ZDiff && ZDiff == other.XDiff)
				|| (XDiff == -other.ZDiff && YDiff == -other.XDiff && ZDiff == other.YDiff)
				|| (XDiff == -other.ZDiff && YDiff == -other.YDiff && ZDiff == other.XDiff)

				|| (XDiff == -other.XDiff && YDiff == other.YDiff && ZDiff == -other.ZDiff)
				|| (XDiff == -other.XDiff && YDiff == other.ZDiff && ZDiff == -other.YDiff)
				|| (XDiff == -other.YDiff && YDiff == other.XDiff && ZDiff == -other.ZDiff)
				|| (XDiff == -other.YDiff && YDiff == other.ZDiff && ZDiff == -other.XDiff)
				|| (XDiff == -other.ZDiff && YDiff == other.XDiff && ZDiff == -other.YDiff)
				|| (XDiff == -other.ZDiff && YDiff == other.YDiff && ZDiff == -other.XDiff)

				|| (XDiff == other.XDiff && YDiff == -other.YDiff && ZDiff == -other.ZDiff)
				|| (XDiff == other.XDiff && YDiff == -other.ZDiff && ZDiff == -other.YDiff)
				|| (XDiff == other.YDiff && YDiff == -other.XDiff && ZDiff == -other.ZDiff)
				|| (XDiff == other.YDiff && YDiff == -other.ZDiff && ZDiff == -other.XDiff)
				|| (XDiff == other.ZDiff && YDiff == -other.XDiff && ZDiff == -other.YDiff)
				|| (XDiff == other.ZDiff && YDiff == -other.YDiff && ZDiff == -other.XDiff);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(XDiff, YDiff, ZDiff);
		}

		public override string ToString()
		{
			return $"{XDiff},{YDiff},{ZDiff}";
		}
	}
}
