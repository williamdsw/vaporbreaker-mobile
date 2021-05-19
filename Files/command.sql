
---------------------------------------------------------------------------------

CREATE TABLE "level" (
	"ID"	INTEGER NOT NULL,
	"NAME"	TEXT NOT NULL,
	"IS_UNLOCKED"	INTEGER NOT NULL,
	"IS_COMPLETED"	INTEGER NOT NULL,
	PRIMARY KEY("ID" AUTOINCREMENT)
);

CREATE TABLE "scoreboard" (
	"ID"	INTEGER NOT NULL,
	"LEVEL_ID"	INTEGER NOT NULL,
	"SCORE"	INTEGER NOT NULL,
	"TIME_SCORE"	INTEGER NOT NULL,
	"MOMENT"	INTEGER NOT NULL,
	FOREIGN KEY("LEVEL_ID") REFERENCES "level"("ID"),
	PRIMARY KEY("ID" AUTOINCREMENT)
);

CREATE TABLE "localization" (
	"ID"	INTEGER NOT NULL,
	"LANGUAGE"	TEXT NOT NULL,
	"CONTENT"	TEXT NOT NULL,
	PRIMARY KEY("ID" AUTOINCREMENT)
);

---------------------------------------------------------------------------------

INSERT INTO "level" ("NAME", "IS_UNLOCKED", "IS_COMPLETED") 
VALUES 
('Level_001', 1, 0), ('Level_002', 0, 0), ('Level_003', 0, 0), ('Level_004', 0, 0), ('Level_005', 0, 0), 
('Level_006', 0, 0), ('Level_007', 0, 0), ('Level_008', 0, 0), ('Level_009', 0, 0), ('Level_010', 0, 0), 
('Level_011', 0, 0), ('Level_012', 0, 0), ('Level_013', 0, 0), ('Level_014', 0, 0), ('Level_015', 0, 0), 
('Level_016', 0, 0), ('Level_017', 0, 0), ('Level_018', 0, 0), ('Level_019', 0, 0), ('Level_020', 0, 0), 
('Level_021', 0, 0), ('Level_022', 0, 0), ('Level_023', 0, 0), ('Level_024', 0, 0), ('Level_025', 0, 0), 
('Level_026', 0, 0), ('Level_027', 0, 0), ('Level_028', 0, 0), ('Level_029', 0, 0), ('Level_030', 0, 0), 
('Level_031', 0, 0), ('Level_032', 0, 0), ('Level_033', 0, 0), ('Level_034', 0, 0), ('Level_035', 0, 0), 
('Level_036', 0, 0), ('Level_037', 0, 0), ('Level_038', 0, 0), ('Level_039', 0, 0), ('Level_040', 0, 0), 
('Level_041', 0, 0), ('Level_042', 0, 0), ('Level_043', 0, 0), ('Level_044', 0, 0), ('Level_045', 0, 0), 
('Level_046', 0, 0), ('Level_047', 0, 0), ('Level_048', 0, 0), ('Level_049', 0, 0), ('Level_050', 0, 0), 
('Level_051', 0, 0), ('Level_052', 0, 0), ('Level_053', 0, 0), ('Level_054', 0, 0), ('Level_055', 0, 0), 
('Level_056', 0, 0), ('Level_057', 0, 0), ('Level_058', 0, 0), ('Level_059', 0, 0), ('Level_060', 0, 0), 
('Level_061', 0, 0), ('Level_062', 0, 0), ('Level_063', 0, 0), ('Level_064', 0, 0), ('Level_065', 0, 0), 
('Level_066', 0, 0), ('Level_067', 0, 0), ('Level_068', 0, 0), ('Level_069', 0, 0), ('Level_070', 0, 0), 
('Level_071', 0, 0), ('Level_072', 0, 0), ('Level_073', 0, 0), ('Level_074', 0, 0), ('Level_075', 0, 0), 
('Level_076', 0, 0), ('Level_077', 0, 0), ('Level_078', 0, 0), ('Level_079', 0, 0), ('Level_080', 0, 0), 
('Level_081', 0, 0), ('Level_082', 0, 0), ('Level_083', 0, 0), ('Level_084', 0, 0), ('Level_085', 0, 0), 
('Level_086', 0, 0), ('Level_087', 0, 0), ('Level_088', 0, 0), ('Level_089', 0, 0), ('Level_090', 0, 0), 
('Level_091', 0, 0), ('Level_092', 0, 0), ('Level_093', 0, 0), ('Level_094', 0, 0), ('Level_095', 0, 0), 
('Level_096', 0, 0), ('Level_097', 0, 0), ('Level_098', 0, 0), ('Level_099', 0, 0), ('Level_100', 0, 0);

INSERT INTO "main"."localization" ("ID", "LANGUAGE", "CONTENT") VALUES ('1', 'en', '{"about":{"rights":"1994-2021, All Rights Reserved."},"configurations":{"backgroundmusic":"Background Music","configurations":"Configurations","musiceffects":"Music Effects","resetprogress":"Reset Progress","soundeffects":"Sound Effects"},"general":{"about":"About","back":"Back","continue":"Continue","no":"No","score":"Score","time":"Time","timescore":"Time Score","yes":"Yes"},"instructions":{"darkblock":"Dark Block: 3 Hits","dragtoaim":"Drag to Aim","instructions":"Instructions","goldblock":"Golden Blocks: 5 Hits","lightblock":"Light Block: 1 Hit","movepaddle":"Move Paddle","normalblock":"Normal Block: 2 Hits","pause":"Pause","releaseball":"Release Ball / Shoot","silverblock":"Silver Blocks: 4 Hits","unbreakableblock":"Unbreakable Blocks"},"levelcomplete":{"bestcombo":"Best Combo","currentballs":"Current Balls","levelcompleted":"Level Completed!","newscore":"New Score!","total":"Total"},"leveldetails":{"bestscore":"Best Score","level":"Level","play":"Play!","besttime":"Best Time"},"pause":{"levels":"Levels","paused":"Paused","restart":"Restart","resume":"Resume"},"powerups":{"allbreakablesblocks":"All Breakable Blocks","ballbigger":"Ball Bigger","ballfaster":"Ball Faster","ballslower":"Ball Slower","ballsmaller":"Ball Smaller","duplicateball":"Duplicate Ball","expandpaddle":"Expand Paddle","fireball":"Fire Ball","onehitblock":"One Hit Block","pullblocksdown":"Pull Blocks Down","pullblocksleft":"Pull Blocks Left","pullblocksright":"Pull Blocks Right","pullblocksup":"Pull Blocks Up","resetballs":"Reset Balls","resetpaddle":"Reset Paddle","shooter":"Shooter","shrinkpaddle":"Shrink Paddle"},"resetprogress":{"areyousure":"Are you sure?","warning":"All your progress will be reseted, such as scores, time scores, and levels unlocked!"},"selectlevels":{"chooseanlevel":"Choose An Level","saving":"Saving"}}');
INSERT INTO "main"."localization" ("ID", "LANGUAGE", "CONTENT") VALUES ('2', 'pt-br', '{"about":{"rights":"1994-2021, Todos Direitos Reservados."},"configurations":{"backgroundmusic":"Musica de Fundo","configurations":"Configuracoes","musiceffects":"Efeitos Musicais","resetprogress":"Redefinir Progresso","soundeffects":"Efeitos Sonoros"},"general":{"about":"Sobre","back":"Voltar","continue":"Continuar","no":"Nao","score":"Pontuacao","time":"Tempo","timescore":"Pontuacao do Tempo","yes":"Sim"},"instructions":{"darkblock":"Bloco Pesado: 3 Golpes","dragtoaim":"Arraste para Mirar","goldblock":"Blocos Dourados: 5 Golpes","instructions":"Instructions","lightblock":"Bloco Leve: 1 Golpe","movepaddle":"Mover Remo","normalblock":"Bloco Normal: 2 Golpes","pause":"Pausar","releaseball":"Soltar Bola / Atirar","silverblock":"Blocos Prateados: 4 Golpes","unbreakableblock":"Bloco Indestrutivel"},"levelcomplete":{"bestcombo":"Melhor Combo","currentballs":"Bolas Restantes","levelcompleted":"Nivel Completo!","newscore":"Nova Pontuacao!","total":"Total"},"leveldetails":{"bestscore":"Melhor Pontuacao","level":"Nivel","play":"Jogar!","besttime":"Melhor Tempo"},"pause":{"levels":"Niveis","paused":"Pause","restart":"Reiniciar","resume":"Resumir"},"powerups":{"allbreakablesblocks":"Todos Blocos Destrutiveis","ballbigger":"Bola Maior","ballfaster":"Bola Rápida","ballslower":"Bola Lenta","ballmaller":"Bola Menor","duplicateball":"Duplica Bola","expandpaddle":"Expande Remo","fireball":"Bola de Fogo","onehitblock":"Blocos de 1 Golpe","pullblocksdown":"Puxa Blocos p/ Baixo","pullblocksleft":"Puxa Blocos p/ Esquerda","pullblocksright":"Puxa Blocos p/ Direita","pullblocksup":"Puxa Blocos p/ Cima","resetballs":"Redefine Bolas","resetpaddle":"Redefine Remo","shooter":"Atirador","shrinkpaddle":"Diminui Remo"},"resetprogress":{"areyousure":"Voce tem certeza?","warning":"Todo seu progresso sera resetado, incluindo pontuacoes e niveis desbloqueados!"},"selectlevels":{"chooseanlevel":"Escolha um Nivel","saving":"Salvando..."}}');