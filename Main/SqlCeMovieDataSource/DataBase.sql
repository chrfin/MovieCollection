CREATE TABLE Movies
(
	id int PRIMARY KEY IDENTITY,
	title ntext NOT NULL,
	title_original ntext,
	year int,
	url ntext,
	country ntext,
	cover image,
	plot ntext,
	rating float DEFAULT 0
);

CREATE TABLE Persons
(
	id int PRIMARY KEY IDENTITY,
	name nvarchar(255) NOT NULL,
	picture image
);

CREATE TABLE Movies_Directors
(
	movies_id int NOT NULL,
	directors_id int NOT NULL,
	CONSTRAINT Movies_Directors_PK PRIMARY KEY (movies_id, directors_id),
	FOREIGN KEY (movies_id) REFERENCES Movies(id),
	FOREIGN KEY (directors_id) REFERENCES Persons(id)
);

CREATE TABLE Movies_Cast
(
	movies_id int NOT NULL,
	cast_id int NOT NULL,
	role ntext,
	CONSTRAINT Movies_Cast_PK PRIMARY KEY (movies_id, cast_id),
	FOREIGN KEY (movies_id) REFERENCES Movies(id),
	FOREIGN KEY (cast_id) REFERENCES Persons(id)
);

CREATE TABLE Genres
(
	id int PRIMARY KEY IDENTITY,
	title nvarchar(255) NOT NULL
);

CREATE TABLE Movies_Genres
(
	movies_id int NOT NULL,
	genres_id int NOT NULL,
	CONSTRAINT Movies_Genres_PK PRIMARY KEY (movies_id, genres_id),
	FOREIGN KEY (movies_id) REFERENCES Movies(id),
	FOREIGN KEY (genres_id) REFERENCES Genres(id)
);

CREATE TABLE MediaFiles
(
	id int PRIMARY KEY IDENTITY,
	path ntext NOT NULL,
	size bigint
);

CREATE TABLE Movies_MediaFiles
(
	movies_id int NOT NULL,
	mediafiles_id int NOT NULL,
	CONSTRAINT Movies_MediaFiles_PK PRIMARY KEY (movies_id, mediafiles_id),
	FOREIGN KEY (movies_id) REFERENCES Movies(id),
	FOREIGN KEY (mediafiles_id) REFERENCES MediaFiles(id)
);

CREATE TABLE VideoProperties
(
	id int PRIMARY KEY IDENTITY,
	duration bigint NOT NULL,
	width int,
	height int,
	format ntext,
	encoding ntext,
	bitrate int
);

CREATE TABLE MediaFiles_VideoProperties
(
	mediafiles_id int NOT NULL,
	videoproperties_id int NOT NULL,
	CONSTRAINT MediaFiles_VideoProperties_PK PRIMARY KEY (mediafiles_id, videoproperties_id),
	FOREIGN KEY (mediafiles_id) REFERENCES MediaFiles(id),
	FOREIGN KEY (videoproperties_id) REFERENCES VideoProperties(id)
);

CREATE TABLE AudioProperties
(
	id int PRIMARY KEY IDENTITY,
	format ntext,
	bitrate int,
	channels int NOT NULL,
	encoding ntext,
	language nvarchar(10)
);

CREATE TABLE MediaFiles_AudioProperties
(
	mediafiles_id int NOT NULL,
	audioproperties_id int NOT NULL,
	CONSTRAINT MediaFiles_AudioProperties_PK PRIMARY KEY (mediafiles_id, audioproperties_id),
	FOREIGN KEY (mediafiles_id) REFERENCES MediaFiles(id),
	FOREIGN KEY (audioproperties_id) REFERENCES AudioProperties(id)
);

CREATE TABLE UserProfiles
(
	id int PRIMARY KEY IDENTITY,
	name ntext NOT NULL
);

CREATE TABLE UserMovieSettings
(
	id int PRIMARY KEY IDENTITY,
	movies_id int NOT NULL,
	seen bit DEFAULT 0,
	rating float DEFAULT 0,
	comment ntext,
	FOREIGN KEY (movies_id) REFERENCES Movies(id)
);

CREATE TABLE UserProfiles_UserMovieSettings
(
	userprofiles_id int NOT NULL,
	usermoviesettings_id int NOT NULL,
	CONSTRAINT UserProfiles_UserMovieSettings_PK PRIMARY KEY (userprofiles_id, usermoviesettings_id),
	FOREIGN KEY (userprofiles_id) REFERENCES UserProfiles(id),
	FOREIGN KEY (usermoviesettings_id) REFERENCES UserMovieSettings(id)
);