USE Granddaddy;
GO

IF NOT EXISTS (SELECT 1 FROM site.Person)
BEGIN
    SET IDENTITY_INSERT site.Person ON;

    INSERT INTO site.Person (Id, FatherId, MotherId, Name, Surname, BirthDate, IdentityNumber) VALUES
    (1,  NULL, NULL, N'Johannes', N'Vermeulen', '1900-01-01', '0001015000084'),
    (2,  NULL, NULL, N'Anna',     N'Vermeulen', '1903-05-12', '0305120001089'),
    (3,  1,    2,    N'Pieter',   N'Vermeulen', '1925-03-10', '2503105001086'),
    (4,  1,    2,    N'Maria',    N'Vermeulen', '1928-07-22', '2807220002089'),
    (5,  1,    2,    N'Hendrik',  N'Vermeulen', '1930-11-05', '3011055002087'),
    (6,  NULL, NULL, N'Sannie',   N'Vermeulen', '1927-02-14', '2702140003087'),  -- m. Pieter
    (7,  3,    6,    N'Jan',      N'Vermeulen', '1950-06-18', '5006185003086'),
    (8,  3,    6,    N'Elsie',    N'Vermeulen', '1952-09-30', '5209300004084'),
    (9,  NULL, NULL, N'Katryn',   N'Vermeulen', '1951-04-25', '5104250005084'),  -- m. Jan
    (10, 7,    9,    N'Willem',   N'Vermeulen', '1975-12-01', '7512015004085'),
    (11, 7,    9,    N'Susanna',  N'Vermeulen', '1978-08-08', '7808080006082'),
    (12, NULL, NULL, N'Lize',     N'Vermeulen', '1976-03-19', '7603190007082'),  -- m. Willem
    (13, 10,   12,   N'Thomas',   N'Vermeulen', '2000-05-20', '0005205005084'),
    (14, 10,   12,   N'Emma',     N'Vermeulen', '2003-10-11', '0310110008086'),
    (15, NULL, NULL, N'Gerrit',   N'Botha',     '1926-09-09', '2609095006082'),  -- m. Maria
    (16, 15,   4,    N'Anika',    N'Botha',     '1949-01-15', '4901150009087'),
    (17, 15,   4,    N'Cornelis', N'Botha',     '1953-07-07', '5307075007082'),

    (100, NULL, NULL, N'Adam',       N'Oudtshoorn', '1350-01-01', '5001015008082'),
    (101,  100,  200, N'Barend',     N'Oudtshoorn', '1377-01-01', '7701015009083'),
    (102,  101,  201, N'Christiaan', N'Oudtshoorn', '1404-01-01', '0401015010085'),
    (103,  102,  202, N'Daniel',     N'Oudtshoorn', '1431-01-01', '3101015011086'),
    (104,  103,  203, N'Eben',       N'Oudtshoorn', '1458-01-01', '5801015012087'),
    (105,  104,  204, N'Frederik',   N'Oudtshoorn', '1485-01-01', '8501015013088'),
    (106,  105,  205, N'Gideon',     N'Oudtshoorn', '1512-01-01', '1201015014080'),
    (107,  106,  206, N'Hannes',     N'Oudtshoorn', '1539-01-01', '3901015015080'),
    (108,  107,  207, N'Izak',       N'Oudtshoorn', '1566-01-01', '6601015016081'),
    (109,  108,  208, N'Jacobus',    N'Oudtshoorn', '1593-01-01', '9301015017083'),
    (110,  109,  209, N'Koos',       N'Oudtshoorn', '1620-01-01', '2001015018084'),
    (111,  110,  210, N'Lodewyk',    N'Oudtshoorn', '1647-01-01', '4701015019085'),
    (112,  111,  211, N'Marius',     N'Oudtshoorn', '1674-01-01', '7401015020087'),
    (113,  112,  212, N'Nicolaas',   N'Oudtshoorn', '1701-01-01', '0101015021088'),
    (114,  113,  213, N'Otto',       N'Oudtshoorn', '1728-01-01', '2801015022089'),
    (115,  114,  214, N'Pieter',     N'Oudtshoorn', '1755-01-01', '5501015023080'),
    (116,  115,  215, N'Quintin',    N'Oudtshoorn', '1782-01-01', '8201015024082'),
    (117,  116,  216, N'Rudolf',     N'Oudtshoorn', '1809-01-01', '0901015025082'),
    (118,  117,  217, N'Stefan',     N'Oudtshoorn', '1836-01-01', '3601015026083'),
    (119,  118,  218, N'Theunis',    N'Oudtshoorn', '1863-01-01', '6301015027085'),
    (120,  119,  219, N'Urias',      N'Oudtshoorn', '1890-01-01', '9001015028086'),
    (121,  120,  220, N'Victor',     N'Oudtshoorn', '1917-01-01', '1701015029087'),
    (122,  121,  221, N'Willem',     N'Oudtshoorn', '1944-01-01', '4401015030089'),
    (123,  122,  222, N'Xander',     N'Oudtshoorn', '1971-01-01', '7101015031080'),
    (200, NULL, NULL, N'Anna',       N'Oudtshoorn', '1351-01-01', '5101010011089'),
    (201, NULL, NULL, N'Bettie',     N'Oudtshoorn', '1378-01-01', '7801010012080'),
    (202, NULL, NULL, N'Carolina',   N'Oudtshoorn', '1405-01-01', '0501010013081'),
    (203, NULL, NULL, N'Dorothea',   N'Oudtshoorn', '1432-01-01', '3201010014083'),
    (204, NULL, NULL, N'Elizabeth',  N'Oudtshoorn', '1459-01-01', '5901010015083'),
    (205, NULL, NULL, N'Francina',   N'Oudtshoorn', '1486-01-01', '8601010016084'),
    (206, NULL, NULL, N'Gertruida',  N'Oudtshoorn', '1513-01-01', '1301010017086'),
    (207, NULL, NULL, N'Helena',     N'Oudtshoorn', '1540-01-01', '4001010018087'),
    (208, NULL, NULL, N'Ida',        N'Oudtshoorn', '1567-01-01', '6701010019088'),
    (209, NULL, NULL, N'Johanna',    N'Oudtshoorn', '1594-01-01', '9401010020080'),
    (210, NULL, NULL, N'Katrien',    N'Oudtshoorn', '1621-01-01', '2101010021081'),
    (211, NULL, NULL, N'Lavinia',    N'Oudtshoorn', '1648-01-01', '4801010022082'),
    (212, NULL, NULL, N'Magdalena',  N'Oudtshoorn', '1675-01-01', '7501010023083'),
    (213, NULL, NULL, N'Nellie',     N'Oudtshoorn', '1702-01-01', '0201010024085'),
    (214, NULL, NULL, N'Ottilie',    N'Oudtshoorn', '1729-01-01', '2901010025085'),
    (215, NULL, NULL, N'Petronella', N'Oudtshoorn', '1756-01-01', '5601010026086'),
    (216, NULL, NULL, N'Quintina',   N'Oudtshoorn', '1783-01-01', '8301010027088'),
    (217, NULL, NULL, N'Rachel',     N'Oudtshoorn', '1810-01-01', '1001010028089'),
    (218, NULL, NULL, N'Susara',     N'Oudtshoorn', '1837-01-01', '3701010029080'),
    (219, NULL, NULL, N'Trijntje',   N'Oudtshoorn', '1864-01-01', '6401010030082'),
    (220, NULL, NULL, N'Ursula',     N'Oudtshoorn', '1891-01-01', '9101010031083'),
    (221, NULL, NULL, N'Valerie',    N'Oudtshoorn', '1918-01-01', '1801010032084'),
    (222, NULL, NULL, N'Wilhelmina', N'Oudtshoorn', '1945-01-01', '4501010033085');

    SET IDENTITY_INSERT site.Person OFF;
END
GO

IF NOT EXISTS (SELECT 1 FROM site.Person WHERE Id = 300)
BEGIN
    SET IDENTITY_INSERT site.Person ON;

    INSERT INTO site.Person (Id, FatherId, MotherId, Name, Surname, BirthDate, IdentityNumber) VALUES
    (300, 100, 200, N'Andries', N'Oudtshoorn', '1377-01-01', '7701015000082'),
    (301, 100, 200, N'Annetjie', N'Oudtshoorn', '1377-06-08', '7706080100089'),
    (302, 101, 201, N'Johanna', N'Oudtshoorn', '1404-04-03', '0404030101087'),
    (303, 102, 202, N'Coenraad', N'Oudtshoorn', '1431-07-05', '3107055001085'),
    (304, 102, 202, N'Magrieta', N'Oudtshoorn', '1431-12-12', '3112120102082'),
    (305, 102, 202, N'Dirk', N'Oudtshoorn', '1431-05-19', '3105195002088'),
    (306, 104, 204, N'Roelof', N'Oudtshoorn', '1485-01-09', '8501095003082'),
    (307, 104, 204, N'Aletta', N'Oudtshoorn', '1485-06-16', '8506160103089'),
    (308, 105, 205, N'Hester', N'Oudtshoorn', '1512-04-11', '1204110104087'),
    (309, 107, 207, N'Susanna', N'Oudtshoorn', '1566-10-15', '6610150105088'),
    (310, 107, 207, N'Schalk', N'Oudtshoorn', '1566-03-22', '6603225004089'),
    (311, 107, 207, N'Martha', N'Oudtshoorn', '1566-08-02', '6608020106088'),
    (312, 108, 208, N'Lourens', N'Oudtshoorn', '1593-01-17', '9301175005084'),
    (313, 108, 208, N'Jacoba', N'Oudtshoorn', '1593-06-24', '9306240107088'),
    (314, 109, 209, N'Sofia', N'Oudtshoorn', '1620-04-19', '2004190108084'),
    (315, 110, 210, N'Stephanus', N'Oudtshoorn', '1647-07-21', '4707215006087'),
    (316, 110, 210, N'Christina', N'Oudtshoorn', '1647-12-01', '4712010109086'),
    (317, 110, 210, N'Hercules', N'Oudtshoorn', '1647-05-08', '4705085007086'),
    (318, 112, 212, N'Petrus', N'Oudtshoorn', '1701-01-25', '0101255008084'),
    (319, 113, 213, N'Engela', N'Oudtshoorn', '1728-04-27', '2804270110086'),
    (320, 113, 213, N'Tobias', N'Oudtshoorn', '1728-09-07', '2809075009086'),
    (321, 114, 214, N'Gerrit', N'Oudtshoorn', '1755-07-02', '5507025010082'),
    (322, 114, 214, N'Cornelia', N'Oudtshoorn', '1755-12-09', '5512090111088'),
    (323, 114, 214, N'Hendrik', N'Oudtshoorn', '1755-05-16', '5505165011084'),
    (324, 116, 216, N'Frans', N'Oudtshoorn', '1809-01-06', '0901065012089'),
    (325, 116, 216, N'Petronella', N'Oudtshoorn', '1809-06-13', '0906130112087'),
    (326, 117, 217, N'Hendrina', N'Oudtshoorn', '1836-04-08', '3604080113083'),
    (327, 118, 218, N'Jurie', N'Oudtshoorn', '1863-07-10', '6307105013083'),
    (328, 118, 218, N'Francina', N'Oudtshoorn', '1863-12-17', '6312170114089'),
    (329, 118, 218, N'Wynand', N'Oudtshoorn', '1863-05-24', '6305245014086'),
    (330, 119, 219, N'Gertruida', N'Oudtshoorn', '1890-10-12', '9010120115084'),
    (331, 121, 221, N'Katrien', N'Oudtshoorn', '1944-04-16', '4404160116083'),
    (332, 121, 221, N'Albertus', N'Oudtshoorn', '1944-09-23', '4409235015087'),
    (333, 122, 222, N'Cobus', N'Oudtshoorn', '1971-07-18', '7107185016082'),
    (334, 122, 222, N'Lavinia', N'Oudtshoorn', '1971-12-25', '7112250117088'),
    (335, 122, 222, N'Danie', N'Oudtshoorn', '1971-05-05', '7105055017081');

    SET IDENTITY_INSERT site.Person OFF;
END
GO
