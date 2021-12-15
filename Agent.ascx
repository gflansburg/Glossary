<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Agent.ascx.cs" Inherits="Gafware.Modules.Glossary.Agent" %>
<asp:Literal ID="litCSS" runat="server"></asp:Literal>
<script language="javascript" type="text/javascript">
    function toTitleCase(str) {
        return str.replace(/(?:^|\s)\w/g, function (match) {
            return match.toUpperCase();
        });
    }
    $(document).ready(function () {
        findGlossaryItems();
        /*Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            findGlossaryItems();
        });*/
    });
	$.fn.outerHTML = function() {
		var $t = $(this);
		if ($t[0].outerHTML !== undefined) {
			return $t[0].outerHTML;
		} else {
			var content = $t.wrap('<div/>').parent().html();
			$t.unwrap();
			return content;
		}
	};	
    function findGlossaryItems() {
        $('a.glossaryitem').hover(function() {
            $(this).css("border-bottom-color", '<%= System.Drawing.ColorTranslator.ToHtml(UnderlineHoverColor) %>');
        });
        $('a.glossaryitem').each(function() {
			if($(this).text().trim().length > 0) {
				var a = this;
				$(this).wrap('<span class=\"glossarycontainer\"></span>');
				$(this).css('border-bottom-color', '<%= System.Drawing.ColorTranslator.ToHtml(UnderlineColor) %>');
				$(this).attr('href', <%= ShowEvent.Equals("click") ? "'javascript:void(null)' ": "'" + GlossaryPage + (UseFriendlyUrl ? "/term/" : "?term=") + "' + escape($(this).text())" %>);
				$(this).attr('alt', 'Definition for ' + escape($(this).text()));
				$(this).attr('role', 'aria-haspopup');
				$(this).attr('aria-haspopup', 'true');
				<asp:Literal id="litJS" runat="server"></asp:Literal>
				$(this).mouseleave(function () {
					$(a).css('border-bottom-color', '<%= System.Drawing.ColorTranslator.ToHtml(UnderlineColor) %>');
				});
			}
        });
        $('span.glossarycontainer').each(function() {
			var container = $(this);
			var a = $(this).children('a.glossaryitem:first');
			if($(a).text().trim().length > 0) {
				container.bind('keydown', function(e) {
					if(e.keyCode === 32) { e.stopImmediatePropagation(); e.preventDefault(); container.qtip('api').show(e); }
				});
				container.bind('keyup', function(e) {
					if(e.keyCode === 32) { e.stopImmediatePropagation(); e.preventDefault(); }
				});
				$(this).qtip({
					content: {
						button: <%= HideEvent.Equals("button") ? "true" : "false" %>,
						title: toTitleCase(a.text()),
						text: function (event, api) {
							$.ajax({
								url: '<%= ControlPath %>Search.ashx?pid=<%= PortalId %>&term=' + escape(a.text())
							})
							.then(function (content) {
								// Set the tooltip content upon successful retrieval
								var html = $('<div>' + content + '</div>');
								var anchors = html.find('a.glossaryitem');
								anchors.each(function() {
									var a = this;
									$(this).css('border-bottom-color', '<%= System.Drawing.ColorTranslator.ToHtml(UnderlineColor) %>');
                                    $(this).attr('href', '<%= GlossaryPage + (UseFriendlyUrl ? "/term/" : "?term=") %>' + escape($(this).text()));
									$(this).attr('alt', 'Definition for ' + escape($(this).text()));
									$(this).attr('role', 'aria-haspopup');
									$(this).mouseleave(function () {
										$(a).css('border-bottom-color', '<%= System.Drawing.ColorTranslator.ToHtml(UnderlineColor) %>');
									});
								});
								content = html.outerHTML();
								var text = content<%= !HideEvent.Equals("mouseleave") ? " + '<br /><br /><a id=\"qtip-glossary-' + api.id + '\" href=\"" + GlossaryPage + (UseFriendlUrl ? "/term/" : "?term=") + "' + escape(a.text()) + '\">Go to Full Glossary Entry</a>'" : "" %>;
								api.set('content.text', text);
								<%= ShowEvent.Equals("click") ? "setTimeout(function() { $('#qtip-glossary-' + api.id).focus(); }, 100);" : "" %>
							}, function (xhr, status, error) {
								// Upon failure... set the tooltip content to error
								api.set('content.text', status + ': ' + error);
							});
							return 'Loading...'; // Set some initial text
						}
					},
					show: {
						event: '<%= ShowEvent %>',
					    solo: true,
					    modal: {
					        on: true,
					        blur: false,
					        stealfocus: true
					    }					
					},
					hide: '<%= HideEvent.Equals("button") ? "click" : HideEvent %>',
					events: {
						show: function(event, api) {
							$('#qtip-' + api.id + ' a.qtip-close').addClass('tabbable').attr('href', 'javascript:void(null)');
							<%= ShowEvent.Equals("click") ? "setTimeout(function() { $('#qtip-glossary-' + api.id).focus(); }, 100);" : "" %>
						},
						hide: function(event, api) {
							a.focus();
						},
						render: function(event, api) {
				            var elem = api.elements.overlay;
						    $(window).bind('keydown', function(e) {
								if(e.keyCode === 27) { api.hide(e); }
							});
						}
					},
					position: {
						my: '<%= PositionMyY %> <%= PositionMyX %>',
						at: '<%= PositionAtY %> <%= PositionAtX %>',
						target: $(this),
						viewport: $(window),
						effect: false
					},
					style: {
						classes: '<%= ToolTipClasses %>'
					} 
				});
			}
        });
    }
</script>