USE [Sts9Store]
GO
/****** Object:  StoredProcedure [dbo].[tx_Inventory_AddUpdate]    Script Date: 10/02/2016 18:14:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rob Kurtz
-- Edited date: Mar 23 2010
-- Description:	This method adds/subtracts items to the ticket stock table based upon availability.
-- Tickets that are packages will be handled by keeping track of one base ticket
--
-- Client must supply a ticket id and a quantity to add/update. Quantity is amount to be added or subtracted.
-- If there are affected rows (@@rowcount) then we insert/update the stock table.
-- Otherwise we return the amount of tickets available to show the user. (this woujld be in the case of non-availability)
--
-- For Merch items, we are only concerned with making sure that the total number of items in the cart is valid
/*

exec [tx_Inventory_AddUpdate_Ticket]
	@guid = 'A34E3DA8-8FFE-41F7-B97F-A98FDBFDF9F4', @sessId='nosessionrightnow', @username = 'me', @idx = 10038,
	@qty = 1, @ttl = 'Feb  5 2007  8:37:58:080PM', @context = 'ticket'

exec [tx_Inventory_AddUpdate_Ticket]
	@guid = 'A34E3DA8-8FFE-41F7-B97F-A98FDBFDF9F4', @sessId='nosessionrightnow', @username = 'me', @idx = 12704,
	@qty = 1, @ttl = 'Feb  5 2007  8:37:58:080PM',

*/
-- =============================================

CREATE   PROC [dbo].[tx_Inventory_AddUpdate](

	@guid		UNIQUEIDENTIFIER,	--refers to the identifier for a sale item
	@sessId		VARCHAR(256),		--session id
	@userName	VARCHAR(256),		--
	@idx		INT,			        --the id of the showticket/merch to add
	@qty		INT,			        --qty of tix to add to pending
	@ttl		DATETIME,		      --how long to keep the items in stock before expiring
	@context	VARCHAR(50)		  --"merch" or "ticket"

)
AS

BEGIN

	DECLARE	@rowsAffected INT

	IF (@context = 'ticket')
	BEGIN

		CREATE TABLE #addTickets ( ticketIdx INT )

		INSERT	#addTickets(ticketIdx)
		SELECT	@idx

		--Also add linked tickets - ticket packages are linked
		INSERT	#addTickets(ticketIdx)
		SELECT	DISTINCT link.[LinkedShowTicketId]
		FROM	[ShowTicketPackageLink] link,
			    [ShowTicket] st
		WHERE	@idx = st.[Id]
			AND link.[ParentShowTicketId] = st.[Id]

		-- handle adds where item is already in ticket stock
		-- @idx is not needed here because it is already in the stock row matched by @guid
		IF EXISTS (SELECT * FROM [TicketStock] WHERE [GUID] = @guid AND @qty > 0)
		BEGIN

			-- update user name and time to live values - sessid and tix id should not need to be changed
			UPDATE	[TicketStock]
			SET	[iQty] = stock.[iQty] + @qty,
          [UserName] = ISNULL(@userName,''),
				  [dtTTL] = @ttl
			FROM	[ShowTicket] ent,
				  [TicketStock] stock
          LEFT OUTER JOIN (
            SELECT	SUM(CASE WHEN [iQty] <= 0 THEN 0 ELSE [iQty] END) AS iQty
            FROM	[TicketStock] stock, #addTickets at
            WHERE	stock.[tShowTicketId] = at.[ticketIdx]
          ) AS pendingStock
          ON (1 = 1)
			WHERE	stock.[GUID] = @guid
				AND stock.[tShowTicketId] = ent.[Id]
				AND ent.[bActive] = 1
				-- availabilility
				AND (ent.[iAllotment] - CASE WHEN pendingStock.[iQty] IS NULL THEN 0 ELSE pendingStock.[iQty] END - ent.[iSold]) >= @qty
				-- only update rows (adding) when active
				-- if the customer began the cart process before the ticket was deactivated and
				-- is trying to add more tickets after they have been deactivated - they will not be allowed to

			SET	@rowsAffected = @@ROWCOUNT

		END

		-- handle cart updates - removing inventory
		ELSE IF EXISTS (SELECT * FROM [TicketStock] WHERE [GUID] = @guid AND @qty < 0)
		BEGIN

			-- note that qty is subtracted to existing qty! Given qty is negative on removals
			-- update user name and time to live values - sessid and tix id should not need to be changed
			-- dont worry about ticket being active - always return inventory
			UPDATE	[TicketStock]
			SET	[iQty] = CASE WHEN [iQty] + @qty <= 0 THEN 0 ELSE [iQty] + @qty END,
				  [UserName] = ISNULL(@userName,''),
				  [dtTTL] = @ttl
			FROM	[TicketStock] stock
			WHERE	stock.[GUID] = @guid

			SET	@rowsAffected = @@ROWCOUNT

			--get rid of the row if it has been updated to zero
			DELETE	FROM [TicketStock]
			WHERE	[GUID] = @guid
				AND [iQty] <= 0

		END

		ELSE IF(@qty > 0)
		BEGIN

			-- @guid does not exist - create a new stock row
			INSERT	TicketStock( [GUID], [SessionId], [UserName], [tShowTicketId], [iQty], [dtTTL])

			SELECT	@guid			            AS 'GUID',
              @sessId			          AS 'SessionId',
              ISNULL(@userName,'')	AS 'UserName',
              ent.[Id]		          AS 'tShowTicketId',
              @qty			            AS 'iQty',
              @ttl			            AS 'dtTTL'
			FROM	[ShowTicket] ent
            LEFT OUTER JOIN (
              SELECT	SUM(CASE WHEN [iQty] <= 0 THEN 0 ELSE [iQty] END) AS iQty
              FROM	[TicketStock] stock, #addTickets at
              WHERE	stock.[tShowTicketId] = at.[ticketIdx]
            ) AS pendingStock
            ON (1 = 1)
			WHERE	ent.[Id] = @idx
				AND ent.[bActive] = 1
				--availabilility
				AND (ent.[iAllotment] - CASE WHEN pendingStock.[iQty] IS NULL THEN 0 ELSE pendingStock.[iQty] END - ent.[iSold]) >= @qty
				--only update rows (adding) when active
				--if the customer began the cart process before the ticket was deactivated and is trying to
				--add more tickets after they have been deactivated - they will not be allowed to

			SET	@rowsAffected = @@ROWCOUNT

		END

		--RETURN RESULTS

		--determine from @@rowsaffected if we were able to add/update rows
		IF(@rowsAffected > 0) BEGIN

			SELECT	'SUCCESS'

		END

		ELSE BEGIN

			-- this indicates that there is no availability and that new additions could not be added
			-- return current number of available tickets to client

			SELECT	CASE WHEN ent.[bActive] = 0 THEN '0'
              ELSE CAST(ent.[iAllotment] - ISNULL(pendingStock.[iQty],0) - ent.[iSold] AS VARCHAR)
              END
			FROM	[ShowTicket] ent
            LEFT OUTER JOIN (
              SELECT	SUM(CASE WHEN [iQty] <= 0 THEN 0 ELSE [iQty] END) AS iQty
              FROM	[TicketStock] stock, #addTickets at
              WHERE	stock.[tShowTicketId] = at.[ticketIdx]
            ) AS pendingStock
            ON (1 = 1)
			WHERE	ent.[Id] = @idx

		END

		--CLEANUP
		DROP TABLE [#addTickets]

	END --ticket context

	ELSE IF(@context = 'merch') BEGIN

		-- for merch, we just go by the total number of items they have in their cart
		--dont bother with pending
		IF EXISTS(
			SELECT	*
			FROM	[Merch] ent
			WHERE	ent.[Id] = @idx
				AND ent.[bActive] = 1
				-- availabilility
				AND (ent.[iAllotment] - ent.[iSold]) >= @qty )
		BEGIN

			SET	@rowsAffected = 1

		END

		--RETURN RESULTS

		--determine from @@rowsaffected if we were able to add/update rows
		IF(@rowsAffected > 0) BEGIN

			SELECT	'SUCCESS'

		END

		ELSE BEGIN

			-- this indicates that there is no availability and that new additions could not be added
			-- return current number of available tickets to client

			SELECT	CASE WHEN ent.[bActive] = 0 THEN '0'
				ELSE CAST(ent.[iAllotment] - ent.[iSold] AS VARCHAR)
				END
			FROM	[Merch] ent
			WHERE	ent.[Id] = @idx

		END

	END -- end of merch context

END --end procedure
GO