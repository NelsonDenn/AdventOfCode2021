using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day8
	{
		public static int Run()
		{
			var data = GetData();

			//var result = RunPart1(data); // 470
			var result = RunPart2(data); // 989396

			return result;
		}

		private static int RunPart2(List<InputOutput> data)
		{
			return data.Select(d => d.GetOutputValuesSum()).Sum();
		}

		private static int RunPart1(List<InputOutput> data)
		{
			// Count the number of times a 1, 4, 7, or 8 appear in the output values
			// 1 = 2 segments
			// 4 = 4 segments
			// 7 = 3 segments
			// 8 = 7 segments
			return data.Select(d => d.NumOutputValuesWithUniqueSegments).Sum();
		}

		private static List<InputOutput> GetTestData()
		{
			List<InputOutput> inputOutputs = new List<InputOutput>();

			// acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf

			string data =
@"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce";

			var lines = data.Split("\r\n");

			foreach (string line in lines)
			{
				var parts = line.Split(" | ");
				string signalPatterns = parts[0];
				string output = parts[1];

				var inputOutput = new InputOutput
				{
					InputSignalPatterns = signalPatterns.Split(" ").ToList(),
					OutputSegmentPatterns = output.Split(" ").ToList()
				};

				inputOutputs.Add(inputOutput);
			}

			return inputOutputs;
		}

		private static List<InputOutput> GetData()
		{
			List<InputOutput> inputOutputs = new List<InputOutput>();

			string data =
@"aecgdbf badcg fbcage gdabce be gaedb bced dfeag adbfgc abe | gedbacf be bdfcga bedfgca
gbeda bf fbgea dgafce fcgedb fgaec bcfa bfg baefgc dbfgace | fb cfba cbedfg afbc
dfegb bcfdae cagbef cedfg cde cadg fcagde dbcefag dc geacf | dc aedcfb ebadgfc cgdef
fgecab ebfd ed acgbd bgadfe dge dbage daefcg gfeba ecdbafg | dfbe fbaeg abegd caefdgb
edbg gd ebfcg cafed dfgecb gdf agcbfde dgfcab fegdc gfbaec | dbge fbaedcg gd dg
fdbagc egbdf dga ebcfad caedgf eadcf dafeg dagfecb gaec ga | fadebc edgaf ga eagc
ed gefacb dfacge fedb cde ecgdfab cgbfe ecdbg edfbgc dbagc | defb edfb ebgadfc efgcb
afgbec adfecg ebfacgd gbfc fb ecdba aebfc debgaf fba gfcea | gadfbe gefdabc dbeac fb
def degfba egfbd ef cabdge gbdfc adbeg eafbdc geaf edacgfb | bfdgae fde fde geadb
fbdcg edafcb dgabef cfedb eadbf ced gcbedfa beca acfgde ce | fcaged eacb bgeadf dbacef
bgcaf fgabd ebfgc cfa dagfce cgefbad gfcbea ca fdbgce aebc | aebc edgcbaf adfgb gbfad
ebgad ea dbfeg bceagdf dgcba aebc edgabc cafdeg abfgdc aeg | dgcba ae ceadgf gbedf
bcdeg gfedab cf cfg aefc fcbdega ebfag cbgadf gbfec acbfge | fc cgf gcf fc
dg adcebg bcdafe dbgf gda bfade adfgbe afedg adefcgb geafc | cgaef dg caegf gfdecab
dgace afbegd bcgdeaf cdgfea ca cdegb cfga egdaf eacfbd cea | eca cfga gafde ca
cfgeab gdfeabc fcbeg fdegba ae ega gcbea dagbc fcgbed feca | degbfca bdfgeca gcbaef caef
cbgfa dfbcg gdb dg fdgbea edcg cfgbde cfdbe cdafbge bdeafc | dg dgb gfdeab bdg
fb bedgfc cbf fegadcb dfacbe bfceg fbgd gdfce dfgaec geabc | adbfce gafcdeb bcf bf
cegfd edagc fadgebc abfegc cf gdefbc aefbgd gedbf gfc cfdb | dcgbfe cdbf fbdc fc
ebcag adbefc defgca fdga gefdacb egfcdb afcge fg gcf edcfa | cgfdbe adefbcg bcfadeg beacg
cebfa begfca efb fe ecbgad bgaec fgecbd fdcgabe gefa dabfc | ef ecabdg egbfcd cgbea
gdae dgebf acgebf dafcgb dg efbgda gfbeacd fdg cbdef efabg | bfcde dg gfdbace dfg
gfcdb fbecad abdce fcdbe dgfeab efca dfe bcadeg ef degbfca | dgabce fdgbc fgdcbae bcgdf
becf ecdafb aefgd gbdcaf cdebafg cdf acbed cafed fc debagc | abdfgc gcbfad cagdeb fc
fabdc fcegdab be deb defbca fbacdg bfade gfcebd beca gdeaf | ebca be bcagdf gcefbd
dacbge cbdae gfaeb gacfbd baged acdebf bdg ecgd gd acfdebg | gd dgb edfbgac dg
bdfecg fgeac agdeb fadb bgeaf bgdfae bgcedaf fb cbadeg gbf | bfeag cabefgd baged fb
ecga cef becdfg bgcdeaf fcdage fedac cdabfg dfagc dbefa ec | ce cdgfbe fec cfbadg
ecbf agbcf beagc dagcf cafgeb bfg dgfbcae egbacd bf adgfeb | fgb fgb cbegfa befc
bdcaefg fdb bdefa edbca cgaedf gefb adfeg fb fcgbad edbafg | fdb bf ebdaf fbeadcg
cba ecgfa bdgc gfabcd bc abfedcg abgfd fabcg bedafc gbfead | cab bc bcgd dgcb
fbeadgc fdacbe abde dagfce cfbag efcda gdefbc bec be feabc | dabe be ecb cefagdb
af eaf debga bcedf ecgfbda acfegb adcf bfgecd ceafdb badef | dfac af fa bfdeac
cdaeg fedcag edfcg fgacedb gadbcf acd egacb dgfceb daef da | begca ad efacgdb cad
cefdg adbg ecbfag cbeafdg geafb bgedf dbf ebcadf fgdeba bd | gedbf cgdaefb dfb gdcef
cdfeg gecafbd bdfea bgfadc gbfaed fcead cdfbae fca ecab ca | fcbgda bcae ecab acf
abdcef gbacdfe fdabc fedac cfbe fde bdagfc eadcg ef edgbaf | efcb adfec fed bcedfa
fcadg efbgc bdf afcdegb bd cfdagb cdgbf bcda gdebfa deafcg | fbcgd agcfdb cgfad db
efg gbdce cgedaf bdfe acbegd egfcdb gbfec ef fdcgbae fbacg | bcdfeg fcgba egf gebcd
cabedf cgfed fdgeb baeg gadfb eb cgafbed edagbf fbe acbgfd | dbfacge edgfabc eb eb
gfecad gebdf egb bdcfe gb gadb ecfgbda egdfa defgab cfabeg | cegafd gafcbde adbg egb
cefbg ceb ce egbdf cagfb bgfedc gcde bdfgae bfdcae gdbfeca | ce gbcefad edgc ce
bacdg cbge ec fdega cafgbd gbecda cea bcdfae fabegdc cedag | eafdcbg cegadb fgdcab eca
gfcadb dfbe dfg egadf gbfaed agbed df dgeacb agefc dfgcabe | fd dbfe dgf bedf
efabg gbcfe bcaefd gc gec gcdb efdbc gfdcae ecgbfd cedbafg | cgfeb cdbfae fdaegbc ecg
dcfgae dcbgfa gfe fgcad afdeg fdbea ge cgdebaf fgcdbe geac | cdgabf dcgabf edfbcg fdebgc
efdacg dbgef defcbg be abgcde dbe cfdge gabfd dafbceg cfeb | fdgbace fdbga bgaedcf dcfeg
cfedgba agdbc gfdcab fg fgab fagdc dfcea becgfd adcebg fgd | fg eafdc gdbacf cadbgf
dgecbf agedfc af dcbae becgfda dbafe bfgdea fbegd gbaf fea | af ebfdg af abefd
afebgd ecbafdg bge bg gcdfe bcdg adcfge ebgcf acefb bedgcf | fecdag fcaeb beg bg
bfgdec de efgca cedabf agcbdf gedb dec gbdcf afbgdec gecdf | dcgfb cde fegca ed
fegcb ecd egda gcbda fadbec edgcafb adgbcf ed gdbec egabcd | cabedfg fgdebac de egad
efcbg cafeb dcafbe ebag gbf bg dgefc ebdafgc bfeacg dbcafg | bg fgecd bfg efgcd
cfbg aedfcb bg gdb ecbdg dbfec adcge aebgdf agcbdfe edbcgf | gbd cegbdf fegdbc gcedb
fcbgae bdacef aedfg gdcafbe fdebc cbda fdaeb ab eab gbdefc | ab dacb agefd gcbefa
gdcfb dfgceb eb feb cfaebg faced fcedgab cbedf gbde fbgacd | feb baecgfd eb eb
fbdaegc efcb bdgaef fbgcde dfc bfedg cbgfd egacdf abgdc fc | dgcba cgdfb bgacd gcafbed
aegfbc gfdbac abfec efb fceg dfbcage bcdae fe eafdbg bafcg | gfedab fe bcfdga ef
fcdbag ecgfabd dgaecb dgcae ed cebd deg egbfda gefac gbacd | de caedg aegbfdc bdcfaeg
adfbc eba bedcga becda efcagdb egad gcdebf cgafbe ea dbegc | efcgdba dage bcgdafe ae
fbgace gafdc adge cfdegab bfedcg fgeacd gd dgf eacfg fcbda | dgcaf bdafc gacef dg
gdecf dge bdcgf fdbgea ebdgfca gfacde cead gecfab de cegfa | edg ed ceda ed
cbedf cbeafg edfagb adec bfade cbgdf ce fdecba ecbdagf fce | cbfed fec aced eacd
gebac ebdcagf facbdg abcgfe fbc bfgde ceaf cgbeda fc bcfeg | bcf bcf fc bfc
facgeb cgfade abc abcdf ceagdfb edbcf ab cdagf abdg bgcdfa | afgdc ba gebfcad dbag
fbadgc defac ga egafd ebdfgca dfecba aegc adfgec defgb gda | ag gaec gdafcb dga
face cag gdafeb dbagce agecbf gbdcf gbcaf fbeadcg egfab ca | cga ca becfga efac
bcgdfa bdgca cgebad fcgb fbedac dgafb dfb faged gbadfec fb | dfbag gdfba fbcg dbf
decabg be gdafebc edgfb ecbdfg ecagfd gcedf dbafg geb cebf | fgebcd dcgebaf gcbdea bfec
dfaecg gdbea cbd bedcfa gcfbde cbfg dfgaceb gdcfe edgbc bc | bc gcbf edgba bc
bgeafc gedfbac dceaf bged fdgbce cgdfb ebc bfecd fcagbd be | cfgbea bec dcgafb adbfgce
fde fcea dgefca dfcag abged ef eadfg cdfgeb fgbacd dcefbag | gdacfe cfea fdcbega cagfbd
gefdca eb bgdfa adegbc edgcf cfabdeg gfdeb bge befc gedfbc | eb fgdecb dbaegc eb
bgadcf aefgb gecdba dcef fgc aegdc cf cafge agdcfe agcfdbe | cf fdcabge fc cgf
dfegb fg fadbe gcdefba baefgd cegdaf dcabfe gcedb gef agbf | fabg gabf aefgdb fg
eadfc defcb fdabcg dabceg gceadf ea eac eagf gbacdfe dgcfa | cae faecbdg ebgdfac eca
adcbgf badfce cfd fdega beafdcg cf caegbd bcgda gdcaf gbfc | cf cfgadb efcbda fabdegc
gcd ecdfb abfcde fgdab bcgfaed gc afedgc gbdcf egdcbf ebcg | cdfbge bcedfg gceb gc
eadbgf cfgbe bdagf decgfa bdac ca bfagc afbcdg cag acfegbd | ca cag edcgaf abdfgec
gcfbea cbeadgf bdcg cd decbga abdce cabge adbfe adgecf cde | gbfeadc cbgd fcedga cbega
fc decf abecgf cdeafg adcfg acdbge agbfd decag cgfeadb cfa | cfa cf bgadf eacgd
bgafe ae afecbg gcaedb bae cfae ebfcgd cgabfed gbadf egbfc | acef egfab bae befgdc
afcdbg aegbd gdbace baecd gd ecgdfab dgb ecgd aebgf baedfc | efdbcag egcd efdacb gbaef
afcgbe gdbc agfde cbdega dc dgbeacf dce afebcd acegd baceg | aecgd cde cdgb dgbc
fabed egab cfgdea fae bafdgc cfbde ae bgdfa gbacfed gedafb | efbad dafeb eaf fagdcbe
dbca agebd gbdcea ebfcadg fgabe bdgcfe eagcdf da gad bgecd | abdc gfbaced gafedc gbfae
cgadb ecfadb agb bdgf dbfac bg dcgea dcgeabf fabegc bfdcga | dgfb gb cbagd gab
bcadf efad ef gebcafd ebf bgedc bfcgad becfd ebfagc cdbefa | cbafd ebf faed dcfab
aegfb gabedcf fbg bf ecfb dbfgca adcgfe afbecg beagd egfac | becf decfga gfb gcafedb
gedbc adgcb da adg feabdg gdaefcb fcad bcagef gbafc gbadcf | dgcebfa da bdgce fgabc
cdgfeab fdeca bcafde cg dgc abdecg fadcg egcf afdbg ceafdg | dgc gdc ecabdfg fdaceg
gdbefa gfacbd dgacfe abcge eacfg dcfe egf gfcad fe gaefbcd | ef edcf egcdfa efgadcb
gacd fabed caebg cdbafeg cd ceabd gbdcae edc dgbefc abcgfe | dec cgda dc cde
bgae agedcb caged dfcaegb gcb dcbag dfaceg cfdab fecdgb bg | abcgd ageb adgcfbe beag
cdbgf dfbagc cfd cegbd bfdag agbefd gcfa fc fbecda fedcabg | gcfdba cfag aegcdbf gfac
gedca efab gcdfab bdeca ecdbagf ab befdc bad abecdf efgcdb | faecgdb ab egacd acged
eaf agebf abfcg edga debfag ae befgd dabfecg dfebca edfgbc | ea aef agde ecdfabg
gdceab dcafgeb ecfgda ef fdbag fbdge fge edgcfb edbcg bcef | gef cdbgfe bedcfg ebcf
cdgbfa fgbec gbdefc acef ebagd ac acbdegf bca ecgba ecbgfa | bcgafd gafecb ca ca
bdaecg cfbg bgdef cfead feabgd gce gc bcdfgea egfcd efcbgd | dceagb befgdc gc cg
dfaecb gbfde gefad gbae bfegdc cafgd ae fae gcfaedb bdagfe | ea gfdaeb bfadeg bfecdga
efa geadf fa bcgafe cagedf dcaf badge cebgdf fcgde fbdaegc | fea afgedc fa cgfde
cebdga de baedgfc cdagfb ecdfg ebgfdc cdgbf dec efbd efcag | egacf bgcdf ed eacfgbd
edac gaebdf afdeg agedbcf acdfge ec fce egfac cebdfg afgbc | fbegcad ce gfcab cdfage
dfaeb edfgb ebg debcgf fedcg gebacd facgde fbgc cadbgfe gb | bfdge gbcf cdgbef fbgc
gc ecgfbd bagfd fgabc gcda gedbfac aefbc bfcagd fadegb gcb | efabc cgda deagfcb gbfedc
gbdea cgefadb gacd ged aebdf gd cgeab eacgbf fbecdg dbacge | dfgbeac dgca faebgcd cbdeag
fdabc bgecf dgab fbaegcd gadbcf fagced cfdabe gaf ga facbg | fdeabc gfbadc gaf cgdbeaf
cbagde cbdefg ad afegb eagdfc fegad dgfce acgebfd dafc agd | fcad eagdbc gda efgda
egcfa adgbe cdfega cebgadf gebcfd bc begac bafc ecb abgfce | ceb egbdcaf edfbcg bafc
dfg gcad dg dcagfbe baefd fdgeac fegac dfbgec cfebag afged | ceafg agcd gadef ecdgbf
eag ea agedbc gafced bcea abgcdef edagb bacgfd dgbac bfegd | acdbeg fagedc aecb abce
eacgf defgcb dfc fagdebc ecgabf fd dcgefa edaf dcgaf bdcag | cegaf cgdaf eadf bcgedf
ecbda ebdf bf cfb fgbcad abedfc abegdc gfeca fbaec dbacefg | fcb egdbac badcge dceba
eagcdfb cfgea cebd cgfdba cbgae eab acbdg eb bceagd gbfeda | abgdce agefdb edfbag fagdbc
decfb gecbafd gefabd gebda efgacd gf egabcd begfd fdg bfga | fg cegabd efbdcga gf
gfce fed febcd ef gacfbd debac adcfebg fdabge gcedbf gbcfd | ef cgbfad fed eabcd
fda gedbf edgca af ceafdb efgda agedcb acgf bgdceaf acedgf | fda fbdace cgfa debfg
gdebca dgcab acgf cf bfdacg fdbegc bfc efbda bgcdafe dafbc | gcebdf cf fcb gebcdf
ecgfdb dbae fagdce fbeag ea afgdceb bfaedg fea bedfg cfbga | eaf dbgef ea bfaegdc
daegfc dfebc cgebf cbaefd dcf ecgdfba dc gadbef aebdf adbc | gedfba gcdabef abedfc dc
edfbag cdeabf decafbg acfgb ac fbcaeg acf gaec fegba cbfdg | cega dbfagec ebgafd cgae
fgabde ecagf fcdae agfbcd ge abfgc gcbe gea dfegcba ebcagf | cfaegdb gebc acfed ge
dgaebc cgb fgbedac debcaf gc dgfc gcbadf afdcb fegab fbacg | gbecda cbfadge gbc fbcdae
egdbc gbcaedf bcf afbg ebfgc bf bagcfe gecadf cefag dafbec | gbcfe dbaecf decfgab fb
befgd caef fgc fdeagc ecgda dcbfag fc acebdg dfceg cgfebda | fc eagdc cf cafe
cgabf acegdf gbadec deaf dcage cfaeg cbgfade egf ef decfgb | cgefdb decabg adceg cgedab
bdce gdfce cbgafd cbdfge facdbeg fce bfaegc fgbdc ce gfeda | agdef ebgfcd fbdcg dfegc
gca bage ga fcedg fbcedga cagbfe cfbae cfbadg facdeb egacf | agbcdef agc gca ga
efgab ce adec ebagcd abdgc abgec bfacgd gbefdc efdacbg ebc | ec egacbdf agfebdc beagf
be fcdeag gcebad adcbe ebcfagd eba ecbg dcfba adegc edgafb | ebgdaf ebgc dgcfea cebadg
febgad geda dagecfb abdfe fbcga dbgaf efgcbd dg dgf cdfabe | gaed adge gdae gaed
fe gbcdaf cfdgae begdf agdbf bdgec bfegad fabgdce bfae gef | gfadbe beaf agfdec egf
gcbef cegabd cgebdf dbcge abgfe cf cdfb fgc gbcdefa agfdec | gcf fgc bedgac dbcf
fcbed dfgeab becdagf egabf fac gafcdb ca cega becfa fceagb | cdbfag cfa bfagcd cfedb
efabgd bgfea ebacdfg afb cfebgd ebdfg af agceb dafceb fagd | fa fbdage afb dafgeb
agb ba fedbgc geabcd fgead fcab fadgceb afbdg cdbfag cdgbf | abcdge ab ab agedcb
bfega feadbg fgbc cgfeabd cfa abcfe afcebg acdeb cf cfaedg | fgbc fac acf dgafce
egfba fdbeag fda ebdf agbfd fd ecfadg bgcfea bacdg fgeabdc | faebgd gfeba eabgf fadgb
decgbfa dcfbag bgf abdg bg ebgfca gfedc bafcd fabcde gbcfd | gb fgecd dgecf gbf
gcbaef cfgead fb acdbfeg acbf gcfea gebcf ebgcd degabf fgb | gaedfbc bf cgefa cbgfead
gfc abcfged edgbfc egafcb fc dbgefa acfb gbeaf aefgc ecagd | fc bgeaf gcf cfba
ecadfb cegbf cedbf egb edbagfc gbfdea gb feagc bdgc dgfbec | bcfed efcbg gb bg
gbfa fa dcbegf dagce bgdface dcafg bdfcg cfa bdgacf fdecab | fgbcda bedcfg cfa gafebdc
afbd gcdeb agecdf dafbeg ebfga cfgaeb fed dbgfe bafgecd fd | df afdb edf gcbed
fgcedb cbg bcfgead agdb dbaegc gcaeb aedcg bg ecbfa fgdeac | bg acgbe bg gb
dgacf bf fbd afbcd afgb dbafcg ebafdgc fadgec dfegcb cdeba | bdf fgab fbd bfga
daecg dabecgf edfagb fcagd cadbgf dabfec bfgc gbadf dcf fc | bafedcg afgcd cdage dcafg
cgafbd beadgc fagbc afb bf gbdca gafce fbdg fgdceba efdbca | cfgbad fab abf bfa
febgd cfdge fcda gbdacfe fc cagde degfac cbedga fabceg fce | ecf befdg cdefg fc
dcaf agecdb dgfbcea cgeadf faegb gecda dgafe fd bedcgf dfg | daegc bcdeag cfad fdgea
dbgea acdeb ecabdg fadce begafc abc dbgc defagcb cb fabged | agdbcfe cagbef dacgbe deacf
fdbge ae caedfg ade agbfcd ebca egadcfb dcabf fedcba daefb | aebc caeb dea bdefg
edab cda fedacb da gbdecf dcgefa fbced acfgb cbafd bacgefd | afegbcd cfebdga ad fgaecd
afgbd cedga cgabd cb bcdfea fgabdc cfbg gcbdefa fdageb bac | efcdabg bfegda cba abdcfe
gfdcab ceg adgbc aefcd gedca cebfga ge ebgd gfaebcd degcab | edfgbca eg gebd gdbe
eafgcb afcbd egbfa dfge fedabg gd fabgd cgbeda dgb aebcdgf | dbg bafeg cegbdaf fdge
cefdg edbfacg cb fabced dgfba bfcgd cdb dcbagf bfadeg acgb | dfeabg abcg bceafgd cbd
bac bagcf acge cbdgf cafedb gafecb fbgae agbecfd ca agefdb | cage cfdgbea fgebca bafgc
bgeacd cbd dgcfeb db bfeac aefdgc fbcedga cfdeb gbdf efgdc | befca dcfeb gedafbc dbc
dbcaefg fadec abfdge gcde ecf ceagbf efadg ec acfdge cfabd | febdag fdage acdefgb cef
badgfc fad eafbc fbdcge fbdge dgae eadbf abedgfc da fbdgea | edbgf gbfcde baecf dfgbce
ce bgefad fecg bedgac dce badcf degafc deagf baegdfc cedfa | ec egfda dce cfeg
gadcbe afebg edgbfc fcad fgcebad fce cf fedcba faceb beacd | cef acegbd dafc adcegb
cagfde dbgcaf baecfd gdebcfa dce cbfde de bgefc bcdfa abde | ebdfagc abgcdf adbe cde
bcaedgf agecb eadgbc egda gbcfed aebcd cdabf ed fgbeca deb | fadcb edcba gcfedab adbegfc
fcgedba cadegb ecdgbf eabcf ga fabdgc cgbed agb dgae abecg | ebdgc abg ceafb cbfae
badcg gdafb dacbgf aegbf fcegadb gcdf afd fd adcbeg defcba | df dacbg cfgd agbedc
bfdc cb gbfad agefbd aegfc gbc agcdbe adfcbg bedcfga cgbaf | bcg dgbaf cb egafdb
bfea aecdb ebdcaf cfb bf gdcfeba cefdg aedgbc bedcf afcbdg | beagdc fb befa ecdab
cabd egfadc cd abdfgec decfb bdgeaf cfd fbdae gcfbe ebfdac | afbcged cd bdafec dc
cgbdafe fdabce cbfgda gb befgca dcagb bfgd acdbf agecd gba | aebgfcd bg dbgac dfgb
gecdafb fe bfdea gbfda bef fceabg adbec degf agebfd afcdgb | bafedg fged fecgabd bgcafe
becadf decga ca adc cgadfeb gadfeb deagb gcba decgf gabdce | cad acd egdca adc
dbaef dagbef cegafb baf gcfbeda fgdb baged efcda bceadg bf | bgdae fb bfcage gdefab
deagbc bedfg gdafcb dcgafeb cg ecdfab cedbg gcea edabc gcd | ecga gabfcde gcea gc
bdfacg cbdag ecdga cebdag geab ea eda begcadf ebfadc gcfed | dea dea dabcg fbgadc
dcga ceafb facbg fgdacb gc fdgecb adgebfc degbaf cgf dbgaf | fcg aefbc gfc dgac
efacgd gabdfe bagd bdfec begacf afgbe da fad ebafd fbgeadc | eafdb agdbef bagd adbgfe
acdgbe eb cfbe gfebca fedagbc bge gefab gcdaef agfdb cfgea | bfcagde fgabd befc afebg
fgedc cfabged bc ebac acbdeg gcedb eabfgd bgc dgabe badcgf | gcb abec efabdcg beca
ebcfgd cbdgaf dbcga facbegd da dga fcad ebcga bcfdg bgdefa | bcgae gcadb da agd
gdeba gbc gc abgec befdga dagcbe bdcagf cbfae cegd bfgaecd | cg dageb egcba gc
cgaeb acgbed cbade abefgcd fdgcba aged edbfc adb da cagfbe | agde aegfbdc da acebd
deb caefgd eb eacb egdca dgcefba dgabfe gdbce cbaegd fgbcd | ecgad egbcda fgbdc ebd
aegfbd bdeagfc facg cgebf abfecd edgcb cef fc agefb gbcefa | cgadfeb fce gcfa fbdega
degafcb cdgbef efdb cbegd fgadce acfgb cfe begcf cdabeg ef | egfacbd bafcg fe caedgf
cbfeadg fceba bcdegf fb ebf eadcf egcab egfcda fedbac fdba | abecg gaceb fgadebc cadefb
efacb cb cgbe afdegb agebf abc cdebafg fcbage cafed abfdcg | bc bceg aedcf cdgbeaf
dabcefg geafb eadg cfedba dgeabf fdgba adf ad fbdcg eabgcf | fgdcb edag bedfca aegbf
gcdbefa bag dbafg aecfbd ag beafgd cfgbd aegf gabedc aedfb | ga deagbc bagfd afedbg
gefdba ea gcdbea adgef abef efcbgd gae egdbf cdgfa cdagbfe | gea feba abfe afbe
bcfag eafdbcg feagc defbgc bdgeaf gbc gbacfd adcb dgfab cb | bc dfagb ceabgfd bacd
cadgfe gefbd bg cdfge ebfdgc bgdafce gdb cgfb fedab gcdeab | bg dgb bedaf dbgfcae
afcdb dcbg eacdf agbfec gbacf bd fdb dcbgeaf befdga gabfcd | bcgaf cbfaeg cfdab bcfad
cebfdag cgafe cdfbe abfec fegabc fdaecg ab eab aegcdb fbag | egcaf eba gbaedc dbgfcea
cbdge gcbdea bdg cbaed defcg ebag gbcdeaf agbdfc bg ecafbd | ageb aebg gb fecbad
dgc gbcad bdafgce cedagf dfbg abcefd dg abgcfd abceg afcdb | gfdb eagbc efdbac dgc
fgdab egdbcaf dbagfc efbadg ge egcdfb cabfe fbage gead geb | gbe adcfgbe ebg beg
bgfd cbafegd dcabe gfcba dagefc gda ecfabg dg bcadfg bcgad | caebd cdgba acgdef gfecbda";

			var lines = data.Split("\r\n");

			foreach (string line in lines)
			{
				var parts = line.Split(" | ");
				string signalPatterns = parts[0];
				string output = parts[1];

				var inputOutput = new InputOutput
				{
					InputSignalPatterns = signalPatterns.Split(" ").ToList(),
					OutputSegmentPatterns = output.Split(" ").ToList()
				};

				inputOutputs.Add(inputOutput);
			}

			return inputOutputs;
		}
	}

	public class InputOutput
	{
		public List<string> InputSignalPatterns { get; set; }
		public List<string> OutputSegmentPatterns { get; set; }

		// Returns the number of output values whose length are either 2, 3, 4, or 8
		public int NumOutputValuesWithUniqueSegments => OutputSegmentPatterns.Where(v => v.Length == 2 || v.Length == 3 || v.Length == 4 || v.Length == 7).Count();

		public int GetOutputValuesSum()
		{
			// Input signals to output segments
			Dictionary<char, char> inputToOutput = new Dictionary<char, char>();

			// We can get some signal patterns by length
			string one = GetByLength(2);
			string seven = GetByLength(3);
			string four = GetByLength(4);
			string eight = GetByLength(7);

			// We'll need to deduce the rest from here on out

			char signalForA = GetSignalForA(one, seven);
			inputToOutput.Add(signalForA, 'a');

			string three = GetThree(one);

			char signalForD = GetSignalForD(seven, four, three);
			inputToOutput.Add(signalForD, 'd');

			char signalForG = GetSignalForG(seven, three, signalForD);
			inputToOutput.Add(signalForG, 'g');

			char signalForB = GetSignalForB(three, four);
			inputToOutput.Add(signalForB, 'b');

			string six = GetSix(one);

			char signalForC = GetSignalForC(one, six);
			inputToOutput.Add(signalForC, 'c');

			char signalForF = GetSignalForF(one, signalForC);
			inputToOutput.Add(signalForF, 'f');

			char signalForE = GetSignalForE(eight, inputToOutput.Keys.ToList());
			inputToOutput.Add(signalForE, 'e');

			string zero = signalForA.ToString() + signalForB + signalForC + signalForE + signalForF + signalForG;
			string two = signalForA.ToString() + signalForC + signalForD + signalForE + signalForG;
			string five = signalForA.ToString() + signalForB + signalForD + signalForF + signalForG;
			string nine = signalForA.ToString() + signalForB + signalForC + signalForD + signalForF + signalForG;

			// Sort patterns alphabetically and map them to their output numbers
			Dictionary<string, int> patternsToNumbers = new Dictionary<string, int>();
			patternsToNumbers.Add(SortString(zero), 0);
			patternsToNumbers.Add(SortString(one), 1);
			patternsToNumbers.Add(SortString(two), 2);
			patternsToNumbers.Add(SortString(three), 3);
			patternsToNumbers.Add(SortString(four), 4);
			patternsToNumbers.Add(SortString(five), 5);
			patternsToNumbers.Add(SortString(six), 6);
			patternsToNumbers.Add(SortString(seven), 7);
			patternsToNumbers.Add(SortString(eight), 8);
			patternsToNumbers.Add(SortString(nine), 9);

			// The integer values represented by the output value segments
			string outputValues = "";

			// Check each output value against each signal pattern
			// If they contain the same characters, then it's a match
			foreach (string pattern in OutputSegmentPatterns)
			{
				string sortedPattern = SortString(pattern);
				int outputValue = patternsToNumbers[sortedPattern];
				outputValues += outputValue.ToString();
			}

			return int.Parse(outputValues);
		}

		private string GetByLength(int length)
		{
			if (length == 2 || length == 3 || length == 4 || length == 7)
			{
				return InputSignalPatterns.Find(v => v.Length == length);
			}

			throw new Exception("Invalid length supplied");
		}

		// Get the character that appears in seven, but not in one
		private char GetSignalForA(string one, string seven)
		{
			for (int i = 0; i < seven.Length; i++)
			{
				if (one.Contains(seven[i]))
				{
					continue;
				}

				return seven[i];
			}

			throw new Exception("Could not find signal for A");
		}

		private string GetThree(string one)
		{
			// There are three patterns with five signals: 2, 3, 5
			var candidates = InputSignalPatterns.Where(v => v.Length == 5).ToList();

			// 3 is the only pattern (acdfg) that contains both signals from 1 (which is two characters long: cf)
			foreach (string candidate in candidates)
			{
				if (candidate.Contains(one[0]) && candidate.Contains(one[1]))
				{
					return candidate;
				}
			}

			throw new Exception("Could not find pattern for 3");
		}

		private char GetSignalForD(string seven, string four, string three)
		{
			// From seven, we know acf
			// From four, we know bcdf
			// From three, we know acdfg
			// So acdfg - acf = dg
			// Then d is the only common character between dg and bcdf

			string candidates = "";

			for (int i = 0; i < three.Length; i++)
			{
				// Skip acf
				if (seven.Contains(three[i]))
				{
					continue;
				}

				// This is either d or g. Add it to our candidates string
				candidates += three[i];
			}

			// Loop over d and g
			for (int i = 0; i < candidates.Length; i++)
			{
				// four contains bcdf, so if we have a match, then we have d
				if (four.Contains(candidates[i]))
				{
					return candidates[i];
				}
			}

			throw new Exception("Could not find signal for D");
		}

		private char GetSignalForG(string seven, string three, char signalForD)
		{
			// From seven, we know acf
			// From three, we know acdfg
			// So g = acdfg - acf - d

			string candidates = "";

			for (int i = 0; i < three.Length; i++)
			{
				// Skip acf
				if (seven.Contains(three[i]))
				{
					continue;
				}

				// This is either d or g. Add it to our candidates string
				candidates += three[i];
			}

			// Loop over d and g
			for (int i = 0; i < candidates.Length; i++)
			{
				// We already know d, so return the other candidate
				if (candidates[i] == signalForD)
				{
					continue;
				}

				// This is g
				return candidates[i];
			}

			throw new Exception("Could not find signal for G");
		}

		private char GetSignalForB(string three, string four)
		{
			// b is the only character in four that is not also in three
			// b = four - three = bcdf - acdfg
			for (int i = 0; i < four.Length; i++)
			{
				// This is c, d, or f
				if (three.Contains(four[i]))
				{
					continue;
				}

				// This is b
				return four[i];
			}

			throw new Exception("Could not find signal for B");
		}

		private string GetSix(string one)
		{
			// There are three patterns with six signals: 0, 6, 9
			var candidates = InputSignalPatterns.Where(v => v.Length == 6).ToList();

			// 6 is the only pattern (abdefg) that does not contain both signals from 1 (which is two characters long: cf)
			foreach (string candidate in candidates)
			{
				if (candidate.Contains(one[0]) && candidate.Contains(one[1]))
				{
					// This is 0 or 9
					continue;
				}

				// This is 6
				return candidate;
			}

			throw new Exception("Could not find pattern for 6");
		}

		private char GetSignalForC(string one, string six)
		{
			// 1 and 6 share only one signal. 1's other signal must be c
			for (int i = 0; i < one.Length; i++)
			{
				// This is f
				if (six.Contains(one[i]))
				{
					continue;
				}

				// This is c
				return one[i];
			}

			throw new Exception("Could not find signal for C");
		}

		private char GetSignalForF(string one, char signalForC)
		{
			// 1 = cf
			// f = cf - c
			for (int i = 0; i < one.Length; i++)
			{
				if (one[i] == signalForC)
				{
					continue;
				}

				// This is f
				return one[i];
			}

			throw new Exception("Could not find signal for F");
		}

		private char GetSignalForE(string eight, List<char> knownSignals)
		{
			// Get the one signal that is not yet known
			for (int i = 0; i < eight.Length; i++)
			{
				if (knownSignals.Contains(eight[i]))
				{
					continue;
				}

				// This is e
				return eight[i];
			}

			throw new Exception("Could not find signal for E");
		}

		private string SortString(string str)
		{
			char[] characters = str.ToCharArray();
			Array.Sort(characters);
			return new string(characters);
		}
	}
}
