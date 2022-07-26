using Autodesk.Revit.UI;
using GridlineBinding.IExternalCommands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GridlineBinding
{
    public class App : IExternalApplication
    {
        private const string TabName = "V2 Tools";
        private const string PanelName = "Инструменты";
        private const string ButtonName1 = "2D привязка осей";
        private const string ButtonName2 = "3D привязка осей";
        private readonly string _buttonTooltip = $"Смена привязки осей активного вида на 2D или на 3D границы.\n{typeof(App).Assembly.GetName().Version}";
        public const string Title = "Обновление флажков";
        private static readonly List<string> RequiredRevitVersions = new() { "2019", "2020", "2021", "2022", "2023" };

        public Result OnStartup(UIControlledApplication application)
        {
            if (RunningWrongRevitVersion(application.ControlledApplication.VersionNumber))
            {
                return Result.Cancelled;
            }

            CreateRibbonTab(application);
            var panel = CreateRibbonPanel(application);
            CreateButton(panel);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private static bool RunningWrongRevitVersion(string currentRevitVersion)
        {
            return !RequiredRevitVersions.Contains(currentRevitVersion);
        }

        private void CreateRibbonTab(UIControlledApplication application)
        {
            try
            {
                application.CreateRibbonTab(TabName);
            }
            catch
            {
                // ignored
            }
        }

        private RibbonPanel CreateRibbonPanel(UIControlledApplication application)
        {
            foreach (RibbonPanel panel in application.GetRibbonPanels(TabName))
            {
                if (panel.Name == PanelName)
                {
                    return panel;
                }
            }

            return application.CreateRibbonPanel(TabName, PanelName);
        }

        private void CreateButton(RibbonPanel panel)
        {
            SplitButtonData splitButtonData = new SplitButtonData("2Dto3Daxis", "3Dto2Daxis");

            SplitButton splitButton = panel.AddItem(splitButtonData) as SplitButton;

            PushButtonData buttonData1 = new PushButtonData(
                $"{nameof(GridlineBinding)}1",
                ButtonName1,
                typeof(ConvertGrid22D).Assembly.Location,
                typeof(ConvertGrid22D).FullName
            );

            PushButtonData buttonData2 = new PushButtonData(
                $"{nameof(GridlineBinding)}2",
                ButtonName2,
                typeof(ConvertGrid23D).Assembly.Location,
                typeof(ConvertGrid23D).FullName
            );

            PushButton pushButton = splitButton.AddPushButton(buttonData1);

            pushButton.LargeImage = GetImageSourceByBitMapFromResource(Properties.Resources.no_encryption_FILL0_wght400_GRAD0_opsz48, 32);
            pushButton.Image = GetImageSourceByBitMapFromResource(Properties.Resources.no_encryption_FILL0_wght400_GRAD0_opsz48, 16);
            pushButton.ToolTip = _buttonTooltip;

            pushButton = splitButton.AddPushButton(buttonData2);

            pushButton.LargeImage = GetImageSourceByBitMapFromResource(Properties.Resources.lock_FILL0_wght400_GRAD0_opsz48, 32);
            pushButton.Image = GetImageSourceByBitMapFromResource(Properties.Resources.lock_FILL0_wght400_GRAD0_opsz48, 16);
            pushButton.ToolTip = _buttonTooltip;
        }

        private ImageSource GetImageSourceByBitMapFromResource(Bitmap source, int size)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                source.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(size, size)
            );
        }
    }
}
