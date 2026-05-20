IF DB_ID('Granddaddy') IS NULL
    CREATE DATABASE Granddaddy;
GO

USE Granddaddy;
GO

IF SCHEMA_ID('site') IS NULL
    EXEC('CREATE SCHEMA site');
GO

IF OBJECT_ID('site.Person') IS NULL
BEGIN
    CREATE TABLE site.Person (
        Id             INT IDENTITY(1,1) PRIMARY KEY,
        FatherId       INT NULL,
        MotherId       INT NULL,
        Name           NVARCHAR(100) NOT NULL,
        Surname        NVARCHAR(100) NOT NULL,
        BirthDate      DATE NULL,
        IdentityNumber VARCHAR(20) NOT NULL,
        CONSTRAINT FK_Person_Father FOREIGN KEY (FatherId) REFERENCES site.Person(Id),
        CONSTRAINT FK_Person_Mother FOREIGN KEY (MotherId) REFERENCES site.Person(Id),
        CONSTRAINT UQ_Person_IdentityNumber UNIQUE (IdentityNumber)
    );
END
GO

DROP INDEX IF EXISTS IX_Person_FatherId ON site.Person;
CREATE INDEX IX_Person_FatherId ON site.Person(FatherId)
    INCLUDE (MotherId, Name, Surname, BirthDate, IdentityNumber);

DROP INDEX IF EXISTS IX_Person_MotherId ON site.Person;
CREATE INDEX IX_Person_MotherId ON site.Person(MotherId)
    INCLUDE (FatherId, Name, Surname, BirthDate, IdentityNumber);
GO
