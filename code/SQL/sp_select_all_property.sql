USE [RMW]
GO
/****** Object:  StoredProcedure [dbo].[sp_select_all_property]    Script Date: 01/31/2013 14:31:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROC [dbo].[sp_select_all_property]

AS

SELECT	rp.MLS, rp.PropertyTypeID, (SELECT pt.PropertyType FROM PropertyTypes AS pt WHERE rp.PropertyTypeID = pt.PropertyTypeID) AS PropertyType, rp.Price, rp.Address1 AS Address, (SELECT c.CityName FROM Cities AS c WHERE rp.CityID = c.CityID) AS City, rp.Owner
FROM ResidentialProperties AS rp

UNION

SELECT	frp.MLS, frp.PropertyTypeID, (SELECT pt.PropertyType FROM PropertyTypes AS pt WHERE frp.PropertyTypeID = pt.PropertyTypeID) AS PropertyType, frp.Price, frp.Address1 AS Address, (SELECT c.CityName FROM Cities AS c WHERE frp.CityID = c.CityID) AS City, frp.Owner
FROM FarmRanchProperties AS frp

UNION

SELECT	vl.MLS, vl.PropertyTypeID, (SELECT pt.PropertyType FROM PropertyTypes AS pt WHERE vl.PropertyTypeID = pt.PropertyTypeID) AS PropertyType, vl.Price, vl.Address1 AS Address, (SELECT c.CityName FROM Cities AS c WHERE vl.CityID = c.CityID) AS City, vl.Owner
FROM VacantLand AS vl

ORDER BY Price

