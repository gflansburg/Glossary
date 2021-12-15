/*
' Copyright (c) 2021  Gafware
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using Gafware.Modules.Glossary.Components;
using DotNetNuke.Entities.Modules;

namespace Gafware.Modules.Glossary
{
    public class GlossaryModuleSettingsBase : ModuleSettingsBase
    {
        protected bool UseFriendlyUrl
        {
            get
            {
                if (Settings.Contains(GlossaryController.USE_FRIENDLY_URL_KEY))
                {
                    return Convert.ToBoolean(Settings[GlossaryController.USE_FRIENDLY_URL_KEY].ToString());
                }
                return true;
            }
        }

        protected bool CategoryVisible
        {
            get
            {
                if (Settings.Contains(GlossaryController.CATEGORY_VISIBLE_KEY))
                {
                    return Convert.ToBoolean(Settings[GlossaryController.CATEGORY_VISIBLE_KEY].ToString());
                }
                return true;
            }
        }

        protected string DefaultCategory
        {
            get
            {
                if (Settings.Contains(GlossaryController.DEFAULT_CATEGORY_KEY))
                {
                    return Settings[GlossaryController.DEFAULT_CATEGORY_KEY].ToString();
                }
                return String.Empty;
            }
        }

        protected string GlossaryPage
        {
            get
            {
                if (Settings.Contains(GlossaryController.GLOSSARY_PAGE_KEY))
                {
                    return Settings[GlossaryController.GLOSSARY_PAGE_KEY].ToString();
                }
                return String.Empty;
            }
        }

        protected string ToolTipStyle
        {
            get
            {
                if (Settings.Contains(GlossaryController.STYLE_KEY))
                {
                    return Settings[GlossaryController.STYLE_KEY].ToString();
                }
                return String.Empty;
            }
        }

        protected bool Rounded
        {
            get
            {
                if (Settings.Contains(GlossaryController.ROUNDED_KEY))
                {
                    return Convert.ToBoolean(Settings[GlossaryController.ROUNDED_KEY].ToString());
                }
                return true;
            }
        }

        protected bool Shadowed
        {
            get
            {
                if (Settings.Contains(GlossaryController.SHADOWED_KEY))
                {
                    return Convert.ToBoolean(Settings[GlossaryController.SHADOWED_KEY].ToString());
                }
                return true;
            }
        }

        protected string PositionMyX
        {
            get
            {
                if (Settings.Contains(GlossaryController.POSITION_MY_X_KEY))
                {
                    return Settings[GlossaryController.POSITION_MY_X_KEY].ToString();
                }
                return "center";
            }
        }

        protected string PositionMyY
        {
            get
            {
                if (Settings.Contains(GlossaryController.POSITION_MY_Y_KEY))
                {
                    return Settings[GlossaryController.POSITION_MY_Y_KEY].ToString();
                }
                return "bottom";
            }
        }

        protected string PositionAtX
        {
            get
            {
                if (Settings.Contains(GlossaryController.POSITION_AT_X_KEY))
                {
                    return Settings[GlossaryController.POSITION_AT_X_KEY].ToString();
                }
                return "center";
            }
        }

        protected string PositionAtY
        {
            get
            {
                if (Settings.Contains(GlossaryController.POSITION_AT_Y_KEY))
                {
                    return Settings[GlossaryController.POSITION_AT_Y_KEY].ToString();
                }
                return "top";
            }
        }

        protected bool UseImage
        {
            get
            {
                if (Settings.Contains(GlossaryController.USE_IMAGE_KEY))
                {
                    return Convert.ToBoolean(Settings[GlossaryController.USE_IMAGE_KEY].ToString());
                }
                return false;
            }
        }

        protected string ImageUrl
        {
            get
            {
                if (Settings.Contains(GlossaryController.IMAGE_KEY))
                {
                    return Settings[GlossaryController.IMAGE_KEY].ToString();
                }
                return String.Empty;
            }
        }

        protected System.Drawing.Color LinkColor
        {
            get
            {
                if (Settings.Contains(GlossaryController.LINK_COLOR_KEY))
                {
                    return System.Drawing.ColorTranslator.FromHtml(Settings[GlossaryController.LINK_COLOR_KEY].ToString());
                }
                return System.Drawing.ColorTranslator.FromHtml("#666666");
            }
        }

        protected System.Drawing.Color HoverColor
        {
            get
            {
                if (Settings.Contains(GlossaryController.HOVER_COLOR_KEY))
                {
                    return System.Drawing.ColorTranslator.FromHtml(Settings[GlossaryController.HOVER_COLOR_KEY].ToString());
                }
                return System.Drawing.ColorTranslator.FromHtml("#3771D1");
            }
        }

        protected System.Drawing.Color UnderlineColor
        {
            get
            {
                if (Settings.Contains(GlossaryController.UNDERLINE_COLOR_KEY))
                {
                    return System.Drawing.ColorTranslator.FromHtml(Settings[GlossaryController.UNDERLINE_COLOR_KEY].ToString());
                }
                return System.Drawing.ColorTranslator.FromHtml("#666666");
            }
        }

        protected System.Drawing.Color UnderlineHoverColor
        {
            get
            {
                if (Settings.Contains(GlossaryController.UNDERLINE_HOVER_COLOR_KEY))
                {
                    return System.Drawing.ColorTranslator.FromHtml(Settings[GlossaryController.UNDERLINE_HOVER_COLOR_KEY].ToString());
                }
                return System.Drawing.ColorTranslator.FromHtml("#3771D1");
            }
        }

        protected string UnderlineStyle
        {
            get
            {
                if (Settings.Contains(GlossaryController.LINK_UNDERLINE_KEY))
                {
                    return Settings[GlossaryController.LINK_UNDERLINE_KEY].ToString();
                }
                return "double";
            }
        }

        protected string ShowEvent
        {
            get
            {
                if (Settings.Contains(GlossaryController.SHOW_KEY))
                {
                    return Settings[GlossaryController.SHOW_KEY].ToString();
                }
                return "mouseenter";
            }
        }

        protected string HideEvent
        {
            get
            {
                if (Settings.Contains(GlossaryController.HIDE_KEY))
                {
                    return Settings[GlossaryController.HIDE_KEY].ToString();
                }
                return "mouseleave";
            }
        }

        protected string UnderlineWidth
        {
            get
            {
                if (Settings.Contains(GlossaryController.UNDERLINE_WIDTH_KEY))
                {
                    return Settings[GlossaryController.UNDERLINE_WIDTH_KEY].ToString();
                }
                return "1px";
            }
        }

        protected string LinkBold
        {
            get
            {
                if (Settings.Contains(GlossaryController.LINK_BOLD_KEY))
                {
                    return Settings[GlossaryController.LINK_BOLD_KEY].ToString();
                }
                return "inherit";
            }
        }
    }
}