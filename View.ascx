<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Gafware.Modules.Glossary.View" %>
<%@ Register Src="~/desktopmodules/Gafware/Glossary/LetterFilter.ascx" TagPrefix="dnn" TagName="LetterFilter" %>
<%@ Register Src="~/desktopmodules/Gafware/Glossary/Icons.ascx" TagPrefix="dnn" TagName="Icons" %>
<div class="glossarybox">
    <div class="TopIconBar" runat="server" visible="false" id="topIconBar">
        <dnn:Icons runat="server" id="icons" />
    </div>
    <asp:Panel ID="pnlSearch" runat="server" style="clear:both">
        <div class="title dnnClear">
            <asp:Label ID="lblSearchTerm" runat="server" ResourceKey="lblSearchTerm" CssClass="NormalBold"></asp:Label>
        </div>
        <div class="dnnFormItem">
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="mFilterTextBox" CssClass="NormalTextBox" runat="server"></asp:TextBox>
                    </td>
                    <td id="rowIn" runat="server" style="padding-bottom: 15px;">
                        <asp:Label ID="lblIn" runat="server" ResourceKey="lblIn" CssClass="Normal"></asp:Label>
                    </td>
                    <td id="rowCategory" runat="server">
                        <asp:DropDownList ID="mCategoryDropDownList" runat="server" DataTextField="CategoryName" DataValueField="GlossaryCategoryDefinitionId" AutoPostBack="true" OnSelectedIndexChanged="mCategoryDropDownList_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:LinkButton ID="mSearchButton" CssClass="dnnPrimaryAction" ResourceKey="mSearchButton" runat="server" OnClick="mSearchButton_Click">SEARCH</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="title">
            <asp:Label ID="lblSelectLetter" runat="server" ResourceKey="lblSelectLetter" CssClass="NormalBold"></asp:Label>
        </div>
        <div class="letterbtn">
            <dnn:LetterFilter runat="server" id="letterFilter" OnClick="letterFilter_Click" />
        </div>
        <br />
        <div class="glossarylist">
            <asp:Repeater ID="rptGlossary" runat="server" OnItemDataBound="rptGlossary_ItemDataBound">
                <ItemTemplate>
                    <div id="header" runat="server" class="glossaryheader" visible="false"></div>
                    <asp:LinkButton ID="lnkDetails" CommandArgument='<%# Eval("GlossaryId") %>' CommandName="View" OnCommand="lnkDetails_Command" runat="server"><%# Eval("Term") %></asp:LinkButton><br />
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlDetails" runat="server" Visible="false">
        <asp:HyperLink ID="lnkBack2" runat="server" NavigateUrl="javascript:history.go(-1);" Visible="false"><< Back</asp:HyperLink>
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click"><< Back</asp:LinkButton>
        <br />
        <div style="float: left; width: 70%">
            <h3><asp:Label ID="lblTitle" runat="server"></asp:Label></h3>
            <asp:Label ID="lblDefinition" runat="server" CssClass="qtip-glossary"></asp:Label>
        </div>
        <div style="float: right; margin-left: 5px; width: 25%">
            <div class="box" id="divAKA" runat="server">
                <h3>Also Known As</h3>
                <asp:Repeater ID="rptAltTerms" runat="server">
                    <ItemTemplate>
                        <%# Eval("Term") %><br />
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <br />
            <div class="box" id="divAppearsOn" runat="server">
                <h3>Appears On</h3>
                <asp:Repeater ID="rptPages" runat="server">
                    <ItemTemplate>
                        <a href='<%# Eval("Url") %>'><%# Eval("Title") %></a><br />
                    </ItemTemplate>
                </asp:Repeater>
            </div>
			<br />
            <asp:LinkButton ID="btnGlossary" runat="server" OnClick="lnkBack_Click" CssClass="primaryButton" Width="212px">See Full Glossary</asp:LinkButton>
        </div>
    </asp:Panel>
</div>
