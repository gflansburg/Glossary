<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditGlossary.ascx.cs" Inherits="Gafware.Modules.Glossary.EditGlossary" %>
<%@ Register Assembly="Gafware.Glossary" Namespace="Gafware.Modules.Glossary" TagPrefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div class="glossarybox">
    <asp:Panel ID="pnlGrid" runat="server">
        <fieldset>
            <cc1:GridViewExtended ID="mTermGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="GlossaryId" OnRowEditing="mTermGridView_RowEditing"
                RowStyle-BackColor="#eeeeee" ShowFooterWhenEmpty="True" ShowHeaderWhenEmpty="True" OnRowDeleting="mTermGridView_RowDeleting"
                RowStyle-Height="18" HeaderStyle-Height="30" GridLines="None" Font-Names="Arial" Font-Size="Small" CellPadding="4" ShowFooter="True" 
                ForeColor="#333333" Width="100%" OnRowCommand="mTermGridView_RowCommand">
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle VerticalAlign="Top" Font-Names="Arial" Font-Size="Small" BackColor="#EFF3FB" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle Font-Names="Arial" Font-Size="Small" BackColor="#FFFFFF" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Font-Size="Small" Font-Names="Arial" ForeColor="White" Font-Bold="True" />
                <AlternatingRowStyle BackColor="White" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderText="&nbsp;"> 
                        <ItemTemplate>
                            <asp:ImageButton ID="editButton" runat="server" ImageUrl="~/DesktopModules/Gafware/Glossary/images/EditIcon1_16px.gif" AlternateText="Edit Term" ToolTip="Edit Term" CommandName="Edit" Text="Edit" CausesValidation="false" /> 
                            <asp:ImageButton ID="deleteButton" runat="server" ImageUrl="~/DesktopModules/Gafware/Glossary/images/DeleteIcon1_16px.gif" AlternateText="Delete Term" ToolTip="Delete Term" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you wish to delete this term?');" CausesValidation="false" /> 
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:ImageButton ID="newButton" runat="server" ImageUrl="~/DesktopModules/Gafware/Glossary/images/NewIcon1_16px.gif" AlternateText="New Term" ToolTip="New Term" CommandName="New" Text="New" CausesValidation="false" /> 
                        </FooterTemplate>
                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                        <FooterStyle Width="100px" HorizontalAlign="Center" />
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Term" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("Term") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Category" ItemStyle-Wrap="false" HeaderStyle-Width="45%" ItemStyle-Width="45%" FooterStyle-Width="45%">
                        <ItemTemplate>
                            <%# GetCategories((int)Eval("GlossaryId")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </cc1:GridViewExtended>
        </fieldset>
        <br />
        <div class="divCommands" class="dnnClear commands">
            <ol class="olCommands">
                <li><asp:LinkButton ID="cmdCloseEdit" runat="server" Text="Close" OnClick="cmdCloseEdit_Click" CssClass="dnnPrimaryAction" /></li>
            </ol>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <fieldset>
            <div class="dnnFormItem">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="middle" style="padding-bottom: 15px;"><dnn:Label ID="lblTerm" runat="server" ResourceKey="lblTerm" ControlName="txtTerm" Suffix=":" /></td>
                        <td><asp:TextBox ID="txtTerm" runat="server" MaxLength="100" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="Glossary" /></td>
                    </tr>
                </table>
                <asp:RequiredFieldValidator ID="rfvTerm" runat="server" ControlToValidate="txtTerm" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ErrorMessage="Please enter a new term." ValidationGroup="Glossary" />
            </div>
            <div class="dnnFormItem">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top"><dnn:Label ID="lblDefinition" runat="server" ResourceKey="lblDefinition" ControlName="txtDefinition" Suffix=":" /></td>
                        <td><dnn:texteditor ID="txtDefinition" runat="server" height="418px" ValidationGroup="Glossary" /></td>
                    </tr>
                </table>
                <asp:RequiredFieldValidator ID="rfvDefinition" runat="server" ControlToValidate="txtDefinition" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ErrorMessage="Term definition is required." ValidationGroup="Glossary" />
            </div>
            <div class="dnnFormItem">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top"><dnn:Label ID="lblCategory" runat="server" ResourceKey="lblCategory" ControlName="lstCategory" Suffix=":" /></td>
                        <td><asp:CheckBoxList ID="lstCategory" DataTextField="CategoryName" DataValueField="GlossaryCategoryDefinitionId" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" ValidationGroup="Glossary"></asp:CheckBoxList></td>
                    </tr>
                </table>
                <asp:CustomValidator runat="server" ID="cvCategory" ClientValidationFunction="ValidateCategoryList" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ErrorMessage="Please select at least one category." ValidationGroup="Glossary" />
            </div>
            <div class="dnnFormItem">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top"><dnn:Label ID="lblAltTerms" runat="server" ResourceKey="lblAltTerms" ControlName="txtAltTerms" Suffix=":" /></td>
                        <td>
                            <cc1:GridViewExtended ID="gvAltTerms" AutoGenerateColumns="False" runat="server" OnRowDeleting="gvAltTerms_RowDeleting" OnRowCommand="gvAltTerms_RowCommand" 
                                Width="100%" ShowFooterWhenEmpty="True" ShowHeaderWhenEmpty="True" GridLines="None" Font-Names="Arial" Font-Size="Small" CellPadding="4" ShowFooter="True" 
                                DataKeyNames="AltId" ForeColor="#333333">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="50px" HeaderText="&nbsp;" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="deleteButton" runat="server" AlternateText="Delete Term" CausesValidation="false" CommandName="Delete" ImageUrl="~/DesktopModules/Gafware/Glossary/images/DeleteIcon1_16px.gif" OnClientClick="return confirm('Are you sure you wish to delete this term?');" Text="Delete" ToolTip="Delete Term" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="newButton" runat="server" AlternateText="New Term" CausesValidation="true" CommandName="New" ImageUrl="~/DesktopModules/Gafware/Glossary/images/NewIcon1_16px.gif" Text="New" ToolTip="New Term" ValidationGroup="GlossaryAlt" />
                                        </FooterTemplate>
                                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        <FooterStyle Width="50px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="200px" HeaderText="Term" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <%# Eval("Term") %>
                                            <asp:HiddenField id="hidTerm" value='<%# Eval("Term") %>' runat="server"></asp:HiddenField>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAltTerm" runat="server" CssClass="NormalTextBox dnnFormRequired" MaxLength="100" ValidationGroup="GlossaryAlt"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAltTerms" runat="server" ControlToValidate="txtAltTerm" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ErrorMessage="Alternate term definition is required." ValidationGroup="GlossaryAlt" />
                                        </FooterTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                        <ItemStyle Width="200px" HorizontalAlign="Left" />
                                        <FooterStyle Width="200px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </cc1:GridViewExtended>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
        <br />
        <div class="divCommands" class="dnnClear commands">
            <ol class="olCommands">
                <li><asp:LinkButton ID="cmdSave" runat="server" Text="Save" OnClick="cmdSave_Click" CssClass="dnnPrimaryAction" ValidationGroup="Glossary" CausesValidation="true" /></li>
                <li><asp:LinkButton ID="cmdCancelEdit" runat="server" Text="Cancel" OnClick="cmdCancelEdit_Click" CssClass="dnnSecondaryAction" CausesValidation="false" /></li>
            </ol>
        </div>
    </asp:Panel>
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/

    function ValidateCategoryList(source, args) {
        var chkListModules = document.getElementById('<%= lstCategory.ClientID %>');
        var chkListinputs = chkListModules.getElementsByTagName("input");
        for (var i = 0; i < chkListinputs.length; i++) {
            if (chkListinputs[i].checked) {
                args.IsValid = true;
                return;
            }
        }
        args.IsValid = false;
    }

    (function ($, Sys) {
        function setupGlossarySiteSettings() {
            jQuery('select, input:text').jqTransform();
        }

        $(document).ready(function () {
            setupGlossarySiteSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupGlossarySiteSettings();
            });
        });

    }(jQuery, window.Sys));
    /*]]>*/
</script>