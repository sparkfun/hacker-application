USE [RMW]
GO
/****** Object:  StoredProcedure [dbo].[sp_select_cities]    Script Date: 01/31/2013 14:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROC [dbo].[sp_select_cities]
AS
	SELECT CityID, CityName
	FROM Cities
	ORDER BY CityName

