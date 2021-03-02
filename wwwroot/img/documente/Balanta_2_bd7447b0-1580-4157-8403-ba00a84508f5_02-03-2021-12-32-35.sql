-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Gazdă: 127.0.0.1:3306
-- Timp de generare: ian. 12, 2021 la 09:59 AM
-- Versiune server: 5.7.31
-- Versiune PHP: 7.3.21

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Bază de date: `shop_laravel`
--

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `failed_jobs`
--

DROP TABLE IF EXISTS `failed_jobs`;
CREATE TABLE IF NOT EXISTS `failed_jobs` (
  `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT,
  `connection` text COLLATE utf8_unicode_ci NOT NULL,
  `queue` text COLLATE utf8_unicode_ci NOT NULL,
  `payload` longtext COLLATE utf8_unicode_ci NOT NULL,
  `exception` longtext COLLATE utf8_unicode_ci NOT NULL,
  `failed_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `migrations`
--

DROP TABLE IF EXISTS `migrations`;
CREATE TABLE IF NOT EXISTS `migrations` (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `migration` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `batch` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Eliminarea datelor din tabel `migrations`
--

INSERT INTO `migrations` (`id`, `migration`, `batch`) VALUES
(1, '2014_10_12_000000_create_users_table', 1),
(2, '2019_08_19_000000_create_failed_jobs_table', 1),
(3, '2021_01_05_080826_create_products_table', 1);

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `products`
--

DROP TABLE IF EXISTS `products`;
CREATE TABLE IF NOT EXISTS `products` (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `category` varchar(70) COLLATE utf8_unicode_ci NOT NULL,
  `quantity` int(11) NOT NULL,
  `gramaj` int(11) DEFAULT NULL,
  `description` varchar(2000) COLLATE utf8_unicode_ci NOT NULL,
  `prop_car` varchar(3000) COLLATE utf8_unicode_ci DEFAULT NULL,
  `image` text COLLATE utf8_unicode_ci NOT NULL,
  `price` double(6,2) NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=50 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Eliminarea datelor din tabel `products`
--

INSERT INTO `products` (`id`, `name`, `category`, `quantity`, `gramaj`, `description`, `prop_car`, `image`, `price`, `created_at`, `updated_at`) VALUES
(49, 'Polen uscat 200g', 'Produse_apicole', 33, 200, 'Polenul uscat este considerat un super-aliment datorita continutului ridicat de enzime, antioxidanti, vitamine si minerale, care sunt foarte hranitoare si ajuta organismul sa lupte impotriva afectiunilor.  ', '<p><strong>Beneficiile produsului:</strong> </p><ul><li>Vitaminizant si mineralizant;</li><li>Combate oboseala, ajutand la cresterea nivelului de energie;</li><li>Regleaza tranzitul intestinal, datorita continutului bogat in fibre</li></ul>', 'polen_200g.jpg', 16.00, NULL, NULL),
(42, 'Pastura 500g', 'Produse_apicole', 10, 150, 'Pastura este polenul crud fermentat in stup, timp de 3 luni, in conditii naturale. Un produs cu insusiri valoroase date de continutul mare in zaharuri simple, enzime si aminoacizi, precum si a aciditatii sporite, ce o face usor accesibila. Fata de polen, valoarea nutritive si antibiotica a pasturii este de 4 ori mai mare.', '<p><strong>Beneficiile produsului:</strong></p><ul>\\r\\n<li>Este un tonic general cu actiune mai puternica decat a polenului;</li>\\r\\n<li>Datorita metabolizarii mai rapide si mai complete, amelioreaza starile de slabiciune cauzate de oboseala si stres;</li>\\r\\n<li>Ajuta la intarirea sistemului imunitar;</li></ul>', 'pastura_500g.jpg', 150.00, NULL, NULL),
(43, 'Polen uscat 200g', 'Produse_apicole', 33, 200, 'Polenul uscat este considerat un super-aliment datorita continutului ridicat de enzime, antioxidanti, vitamine si minerale, care sunt foarte hranitoare si ajuta organismul sa lupte impotriva afectiunilor.  ', '<p><strong>Beneficiile produsului:</strong> </p><ul><li>Vitaminizant si mineralizant;</li><li>Combate oboseala, ajutand la cresterea nivelului de energie;</li><li>Regleaza tranzitul intestinal, datorita continutului bogat in fibre</li></ul>', 'polen_200g.jpg', 16.00, NULL, NULL),
(44, 'Polen crud paducel 250g', 'Produse_apicole', 13, 250, '<strong>Polenul crud de Paducel</strong> este un protector al inimii si al circulatiei sangvine. Polenul crud de albine este un aliment VIU si este considerat un panaceu pe care natura il pune la dispozitia noastra, datorita continutului de vitamine (A, beta-carotenoide, E, grupul vitaminelor B), minerale, aminoacizi, bioflavonoide precum si probiotice, lactofermenti si bifidobacterii. ', '<p><strong>Beneficiile produsului: </strong></p><ul><li>Efect antioxidant, contribuind la regenerarea celulelor impotriva stresului oxidativ;</li><li>Sustine sistemul imunitar in perioadele de suprasolicitare psihica si fizica;</li><li>Protejeaza sistemul cardiovascular si mentine valorile normale ale tensiunii arteriale;</li><li>Ajuta la functionarea normala a sistemului nervos.</li></ul>', 'polen_crud_paducel_250g.jpg', 23.00, NULL, NULL),
(45, 'Ser intensiv antiacnee 30 ml', 'Cosmetice', 30, 30, '<p>Serul este un produs concentrat, bogat in ingrediente active naturale cu efect intensiv antiacnee.</p>\\r\\n<p>Serul Apiterra contine un complex de 5 active eficiente in combaterea acneei, care actioneaza in sinergie: reduc productia de sebum si inflamatiile, previn inrosirea pielii, calmeaza tenul iritat si regenereaza celulele epiteliale. Propolisul este cel mai eficient antibiotic natural datorita proprietatilor sale antibacteriene. Extractul de propolis este obtinut printr-o metoda inovativa brevetata. Studiile clinice arata ca reduce leziunile acneice si productia de sebum. Extractul de salcie are un continut ridicat de salicilati naturali care amelioreaza inflamatiile si roseata, au efect antimicrobian si stimuleaza regenerarea celulara. Argintul colloidal este un ingredient natural cu efect rapid, care distruge membrana celulara a fungilor si bacteriilor. Mierea de manuka este recunoscuta pentru proprietatile sale antiinflamatoare si calmante.</p>\\r\\n<p>Extractul de castravete improspateaza pielea,are efect astringent, revitalizant si antiaging.</p><p>Acidul hialuronic pur cu greutate moleculara mica retine apa in straturile profunde ale pielii, asigurand hidratarea in profunzime a epidermei, reducerea vizibila a liniilor fine si recastigarea elasticitatea pielii</p><p>Coenzima Q10 si laptisorul de matca BIO au capacitatea de a diminua efectele radicalilor liberi asupra celulelor si stimuleaza productia compusilor esentiali ai pielii, prevenind astfel imbatranirea si aparitia ridurilor.</p>', '', 'ser_acnee.jpg', 61.00, NULL, NULL),
(46, 'Scrub de fata 75ml', 'Cosmetice', 31, 75, 'Scrubul de fata exfoliaza bland, indepartand impuritatile, excesul de sebum si celulele moarte. Stimuleaza regenerarea celulara, incetinand procesele de imbatranire ale pielii, lasa pielea fina, catifelata si radianta.', '', 'scrub.jpg', 31.00, NULL, NULL),
(47, 'Matca imperecheata natural', 'Matci', 10, 0, '<ul><li>Clipate (taierea aripilor pana la indicele cubital) - la cerere in schimbul sumei de 5 lei pe bucata.</li>\\r\\n<li>marcate 2021, 2022, 2023, 2019, 2020</li>\\r\\n<li>obtinute prin transvazare</li>\\r\\n<li>ne preocupa in principal cresterea trantorilor de care depinde calitatea matcilor.</li>', '', 'matca.jpeg', 86.00, NULL, NULL),
(48, 'Familie de albine', 'Familii', 10, 0, 'Stupii sunt dotati cu : fund(soclu) antivaroua cu plasa din aluminiu, subar din tabla zincata, corp cu suport din aluminiu pentru rame, cat cu 10 rame din plastic crescute si suport pentru rame din aluminiu, podisor, hranitor, gratie hanneman si capac invelit cu tabla zincata. Stupii sunt vopsiti si au o vechime de 3 ani.', '', 'stup_deschis.jpeg', 500.00, NULL, NULL),
(41, 'Laptisor de matca 100g', 'Produse_apicole', 20, 100, '<p><strong>Laptisorul de matca</strong> este unul dintre cele mai valoroase produse apicole deoarece contine substanta activa Acid 10-HDA- prezenta exclusiv in laptisorul de matca pur, care ofera efecte antibacteriene, antifungice si antivirale, contribuind astfel la buna functionare a sistemului imunitar.</p><p>Este singura sursa naturala de Acetilcolina, care faciliteaza transmiterea influxului nervos intre neuroni.</p><p>Contine polifenoli care au efect antioxidant si contribuie la eliminarea radicalilor liberi.</p><p>Este sursa de vitamine din complexul B, indispensabile pentru buna functionare a organismului (B1, B2, B3, B6), in special vitamina B5 (acidul pantotenic), supranumita â€œvitamina antistresâ€.</p><p>Contine toata gama de aminoacizi esentiali (in special prolina si lizina) pe care organismul nu ii poate sintetiza, desi sunt necesari intr-o alimentatie zilnica echilibrata.</p><p>Se recomanda consumul primavara si toamna in cure de 6-8 saptamani, insa se poate utiliza fara restrictii pe tot timpul anului.</p>', '<strong>Beneficiile produsului: </strong>\\r\\n<p><strong>UZ INTERN:</strong></p><ul>\\r\\n<li>Vitalizant, imunomodulator, contribuie la buna functionare a sistemului imunitar;</li>\\r\\n<li>Are efect antioxidant si intarzie procesele de imbatranire;</li>\\r\\n<li>Tonic in perioadele de oboseala fizica si psihica, tulburari de memorie;</li>\\r\\n<li>Tulburari de menstruatie sau menopauza;</li>\\r\\n<li>Ajuta la protejarea ficatului;</li>\\r\\n<li>Mentine valorile normale ale tensiunii arteriale si colesterolului din sange;</li>\\r\\n<li>Remineralizant pentru unghii si par.</li></ul>\\r\\n<p><strong>UZ EXTERN:</strong></p><ul>\\r\\n<li>Este folosit in prepararea cremelor si mastilor de fata.</li>\\r\\n<li>Stimuleaza producerea colagenului natural;</li>\\r\\n<li>Regenereaza tesutul epitelial;</li>\\r\\n<li>Incetineste procesul de imbatranire;</li>\\r\\n<li>Diminueaza ridurile existente si previne aparitia altora noi;</li>\\r\\n<li>Mentine elasticitatea si fermitatea tenului;</li>\\r\\n<li>Hidrateaza tenul;</li>\\r\\n<li>Diminueaza acneea.</li></ul>', 'laptisor_100g.jpg', 113.00, NULL, NULL),
(37, 'Miere de zmeura 420g', 'Produse_apicole', 40, 420, '<strong>Miere de zmeurÄƒ</strong> â€“ este o miere bio parfumatÄƒ, cu aromÄƒ de zmeurÄƒ, obÈ›inutÄƒ de cÄƒtre apicultori prin plasarea stupilor de albine lÃ¢ngÄƒ culturile intense de zmeurÄƒ, Ã®n lunile iunie-iulie, perioada de florenscenÈ›Äƒ a zmeurei.<p>\\r\\n<strong>Mierea de zmeurÄƒ</strong> o miere Ã®n care uneori pot fi gÄƒsite particule de polen, rezultÃ¢nd astfel o miere cu aromÄƒ mai puternicÄƒ.</p>', '', 'zmeura.png', 23.00, NULL, NULL),
(38, 'Combinezon apicol', 'Echipamente', 15, 0, 'Combinezon apicol din bumbac cu masca detasabila. Sistemul de prindere al mastii este prin doua fermuare, iar masca se poate detasa complet pentru spalarea combinezonului in masina de spalat. Are fermuar vertical folosit la imbracarea combinezonului, maneci si picioare prevazute cu elastic pentru a impiedica intrarea albinelor, buzunare la pantalon si piept.Un model de combinezon practic si usor de utilizat.', NULL, 'combinezon.jpg', 150.00, NULL, '2021-01-11 14:46:45'),
(39, 'Crema antiaging ten sensibil', 'Cosmetice', 30, 50, 'Crema este ideala pentru tenurile sensibile, reactive si intolerante, deoarece calmeaza, ofera o senzatie de prospetime si confort, reduce sau previne aparitia rosetei. Are efect antiinflamator, antiiritant, reparator, hidratant, antioxidant si antiaging. Protejeaza pielea sensibila de efectele nocive ale poluarii, care au efect iritant si previne procesele de imbatranire cauzate de stresul oxidativ.', '', 'crema_aa_sensibil_50ml.jpg', 51.00, NULL, NULL),
(40, 'Laptisor de matca 10g', 'Produse_apicole', 44, 10, '<p><strong>Laptisorul de matca</strong> este unul dintre cele mai valoroase produse apicole deoarece contine substanta activa Acid 10-HDA- prezenta exclusiv in laptisorul de matca pur, care ofera efecte antibacteriene, antifungice si antivirale, contribuind astfel la buna functionare a sistemului imunitar.</p><p>Este singura sursa naturala de Acetilcolina, care faciliteaza transmiterea influxului nervos intre neuroni.</p><p>Contine polifenoli care au efect antioxidant si contribuie la eliminarea radicalilor liberi.</p><p>Este sursa de vitamine din complexul B, indispensabile pentru buna functionare a organismului (B1, B2, B3, B6), in special vitamina B5 (acidul pantotenic), supranumita â€œvitamina antistresâ€.</p><p>Contine toata gama de aminoacizi esentiali (in special prolina si lizina) pe care organismul nu ii poate sintetiza, desi sunt necesari intr-o alimentatie zilnica echilibrata.</p><p>Se recomanda consumul primavara si toamna in cure de 6-8 saptamani, insa se poate utiliza fara restrictii pe tot timpul anului.</p>', '<strong>Beneficiile produsului: </strong>\\r\\n<p><strong>UZ INTERN:</strong></p><ul>\\r\\n<li>Vitalizant, imunomodulator, contribuie la buna functionare a sistemului imunitar;</li>\\r\\n<li>Are efect antioxidant si intarzie procesele de imbatranire;</li>\\r\\n<li>Tonic in perioadele de oboseala fizica si psihica, tulburari de memorie;</li>\\r\\n<li>Tulburari de menstruatie sau menopauza;</li>\\r\\n<li>Ajuta la protejarea ficatului;</li>\\r\\n<li>Mentine valorile normale ale tensiunii arteriale si colesterolului din sange;</li>\\r\\n<li>Remineralizant pentru unghii si par.</li></ul>\\r\\n<p><strong>UZ EXTERN:</strong></p><ul>\\r\\n<li>Este folosit in prepararea cremelor si mastilor de fata.</li>\\r\\n<li>Stimuleaza producerea colagenului natural;</li>\\r\\n<li>Regenereaza tesutul epitelial;</li>\\r\\n<li>Incetineste procesul de imbatranire;</li>\\r\\n<li>Diminueaza ridurile existente si previne aparitia altora noi;</li>\\r\\n<li>Mentine elasticitatea si fermitatea tenului;</li>\\r\\n<li>Hidrateaza tenul;</li>\\r\\n<li>Diminueaza acneea.</li></ul>', 'laptisor_10g.jpg', 10.00, NULL, NULL),
(35, 'Miere de rapita 1kg', 'Produse_apicole', 100, 1000, 'Aceasta este o miere monoflora, adica este produsa in cea mai mare parte din nectar si, uneori, din polenul de flori de rapita. Mierea de rapita este colectata in luna mai si este una dintre primele recolte de miere ale anului. Acest tip de miere are cateva caracteristici unice, cum ar fi un timp de cristalizare rapid si un gust delicios, fiind preferata de multi iubitori de miere, deoarece nu este foarte dulce.', 'Mierea de rapita are proprietati de vindecare, fiind recomandata pentru tratarea problemelor de sanatate a rinichilor precum si a bolilor legate de ochi. Uleiul de plante de rapita contine Q3, un element extrem de important pentru dezvoltarea osoasa. Din acest motiv, mierea de rapita este utilizata pentru a trata osteoporoza. De asemenea, ajuta la regenerarea si mentinerea elasticitatii peretilor vasculari. Consumul de miere din rapita protejeaza ficatul, splina si pancreasul de diverse boli. Este recomandat persoanelor care sufera de arsuri la stomac datorita aciditatii foarte scazute. Abundenta vitaminei E in acest produs natural este de o importanta primordiala atunci cand vine vorba de incetinirea imbatranirii pielii, care va arata vitala si sanatoasa ca urmare a consumului de miere de rapita.', 'rapita_ursulet.jpeg', 30.00, NULL, '2021-01-11 14:50:48'),
(36, 'Miere de tei 1kg', 'Produse_apicole', 50, 1000, '<p>Iunie este luna centralÄƒ a Ã®nfloririi unei alte surse nectarifere importante pentru albine, care produc o miere apreciatÄƒ atÃ¢t pentru gustul sÄƒu, cÃ¢t È™i pentru diferitele sale proprietÄƒÈ›i benefice: mierea de tei.</p><p>\\r\\nPlanta rÄƒspÃ¢nditÄƒ Ã®n Ã®ntreaga EuropÄƒ, arborele de tei este folosit pe scarÄƒ largÄƒ Ã®n fitoterapie pentru proprietÄƒÈ›ile sale calmante È™i pentru decongestionarea cÄƒilor respiratorii.</p><p>\\r\\nAre o culoare galben deschis care se Ã®ntunecÄƒ cu timpul. Ca toate tipurile de miere, ea Ã®n mod natural tinde spre cristalizare, deÈ™i pentru mierea de tei este o operaÈ›ie care are loc destul de Ã®ncet. OdatÄƒ cristalizatÄƒ, mierea de tei devine alb-fildeÈ™. La gust are o aromÄƒ proaspÄƒtÄƒ, dulce, aromatÄƒ, care aminteÈ™te de plantele de munte È™i mentÄƒ, pÄƒstrÃ¢nd o anumitÄƒ intensitate È™i persistenÈ›Äƒ.</p>', 'Mierea de tei este beneficÄƒ pentru organism, cu o gamÄƒ largÄƒ de aplicaÅ£ii. Este utilizatÄƒ Ã®n principal ca diaforetic Ã®n tratarea rÄƒceliilor È™i a febrei. Calitatea antibacterianÄƒ a mierii de tei o face idealÄƒ pentru controlul inflamaÈ›iei organelor respiratorii. Este de asemenea folositÄƒ ca un agent fortificant È™i susÈ›ine inima. Se aplicÄƒ extern pentru a ajuta la vindecarea rÄƒnilor pe piele, eczeme È™i arsuri. Zaharurile naturale din miere au efect prebiotic, alimentÃ¢nd bacteriile bune din sistemul digestiv. Mierea de tei este un tonic natural, creÈ™te nivelul de energie È™i Ã®mbunÄƒtÄƒÈ›eÅŸte sistemul imunitar. ConsistenÈ›a vÃ¢scoasÄƒ a mierii Ã®i permite sÄƒ se lipeascÄƒ de cÄƒptuÈ™eala gÃ¢tului, iar aceastÄƒ capacitate, Ã®mpreunÄƒ cu proprietÄƒÈ›ile sale antibacteriene, face ca mierea de tei sÄƒ fie un remediu excelent pentru dureri Ã®n gÃ¢t È™i tuse.', 'tei.png', 31.00, NULL, NULL),
(34, 'Miere de salcam 1kg', 'Produse_apicole', 100, 1000, '<p>Mierea de salcam este o miere care, dupa cum se poate ghici, derivÄƒ din planta de salcam, o planta medicinala apartinand familiei Fabaceae. </p><p> Ca aspect, mierea de salcam are o culoare foarte clara si o consistenta destul de lichida, fara cristale. Mirosul ei este dulce si floral. Gustul sau este dulce si delicat. Cristalizeaza incet, 1-2 ani de la extractie. Capacitatea sa de a ramane intr-o stare lichida pentru o perioada lunga de timp, alaturi de culoarea ei foarte clara, si ofera un aspect comercial deosebit si duce la o dorinta irezistibila de a va scufunda degetele chiar in borcanelul cu miere de salcam.</p>', '<p>In plus fata de utilizarea ca indulcitor in loc de zahar, produsele din miere de salcam au si alte utilizari datorita proprietatilor lor benefice. In primul rand, este un produs natural energizant, furnizand sistemului nervos o zi plina de energie. Mierea de salcam este caracterizata de un continut ridicat de proteine, saruri minerale, vitamine si aminoacizi. Aceasta combinatie naturala face ca acest sortiment de miere sa fie un tonic excelent. Mierea de salcam, in plus, este un remediu care poate fi de asemenea utilizat de catre diabetici, deoarece continutul de zahar deriva in cea mai mare parte din fructoza, care nu are nevoie de insulina pentru a fi metabolizata. </p><p> Are o actiune de detoxifiere care ajuta la mentinerea sanatatii ficatului.</p>', 'honey1.jpg', 40.00, NULL, NULL);

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `email` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `email_verified_at` timestamp NULL DEFAULT NULL,
  `password` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `remember_token` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `users_email_unique` (`email`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Eliminarea datelor din tabel `users`
--

INSERT INTO `users` (`id`, `name`, `email`, `email_verified_at`, `password`, `remember_token`, `created_at`, `updated_at`) VALUES
(1, 'Moisi Olivia-Debora', 'deboramoisi@yahoo.com', NULL, '$2y$10$65ZxxSwZA3JRDUxTpmilOumQVKu.xvlbz29Rasaxl5U1pJsGuby4.', NULL, '2021-01-05 10:24:51', '2021-01-05 10:24:51'),
(2, 'Bogdan-Ioan Groza', 'grozabogdan@gmail.com', NULL, '$2y$10$pC.xZYxe9/wiUx4VEmLkzOEy3f/QAF/FX5OMRODJv5vJBiy0qBKP.', NULL, '2021-01-05 19:38:24', '2021-01-05 19:38:24');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
