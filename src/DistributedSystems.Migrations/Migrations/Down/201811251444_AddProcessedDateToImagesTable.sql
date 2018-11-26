﻿IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Images') AND COL_LENGTH('Images', 'ProcessedDate') IS NOT NULL
BEGIN
    ALTER TABLE Images DROP COLUMN ProcessedDate;
END