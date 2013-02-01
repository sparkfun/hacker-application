USE [RMW]
GO
/****** Object:  StoredProcedure [dbo].[sp_update_picture]    Script Date: 01/31/2013 14:33:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROC [dbo].[sp_update_picture]
	@PictureID			int,
	@MLS				int,
	@PictureComments		varchar(200),
	@DefaultPicture			bit
AS

DECLARE @tempPicID	int
DECLARE @tempCount	int

IF @DefaultPicture = 1
BEGIN
	UPDATE Pictures
		SET DefaultPicture = '0'
		WHERE MLS = @MLS
	UPDATE Pictures
		SET PictureComments = @PictureComments,
			DefaultPicture = @DefaultPicture
		WHERE PictureID = @PictureID
END
ELSE
BEGIN
	UPDATE Pictures
			SET PictureComments = @PictureComments,
				DefaultPicture = '0'
			WHERE PictureID = @PictureID

	SELECT @tempCount = (SELECT COUNT(*) FROM Pictures WHERE MLS = @MLS AND DefaultPicture = '1')
	IF @tempCount = 0
	BEGIN
		SELECT @tempPicID = (SELECT TOP 1 PictureID FROM Pictures WHERE MLS = @MLS)
		UPDATE Pictures
			SET DefaultPicture = '1'
		WHERE PictureID = @tempPicID
	END
END

