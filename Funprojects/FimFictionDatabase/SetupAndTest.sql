USE FimFictionDatabase

SELECT * FROM Stories

--CREATE TABLE Stories
--(
--	[Id] INT NOT NULL PRIMARY KEY IDENTITY(0,1),
--	[StoryNo] INT NOT NULL,
--	[Progress] INT NOT NULL,
--	[Name] NVARCHAR(MAX) NOT NULL, 
--	[WordCount] INT NOT NULL,
--	[ChapterCount] INT NOT NULL,
--	[Tags] NVARCHAR(MAX) NOT NULL,
--	[Audience] INT NOT NULL,
--	[Status] INT NOT NULL,
--	[Sequels] NVARCHAR(MAX),
--	[Author] NVARCHAR(MAX) NOT NULL,
--	[Rating] INT,
--	[DownloadStatus] INT NOT NULL,
--	[Series] NVARCHAR(MAX),
--	[Notes] NVARCHAR(MAX)
--)

INSERT INTO Stories (StoryNo, Progress, Name, WordCount, ChapterCount, Tags, Audience, [Status], Sequels, Author, Rating, DownloadStatus, Series, Notes, StoryLink) 
VALUES(1, 1, '413 Mulberry Lane : A Report (With Annotations by Twilight Sparkle)', 9377, 1, 'Dark', 11, 1, NULL, 'Starsong', 6, 1, NULL, NULL, 'http://www.fimfiction.net/story/93700/413-mulberry-lane-a-report-with-annotations-by-twilight-sparkle') 
