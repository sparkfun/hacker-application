USE [RMW]
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_picture]    Script Date: 01/31/2013 14:29:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROC [dbo].[sp_insert_picture]
	@MLS				int,
	@PicturePathThumb		varchar(50),
	@PicturePathFull  		varchar(50),
	@PictureComments		varchar(80) = NULL,
	@DefaultPicture			bit = '1'
AS

-- create local variable to hold record count
DECLARE @RecordCount int
-- select the current record count
SELECT @RecordCount = (SELECT COUNT(*) FROM Pictures WHERE MLS = @MLS)

-- if the recordset is empty make the primary picture default
IF @RecordCount = 0
	INSERT INTO Pictures
		VALUES(@MLS, @PicturePathThumb, @PicturePathFull, @PictureComments, @DefaultPicture)
ELSE
	INSERT INTO Pictures
		VALUES(@MLS, @PicturePathThumb, @PicturePathFull, @PictureComments, DEFAULT)

