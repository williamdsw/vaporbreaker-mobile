
---------------------------------------------------------------------------------

CREATE TABLE "level" (
	"ID"	INTEGER NOT NULL,
	"NAME"	TEXT NOT NULL
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

---------------------------------------------------------------------------------

INSERT INTO "level" ("NAME") 
VALUES 
('Level_001'), ('Level_002'), ('Level_003'), ('Level_004'), ('Level_005'), 
('Level_006'), ('Level_007'), ('Level_008'), ('Level_009'), ('Level_010'), 
('Level_011'), ('Level_012'), ('Level_013'), ('Level_014'), ('Level_015'), 
('Level_016'), ('Level_017'), ('Level_018'), ('Level_019'), ('Level_020'), 
('Level_021'), ('Level_022'), ('Level_023'), ('Level_024'), ('Level_025'), 
('Level_026'), ('Level_027'), ('Level_028'), ('Level_029'), ('Level_030'), 
('Level_031'), ('Level_032'), ('Level_033'), ('Level_034'), ('Level_035'), 
('Level_036'), ('Level_037'), ('Level_038'), ('Level_039'), ('Level_040'), 
('Level_041'), ('Level_042'), ('Level_043'), ('Level_044'), ('Level_045'), 
('Level_046'), ('Level_047'), ('Level_048'), ('Level_049'), ('Level_050'), 
('Level_051'), ('Level_052'), ('Level_053'), ('Level_054'), ('Level_055'), 
('Level_056'), ('Level_057'), ('Level_058'), ('Level_059'), ('Level_060'), 
('Level_061'), ('Level_062'), ('Level_063'), ('Level_064'), ('Level_065'), 
('Level_066'), ('Level_067'), ('Level_068'), ('Level_069'), ('Level_070'), 
('Level_071'), ('Level_072'), ('Level_073'), ('Level_074'), ('Level_075'), 
('Level_076'), ('Level_077'), ('Level_078'), ('Level_079'), ('Level_080'), 
('Level_081'), ('Level_082'), ('Level_083'), ('Level_084'), ('Level_085'), 
('Level_086'), ('Level_087'), ('Level_088'), ('Level_089'), ('Level_090'), 
('Level_091'), ('Level_092'), ('Level_093'), ('Level_094'), ('Level_095'), 
('Level_096'), ('Level_097'), ('Level_098'), ('Level_099'), ('Level_100');

