<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="Gafware.Modules.Glossary.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:Label ID="lblCategoryVisible" runat="server" ResourceKey="lblCategoryVisible" ControlName="chkCategoryVisible" Suffix=":" /> 
        <asp:CheckBox ID="chkCategoryVisible" runat="server" Checked="true" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblDefaultCategory" runat="server" ResourceKey="lblDefaultCategory" ControlName="lstDefaultCategory" Suffix=":" />
        <asp:DropDownList ID="lstDefaultCategory" runat="server" DataTextField="CategoryName" DataValueField="GlossaryCategoryDefinitionId" />
    </div>
</fieldset>
