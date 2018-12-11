﻿IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CompoundImageTags') AND COL_LENGTH('CompoundImageTags', 'Tag') IS NOT NULL
BEGIN
	ALTER TABLE CompoundImageTags
		DROP CONSTRAINT PK_CompoundImageTags;
    ALTER TABLE CompoundImageTags 
		ALTER COLUMN [Tag] VARCHAR(16) NOT NULL;
	ALTER TABLE CompoundImageTags
		ADD CONSTRAINT PK_CompoundImageTags PRIMARY KEY (CompoundImageId, Tag);
END