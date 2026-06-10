-- ------------------------------------------------------------
-- Afghanistan (AF)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
	('Aībak'),
	('Andkhōy'),
	('Āqchah'),
	('Asadābād'),
	('Baghlān'),
	('Bagrāmī'),
	('Bālā Kōh'),
	('Balkh'),
	('Bāmyān'),
	('Barakī'),
	('Barakī Barak'),
	('Bāzār-e Yakāwlang'),
	('Bāzārak'),
	('Chārīkār'),
	('Chīchkah'),
	('Deh-e Shū'),
	('Faīẕābād'),
	('Farāh'),
	('Fayrōz Kōh'),
	('Gardēz'),
	('Gereshk'),
	('Ghaznī'),
	('Ghōriyān'),
	('Gūdārah'),
	('Haskah Mēnah'),
	('Herāt'),
	('Ḩukūmatī Baghrān'),
	('Ḩukūmatī Gīzāb'),
	('Imām Şāḩib'),
	('Ishkāshim'),
	('Islām Qal‘ah'),
	('Jalālābād'),
	('Kabul'),
	('Kandahār'),
	('Karukh'),
	('Khānābād'),
	('Khōst'),
	('Khulm'),
	('Kōṯah-ye ‘As̲h̲rō'),
	('Kuhsān'),
	('Kunduz'),
	('Kushk'),
	('Lashkar Gāh'),
	('Maḩmūd-e Rāqī'),
	('Maīdān Shahr'),
	('Maīmanah'),
	('Māmā Khēl'),
	('Mazār-e Sharīf'),
	('Mehtar Lām'),
	('Nīlī'),
	('Paghmān'),
	('Panjāb'),
	('Pārūn'),
	('Pul-e ‘Alam'),
	('Pul-e Khumrī'),
	('Qal‘ah-ye Now'),
	('Qal‘ah-ye Zāl'),
	('Qalāt'),
	('Qarqīn'),
	('Sangīn'),
	('Sar-e Pul'),
	('Sharan'),
	('Sheghnān'),
	('Shibirghān'),
	('Spīn Bōldak'),
	('Tāluqān'),
	('Tarīn Kōṯ'),
	('Taywarah'),
	('Tujg'),
	('Urgūn'),
	('Zaṟah Sharan'),
	('Zaranj'),
	('Zarghūn Shahr')
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
	('Bërxull'),
	('Buçimas'),
	('Burrel'),
	('Çorovodë'),
	('Durrës'),
	('Elbasan'),
	('Ersekë'),
	('Fier'),
	('Fushë-Krujë'),
	('Gjirokastër'),
	('Gramsh'),
	('Kamëz'),
	('Kavajë'),
	('Korçë'),
	('Krujë'),
	('Kuçovë'),
	('Kukës'),
	('Laç'),
	('Lezhë'),
	('Libonik'),
	('Librazhd'),
	('Librazhd-Qendër'),
	('Lushnjë'),
	('Nikël'),
	('Patos'),
	('Përmet'),
	('Perondi'),
	('Peshkopi'),
	('Pogradec'),
	('Pukë'),
	('Rrëshen'),
	('Sarandë'),
	('Shijak'),
	('Shkodër'),
	('Shushicë'),
	('Sukth'),
	('Tepelenë'),
	('Tirana'),
	('Vlorë'),
	('Vorë'),
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
	('‘Aïn el Hadjel'),
	('’Aïn Abessa'),
	('’Aïn Abid'),
	('’Aïn Arnat'),
	('’Aïn Azel'),
	('’Aïn Babouche'),
	('’Aïn Boucif'),
	('’Aïn el Arbaa'),
	('’Aïn el Assel'),
	('’Aïn el Bell'),
	('’Aïn el Hammam'),
	('’Aïn el Melh'),
	('’Aïn el Turk'),
	('’Aïn Fekan'),
	('’Aïn Kerma'),
	('’Aïn Mabed'),
	('’Aïn Merane'),
	('’Aïn Mouilah'),
	('’Aïn Naga'),
	('’Aïn Roua'),
	('’Aïn Taghrout'),
	('’Aïn Tellout'),
	('’Aïn Tolba'),
	('Abadla'),
	('Abalessa'),
	('Abou el Hassan'),
	('Adekar Kebouche'),
	('Adrar'),
	('Afir'),
	('Aflou'),
	('Ahmed Rachedi'),
	('Ahmer el ’Aïn'),
	('Aïn Beïda'),
	('Aïn Bessem'),
	('Aïn Defla'),
	('Aïn el Bya'),
	('Aïn el Hadid'),
	('Aïn el Hadjar'),
	('Aïn Fakroun'),
	('Aïn Feka'),
	('Aïn Kechera'),
	('Aïn Kercha'),
	('Aïn Lechiakh'),
	('Aïn M’Lila'),
	('Aïn Nouissy'),
	('Aïn Oulmene'),
	('Aïn Oussera'),
	('Aïn Sefra'),
	('Aïn Smara'),
	('Aïn Taya'),
	('Aïn Tedeles'),
	('Aïn Temouchent'),
	('Aïn Touta'),
	('Aïn Youcef'),
	('Aïn Zaouïa'),
	('Aït Yaïch'),
	('Akabli'),
	('Akbou'),
	('Algiers'),
	('Amalou'),
	('Ammi Moussa'),
	('Amoucha'),
	('Annaba'),
	('Aomar'),
	('Aougrout'),
	('Arbaoun'),
	('Arbatache'),
	('Arhribs'),
	('Arris'),
	('Arzew'),
	('Asfour'),
	('Assi Bou Nif'),
	('Assi-Ben Okba'),
	('Azazga'),
	('Azeffoun'),
	('Azzaba'),
	('Bab Ezzouar'),
	('Baba Hassen'),
	('Babar'),
	('Baghlia'),
	('Bahmer'),
	('Baraki'),
	('Barbacha'),
	('Barika'),
	('Batna'),
	('Béchar'),
	('Bechloul'),
	('Bejaïa'),
	('Bekkaria'),
	('Bel Aïba'),
	('Bel Imour'),
	('Belkheir'),
	('Bellaa'),
	('Ben ’Aknoûn'),
	('Ben Chicao'),
	('Ben Daoud'),
	('Ben N’Choud'),
	('Ben Nasseur'),
	('Benairia'),
	('Benfreha'),
	('Beni Abbès'),
	('Beni Amrane'),
	('Beni Douala'),
	('Beni Fouda'),
	('Beni Haoua'),
	('Beni Mered'),
	('Beni Ounif'),
	('Beni Rached'),
	('Beni Saf'),
	('Beni Slimane'),
	('Beni Tamou'),
	('Bensekrane'),
	('Benyahia Abderrahmane'),
	('Berhoum'),
	('Berriane'),
	('Berriche'),
	('Berrouaghia'),
	('Bettioua'),
	('Bir Ben Laabed'),
	('Bir el Ater'),
	('Bir el Djir'),
	('Bir Ghbalou'),
	('Bir Kasdali'),
	('Birine'),
	('Birkhadem'),
	('Birtouta'),
	('Biskra'),
	('Blida'),
	('Blidet Amor'),
	('Boghni'),
	('Bordj Bou Arreridj'),
	('Bordj Bounaama'),
	('Bordj el Bahri'),
	('Bordj el Kiffan'),
	('Bordj Ghdir'),
	('Bordj Menaïel'),
	('Bordj Mokhtar'),
	('Bordj Okhriss'),
	('Bordj Zemoura'),
	('Boû Arfa'),
	('Bou Hadjar'),
	('Bou Hamza'),
	('Bou Hanifia el Hamamat'),
	('Bou Khadra'),
	('Bou Nouh'),
	('Bou Noura'),
	('Bou Saada'),
	('Bou Sfer'),
	('Bouaiche'),
	('Bouati Mahmoud'),
	('Bouchagroun'),
	('Bouchegouf'),
	('Boudjima'),
	('Boudouaou'),
	('Boudouaou el Bahri'),
	('Boufarik'),
	('Boufatis'),
	('Bougaa'),
	('Bougara'),
	('Bougtob'),
	('Bouguirat'),
	('Bougzoul'),
	('Bouhmama'),
	('Bouira'),
	('Bouira el Adebad'),
	('Boukadir'),
	('Boukhralfa'),
	('Boumahra Ahmed'),
	('Boumerdes'),
	('Bourkika'),
	('Bouskene'),
	('Bouzeghaia'),
	('Bouzina'),
	('Chaabat el Leham'),
	('Chabet el Ameur'),
	('Chahana'),
	('Chahbounia'),
	('Charef'),
	('Charouine'),
	('Chebli'),
	('Chekfa'),
	('Chelghoum el Aïd'),
	('Chellalat el Adhaouara'),
	('Chemini'),
	('Cherchell'),
	('Cheria'),
	('Chetma'),
	('Chetouane'),
	('Chlef'),
	('Chorfa'),
	('Collo'),
	('Constantine'),
	('Damous'),
	('Dar Chioukh'),
	('Dar el Beïda'),
	('Debbache el Hadj Douadi'),
	('Debila'),
	('Dellys'),
	('Dhalaa'),
	('Didouche Mourad'),
	('Djamaa'),
	('Djanet'),
	('Djebahia'),
	('Djelfa'),
	('Djemmorah'),
	('Djendel'),
	('Djinet'),
	('Djouab'),
	('Douar Bou Tlelis'),
	('Douera'),
	('Draa Ben Khedda'),
	('Draa el Mizan'),
	('Drean'),
	('Ebn Ziad'),
	('El Abadia'),
	('El Abiodh Sidi Cheikh'),
	('El Ach'),
	('El Achir'),
	('El Adjiba'),
	('El Affroun'),
	('El Amria'),
	('El Ancer'),
	('El Ançor'),
	('El Aouana'),
	('El Arrouch'),
	('El Ateuf'),
	('El Attaf'),
	('El Bayadh'),
	('El Bordj'),
	('El Esnam'),
	('El Eulma'),
	('El Ghomri'),
	('El Golea'),
	('El Hachimia'),
	('El Hadjar'),
	('El Hadjira'),
	('El Hamel'),
	('El Hammadia'),
	('El Idrissia'),
	('El Karimia'),
	('El Kerma'),
	('El Khroub'),
	('El Kouif'),
	('El Kseur'),
	('El Malah'),
	('El Marsa'),
	('El Marsa'),
	('El Meghaïer'),
	('El Milia'),
	('El Omaria'),
	('El Oued'),
	('El Outaya'),
	('El Tarf'),
	('Emîr Abdelkader'),
	('Es Sebt'),
	('Es Senia'),
	('Feidh el Botma'),
	('Fenoughil'),
	('Fornaka'),
	('Foughala'),
	('Fouka'),
	('Freha'),
	('Frenda'),
	('Froha'),
	('Galbois'),
	('Ghardaïa'),
	('Ghazaouet'),
	('Ghriss'),
	('Guelma'),
	('Gueltat Sidi Saad'),
	('Guemar'),
	('Guerouma'),
	('Hacine'),
	('Had Sahary'),
	('Hadjadj'),
	('Hadjout'),
	('Hamala'),
	('Hamma Bouziane'),
	('Hammam Bou Hadjar'),
	('Hammam Dalaa'),
	('Hammam M’Baïls'),
	('Harchoune'),
	('Hassi Bahbah'),
	('Hassi el Ghella'),
	('Hassi Fedoul'),
	('Hassi Khelifa'),
	('Hassi Maameche'),
	('Hassi Messaoud'),
	('Hennaya'),
	('Herenfa'),
	('I-n-Amenas'),
	('I-n-Amguel'),
	('I-n-Salah'),
	('Iferhounene'),
	('Ifigha'),
	('Iflissen'),
	('Ighram'),
	('Illiltene'),
	('Illizi'),
	('In Guezzam'),
	('Isser'),
	('Jean-Mermoz'),
	('Jijel'),
	('Kadiria'),
	('Kaïs'),
	('Kaous'),
	('Kenadsa'),
	('Khadra'),
	('Kheïredine'),
	('Khelil'),
	('Khemis el Khechna'),
	('Khemis Miliana'),
	('Khenchela'),
	('Kherrata'),
	('Kolea'),
	('Kouinine'),
	('Ksar Belezma'),
	('Ksar Chellala'),
	('Ksar el Boukhari'),
	('Ksar el Hirane'),
	('Ksar Sbahi'),
	('L’Arbaa Naït Irathen'),
	('Labiod Medjadja'),
	('Lac des Oiseaux'),
	('Laghouat'),
	('Lakhdaria'),
	('Lichana'),
	('Lioua'),
	('M’Chedallah'),
	('M’Chouneche'),
	('M’Sila'),
	('Maghnia'),
	('Magra'),
	('Makouda'),
	('Mansoûra'),
	('Mansourah'),
	('Mansourah'),
	('Maoussa'),
	('Mascara'),
	('Mazagran'),
	('Mecheraa Asfa'),
	('Mecheria'),
	('Mechta Ouled Oulha'),
	('Mechtras'),
	('Médéa'),
	('Medjana'),
	('Medjedel'),
	('Medrissa'),
	('Medroussa'),
	('Meftah'),
	('Megarine'),
	('Mehdia'),
	('Mekla'),
	('Melouza'),
	('Menaa'),
	('Menaceur'),
	('Merad'),
	('Merouana'),
	('Mers el Hadjad'),
	('Mers el Kebir'),
	('Meskiana'),
	('Mesra'),
	('Messaad'),
	('Metlili Chaamba'),
	('Mila'),
	('Miliana'),
	('Misserghin'),
	('Mnagueur'),
	('Mohammadia'),
	('Morsott'),
	('Mostaganem'),
	('Moudjbara'),
	('Mouiat Ouennsa'),
	('Mouzaïa'),
	('N’Gaous'),
	('N’Goussa'),
	('Naama'),
	('Naciria'),
	('Nakhla'),
	('Nechmeya'),
	('Nedroma'),
	('Negrine'),
	('Oggaz'),
	('Oran'),
	('Ouadhia'),
	('Ouamri'),
	('Ouargla'),
	('Oued Athmenia'),
	('Oued Cheham'),
	('Oued el Abtal'),
	('Oued el Alleug'),
	('Oued el Aneb'),
	('Oued el Djemaa'),
	('Oued el Kheïr'),
	('Oued el Ma'),
	('Oued Essalem'),
	('Oued Fodda'),
	('Oued Rhiou'),
	('Oued Sebbah'),
	('Oued Seguin'),
	('Oued Sly'),
	('Oued Taria'),
	('Oued Tlélat'),
	('Oued Zenati'),
	('Ouenza'),
	('Oulad Dahmane'),
	('Oulad Yaïch'),
	('Ouled Abbes'),
	('Ouled Ben Abd el Kader'),
	('Ouled Beni Messous'),
	('Ouled Brahim'),
	('Ouled Chebel'),
	('Ouled Djellal'),
	('Ouled Fares'),
	('Ouled Fayet'),
	('Ouled Haddaj'),
	('Ouled Mimoun'),
	('Ouled Moussa'),
	('Ouled Rabah'),
	('Ouled Rached'),
	('Ouled Rahmoun'),
	('Ouled Rahou'),
	('Ouled Sidi Brahim'),
	('Ouled Slama Tahta'),
	('Oum Drou'),
	('Oum el Bouaghi'),
	('Ouzera'),
	('Rabta'),
	('Râs el Ma'),
	('Râs el Oued'),
	('Rechaïga'),
	('Reggane'),
	('Reghaïa'),
	('Reguiba'),
	('Relizane'),
	('Remchi'),
	('Robbah'),
	('Rouached'),
	('Rouiba'),
	('Rouissat'),
	('Sabra'),
	('Saf Saf'),
	('Saïda'),
	('Sali'),
	('Saoula'),
	('Sayada'),
	('Sebdou'),
	('Seddouk Oufella'),
	('Selmana'),
	('Selmane'),
	('Sendjas'),
	('Sétif'),
	('Settara'),
	('Sfizef'),
	('Si Mustapha'),
	('Sidi Abd el Moumene'),
	('Sidi Abdelaziz'),
	('Sidi Aïssa'),
	('Sidi Akkacha'),
	('Sidi Ali'),
	('Sidi Amrane'),
	('Sidi Aoun'),
	('Sidi Baizid'),
	('Sidi Bel Abbès'),
	('Sidi Ben Adda'),
	('Sidi Brahim'),
	('Sidi Daoud'),
	('Sidi ech Chahmi'),
	('Sidi Embarek'),
	('Sidi Ghiles'),
	('Sidi Kada'),
	('Sidi Khaled'),
	('Sidi Ladjel'),
	('Sidi Lahssen'),
	('Sidi Lakhdar'),
	('Sidi Lakhdar'),
	('Sidi Makhlouf'),
	('Sidi Merouane'),
	('Sidi Moussa'),
	('Sidi Namane'),
	('Sidi Okba'),
	('Sig'),
	('Sigus'),
	('Sirat'),
	('Skikda'),
	('Souaflia'),
	('Souagui'),
	('Souahlia'),
	('Souama'),
	('Sougueur'),
	('Souk Ahras'),
	('Souma'),
	('Sour'),
	('Sour el Ghozlane'),
	('Staoueli'),
	('Stidia'),
	('Tablat'),
	('Tacheta Zougagha'),
	('Tadjenanet'),
	('Tadjmout'),
	('Tadmaït'),
	('Tafaraoui'),
	('Taher'),
	('Taïbet'),
	('Takerbouzt'),
	('Takhemaret'),
	('Tala Yfassene'),
	('Tamalous'),
	('Tamanrasset'),
	('Tamentit'),
	('Tamza'),
	('Tamzoura'),
	('Taougrite'),
	('Tarhzout'),
	('Tarmount'),
	('Tassera'),
	('Tazmalt'),
	('Tazoult-Lambese'),
	('Tebesbest'),
	('Tébessa'),
	('Télagh'),
	('Telerghma'),
	('Temacine'),
	('Tenedla'),
	('Ténès'),
	('Teniet el Abed'),
	('Terga'),
	('Texenna'),
	('Thenia'),
	('Theniet el Had'),
	('Tiaret'),
	('Tichi'),
	('Tidjelabine'),
	('Tifra'),
	('Tighenif'),
	('Tigzirt'),
	('Timimoun'),
	('Timizart'),
	('Timoktene'),
	('Tindouf'),
	('Tipasa'),
	('Tirmitine'),
	('Tissemsilt'),
	('Tixter'),
	('Tizi'),
	('Tizi Gheniff'),
	('Tizi Ouzou'),
	('Tizi Rached'),
	('Tizi-n-Bechar'),
	('Tizi-n-Tleta'),
	('Tlemcen'),
	('Toudja'),
	('Touggourt'),
	('Tsabit'),
	('Yakouren'),
	('Zahana'),
	('Zaouiet Kounta'),
	('Zeboudja'),
	('Zelfana'),
	('Zemmouri'),
	('Zemoura'),
	('Zeralda'),
	('Zérizer'),
	('Ziama Mansouria'),
	('Zighout Youcef'),
	('Zoubiria')
) AS v(name) ON c.code = 'DZ'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- American Samoa (AS)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Pago Pago')
) AS v(name) ON c.code = 'AS'
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
    ('Sant Julia de Loria'),
	('Sant Pere')
) AS v(name) ON c.code = 'AD'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Angola (AO)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Alto-Cuilo'),
    ('Ambiula'),
    ('Ambriz'),
    ('Andulo'),
    ('Baía Farta'),
    ('Bala Cangamba'),
    ('Balombo'),
    ('Banga'),
    ('Barra do Dande'),
    ('Belas'),
    ('Belize'),
    ('Bembe'),
    ('Benfica'),
    ('Benguela'),
    ('Bibala'),
    ('Bocoio'),
    ('Bolongongo'),
    ('Buco Zau'),
    ('Bula Atumba'),
    ('Caála'),
    ('Cabinda'),
    ('Cacolo'),
    ('Caconda'),
    ('Cacongo'),
    ('Cacuaco'),
    ('Cacula'),
    ('Cacuso'),
    ('Cahama'),
    ('Caimbambo'),
    ('Caiundo'),
    ('Calandala'),
    ('Calenga'),
    ('Calulo'),
    ('Caluquembe'),
    ('Camabatela'),
    ('Camacupa'),
    ('Camanongue'),
    ('Cambambe'),
    ('Cambulo'),
    ('Cambundi Catembo'),
    ('Cangandala'),
    ('Cangola'),
    ('Caombo'),
    ('Capenda Camulemba'),
    ('Cassongue'),
    ('Catabola'),
    ('Catape'),
    ('Catchiungo'),
    ('Catete'),
    ('Catumbela'),
    ('Caungula'),
    ('Caxito'),
    ('Cazanga'),
    ('Cazenga'),
    ('Cazombo'),
	('Chibemba'),
	('Chibia'),
	('Chicomba'),
	('Chinguar'),
	('Chipindo'),
	('Chitembo'),
	('Chitemo'),
	('Chongoroi'),
	('Conda'),
	('Cuango'),
	('Cubal'),
	('Cuchi'),
	('Cuemba'),
	('Cuilo'),
	('Cuímba'),
	('Cuito'),
	('Cuito Cuanavale'),
	('Cunda diá Baze'),
	('Cunhinga'),
	('Cuvelai'),
	('Dala'),
	('Damba'),
	('Dondo'),
	('Dundo'),
	('Ebo'),
	('Gabela'),
	('Ganda'),
	('Golungo Alto'),
	('Huambo'),
	('Humpata'),
	('Kuvango'),
	('Lobito'),
	('Londuimbali'),
	('Longa'),
	('Longonjo'),
	('Lóvua'),
	('Luacano'),
	('Luanda'),
	('Luau'),
	('Lubalo'),
	('Lubango'),
	('Lucala'),
	('Lucapa'),
	('Luena'),
	('Luquembo'),
	('Malanje'),
	('Maquela do Zombo'),
	('Marimba'),
	('Massango'),
	('Matala'),
	('Mavinga'),
	('Mbanza Kongo'),
	('Menongue'),
	('Moçâmedes'),
	('Mucaba'),
	('Mucari'),
	('Muconda'),
	('Mungo'),
	('Mussende'),
	('Namacunde'),
	('Nambuangongo'),
	('Ndalatando'),
	('Ndjamba'),
	('Ndulo'),
	('Negage'),
	('Nóqui'),
	('Nova Esperança'),
	('Nzeto'),
	('Ondjiva'),
	('Porto Alexandre'),
	('Porto Amboim'),
	('Puri'),
	('Quela'),
	('Quibala'),
	('Quibaxi'),
	('Quiculungo'),
	('Quilenda'),
	('Quilengues'),
	('Quilevo'),
	('Quimavango'),
	('Quimbele'),
	('Quipungo'),
	('Quirima'),
	('Samba Cango'),
	('Sanza Pombo'),
	('Saurimo'),
	('Soio'),
	('Songo'),
	('Sumbe'),
	('Sungo'),
	('Talatona'),
	('Tchindjendje'),
	('Tchitado'),
	('Tchitato'),
	('Tombôco'),
	('Uacu Cungo'),
	('Ucu Seles'),
	('Ucuma'),
	('Uíge'),
	('Vila Teixeira da Silva'),
	('Virei'),
	('Xá Muteba'),
	('Xangongo'),
) AS v(name) ON c.code = 'AO'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Anguilla (AI)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('The Valley')
) AS v(name) ON c.code = 'AI'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Antigua and Barbuda (AG)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
    ('Saint John’s')
) AS v(name) ON c.code = 'AG'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Argentina (AR)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
	('Abra Pampa'),
	('Acasusso'),
	('Adolfo Gonzáles Chaves'),
	('Adrogue'),
	('Aguaray'),
	('Aguilares'),
	('Alcorta'),
	('Aldo Bonzi'),
	('Alejandro Korn'),
	('Allen'),
	('Almafuerte'),
	('Alta Gracia'),
	('Alto Río Senguer'),
	('Alvear'),
	('Añatuya'),
	('Andalgalá'),
	('Apolinario Saravia'),
	('Apóstoles'),
	('Aristóbulo del Valle'),
	('Armstrong'),
	('Arrecifes'),
	('Arroyito'),
	('Arroyo Seco'),
	('Avellaneda'),
	('Ayacucho'),
	('Azul'),
	('Bahía Blanca'),
	('Balcarce'),
	('Balneario Monte Hermoso'),
	('Banda del Río Salí'),
	('Banfield'),
	('Baradero'),
	('Barranqueras'),
	('Basavilbaso'),
	('Batán'),
	('Beccar'),
	('Belén'),
	('Belén de Escobar'),
	('Bell Ville'),
	('Bella Vista'),
	('Benito Juárez'),
	('Berazategui'),
	('Berisso'),
	('Bernal'),
	('Bernardo de Irigoyen'),
	('Bialet Massé'),
	('Billinghurst'),
	('Bovril'),
	('Bragado'),
	('Brandsen'),
	('Brinkmann'),
	('Buenos Aires'),
	('Burzaco'),
	('Cafayate'),
	('Caleta Olivia'),
	('Calilegua'),
	('Campana'),
	('Campo Grande'),
	('Campo Largo'),
	('Campo Quijano'),
	('Campo Ramón'),
	('Campo Viera'),
	('Cañada de Gómez'),
	('Candelaria'),
	('Cañuelas'),
	('Capilla del Monte'),
	('Capilla del Señor'),
	('Capitán Bermúdez'),
	('Capitán Sarmiento'),
	('Carcarañá'),
	('Carhué'),
	('Carlos Casares'),
	('Carlos Spegazzini'),
	('Carlos Tejedor'),
	('Carmen de Areco'),
	('Carmen de Patagones'),
	('Caroya'),
	('Caseros'),
	('Casilda'),
	('Castelar'),
	('Castelli'),
	('Catamarca'),
	('Catriel'),
	('Caucete'),
	('Centenario'),
	('Ceres'),
	('Cerrillos'),
	('Cevil Redondo'),
	('Chacabuco'),
	('Chajarí'),
	('Chamical'),
	('Charata'),
	('Chascomús'),
	('Chepes'),
	('Chicoana'),
	('Chilecito'),
	('Chimbas'),
	('Chivilcoy'),
	('Choele Choel'),
	('Chos Malal'),
	('Chumbicha'),
	('Cinco Saltos'),
	('Cipolletti'),
	('City Bell'),
	('Ciudad de Loreto'),
	('Ciudad General Belgrano'),
	('Ciudadela'),
	('Claypole'),
	('Clorinda'),
	('Colón'),
	('Colonia Aurora'),
	('Comallo'),
	('Comandante Fontana'),
	('Comandante Luis Piedra Buena'),
	('Comodoro Rivadavia'),
	('Concepción'),
	('Concepción de la Sierra'),
	('Concepción del Uruguay'),
	('Concordia'),
	('Córdoba'),
	('Coronda'),
	('Coronel Dorrego'),
	('Coronel Du Graty'),
	('Coronel Juan Solá'),
	('Coronel Moldes'),
	('Coronel Suárez'),
	('Corral de Bustos'),
	('Corrientes'),
	('Corzuela'),
	('Cosquín'),
	('Crespo'),
	('Cruz del Eje'),
	('Curuzú Cuatiá'),
	('Cutral-Có'),
	('Daireaux'),
	('Darregueira'),
	('Deán Funes'),
	('Delfín Gallo'),
	('Diamante'),
	('Dock Sur'),
	('Dolores'),
	('Don Bosco'),
	('Don Torcuato'),
	('Dos de Mayo'),
	('El Bolsón'),
	('El Calafate'),
	('El Carmen'),
	('El Carril'),
	('El Chañar'),
	('El Colorado'),
	('El Galpón'),
	('El Maitén'),
	('El Palomar'),
	('El Quebrachal'),
	('El Soberbio'),
	('El Talar de Pacheco'),
	('El Trébol'),
	('Eldorado'),
	('Embalse'),
	('Embarcación'),
	('Empedrado'),
	('Ensenada'),
	('Ensenada Berisso'),
	('Esperanza'),
	('Esquel'),
	('Esquina'),
	('Esteban Echeverría'),
	('Ezpeleta'),
	('Famaillá'),
	('Federación'),
	('Federal'),
	('Fiambalá'),
	('Firmat'),
	('Florencio Varela'),
	('Florida'),
	('Fontana'),
	('Formosa'),
	('Fraile Pintado'),
	('Fray Luis A. Beltrán'),
	('Frías'),
	('Frontera'),
	('Funes'),
	('Gálvez'),
	('Garín'),
	('Garuhapé'),
	('Garupá'),
	('Gastre'),
	('General Acha'),
	('General Alvear'),
	('General Arenales'),
	('General Belgrano'),
	('General Cabrera'),
	('General Conesa'),
	('General Deheza'),
	('General Enrique Mosconi'),
	('General José de San Martín'),
	('General Juan Madariaga'),
	('General La Madrid'),
	('General Las Heras'),
	('General Martín Miguel de Güemes'),
	('General Pacheco'),
	('General Pico'),
	('General Pinedo'),
	('General Ramírez'),
	('General Roca'),
	('General Rodríguez'),
	('General San Martín'),
	('General Viamonte'),
	('General Villegas'),
	('Gerli'),
	('Glew'),
	('Gobernador Gálvez'),
	('Gobernador Gregores'),
	('Gobernador Virasora'),
	('Godoy Cruz'),
	('González Catán'),
	('Goya'),
	('Granadero Baigorria'),
	('Grand Bourg'),
	('Gualeguay'),
	('Gualeguaychú'),
	('Guernica'),
	('Haedo'),
	('Helvecia'),
	('Henderson'),
	('Hernando'),
	('Huillapima'),
	('Huinca Renancó'),
	('Humahuaca'),
	('Hurlingham'),
	('Ibarlucea'),
	('Ibicuy'),
	('Ingeniero Guillermo N. Juárez'),
	('Ingeniero Jacobacci'),
	('Ingeniero Maschwitz'),
	('Ingeniero Pablo Nogués'),
	('Ingeniero White'),
	('Isidro Casanova'),
	('Itatí'),
	('Ituzaingó'),
	('Ituzaingó'),
	('Jardín América'),
	('Jesús María'),
	('Joaquín V. González'),
	('José C. Paz'),
	('José María Ezeiza'),
	('José Mármol'),
	('Juan Bautista Alberdi'),
	('Junín'),
	('Junín de los Andes'),
	('Justiniano Posse'),
	('Justo Daract'),
	('Kaiken'),
	('La Banda'),
	('La Calera'),
	('La Carlota'),
	('La Cruz'),
	('La Falda'),
	('La Leonesa'),
	('La Lucila'),
	('La Merced'),
	('La Paz'),
	('La Plata'),
	('La Quiaca'),
	('La Reja'),
	('La Rioja'),
	('Laboulaye'),
	('Laguna Blanca'),
	('Laguna Paiva'),
	('Lanús'),
	('Las Breñas'),
	('Las Flores'),
	('Las Heras'),
	('Las Lajas'),
	('Las Lajitas'),
	('Las Lomitas'),
	('Las Rosas'),
	('Las Toscas'),
	('Las Varillas'),
	('Leandro N. Alem'),
	('Leones'),
	('Libertad'),
	('Libertador General San Martín'),
	('Lincoln'),
	('Llavallol'),
	('Lobería'),
	('Lobos'),
	('Lomas de Zamora'),
	('Lomas del Mirador'),
	('Longchamps'),
	('Los Altos'),
	('Los Blancos'),
	('Los Polvorines'),
	('Los Ralos'),
	('Luján'),
	('Luján de Cuyo'),
	('Machagai'),
	('Magdalena'),
	('Maipú'),
	('Malabrigo'),
	('Malagueño'),
	('Malargüe'),
	('Malvinas Argentinas'),
	('Manuel B. Gonnet'),
	('Mar de Ajó'),
	('Mar del Plata'),
	('Marcos Juárez'),
	('Marcos Paz'),
	('Mariano Acosta'),
	('Martín Coronado'),
	('Martínez'),
	('Matheu'),
	('Mburucuyá'),
	('Melchor Romero'),
	('Mendoza'),
	('Mercedes'),
	('Merlo'),
	('Mina Clavero'),
	('Miramar'),
	('Mocoretá'),
	('Monte Caseros'),
	('Monte Chingolo'),
	('Monte Cristo'),
	('Monte Quemado'),
	('Monte Rico'),
	('Monte Vera'),
	('Montecarlo'),
	('Monteros'),
	('Morón'),
	('Morteros'),
	('Muñiz'),
	('Munro'),
	('Navarro'),
	('Necochea'),
	('Neuquén'),
	('Nogoyá'),
	('Nuestra Señora del Rosario de Caa Catí'),
	('Nueve de Julio'),
	('Oberá'),
	('Olavarría'),
	('Oliva'),
	('Oncativo'),
	('Palpalá'),
	('Pampa del Indio'),
	('Pampa del Infierno'),
	('Paraná'),
	('Paso de la Patria'),
	('Paso de los Libres'),
	('Pedro Luro'),
	('Pehuajó'),
	('Pérez'),
	('Pergamino'),
	('Perico'),
	('Perito Moreno'),
	('Pichanal'),
	('Pico Truncado'),
	('Pigüé'),
	('Pilar'),
	('Pinamar'),
	('Pirané'),
	('Plaza Huincul'),
	('Plottier'),
	('Posadas'),
	('Presidencia de la Plaza'),
	('Presidencia Roque Sáenz Peña')
	('Profesor Salvador Mazza'),
	('Puan'),
	('Puerto Deseado'),
	('Puerto Esperanza'),
	('Puerto Ibicuy'),
	('Puerto Iguazú'),
	('Puerto Madryn'),
	('Puerto Piray'),
	('Puerto Rico'),
	('Puerto San Martín'),
	('Puerto Tirol'),
	('Puerto Vilelas'),
	('Punta Alta'),
	('Punta Indio'),
	('Quilmes'),
	('Quimilí'),
	('Quitilipi'),
	('Rada Tilly'),
	('Rafael Calzada'),
	('Rafael Castillo'),
	('Rafaela'),
	('Ramallo'),
	('Ramos Mejía'),
	('Ranchos'),
	('Rauch'),
	('Rawson'),
	('Reconquista'),
	('Recreo'),
	('Remedios de Escalada'),
	('Resistencia'),
	('Rinconada'),
	('Río Colorado'),
	('Río Cuarto'),
	('Río Gallegos'),
	('Río Grande'),
	('Río Mayo'),
	('Río Primero'),
	('Río Segundo'),
	('Río Tercero'),
	('Rivadavia'),
	('Rodeo'),
	('Roldán'),
	('Romang'),
	('Roque Pérez'),
	('Rosario'),
	('Rosario de la Frontera'),
	('Rosario de Lerma'),
	('Rosario del Tala'),
	('Rufino'),
	('Saavedra'),
	('Saladas'),
	('Salsipuedes'),
	('Salta'),
	('Sampacho'),
	('San Andrés de Giles'),
	('San Antonio'),
	('San Antonio de Areco'),
	('San Antonio de los Cobres'),
	('San Antonio de Padua'),
	('San Antonio Oeste'),
	('San Benito'),
	('San Bernardo'),
	('San Carlos Centro'),
	('San Carlos de Bariloche'),
	('San Cristóbal'),
	('San Fernando'),
	('San Francisco'),
	('San Francisco Solano'),
	('San Genaro'),
	('San Guillermo'),
	('San Isidro'),
	('San Isidro de Lules'),
	('San Javier'),
	('San Jorge'),
	('San José de Feliciano'),
	('San José de Jáchal'),
	('San José del Rincón'),
	('San Juan'),
	('San Julián'),
	('San Justo'),
	('San Lorenzo'),
	('San Luis'),
	('San Luis del Palmar'),
	('San Martín de los Andes'),
	('San Miguel'),
	('San Miguel de Tucumán'),
	('San Nicolás de los Arroyos'),
	('San Pedro'),
	('San Rafael'),
	('San Ramón de la Nueva Orán'),
	('San Roque'),
	('San Salvador'),
	('San Salvador de Jujuy'),
	('San Vicente'),
	('Santa Elena'),
	('Santa Fe'),
	('Santa Lucía'),
	('Santa María'),
	('Santa Rosa'),
	('Santa Rosa de Calamuchita'),
	('Santa Rosa de Río Primero'),
	('Santa Sylvina'),
	('Santa Victoria'),
	('Santiago del Estero'),
	('Santo Tomé'),
	('Sarandí'),
	('Sarmiento'),
	('Sauce'),
	('Sauce Viejo'),
	('Senillosa'),
	('Sierra Colorada'),
	('Sunchales'),
	('Susques'),
	('Taco Pozo'),
	('Tafí Viejo'),
	('Tandil'),
	('Tanti'),
	('Tapiales'),
	('Tartagal'),
	('Telsen'),
	('Temperley'),
	('Termas de Río Hondo'),
	('Tigre'),
	('Tinogasta'),
	('Tolosa'),
	('Tornquist'),
	('Tortuguitas'),
	('Tostado'),
	('Totoras'),
	('Trancas'),
	('Trelew'),
	('Trenque Lauquen'),
	('Tres Arroyos'),
	('Tres Isletas'),
	('Tres Lomas'),
	('Tunuyán'),
	('Unquillo'),
	('Urdinarrain'),
	('Ushuaia'),
	('Uspallata'),
	('Veinticinco de Mayo'),
	('Veintiocho de Noviembre'),
	('Venado Tuerto'),
	('Vera'),
	('Viale'),
	('Vicente López'),
	('Victoria'),
	('Victorica'),
	('Vicuña Mackenna'),
	('Viedma'),
	('Villa Aberastain'),
	('Villa Adelina'),
	('Villa Alsina'),
	('Villa Ángela'),
	('Villa Ballester'),
	('Villa Berthet'),
	('Villa Cañás'),
	('Villa Carlos Paz'),
	('Villa Celina'),
	('Villa Constitución'),
	('Villa de Mayo'),
	('Villa de Soto'),
	('Villa del Rosario'),
	('Villa del Totoral'),
	('Villa Dolores'),
	('Villa Domínico'),
	('Villa Elisa'),
	('Villa General Belgrano'),
	('Villa Gesell'),
	('Villa Krause'),
	('Villa La Angostura'),
	('Villa Luzuriaga'),
	('Villa María'),
	('Villa María Grande'),
	('Villa Mercedes'),
	('Villa Nougues'),
	('Villa Nueva'),
	('Villa Ocampo'),
	('Villa Ojo de Agua'),
	('Villa Paranacito'),
	('Villa Regina'),
	('Villa Rumipal'),
	('Villa San José'),
	('Villa Sarmiento'),
	('Villa Unión'),
	('Villaguay'),
	('Villalonga'),
	('Virreyes'),
	('Wanda'),
	('Wilde'),
	('Yacimiento Río Turbio'),
	('Yerba Buena'),
	('Yuto'),
	('Zapala'),
	('Zárate')
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
	('Akhuryan'),
	('Alaverdi'),
	('Armavir'),
	('Artashat'),
	('Artik'),
	('Ashtarak'),
	('Berd'),
	('Byureghavan'),
	('Charentsavan'),
	('Dilijan'),
	('Ejmiatsin'),
	('Gavarr'),
	('Goris'),
	('Gyumri'),
	('Hrazdan'),
	('Ijevan'),
	('Kapan'),
	('Martuni'),
	('Masis'),
	('Metsamor'),
	('Nerk’in Getashen'),
	('Nor Hachn'),
	('Sevan'),
	('Sisian'),
	('Spitak'),
	('Stepanavan'),
	('Tashir'),
	('Vanadzor'),
	('Vardenik'),
	('Vardenis'),
	('Vedi'),
	('Yeghegnadzor'),
	('Yeghvard'),
	('Yerevan')
) AS v(name) ON c.code = 'AM'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Aruba (AW)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
	('Oranjestad'),
	('Tanki Leendert')
) AS v(name) ON c.code = 'AW'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Australia (AU)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
	('Adelaide'),
	('Adelaide River'),
	('Albany'),
	('Albury'),
	('Alice Springs'),
	('Andamooka'),
	('Andergrove'),
	('Ararat'),
	('Armidale'),
	('Atherton'),
	('Australind'),
	('Ayr'),
	('Bairnsdale'),
	('Ballarat'),
	('Ballina'),
	('Banora Point'),
	('Barcaldine'),
	('Bargara'),
	('Barwon Heads'),
	('Batemans Bay'),
	('Bathurst'),
	('Bedourie'),
	('Benalla'),
	('Bendigo'),
	('Berri'),
	('Bicheno'),
	('Biloela'),
	('Birdsville'),
	('Bli Bli'),
	('Bongaree'),
	('Bordertown'),
	('Boulia'),
	('Bourke'),
	('Bowen'),
	('Bowral'),
	('Brisbane'),
	('Broken Hill'),
	('Broome'),
	('Buderim'),
	('Bunbury'),
	('Bundaberg'),
	('Burketown'),
	('Burnie'),
	('Burpengary'),
	('Busselton'),
	('Byron Bay'),
	('Caboolture'),
	('Cairns'),
	('Caloundra'),
	('Camooweal'),
	('Campbelltown'),
	('Canberra'),
	('Carnarvon'),
	('Casino'),
	('Ceduna'),
	('Central Coast'),
	('Cessnock'),
	('Charleville'),
	('Charters Towers'),
	('Clare'),
	('Cloncurry'),
	('Cobram'),
	('Coffs Harbour'),
	('Colac'),
	('Coolum Beach'),
	('Cooma'),
	('Coomera'),
	('Corinda'),
	('Cowell'),
	('Cowra'),
	('Cranbourne'),
	('Currumbin'),
	('Dalby'),
	('Dandenong'),
	('Darley'),
	('Darwin'),
	('Deniliquin'),
	('Devonport'),
	('Drouin'),
	('Dubbo'),
	('Echuca'),
	('Eidsvold'),
	('Emerald'),
	('Esperance'),
	('Exmouth'),
	('Flemington'),
	('Forbes'),
	('Forster'),
	('Frankston'),
	('Fremantle'),
	('Gawler'),
	('Geelong'),
	('Georgetown'),
	('Geraldton'),
	('Gingin'),
	('Gisborne'),
	('Gladstone'),
	('Gold Coast'),
	('Goolwa'),
	('Goondiwindi'),
	('Goonellabah'),
	('Goulburn'),
	('Gracemere'),
	('Grafton'),
	('Griffith'),
	('Gunnedah'),
	('Gympie'),
	('Halls Creek'),
	('Hamilton'),
	('Hervey Bay'),
	('Highfields'),
	('Hobart'),
	('Horsham'),
	('Hughenden'),
	('Ingham'),
	('Innisfail'),
	('Inverell'),
	('Ipswich'),
	('Ivanhoe'),
	('Jimboomba'),
	('Kalbarri'),
	('Kalgoorlie'),
	('Kangaroo Flat'),
	('Karratha'),
	('Karumba'),
	('Katanning'),
	('Katherine'),
	('Katoomba'),
	('Kelso'),
	('Kempsey'),
	('Kiama'),
	('Kilmore'),
	('Kimba'),
	('Kingaroy'),
	('Kingoonya'),
	('Kingston'),
	('Kingston South East'),
	('Kununurra'),
	('Kurri Kurri'),
	('Kwinana'),
	('Lara'),
	('Launceston'),
	('Laverton'),
	('Leeton'),
	('Leonora'),
	('Leopold'),
	('Lismore'),
	('Lithgow'),
	('Longreach'),
	('Mackay'),
	('Maitland'),
	('Mandurah'),
	('Manjimup'),
	('Mareeba'),
	('Margaret River'),
	('Maroochydore'),
	('Maryborough'),
	('McMinns Lagoon'),
	('Meekatharra'),
	('Melbourne'),
	('Melton'),
	('Meningie'),
	('Merimbula'),
	('Merredin'),
	('Mildura'),
	('Moe'),
	('Moranbah'),
	('Morawa'),
	('Moree'),
	('Morwell'),
	('Moss Vale'),
	('Mount Barker'),
	('Mount Eliza'),
	('Mount Evelyn'),
	('Mount Gambier'),
	('Mount Isa'),
	('Mount Magnet'),
	('Mudgee'),
	('Murray Bridge'),
	('Murwillumbah'),
	('Muswellbrook'),
	('Nambour'),
	('Narangba'),
	('Narrabri'),
	('Narrogin'),
	('Nerang'),
	('Newcastle'),
	('Newman'),
	('Norseman'),
	('Northam'),
	('Nowra'),
	('Oatlands'),
	('Onslow'),
	('Orange'),
	('Ormeau'),
	('Ouyen'),
	('Pakenham'),
	('Palmerston'),
	('Pannawonica'),
	('Parkes'),
	('Penola'),
	('Penrith'),
	('Perth'),
	('Peterborough'),
	('Pine Creek'),
	('Port Augusta'),
	('Port Denison'),
	('Port Douglas'),
	('Port Hedland'),
	('Port Lincoln'),
	('Port Macquarie'),
	('Port Pirie'),
	('Portarlington'),
	('Portland'),
	('Proserpine'),
	('Quakers Hill'),
	('Queanbeyan'),
	('Queenstown'),
	('Quilpie'),
	('Ravensthorpe'),
	('Raymond Terrace'),
	('Redlynch'),
	('Richmond'),
	('Rochedale'),
	('Rockhampton'),
	('Roebourne'),
	('Roma'),
	('Rutherford'),
	('Rye'),
	('Saint Leonards'),
	('Sale'),
	('Scone'),
	('Scottsdale'),
	('Seymour'),
	('Shepparton'),
	('Singleton'),
	('Smithton'),
	('Somerville'),
	('Southern Cross'),
	('Southport'),
	('Stawell'),
	('Streaky Bay'),
	('Sunbury'),
	('Swan Hill'),
	('Sydney'),
	('Tamworth'),
	('Taree'),
	('Thargomindah'),
	('Theodore'),
	('Three Springs'),
	('Tom Price'),
	('Toowoomba'),
	('Torquay'),
	('Townsville'),
	('Traralgon'),
	('Tumby Bay'),
	('Tumut'),
	('Tweed Heads'),
	('Ulladulla'),
	('Ulverstone'),
	('Victor Harbor'),
	('Wagga Wagga'),
	('Wagin'),
	('Wallan'),
	('Wallaroo'),
	('Wangaratta'),
	('Warragul'),
	('Warrnambool'),
	('Warwick'),
	('Weipa'),
	('Whyalla'),
	('Wilcannia'),
	('Windorah'),
	('Winton'),
	('Wodonga'),
	('Wollert'),
	('Wollongong'),
	('Wonthaggi'),
	('Woomera'),
	('Wyndham'),
	('Yamba'),
	('Yanchep'),
	('Yeppoon'),
	('Young'),
	('Yulara')
) AS v(name) ON c.code = 'AU'
ON CONFLICT (name, country_id) DO NOTHING;

-- ------------------------------------------------------------
-- Austria (AT)
-- ------------------------------------------------------------
INSERT INTO auth.cities (name, country_id)
SELECT v.name, c.id
FROM auth.countries c
JOIN (VALUES
	('Altmünster'),
	('Amstetten'),
	('Ansfelden'),
	('Attnang-Puchheim'),
	('Bad Ischl'),
	('Bad Vöslau'),
	('Baden'),
	('Berndorf'),
	('Bischofshofen'),
	('Bludenz'),
	('Braunau am Inn'),
	('Bregenz'),
	('Bruck an der Mur'),
	('Brunn am Gebirge'),
	('Deutsch-Wagram'),
	('Deutschlandsberg'),
	('Dornbirn'),
	('Ebreichsdorf'),
	('Eisenstadt'),
	('Engerwitzdorf'),
	('Enns'),
	('Feldbach'),
	('Feldkirch'),
	('Feldkirchen'),
	('Fürstenfeld'),
	('Gänserndorf'),
	('Gerasdorf bei Wien'),
	('Gleisdorf'),
	('Gmunden'),
	('Götzis'),
	('Graz'),
	('Groß-Enzersdorf'),
	('Guntramsdorf'),
	('Hall in Tirol'),
	('Hallein'),
	('Hard'),
	('Höchst'),
	('Hohenems'),
	('Hollabrunn'),
	('Imst'),
	('Innsbruck'),
	('Judenburg'),
	('Kapfenberg'),
	('Klagenfurt'),
	('Klosterneuburg'),
	('Knittelfeld'),
	('Köflach'),
	('Korneuburg'),
	('Krems an der Donau'),
	('Kufstein'),
	('Laakirchen'),
	('Lauterach'),
	('Leibnitz'),
	('Leoben'),
	('Leonding'),
	('Lienz'),
	('Linz'),
	('Lochau'),
	('Lustenau'),
	('Marchtrenk'),
	('Maria Enzersdorf'),
	('Mistelbach'),
	('Mödling'),
	('Neunkirchen'),
	('Neusiedl am See'),
	('Perchtoldsdorf'),
	('Perg'),
	('Purkersdorf'),
	('Rankweil'),
	('Ried im Innkreis'),
	('Rum'),
	('Saalfelden am Steinernen Meer'),
	('Salzburg'),
	('Sankt Andrä'),
	('Sankt Johann im Pongau'),
	('Sankt Johann in Tirol'),
	('Sankt Pölten'),
	('Sankt Valentin'),
	('Sankt Veit an der Glan'),
	('Schwaz'),
	('Schwechat'),
	('Seekirchen am Wallersee'),
	('Sierning'),
	('Spittal an der Drau'),
	('Stainz'),
	('Steyr'),
	('Stockerau'),
	('Strasshof an der Nordbahn'),
	('Telfs'),
	('Ternitz'),
	('Traiskirchen'),
	('Traun'),
	('Trofaiach'),
	('Velden am Wörthersee'),
	('Vienna'),
	('Villach'),
	('Vöcklabruck'),
	('Voitsberg'),
	('Völkermarkt'),
	('Waidhofen an der Ybbs'),
	('Weiz'),
	('Wels'),
	('Wiener Neudorf'),
	('Wiener Neustadt'),
	('Wolfsberg'),
	('Wolfurt'),
	('Wörgl'),
	('Zell am See'),
	('Zwettl'),
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
	('Pago Pago'),
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
