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
	<title>Monthly Mortgage Payment and Affordability Calculators.</title>
	
	<meta name="Description" content="Mortgage calculators to calculate your monthly mortgage payment and the affordability of a home.">

	<meta name="Keywords" content="Mortgage Calculator, Monthly Payments, Mortgage Payment, Affordability Calculator">

	<meta name="Robots" content="all">

	<link rel="shortcut icon" type="image/ico" href="/EdwardsIcon.ico" />
	<link rel="stylesheet" href="/includes/ce.css" type="text/css">
	<link rel="stylesheet" href="/includes/forms.css" type="text/css">
	<script language="JavaScript1.2" src="/includes/mortgage_calc.js"></script>
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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate1.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-seller.jpg" alt="Contact Chris Edwards for Home Seller Assistance in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Calculating Your Mortgage Payment</h1>
						
				<p>One of the first things that you will need to do is calculate what kind of home you can afford.  You will then be able to determine where to look for a new home and what features to expect in your new home.</p>
					
				<p>You will find two simple mortgage calculators below.  One for calculating your monthly payment and the other for determining how much you can afford.  Neither of these calculators include the total cost, including the closing costs, involved with getting a new home.</p>
				
				<p class="textblue">To see an accurate picture of your total costs we recommend the True Cost Calculator from Fannie Mae at:</p>

				<p class="center"><a href="http://www.fmcalcs.com/tools-tcc/fanniemae/calculator" target="_blank">http://www.fmcalcs.com/tools-tcc/fanniemae/calculator</a></p>

					<form id="form1" method="post" action="">
						<table cellpadding="0" cellspacing="0" class="mortgagecalc" border="1">
							<tbody>
								<tr>
									<th colspan="2" class="mortgagecalchead">
										Monthly Payment Calculator
									</th>
								</tr>
								<tr>
									<td class="mortgagecalcdata">
										Enter the Amount You Want to Borrow:
									</td>
									<td class="mortgagecalcdata2">
										<input type="text" name="borrow1" size="12" maxlength="12">
									</td>
								</tr>
								<tr>
									<td class="mortgagecalcdata">
										Enter the Number of Months to Repay Loan:
									</td>
									<td class="mortgagecalcdata2">
										<input type="text" name="months1" size="5" maxlength="5" value="360">
									</td>
								</tr>
								<tr>
									<td class="mortgagecalcdata">
										Enter the Estimated Loan Rate (APR):
									</td>
									<td class="mortgagecalcdata2">
										<input type="text" name="rate1" size="6" maxlength="6">
									</td>
								</tr>
								<tr>
									<td colspan="2" class="mortgagecalcdata">
										<input type="button" name="monthcalc" value="Calculate Monthly Payment" onclick="getpayment(this.form)">&nbsp;&nbsp;&nbsp;<input type="reset" name="reset1" value="Start Over">
									</td>
								</tr>
								<tr>
									<td class="mortgagecalcdata">
										<span class="bold">Your Monthly Payment:</span>
									</td>
									<td class="mortgagecalcdata2">
										<input type="text" name="payment1" size="12" maxlength="12">
									</td>
								</tr>
							</tbody>
						</table>
					</form>
					
					<form id="form2" method="post" action="">
						<table cellpadding="0" cellspacing="0" class="mortgagecalc" border="1">
							<tbody>
								<tr>
									<th colspan="2" class="mortgagecalchead">
										Affordability Calculator
									</th>
								</tr>
								<tr>
									<td class="mortgagecalcdata">
										Enter the Monthly Payment Amount You Want to Make Each Month:
									</td>
									<td class="mortgagecalcdata2">
										<input type="text" name="borrow22" size="12" maxlength="12">
									</td>
								</tr>
								<tr>
									<td class="mortgagecalcdata">
										Enter the Number of Months to Repay Loan:
									</td>
									<td class="mortgagecalcdata2">
										<input type="text" name="months22" size="5" maxlength="5" value="360">
									</td>
								</tr>
								<tr>
									<td class="mortgagecalcdata">
										Enter the Estimated Loan Rate (APR):
									</td>
									<td class="mortgagecalcdata2">
										<input type="text" name="rate22" size="6" maxlength="6">
									</td>
								</tr>
								<tr>
									<td colspan="2" class="mortgagecalcdata">
										<input type="button" name="affcalc" value="Calculate Loan Amount" onclick="getpayment2(this.form)">&nbsp;&nbsp;&nbsp;<input type="reset" name="reset2" value="Start Over">
									</td>
								</tr>
								<tr>
									<td class="mortgagecalcdata">
										<span class="bold">Loan Amount to Borrow:</span>
									</td>
									<td class="mortgagecalcdata2">
										<input type="text" name="payment22" size="12" maxlength="12">
									</td>
											</tr>
										</tbody>
									</table>
								</form>
							
								<!-- end mortgage calculator -->
						
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
