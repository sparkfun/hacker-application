USE [RMW]
GO
/****** Object:  StoredProcedure [dbo].[sp_select_agents]    Script Date: 01/31/2013 14:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROC [dbo].[sp_select_agents]
AS
	SELECT AgentID, LastName + ', ' + FirstName AS FullName
	FROM Agents
	ORDER BY FullName

