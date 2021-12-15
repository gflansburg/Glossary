<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCategory.ascx.cs" Inherits="Gafware.Modules.Glossary.EditCategory" %>
<%@ Register Assembly="Gafware.Glossary" Namespace="Gafware.Modules.Glossary" TagPrefix="cc1" %>
<fieldset>
    <cc1:GridViewExtended ID="mCategoryGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="GlossaryCategoryDefinitionId"
        OnRowEditing="mCategoryGridView_RowEditing" RowStyle-BackColor="#eeeeee" ShowFooterWhenEmpty="True" ShowHeaderWhenEmpty="True"
        RowStyle-Height="18" HeaderStyle-Height="30" GridLines="None" Font-Names="Arial" Font-Size="Small" CellPadding="4" ShowFooter="True" 
        OnRowCommand="mCategoryGridView_RowCommand" ForeColor="#333333" OnRowUpdating="mCategoryGridView_RowUpdating" 
        OnRowCancelingEdit="mCategoryGridView_RowCancelingEdit" OnRowDataBound="mCategoryGridView_RowDataBound" OnRowDeleting="mCategoryGridView_RowDeleting">
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle VerticalAlign="Top" Font-Names="Arial" Font-Size="Small" BackColor="#EFF3FB" />
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle Font-Names="Arial" Font-Size="Small" BackColor="#FFFFFF" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Font-Size="Small" Font-Names="Arial" ForeColor="White" Font-Bold="True" />
        <AlternatingRowStyle BackColor="White" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"> 
                <ItemTemplate>
                    <asp:ImageButton ID="editButton" runat="server" ImageUrl="~/DesktopModules/Gafware/Glossary/images/EditIcon1_16px.gif" AlternateText="Edit Category" ToolTip="Edit Category" CommandName="Edit" Text="Edit" CausesValidation="false" /> 
                    <asp:ImageButton ID="deleteButton" runat="server" ImageUrl="~/DesktopModules/Gafware/Glossary/images/DeleteIcon1_16px.gif" AlternateText="Delete Category" ToolTip="Delete Category" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you wish to delete this category?');" CausesValidation="false" /> 
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:ImageButton ID="saveButton" runat="server" ImageUrl="~/DesktopModules/Gafware/Glossary/images/save.gif" AlternateText="Save Category" ToolTip="Save Category" CommandName="Update" Text="Update" CausesValidation="true" /> 
                    <asp:ImageButton ID="cancelButton" runat="server" ImageUrl="~/DesktopModules/Gafware/Glossary/images/DeleteIcon1_16px.gif" AlternateText="Cancel Edit" ToolTip="Cancel Edit" CommandName="Cancel" Text="Cancel" CausesValidation="false" /> 
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:ImageButton ID="newButton" runat="server" ImageUrl="~/DesktopModules/Gafware/Glossary/images/NewIcon1_16px.gif" AlternateText="New Category" ToolTip="New Category" CommandName="New" Text="New" CausesValidation="false" /> 
                    <asp:ImageButton ID="saveButton2" runat="server" ImageUrl="~/DesktopModules/Gafware/Glossary/images/save.gif" AlternateText="Save Category" ToolTip="Save Category" CommandName="Insert" Text="Insert" Visible="false" CausesValidation="true" /> 
                    <asp:ImageButton ID="cancelButton2" runat="server" ImageUrl="~/DesktopModules/Gafware/Glossary/images/DeleteIcon1_16px.gif" AlternateText="Cancel Insert" ToolTip="Cancel Insert" CommandName="Cancel" Text="Cancel" Visible="false" CausesValidation="false" /> 
                </FooterTemplate>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="Category Name" ItemStyle-Wrap="false" ItemStyle-Width="100px">
                <ItemTemplate>
                    <%# Eval("CategoryName") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbCategoryName" runat="server" Text='<%# Eval("CategoryName") %>' MaxLength="50" Width="100%"></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="tbCategoryName2" runat="server" Text='<%# Eval("CategoryName") %>' MaxLength="50" Width="100%"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridViewExtended>
</fieldset>
<br />
<div class="CloseEditModeLink">
	<asp:LinkButton ID="cmdCloseEdit" runat="server" Text="Close" OnClick="cmdCloseEdit_Click" CssClass="dnnPrimaryAction" />
</div>
