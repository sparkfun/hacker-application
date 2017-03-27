USE [Sts9Store]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_PendingStock]    Script Date: 10/02/2016 18:15:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Rob Kurtz
-- CreateDate:	?
-- Title:		[fn_PendingStock]
-- Description: Returns the real-time inventory count for a product line
/*
	The base idx is the first idx in the series
	A base ticket allows us to group tickets being sold in a package/multi-day run

	example:
	you have a list of item ids and their respective quantities

	select * from  fn_pendingstock('ticket')
	select * from  fn_pendingstock('merch')
*/
-- =============================================

CREATE	FUNCTION [dbo].[fn_PendingStock](

	@context VARCHAR(50)

)
RETURNS @PendingStock TABLE (

	idx		INT NOT NULL,
	iQty	INT NOT NULL

)
AS

BEGIN

	IF (@context = 'ticket')
	BEGIN

		-- GroupStock quantifies the inventory for a base ticket entity
		-- groupingMechanism  groups by base ticket id
		DECLARE @GroupedStock
			TABLE (
				BaseIdx				INT,
				GroupingMechanism	VARCHAR(50),
				Qty					INT
			)

		INSERT	@GroupedStock (BaseIdx, GroupingMechanism, Qty)
		SELECT
			MIN(base.[tShowTicketId]) AS baseIdx,
			base.[GroupingMechanism],
			SUM(base.[iQty]) AS Qty
		FROM
		(
			SELECT	DISTINCT stock.[tShowTicketId], link.[GroupIdentifier],
					CASE WHEN link.[GroupIdentifier] IS NULL
						THEN CAST(stock.[tShowTicketId] AS VARCHAR(50))
						ELSE CAST(link.[GroupIdentifier] AS VARCHAR(50))
					END AS GroupingMechanism,
					stock.[iQty]
			FROM	[TicketStock] stock
				LEFT OUTER JOIN [ShowTicketPackageLink] link
					ON	stock.[tShowTicketId] = link.[ParentShowTicketId]
		) AS base
		GROUP BY base.[GroupingMechanism]


		/*RETURN THE ROWS*/
		INSERT	@PendingStock ( idx, iQty)

		SELECT	tgs.[baseIdx] AS idx, tgs.[Qty] AS iQty
		FROM	@GroupedStock tgs

		UNION

		SELECT	link.[LinkedShowTicketId] AS idx, tgs.[Qty] AS iQty
		FROM	@GroupedStock tgs, [ShowTicketPackageLink] link
		WHERE	tgs.[baseIdx] = link.[ParentShowTicketId]

		ORDER	BY [idx]

	END ELSE IF (@context = 'merch') BEGIN

		INSERT	@PendingStock ( idx, iQty)

		SELECT	stock.[tMerchId], SUM([iQty]) AS iQty
		FROM	[MerchStock] stock
		GROUP BY stock.[tMerchId]

	END

	RETURN

END
GO