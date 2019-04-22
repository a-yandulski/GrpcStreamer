USE [GrpcStreamer];
GO

SET IDENTITY_INSERT [dbo].[TItem] ON;

  INSERT INTO [dbo].[TItem] (ItemId, [Value], StatusId)
  SELECT Id, CONCAT('Value ', Id), 0
  FROM (
  SELECT TOP (10000000) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Id 
  FROM sys.all_columns  T1
  CROSS APPLY ( SELECT TOP (100000) 1 AS [T2Col]
                FROM sys.all_columns  ) T2 ) T

  SET IDENTITY_INSERT [dbo].[TItem] OFF;
