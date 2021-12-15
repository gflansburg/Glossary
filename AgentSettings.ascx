<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgentSettings.ascx.cs" Inherits="Gafware.Modules.Glossary.AgentSettings" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/UrlControl.ascx" %>
<style type="text/css">
    svg path.glossary,
    svg polygon.glossary {
        fill: black;
        stroke: black;
    }
    svg path.glossary:hover,
    svg polygon.glossary:hover {
        fill: black;
        stroke: black;
    }
</style>
<div id="glossaryAgentSettings">
    <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblGlossaryPage" runat="server" ResourceKey="lblGlossaryPage" ControlName="ctlGlossaryPage" Suffix=":" /> 
            <dnn:Url ID="ctlGlossaryPage" runat="server" Required="true" ShowDatabase="false" ShowFiles="false"  ShowImages="false" ShowLog="false" ShowNewWindow="false" ShowNone="false" ShowSecure="false" ShowTabs="true" ShowTrack="false" ShowUpLoad="false" ShowUrls="false" ShowUsers="false" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblStyle" runat="server" ResourceKey="lblStyle" ControlName="lstStyle" Suffix=":" /> 
            <asp:DropDownList ID="lstStyle" runat="server">
                <asp:ListItem Value="" Text="Default style" Selected="True" />
                <asp:ListItem Value="qtip-light" Text="Light colored style" />
                <asp:ListItem Value="qtip-dark" Text="Alternative dark style" />
                <asp:ListItem Value="qtip-cream" Text="Cream-esque style, similar to the default" />
                <asp:ListItem Value="qtip-red" Text="Alert-ful red style" />
                <asp:ListItem Value="qtip-green" Text="Positive green style" />
                <asp:ListItem Value="qtip-blue" Text="Informative blue style" />
                <asp:ListItem Value="qtip-bootstrap" Text="Twitter Bootstrap style" />
                <asp:ListItem Value="qtip-youtube" Text="Google's new YouTube style" />
                <asp:ListItem Value="qtip-tipsy" Text="Minimalist Tipsy style" />
                <asp:ListItem Value="qtip-tipped" Text="Tipped libraries 'Blue' style" />
                <asp:ListItem Value="qtip-jtools" Text="jTools tooltip style" />
                <asp:ListItem Value="qtip-cluetip" Text="Good ole' ClueTip style" />
            </asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblRounded" runat="server" ResourceKey="lblRounded" ControlName="chkRounded" Suffix=":" /> 
            <asp:CheckBox ID="chkRounded" runat="server" Checked="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShadowed" runat="server" ResourceKey="lblShadowed" ControlName="chkShadowed" Suffix=":" /> 
            <asp:CheckBox ID="chkShadowed" runat="server" Checked="true" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblPositionMy" runat="server" ResourceKey="lblPositionMy" Suffix=":" /> 
            <asp:DropDownList id="lstPostionMyY" runat="server" Width="100px">
                <asp:ListItem Value="top" Text="Top" />
                <asp:ListItem Value="center" Text="Center" />
                <asp:ListItem Value="bottom" Text="Bottom" Selected="True" />
            </asp:DropDownList>
            <asp:DropDownList id="lstPostionMyX" runat="server" Width="100px">
                <asp:ListItem Value="left" Text="Left" />
                <asp:ListItem Value="center" Text="Center" Selected="True" />
                <asp:ListItem Value="right" Text="Right" />
            </asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPositionAt" runat="server" ResourceKey="lblPositionAt" Suffix=":" /> 
            <asp:DropDownList id="lstPostionAtY" runat="server" Width="100px">
                <asp:ListItem Value="top" Text="Top" Selected="True" />
                <asp:ListItem Value="center" Text="Center" />
                <asp:ListItem Value="bottom" Text="Bottom" />
            </asp:DropDownList>
            <asp:DropDownList id="lstPostionAtX" runat="server" Width="100px">
                <asp:ListItem Value="left" Text="Left" />
                <asp:ListItem Value="center" Text="Center" Selected="True" />
                <asp:ListItem Value="right" Text="Right" />
            </asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblUseImage" runat="server" ResourceKey="lblUseImage" ControlName="chkUseImage" Suffix=":" /> 
            <asp:CheckBox ID="chkUseImage" runat="server" AutoPostBack="true" OnCheckedChanged="chkUseImage_CheckedChanged" />
        </div>
        <asp:Panel ID="pnlImage" runat="server" Visible="false">
            <div class="dnnFormItem">
                <dnn:Label ID="lblImage" runat="server" ResourceKey="lblImage" ControlName="ddlImage" Suffix=":" /> 
                <div class="picker"><asp:DropDownList ID="ddlImage" runat="server" CssClass="image-picker" DataTextField="Text" DataValueField="Value" /></div>
            </div>
        </asp:Panel>
        <div class="dnnFormItem">
            <dnn:Label ID="lblLinkColor" runat="server" ResourceKey="lblLinkColor" ControlName="cpLinkColor" Suffix=":" /> 
            <telerik:RadColorPicker style="display: inline-block; position: relative; padding-top: 5px; padding-bottom: 5px;" runat="server" ID="cpLinkColor" PaletteModes="All" CssClass="ColorPickerPreview" KeepInScreenBounds="true" ShowIcon="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblHoverColor" runat="server" ResourceKey="lblHoverColor" ControlName="cpHoverColor" Suffix=":" /> 
            <telerik:RadColorPicker style="display: inline-block; position: relative; padding-top: 5px; padding-bottom: 5px;" runat="server" ID="cpHoverColor" PaletteModes="All" CssClass="ColorPickerPreview" KeepInScreenBounds="true" ShowIcon="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblUnderlineColor" runat="server" ResourceKey="lblUnderlineColor" ControlName="cpUnderlineColor" Suffix=":" /> 
            <telerik:RadColorPicker style="display: inline-block; position: relative; padding-top: 5px; padding-bottom: 5px;" runat="server" ID="cpUnderlineColor" PaletteModes="All" CssClass="ColorPickerPreview" KeepInScreenBounds="true" ShowIcon="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblUnderlineHoverColor" runat="server" ResourceKey="lblUnderlineHoverColor" ControlName="cpUnderlineHoverColor" Suffix=":" /> 
            <telerik:RadColorPicker style="display: inline-block; position: relative; padding-top: 5px; padding-bottom: 5px;" runat="server" ID="cpUnderlineHoverColor" PaletteModes="All" CssClass="ColorPickerPreview" KeepInScreenBounds="true" ShowIcon="true" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblLinkBold" runat="server" ResourceKey="lblLinkBold" ControlName="lstLinkBold" Suffix=":" /> 
            <asp:DropDownList ID="lstLinkBold" runat="server">
                <asp:ListItem Value="inherit" Text="Inherit" Selected="True" />
                <asp:ListItem Value="bold" Text="Bold" />
                <asp:ListItem Value="normal" Text="Normal" />
            </asp:DropDownList>
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblUnderline" runat="server" ResourceKey="lblUnderline" ControlName="lstUnderline" Suffix=":" /> 
            <asp:DropDownList ID="lstUnderline" runat="server">
                <asp:ListItem Value="none" Text="None" />
                <asp:ListItem Value="solid" Text="Solid" />
                <asp:ListItem Value="double" Text="Double" Selected="True" />
                <asp:ListItem Value="dotted" Text="Dotted" />
                <asp:ListItem Value="dashed" Text="Dashed" />
            </asp:DropDownList>
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblUnderlineWidth" runat="server" ResourceKey="lblUnderlineWidth" ControlName="lstUnderlineWidth" Suffix=":" /> 
            <asp:DropDownList ID="lstUnderlineWidth" runat="server">
                <asp:ListItem Value="1px" Text="1 px" />
                <asp:ListItem Value="2px" Text="2 px" />
                <asp:ListItem Value="3px" Text="3 px" Selected="True" />
                <asp:ListItem Value="4px" Text="4 px" />
                <asp:ListItem Value="5px" Text="5 px" />
            </asp:DropDownList>
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblShow" runat="server" ResourceKey="lblShow" ControlName="lstShow" Suffix=":" /> 
            <asp:DropDownList ID="lstShow" runat="server">
                <asp:ListItem Value="mouseenter" Text="Mouse Enter" Selected="True" />
                <asp:ListItem Value="click" Text="Click" />
            </asp:DropDownList>
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblHide" runat="server" ResourceKey="lblHide" ControlName="lstHide" Suffix=":" /> 
            <asp:DropDownList ID="lstHide" runat="server">
                <asp:ListItem Value="mouseleave" Text="Mouse Leave" Selected="True" />
                <asp:ListItem Value="click" Text="Click" />
                <asp:ListItem Value="button" Text="Close Button" />
            </asp:DropDownList>
        </div>

    </fieldset>
</div>
<script language="javascript" type="text/javascript">
    (function ($, Sys) {
        function setUpGlossaryAgentModuleSettings() {
            $('#<%= ddlImage.ClientID %>').imagepicker({hide_select : false});
        }
        $(document).ready(function () {
            setUpGlossaryAgentModuleSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setUpGlossaryAgentModuleSettings();
            });
        });
    } (jQuery, window.Sys));
</script>
