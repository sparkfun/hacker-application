USE [RMW]
GO
/****** Object:  StoredProcedure [dbo].[sp_login_user]    Script Date: 01/31/2013 14:30:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROC [dbo].[sp_login_user]
	@UserName			varchar(10),
	@Password			varchar(40)
AS

SELECT COUNT(*)
FROM Users
WHERE UserName = @UserName AND Password = @Password



