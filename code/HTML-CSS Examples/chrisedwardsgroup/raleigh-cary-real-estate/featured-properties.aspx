<%@ Page Debug="False" Trace="False" EnableViewState="True" Inherits="ChrisEdwardsGroup.Website.BrowseSingleFamilyHome" %>

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
	<title>Featured Raleigh Real Estate and Homes</title>
	
	<meta name="Description" content="Featured Raleigh Real Estate, Homes and Properties in North Carolina.">

	<meta name="Keywords" content="Raleigh Real Estate, Raleigh Homes, Raleigh Properties, Cary Properties, Apex Properties, Cary Homes, Apex Homes, North Carolina Properties">

	<meta name="Robots" content="all">

	<link rel="shortcut icon" type="image/ico" href="/EdwardsIcon.ico" />
	<link rel="stylesheet" href="/includes/ce.css" type="text/css">
	<link rel="stylesheet" href="/includes/listings.css" type="text/css">
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
			
					<h1 class="body">
						Featured Raleigh Real Estate and Homes
					</h1>
	
					<asp:Label id="lblStatus" visible="False" runat="server" />
										
					<p>
						<asp:Label id="lblRecordCount" runat="server" />
					</p>
					
					<asp:DataList id="dtlBrowseSingleFamilyHome" ExtractTemplateRows="False" GridLines="None" RepeatLayout="Table" ShowFooter="False" ShowHeader="False" OnItemCreated="BrowseSingleFamilyHome_ItemCreated" runat="server">
			
			   		<ItemTemplate>
							<asp:Table id="tblResItem" CssClass="feathomes" GridLines="None" runat="server">
							
								<asp:TableRow>
								
									<asp:TableCell RowSpan="3" CssClass="thmimg">
										<a href="single-family-home.aspx?MLSID=<%# DataBinder.Eval(Container.DataItem, "MLS") %>"><asp:Image id="imgThumbnail" runat="server" /></a>
			      </asp:TableCell>
									<asp:TableCell ColumnSpan="2">
										<span class="bold">
											<%# DataBinder.Eval(Container.DataItem, "Tagline") %>
										</span>
			      </asp:TableCell>
									
								</asp:TableRow>
								
								<asp:TableRow>
								
									<asp:TableCell ColumnSpan="2">
										<%# DataBinder.Eval(Container.DataItem, "FeaturesDescription") %>
			      </asp:TableCell>
									
								</asp:TableRow>
								
								<asp:TableRow>
								
									<asp:TableCell>
										<%# DataBinder.Eval(Container.DataItem, "SquareFt") %> Sq. Ft.
			      </asp:TableCell>
									
									<asp:TableCell>
										<%# DataBinder.Eval(Container.DataItem, "Bedrooms") %>Bed, <%# DataBinder.Eval(Container.DataItem, "Baths") %>Bath
			      </asp:TableCell>
									
								</asp:TableRow>
								
								<asp:TableRow>
								
									<asp:TableCell ColumnSpan="3">
										<span class="bold">
											<%# DataBinder.Eval(Container.DataItem, "Price", "{0:C}") %>
										</span>
			      			</asp:TableCell>
									
								</asp:TableRow>
								
								<asp:TableRow>
								
									<asp:TableCell>
									<%# DataBinder.Eval(Container.DataItem, "Address1") %> <br />
									<%# DataBinder.Eval(Container.DataItem, "CityName") %>, NC <%# DataBinder.Eval(Container.DataItem, "ZipCode") %>
			      			</asp:TableCell>
									
									<asp:TableCell ColumnSpan="2">
										<img src="/images/mag.gif" alt="" width="12" height="13" border="0" />&nbsp;&nbsp;<a href="single-family-home.aspx?MLSID=<%# DataBinder.Eval(Container.DataItem, "MLS") %>">View full property details...</a>
			      			</asp:TableCell>
									
								</asp:TableRow>
								
							</asp:Table>
							
						</ItemTemplate>
						
					</asp:DataList>
								
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
