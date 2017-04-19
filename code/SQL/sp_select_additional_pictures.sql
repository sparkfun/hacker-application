USE [RMW]
GO
/****** Object:  StoredProcedure [dbo].[sp_select_additional_pictures]    Script Date: 01/31/2013 14:30:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROC [dbo].[sp_select_additional_pictures]
	@MLS		int
AS
SELECT PictureID, MLS, PicturePathThumb AS ThumbnailPath, PictureComments AS Comments
FROM Pictures
WHERE MLS = @MLS AND DefaultPicture = '0'
ORDER BY PictureID

