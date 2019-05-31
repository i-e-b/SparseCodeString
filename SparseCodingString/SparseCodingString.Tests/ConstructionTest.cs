using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
// ReSharper disable PossibleNullReferenceException

namespace SparseCodingString.Tests
{
    [TestFixture]
    public class ConstructionTest
    {
        [Test]
        public void constructing_from_a_utf8_string() {
            var complexString = "Hello, 世界 ... 𫢹";

            var charCount = complexString.Length;
            var byteCount = Encoding.UTF8.GetBytes(complexString).Length;

            Assert.That(charCount, Is.Not.EqualTo(byteCount), "String is not complex enough");
            Console.WriteLine($"{charCount} -> {byteCount}; Plain UTF32 would be {charCount * sizeof(UInt32)}");

            var subject = SparseString.FromString(complexString);
            Console.WriteLine($"Sparse string size = {subject.ByteSize()}, of which {subject.StringCoeffs.Length} are co-efficients");

            var result = subject.ToString();

            Assert.That(result, Is.EqualTo(complexString));
        }

        [Test]
        public void very_redundant_source_string() {
            var complexString = "ABBBABABABABAAAB";

            var charCount = complexString.Length;
            var byteCount = Encoding.UTF8.GetBytes(complexString).Length;

            Console.WriteLine($"{charCount} -> {byteCount}; Plain UTF32 would be {charCount * sizeof(UInt32)}");

            var subject = SparseString.FromString(complexString);
            Console.WriteLine($"Sparse string size = {subject.ByteSize()}, of which {subject.StringCoeffs.Length} are co-efficients");

            Assert.That(subject.CharDictionary.Length, Is.EqualTo(2));
            Assert.That(subject.StringLength, Is.EqualTo(16));
            Assert.That(subject.StringCoeffs.Length, Is.EqualTo(2), "String representation is inefficient");

            var result = subject.ToString();

            Assert.That(result, Is.EqualTo(complexString));
        }

        [Test]
        public void japanese_language_source_string()
        {
            var complexString = "ソフトマターの秩序形成において、ジャイロイドや、ジャイロイドを入れ子に組み合わせた" +
                                "ダブルジャイロイドがみられることがある。異なる種類の高分子を共有結合でつなげたブロ" +
                                "ック共重合体による秩序構造や界面活性剤と水の系（ライオトロピック系）がつくる構造で" +
                                "はある条件でダブルジャイロイドの周期構造を形成することがわかっている（ただし、界面" +
                                "活性剤では単に立方相や共連続立方相などの別の名前で呼ばれていることが多い）。また、" +
                                "生体内でもミトコンドリアの内膜や滑面小胞体（SER）が場合に応じてジャイロイド構造や" +
                                "ダイアモンド構造などに変化していることが透過型電子顕微鏡の観察から明らかになってい" +
                                "る[3]。 ジャイロイド構造は蝶の鱗粉の微細構造にも見られ、特定の波長の光だけを通さな" +
                                "いことで鱗粉の色を作るのに役立っているとされる[4]。";


            var charCount = complexString.Length;
            var byteCount = Encoding.UTF8.GetBytes(complexString).Length;
            var subject = SparseString.FromString(complexString);

            Console.WriteLine($"Source characters = {charCount} -> {byteCount} UTF8 bytes; Plain UTF32 would be {charCount * sizeof(UInt32)}");
            Console.WriteLine($"Sparse string size = {subject.ByteSize()}, of which {subject.StringCoeffs.Length} are co-efficients");
            Console.WriteLine($"Source string contains {subject.CharDictionary.Length} unique characters.");

            Console.WriteLine("Dictionary: "+string.Join("",subject.CharDictionary.Select(b=>(char)b)));
            Console.WriteLine("Coefficients:\r\n"+string.Join("", subject.StringCoeffs.Select(b=>b.ToString("X2"))));

            var result = subject.ToString();

            Assert.That(result, Is.EqualTo(complexString));
        }

        [Test]
        public void korean_language_source_string()
        {
            var complexString = "어린 시절\r\n드니프로페트로우스크주 크리비리흐에서 유대인 부모의 아들로 태어났다. 그의 아버지인 올렉산드르" +
                                "젤렌스키는 크리비리흐 경제 연구소에서 사이버 네트워크 및 컴퓨터 하드웨어학과 교수로 근무했으며 그의 어머니인" +
                                "림마 젤렌스카는 공학자로 활동했다.\r\n\r\n볼로디미르 젤렌스키는 문법학교에 입학하기 이전에 4년 동안 아버지가" +
                                "일하던 몽골 에르데네트에 거주했다. 젤렌스키는 키예프 국립 경제 대학교 크리비리흐 캠퍼스에서 법학과를 전공했지만" +
                                "법학자로 활동하지 않았다.\r\n\r\n연예인\r\n17세 시절에 크리비리흐에서 텔레비전 유머 경연 프로그램인" +
                                "KVN에 합류했고 1997년에는 우크라이나의 자포리자-크리비리흐 교통 희극단에 입단하여 KVN 메이저 리그에서" +
                                "우승을 차지했다.\r\n\r\n1997년에 희극단 크바르탈 95를 결성하여 지휘했고 1998년부터 2003년까지 KVN의 메이저" +
                                "리그와 우크라이나 리그에서 공연을 전개했다. 팀원들은 러시아 모스크바에서 많은 시간을 보냈으며 구 소련 국가를" +
                                "순회하면서 희극 공연을 전개했다. 2003년에는 우크라이나의 텔레비전 방송국 1+1에서 텔레비전 프로그램 제작을 시작했고" +
                                "2005년에는 동료들과 함께 텔레비전 방송국 인테르로 자리를 옮겼다.\r\n\r\n볼로디미르 젤렌스키는 2010년부터" +
                                "2012년까지 우크라이나의 텔레비전 방송국 인테르의 이사 겸 총 프로듀서로 활동했다. 2015년에는 크바르탈 95에서" +
                                "제작한 텔레비전 드라마 《국민의 일꾼》에서 우크라이나의 대통령 역할을 맡았다. 이 드라마는 30대 고등학교 역사" +
                                "교사가 우크라이나 정부의 부패를 비판하는 인터넷 동영상에 출연한 이후에 대통령 선거에 출마하여 당선된다는 내용을" +
                                "담고 있다.\r\n\r\n정치 활동\r\n젤렌스키는 2013년부터 2014년까지 우크라이나에서 일어난 유로마이단 시위를 지지했다." +
                                "또한 돈바스 전쟁에서는 우크라이나 정부군을 지지했다. 2018년 3월에는 텔레비전 방송 제작사인 크바르탈 95 소속" +
                                "인사들과 함께 이 회사가 제작한 텔레비전 드라마에서 이름을 딴 정당인 국민의 일꾼을 창당했다.\r\n\r\n사생활\r\n" +
                                "볼로디미르 젤렌스키는 2003년 9월에 올레나 젤렌스카(혼전 성은 키야슈코)와 결혼했다. 올레나 젤렌스카는 젤렌스키가" +
                                "다녔던 문법학교 출신이다. 2004년 7월에는 그의 첫째 딸인 올렉산드라가 태어났고 2013년 1월에는 그의 둘째 아들인" +
                                "키릴 젤렌스키가 태어났다.";


            var charCount = complexString.Length;
            var byteCount = Encoding.UTF8.GetBytes(complexString).Length;
            var subject = SparseString.FromString(complexString);

            Console.WriteLine($"Source characters = {charCount} -> {byteCount} UTF8 bytes; Plain UTF32 would be {charCount * sizeof(UInt32)}");
            Console.WriteLine($"Sparse string size = {subject.ByteSize()}, of which {subject.StringCoeffs.Length} are co-efficients");
            Console.WriteLine($"Source string contains {subject.CharDictionary.Length} unique characters.");

            Console.WriteLine("Dictionary: "+string.Join("",subject.CharDictionary.Select(b=>(char)b)));
            Console.WriteLine("Coefficients:\r\n"+string.Join("", subject.StringCoeffs.Select(b=>b.ToString("X2"))));

            var result = subject.ToString();

            Assert.That(result, Is.EqualTo(complexString));
        }
        
        [Test]
        public void english_language_source_string() {
            var complexString = "To be or not to be that is the question "+
                                "Whether tis nobler in the mind to suffer "+
                                "The slings and arrows of outrageous fortune "+
                                "Or to take Arms against a Sea of troubles "+
                                "And by opposing end them to die to sleep "+
                                "No more and by a sleep to say we end "+
                                "the heartache and the thousand natural shocks "+
                                "that Flesh is heir to Tis a consummation "+
                                "devoutly to be wished To die to sleep "+
                                "To sleep perchance to Dream aye theres the rub "+
                                "for in that sleep of death what dreams may come "+
                                "when we have shuffled off this mortal coil "+
                                "must give us pause";

            var charCount = complexString.Length;
            var byteCount = Encoding.UTF8.GetBytes(complexString).Length;
            var subject = SparseString.FromString(complexString);

            Console.WriteLine($"Source characters = {charCount} -> {byteCount} UTF8 bytes; Plain UTF32 would be {charCount * sizeof(UInt32)}");
            Console.WriteLine($"Sparse string size = {subject.ByteSize()}, of which {subject.StringCoeffs.Length} are co-efficients");
            Console.WriteLine($"Source string contains {subject.CharDictionary.Length} unique characters.");

            Console.WriteLine("Dictionary: "+string.Join("",subject.CharDictionary.Select(b=>(char)b)));
            Console.WriteLine("Coefficients:\r\n"+string.Join("", subject.StringCoeffs.Select(b=>b.ToString("X2"))));

            var result = subject.ToString();

            Assert.That(result, Is.EqualTo(complexString));
        }

        [Test]
        public void large_synthetic_english_language_source_string() {
            var complexString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec urna odio, efficitur ac blandit tempor, congue eget dolor. Nulla facilisi. Praesent efficitur lacinia sem, nec imperdiet massa semper quis. Donec quis quam eleifend, egestas ante ornare, pulvinar ipsum. Mauris efficitur, nunc id congue placerat, velit enim iaculis ante, sed finibus orci metus eu tellus. Cras in ultrices lectus. Nulla vel lectus molestie, consectetur orci vel, bibendum est. Curabitur vestibulum neque vel urna egestas scelerisque. Nam suscipit laoreet lobortis. Pellentesque ullamcorper ante ultrices sapien feugiat luctus. Donec eu ultrices urna, vel lacinia libero. Mauris et molestie velit. Maecenas consequat ante mi, placerat scelerisque lectus ultricies a. In tempus molestie tincidunt.\r\n\r\nNulla sem purus, maximus id nulla quis, accumsan lobortis ante. Aliquam faucibus sapien arcu, vel eleifend nibh mollis blandit. Praesent ornare felis in est efficitur condimentum. Maecenas facilisis eget neque ac ornare. Praesent fermentum turpis quis urna volutpat volutpat. In blandit sed felis ac pulvinar. Nullam consectetur nibh mauris, eu porta ex pharetra nec. Aliquam ultrices sem non enim mattis elementum. Pellentesque elementum, mauris vel convallis semper, libero nibh euismod nibh, vitae semper eros est a arcu. Proin elementum, libero gravida finibus posuere, nibh nibh vestibulum mi, et luctus metus leo vitae odio. In ultrices est molestie porta pharetra. Cras volutpat, nibh ut laoreet euismod, mauris erat ullamcorper massa, semper maximus nisi diam sed mauris. Ut pretium mi at mauris accumsan, viverra malesuada erat cursus. Etiam tincidunt nibh in arcu malesuada, nec posuere nibh eleifend. Sed semper facilisis purus, et viverra leo molestie at. In laoreet mi sed gravida rhoncus.\r\n\r\nMauris malesuada nunc eget mauris egestas vestibulum. Maecenas nec leo fringilla, iaculis arcu eu, tempus massa. Nam suscipit est eu turpis venenatis vehicula. Nunc laoreet mauris urna, vel mollis lorem vulputate id. In dolor tortor, consequat ut risus quis, sollicitudin ornare diam. Quisque justo nulla, condimentum ut ligula eget, aliquet consequat leo. Fusce in lobortis velit. Nullam ac maximus enim, id vulputate dui. Mauris hendrerit libero et dolor dictum egestas. Nam congue commodo tellus eu maximus. Mauris id venenatis tortor, eu eleifend mauris. Fusce ac commodo neque, id posuere eros. Nam ligula eros, dignissim vitae augue a, molestie maximus velit. Maecenas dignissim urna at ex dignissim hendrerit. Ut tristique ornare dapibus.\r\n\r\nDonec porttitor orci at ante vulputate vulputate. Suspendisse pharetra arcu mauris, et porttitor metus sollicitudin sit amet. Duis in diam pulvinar magna commodo dignissim. Morbi sodales tempor arcu eu ultricies. Nulla lacinia nunc quis mi fermentum mollis. Nullam consequat arcu eu nisi tristique elementum. Duis condimentum tellus id consequat aliquet. Vivamus facilisis pulvinar fringilla.\r\n\r\nPraesent eu vestibulum turpis. Nam vulputate tellus ac felis convallis eleifend. Suspendisse viverra convallis tellus, id condimentum turpis malesuada ac. Maecenas convallis viverra nisl eu ornare. Duis gravida lorem eu purus efficitur placerat. Etiam volutpat fringilla bibendum. Sed maximus efficitur lacus tristique aliquam. Nam mollis est ac justo pellentesque tincidunt. In sed mauris posuere, varius metus quis, scelerisque mauris. Suspendisse faucibus iaculis iaculis. Cras vestibulum, ex sit amet iaculis sagittis, magna nulla auctor enim, sit amet euismod erat sapien in erat. Sed consequat eget nunc at gravida. Aliquam tortor lorem, sollicitudin a neque vel, tempus porta diam.\r\n\r\nDonec non consectetur diam. Vestibulum nec tincidunt lorem. In sit amet nisl vel augue convallis pulvinar ut nec tortor. Proin malesuada nibh nec turpis posuere, quis pretium massa finibus. Etiam lorem quam, sodales ultricies hendrerit a, euismod at tortor. Etiam rutrum dictum dapibus. Suspendisse facilisis congue mi at aliquet. In dolor dui, consequat et fermentum et, lacinia ac libero. Maecenas eu posuere turpis. Aenean tempor arcu ac suscipit lobortis. Nunc fermentum mauris quis varius imperdiet. Vestibulum nec massa condimentum, fringilla libero ac, dignissim lectus. Cras ac interdum nunc. Sed quis varius mi.\r\n\r\nCurabitur tincidunt imperdiet arcu, egestas maximus massa molestie a. Pellentesque eget porttitor urna. Integer aliquet justo mollis, imperdiet magna nec, finibus ipsum. Donec interdum nec ex sed malesuada. Mauris commodo purus metus, ac scelerisque turpis cursus sit amet. Proin blandit eros quis aliquet facilisis. Praesent luctus lacinia neque nec suscipit. Suspendisse potenti. Ut mollis odio in nunc malesuada auctor. Cras malesuada libero pellentesque euismod convallis. Praesent fermentum ut lacus ut auctor. Curabitur vulputate lacinia sem, vel suscipit est porttitor nec. Interdum et malesuada fames ac ante ipsum primis in faucibus.\r\n\r\nAenean vel tellus in dolor scelerisque rutrum. Suspendisse non facilisis ligula. Vestibulum nisi sem, molestie ac tortor efficitur, commodo commodo metus. Quisque posuere dignissim tortor. Etiam sollicitudin tincidunt magna eu ultricies. Etiam purus magna, elementum eu turpis sit amet, iaculis porta metus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam dapibus at libero sed posuere. In hac habitasse platea dictumst. Phasellus et risus non quam feugiat tristique. Etiam molestie enim eu hendrerit rutrum.\r\n\r\nSed in rhoncus tortor. Praesent et malesuada magna, ut auctor lorem. Nullam consequat libero id congue tempor. Duis nisl leo, lacinia vel ante nec, ullamcorper dictum augue. Donec dignissim justo urna, eu eleifend dui volutpat at. Phasellus porta eu ante id auctor. Cras in massa velit. Aenean et porta risus, accumsan tempus tortor.\r\n\r\nNam aliquam ante felis, eu hendrerit quam semper quis. Aenean dictum sapien id mauris dictum aliquam. Integer mattis nec est ut lobortis. Vestibulum risus metus, ultrices in cursus id, gravida ac magna. Proin id sagittis sapien. Nulla ultricies imperdiet nunc, id fermentum lorem commodo ut. Maecenas ut odio at quam elementum vestibulum quis sit amet ipsum. Curabitur id leo varius, aliquet massa at, viverra tortor.\r\n\r\nDonec vel efficitur velit. Nulla varius, augue id ornare vulputate, felis dolor consectetur quam, ut faucibus nisi ligula eget eros. Duis sed lectus vitae est euismod rhoncus nec sed nibh. Curabitur at aliquet risus, a fermentum odio. Nulla iaculis tellus id neque interdum, sit amet imperdiet tellus blandit. Nulla odio diam, maximus vitae aliquam eu, dictum posuere nisi. Nam suscipit tristique lacus, ac suscipit elit ornare quis. Quisque dapibus fermentum arcu sit amet dictum. Etiam sed urna ut leo tempus mollis. In maximus tempus tellus in porttitor. Donec eleifend venenatis sapien, ut scelerisque odio imperdiet a. Nunc quis nisl ipsum. Morbi ut sollicitudin felis. Mauris a ipsum magna. Quisque ullamcorper bibendum velit, et consequat massa dignissim accumsan. Nulla facilisi.\r\n\r\nPellentesque non arcu in libero rhoncus scelerisque. Sed tristique pellentesque imperdiet. In fringilla sapien eu neque rutrum dictum. Ut id vehicula massa. Mauris lacinia eros quis orci vehicula, in cursus elit mollis. Aliquam ultrices dui sit amet venenatis facilisis. Curabitur vestibulum blandit sapien, et gravida ligula imperdiet a. Integer tellus sem, placerat eget dolor non, eleifend bibendum ligula. In sed velit a purus accumsan suscipit in sodales quam.\r\n\r\nNam cursus efficitur nulla. Vivamus justo orci, blandit sit amet hendrerit et, dictum vitae lorem. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nunc varius vehicula odio, in cursus diam vestibulum et. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Vestibulum eu purus lobortis, congue mauris id, porttitor mi. In metus diam, dictum vel lacus id, mattis hendrerit elit. Nullam gravida velit diam, sed varius elit accumsan pulvinar. Phasellus dui diam, rhoncus ac porta nec, finibus quis lectus. Donec convallis justo quis hendrerit posuere.\r\n\r\nVivamus dictum enim at tristique auctor. Donec dapibus lorem ornare, tincidunt enim quis, volutpat odio. Sed libero arcu, congue et massa sed, bibendum interdum tortor. Aliquam erat volutpat. Etiam lobortis bibendum lacus, at viverra libero rutrum eu. Vestibulum vestibulum pulvinar lectus, non pretium nisi interdum eu. Donec ultricies justo id neque vestibulum gravida. Phasellus a ante ac risus venenatis accumsan. Phasellus magna risus, lobortis ut congue vel, tincidunt eu mauris. Vivamus vitae elit non velit sodales varius. Aliquam erat volutpat. Quisque convallis tortor dolor, vel varius quam ultrices eu.\r\n\r\nCurabitur accumsan libero ex, vel varius dui dapibus quis. Nam ac nisi maximus, vulputate nunc at, mattis odio. Nam finibus mi vel rutrum mattis. Aliquam viverra lectus eget tortor molestie, id egestas purus feugiat. Vestibulum interdum vulputate pellentesque. Nam at erat nec sem maximus pretium non vitae nunc. Aliquam faucibus ante a blandit gravida. Sed venenatis dui ac augue fermentum, nec ultrices elit mollis. Vestibulum porta vel leo sit amet dignissim. Curabitur quis nulla vel erat fringilla varius sit amet in urna. Quisque quis mauris justo. Nam pellentesque consectetur commodo. Quisque sit amet eros fermentum nisl semper rutrum. Vivamus condimentum leo ut condimentum porta. Proin eget libero sed turpis tempus laoreet in tristique urna.\r\n\r\nDonec ante purus, finibus ac ex sed, vehicula sagittis risus. Duis a lacus lorem. Donec finibus quam id ex pretium, ut malesuada sapien tempor. Praesent at tortor et nulla molestie bibendum non eget magna. Praesent imperdiet placerat iaculis. Phasellus dictum tortor nec nunc posuere, non blandit felis sollicitudin. Ut quam libero, scelerisque non diam ac, pharetra iaculis ipsum. Fusce malesuada tellus nulla, eu blandit nulla suscipit sed. Maecenas justo odio, auctor sit amet est id, auctor sodales risus. Nunc et quam finibus, tincidunt ante a, viverra nunc. Quisque bibendum vulputate nulla a gravida. Morbi in metus libero. Integer vehicula eget enim vel gravida. Etiam finibus est ac arcu fringilla viverra. Curabitur euismod nulla magna, et facilisis enim sodales in.\r\n\r\nIn cursus nulla eget feugiat aliquet. Vivamus vel viverra turpis. Donec sagittis lorem at lectus pharetra, vitae volutpat purus aliquam. Curabitur rhoncus pulvinar sapien, vitae tristique lectus bibendum quis. Sed ut nulla sem. Etiam hendrerit massa a nibh sodales aliquet. Proin ac erat at nisl sollicitudin sollicitudin. Etiam lacinia, odio et blandit mollis, nisi arcu laoreet odio, a rutrum ligula neque at massa. Suspendisse condimentum libero in nibh efficitur, vel posuere nisi pulvinar. In viverra mollis rutrum. Maecenas commodo ex eu ex sollicitudin placerat. Nulla nec purus vehicula, aliquam ipsum at, tempus sem. Aenean iaculis diam in pharetra faucibus.\r\n\r\nNunc aliquam tempor iaculis. Nullam feugiat scelerisque diam. In venenatis ante nec mi sagittis tincidunt. Quisque vitae turpis porttitor, finibus dui elementum, lacinia risus. Quisque lobortis aliquam sem ut dapibus. Sed tempus nibh non ante feugiat mollis. Mauris hendrerit facilisis porta. Pellentesque congue tempor maximus. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Curabitur facilisis, est vel venenatis ullamcorper, libero quam luctus mi, quis porttitor nulla arcu quis ante. Duis porttitor rutrum efficitur.\r\n\r\nCras nec varius massa. Donec dolor felis, dapibus sit amet mattis eleifend, cursus id massa. Donec semper nisl mi, eu scelerisque erat aliquet sed. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Vestibulum accumsan diam rhoncus, ullamcorper est non, dignissim ligula. In suscipit a ex eu consectetur. Fusce at hendrerit libero. Suspendisse dignissim a felis id vestibulum. Suspendisse varius tellus sit amet tortor elementum finibus. Vivamus ut nunc in ante lacinia tincidunt at sed mauris. Nunc pellentesque sollicitudin nisl, a lobortis magna facilisis et.\r\n\r\nSuspendisse posuere cursus purus non ultricies. Donec fermentum posuere tellus, vel placerat erat viverra ut. Vestibulum egestas, turpis quis fringilla sollicitudin, elit eros sagittis mi, eget efficitur elit orci ut augue. Integer semper interdum dui, nec accumsan tortor lacinia non. Aliquam efficitur nec lorem quis malesuada. Phasellus ut purus ut velit dapibus tempus. Vestibulum ut commodo magna. In sed sapien at eros interdum fringilla et id eros. Praesent dignissim volutpat egestas. Morbi interdum ipsum arcu, non feugiat dolor venenatis vitae. Nam velit metus, facilisis in diam eget, imperdiet ultrices mi. Phasellus sagittis in nulla et blandit.\r\n\r\nAliquam erat volutpat. Sed interdum ex ante, ac posuere lectus mattis eget. Cras a enim magna. Aliquam dui massa, imperdiet eget nisi sed, bibendum sagittis risus. Curabitur commodo vestibulum imperdiet. Aliquam erat volutpat. Donec varius tristique lacinia. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Duis ac est et diam pulvinar tristique. Nam pulvinar placerat tortor, porta aliquam justo dictum sed. Sed varius, nulla non cursus molestie, justo felis aliquet erat, mattis mattis ex ligula nec erat. Phasellus dolor lectus, condimentum quis luctus sit amet, varius et nisi. Integer nunc erat, pellentesque eget tincidunt sagittis, rutrum a mauris.\r\n\r\nDuis imperdiet libero non laoreet bibendum. Quisque neque magna, ultricies varius suscipit nec, congue sit amet est. Nulla lectus augue, tincidunt sed odio non, facilisis sagittis sem. In quis eleifend magna, efficitur iaculis orci. Duis vestibulum, est id ultrices varius, mi leo interdum eros, at interdum leo ex ut diam. Nullam commodo hendrerit vehicula. Vestibulum bibendum magna eget ante tristique faucibus. Nullam elementum justo ac libero posuere gravida. Phasellus libero leo, vulputate tincidunt lacus vel, feugiat ultrices nulla. Maecenas iaculis convallis lacinia. Nam interdum metus eros, ac pharetra ligula volutpat vitae. Curabitur sit amet ligula sem. Donec mollis faucibus iaculis.\r\n\r\nEtiam bibendum maximus convallis. Vivamus maximus eleifend sem, non auctor quam. Integer ac tortor nisi. Etiam quis condimentum turpis. Aliquam pharetra, lorem sed semper dignissim, turpis diam eleifend ipsum, quis pharetra felis nunc sit amet tellus. Nunc in consectetur dolor, ut pellentesque dui. Curabitur quis ex sit amet libero luctus sodales. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam ac suscipit ante. Phasellus ultrices, risus nec ultricies congue, augue mi porta mauris, eget posuere dolor nulla mollis lectus. Fusce euismod dui eget aliquam sodales.\r\n\r\nQuisque lobortis quam a rutrum placerat. Donec porttitor sapien at turpis fermentum varius. Maecenas pretium at libero ac ullamcorper. Curabitur dignissim velit metus, in tincidunt tortor molestie quis. Phasellus ac tempor dolor. Praesent posuere, erat eget gravida vulputate, urna augue consequat justo, quis pretium magna quam vestibulum erat. Vivamus suscipit fermentum arcu vel tempor. Proin non ex leo. Etiam ligula risus, sollicitudin ut ullamcorper sagittis, maximus sit amet mauris. Maecenas porta sem eget ex bibendum, consequat varius ligula finibus. Maecenas sed vestibulum diam. Praesent a rutrum lectus, accumsan ullamcorper purus. Phasellus varius arcu et sem convallis bibendum. Fusce eu facilisis velit.\r\n\r\nDonec vitae lectus nec velit feugiat pretium. Integer bibendum suscipit leo quis vestibulum. Proin consectetur ex nibh, congue placerat ante ultrices ut. Morbi pharetra purus vel consectetur finibus. Mauris ultricies dapibus tellus eget posuere. In mollis urna ut nibh venenatis commodo nec ut quam. Vivamus sapien leo, volutpat id imperdiet et, venenatis vel magna. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Sed vel malesuada dui, sed mollis ipsum. Fusce sit amet ex a ante interdum venenatis eu in tortor. Proin imperdiet lacus nunc, eu auctor urna convallis et. Suspendisse facilisis mauris eget lacus commodo lacinia. In facilisis enim vel dui hendrerit aliquam.";

            var charCount = complexString.Length;
            var byteCount = Encoding.UTF8.GetBytes(complexString).Length;
            var subject = SparseString.FromString(complexString);

            Console.WriteLine($"Source characters = {charCount} -> {byteCount} UTF8 bytes; Plain UTF32 would be {charCount * sizeof(UInt32)}");
            Console.WriteLine($"Sparse string size = {subject.ByteSize()}, of which {subject.StringCoeffs.Length} are co-efficients");
            Console.WriteLine($"Source string contains {subject.CharDictionary.Length} unique characters.");
            Console.WriteLine("Dictionary: "+string.Join("",subject.CharDictionary.Select(b=>(char)b)));

            var result = subject.ToString();

            Assert.That(result, Is.EqualTo(complexString));
        }
        
        [Test]
        public void long_redundant_source_string() {
            var complexString = "CABDABABABABAAABCDBABABABABAAABABBBABDDDBABAAABACCCABABABDBAACBA";

            var charCount = complexString.Length;
            var byteCount = Encoding.UTF8.GetBytes(complexString).Length;

            Console.WriteLine($"{charCount} -> {byteCount}; Plain UTF32 would be {charCount * sizeof(UInt32)}");

            var subject = SparseString.FromString(complexString);
            Console.WriteLine($"Sparse string size = {subject.ByteSize()}, of which {subject.StringCoeffs.Length} are co-efficients");

            Assert.That(subject.CharDictionary.Length, Is.EqualTo(4));
            Assert.That(subject.StringLength, Is.EqualTo(64));
            Assert.That(subject.StringCoeffs.Length, Is.EqualTo(16), "String representation is inefficient");

            var result = subject.ToString();

            Assert.That(result, Is.EqualTo(complexString));
        }
    }
}
