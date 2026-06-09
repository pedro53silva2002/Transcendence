-- ------------------------------------------------------------
-- Afghanistan (AF)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
	('Andkhoy'),
	('Aqchah'),
    ('Asadabad'),
    ('Aybak'),
    ('Baghlan'),
	('Bagrami'),
	('Bala Koh'),
    ('Balkh'),
    ('Bamyan'),
	('Baraki'),
	('Baraki Barak'),
	('Bazar-e Yakawlang'),
	('Bazarak'),
    ('Charikar'),
	('Chichkah'),
	('Deh-e Shu'),
    ('Faizabad'),
    ('Farah'),
	('Fayroz Koh'),
    ('Gardez'),
	('Gereshk'),
    ('Ghazni'),
	('Ghoryan'),
	('Gudarah'),
	('Haska Meyna'),
    ('Herat'),
	('Hukumati Baghran'),
	('Hukumati Gizab'),
	('Imam Sahib'),
	('Ishkashim'),
	('Islam Qalah'),
    ('Jalalabad'),
    ('Kabul'),
    ('Kandahar'),
	('Karukh'),
	('Khanabad'),
	('Khost'),
	('Khulm'),
	('Kotah-ye Ashro'),
	('Kuhsan'),
    ('Kunduz'),
	('Kushk'),
    ('Lashkar Gah'),
    ('Mahmud-e Raqi'),
    ('Maidan Shar'),
	('Maimana'),
	('Mama Khel'),
    ('Mazar-e-Sharif'),
    ('Mehtar Lam'),
    ('Nili'),
	('Paghman'),
	('Panjab'),
    ('Parun'),
    ('Pul-e Alam'),
    ('Pul-e Khumri'),
    ('Qalah-ye Now'),
	('Qalah-ye Zal'),
    ('Qalat'),
	('Qarqin'),
	('Sangin'),
	('Sar-e Pul'),
	('Sharan'),
    ('Sheghnan'),
	('Shibirghan'),
	('Spin Boldak'),
	('Taluqan'),
	('Tarin Kot'),
	('Taywarah'),
	('Tujg'),
	('Urgun'),
	('Zarah Sharan'),
    ('Zaranj'),
	('Zarghun Shahr')
) AS v(name) ON c.code = 'AF'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Albania (AL)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bajram Curri'),
	('Belsh'),
	('Berat'),
	('Berxull'),
	('Bucimas'),
    ('Burrel'),
    ('Corovode'),
    ('Delvine'),
	('Durres'),
    ('Elbasan'),
	('Erseke'),
    ('Fier'),
	('Fushe-Kruje'),
    ('Gjirokaster'),
    ('Gramsh'),
    ('Kamez'),
    ('Kavaje'),
    ('Korce'),
    ('Kruje'),
    ('Kucove'),
    ('Kukes'),
    ('Lac'),
    ('Lezhe'),
    ('Libonik'),
	('Librazhd'),
    ('Librazhd-Qender'),
	('Lushnje'),
    ('Nikel'),
	('Patos'),
    ('Permet'),
	('Perondi'),
	('Peshkopi')
    ('Pogradec'),
    ('Puke'),
    ('Rreshen'),
    ('Sarande'),
	('Shijak'),
    ('Shkoder'),
	('Shushice'),
	('Sukth'),
    ('Tepelene'),
    ('Tirana'),
    ('Vlore'),
	('Vore'),
	('Xhafzotaj')
) AS v(name) ON c.code = 'AL'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Algeria (DZ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Adrar'),
    ('Ain Beida'),
    ('Ain Defla'),
    ('Ain Oussera'),
    ('Ain Temouchent'),
    ('Algiers'),
    ('Annaba'),
    ('Barika'),
    ('Batna'),
    ('Bejaia'),
    ('Bechar'),
    ('Biskra'),
    ('Blida'),
    ('Bordj Bou Arreridj'),
    ('Boufarik'),
    ('Boumerdes'),
    ('Chlef'),
    ('Constantine'),
    ('Djelfa'),
    ('El Bayadh'),
    ('El Khroub'),
    ('El Meghaier'),
    ('El Oued'),
    ('Ghardaia'),
    ('Guelma'),
    ('Illizi'),
    ('Jijel'),
    ('Khenchela'),
    ('Laghouat'),
    ('Mascara'),
    ('Medea'),
    ('Mila'),
    ('Mostaganem'),
    ('Msila'),
    ('Naama'),
    ('Oran'),
    ('Ouargla'),
    ('Oum el Bouaghi'),
    ('Relizane'),
    ('Saida'),
    ('Setif'),
    ('Sidi Bel Abbes'),
    ('Skikda'),
    ('Souk Ahras'),
    ('Tamanrasset'),
    ('Tebessa'),
    ('Tiaret'),
    ('Tindouf'),
    ('Tipaza'),
    ('Tissemsilt'),
    ('Tizi Ouzou'),
    ('Tlemcen')
) AS v(name) ON c.code = 'DZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Andorra (AD)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Andorra la Vella'),
    ('Canillo'),
    ('Encamp'),
    ('Escaldes-Engordany'),
    ('La Massana'),
    ('Ordino'),
    ('Sant Julia de Loria')
) AS v(name) ON c.code = 'AD'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Angola (AO)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Benguela'),
    ('Cabinda'),
    ('Caluquembe'),
    ('Camacupa'),
    ('Caxito'),
    ('Cubal'),
    ('Cuito'),
    ('Dundo'),
    ('Gabela'),
    ('Huambo'),
    ('Kuito'),
    ('Lobito'),
    ('Luanda'),
    ('Luau'),
    ('Lubango'),
    ('Lucapa'),
    ('Luena'),
    ('Malanje'),
    ('Mbanza Kongo'),
    ('Menongue'),
    ('Ndalatando'),
    ('Ngiva'),
    ('Ondjiva'),
    ('Porto Amboim'),
    ('Saurimo'),
    ('Soio'),
    ('Sumbe'),
    ('Uige'),
    ('Xangongo')
) AS v(name) ON c.code = 'AO'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Antigua and Barbuda (AG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('All Saints'),
    ('Codrington'),
    ('Falmouth'),
    ('Liberta'),
    ('Potters Village'),
    ('Saint John''s'),
    ('Woods')
) AS v(name) ON c.code = 'AG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Argentina (AR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bahia Blanca'),
    ('Buenos Aires'),
    ('Catamarca'),
    ('Comodoro Rivadavia'),
    ('Concordia'),
    ('Cordoba'),
    ('Corrientes'),
    ('Formosa'),
    ('General Roca'),
    ('General San Martin'),
    ('Godoy Cruz'),
    ('Guaymallen'),
    ('Jujuy'),
    ('La Matanza'),
    ('La Plata'),
    ('La Rioja'),
    ('Lanus'),
    ('Lomas de Zamora'),
    ('Mar del Plata'),
    ('Mendoza'),
    ('Mercedes'),
    ('Merlo'),
    ('Moreno'),
    ('Moron'),
    ('Neuquen'),
    ('Parana'),
    ('Posadas'),
    ('Quilmes'),
    ('Rawson'),
    ('Resistencia'),
    ('Rio Gallegos'),
    ('Rosario'),
    ('Salta'),
    ('San Juan'),
    ('San Luis'),
    ('San Miguel de Tucuman'),
    ('San Rafael'),
    ('Santa Fe'),
    ('Santa Rosa'),
    ('Santiago del Estero'),
    ('Tigre'),
    ('Tres de Febrero'),
    ('Ushuaia'),
    ('Viedma'),
    ('Villa Mercedes')
) AS v(name) ON c.code = 'AR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Armenia (AM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Abovyan'),
    ('Artashat'),
    ('Ararat'),
    ('Armavir'),
    ('Avan'),
    ('Charentsavan'),
    ('Dilijan'),
    ('Erebuni'),
    ('Gavar'),
    ('Goris'),
    ('Gyumri'),
    ('Hrazdan'),
    ('Ijevan'),
    ('Kapan'),
    ('Masis'),
    ('Noyemberyan'),
    ('Sevan'),
    ('Sisian'),
    ('Stepanakert'),
    ('Vagharshapat'),
    ('Vanadzor'),
    ('Yerevan')
) AS v(name) ON c.code = 'AM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Australia (AU)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Adelaide'),
    ('Alice Springs'),
    ('Ballarat'),
    ('Bendigo'),
    ('Brisbane'),
    ('Bunbury'),
    ('Bundaberg'),
    ('Cairns'),
    ('Canberra'),
    ('Darwin'),
    ('Geelong'),
    ('Gold Coast'),
    ('Hobart'),
    ('Launceston'),
    ('mackay'),
    ('Mandurah'),
    ('Melbourne'),
    ('Mildura'),
    ('Newcastle'),
    ('Perth'),
    ('Rockhampton'),
    ('Shepparton'),
    ('Sunshine Coast'),
    ('Sydney'),
    ('Toowoomba'),
    ('Townsville'),
    ('Wagga Wagga'),
    ('Wollongong')
) AS v(name) ON c.code = 'AU'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Austria (AT)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Amstetten'),
    ('Baden'),
    ('Bregenz'),
    ('Dornbirn'),
    ('Eisenstadt'),
    ('Feldkirch'),
    ('Graz'),
    ('Innsbruck'),
    ('Kapfenberg'),
    ('Klagenfurt'),
    ('Krems an der Donau'),
    ('Leoben'),
    ('Leonding'),
    ('Linz'),
    ('Salzburg'),
    ('Sankt Polten'),
    ('Steyr'),
    ('Traun'),
    ('Vienna'),
    ('Villach'),
    ('Wels'),
    ('Wiener Neustadt')
) AS v(name) ON c.code = 'AT'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Azerbaijan (AZ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Agdam'),
    ('Agdash'),
    ('Aghjabadi'),
    ('Astara'),
    ('Baku'),
    ('Balakan'),
    ('Barda'),
    ('Beylagan'),
    ('Ganja'),
    ('Goychay'),
    ('Imishli'),
    ('Julfa'),
    ('Khachmaz'),
    ('Kurdamir'),
    ('Lankaran'),
    ('Lerik'),
    ('Masally'),
    ('Mingachevir'),
    ('Nakhchivan'),
    ('Neftchala'),
    ('Oguz'),
    ('Qazakh'),
    ('Quba'),
    ('Saatly'),
    ('Sabirabad'),
    ('Salyan'),
    ('Shamakhi'),
    ('Shaki'),
    ('Shamkir'),
    ('Shirvan'),
    ('Sumqayit'),
    ('Tovuz'),
    ('Ujar'),
    ('Yevlakh'),
    ('Zaqatala')
) AS v(name) ON c.code = 'AZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Bahamas (BS)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Alice Town'),
    ('Andros Town'),
    ('Cooper''s Town'),
    ('Freeport'),
    ('George Town'),
    ('Governor''s Harbour'),
    ('Marsh Harbour'),
    ('Matthew Town'),
    ('Nassau'),
    ('Rock Sound'),
    ('Spanish Wells')
) AS v(name) ON c.code = 'BS'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Bahrain (BH)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('A''ali'),
    ('Al Hadd'),
    ('Al Muharraq'),
    ('Budaiya'),
    ('Hamad Town'),
    ('Isa Town'),
    ('Jidhafs'),
    ('Manama'),
    ('Riffa'),
    ('Sitra'),
    ('Tubli')
) AS v(name) ON c.code = 'BH'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Bangladesh (BD)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Barisal'),
    ('Bogura'),
    ('Brahmanbaria'),
    ('Chandpur'),
    ('Chittagong'),
    ('Comilla'),
    ('Cox''s Bazar'),
    ('Dhaka'),
    ('Dinajpur'),
    ('Faridpur'),
    ('Feni'),
    ('Gazipur'),
    ('Jamalpur'),
    ('Jessore'),
    ('Jhenaidah'),
    ('Khulna'),
    ('Kishoreganj'),
    ('Kushtia'),
    ('Manikganj'),
    ('Mymensingh'),
    ('Narayanganj'),
    ('Narsingdi'),
    ('Nawabganj'),
    ('Noakhali'),
    ('Pabna'),
    ('Rajshahi'),
    ('Rangpur'),
    ('Savar'),
    ('Sirajganj'),
    ('Sylhet'),
    ('Tangail')
) AS v(name) ON c.code = 'BD'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Barbados (BB)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bathsheba'),
    ('Bridgetown'),
    ('Holetown'),
    ('Oistins'),
    ('Speightstown')
) AS v(name) ON c.code = 'BB'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Belarus (BY)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Baranovichi'),
    ('Bobruisk'),
    ('Brest'),
    ('Dzerzhinsk'),
    ('Gomel'),
    ('Grodno'),
    ('Kobrin'),
    ('Lida'),
    ('Minsk'),
    ('Mogilev'),
    ('Molodechno'),
    ('Mozyr'),
    ('Novopolotsk'),
    ('Orsha'),
    ('Pinsk'),
    ('Polotsk'),
    ('Slutsk'),
    ('Soligorsk'),
    ('Vitebsk'),
    ('Zhodino')
) AS v(name) ON c.code = 'BY'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Belgium (BE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aalst'),
    ('Anderlecht'),
    ('Antwerp'),
    ('Arlon'),
    ('Brugge'),
    ('Brussels'),
    ('Charleroi'),
    ('Gent'),
    ('Genk'),
    ('Hasselt'),
    ('Kortrijk'),
    ('Leuven'),
    ('Liege'),
    ('Mechelen'),
    ('Mons'),
    ('Mouscron'),
    ('Namur'),
    ('Ostend'),
    ('Roeselare'),
    ('Saint-Nicolas'),
    ('Seraing'),
    ('Sint-Truiden'),
    ('Tournai')
) AS v(name) ON c.code = 'BE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Belize (BZ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Belmopan'),
    ('Belize City'),
    ('Dangriga'),
    ('Orange Walk'),
    ('Punta Gorda'),
    ('San Ignacio'),
    ('San Pedro')
) AS v(name) ON c.code = 'BZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Benin (BJ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Abomey'),
    ('Abomey-Calavi'),
    ('Allada'),
    ('Bohicon'),
    ('Cotonou'),
    ('Djougou'),
    ('Kandi'),
    ('Lokossa'),
    ('Natitingou'),
    ('Ouidah'),
    ('Parakou'),
    ('Porto-Novo'),
    ('Savalou'),
    ('Save'),
    ('Tchaourou')
) AS v(name) ON c.code = 'BJ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Bhutan (BT)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bumthang'),
    ('Gelephu'),
    ('Jakar'),
    ('Mongar'),
    ('Paro'),
    ('Phuentsholing'),
    ('Punakha'),
    ('Samdrup Jongkhar'),
    ('Thimphu'),
    ('Trashigang'),
    ('Trongsa'),
    ('Wangdue Phodrang')
) AS v(name) ON c.code = 'BT'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Bolivia (BO)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Camiri'),
    ('Cobija'),
    ('Cochabamba'),
    ('Colcapirhua'),
    ('El Alto'),
    ('Guayaramerin'),
    ('La Paz'),
    ('Llallagua'),
    ('Montero'),
    ('Oruro'),
    ('Potosi'),
    ('Quillacollo'),
    ('Riberalta'),
    ('Sacaba'),
    ('Santa Cruz de la Sierra'),
    ('Sucre'),
    ('Tarija'),
    ('Trinidad'),
    ('Tupiza'),
    ('Villazón'),
    ('Warnes'),
    ('Yacuiba')
) AS v(name) ON c.code = 'BO'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Bosnia and Herzegovina (BA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Banja Luka'),
    ('Bijeljina'),
    ('Bihac'),
    ('Bosanska Krupa'),
    ('Brčko'),
    ('Doboj'),
    ('Gorazde'),
    ('Gradacac'),
    ('Mostar'),
    ('Prijedor'),
    ('Sarajevo'),
    ('Trebinje'),
    ('Tuzla'),
    ('Zenica'),
    ('Zvornik')
) AS v(name) ON c.code = 'BA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Botswana (BW)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Francistown'),
    ('Gaborone'),
    ('Janeng'),
    ('Kanye'),
    ('Kasane'),
    ('Letlhakane'),
    ('Lobatse'),
    ('Maun'),
    ('Mochudi'),
    ('Mogoditshane'),
    ('Molepolole'),
    ('Palapye'),
    ('Ramotswa'),
    ('Selibe Phikwe'),
    ('Serowe'),
    ('Tlokweng')
) AS v(name) ON c.code = 'BW'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Brazil (BR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Alagoinhas'),
    ('Anapolis'),
    ('Aparecida de Goiania'),
    ('Aracaju'),
    ('Belem'),
    ('Belo Horizonte'),
    ('Betim'),
    ('Blumenau'),
    ('Boa Vista'),
    ('Caruaru'),
    ('Carapicuiba'),
    ('Campo Grande'),
    ('Campinas'),
    ('Campos dos Goytacazes'),
    ('Caucaia'),
    ('Contagem'),
    ('Cuiaba'),
    ('Curitiba'),
    ('Diadema'),
    ('Duque de Caxias'),
    ('Feira de Santana'),
    ('Florianopolis'),
    ('Fortaleza'),
    ('Goiania'),
    ('Guarulhos'),
    ('Imperatriz'),
    ('Joao Pessoa'),
    ('Joinville'),
    ('Juiz de Fora'),
    ('Londrina'),
    ('Macapa'),
    ('Maceio'),
    ('Manaus'),
    ('Maringa'),
    ('Maua'),
    ('Mogi das Cruzes'),
    ('Natal'),
    ('Niteroi'),
    ('Nova Iguacu'),
    ('Olinda'),
    ('Osasco'),
    ('Palmas'),
    ('Porto Alegre'),
    ('Porto Velho'),
    ('Recife'),
    ('Ribeirao Preto'),
    ('Rio Branco'),
    ('Rio de Janeiro'),
    ('Salvador'),
    ('Santo Andre'),
    ('Santos'),
    ('Sao Goncalo'),
    ('Sao Jose dos Campos'),
    ('Sao Luis'),
    ('Sao Paulo'),
    ('Serra'),
    ('Sorocaba'),
    ('Teresina'),
    ('Uberlandia'),
    ('Vila Velha'),
    ('Vitoria')
) AS v(name) ON c.code = 'BR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Brunei Darussalam (BN)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bandar Seri Begawan'),
    ('Kuala Belait'),
    ('Seria'),
    ('Tutong')
) AS v(name) ON c.code = 'BN'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Bulgaria (BG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Asenovgrad'),
    ('Blagoevgrad'),
    ('Burgas'),
    ('Dobrich'),
    ('Dupnitsa'),
    ('Gabrovo'),
    ('Haskovo'),
    ('Kardzhali'),
    ('Kazanlak'),
    ('Kyustendil'),
    ('Lovech'),
    ('Montana'),
    ('Pazardzhik'),
    ('Pernik'),
    ('Pleven'),
    ('Plovdiv'),
    ('Razgrad'),
    ('Ruse'),
    ('Shumen'),
    ('Silistra'),
    ('Sliven'),
    ('Smolyan'),
    ('Sofia'),
    ('Stara Zagora'),
    ('Targovishte'),
    ('Varna'),
    ('Vidin'),
    ('Vratsa'),
    ('Yambol')
) AS v(name) ON c.code = 'BG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Burkina Faso (BF)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Banfora'),
    ('Bogande'),
    ('Bobo-Dioulasso'),
    ('Dedougou'),
    ('Diapaga'),
    ('Diebougou'),
    ('Fada N''gourma'),
    ('Gaoua'),
    ('Gayeri'),
    ('Gourcy'),
    ('Kaya'),
    ('Kombissiri'),
    ('Kongoussi'),
    ('Koudougou'),
    ('Manga'),
    ('Nouna'),
    ('Ouagadougou'),
    ('Ouahigouya'),
    ('Po'),
    ('Reo'),
    ('Tenkodogo'),
    ('Titao'),
    ('Tougan'),
    ('Ziniere'),
    ('Zorgho')
) AS v(name) ON c.code = 'BF'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Burundi (BI)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bubanza'),
    ('Bujumbura'),
    ('Bururi'),
    ('Gitega'),
    ('Karuzi'),
    ('Kayanza'),
    ('Kirundo'),
    ('Makamba'),
    ('Muramvya'),
    ('Muyinga'),
    ('Mwaro'),
    ('Ngozi'),
    ('Rutana'),
    ('Rumonge'),
    ('Ruyigi')
) AS v(name) ON c.code = 'BI'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Cabo Verde (CV)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Assomada'),
    ('Mindelo'),
    ('Pedra Badejo'),
    ('Porto Novo'),
    ('Praia'),
    ('Ribeira Grande'),
    ('Santa Maria'),
    ('Sao Filipe'),
    ('Tarrafal')
) AS v(name) ON c.code = 'CV'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Cambodia (KH)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Anlong Veng'),
    ('Banlung'),
    ('Battambang'),
    ('Kampong Cham'),
    ('Kampong Chhnang'),
    ('Kampong Speu'),
    ('Kampong Thom'),
    ('Kampot'),
    ('Kep'),
    ('Koh Kong'),
    ('Kratié'),
    ('Pailin'),
    ('Phnom Penh'),
    ('Preah Sihanouk'),
    ('Prey Veng'),
    ('Pursat'),
    ('Siem Reap'),
    ('Senmonorom'),
    ('Sisophon'),
    ('Stung Treng'),
    ('Svay Rieng'),
    ('Takeo'),
    ('Tbong Khmum')
) AS v(name) ON c.code = 'KH'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Cameroon (CM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bafoussam'),
    ('Bamenda'),
    ('Bertoua'),
    ('Buea'),
    ('Douala'),
    ('Edea'),
    ('Ebolowa'),
    ('Garoua'),
    ('Garoua-Boulai'),
    ('Kumba'),
    ('Limbe'),
    ('Loum'),
    ('Maroua'),
    ('Mbalmayo'),
    ('Mbouda'),
    ('Mora'),
    ('Nkongsamba'),
    ('Ngaoundere'),
    ('Sangmelima'),
    ('Tibati'),
    ('Yagoua'),
    ('Yaounde')
) AS v(name) ON c.code = 'CM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Canada (CA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Abbotsford'),
    ('Barrie'),
    ('Brampton'),
    ('Brantford'),
    ('Burlington'),
    ('Burnaby'),
    ('Calgary'),
    ('Cambridge'),
    ('Charlottetown'),
    ('Chilliwack'),
    ('Edmonton'),
    ('Fredericton'),
    ('Greater Sudbury'),
    ('Guelph'),
    ('Halifax'),
    ('Hamilton'),
    ('Iqaluit'),
    ('Kelowna'),
    ('Kingston'),
    ('Kitchener'),
    ('Langley'),
    ('Laval'),
    ('London'),
    ('Longueuil'),
    ('Markham'),
    ('Mississauga'),
    ('Moncton'),
    ('Montreal'),
    ('Nanaimo'),
    ('Oakville'),
    ('Ottawa'),
    ('Quebec City'),
    ('Red Deer'),
    ('Regina'),
    ('Richmond'),
    ('Richmond Hill'),
    ('Saskatoon'),
    ('Saint John'),
    ('Saguenay'),
    ('Sherbrooke'),
    ('Surrey'),
    ('Thunder Bay'),
    ('Toronto'),
    ('Trois-Rivieres'),
    ('Vancouver'),
    ('Vaughan'),
    ('Victoria'),
    ('Whitehorse'),
    ('Windsor'),
    ('Winnipeg'),
    ('Yellowknife')
) AS v(name) ON c.code = 'CA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Central African Republic (CF)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bambari'),
    ('Bangassou'),
    ('Bangui'),
    ('Berberati'),
    ('Bimbo'),
    ('Bossangoa'),
    ('Bouar'),
    ('Bria'),
    ('Carnot'),
    ('Kaga-Bandoro'),
    ('Mbaiki'),
    ('Mobaye'),
    ('Nola'),
    ('Sibut'),
    ('Yaloke')
) AS v(name) ON c.code = 'CF'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Chad (TD)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Abeche'),
    ('Ati'),
    ('Bongor'),
    ('Doba'),
    ('Faya-Largeau'),
    ('Goz Beida'),
    ('Kelo'),
    ('Koumra'),
    ('Lai'),
    ('Mao'),
    ('Moundou'),
    ('N''Djamena'),
    ('Pala'),
    ('Sarh'),
    ('Mongo')
) AS v(name) ON c.code = 'TD'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Chile (CL)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Antofagasta'),
    ('Arica'),
    ('Calama'),
    ('Chillan'),
    ('Concepcion'),
    ('Copiapo'),
    ('Coquimbo'),
    ('Curico'),
    ('Iquique'),
    ('La Serena'),
    ('Los Angeles'),
    ('Osorno'),
    ('Puerto Montt'),
    ('Punta Arenas'),
    ('Quilpue'),
    ('Rancagua'),
    ('San Antonio'),
    ('San Bernardo'),
    ('Santiago'),
    ('Talca'),
    ('Talcahuano'),
    ('Temuco'),
    ('Valdivia'),
    ('Valparaiso'),
    ('Vina del Mar')
) AS v(name) ON c.code = 'CL'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- China (CN)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Baoding'),
    ('Baotou'),
    ('Beijing'),
    ('Changchun'),
    ('Changsha'),
    ('Changzhou'),
    ('Chengdu'),
    ('Chongqing'),
    ('Dalian'),
    ('Dongguan'),
    ('Foshan'),
    ('Fuzhou'),
    ('Guangzhou'),
    ('Guiyang'),
    ('Haikou'),
    ('Hangzhou'),
    ('Harbin'),
    ('Hefei'),
    ('Hohhot'),
    ('Jinan'),
    ('Kunming'),
    ('Lanzhou'),
    ('Lhasa'),
    ('Luoyang'),
    ('Nanchang'),
    ('Nanjing'),
    ('Nanning'),
    ('Ningbo'),
    ('Qingdao'),
    ('Shenyang'),
    ('Shenzhen'),
    ('Shijiazhuang'),
    ('Suzhou'),
    ('Taiyuan'),
    ('Tangshan'),
    ('Tianjin'),
    ('Urumqi'),
    ('Wenzhou'),
    ('Wuhan'),
    ('Wuxi'),
    ('Xiamen'),
    ('Xi''an'),
    ('Xining'),
    ('Yinchuan'),
    ('Zhengzhou'),
    ('Zhongshan'),
    ('Zibo')
) AS v(name) ON c.code = 'CN'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Colombia (CO)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Armenia'),
    ('Barranquilla'),
    ('Bogota'),
    ('Bucaramanga'),
    ('Buenaventura'),
    ('Bello'),
    ('Cartagena'),
    ('Cucuta'),
    ('Envigado'),
    ('Floridablanca'),
    ('Ibague'),
    ('Itagui'),
    ('Manizales'),
    ('Medellin'),
    ('Monteria'),
    ('Neiva'),
    ('Pasto'),
    ('Pereira'),
    ('Popayan'),
    ('Riohacha'),
    ('Santa Marta'),
    ('Sincelejo'),
    ('Soacha'),
    ('Soledad'),
    ('Tunja'),
    ('Valledupar'),
    ('Villavicencio')
) AS v(name) ON c.code = 'CO'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Comoros (KM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Domoni'),
    ('Fomboni'),
    ('Moroni'),
    ('Moutsamoudou'),
    ('Tsimbeo')
) AS v(name) ON c.code = 'KM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Congo (CG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Brazzaville'),
    ('Dolisie'),
    ('Gamboma'),
    ('Impfondo'),
    ('Kayes'),
    ('Kinkala'),
    ('Loandjili'),
    ('Madingou'),
    ('Makoua'),
    ('Mindouli'),
    ('Nkayi'),
    ('Ouesso'),
    ('Owando'),
    ('Pointe-Noire'),
    ('Sibiti')
) AS v(name) ON c.code = 'CG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Costa Rica (CR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Alajuela'),
    ('Cartago'),
    ('Desamparados'),
    ('Grecia'),
    ('Heredia'),
    ('Liberia'),
    ('Limon'),
    ('Paraiso'),
    ('Puntarenas'),
    ('Quesada'),
    ('San Jose'),
    ('San Rafael'),
    ('Santa Ana'),
    ('Turrialba')
) AS v(name) ON c.code = 'CR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Croatia (HR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bjelovar'),
    ('Dubrovnik'),
    ('Karlovac'),
    ('Koprivnica'),
    ('Osijek'),
    ('Pozega'),
    ('Pula'),
    ('Rijeka'),
    ('Sibenik'),
    ('Sisak'),
    ('Slavonski Brod'),
    ('Split'),
    ('Varazdin'),
    ('Virovitica'),
    ('Vukovar'),
    ('Zadar'),
    ('Zagreb')
) AS v(name) ON c.code = 'HR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Cuba (CU)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bayamo'),
    ('Camaguey'),
    ('Ciego de Avila'),
    ('Cienfuegos'),
    ('Guantanamo'),
    ('Holguin'),
    ('Las Tunas'),
    ('Matanzas'),
    ('Moa'),
    ('Nueva Gerona'),
    ('Pinar del Rio'),
    ('Sancti Spiritus'),
    ('Santa Clara'),
    ('Santiago de Cuba'),
    ('Trinidad')
) AS v(name) ON c.code = 'CU'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Cyprus (CY)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Famagusta'),
    ('Kyrenia'),
    ('Larnaca'),
    ('Limassol'),
    ('Nicosia'),
    ('Paphos'),
    ('Paralimni'),
    ('Strovolos')
) AS v(name) ON c.code = 'CY'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Czechia (CZ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Brno'),
    ('Ceske Budejovice'),
    ('Chomutov'),
    ('Decin'),
    ('Frydek-Mistek'),
    ('Havirov'),
    ('Hradec Kralove'),
    ('Jihlava'),
    ('Karlovy Vary'),
    ('Karvina'),
    ('Kladno'),
    ('Liberec'),
    ('Most'),
    ('Mlada Boleslav'),
    ('Olomouc'),
    ('Opava'),
    ('Ostrava'),
    ('Pardubice'),
    ('Plzen'),
    ('Prague'),
    ('Prerov'),
    ('Prostejov'),
    ('Teplice'),
    ('Usti nad Labem'),
    ('Zlin')
) AS v(name) ON c.code = 'CZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Côte d'Ivoire (CI)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Abengourou'),
    ('Abidjan'),
    ('Aboisso'),
    ('Adzope'),
    ('Agboville'),
    ('Bondoukou'),
    ('Bouake'),
    ('Bouna'),
    ('Boundiali'),
    ('Dabou'),
    ('Daloa'),
    ('Dimbokro'),
    ('Divo'),
    ('Ferkessedougou'),
    ('Gagnoa'),
    ('Grand-Bassam'),
    ('Guiglo'),
    ('Issia'),
    ('Katiola'),
    ('Korhogo'),
    ('Man'),
    ('Odienne'),
    ('San-Pedro'),
    ('Sassandra'),
    ('Seguela'),
    ('Tabou'),
    ('Toumodi'),
    ('Yamoussoukro')
) AS v(name) ON c.code = 'CI'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Democratic Republic of the Congo (CD)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bandundu'),
    ('Beni'),
    ('Bunia'),
    ('Bukavu'),
    ('Butembo'),
    ('Goma'),
    ('Kabinda'),
    ('Kalemie'),
    ('Kamina'),
    ('Kananga'),
    ('Kikwit'),
    ('Kindu'),
    ('Kinshasa'),
    ('Kisangani'),
    ('Kolwezi'),
    ('Likasi'),
    ('Lubumbashi'),
    ('Matadi'),
    ('Mbandaka'),
    ('Mbuji-Mayi'),
    ('Mwene-Ditu'),
    ('Tshikapa'),
    ('Uvira'),
    ('Uvira'),
    ('Zongo')
) AS v(name) ON c.code = 'CD'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Denmark (DK)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aalborg'),
    ('Aarhus'),
    ('Copenhagen'),
    ('Esbjerg'),
    ('Frederiksberg'),
    ('Fredericia'),
    ('Gentofte'),
    ('Gladsaxe'),
    ('Helsingborg'),
    ('Helsingør'),
    ('Herning'),
    ('Hillerød'),
    ('Holstebro'),
    ('Horsens'),
    ('Kolding'),
    ('Lyngby'),
    ('Naestved'),
    ('Odense'),
    ('Randers'),
    ('Roskilde'),
    ('Silkeborg'),
    ('Slagelse'),
    ('Sonderborg'),
    ('Vejle'),
    ('Viborg')
) AS v(name) ON c.code = 'DK'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Djibouti (DJ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ali Sabieh'),
    ('Arta'),
    ('Dikhil'),
    ('Djibouti'),
    ('Obock'),
    ('Tadjourah')
) AS v(name) ON c.code = 'DJ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Dominica (DM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Berekua'),
    ('Marigot'),
    ('Portsmouth'),
    ('Roseau'),
    ('Saint Joseph')
) AS v(name) ON c.code = 'DM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Dominican Republic (DO)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Azua'),
    ('Barahona'),
    ('Boca Chica'),
    ('Bonao'),
    ('Cotui'),
    ('Higuey'),
    ('La Romana'),
    ('La Vega'),
    ('Mao'),
    ('Moca'),
    ('Monte Cristi'),
    ('Monte Plata'),
    ('Puerto Plata'),
    ('San Cristobal'),
    ('San Francisco de Macoris'),
    ('San Juan'),
    ('San Pedro de Macoris'),
    ('Santiago de los Caballeros'),
    ('Santo Domingo'),
    ('Valverde')
) AS v(name) ON c.code = 'DO'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Ecuador (EC)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ambato'),
    ('Azogues'),
    ('Babahoyo'),
    ('Cuenca'),
    ('Duran'),
    ('Esmeraldas'),
    ('Guayaquil'),
    ('Ibarra'),
    ('Loja'),
    ('Lago Agrio'),
    ('Latacunga'),
    ('Machala'),
    ('Manta'),
    ('Milagro'),
    ('Portoviejo'),
    ('Quevedo'),
    ('Quito'),
    ('Riobamba'),
    ('Salinas'),
    ('Santa Elena'),
    ('Santo Domingo'),
    ('Tulcan')
) AS v(name) ON c.code = 'EC'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Egypt (EG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Alexandria'),
    ('Arish'),
    ('Assiut'),
    ('Aswan'),
    ('Banha'),
    ('Beni Suef'),
    ('Cairo'),
    ('Damietta'),
    ('Faiyum'),
    ('Giza'),
    ('Helwan'),
    ('Hurghada'),
    ('Ismailia'),
    ('Kafr el-Sheikh'),
    ('Luxor'),
    ('Mansoura'),
    ('Minya'),
    ('New Cairo'),
    ('Port Said'),
    ('Qena'),
    ('Sharm el-Sheikh'),
    ('Sohag'),
    ('Suez'),
    ('Tanta'),
    ('Zagazig')
) AS v(name) ON c.code = 'EG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- El Salvador (SV)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ahuachapan'),
    ('Apopa'),
    ('Chalatenango'),
    ('Cojutepeque'),
    ('Delgado'),
    ('Ilopango'),
    ('La Union'),
    ('Mejicanos'),
    ('Metapan'),
    ('Nueva San Salvador'),
    ('San Miguel'),
    ('San Salvador'),
    ('San Vicente'),
    ('Sonsonate'),
    ('Soyapango'),
    ('Usulutan'),
    ('Zacatecoluca')
) AS v(name) ON c.code = 'SV'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Equatorial Guinea (GQ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aconibe'),
    ('Añisoc'),
    ('Bata'),
    ('Ebebiyin'),
    ('Evinayong'),
    ('Luba'),
    ('Malabo'),
    ('Mbini'),
    ('Mongomo'),
    ('Rebola')
) AS v(name) ON c.code = 'GQ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Eritrea (ER)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Agordat'),
    ('Akordat'),
    ('Asmara'),
    ('Assab'),
    ('Barentu'),
    ('Dekemhare'),
    ('Enda Selassie'),
    ('Keren'),
    ('Massawa'),
    ('Mendefera'),
    ('Nakfa'),
    ('Tesseney')
) AS v(name) ON c.code = 'ER'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Estonia (EE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Haapsalu'),
    ('Johvi'),
    ('Keila'),
    ('Kohtla-Jarve'),
    ('Kuressaare'),
    ('Narva'),
    ('Narva-Joesuu'),
    ('Paide'),
    ('Parnu'),
    ('Rakvere'),
    ('Sillamae'),
    ('Tallinn'),
    ('Tartu'),
    ('Valga'),
    ('Viljandi'),
    ('Voru')
) AS v(name) ON c.code = 'EE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Eswatini (SZ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Big Bend'),
    ('Hluti'),
    ('Lobamba'),
    ('Manzini'),
    ('Mbabane'),
    ('Mhlume'),
    ('Nhlangano'),
    ('Piggs Peak'),
    ('Siteki'),
    ('Tshaneni')
) AS v(name) ON c.code = 'SZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Ethiopia (ET)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Addis Ababa'),
    ('Adama'),
    ('Arba Minch'),
    ('Asella'),
    ('Assosa'),
    ('Axum'),
    ('Bahir Dar'),
    ('Debre Berhan'),
    ('Debre Markos'),
    ('Debre Tabor'),
    ('Dessie'),
    ('Dire Dawa'),
    ('Gambela'),
    ('Gondar'),
    ('Harar'),
    ('Hawassa'),
    ('Jijiga'),
    ('Jimma'),
    ('Lalibela'),
    ('Mekele'),
    ('Nekemte'),
    ('Shashamane'),
    ('Shire'),
    ('Woldia'),
    ('Wolaita Sodo')
) AS v(name) ON c.code = 'ET'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Fiji (FJ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ba'),
    ('Labasa'),
    ('Lautoka'),
    ('Levuka'),
    ('Nadi'),
    ('Nasinu'),
    ('Nausori'),
    ('Savusavu'),
    ('Sigatoka'),
    ('Suva'),
    ('Tavua')
) AS v(name) ON c.code = 'FJ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Finland (FI)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Espoo'),
    ('Helsinki'),
    ('Hyvinkaa'),
    ('Joensuu'),
    ('Jyvaskyla'),
    ('Kajaani'),
    ('Kokkola'),
    ('Kotka'),
    ('Kouvola'),
    ('Kuopio'),
    ('Lahti'),
    ('Lappeenranta'),
    ('Mikkeli'),
    ('Oulu'),
    ('Pori'),
    ('Porvoo'),
    ('Rauma'),
    ('Rovaniemi'),
    ('Seinajoki'),
    ('Tampere'),
    ('Turku'),
    ('Vaasa'),
    ('Vantaa')
) AS v(name) ON c.code = 'FI'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- France (FR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Amiens'),
    ('Angers'),
    ('Annecy'),
    ('Antibes'),
    ('Argenteuil'),
    ('Asnières-sur-Seine'),
    ('Aubervilliers'),
    ('Avignon'),
    ('Besançon'),
    ('Bordeaux'),
    ('Boulogne-Billancourt'),
    ('Brest'),
    ('Caen'),
    ('Cannes'),
    ('Clermont-Ferrand'),
    ('Dijon'),
    ('Dunkirk'),
    ('Fort-de-France'),
    ('Grenoble'),
    ('Le Havre'),
    ('Le Mans'),
    ('Lille'),
    ('Limoges'),
    ('Lyon'),
    ('Marseille'),
    ('Metz'),
    ('Montpellier'),
    ('Mulhouse'),
    ('Nancy'),
    ('Nanterre'),
    ('Nantes'),
    ('Nice'),
    ('Nîmes'),
    ('Orléans'),
    ('Paris'),
    ('Pau'),
    ('Perpignan'),
    ('Reims'),
    ('Rennes'),
    ('Rouen'),
    ('Saint-Denis'),
    ('Saint-Etienne'),
    ('Strasbourg'),
    ('Toulon'),
    ('Toulouse'),
    ('Tours'),
    ('Villeurbanne')
) AS v(name) ON c.code = 'FR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Gabon (GA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bitam'),
    ('Booue'),
    ('Franceville'),
    ('Koulamoutou'),
    ('Lambarene'),
    ('Lastoursville'),
    ('Libreville'),
    ('Makokou'),
    ('Moanda'),
    ('Mouila'),
    ('Ntoum'),
    ('Oyem'),
    ('Port-Gentil'),
    ('Tchibanga')
) AS v(name) ON c.code = 'GA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Gambia (GM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Banjul'),
    ('Bansang'),
    ('Basse Santa Su'),
    ('Brikama'),
    ('Farafenni'),
    ('Janjanbureh'),
    ('Kanifing'),
    ('Kerewan'),
    ('Kuntaur'),
    ('Serekunda')
) AS v(name) ON c.code = 'GM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Georgia (GE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Akhaltsikhe'),
    ('Akhalkalaki'),
    ('Ambrolauri'),
    ('Batumi'),
    ('Bolnisi'),
    ('Chiatura'),
    ('Gori'),
    ('Gurjaani'),
    ('Khashuri'),
    ('Kobuleti'),
    ('Kutaisi'),
    ('Lentekhi'),
    ('Mtskheta'),
    ('Ozurgeti'),
    ('Poti'),
    ('Rustavi'),
    ('Samtredia'),
    ('Senaki'),
    ('Sighnaghi'),
    ('Tbilisi'),
    ('Telavi'),
    ('Tskhinvali'),
    ('Tsqaltubo'),
    ('Zugdidi')
) AS v(name) ON c.code = 'GE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Germany (DE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aachen'),
    ('Augsburg'),
    ('Berlin'),
    ('Bielefeld'),
    ('Bochum'),
    ('Bonn'),
    ('Bottrop'),
    ('Braunschweig'),
    ('Bremen'),
    ('Chemnitz'),
    ('Cologne'),
    ('Dortmund'),
    ('Dresden'),
    ('Duisburg'),
    ('Dusseldorf'),
    ('Erfurt'),
    ('Erlangen'),
    ('Essen'),
    ('Frankfurt'),
    ('Freiburg im Breisgau'),
    ('Gelsenkirchen'),
    ('Hagen'),
    ('Halle'),
    ('Hamburg'),
    ('Hamm'),
    ('Hanover'),
    ('Heidelberg'),
    ('Heilbronn'),
    ('Ingolstadt'),
    ('Karlsruhe'),
    ('Kiel'),
    ('Krefeld'),
    ('Leipzig'),
    ('Leverkusen'),
    ('Lubeck'),
    ('Magdeburg'),
    ('Mainz'),
    ('Mannheim'),
    ('Moers'),
    ('Monchengladbach'),
    ('Mulheim an der Ruhr'),
    ('Munich'),
    ('Munster'),
    ('Nuremberg'),
    ('Oberhausen'),
    ('Oldenburg'),
    ('Osnabruck'),
    ('Paderborn'),
    ('Potsdam'),
    ('Recklinghausen'),
    ('Regensburg'),
    ('Rostock'),
    ('Saarbrucken'),
    ('Solingen'),
    ('Stuttgart'),
    ('Wiesbaden'),
    ('Wuppertal'),
    ('Wurzburg')
) AS v(name) ON c.code = 'DE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Ghana (GH)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Accra'),
    ('Ashaiman'),
    ('Berekum'),
    ('Bo'),
    ('Cape Coast'),
    ('Ejura'),
    ('Ho'),
    ('Koforidua'),
    ('Kumasi'),
    ('Mampong'),
    ('Nkoranza'),
    ('Obuasi'),
    ('Prestea'),
    ('Sekondi-Takoradi'),
    ('Suhum'),
    ('Sunyani'),
    ('Tamale'),
    ('Techiman'),
    ('Tema'),
    ('Wa'),
    ('Winneba'),
    ('Yendi')
) AS v(name) ON c.code = 'GH'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Greece (GR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Agrinio'),
    ('Alexandroupoli'),
    ('Athens'),
    ('Chalkida'),
    ('Chania'),
    ('Drama'),
    ('Heraklion'),
    ('Ioannina'),
    ('Kalamata'),
    ('Kallithea'),
    ('Katerini'),
    ('Kavala'),
    ('Keratsini'),
    ('Kilkis'),
    ('Komotini'),
    ('Kozani'),
    ('Lamia'),
    ('Larissa'),
    ('Livadeia'),
    ('Mytilene'),
    ('Nikaia'),
    ('Patra'),
    ('Peristeri'),
    ('Piraeus'),
    ('Rhodes'),
    ('Serres'),
    ('Thessaloniki'),
    ('Trikala'),
    ('Volos'),
    ('Xanthi')
) AS v(name) ON c.code = 'GR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Grenada (GD)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Gouyave'),
    ('Grenville'),
    ('Saint David''s'),
    ('Saint George''s'),
    ('Victoria')
) AS v(name) ON c.code = 'GD'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Guatemala (GT)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Amatitlan'),
    ('Antigua Guatemala'),
    ('Chimaltenango'),
    ('Chiquimula'),
    ('Coban'),
    ('Escuintla'),
    ('Guatemala City'),
    ('Huehuetenango'),
    ('Jalapa'),
    ('Jutiapa'),
    ('Mazatenango'),
    ('Mixco'),
    ('Petapa'),
    ('Puerto Barrios'),
    ('Quetzaltenango'),
    ('Retalhuleu'),
    ('San Marcos'),
    ('Santa Ana'),
    ('Solola'),
    ('Totonicapan'),
    ('Villa Nueva'),
    ('Zacapa')
) AS v(name) ON c.code = 'GT'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Guinea (GN)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Boke'),
    ('Coyah'),
    ('Conakry'),
    ('Dabola'),
    ('Dinguiraye'),
    ('Faranah'),
    ('Forecariah'),
    ('Fria'),
    ('Gueckedou'),
    ('Kankan'),
    ('Kerouane'),
    ('Kindia'),
    ('Kissidougou'),
    ('Koubia'),
    ('Koundara'),
    ('Kouroussa'),
    ('Labe'),
    ('Lola'),
    ('Macenta'),
    ('Mali'),
    ('Mamou'),
    ('Nzerekore'),
    ('Pita'),
    ('Siguiri'),
    ('Telimele'),
    ('Tougue'),
    ('Yomou')
) AS v(name) ON c.code = 'GN'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Guinea-Bissau (GW)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bafata'),
    ('Bissau'),
    ('Bolama'),
    ('Buba'),
    ('Canchungo'),
    ('Farim'),
    ('Gabu'),
    ('Mansoa'),
    ('Quinhamel')
) AS v(name) ON c.code = 'GW'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Guyana (GY)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Anna Regina'),
    ('Bartica'),
    ('Georgetown'),
    ('Linden'),
    ('Mahaica'),
    ('New Amsterdam'),
    ('Parika'),
    ('Rose Hall'),
    ('Skeldon')
) AS v(name) ON c.code = 'GY'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Haiti (HT)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Cap-Haitien'),
    ('Carrefour'),
    ('Delmas'),
    ('Gonaives'),
    ('Jacmel'),
    ('Jeremie'),
    ('Les Cayes'),
    ('Leogane'),
    ('Miragoane'),
    ('Petionville'),
    ('Port-au-Prince'),
    ('Port-de-Paix'),
    ('Saint-Marc'),
    ('Tabarre')
) AS v(name) ON c.code = 'HT'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Honduras (HN)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Choloma'),
    ('Choluteca'),
    ('Comayagua'),
    ('Danli'),
    ('El Progreso'),
    ('Juticalpa'),
    ('La Ceiba'),
    ('La Lima'),
    ('Nacaome'),
    ('Olanchito'),
    ('Puerto Cortes'),
    ('San Pedro Sula'),
    ('Santa Barbara'),
    ('Santa Rosa de Copan'),
    ('Siguatepeque'),
    ('Tela'),
    ('Tegucigalpa'),
    ('Tocoa'),
    ('Trujillo'),
    ('Villanueva')
) AS v(name) ON c.code = 'HN'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Hungary (HU)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bekescsaba'),
    ('Budapest'),
    ('Debrecen'),
    ('Dunaujvaros'),
    ('Eger'),
    ('Erd'),
    ('Gyor'),
    ('Hodmezovasarhely'),
    ('Kaposvar'),
    ('Kecskemet'),
    ('Miskolc'),
    ('Nagykanizsa'),
    ('Nyiregyhaza'),
    ('Pecs'),
    ('Salotarjan'),
    ('Sopron'),
    ('Szekesfehervar'),
    ('Szolnok'),
    ('Szombathely'),
    ('Tatabanya'),
    ('Veszprem'),
    ('Zalaegerszeg')
) AS v(name) ON c.code = 'HU'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Iceland (IS)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Akranes'),
    ('Akureyri'),
    ('Borgarnes'),
    ('Egilsstadir'),
    ('Hafnarfjordur'),
    ('Husavik'),
    ('Isafjordur'),
    ('Keflavik'),
    ('Kopavogur'),
    ('Mosfellsbaer'),
    ('Reykjavik'),
    ('Selfoss'),
    ('Siglufjordur'),
    ('Vestmannaeyjar')
) AS v(name) ON c.code = 'IS'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- India (IN)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Agra'),
    ('Ahmedabad'),
    ('Aligarh'),
    ('Amritsar'),
    ('Asansol'),
    ('Aurangabad'),
    ('Bangalore'),
    ('Bareilly'),
    ('Bhopal'),
    ('Bhubaneswar'),
    ('Chandigarh'),
    ('Chennai'),
    ('Coimbatore'),
    ('Delhi'),
    ('Dhanbad'),
    ('Faridabad'),
    ('Ghaziabad'),
    ('Guwahati'),
    ('Gwalior'),
    ('Howrah'),
    ('Hyderabad'),
    ('Indore'),
    ('Jabalpur'),
    ('Jaipur'),
    ('Jalandhar'),
    ('Jammu'),
    ('Jodhpur'),
    ('Kanpur'),
    ('Kochi'),
    ('Kolkata'),
    ('Kozhikode'),
    ('Lucknow'),
    ('Ludhiana'),
    ('Madurai'),
    ('Meerut'),
    ('Mumbai'),
    ('Mysore'),
    ('Nagpur'),
    ('Nashik'),
    ('Patna'),
    ('Prayagraj'),
    ('Pune'),
    ('Raipur'),
    ('Rajkot'),
    ('Ranchi'),
    ('Srinagar'),
    ('Surat'),
    ('Thiruvananthapuram'),
    ('Tiruchirappalli'),
    ('Vadodara'),
    ('Varanasi'),
    ('Vijayawada'),
    ('Visakhapatnam')
) AS v(name) ON c.code = 'IN'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Indonesia (ID)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ambon'),
    ('Balikpapan'),
    ('Banda Aceh'),
    ('Bandar Lampung'),
    ('Banjarmasin'),
    ('Batam'),
    ('Bekasi'),
    ('Bengkulu'),
    ('Bogor'),
    ('Bukittinggi'),
    ('Cimahi'),
    ('Depok'),
    ('Denpasar'),
    ('Jakarta'),
    ('Jambi'),
    ('Jayapura'),
    ('Kediri'),
    ('Kupang'),
    ('Makassar'),
    ('Malang'),
    ('Manado'),
    ('Mataram'),
    ('Medan'),
    ('Padang'),
    ('Palangkaraya'),
    ('Palembang'),
    ('Palu'),
    ('Pekanbaru'),
    ('Pontianak'),
    ('Samarinda'),
    ('Semarang'),
    ('Serang'),
    ('Sorong'),
    ('Surabaya'),
    ('Surakarta'),
    ('Tangerang'),
    ('Tasikmalaya'),
    ('Ternate'),
    ('Yogyakarta')
) AS v(name) ON c.code = 'ID'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Iran (IR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ahvaz'),
    ('Arak'),
    ('Ardabil'),
    ('Babol'),
    ('Bandar Abbas'),
    ('Birjand'),
    ('Bojnord'),
    ('Bushehr'),
    ('Gorgan'),
    ('Hamadan'),
    ('Isfahan'),
    ('Karaj'),
    ('Kerman'),
    ('Kermanshah'),
    ('Khomeinishahr'),
    ('Khorramabad'),
    ('Mashhad'),
    ('Qazvin'),
    ('Qom'),
    ('Rasht'),
    ('Sabzevar'),
    ('Sanandaj'),
    ('Semnan'),
    ('Shahrekord'),
    ('Shiraz'),
    ('Sari'),
    ('Tabriz'),
    ('Tehran'),
    ('Urmia'),
    ('Yasuj'),
    ('Yazd'),
    ('Zahedan'),
    ('Zanjan')
) AS v(name) ON c.code = 'IR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Iraq (IQ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Al Amarah'),
    ('Al Fallujah'),
    ('Al Hillah'),
    ('Al Kut'),
    ('Al Najaf'),
    ('Al Nasiriyah'),
    ('Al Ramadi'),
    ('Al Samawah'),
    ('Ar Rutba'),
    ('Arbil'),
    ('As Sulaymaniyah'),
    ('Baghdad'),
    ('Basra'),
    ('Dohuk'),
    ('Halabja'),
    ('Karbala'),
    ('Kirkuk'),
    ('Mosul'),
    ('Sinjar'),
    ('Tikrit'),
    ('Zakho')
) AS v(name) ON c.code = 'IQ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Ireland (IE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Athlone'),
    ('Bray'),
    ('Carlow'),
    ('Castlebar'),
    ('Cavan'),
    ('Clonmel'),
    ('Cork'),
    ('Drogheda'),
    ('Dublin'),
    ('Dundalk'),
    ('Ennis'),
    ('Galway'),
    ('Kilkenny'),
    ('Letterkenny'),
    ('Limerick'),
    ('Longford'),
    ('Mullingar'),
    ('Naas'),
    ('Navan'),
    ('Portlaoise'),
    ('Sligo'),
    ('Tralee'),
    ('Tullamore'),
    ('Waterford'),
    ('Wexford')
) AS v(name) ON c.code = 'IE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Israel (IL)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ashdod'),
    ('Ashkelon'),
    ('Bat Yam'),
    ('Beer Sheva'),
    ('Bnei Brak'),
    ('Eilat'),
    ('Haifa'),
    ('Herzliya'),
    ('Holon'),
    ('Jerusalem'),
    ('Kfar Saba'),
    ('Lod'),
    ('Nahariya'),
    ('Nazareth'),
    ('Netanya'),
    ('Petah Tikva'),
    ('Ramat Gan'),
    ('Rishon LeZion'),
    ('Rehovot'),
    ('Tel Aviv'),
    ('Tiberias')
) AS v(name) ON c.code = 'IL'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Italy (IT)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ancona'),
    ('Arezzo'),
    ('Bari'),
    ('Bergamo'),
    ('Bologna'),
    ('Bolzano'),
    ('Brescia'),
    ('Cagliari'),
    ('Catania'),
    ('Ferrara'),
    ('Florence'),
    ('Foggia'),
    ('Genoa'),
    ('Giugliano in Campania'),
    ('Livorno'),
    ('Messina'),
    ('Milan'),
    ('Modena'),
    ('Monza'),
    ('Naples'),
    ('Padua'),
    ('Palermo'),
    ('Parma'),
    ('Perugia'),
    ('Prato'),
    ('Ravenna'),
    ('Reggio Calabria'),
    ('Reggio Emilia'),
    ('Rimini'),
    ('Rome'),
    ('Salerno'),
    ('Sassari'),
    ('Syracuse'),
    ('Taranto'),
    ('Trieste'),
    ('Turin'),
    ('Venice'),
    ('Verona'),
    ('Vicenza')
) AS v(name) ON c.code = 'IT'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Jamaica (JM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Black River'),
    ('Falmouth'),
    ('Kingston'),
    ('Mandeville'),
    ('May Pen'),
    ('Montego Bay'),
    ('Morant Bay'),
    ('Old Harbour'),
    ('Portmore'),
    ('Port Antonio'),
    ('Saint Ann''s Bay'),
    ('Savanna-la-Mar'),
    ('Spanish Town')
) AS v(name) ON c.code = 'JM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Japan (JP)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Chiba'),
    ('Fukuoka'),
    ('Fukushima'),
    ('Gifu'),
    ('Hamamatsu'),
    ('Hiroshima'),
    ('Kagoshima'),
    ('Kanazawa'),
    ('Kawasaki'),
    ('Kitakyushu'),
    ('Kobe'),
    ('Kumamoto'),
    ('Kyoto'),
    ('Matsuyama'),
    ('Nagano'),
    ('Nagasaki'),
    ('Nagoya'),
    ('Naha'),
    ('Niigata'),
    ('Okayama'),
    ('Osaka'),
    ('Sagamihara'),
    ('Saitama'),
    ('Sapporo'),
    ('Sendai'),
    ('Shizuoka'),
    ('Tokyo'),
    ('Utsunomiya'),
    ('Yokohama'),
    ('Yokosuka')
) AS v(name) ON c.code = 'JP'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Jordan (JO)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aqaba'),
    ('Amman'),
    ('Irbid'),
    ('Jerash'),
    ('Karak'),
    ('Madaba'),
    ('Mafraq'),
    ('Maan'),
    ('Ramtha'),
    ('Russeifa'),
    ('Salt'),
    ('Tafilah'),
    ('Zarqa')
) AS v(name) ON c.code = 'JO'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Kazakhstan (KZ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aktau'),
    ('Aktobe'),
    ('Almaty'),
    ('Astana'),
    ('Atyrau'),
    ('Baikonur'),
    ('Ekibastuz'),
    ('Karaganda'),
    ('Kokshetau'),
    ('Kostanay'),
    ('Kyzylorda'),
    ('Pavlodar'),
    ('Petropavl'),
    ('Ridder'),
    ('Rudny'),
    ('Semey'),
    ('Shymkent'),
    ('Taraz'),
    ('Temirtau'),
    ('Turkestan'),
    ('Uralsk'),
    ('Ust-Kamenogorsk'),
    ('Zhambyl'),
    ('Zhezkazgan')
) AS v(name) ON c.code = 'KZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Kenya (KE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Eldoret'),
    ('Embu'),
    ('Garissa'),
    ('Homabay'),
    ('Isiolo'),
    ('Kakamega'),
    ('Kericho'),
    ('Kisii'),
    ('Kisumu'),
    ('Kitale'),
    ('Lamu'),
    ('Lodwar'),
    ('Machakos'),
    ('Malindi'),
    ('Marsabit'),
    ('Meru'),
    ('Mombasa'),
    ('Moyale'),
    ('Muranga'),
    ('Nairobi'),
    ('Nakuru'),
    ('Nanyuki'),
    ('Narok'),
    ('Nyahururu'),
    ('Nyeri'),
    ('Thika'),
    ('Voi'),
    ('Wajir'),
    ('Webuye')
) AS v(name) ON c.code = 'KE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Kiribati (KI)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Betio'),
    ('Bikenibeu'),
    ('Bonriki'),
    ('Eita'),
    ('London'),
    ('Tabwakea'),
    ('Tarawa')
) AS v(name) ON c.code = 'KI'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Kuwait (KW)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Abu Halifa'),
    ('Al Ahmadi'),
    ('Al Farwaniyah'),
    ('Al Jahra'),
    ('Al Mahboula'),
    ('Fahaheel'),
    ('Hawalli'),
    ('Kuwait City'),
    ('Mangaf'),
    ('Rumaithiya'),
    ('Sabah Al Salem'),
    ('Salmiya')
) AS v(name) ON c.code = 'KW'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Kyrgyzstan (KG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Balykchy'),
    ('Bishkek'),
    ('Isfana'),
    ('Jalal-Abad'),
    ('Karakol'),
    ('Kant'),
    ('Kara-Suu'),
    ('Naryn'),
    ('Nookat'),
    ('Osh'),
    ('Talas'),
    ('Tokmok'),
    ('Uzgen')
) AS v(name) ON c.code = 'KG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Laos (LA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Attapeu'),
    ('Ban Houayxay'),
    ('Luang Namtha'),
    ('Luang Prabang'),
    ('Paksan'),
    ('Pakxe'),
    ('Phonsavan'),
    ('Sainyabuli'),
    ('Salavan'),
    ('Sam Neua'),
    ('Savannakhet'),
    ('Thakhek'),
    ('Vientiane'),
    ('Xam Nua'),
    ('Xekong')
) AS v(name) ON c.code = 'LA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Latvia (LV)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bauska'),
    ('Cesis'),
    ('Daugavpils'),
    ('Jekabpils'),
    ('Jelgava'),
    ('Jurmala'),
    ('Liepaja'),
    ('Limbazi'),
    ('Ludza'),
    ('Ogre'),
    ('Rezekne'),
    ('Riga'),
    ('Salaspils'),
    ('Sigulda'),
    ('Talsi'),
    ('Tukums'),
    ('Valmiera'),
    ('Ventspils')
) AS v(name) ON c.code = 'LV'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Lebanon (LB)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Baalbek'),
    ('Beirut'),
    ('Byblos'),
    ('Jounieh'),
    ('Nabatieh'),
    ('Saida'),
    ('Tripoli'),
    ('Tyre'),
    ('Zahle')
) AS v(name) ON c.code = 'LB'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Lesotho (LS)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Butha-Buthe'),
    ('Hlotse'),
    ('Mafeking'),
    ('Mafeteng'),
    ('Maseru'),
    ('Mohale''s Hoek'),
    ('Mokhotlong'),
    ('Qacha''s Nek'),
    ('Quthing'),
    ('Teyateyaneng')
) AS v(name) ON c.code = 'LS'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Liberia (LR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bensonville'),
    ('Buchanan'),
    ('Fish Town'),
    ('Ganta'),
    ('Gbarnga'),
    ('Harper'),
    ('Kakata'),
    ('Monrovia'),
    ('Robertsport'),
    ('Sanniquellie'),
    ('Tubmanburg'),
    ('Voinjama'),
    ('Zwedru')
) AS v(name) ON c.code = 'LR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Libya (LY)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ajdabiya'),
    ('Al Bayda'),
    ('Al Jufrah'),
    ('Al Kufrah'),
    ('Az Zawiyah'),
    ('Bani Walid'),
    ('Benghazi'),
    ('Derna'),
    ('Ghadames'),
    ('Gharyan'),
    ('Ghat'),
    ('Khoms'),
    ('Misrata'),
    ('Murzuq'),
    ('Sabha'),
    ('Sabratha'),
    ('Sirte'),
    ('Tobruk'),
    ('Tripoli'),
    ('Zintan'),
    ('Zliten')
) AS v(name) ON c.code = 'LY'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Liechtenstein (LI)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Balzers'),
    ('Eschen'),
    ('Gamprin'),
    ('Mauren'),
    ('Planken'),
    ('Ruggell'),
    ('Schaan'),
    ('Schellenberg'),
    ('Triesen'),
    ('Triesenberg'),
    ('Vaduz')
) AS v(name) ON c.code = 'LI'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Lithuania (LT)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Alytus'),
    ('Druskininkai'),
    ('Elektrenai'),
    ('Jonava'),
    ('Joniskis'),
    ('Kaunas'),
    ('Klaipeda'),
    ('Marijampole'),
    ('Mazeikiai'),
    ('Neringa'),
    ('Pakruojis'),
    ('Palanga'),
    ('Panevezys'),
    ('Plunge'),
    ('Radviliskis'),
    ('Rokiskis'),
    ('Siauliai'),
    ('Skuodas'),
    ('Taurage'),
    ('Telsiai'),
    ('Ukmerge'),
    ('Utena'),
    ('Varena'),
    ('Vilnius'),
    ('Visaginas')
) AS v(name) ON c.code = 'LT'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Luxembourg (LU)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Differdange'),
    ('Dudelange'),
    ('Echternach'),
    ('Esch-sur-Alzette'),
    ('Ettelbruck'),
    ('Luxembourg City'),
    ('Remich'),
    ('Rumelange'),
    ('Wiltz')
) AS v(name) ON c.code = 'LU'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Madagascar (MG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ambanja'),
    ('Ambatondrazaka'),
    ('Ambovombe'),
    ('Amparafaravola'),
    ('Antalaha'),
    ('Antananarivo'),
    ('Antsiranana'),
    ('Antsirabe'),
    ('Antsohihy'),
    ('Farafangana'),
    ('Fianarantsoa'),
    ('Ihosy'),
    ('Maintirano'),
    ('Mahajanga'),
    ('Mananjary'),
    ('Mananara'),
    ('Maroantsetra'),
    ('Miandrivazo'),
    ('Moramanga'),
    ('Morondava'),
    ('Nosy Be'),
    ('Sambava'),
    ('Tamatave'),
    ('Toamasina'),
    ('Tolanaro'),
    ('Toliary'),
    ('Tsiroanomandidy')
) AS v(name) ON c.code = 'MG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Malawi (MW)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Balaka'),
    ('Blantyre'),
    ('Chiradzulu'),
    ('Chitipa'),
    ('Dedza'),
    ('Dowa'),
    ('Karonga'),
    ('Kasungu'),
    ('Lilongwe'),
    ('Liwonde'),
    ('Machinga'),
    ('Mangochi'),
    ('Mchinji'),
    ('Mponela'),
    ('Mzimba'),
    ('Mzuzu'),
    ('Nkhotakota'),
    ('Nsanje'),
    ('Ntcheu'),
    ('Ntchisi'),
    ('Rumphi'),
    ('Salima'),
    ('Thyolo'),
    ('Zomba')
) AS v(name) ON c.code = 'MW'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Malaysia (MY)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Alor Setar'),
    ('Batu Pahat'),
    ('Ipoh'),
    ('Johor Bahru'),
    ('Kajang'),
    ('Klang'),
    ('Kota Bahru'),
    ('Kota Kinabalu'),
    ('Kuala Lumpur'),
    ('Kuala Terengganu'),
    ('Kuching'),
    ('Miri'),
    ('Petaling Jaya'),
    ('Sandakan'),
    ('Seremban'),
    ('Shah Alam'),
    ('Sibu'),
    ('Subang Jaya'),
    ('Tawau')
) AS v(name) ON c.code = 'MY'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Maldives (MV)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Addu City'),
    ('Eydhafushi'),
    ('Fuvahmulah'),
    ('Kulhudhuffushi'),
    ('Male'),
    ('Naifaru'),
    ('Thinadhoo'),
    ('Ungoofaaru')
) AS v(name) ON c.code = 'MV'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Mali (ML)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bamako'),
    ('Bougouni'),
    ('Gao'),
    ('Kayes'),
    ('Kidal'),
    ('Kita'),
    ('Koulikoro'),
    ('Koutiala'),
    ('Markala'),
    ('Menaka'),
    ('Mopti'),
    ('Niono'),
    ('San'),
    ('Segou'),
    ('Sikasso'),
    ('Taoudenit'),
    ('Tessalit'),
    ('Timbuktu'),
    ('Yorosso')
) AS v(name) ON c.code = 'ML'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Malta (MT)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Attard'),
    ('Birkirkara'),
    ('Birgu'),
    ('Fgura'),
    ('Gzira'),
    ('Hamrun'),
    ('Iklin'),
    ('Marsaskala'),
    ('Mosta'),
    ('Naxxar'),
    ('Paola'),
    ('Qormi'),
    ('Rabat'),
    ('Sliema'),
    ('St Julian''s'),
    ('St Paul''s Bay'),
    ('Swieqi'),
    ('Valletta'),
    ('Zabbar'),
    ('Zejtun'),
    ('Zurrieq')
) AS v(name) ON c.code = 'MT'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Marshall Islands (MH)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ailuk'),
    ('Arno'),
    ('Ebeye'),
    ('Jabor'),
    ('Majuro'),
    ('Wotje')
) AS v(name) ON c.code = 'MH'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Mauritania (MR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aleg'),
    ('Atar'),
    ('Boghe'),
    ('Kaedi'),
    ('Kiffa'),
    ('Nema'),
    ('Nouadhibou'),
    ('Nouakchott'),
    ('Rosso'),
    ('Selibaby'),
    ('Tidjikja'),
    ('Zouerate')
) AS v(name) ON c.code = 'MR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Mauritius (MU)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Beau Bassin-Rose Hill'),
    ('Curepipe'),
    ('Four Bornes'),
    ('Port Louis'),
    ('Quatre Bornes'),
    ('Vacoas-Phoenix')
) AS v(name) ON c.code = 'MU'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Mexico (MX)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Acapulco'),
    ('Aguascalientes'),
    ('Campeche'),
    ('Cancun'),
    ('Celaya'),
    ('Chihuahua'),
    ('Ciudad Juarez'),
    ('Ciudad Obregon'),
    ('Coatzacoalcos'),
    ('Colima'),
    ('Culiacan'),
    ('Durango'),
    ('Ecatepec'),
    ('Ensenada'),
    ('Guadalajara'),
    ('Hermosillo'),
    ('Irapuato'),
    ('Leon'),
    ('Mazatlan'),
    ('Merida'),
    ('Mexicali'),
    ('Mexico City'),
    ('Monclova'),
    ('Monterrey'),
    ('Morelia'),
    ('Naucalpan'),
    ('Nezahualcoyotl'),
    ('Nuevo Laredo'),
    ('Oaxaca'),
    ('Pachuca'),
    ('Puebla'),
    ('Queretaro'),
    ('Reynosa'),
    ('Saltillo'),
    ('San Luis Potosi'),
    ('Tampico'),
    ('Tijuana'),
    ('Tlalnepantla'),
    ('Toluca'),
    ('Torreon'),
    ('Tuxtla Gutierrez'),
    ('Veracruz'),
    ('Villahermosa'),
    ('Xalapa'),
    ('Zapopan')
) AS v(name) ON c.code = 'MX'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Micronesia (FM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Colonia'),
    ('Kolonia'),
    ('Lelu'),
    ('Palikir'),
    ('Tofol'),
    ('Weno')
) AS v(name) ON c.code = 'FM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Moldova (MD)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Balti'),
    ('Bender'),
    ('Cahul'),
    ('Chisinau'),
    ('Comrat'),
    ('Edinet'),
    ('Floresti'),
    ('Hincesti'),
    ('Orhei'),
    ('Rezina'),
    ('Rybnitsa'),
    ('Singerei'),
    ('Soroca'),
    ('Straseni'),
    ('Tiraspol'),
    ('Ungheni')
) AS v(name) ON c.code = 'MD'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Monaco (MC)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Fontvieille'),
    ('La Condamine'),
    ('Monaco-Ville'),
    ('Monte Carlo'),
    ('Moneghetti')
) AS v(name) ON c.code = 'MC'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Mongolia (MN)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Altai'),
    ('Arvayheer'),
    ('Baganuur'),
    ('Baruun-Urt'),
    ('Bayanhongor'),
    ('Bulgan'),
    ('Choibalsan'),
    ('Choir'),
    ('Darkhan'),
    ('Dalandzadgad'),
    ('Erdenet'),
    ('Khatgal'),
    ('Mandalgovi'),
    ('Moron'),
    ('Nalaikh'),
    ('Ovoot'),
    ('Sainshanda'),
    ('Sukhbaatar'),
    ('Ulaan-Uul'),
    ('Ulaanbaatar'),
    ('Ulaangom'),
    ('Uliastai'),
    ('Undurkhaan'),
    ('Zuunmod')
) AS v(name) ON c.code = 'MN'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Montenegro (ME)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bar'),
    ('Berane'),
    ('Bijelo Polje'),
    ('Budva'),
    ('Cetinje'),
    ('Herceg Novi'),
    ('Kotor'),
    ('Niksic'),
    ('Pljevlja'),
    ('Podgorica'),
    ('Rozaje'),
    ('Tivat'),
    ('Ulcinj')
) AS v(name) ON c.code = 'ME'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Morocco (MA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Agadir'),
    ('Aït Melloul'),
    ('Al Hoceima'),
    ('Beni Mellal'),
    ('Berrechid'),
    ('Casablanca'),
    ('El Jadida'),
    ('Errachidia'),
    ('Essaouira'),
    ('Fes'),
    ('Guelmim'),
    ('Inezgane'),
    ('Kenitra'),
    ('Khemisset'),
    ('Khenifra'),
    ('Khouribga'),
    ('Laayoune'),
    ('Larache'),
    ('Marrakech'),
    ('Meknes'),
    ('Mohammedia'),
    ('Nador'),
    ('Oujda'),
    ('Rabat'),
    ('Safi'),
    ('Sale'),
    ('Settat'),
    ('Sidi Kacem'),
    ('Tanger'),
    ('Taza'),
    ('Temara'),
    ('Tetouan'),
    ('Tiznit')
) AS v(name) ON c.code = 'MA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Mozambique (MZ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Beira'),
    ('Chimoio'),
    ('Cuamba'),
    ('Dondo'),
    ('Inhambane'),
    ('Lichinga'),
    ('Maputo'),
    ('Matola'),
    ('Maxixe'),
    ('Mocuba'),
    ('Monapo'),
    ('Nacala'),
    ('Nampula'),
    ('Pemba'),
    ('Quelimane'),
    ('Tete'),
    ('Xai-Xai'),
    ('Zambezi')
) AS v(name) ON c.code = 'MZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Myanmar (MM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bago'),
    ('Bassein'),
    ('Dawei'),
    ('Hakha'),
    ('Hpa-An'),
    ('Kalay'),
    ('Kengtung'),
    ('Kyaukme'),
    ('Lashio'),
    ('Loikaw'),
    ('Magway'),
    ('Mandalay'),
    ('Mawlamyine'),
    ('Meiktila'),
    ('Monywa'),
    ('Myingyan'),
    ('Myitkyina'),
    ('Naypyidaw'),
    ('Pakokku'),
    ('Pathein'),
    ('Pyay'),
    ('Sagaing'),
    ('Sittwe'),
    ('Taunggyi'),
    ('Thaton'),
    ('Yangon')
) AS v(name) ON c.code = 'MM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Namibia (NA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aranos'),
    ('Eenhana'),
    ('Gobabis'),
    ('Grootfontein'),
    ('Katima Mulilo'),
    ('Keetmanshoop'),
    ('Luderitz'),
    ('Mariental'),
    ('Okahandja'),
    ('Ongwediva'),
    ('Opuwo'),
    ('Oshakati'),
    ('Otjiwarongo'),
    ('Outapi'),
    ('Rehoboth'),
    ('Rundu'),
    ('Swakopmund'),
    ('Tsumeb'),
    ('Walvis Bay'),
    ('Windhoek')
) AS v(name) ON c.code = 'NA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Nauru (NR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aiwo'),
    ('Anabar'),
    ('Anetan'),
    ('Baiti'),
    ('Boe'),
    ('Buada'),
    ('Denigomodu'),
    ('Ewa'),
    ('Ijuw'),
    ('Meneng'),
    ('Nibok'),
    ('Uaboe'),
    ('Yaren')
) AS v(name) ON c.code = 'NR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Nepal (NP)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Baglung'),
    ('Banepa'),
    ('Bharatpur'),
    ('Bhimdatta'),
    ('Biratnagar'),
    ('Birgunj'),
    ('Butwal'),
    ('Damak'),
    ('Dhangadhi'),
    ('Dharan'),
    ('Ghorahi'),
    ('Hetauda'),
    ('Itahari'),
    ('Janakpur'),
    ('Kathmandu'),
    ('Kirtipur'),
    ('Lalitpur'),
    ('Lekhnath'),
    ('Nepalgunj'),
    ('Pokhara'),
    ('Siddharthanagar'),
    ('Tulsipur')
) AS v(name) ON c.code = 'NP'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Netherlands (NL)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Alkmaar'),
    ('Almere'),
    ('Amersfoort'),
    ('Amsterdam'),
    ('Apeldoorn'),
    ('Arnhem'),
    ('Breda'),
    ('Delft'),
    ('Deventer'),
    ('Dordrecht'),
    ('Ede'),
    ('Eindhoven'),
    ('Emmen'),
    ('Enschede'),
    ('Groningen'),
    ('Haarlem'),
    ('Haarlemmermeer'),
    ('Leeuwarden'),
    ('Leiden'),
    ('Maastricht'),
    ('Nijmegen'),
    ('Rotterdam'),
    ('s-Hertogenbosch'),
    ('The Hague'),
    ('Tilburg'),
    ('Utrecht'),
    ('Venlo'),
    ('Westland'),
    ('Zaanstad'),
    ('Zoetermeer')
) AS v(name) ON c.code = 'NL'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- New Zealand (NZ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Auckland'),
    ('Blenheim'),
    ('Christchurch'),
    ('Dunedin'),
    ('Gisborne'),
    ('Hamilton'),
    ('Hastings'),
    ('Invercargill'),
    ('Lower Hutt'),
    ('Masterton'),
    ('Napier'),
    ('Nelson'),
    ('New Plymouth'),
    ('Palmerston North'),
    ('Porirua'),
    ('Rotorua'),
    ('Tauranga'),
    ('Upper Hutt'),
    ('Wellington'),
    ('Whanganui'),
    ('Whangarei')
) AS v(name) ON c.code = 'NZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Nicaragua (NI)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bluefields'),
    ('Boaco'),
    ('Chinandega'),
    ('Chontales'),
    ('Ciudad Sandino'),
    ('Diriamba'),
    ('El Viejo'),
    ('Esteli'),
    ('Granada'),
    ('Jinotega'),
    ('Jinotepe'),
    ('Leon'),
    ('Managua'),
    ('Masaya'),
    ('Matagalpa'),
    ('Nueva Guinea'),
    ('Ocotal'),
    ('Puerto Cabezas'),
    ('Rivas'),
    ('Somoto')
) AS v(name) ON c.code = 'NI'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Niger (NE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Agadez'),
    ('Arlit'),
    ('Birni-N''Konni'),
    ('Diffa'),
    ('Dosso'),
    ('Gaya'),
    ('Loga'),
    ('Maradi'),
    ('Niamey'),
    ('Tahoua'),
    ('Tessaoua'),
    ('Tillaberi'),
    ('Zinder')
) AS v(name) ON c.code = 'NE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Nigeria (NG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aba'),
    ('Abeokuta'),
    ('Abuja'),
    ('Ado-Ekiti'),
    ('Akure'),
    ('Asaba'),
    ('Awka'),
    ('Bauchi'),
    ('Benin City'),
    ('Birnin Kebbi'),
    ('Calabar'),
    ('Damaturu'),
    ('Dutse'),
    ('Ekpoma'),
    ('Enugu'),
    ('Gombe'),
    ('Gusau'),
    ('Ibadan'),
    ('Ilorin'),
    ('Jos'),
    ('Kaduna'),
    ('Kano'),
    ('Katsina'),
    ('Lagos'),
    ('Lafia'),
    ('Lokoja'),
    ('Maiduguri'),
    ('Makurdi'),
    ('Minna'),
    ('Ogbomosho'),
    ('Onitsha'),
    ('Oshogbo'),
    ('Owerri'),
    ('Port Harcourt'),
    ('Sokoto'),
    ('Umuahia'),
    ('Uyo'),
    ('Warri'),
    ('Yenagoa'),
    ('Yola'),
    ('Zaria')
) AS v(name) ON c.code = 'NG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- North Korea (KP)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Anju'),
    ('Chongjin'),
    ('Haeju'),
    ('Hamhung'),
    ('Hyesan'),
    ('Kaechon'),
    ('Kaesong'),
    ('Kanggye'),
    ('Kimchaek'),
    ('Kosong'),
    ('Nampo'),
    ('Pyongyang'),
    ('Rason'),
    ('Sariwon'),
    ('Sinuiju'),
    ('Songnim'),
    ('Tanchon'),
    ('Wonsan')
) AS v(name) ON c.code = 'KP'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- North Macedonia (MK)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bitola'),
    ('Debar'),
    ('Delcevo'),
    ('Gevgelija'),
    ('Gostivar'),
    ('Kavadarci'),
    ('Kicevo'),
    ('Kocani'),
    ('Kratovo'),
    ('Kumanovo'),
    ('Negotino'),
    ('Ohrid'),
    ('Prilep'),
    ('Probistip'),
    ('Radovis'),
    ('Skopje'),
    ('Stip'),
    ('Struga'),
    ('Strumica'),
    ('Sveti Nikole'),
    ('Tetovo'),
    ('Valandovo'),
    ('Veles'),
    ('Vinica')
) AS v(name) ON c.code = 'MK'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Norway (NO)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Alesund'),
    ('Alta'),
    ('Bergen'),
    ('Bodo'),
    ('Drammen'),
    ('Fredrikstad'),
    ('Hamar'),
    ('Haugesund'),
    ('Kristiansand'),
    ('Kristiansund'),
    ('Larvik'),
    ('Lillehammer'),
    ('Mo i Rana'),
    ('Moss'),
    ('Narvik'),
    ('Oslo'),
    ('Porsgrunn'),
    ('Sandefjord'),
    ('Sandnes'),
    ('Sarpsborg'),
    ('Skien'),
    ('Stavanger'),
    ('Tonsberg'),
    ('Tromso'),
    ('Trondheim')
) AS v(name) ON c.code = 'NO'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Oman (OM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Adam'),
    ('Al Buraymi'),
    ('Bahla'),
    ('Ibra'),
    ('Ibri'),
    ('Khasab'),
    ('Muscat'),
    ('Nizwa'),
    ('Ruwi'),
    ('Salalah'),
    ('Sohar'),
    ('Sur'),
    ('Thumrait')
) AS v(name) ON c.code = 'OM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Pakistan (PK)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Abbottabad'),
    ('Bahawalpur'),
    ('Bhimber'),
    ('Dera Ghazi Khan'),
    ('Faisalabad'),
    ('Gujranwala'),
    ('Gujrat'),
    ('Hyderabad'),
    ('Islamabad'),
    ('Jhang'),
    ('Jhelum'),
    ('Karachi'),
    ('Kasur'),
    ('Khanewal'),
    ('Khushab'),
    ('Lahore'),
    ('Larkana'),
    ('Mardan'),
    ('Mingora'),
    ('Mirpur Khas'),
    ('Multan'),
    ('Muzaffarabad'),
    ('Nawabshah'),
    ('Okara'),
    ('Peshawar'),
    ('Quetta'),
    ('Rahimyar Khan'),
    ('Rawalpindi'),
    ('Sahiwal'),
    ('Sargodha'),
    ('Sheikhupura'),
    ('Sialkot'),
    ('Sibi'),
    ('Sukkur'),
    ('Wah Cantonment')
) AS v(name) ON c.code = 'PK'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Palau (PW)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Airai'),
    ('Kloulklubed'),
    ('Koror'),
    ('Meyungs'),
    ('Ngeraard'),
    ('Ngerulmud'),
    ('Ngchesar')
) AS v(name) ON c.code = 'PW'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Palestine (PS)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bethlehem'),
    ('Gaza'),
    ('Hebron'),
    ('Jericho'),
    ('Jenin'),
    ('Khan Yunis'),
    ('Nablus'),
    ('Qalqilya'),
    ('Rafah'),
    ('Ramallah'),
    ('Salfit'),
    ('Tulkarm')
) AS v(name) ON c.code = 'PS'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Panama (PA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Arraijan'),
    ('Chitre'),
    ('Colon'),
    ('David'),
    ('La Chorrera'),
    ('Las Tablas'),
    ('Panama City'),
    ('Penonome'),
    ('San Miguelito'),
    ('Santiago')
) AS v(name) ON c.code = 'PA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Papua New Guinea (PG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Arawa'),
    ('Daru'),
    ('Goroka'),
    ('Kavieng'),
    ('Kerema'),
    ('Kimbe'),
    ('Kokopo'),
    ('Kundiawa'),
    ('Lae'),
    ('Lorengau'),
    ('Madang'),
    ('Mendi'),
    ('Mount Hagen'),
    ('Popondetta'),
    ('Port Moresby'),
    ('Rabaul'),
    ('Vanimo'),
    ('Wabag'),
    ('Wewak')
) AS v(name) ON c.code = 'PG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Paraguay (PY)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Areguá'),
    ('Asunción'),
    ('Caaguazú'),
    ('Caazapá'),
    ('Capiatá'),
    ('Ciudad del Este'),
    ('Concepción'),
    ('Coronel Oviedo'),
    ('Encarnación'),
    ('Fernando de la Mora'),
    ('Hernandarias'),
    ('Lambaré'),
    ('Limpio'),
    ('Luque'),
    ('Mariano Roque Alonso'),
    ('Nemby'),
    ('Pedro Juan Caballero'),
    ('Pilar'),
    ('Presidente Franco'),
    ('San Lorenzo'),
    ('Villa Hayes'),
    ('Villarrica')
) AS v(name) ON c.code = 'PY'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Peru (PE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Arequipa'),
    ('Ayacucho'),
    ('Cajamarca'),
    ('Callao'),
    ('Chiclayo'),
    ('Chimbote'),
    ('Cusco'),
    ('Huancayo'),
    ('Huánuco'),
    ('Ica'),
    ('Iquitos'),
    ('Juliaca'),
    ('Lima'),
    ('Piura'),
    ('Pucallpa'),
    ('Sullana'),
    ('Tacna'),
    ('Tarapoto'),
    ('Trujillo')
) AS v(name) ON c.code = 'PE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Philippines (PH)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Angeles'),
    ('Antipolo'),
    ('Bacolod'),
    ('Baguio'),
    ('Batangas'),
    ('Butuan'),
    ('Cabanatuan'),
    ('Cagayan de Oro'),
    ('Caloocan'),
    ('Cebu City'),
    ('Cotabato'),
    ('Dagupan'),
    ('Davao'),
    ('General Santos'),
    ('Iligan'),
    ('Iloilo City'),
    ('Las Piñas'),
    ('Lapu-Lapu'),
    ('Legaspi'),
    ('Lucena'),
    ('Makati'),
    ('Malabon'),
    ('Mandaluyong'),
    ('Mandaue'),
    ('Manila'),
    ('Marikina'),
    ('Muntinlupa'),
    ('Navotas'),
    ('Olongapo'),
    ('Ormoc'),
    ('Paranaque'),
    ('Pasay'),
    ('Pasig'),
    ('Quezon City'),
    ('San Jose del Monte'),
    ('San Juan'),
    ('San Pablo'),
    ('Santa Rosa'),
    ('Taguig'),
    ('Valenzuela'),
    ('Zamboanga')
) AS v(name) ON c.code = 'PH'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Poland (PL)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Białystok'),
    ('Bielsko-Biała'),
    ('Bydgoszcz'),
    ('Bytom'),
    ('Chorzów'),
    ('Częstochowa'),
    ('Dąbrowa Górnicza'),
    ('Elbląg'),
    ('Gdańsk'),
    ('Gdynia'),
    ('Gliwice'),
    ('Gorzów Wielkopolski'),
    ('Katowice'),
    ('Kielce'),
    ('Kraków'),
    ('Legnica'),
    ('Lublin'),
    ('Łódź'),
    ('Olsztyn'),
    ('Opole'),
    ('Płock'),
    ('Poznań'),
    ('Radom'),
    ('Ruda Śląska'),
    ('Rzeszów'),
    ('Sosnowiec'),
    ('Szczecin'),
    ('Toruń'),
    ('Tychy'),
    ('Warsaw'),
    ('Włocławek'),
    ('Wrocław'),
    ('Zabrze'),
    ('Zielona Góra')
) AS v(name) ON c.code = 'PL'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Portugal (PT)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Almada'),
    ('Amadora'),
    ('Angra do Heroísmo'),
    ('Aveiro'),
    ('Barcelos'),
    ('Barreiro'),
    ('Braga'),
    ('Bragança'),
    ('Cascais'),
    ('Castelo Branco'),
    ('Coimbra'),
    ('Évora'),
    ('Faro'),
    ('Figueira da Foz'),
    ('Funchal'),
    ('Gondomar'),
    ('Guimarães'),
    ('Leiria'),
    ('Lisboa'),
    ('Loures'),
    ('Matosinhos'),
	('Melres'),
    ('Odivelas'),
    ('Oeiras'),
    ('Ponta Delgada'),
    ('Portimão'),
    ('Porto'),
    ('Póvoa de Varzim'),
    ('Queluz'),
    ('Santarém'),
    ('Seixal'),
    ('Setúbal'),
    ('Sintra'),
    ('Viana do Castelo'),
    ('Vila Franca de Xira'),
    ('Vila Nova de Gaia'),
	('Vila Real'),
    ('Viseu')
) AS v(name) ON c.code = 'PT'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Qatar (QA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Al Khor'),
    ('Al Rayyan'),
    ('Al Wakrah'),
    ('Doha'),
    ('Dukhan'),
    ('Lusail'),
    ('Madinat ash Shamal'),
    ('Mesaieed'),
    ('Umm Salal')
) AS v(name) ON c.code = 'QA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Romania (RO)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Arad'),
    ('Bacău'),
    ('Baia Mare'),
    ('Bistrița'),
    ('Botoșani'),
    ('Brăila'),
    ('Brașov'),
    ('Bucharest'),
    ('Buzău'),
    ('Cluj-Napoca'),
    ('Constanța'),
    ('Craiova'),
    ('Deva'),
    ('Drobeta-Turnu Severin'),
    ('Focșani'),
    ('Galați'),
    ('Iași'),
    ('Oradea'),
    ('Piatra Neamț'),
    ('Pitești'),
    ('Ploiești'),
    ('Râmnicu Vâlcea'),
    ('Reșița'),
    ('Satu Mare'),
    ('Sfântu Gheorghe'),
    ('Sibiu'),
    ('Slatina'),
    ('Slobozia'),
    ('Suceava'),
    ('Târgoviște'),
    ('Târgu Jiu'),
    ('Târgu Mureș'),
    ('Timișoara'),
    ('Tulcea'),
    ('Vaslui'),
    ('Zalău'),
    ('Alexandria')
) AS v(name) ON c.code = 'RO'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Russia (RU)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Arkhangelsk'),
    ('Astrakhan'),
    ('Barnaul'),
    ('Bryansk'),
    ('Chelyabinsk'),
    ('Chita'),
    ('Irkutsk'),
    ('Ivanovo'),
    ('Izhevsk'),
    ('Kazan'),
    ('Kemerovo'),
    ('Khabarovsk'),
    ('Kirov'),
    ('Krasnodar'),
    ('Krasnoyarsk'),
    ('Kursk'),
    ('Lipetsk'),
    ('Magnitogorsk'),
    ('Moscow'),
    ('Murmansk'),
    ('Naberezhnye Chelny'),
    ('Nizhny Novgorod'),
    ('Novokuznetsk'),
    ('Novosibirsk'),
    ('Omsk'),
    ('Orenburg'),
    ('Penza'),
    ('Perm'),
    ('Rostov-on-Don'),
    ('Ryazan'),
    ('Saint Petersburg'),
    ('Samara'),
    ('Saratov'),
    ('Stavropol'),
    ('Tolyatti'),
    ('Tomsk'),
    ('Tula'),
    ('Tver'),
    ('Tyumen'),
    ('Ufa'),
    ('Ulyanovsk'),
    ('Vladivostok'),
    ('Volgograd'),
    ('Voronezh'),
    ('Yakutsk'),
    ('Yaroslavl'),
    ('Yekaterinburg')
) AS v(name) ON c.code = 'RU'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Rwanda (RW)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Butare'),
    ('Byumba'),
    ('Cyangugu'),
    ('Gisenyi'),
    ('Gitarama'),
    ('Kibungo'),
    ('Kibuye'),
    ('Kigali'),
    ('Musanze'),
    ('Ruhengeri')
) AS v(name) ON c.code = 'RW'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Saint Kitts and Nevis (KN)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Basseterre'),
    ('Charlestown'),
    ('Dieppe Bay Town'),
    ('Gingerland'),
    ('Nicola Town'),
    ('Sandy Point Town')
) AS v(name) ON c.code = 'KN'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Saint Lucia (LC)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Anse La Raye'),
    ('Canaries'),
    ('Castries'),
    ('Choiseul'),
    ('Dennery'),
    ('Gros Islet'),
    ('Laborie'),
    ('Micoud'),
    ('Soufrière'),
    ('Vieux Fort')
) AS v(name) ON c.code = 'LC'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Saint Vincent and the Grenadines (VC)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Barrouallie'),
    ('Chateaubelair'),
    ('Georgetown'),
    ('Kingstown'),
    ('Layou'),
    ('Mesopotamia'),
    ('Port Elizabeth')
) AS v(name) ON c.code = 'VC'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Samoa (WS)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Apia'),
    ('Asau'),
    ('Mulifanua'),
    ('Safotu'),
    ('Salelologa')
) AS v(name) ON c.code = 'WS'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- San Marino (SM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Acquaviva'),
    ('Borgo Maggiore'),
    ('Chiesanuova'),
    ('Domagnano'),
    ('Faetano'),
    ('Fiorentino'),
    ('Montegiardino'),
    ('San Marino'),
    ('Serravalle')
) AS v(name) ON c.code = 'SM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Sao Tome and Principe (ST)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Neves'),
    ('Santana'),
    ('Santo António'),
    ('São Tomé'),
    ('Trinidade')
) AS v(name) ON c.code = 'ST'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Saudi Arabia (SA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Abha'),
    ('Al Hufuf'),
    ('Al Jubayl'),
    ('Al Khobar'),
    ('Al Qatif'),
    ('Buraydah'),
    ('Dammam'),
    ('Hail'),
    ('Jeddah'),
    ('Jizan'),
    ('Khamis Mushait'),
    ('Mecca'),
    ('Medina'),
    ('Najran'),
    ('Riyadh'),
    ('Sakaka'),
    ('Tabuk'),
    ('Taif'),
    ('Yanbu')
) AS v(name) ON c.code = 'SA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Senegal (SN)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Dakar'),
    ('Diourbel'),
    ('Fatick'),
    ('Kaolack'),
    ('Kolda'),
    ('Louga'),
    ('Matam'),
    ('Mbour'),
    ('Richard-Toll'),
    ('Saint-Louis'),
    ('Tambacounda'),
    ('Thiès'),
    ('Touba'),
    ('Ziguinchor')
) AS v(name) ON c.code = 'SN'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Serbia (RS)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Belgrade'),
    ('Čačak'),
    ('Kragujevac'),
    ('Kruševac'),
    ('Leskovac'),
    ('Niš'),
    ('Novi Pazar'),
    ('Novi Sad'),
    ('Pančevo'),
    ('Pirot'),
    ('Požarevac'),
    ('Priština'),
    ('Smederevo'),
    ('Šabac'),
    ('Subotica'),
    ('Užice'),
    ('Valjevo'),
    ('Vranje'),
    ('Zaječar'),
    ('Zrenjanin')
) AS v(name) ON c.code = 'RS'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Seychelles (SC)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Anse Boileau'),
    ('Beau Vallon'),
    ('Bel Ombre'),
    ('Cascade'),
    ('Glacis'),
    ('Grand Anse'),
    ('Port Glaud'),
    ('Takamaka'),
    ('Victoria')
) AS v(name) ON c.code = 'SC'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Sierra Leone (SL)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bo'),
    ('Bonthe'),
    ('Freetown'),
    ('Kabala'),
    ('Kenema'),
    ('Koidu'),
    ('Lunsar'),
    ('Magburaka'),
    ('Makeni'),
    ('Port Loko'),
    ('Waterloo')
) AS v(name) ON c.code = 'SL'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Singapore (SG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Singapore')
) AS v(name) ON c.code = 'SG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Slovakia (SK)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Banská Bystrica'),
    ('Bratislava'),
    ('Košice'),
    ('Martin'),
    ('Nitra'),
    ('Poprad'),
    ('Prešov'),
    ('Ružomberok'),
    ('Trenčín'),
    ('Trnava'),
    ('Žilina'),
    ('Zvolen')
) AS v(name) ON c.code = 'SK'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Slovenia (SI)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Celje'),
    ('Koper'),
    ('Kranj'),
    ('Ljubljana'),
    ('Maribor'),
    ('Murska Sobota'),
    ('Nova Gorica'),
    ('Novo Mesto'),
    ('Ptuj'),
    ('Velenje')
) AS v(name) ON c.code = 'SI'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Solomon Islands (SB)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Auki'),
    ('Gizo'),
    ('Honiara'),
    ('Kirakira'),
    ('Lata'),
    ('Tulagi')
) AS v(name) ON c.code = 'SB'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Somalia (SO)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Baidoa'),
    ('Balcad'),
    ('Berbera'),
    ('Bosaso'),
    ('Burao'),
    ('Galkayo'),
    ('Hargeysa'),
    ('Jawhar'),
    ('Kismayo'),
    ('Luuq'),
    ('Marka'),
    ('Mogadishu')
) AS v(name) ON c.code = 'SO'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- South Africa (ZA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Benoni'),
    ('Bloemfontein'),
    ('Cape Town'),
    ('Durban'),
    ('East London'),
    ('Ekurhuleni'),
    ('George'),
    ('Johannesburg'),
    ('Kimberley'),
    ('Krugersdorp'),
    ('Mangaung'),
    ('Midrand'),
    ('Nelspruit'),
    ('Pietermaritzburg'),
    ('Polokwane'),
    ('Port Elizabeth'),
    ('Pretoria'),
    ('Rustenburg'),
    ('Soweto'),
    ('Stellenbosch'),
    ('Tembisa'),
    ('Welkom')
) AS v(name) ON c.code = 'ZA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- South Korea (KR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ansan'),
    ('Anyang'),
    ('Bucheon'),
    ('Busan'),
    ('Changwon'),
    ('Cheongju'),
    ('Chuncheon'),
    ('Daegu'),
    ('Daejeon'),
    ('Gangneung'),
    ('Gimhae'),
    ('Goyang'),
    ('Gwangju'),
    ('Hwaseong'),
    ('Incheon'),
    ('Jeonju'),
    ('Masan'),
    ('Pohang'),
    ('Seongnam'),
    ('Seoul'),
    ('Suwon'),
    ('Ulsan'),
    ('Usan'),
    ('Wonju'),
    ('Yongin')
) AS v(name) ON c.code = 'KR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- South Sudan (SS)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aweil'),
    ('Bentiu'),
    ('Bor'),
    ('Juba'),
    ('Malakal'),
    ('Rumbek'),
    ('Torit'),
    ('Wau'),
    ('Yambio'),
    ('Yei')
) AS v(name) ON c.code = 'SS'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Spain (ES)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Alcalá de Henares'),
    ('Alcorcón'),
    ('Alicante'),
    ('Almería'),
    ('Badajoz'),
    ('Badalona'),
    ('Barcelona'),
    ('Bilbao'),
    ('Burgos'),
    ('Cartagena'),
    ('Castellón de la Plana'),
    ('Córdoba'),
    ('Elche'),
    ('Getafe'),
    ('Gijón'),
    ('Granada'),
    ('Hospitalet de Llobregat'),
    ('Huelva'),
    ('Jaén'),
    ('Jerez de la Frontera'),
    ('La Coruña'),
    ('Las Palmas de Gran Canaria'),
    ('Leganés'),
    ('León'),
    ('Logroño'),
    ('Madrid'),
    ('Málaga'),
    ('Móstoles'),
    ('Murcia'),
    ('Oviedo'),
    ('Palma'),
    ('Pamplona'),
    ('Sabadell'),
    ('Salamanca'),
    ('San Sebastián'),
    ('Santa Cruz de Tenerife'),
    ('Santander'),
    ('Seville'),
    ('Tarragona'),
    ('Terrassa'),
    ('Toledo'),
    ('Valencia'),
    ('Valladolid'),
    ('Vigo'),
    ('Vitoria-Gasteiz'),
    ('Zaragoza')
) AS v(name) ON c.code = 'ES'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Sri Lanka (LK)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Anuradhapura'),
    ('Badulla'),
    ('Batticaloa'),
    ('Colombo'),
    ('Galle'),
    ('Jaffna'),
    ('Kandy'),
    ('Kurunegala'),
    ('Matara'),
    ('Moratuwa'),
    ('Negombo'),
    ('Nuwara Eliya'),
    ('Ratnapura'),
    ('Sri Jayawardenepura Kotte'),
    ('Trincomalee')
) AS v(name) ON c.code = 'LK'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Sudan (SD)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Al Fashir'),
    ('Al Qadarif'),
    ('Atbara'),
    ('Geneina'),
    ('Kassala'),
    ('Khartoum'),
    ('Kosti'),
    ('Nyala'),
    ('Omdurman'),
    ('Port Sudan'),
    ('Sennar'),
    ('Wad Madani')
) AS v(name) ON c.code = 'SD'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Suriname (SR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Albina'),
    ('Lelydorp'),
    ('Moengo'),
    ('Nieuw Nickerie'),
    ('Paramaribo'),
    ('Wanica')
) AS v(name) ON c.code = 'SR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Sweden (SE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Borås'),
    ('Eskilstuna'),
    ('Gävle'),
    ('Gothenburg'),
    ('Helsingborg'),
    ('Jönköping'),
    ('Karlstad'),
    ('Linköping'),
    ('Luleå'),
    ('Lund'),
    ('Malmö'),
    ('Norrköping'),
    ('Örebro'),
    ('Stockholm'),
    ('Södertälje'),
    ('Umeå'),
    ('Uppsala'),
    ('Västerås'),
    ('Växjö')
) AS v(name) ON c.code = 'SE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Switzerland (CH)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Basel'),
    ('Bern'),
    ('Biel/Bienne'),
    ('Fribourg'),
    ('Geneva'),
    ('Lausanne'),
    ('Lucerne'),
    ('Lugano'),
    ('Schaffhausen'),
    ('Sion'),
    ('St. Gallen'),
    ('Thun'),
    ('Winterthur'),
    ('Zurich')
) AS v(name) ON c.code = 'CH'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Syria (SY)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Al Bab'),
    ('Al Hasakah'),
    ('Al Qamishli'),
    ('Aleppo'),
    ('Ar Raqqah'),
    ('Baniyas'),
    ('Damascus'),
    ('Dar''a'),
    ('Dayr az Zawr'),
    ('Hama'),
    ('Homs'),
    ('Idlib'),
    ('Latakia'),
    ('Manbij'),
    ('Tartus')
) AS v(name) ON c.code = 'SY'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Taiwan (TW)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Banqiao'),
    ('Changhua'),
    ('Chiayi'),
    ('Hsinchu'),
    ('Hualien'),
    ('Jilung'),
    ('Kaohsiung'),
    ('Pingtung'),
    ('Taichung'),
    ('Tainan'),
    ('Taipei'),
    ('Taitung'),
    ('Taoyuan'),
    ('Xinzhuang'),
    ('Yilan'),
    ('Zhongli')
) AS v(name) ON c.code = 'TW'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Tajikistan (TJ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Buston'),
    ('Danghara'),
    ('Dushanbe'),
    ('Hisor'),
    ('Isfara'),
    ('Istaravshan'),
    ('Khujand'),
    ('Kulob'),
    ('Norak'),
    ('Panjakent'),
    ('Qurghonteppa'),
    ('Tursunzoda')
) AS v(name) ON c.code = 'TJ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Tanzania (TZ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Arusha'),
    ('Bukoba'),
    ('Dar es Salaam'),
    ('Dodoma'),
    ('Iringa'),
    ('Kigoma'),
    ('Lindi'),
    ('Mbeya'),
    ('Morogoro'),
    ('Moshi'),
    ('Mtwara'),
    ('Musoma'),
    ('Mwanza'),
    ('Shinyanga'),
    ('Singida'),
    ('Sumbawanga'),
    ('Tabora'),
    ('Tanga'),
    ('Zanzibar City')
) AS v(name) ON c.code = 'TZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Thailand (TH)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bangkok'),
    ('Chiang Mai'),
    ('Chiang Rai'),
    ('Chonburi'),
    ('Hat Yai'),
    ('Khon Kaen'),
    ('Lampang'),
    ('Nakhon Ratchasima'),
    ('Nakhon Sawan'),
    ('Nakhon Si Thammarat'),
    ('Nonthaburi'),
    ('Pak Kret'),
    ('Pattaya'),
    ('Phitsanulok'),
    ('Phuket'),
    ('Samut Prakan'),
    ('Songkhla'),
    ('Surat Thani'),
    ('Ubon Ratchathani'),
    ('Udon Thani')
) AS v(name) ON c.code = 'TH'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Timor-Leste (TL)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aileu'),
    ('Ainaro'),
    ('Baucau'),
    ('Dili'),
    ('Ermera'),
    ('Liquiçá'),
    ('Maliana'),
    ('Manatuto'),
    ('Same'),
    ('Suai'),
    ('Viqueque')
) AS v(name) ON c.code = 'TL'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Togo (TG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aného'),
    ('Atakpamé'),
    ('Bassar'),
    ('Dapaong'),
    ('Kara'),
    ('Lomé'),
    ('Notsé'),
    ('Sokodé'),
    ('Tsévié')
) AS v(name) ON c.code = 'TG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Tonga (TO)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Haveluloto'),
    ('Neiafu'),
    ('Nuku''alofa'),
    ('Pangai'),
    ('Vaini')
) AS v(name) ON c.code = 'TO'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Trinidad and Tobago (TT)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Arima'),
    ('Chaguanas'),
    ('Port of Spain'),
    ('San Fernando'),
    ('Scarborough'),
    ('Siparia'),
    ('Tunapuna')
) AS v(name) ON c.code = 'TT'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Tunisia (TN)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ariana'),
    ('Ben Arous'),
    ('Béja'),
    ('Bizerte'),
    ('Gabès'),
    ('Gafsa'),
    ('Jendouba'),
    ('Kairouan'),
    ('Kasserine'),
    ('Kebili'),
    ('La Manouba'),
    ('Médenine'),
    ('Monastir'),
    ('Nabeul'),
    ('Sfax'),
    ('Sidi Bouzid'),
    ('Siliana'),
    ('Sousse'),
    ('Tataouine'),
    ('Tozeur'),
    ('Tunis'),
    ('Zaghouan')
) AS v(name) ON c.code = 'TN'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Turkey (TR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Adana'),
    ('Ankara'),
    ('Antalya'),
    ('Balıkesir'),
    ('Bursa'),
    ('Denizli'),
    ('Diyarbakır'),
    ('Elazığ'),
    ('Erzurum'),
    ('Eskişehir'),
    ('Gaziantep'),
    ('Istanbul'),
    ('İzmir'),
    ('Kahramanmaraş'),
    ('Kayseri'),
    ('Kocaeli'),
    ('Konya'),
    ('Malatya'),
    ('Manisa'),
    ('Mersin'),
    ('Ordu'),
    ('Sakarya'),
    ('Samsun'),
    ('Şanlıurfa'),
    ('Sivas'),
    ('Tekirdağ'),
    ('Trabzon'),
    ('Van')
) AS v(name) ON c.code = 'TR'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Turkmenistan (TM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Ashgabat'),
    ('Balkanabat'),
    ('Bayramaly'),
    ('Daşoguz'),
    ('Mary'),
    ('Türkmenabat'),
    ('Türkmenbaşy')
) AS v(name) ON c.code = 'TM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Tuvalu (TV)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Funafuti'),
    ('Savave'),
    ('Tanrake'),
    ('Toga'),
    ('Tumaseu')
) AS v(name) ON c.code = 'TV'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Uganda (UG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Arua'),
    ('Entebbe'),
    ('Fort Portal'),
    ('Gulu'),
    ('Jinja'),
    ('Kampala'),
    ('Kasese'),
    ('Lira'),
    ('Masaka'),
    ('Mbale'),
    ('Mbarara'),
    ('Soroti')
) AS v(name) ON c.code = 'UG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Ukraine (UA)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bila Tserkva'),
    ('Cherkasy'),
    ('Chernihiv'),
    ('Chernivtsi'),
    ('Dnipro'),
    ('Donetsk'),
    ('Ivano-Frankivsk'),
    ('Kharkiv'),
    ('Kherson'),
    ('Khmelnytskyi'),
    ('Kropyvnytskyi'),
    ('Kremenchuk'),
    ('Kyiv'),
    ('Luhansk'),
    ('Lutsk'),
    ('Lviv'),
    ('Mariupol'),
    ('Mykolaiv'),
    ('Odessa'),
    ('Poltava'),
    ('Rivne'),
    ('Sumy'),
    ('Ternopil'),
    ('Uzhhorod'),
    ('Vinnytsia'),
    ('Zaporizhzhia'),
    ('Zhytomyr')
) AS v(name) ON c.code = 'UA'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- United Arab Emirates (AE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Abu Dhabi'),
    ('Ajman'),
    ('Al Ain'),
    ('Dubai'),
    ('Fujairah'),
    ('Ras al-Khaimah'),
    ('Sharjah'),
    ('Umm al-Quwain')
) AS v(name) ON c.code = 'AE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- United Kingdom (GB)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aberdeen'),
    ('Bath'),
    ('Birmingham'),
    ('Bradford'),
    ('Brighton'),
    ('Bristol'),
    ('Cambridge'),
    ('Canterbury'),
    ('Cardiff'),
    ('Carlisle'),
    ('Chelmsford'),
    ('Chester'),
    ('Chichester'),
    ('Coventry'),
    ('Derby'),
    ('Durham'),
    ('Edinburgh'),
    ('Ely'),
    ('Exeter'),
    ('Glasgow'),
    ('Gloucester'),
    ('Hereford'),
    ('Kingston upon Hull'),
    ('Lancaster'),
    ('Leeds'),
    ('Leicester'),
    ('Lichfield'),
    ('Lincoln'),
    ('Liverpool'),
    ('London'),
    ('Manchester'),
    ('Newcastle upon Tyne'),
    ('Norwich'),
    ('Nottingham'),
    ('Oxford'),
    ('Peterborough'),
    ('Plymouth'),
    ('Portsmouth'),
    ('Preston'),
    ('Ripon'),
    ('Salford'),
    ('Salisbury'),
    ('Sheffield'),
    ('Southampton'),
    ('Stoke-on-Trent'),
    ('Sunderland'),
    ('Truro'),
    ('Wakefield'),
    ('Wells'),
    ('Westminster'),
    ('Winchester'),
    ('Wolverhampton'),
    ('Worcester'),
    ('York')
) AS v(name) ON c.code = 'GB'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- United States (US)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Albuquerque'),
    ('Anchorage'),
    ('Atlanta'),
    ('Austin'),
    ('Baltimore'),
    ('Boston'),
    ('Charlotte'),
    ('Chicago'),
    ('Cleveland'),
    ('Colorado Springs'),
    ('Columbus'),
    ('Dallas'),
    ('Denver'),
    ('Detroit'),
    ('El Paso'),
    ('Fort Worth'),
    ('Fresno'),
    ('Honolulu'),
    ('Houston'),
    ('Indianapolis'),
    ('Jacksonville'),
    ('Kansas City'),
    ('Las Vegas'),
    ('Long Beach'),
    ('Los Angeles'),
    ('Louisville'),
    ('Memphis'),
    ('Mesa'),
    ('Miami'),
    ('Milwaukee'),
    ('Minneapolis'),
    ('Nashville'),
    ('New Orleans'),
    ('New York City'),
    ('Oakland'),
    ('Oklahoma City'),
    ('Omaha'),
    ('Philadelphia'),
    ('Phoenix'),
    ('Portland'),
    ('Raleigh'),
    ('Sacramento'),
    ('San Antonio'),
    ('San Diego'),
    ('San Francisco'),
    ('San Jose'),
    ('Seattle'),
    ('Tucson'),
    ('Tulsa'),
    ('Virginia Beach'),
    ('Washington')
) AS v(name) ON c.code = 'US'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Uruguay (UY)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Artigas'),
    ('Canelones'),
    ('Ciudad de la Costa'),
    ('Colonia del Sacramento'),
    ('Durazno'),
    ('Florida'),
    ('Las Piedras'),
    ('Maldonado'),
    ('Mercedes'),
    ('Minas'),
    ('Montevideo'),
    ('Paysandú'),
    ('Rivera'),
    ('Rocha'),
    ('Salto'),
    ('San José de Mayo'),
    ('Tacuarembó'),
    ('Treinta y Tres')
) AS v(name) ON c.code = 'UY'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Uzbekistan (UZ)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Andijon'),
    ('Buxoro'),
    ('Chirchiq'),
    ('Farg''ona'),
    ('Guliston'),
    ('Jizzax'),
    ('Namangan'),
    ('Navoiy'),
    ('Nukus'),
    ('Qarshi'),
    ('Samarqand'),
    ('Tashkent'),
    ('Termiz'),
    ('Urganch')
) AS v(name) ON c.code = 'UZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Vanuatu (VU)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Isangel'),
    ('Lakatoro'),
    ('Luganville'),
    ('Port Vila'),
    ('Sola'),
    ('Xanur')
) AS v(name) ON c.code = 'VU'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Venezuela (VE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Barcelona'),
    ('Barinas'),
    ('Barquisimeto'),
    ('Cabimas'),
    ('Caracas'),
    ('Ciudad Bolívar'),
    ('Ciudad Guayana'),
    ('Coro'),
    ('Cumana'),
    ('Los Teques'),
    ('Maracaibo'),
    ('Maracay'),
    ('Maturín'),
    ('Mérida'),
    ('Petare'),
    ('Puerto La Cruz'),
    ('San Cristóbal'),
    ('San Fernando de Apure'),
    ('Turmero'),
    ('Valencia')
) AS v(name) ON c.code = 'VE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Vietnam (VN)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Biên Hòa'),
    ('Buôn Ma Thuột'),
    ('Cần Thơ'),
    ('Đà Lạt'),
    ('Đà Nẵng'),
    ('Hải Dương'),
    ('Hải Phòng'),
    ('Hạ Long'),
    ('Hanoi'),
    ('Ho Chi Minh City'),
    ('Huế'),
    ('Long Xuyên'),
    ('Mỹ Tho'),
    ('Nam Định'),
    ('Nha Trang'),
    ('Phủ Lý'),
    ('Phan Thiết'),
    ('Pleiku'),
    ('Quy Nhơn'),
    ('Rạch Giá'),
    ('Thái Nguyên'),
    ('Thanh Hóa'),
    ('Thủ Dầu Một'),
    ('Vinh'),
    ('Vũng Tàu')
) AS v(name) ON c.code = 'VN'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Yemen (YE)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Aden'),
    ('Al Hudaydah'),
    ('Al Mukalla'),
    ('Dhamar'),
    ('Ibb'),
    ('Marib'),
    ('Saada'),
    ('Sanaa'),
    ('Taiz'),
    ('Zinjibar')
) AS v(name) ON c.code = 'YE'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Zambia (ZM)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Chingola'),
    ('Chipata'),
    ('Kabwe'),
    ('Kafue'),
    ('Kasama'),
    ('Kitwe'),
    ('Livingstone'),
    ('Luanshya'),
    ('Lusaka'),
    ('Mongu'),
    ('Mufulira'),
    ('Ndola'),
    ('Solwezi')
) AS v(name) ON c.code = 'ZM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Zimbabwe (ZW)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Bindura'),
    ('Bulawayo'),
    ('Chinhoyi'),
    ('Chiredzi'),
    ('Gweru'),
    ('Harare'),
    ('Hwange'),
    ('Kadoma'),
    ('Kwekwe'),
    ('Marondera'),
    ('Masvingo'),
    ('Mutare'),
    ('Norton'),
    ('Rusape'),
    ('Victoria Falls')
) AS v(name) ON c.code = 'ZW'
ON CONFLICT (name, country_id) DO NOTHING;
