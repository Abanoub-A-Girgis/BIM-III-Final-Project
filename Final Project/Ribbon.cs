using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Ribbon
{
    internal class Ribbon : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {

            application.CreateRibbonTab("Scheduling to CSV");

            RibbonPanel ribbonpanel =  application.CreateRibbonPanel("Scheduling to CSV", "Scheduling in Revit");
            RibbonPanel ribbonpanel2 = application.CreateRibbonPanel("Scheduling to CSV", "Scheduling to CSV");

            string path = Assembly.GetExecutingAssembly().Location;

            PushButtonData button1 = new PushButtonData("Button 1", "Create Beam Schedule", path, "Final_Project.Beams");
            PushButtonData button2 = new PushButtonData("Button 2", "Create Roof Schedule", path, "Final_Project.Roof");
            PushButtonData button3 = new PushButtonData("Button 3", "Create Columns Schedule", path, "Final_Project.Columns");
            PushButtonData button4 = new PushButtonData("Button 4", "Create Walls Schedule", path, "Final_Project.Walls");
            PushButtonData button5 = new PushButtonData("Button 5", "Create Doors Schedule", path, "Final_Project.Doors");
            PushButtonData button6 = new PushButtonData("Button 6", "Create Windows Schedule", path, "Final_Project.Windows");
            PushButtonData button7 = new PushButtonData("Button 7", "Create Floors Schedule", path, "Final_Project.Floors");

            PushButtonData button8 = new PushButtonData("Button 7", "Export Schedules", path, "Final_Project.ExportAllSchedules");

            PushButton pushButton1 = ribbonpanel.AddItem(button1) as PushButton;
            PushButton pushButton2 = ribbonpanel.AddItem(button2) as PushButton;
            PushButton pushButton3 = ribbonpanel.AddItem(button3) as PushButton;
            PushButton pushButton4 = ribbonpanel.AddItem(button4) as PushButton;
            PushButton pushButton5 = ribbonpanel.AddItem(button5) as PushButton;
            PushButton pushButton6 = ribbonpanel.AddItem(button6) as PushButton;
            PushButton pushButton7 = ribbonpanel.AddItem(button7) as PushButton;

            PushButton pushButton8 = ribbonpanel2.AddItem(button8) as PushButton;

            pushButton1.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Final Project;component/Icons/Beam.png"));
            pushButton2.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Final Project;component/Icons/Roof.png"));
            pushButton3.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Final Project;component/Icons/Column.png"));
            pushButton4.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Final Project;component/Icons/Wall.png"));
            pushButton5.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Final Project;component/Icons/Door.png"));
            pushButton6.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Final Project;component/Icons/Window.png"));
            pushButton7.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Final Project;component/Icons/Floor.png"));

            pushButton8.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Final Project;component/Icons/Export.png"));

            pushButton1.ToolTip = "Create Beams Scheduling";
            pushButton2.ToolTip = "Create Roof Scheduling";
            pushButton3.ToolTip = "Create Column Scheduling";
            pushButton4.ToolTip = "Create Wall Scheduling";
            pushButton5.ToolTip = "Create Door Scheduling";
            pushButton6.ToolTip = "Create Window Scheduling";
            pushButton7.ToolTip = "Create Floor Scheduling";

            pushButton8.ToolTip = "Export All Created Schedules to CSV";

            return Result.Succeeded;
        }
    }
}
