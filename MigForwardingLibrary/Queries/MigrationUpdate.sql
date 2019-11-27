  ALTER TABLE log_LPRES_ATNA_Simplified ADD [MaywoodsID]	BIGINT IDENTITY (1,1);
  ALTER TABLE log_LPRES_ATNA_Simplified ADD [MaywoodsDateTime] DATETIME;
  ALTER TABLE log_LPRES_ATNA_Simplified ADD [MaywoodsAuditID] BIGINT;
  GO

  ALTER TABLE log_LPRES_ATNA_Simplified ADD CONSTRAINT PK_MaywoodsID PRIMARY KEY(MaywoodsID);

  IF EXISTS ( SELECT NAME FROM dbo.sysindexes WHERE name = 'idx_MaywoodsDateTime')
  DROP INDEX [log_LPRES_ATNA_Simplified].[idx_MaywoodsDateTime]
  GO

    IF NOT EXISTS ( SELECT NAME FROM dbo.sysindexes WHERE name = 'idx_MaywoodsDateTime')
  CREATE INDEX idx_MaywoodsDateTime ON [log_LPRES_ATNA_Simplified] (MaywoodsDateTime)
  GO
