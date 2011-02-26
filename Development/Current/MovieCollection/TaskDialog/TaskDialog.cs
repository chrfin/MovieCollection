using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

// Found at: http://www.codeproject.com/KB/vista/Vista_TaskDialog_Wrapper.aspx

namespace MovieCollection
{
    //--------------------------------------------------------------------------------
    #region PUBLIC enums
    //--------------------------------------------------------------------------------
    public enum TaskDialogIcons { Information, Question, Warning, Error };
    public enum TaskDialogButtons { YesNo, YesNoCancel, OKCancel, OK, Close, Cancel, None }
    #endregion

    //--------------------------------------------------------------------------------
    public static class TaskDialog
    {
        // PUBLIC static values...
        static public bool VerificationChecked = false;
        static public int RadioButtonResult = -1;
        static public int CommandButtonResult = -1;
        static public int EmulatedFormWidth = 450;
        static public bool ForceEmulationMode = false;
        static public bool UseToolWindowOnXP = true;
        static public bool PlaySystemSounds = true;

        //--------------------------------------------------------------------------------
        #region ShowTaskDialogBox
        //--------------------------------------------------------------------------------
        static public DialogResult ShowTaskDialogBox(string Title,
                                                     string MainInstruction,
                                                     string Content,
                                                     string ExpandedInfo,
                                                     string Footer,
                                                     string VerificationText,
                                                     string RadioButtons,
                                                     string CommandButtons,
                                                     TaskDialogButtons Buttons,
                                                     TaskDialogIcons MainIcon,
                                                     TaskDialogIcons FooterIcon)
        {
            if (VistaTaskDialog.IsAvailableOnThisOS && !ForceEmulationMode)
            {
                // [OPTION 1] Show Vista TaskDialog
                VistaTaskDialog vtd = new VistaTaskDialog();

                vtd.WindowTitle = Title;
                vtd.MainInstruction = MainInstruction;
                vtd.Content = Content;
                vtd.ExpandedInformation = ExpandedInfo;
                vtd.Footer = Footer;

                // Radio Buttons
                if (RadioButtons != "")
                {
                    List<VistaTaskDialogButton> lst = new List<VistaTaskDialogButton>();
                    string[] arr = RadioButtons.Split(new char[] { '|' });
                    for (int i = 0; i < arr.Length; i++)
                    {
                        try
                        {
                            VistaTaskDialogButton button = new VistaTaskDialogButton();
                            button.ButtonId = 1000 + i;
                            button.ButtonText = arr[i];
                            lst.Add(button);
                        }
                        catch (FormatException)
                        {
                        }
                    }
                    vtd.RadioButtons = lst.ToArray();
                }

                // Custom Buttons
                if (CommandButtons != "")
                {
                    List<VistaTaskDialogButton> lst = new List<VistaTaskDialogButton>();
                    string[] arr = CommandButtons.Split(new char[] { '|' });
                    for (int i = 0; i < arr.Length; i++)
                    {
                        try
                        {
                            VistaTaskDialogButton button = new VistaTaskDialogButton();
                            button.ButtonId = 2000 + i;
                            button.ButtonText = arr[i];
                            lst.Add(button);
                        }
                        catch (FormatException)
                        {
                        }
                    }
                    vtd.Buttons = lst.ToArray();
                }

                switch (Buttons)
                {
                    case TaskDialogButtons.YesNo:
                        vtd.CommonButtons = VistaTaskDialogCommonButtons.Yes | VistaTaskDialogCommonButtons.No;
                        break;
                    case TaskDialogButtons.YesNoCancel:
                        vtd.CommonButtons = VistaTaskDialogCommonButtons.Yes | VistaTaskDialogCommonButtons.No | VistaTaskDialogCommonButtons.Cancel;
                        break;
                    case TaskDialogButtons.OKCancel:
                        vtd.CommonButtons = VistaTaskDialogCommonButtons.Ok | VistaTaskDialogCommonButtons.Cancel;
                        break;
                    case TaskDialogButtons.OK:
                        vtd.CommonButtons = VistaTaskDialogCommonButtons.Ok;
                        break;
                    case TaskDialogButtons.Close:
                        vtd.CommonButtons = VistaTaskDialogCommonButtons.Close;
                        break;
                    case TaskDialogButtons.Cancel:
                        vtd.CommonButtons = VistaTaskDialogCommonButtons.Cancel;
                        break;
                    default:
                        vtd.CommonButtons = 0;
                        break;
                }

                switch (MainIcon)
                {
                    case TaskDialogIcons.Information: vtd.MainIcon = VistaTaskDialogIcon.Information; break;
                    case TaskDialogIcons.Question: vtd.MainIcon = VistaTaskDialogIcon.Information; break;
                    case TaskDialogIcons.Warning: vtd.MainIcon = VistaTaskDialogIcon.Warning; break;
                    case TaskDialogIcons.Error: vtd.MainIcon = VistaTaskDialogIcon.Error; break;
                }

                switch (FooterIcon)
                {
                    case TaskDialogIcons.Information: vtd.FooterIcon = VistaTaskDialogIcon.Information; break;
                    case TaskDialogIcons.Question: vtd.FooterIcon = VistaTaskDialogIcon.Information; break;
                    case TaskDialogIcons.Warning: vtd.FooterIcon = VistaTaskDialogIcon.Warning; break;
                    case TaskDialogIcons.Error: vtd.FooterIcon = VistaTaskDialogIcon.Error; break;
                }

                vtd.EnableHyperlinks = false;
                vtd.ShowProgressBar = false;
                vtd.AllowDialogCancellation = (Buttons == TaskDialogButtons.Cancel ||
                                               Buttons == TaskDialogButtons.Close ||
                                               Buttons == TaskDialogButtons.OKCancel ||
                                               Buttons == TaskDialogButtons.YesNoCancel);
                vtd.CallbackTimer = false;
                vtd.ExpandedByDefault = false;
                vtd.ExpandFooterArea = false;
                vtd.PositionRelativeToWindow = true;
                vtd.RightToLeftLayout = false;
                vtd.NoDefaultRadioButton = false;
                vtd.CanBeMinimized = false;
                vtd.ShowMarqueeProgressBar = false;
                vtd.UseCommandLinks = (CommandButtons != "");
                vtd.UseCommandLinksNoIcon = false;
                vtd.VerificationText = VerificationText;
                vtd.VerificationFlagChecked = false;
                vtd.ExpandedControlText = Properties.Resources.TASKDIALOG_HIDEDETAILS;
                vtd.CollapsedControlText = Properties.Resources.TASKDIALOG_SHOWDETAILS;
                vtd.Callback = null;

                // Show the Dialog
                DialogResult result = (DialogResult)vtd.Show((vtd.CanBeMinimized ? null : Form.ActiveForm), out VerificationChecked, out RadioButtonResult);

                // if a command button was clicked, then change return result
                // to "DialogResult.OK" and set the CommandButtonResult
                if ((int)result >= 2000)
                {
                    CommandButtonResult = ((int)result - 2000);
                    result = DialogResult.OK;
                }
                if (RadioButtonResult >= 1000)
                    RadioButtonResult -= 1000;  // deduct the ButtonID start value for radio buttons

                return result;
            }
            else
            {
                // [OPTION 2] Show Emulated Form
                EmulatedTaskDialog td = new EmulatedTaskDialog();
                td.Title = Title;
                td.MainInstruction = MainInstruction;
                td.Content = Content;
                td.ExpandedInfo = ExpandedInfo;
                td.Footer = Footer;
                td.RadioButtons = RadioButtons;
                td.CommandButtons = CommandButtons;
                td.Buttons = Buttons;
                td.MainIcon = MainIcon;
                td.FooterIcon = FooterIcon;
                td.VerificationText = VerificationText;
                td.Width = EmulatedFormWidth;
                td.BuildForm();
                DialogResult result = td.ShowDialog();

                RadioButtonResult = td.RadioButtonIndex;
                CommandButtonResult = td.CommandButtonClickedIndex;
                VerificationChecked = td.VerificationCheckBoxChecked;
                return result;
            }
        }
        #endregion

        //--------------------------------------------------------------------------------
        #region MessageBox
        //--------------------------------------------------------------------------------
        static public DialogResult MessageBox(string Title,
                                              string MainInstruction,
                                              string Content,
                                              string ExpandedInfo,
                                              string Footer,
                                              string VerificationText,
                                              TaskDialogButtons Buttons,
                                              TaskDialogIcons MainIcon,
                                              TaskDialogIcons FooterIcon)
        {
            return ShowTaskDialogBox(Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText,
                                     "", "", Buttons, MainIcon, FooterIcon);
        }

        //--------------------------------------------------------------------------------
        static public DialogResult MessageBox(string Title,
                                              string MainInstruction,
                                              string Content,
                                              TaskDialogButtons Buttons,
                                              TaskDialogIcons MainIcon)
        {
            return MessageBox(Title, MainInstruction, Content, "", "", "", Buttons, MainIcon, TaskDialogIcons.Information);
        }

        //--------------------------------------------------------------------------------
        #endregion

        //--------------------------------------------------------------------------------
        #region ShowRadioBox
        //--------------------------------------------------------------------------------
        static public int ShowRadioBox(string Title,
                                       string MainInstruction,
                                       string Content,
                                       string ExpandedInfo,
                                       string Footer,
                                       string VerificationText,
                                       string RadioButtons,
                                       TaskDialogIcons MainIcon,
                                       TaskDialogIcons FooterIcon)
        {
            DialogResult res = ShowTaskDialogBox(Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText,
                                                 RadioButtons, "", TaskDialogButtons.OKCancel,
                                                 MainIcon, FooterIcon);
            if (res == DialogResult.OK)
                return RadioButtonResult;
            else
                return -1;
        }

        //--------------------------------------------------------------------------------
        // Simple overloaded version
        static public int ShowRadioBox(string Title,
                                       string MainInstruction,
                                       string Content,
                                       string RadioButtons)
        {
            return ShowRadioBox(Title, MainInstruction, Content, "", "", "", RadioButtons, TaskDialogIcons.Question, TaskDialogIcons.Information);
        }
        #endregion

        //--------------------------------------------------------------------------------
        #region ShowCommandBox
        //--------------------------------------------------------------------------------
        static public int ShowCommandBox(string Title,
                                         string MainInstruction,
                                         string Content,
                                         string ExpandedInfo,
                                         string Footer,
                                         string VerificationText,
                                         string CommandButtons,
                                         bool ShowCancelButton,
                                         TaskDialogIcons MainIcon,
                                         TaskDialogIcons FooterIcon)
        {
            DialogResult res = ShowTaskDialogBox(Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText,
                                                 "", CommandButtons, (ShowCancelButton ? TaskDialogButtons.Cancel : TaskDialogButtons.None),
                                                 MainIcon, FooterIcon);
            if (res == DialogResult.OK)
                return CommandButtonResult;
            else
                return -1;
        }

        //--------------------------------------------------------------------------------
        // Simple overloaded version
        static public int ShowCommandBox(string Title, string MainInstruction, string Content, string CommandButtons, bool ShowCancelButton)
        {
            return ShowCommandBox(Title, MainInstruction, Content, "", "", "", CommandButtons, ShowCancelButton,
                                  TaskDialogIcons.Question, TaskDialogIcons.Information);
        }

        #endregion

        //--------------------------------------------------------------------------------
    }
}
