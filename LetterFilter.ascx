<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LetterFilter.ascx.cs" Inherits="Gafware.Modules.Glossary.LetterFilter" %>
<table cellspacing="0" CellPadding="3" CellSpacing="3" width="100%" border="0">
    <tr>
        <td>
            <asp:repeater id="rptLetters" runat="server" OnItemCommand="rptLetters_ItemCommand" OnItemDataBound="rptLetters_ItemDataBound">
                <itemtemplate>
                    <asp:linkbutton id="lnkLetter" runat="server" CssClass="linkButton" onmouseover="this.style.textDecoration ='underline';" onmouseout="this.style.textDecoration='none';" commandname="Filter" commandargument='<%# DataBinder.Eval(Container, "DataItem.Letter")%>'>
                        <%# DataBinder.Eval(Container, "DataItem.Letter")%>
                    </asp:linkbutton>
                    <%# GetSpacer(Convert.ToInt32(Container.ItemIndex)) %>
                </itemtemplate>
            </asp:repeater>
        </td>
    </tr>
</table>