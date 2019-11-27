
  IF EXISTS ( SELECT NAME FROM dbo.sysindexes WHERE name = 'idx_MaywoodsDateTime')
  DROP INDEX [log_LPRES_ATNA_Simplified].[idx_MaywoodsDateTime]
  GO

ALTER TABLE log_LPRES_ATNA_Simplified DROP CONSTRAINT PK_MaywoodsID;

  ALTER TABLE log_LPRES_ATNA_Simplified DROP COLUMN [MaywoodsID];
  ALTER TABLE log_LPRES_ATNA_Simplified DROP COLUMN [MaywoodsDateTime] ;
  ALTER TABLE log_LPRES_ATNA_Simplified DROP COLUMN [MaywoodsAuditID] ;
  GO
