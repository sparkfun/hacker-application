<%@ Control Language="C#" Debug="False" Inherits="ChrisEdwardsGroup.ListManage.TopLogin" %>

	<td><img src="graphics/chrisedwardslogo_trans.gif" alt="Chris Edwards Group" width="150" height="72" border="0"></td>

	<td class="heading">Chris Edwards Group<br /> <span class="cadetblue">Management Portal</span></td>

	<td class="padleft">Logged in as <asp:Label id="lblUser" CssClass="userhighlight" runat="server"/>&nbsp;&nbsp;|&nbsp;&nbsp;<asp:LinkButton id="lbtnLogout" CssClass="logout" Text="Logout" CausesValidation="False" OnClick="DoSignOut" runat="server" />&nbsp;&nbsp;|&nbsp;&nbsp;Help</td>