USE [RMW]
GO
/****** Object:  StoredProcedure [dbo].[sp_select_residential]    Script Date: 01/31/2013 14:32:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROC [dbo].[sp_select_residential]
	@MLS			int
AS

SELECT rp.MLS, rp.ListingDateTime, rp.LastEditDateTime, rp.AltMLS, rp.Price, rp.Owner, ag1.FirstName + ' ' + ag1.LastName AS Agent1, rp.AgentID1, ag2.FirstName + ' ' + ag2.LastName AS Agent2, rp.AgentID2, rp.OfficeID, oc.OfficeCity, rp.Tagline, rp.Address1, rp.Address2, rp.CityID, ct.CityName, rp.Subdivision, rp.AnnualTaxes, rp.AnnualTaxYear, rp.ScheduleNumber, rp.Assessments, rp.Bedrooms, rp.Baths, rp.SquareFt, rp.YearBuilt, rp.YearRemodeled, rp.ParcelSize, rp.Style, rp.Foundation, rp.Construction, rp.Roof, rp.Garage, rp.Patio, rp.Deck, rp.Fenced, rp.FencingDescription, rp.Heating, rp.Fireplace, rp.Woodstove, rp.ElectricityProvider, rp.ElectricityMonthlyCost, rp.DomWaterProvider, rp.DomWaterMonthlyCost, rp.IrrWaterProvider, rp.IrrWaterShares, rp.IrrWaterMonthlyCost, rp.Sewer, rp.KitchenDim, rp.LivingRoomDim, rp.DiningRoomDim, rp.FamilyRoomDim, rp.MasterBedDim, rp.Bedroom2Dim, rp.Bedroom3Dim, rp.Bedroom4Dim, rp.Bathroom1Dim, rp.Bathroom2Dim, rp.Bathroom3Dim, rp.Bathroom4Dim, rp.BasementDim, rp.GarageDim, rp.PatioDim, rp.DeckDim, rp.ShedDim, rp.OfficeDim, rp.MediaRoomDim, rp.LaundryRoomDim, rp.SunroomDim, rp.Possession, rp.EarnestMoney, rp.FeaturesDescription, rp.InclusionsDescription, rp.ExclusionsDescription, rp.OutbuildingsDescription, rp.DisclosuresDescription, rp.MapDirections 
FROM ResidentialProperties AS rp
	INNER JOIN Cities AS ct
	ON rp.CityID = ct.CityID
	INNER JOIN Offices AS oc
	ON rp.OfficeID = oc.OfficeID
	INNER JOIN Agents AS ag1
	ON rp.AgentID1 = ag1.AgentID
	LEFT OUTER JOIN Agents AS ag2
	ON rp.AgentID2 = ag2.AgentID
WHERE MLS = @MLS

