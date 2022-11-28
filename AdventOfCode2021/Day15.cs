﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day15
	{
		private static int width;
		private static int height;

		public static int Run()
		{
			//var data = GetTestData();
			var data = GetData();

			var result = RunPart1(data); // test: 40, 
			//var result = RunPart2(data);

			return result;
		}

		private static int RunPart2(int[,] data)
		{
			return 0;
		}

		private static int RunPart1(int[,] plot)
		{
			width = plot.GetLength(0);
			height = plot.GetLength(1);

			int lowestRiskLevel = GetLowestPathRiskOfInnerPlot(plot, 0, 0, 0);

			// Exclude the starting point
			return lowestRiskLevel - plot[0, 0];
		}

		// Recursive method to find the lowest risk path of the current plot plus its inner plot
		private static int GetLowestPathRiskOfInnerPlot(int[,] plot, int xyPlotStartingPoint, int xEntryPoint, int yEntryPoint)
		{
			if (xyPlotStartingPoint == width - 1)
			{
				return plot[xyPlotStartingPoint, xyPlotStartingPoint];
			}

			int lowestPathRiskLevel = int.MaxValue;
			int currentPathRiskLevel = 0; // The risk level of the path in the current plot before entering the next inner plot

			// Skip this loop for the bottom row
			if (yEntryPoint != height - 1)
			{
				// For each column of the current plot, starting where we entered the plot
				for (int x = xEntryPoint; x < width; x++)
				{
					// Add the current point's risk level to the current path risk level
					//currentPathRiskLevel += plot[x, xyPlotStartingPoint];
					currentPathRiskLevel += plot[x, yEntryPoint];

					// We're looking at the top left corner of the plot. We need to move to the right before moving into the inner plot.
					if (x == xyPlotStartingPoint)
					{
						continue;
					}

					int pathRiskLevel = currentPathRiskLevel + GetLowestPathRiskOfInnerPlot(plot, xyPlotStartingPoint + 1, x, xyPlotStartingPoint + 1);
					lowestPathRiskLevel = Math.Min(lowestPathRiskLevel, pathRiskLevel);
				}
			}

			// Reset the outer path risk level
			currentPathRiskLevel = 0;

			// Skip this loop for the right column
			if (xEntryPoint != width - 1)
			{
				// For each row of the current plot, starting where we entered the plot
				for (int y = yEntryPoint; y < height; y++)
				{
					// Add the current point's risk level to the current path risk level
					//currentPathRiskLevel += plot[xyPlotStartingPoint, y];
					currentPathRiskLevel += plot[xEntryPoint, y];

					// We're looking at the top left corner of the plot. We need to move to the right before moving into the inner plot.
					if (y == xyPlotStartingPoint)
					{
						continue;
					}

					int pathRiskLevel = currentPathRiskLevel + GetLowestPathRiskOfInnerPlot(plot, xyPlotStartingPoint + 1, xyPlotStartingPoint + 1, y);
					lowestPathRiskLevel = Math.Min(lowestPathRiskLevel, pathRiskLevel);
				}
			}

			return lowestPathRiskLevel;
		}

		private static int[,] GetTestData()
		{
			string data =
@"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";

			return ParseData(data);
		}

		private static int[,] GetData()
		{
			string data =
@"3729872819193991183741381921327255521912952378488598118157113984111185733831264519216187279992419418
5839825522941613758499639687561111615124625155472171543113322145452129392716221276818211881511729352
4134729959612251917116193522113292497262149491996954282652192113892225279647399619715952115938324548
8387823489361238911321196479271121241798431843675221612894614723199459142261892562892477475897214512
2718311642158195471366126437626684361191984281192441149813281131372523114141737811963891392161137188
1633716976828186977272984618799494164582512419123433193444192745176719174622119174162711842752234165
8536196119751362771594822831112563792161411228119529921951212333211112374111122385191395119499226196
4424317748893842284116292918291167918211295529958217188311851137241537379742135219141663113153715852
3118475188124822611422973296416158186529284923987111664121452139411642146719148931134139633722641847
6851934149531611188541342575162991912832471661544139732761133218933646738718723992129187399613818363
8153996929154182715146321353713617999211911991712941292333288249481822115792643477119536793973947672
7219715818738959216685957518194222695783428395164128457323484552944171592198421251883239713548617221
1262715395664469179893169118711838439794991291113611518666789621183671761419168175156219311493971496
5241795258244271119913665291444149925518231579149351154234692197423721632373259921734111194199878154
4238919311942942962853734193142392439162389918892922614616712112651496157996572162932176861513321689
9397215672529142519895713347129712324116226286229997859151411981342537141915625372164711292712271421
3395998782141141719726913999455621113392579612663513951212835919421421242919833637317529711498219116
2193481471711852229569989879692416197122721216812227229992815761918531423489811393438911922965159619
4842769293869661728152444171799231294143174132199496274679281251135395246427174194254112411652171336
7992213251961243149212217916322343145475259214827893275457252191114725328937222915223442914328557723
2257187265321614694969351332168129755211199211175225132312414285271189179368149151732736998989183162
8216159952412315973921944882311215542361211713196411468113138592199962919177359812769281129128192484
7491712861972818586611375111891433737952336953282859622126886116519113818731197552121618979191289214
9519299558167367473182241275151831439451481131239621792576394619881973176817252322399699919212924199
2315922512218947577619523732973112884923978163221913621269686911182181363219113486556931341519945112
6114115165916192895839149184894622929498814157381736592893293169135524712244127981174929812544513413
5186913211322157317731247169519515226295142118961713817836883118248313123316421981183166894416113212
4117695592141811484382169917655234176199986738357695382819216661977178922912172994913241815492391665
1571136364983453162181411823855156449192935349821291922512428561912864382151915377191817265141217962
1511291521659137275227657389911232174472896149815797911894194717839991395338952281616815662228492261
5828649125911123147137111832461381136612461674311161311833567156151233413382644122117615261229934173
6151177376158711154321611441522594214112311812241881211943918197935521371919719146218811223421423894
8769116935173916741343771261675921553829389184959278736924912651794116911316749155111581161921181169
1711211239127111911176889918113329893131982442519774356963635189623532958831392412373131996442229141
8633897991511824191382413558492722818991942928281925122119259339589111712291922129859161986267128135
9592118213811241524721679636318994375941713868297714991252937192511552165416131951668421172623281224
3224967269576379116111249675581273884425919773572225149511812863246891912168591295183221461718421422
1554432656234281738838111221231519664799394131692122691811183425679412111316991512929619825198183867
8248994927649116365583392331561911911837163132517663179873111735733184412973866711251251838923138449
5259211459891128167134999531169162279172418131658691816665194211521987621113113198831211278839577946
2785766599294435194392639953157165111218825739651111991224243551822321261168277364891346786626349118
2714176168296153336844183231182651535252463889562247241432572343373223788151338686163198268922373524
1355199994479811117234865127578891964939314321321131179231184988525399914955371213194451337347174271
2116691425521958741819654741947979988533162911541518281862591639174833194413595438171721318151996214
7199413117511419274877394179775819313934612491919181143337191211418784118822152826611134757325297537
9525471113816242245487658916222149127519318152786998981513273463154343973912519194569122512192576721
8168293345397118811396943189777839452129229161163228361721767527622711159864283942348157872324991142
1191859221977482194691487167621999239712994872537873851817413832133757531978721219176129127569918815
2114592297313315112911251614298395213117958733251192212175912913123511396311931271738262121284792536
2869919932971461318762683112553939622994172781357518899238953989332763946514119128918136121797522992
1349359123923859364619598285283386628323357861442156348911212121163812726264112246252161153121465435
3221714127113384432737491292891511495246829324132792729299648883264213925176172541324123571149461999
2123452292359227318591393119291173478879314981248346349367731941746977191783914861198178388492721198
6211151669423929481934553652811251118648971383154888223416145752294918295652137651391623141933251291
1968199132837222725229115976261297331767978518644196281478978288879576342326944551848174958343125397
9331187439143975289119938671749215743238865389127911933921288941828132496128811145715171716399781599
8258695688469693682244411353746194449428433941532714551256339647221249128225651966262619631137432182
1119815925889242989958161716735179151476111719411733228249429593464279127861485547371295945237111819
1918941314483889391331161963259121114221361714162111171971755198331181987585351939382129399318456711
2262369845812951662915911785311143553431911972946357841843955617392141453143811149171162189138215235
9781738821684639758119216888999826613624827494919951411173121215253215182387663649198281989862882821
5223471599176435949148791138949213439183587697771591962269762454282212636422227573816229614333991413
1811857347615983396146881211143218878911161157132571218225549931871192247141659276739241119657253519
9391116719296421398791622137739193128374551475718131458235726213387973681553419632215736219214115921
2577296995887712122121271337118125955881125383969163187854885533588966136231227268367642651291119112
8285247192128184472392664649591512698197881133414392149947313121191253442792749846313714931567561541
5914998569937141423125519223543734716223412411297159869613731926111162615427219296342112996881399614
9731617327324492677486876137627244986422232423294135291891755252141477994628216979396311119149141391
6317494852428673391351138315428114518732111171227125466441941312854564642431811268458791719112129369
5641722963924711738382184153391487755941992731689138293622141914971722786511194329483914836177721721
2191711541319495518458932467456256154389594979661481111229819291398162784169185516522936311913342435
4661927857362831214943291511527118581986618335355199294413511719841391915159192145187217776222362274
1313423119181436167418611172917428394185995441185543711811411839751923935879331959894382314286712886
2862378195197428119549152195162131821795674762115185761752932583827579739227163815129181532289251591
3148183931321398349281623744934235751418141164229128288998984592191421546943449113926281979912992143
6115611238311871526265173386221167312321173417322118912121514233272921373414511252314326187317315994
5811391547139123152186991645788816694273223339633121964738848994167111299136891815992738119115287251
9725141913213349381912262371128529445291223261493393657279646629881882917237532642243639121216841143
3148319878619529254721119699214126285353918292829935411789925221122521243761861198451134393197484183
2435892433518185918762215822812512297918145719969442131992693291494353423274863566659211899632347393
3148114389167985389933767531929983537978116198826159249714966381987574965861944917253114237219219761
2231321238248499915232934551158911163849872299989188311411373921315888123377868546876123317221893431
7912151221119349216814212491382123658643142191841111739865439144111199119989819652867621232625986243
9859438127152434312655293318611182747593379469173271121766249593771491955721156621892496492641213444
4929938238912922229422954293129124132372163439223878142913281213986317472199131824363419139431219311
1968711715383961221294128819426217422912365715915298613143986568719211322631198568127732915271881651
1239331149933523962637128912698656291169897151994262819396381113789171724783976127346481695985869225
5289966393551655752114812379691166211454531182299813127121156612915235229222218314795665421111633258
3127918537891332228227411221581128459148111972112426655116663413129719191831523473352344966511991919
8512892492529735512912187137642211534129682216299596715181914732116586298727395149931114125495922665
2113381417361993148552393738241411426473221311118675228881719381122349492151915996115916113761769471
1193331211133269118281192734513221196629614542378821919122127511913582111219462941913911592896921567
1928839511249353121114797115153112412181184939817233951399211343292439385522184128815982824112251117
2827119576811541383193518515431343378695916957871431921151853841937126894841832535928653198383218118
1323643111222957224134137112939211788117112179862952393114148713251912769398353381195595723195928165
8579198914969811424799274417548281918819941211372431422234999171533811523538112681312838314493612252
9375681881414251511777522314895223498189769112441916199178382117741713498857163298814216928591589821
3232841129939327192792126115197721144739941873884315514136993422182184312983669392631413116981372991
9391291341187851374493262858523912451129322182139437634233926139214699174658694128513176871491776159
1962692319292956392768329433119198234612213218634341969199512619382115112923378297838795174731871477";

			return ParseData(data);
		}

		private static int[,] ParseData(string data)
		{
			var rows = data.Split("\r\n");

			var dataToReturn = new int[rows[0].Length, rows.Length];

			for (int y = 0; y < rows.Length; y++)
			{
				var row = rows[y].ToCharArray();

				for (int x = 0; x < row.Length; x++)
				{
					dataToReturn[x, y] = row[x] - '0'; // Convert to int and add to list
				}
			}

			return dataToReturn;
		}
	}
}
