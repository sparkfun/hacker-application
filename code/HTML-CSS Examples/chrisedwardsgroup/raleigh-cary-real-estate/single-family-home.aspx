<%@ Page Debug="False" Trace="False" EnableViewState="True" Inherits="ChrisEdwardsGroup.Website.SingleFamilyHome" %>

<%@ Register Tagprefix="Top" Tagname="FeaturedProperty" Src="/includes/featured.ascx" %>
<%@ Register Tagprefix="Top" Tagname="TopLinks" Src="/includes/toplinks.ascx" %>
<%@ Register Tagprefix="Top" Tagname="LeftLinks" Src="/includes/leftlinks.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Address" Src="/includes/address.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Neighborhoods" Src="/includes/neighborhoods.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Logos" Src="/includes/logos.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
	<title>Single Family Home: Homes in Raleigh, Cary and Apex, North Carolina</title>
	
	<meta name="Description" content="Single family homes for sale in Raleigh, Cary and Apex, North Carolina.">

	<meta name="Keywords" content="Raleigh Real Estate,Raleigh Homes,Cary Homes,Apex Homes,North Carolina Homes,Single Family Homes">

	<meta name="Robots" content="all">

	<link rel="shortcut icon" type="image/ico" href="/EdwardsIcon.ico" />
	<link rel="stylesheet" href="/includes/ce.css" type="text/css">
	<link rel="stylesheet" href="/includes/listings.css" type="text/css">
	<script language="JavaScript" type="text/javascript" src="/includes/extras.js"></script>
	<script type="text/javascript">
    
      var _gaq = _gaq || [];
      _gaq.push(['_setAccount', 'UA-2016415-2']);
      _gaq.push(['_trackPageview']);
    
      (function() {
        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
      })();
    
    </script>
</head>

<body>

<!-- start header and toplinks -->
<Top:TopLinks id="ctlTopLinks" runat="server" />
<!-- end header and toplinks -->
	
	<tr>
		
		<!-- start left link column -->
		<Top:LeftLinks id="ctlLeftLinks" runat="server" />
		<!-- end left link column -->
		
		<!-- start right column -->
		<td id="right">
			
			<!-- start flash & featured listing -->
				<table border="0">
					<tr>
						
						<!-- start flash or photo -->
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate1.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-welcome-triangle.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
						<!-- end flash or photo -->
						
						<!-- featured listing -->
						<td>
							
							<Top:FeaturedProperty id="featProperty" runat="server" />
						
						</td>
						<!-- end listing -->
						
					</tr>
				</table>
				
			<!-- end flash & featured listing -->
			
			<!-- start text -->
			
				<h1 class="body">Single Family Home in Raleigh</h1>
					
				<h2>Featured Raleigh Real Estate and Homes</h2>
					
				<asp:Label id="lblOutError" visible="false" runat="server" />
					
				<asp:Repeater id="rprSingleFamilyHome" OnItemDataBound="SingleFamilyHome_ItemDataBound" runat="server">

			   <ItemTemplate>
							
							<h2>
								<%# DataBinder.Eval(Container.DataItem, "Tagline") %> - <%# DataBinder.Eval(Container.DataItem, "Price", "{0:C}") %>
							</h2>
							
							<table cellspacing="0" cellpadding="0" border="0" class="listingphoto">
								<tbody>
									<tr>
										<td rowspan="4">
											<asp:Image id="imgFull" CssClass="lgphoto" runat="server" />
										</td>
										<td>
											<a href="" onclick="return ContactAbout('contact_about.aspx?MLSID=<%# DataBinder.Eval(Container.DataItem, "MLS") %>&amp;propertyURL=residential.aspx')"><img src="/images/contact.gif" alt="" width="75" height="75" border="0" /></a>
										</td>
									</tr>
									<tr>
										<td>
											<a href="javascript:void(0)" onclick="javascript:PrintContents();"><img src="/images/print.gif" alt="" width="75" height="75" border="0" /></a>
										</td>
									</tr>
									<tr>
										<td>
											<a href="javascript:void(0)" onclick="return EmailAFriend('email_a_friend.aspx?MLSID=<%# DataBinder.Eval(Container.DataItem, "MLS") %>&amp;propertyURL=residential.aspx')"><img src="/images/emailfriend.gif" alt="" width="75" height="75" border="0" /></a>
										</td>
									</tr>
									<tr>
										<td>
											<a href="http://www.mapquest.com/maps/map.adp?country=US&addtohistory=&address=<%# DataBinder.Eval(Container.DataItem, "Address1") %>&city=<%# DataBinder.Eval(Container.DataItem, "CityName") %>&state=NC&zipcode=<%# DataBinder.Eval(Container.DataItem, "ZipCode") %>&homesubmit=Get+Map" target="_blank"><img src="/images/map.gif" alt="" width="75" height="75" border="0" /></a>
										</td>
									</tr>
								</tbody>
							</table>
							
							<asp:Table Id="virtualTour" Visible="false" CssClass="vtour" RunAt="server" />
							
							<asp:datalist ID="photoGallery" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="5" CssClass="addphotos" runat="server">
								
								<HeaderStyle CssClass="header"></HeaderStyle>

							  <HeaderTemplate>
							  	Additional Photos (click to view larger photos)
							  </HeaderTemplate>
							  <ItemTemplate>
									<a href="javascript:void(0)" onclick="return ExtraPhoto('photo_details.aspx?PictureID=<%# DataBinder.Eval(Container.DataItem, "PictureID") %>')"><img src="/listings/<%# DataBinder.Eval(Container.DataItem, "MLS") %>/<%# DataBinder.Eval(Container.DataItem, "ThumbnailPath") %>" alt="<%# DataBinder.Eval(Container.DataItem, "Comments") %>" width="125" height="125" border="1" /></a>
							  </ItemTemplate>
							</asp:datalist>

			   			<table summary="Property Features" border="0" class="listingdetails">
								<tbody>
									<tr>
										<th colspan="3" class="title">
											Property Information
										</th>
									</tr>
									<tr>
										<th>
											MLS:
										</th>
										<th>
											Price:
										</th>
										<th>
											Owner:
										</th>
									</tr>
									<tr>
										<td>
											<asp:Literal id="ltlMLS" runat="server" />
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Price", "{0:C}") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Owner") %>
										</td>
									</tr>
								</tbody>
							</table>
							
							<table summary="Property Features" border="0" class="listingdetails">
								<tbody>
									<tr>
										<th colspan="3" class="title">
											Address
										</th>
									</tr>
									<tr>
										<th>
											Address:
										</th>
										<th>
											City:
										</th>
										<th>
											Subdivision:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Address1") %><br />
											<%# DataBinder.Eval(Container.DataItem, "Address2") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "CityName") %>, NC  <%# DataBinder.Eval(Container.DataItem, "ZipCode") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Subdivision") %>
										</td>
									</tr>
								</tbody>
							</table>
							
							<table summary="Property Features" border="0" class="listingdetails">
								<tbody>
									<tr>
										<th colspan="3" class="title">
											Property Features
										</th>
									</tr>
									<tr>
										<th>
											Bedrooms:
										</th>
										<th>
											Bathrooms:
										</th>
										<th>
											Square Feet:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Bedrooms") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Baths") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "SquareFt") %>+/-
										</td>
									</tr>
									<tr>
										<th>
											Year Built:
										</th>
										<th>
											Year Remodeled:
										</th>
										<th>
											Parcel Size:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "YearBuilt") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "YearRemodeled") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "ParcelSize") %>
										</td>
									</tr>
									<tr>
										<th>
											Style:
										</th>
										<th>
											Foundation:
										</th>
										<th>
											Construction:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Style") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Foundation") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Construction") %>
										</td>
									</tr>
									<tr>
										<th>
											Roof:
										</th>
										<th>
											Garage:
										</th>
										<th>
											Patio:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Roof") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Garage") %>
										</td>
										<td>
											<%# ConvertTrueFalse((bool)DataBinder.Eval(Container.DataItem, "Patio")) %>
										</td>
									</tr>
									<tr>
										<th>
											Deck:
										</th>
										<th>
											Fenced:
										</th>
										<th>
											Fencing Description:
										</th>
									</tr>
									<tr>
										<td>
											<%# ConvertTrueFalse((bool)DataBinder.Eval(Container.DataItem, "Deck")) %>
										</td>
										<td>
											<%# ConvertTrueFalse((bool)DataBinder.Eval(Container.DataItem, "Fenced")) %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "FencingDescription") %>
										</td>
									</tr>
								</tbody>
							</table>
							
							<table summary="Property Features" border="0" class="listingdetails">
								<tbody>
									<tr>
										<th colspan="3" class="title">
											Utilities
										</th>
									</tr>
									<tr>
										<th>
											Heating:
										</th>
										<th>
											Fireplace:
										</th>
										<th>
											Woodstove/Pellet Stove:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Heating") %>
										</td>
										<td>
											<%# ConvertTrueFalse((bool)DataBinder.Eval(Container.DataItem, "Fireplace")) %>
										</td>
										<td>
											<%# ConvertTrueFalse((bool)DataBinder.Eval(Container.DataItem, "Woodstove")) %>
										</td>
									</tr>
									<tr>
										<th colspan="3">
											Sewer:
										</th>
									</tr>
									<tr>
										<td colspan="3">
											<%# DataBinder.Eval(Container.DataItem, "Sewer") %>
										</td>
									</tr>
								</tbody>
							</table>
							
							<table summary="Property Features" border="0" class="listingdetails">
								<tbody>
									<tr>
										<th colspan="3" class="title">
											Room Dimensions
										</th>
									</tr>
									<tr>
										<th>
											Kitchen:
										</th>
										<th>
											Living Room:
										</th>
										<th>
											Dining Room:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "KitchenDim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "LivingRoomDim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "DiningRoomDim") %>
										</td>
									</tr>
									<tr>
										<th colspan="3">
											Family Room:
										</th>
									</tr>
									<tr>
										<td colspan="3">
											<%# DataBinder.Eval(Container.DataItem, "FamilyRoomDim") %>
										</td>
									</tr>
								</tbody>
							</table>
							
							<table summary="Property Features" border="0" class="listingdetails">
								<tbody>
									<tr>
										<th colspan="3" class="title">
											Bedroom Dimensions
										</th>
									</tr>
									<tr>
										<th>
											Master Bedroom:
										</th>
										<th>
											Bedroom #2:
										</th>
										<th>
											Bedroom #3:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "MasterBedDim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Bedroom2Dim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Bedroom3Dim") %>
										</td>
									</tr>
									<tr>
										<th colspan="3">
											Bedroom #4:
										</th>
									</tr>
									<tr>
										<td colspan="3">
											<%# DataBinder.Eval(Container.DataItem, "Bedroom4Dim") %>
										</td>
									</tr>
								</tbody>
							</table>
							
							<table summary="Property Features" border="0" class="listingdetails">
								<tbody>
									<tr>
										<th colspan="3" class="title">
											Bathroom Dimensions
										</th>
									</tr>
									<tr>
										<th>
											Bathroom #1:
										</th>
										<th>
											Bathroom #2:
										</th>
										<th>
											Bathroom #3:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Bathroom1Dim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Bathroom2Dim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "Bathroom3Dim") %>
										</td>
									</tr>
									<tr>
										<th colspan="3">
											Bathroom #4:
										</th>
									</tr>
									<tr>
										<td colspan="3">
											<%# DataBinder.Eval(Container.DataItem, "Bathroom4Dim") %>
										</td>
									</tr>
								</tbody>
							</table>
							
							<table summary="Property Features" border="0" class="listingdetails">
								<tbody>
									<tr>
										<th colspan="3" class="title">
											Other Dimensions
										</th>
									</tr>
									<tr>
										<th>
											Basement Dimensions:
										</th>
										<th>
											Garage Dimensions:
										</th>
										<th>
											Patio Dimensions:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "BasementDim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "GarageDim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "PatioDim") %>
										</td>
									</tr>
									<tr>
										<th>
											Deck Dimensions:
										</th>
										<th>
											Shed Dimensions:
										</th>
										<th>
											Office Dimensions:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "DeckDim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "ShedDim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "OfficeDim") %>
										</td>
									</tr>
									<tr>
										<th>
											Media Room Dimensions:
										</th>
										<th>
											Laundry Room Dimensions:
										</th>
										<th>
											Sunroom Dimensions:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "MediaRoomDim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "LaundryRoomDim") %>
										</td>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "SunroomDim") %>
										</td>
									</tr>
								</tbody>
							</table>
							
							<table summary="Property Features" border="0" class="listingdetails">
								<tbody>
									<tr>
										<th class="relisttitle">
											Description
										</th>
									</tr>
									<tr>
										<th>
											Features Description:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "FeaturesDescription") %>
										</td>
									</tr>
									<tr>
										<th>
											Inclusions Description:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "InclusionsDescription") %>
										</td>
									</tr>
									<tr>
										<th>
											Exclusions Description:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "ExclusionsDescription") %>
										</td>
									</tr>
									<tr>
										<th>
											Outbuildings Description:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "OutbuildingsDescription") %>
										</td>
									</tr>
									<tr>
										<th>
											Disclosures Description:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "DisclosuresDescription") %>
										</td>
									</tr>
								</tbody>
							</table>
							
							<table summary="Property Features" border="0" class="listingdetails">
								<tbody>
									<tr>
										<th class="relisttitle">
											Map Directions
										</th>
									</tr>
									<tr>
										<th>
											Map Directions to Property:
										</th>
									</tr>
									<tr>
										<td>
											<%# DataBinder.Eval(Container.DataItem, "MapDirections") %>
										</td>
									</tr>
								</tbody>
							</table>
			   </ItemTemplate>
					
					</asp:Repeater>
								
				<!-- end text -->
			
				<!-- start address -->
				<Middle:Address id="ctlAddress" runat="server" />
				<!-- end address -->
				
				<!-- start neighborhoods -->
				<Middle:Neighborhoods id="ctlNeighborhoods" runat="server" />
				<!-- end neighborhoods -->
				
		</td>
		<!-- end right column -->
	
	</tr>
</table>

<!-- start logos -->
<Middle:Logos id="ctlLogos" runat="server" />
<!-- end logos -->

<!-- start footer -->
<Bottom:Footer id="ctlFooter" runat="server" />
<!-- end footer -->

</body>
</html>
