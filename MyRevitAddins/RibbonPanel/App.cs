﻿#region Namespaces
using System;
using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using cn = ConnectConnectors.ConnectConnectors;
using tl = TotalLineLength.TotalLineLength;
using piv = PipeInsulationVisibility.PipeInsulationVisibility;
using ped = PED.InitPED;
using Shared;
//using Document = Autodesk.Revit.Creation.Document;

#endregion

namespace MyRibbonPanel
{
    [Transaction(TransactionMode.Manual)]
    class App : IExternalApplication
    {
        public const string myRibbonPanelToolTip = "My Own Ribbon Panel";

        //Method to get the button image
        BitmapImage NewBitmapImage(Assembly a, string imageName)
        {
            Stream s = a.GetManifestResourceStream(imageName);

            BitmapImage img = new BitmapImage();

            img.BeginInit();
            img.StreamSource = s;
            img.EndInit();

            return img;
        }

        // get the absolute path of this assembly
        static string ExecutingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        // get ref to assembly
        Assembly exe = Assembly.GetExecutingAssembly();

        public Result OnStartup(UIControlledApplication application)
        {
            AddMenu(application);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private void AddMenu(UIControlledApplication application)
        {
            RibbonPanel rvtRibbonPanel = application.CreateRibbonPanel("MyRevitAddins");

            //ConnectConnectors
            PushButtonData data = new PushButtonData("ConnectConnectors", "Connect Connectors", ExecutingAssemblyPath,
                "MyRibbonPanel.ConnectConnectors");
            data.ToolTip = myRibbonPanelToolTip;
            data.Image = NewBitmapImage(exe, "MyRibbonPanel.Resources.ImgConnectConnectors16.png");
            data.LargeImage = NewBitmapImage(exe, "MyRibbonPanel.Resources.ImgConnectConnectors32.png");
            PushButton connectCons = rvtRibbonPanel.AddItem(data) as PushButton;

            //TotalLineLengths
            data = new PushButtonData("TotalLineLengths", "Total length of lines", ExecutingAssemblyPath, "MyRibbonPanel.TotalLineLengths");
            data.ToolTip = myRibbonPanelToolTip;
            data.Image = NewBitmapImage(exe, "MyRibbonPanel.Resources.ImgTotalLineLength16.png");
            data.LargeImage = NewBitmapImage(exe, "MyRibbonPanel.Resources.ImgTotalLineLength32.png");
            PushButton totLentgths = rvtRibbonPanel.AddItem(data) as PushButton;

            //PipeInsulationVisibility
            data = new PushButtonData("PipeInsulationVisibility", "Toggle Pipe Insulation visibility", ExecutingAssemblyPath, "MyRibbonPanel.PipeInsulationVisibility");
            data.ToolTip = myRibbonPanelToolTip;
            data.Image = NewBitmapImage(exe, "MyRibbonPanel.Resources.ImgPipeInsulationVisibility16.png");
            data.LargeImage = NewBitmapImage(exe, "MyRibbonPanel.Resources.ImgPipeInsulationVisibility32.png");
            PushButton pipeInsulationVisibility = rvtRibbonPanel.AddItem(data) as PushButton;

            //PlaceSupports
            data = new PushButtonData("PlaceSupports", "Place supports", ExecutingAssemblyPath, "MyRibbonPanel.PlaceSupports");
            data.ToolTip = myRibbonPanelToolTip;
            data.Image = NewBitmapImage(exe, "MyRibbonPanel.Resources.ImgPlaceSupport16.png");
            data.LargeImage = NewBitmapImage(exe, "MyRibbonPanel.Resources.ImgPlaceSupport32.png");
            PushButton placeSupports = rvtRibbonPanel.AddItem(data) as PushButton;

            //PED
            data = new PushButtonData("PED", "PED", ExecutingAssemblyPath, "MyRibbonPanel.PEDclass");
            data.ToolTip = myRibbonPanelToolTip;
            data.Image = NewBitmapImage(exe, "MyRibbonPanel.Resources.ImgPED16.png");
            data.LargeImage = NewBitmapImage(exe, "MyRibbonPanel.Resources.ImgPED32.png");
            PushButton PED = rvtRibbonPanel.AddItem(data) as PushButton;
        }
    }
    
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class ConnectConnectors : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                using (Transaction trans = new Transaction(commandData.Application.ActiveUIDocument.Document))
                {
                    trans.Start("Connect the Connectors!");
                    cn.ConnectTheConnectors(commandData);
                    trans.Commit();
                }
                return Result.Succeeded;
            }

            catch (Autodesk.Revit.Exceptions.OperationCanceledException) { return Result.Cancelled; }

            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class TotalLineLengths : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                using (Transaction trans = new Transaction(commandData.Application.ActiveUIDocument.Document))
                {
                    trans.Start("Calculate total length of selected lines!");
                    tl.TotalLineLengths(commandData);
                    trans.Commit();
                }
                return Result.Succeeded;
            }

            catch (Autodesk.Revit.Exceptions.OperationCanceledException) { return Result.Cancelled; }

            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class PipeInsulationVisibility : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                using (Transaction trans = new Transaction(commandData.Application.ActiveUIDocument.Document))
                {
                    trans.Start("Toggle Pipe Insulation visibility!");
                    piv.TogglePipeInsulationVisibility(commandData);
                    trans.Commit();
                }
                return Result.Succeeded;
            }

            catch (Autodesk.Revit.Exceptions.OperationCanceledException) { return Result.Cancelled; }

            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class PlaceSupports : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiApp = commandData.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            MEPModel mepModel = null;

            try
            {
                Tuple<Pipe, Element> returnTuple;

                using (Transaction trans = new Transaction(commandData.Application.ActiveUIDocument.Document))
                {
                    trans.Start("Place Supports");
                    returnTuple = PlaceSupport.PlaceSupport.PlaceSupports(commandData);
                    trans.Commit();
                }

                using (Transaction trans1 = new Transaction(commandData.Application.ActiveUIDocument.Document))
                {
                    trans1.Start("Set system of the pipe");
                    //Get the created elements
                    Pipe pipe = returnTuple.Item1;
                    Element elementToAdd = returnTuple.Item2;
                    //Get the pipe type from pipe
                    ElementId pipeTypeId = pipe.PipeType.Id;

                    //Get system type from pipe
                    ConnectorSet pipeConnectors = pipe.ConnectorManager.Connectors;
                    Connector pipeConnector = (from Connector c in pipeConnectors where true select c).FirstOrDefault();
                    ElementId pipeSystemType = pipeConnector.MEPSystem.GetTypeId();

                    //Collect levels and select one level
                    FilteredElementCollector collector = new FilteredElementCollector(doc);
                    ElementClassFilter levelFilter = new ElementClassFilter(typeof(Level));
                    ElementId levelId = collector.WherePasses(levelFilter).FirstElementId();

                    //Get the connector from the support
                    FamilyInstance familyInstanceToAdd = (FamilyInstance)elementToAdd;
                    ConnectorSet connectorSetToAdd = new ConnectorSet();
                    mepModel = familyInstanceToAdd.MEPModel;
                    connectorSetToAdd = mepModel.ConnectorManager.Connectors;
                    if (connectorSetToAdd.IsEmpty)
                        throw new Exception(
                            "The support family lacks a connector. Please read the documentation for correct procedure of setting up a support element.");
                    Connector connectorToConnect =
                        (from Connector c in connectorSetToAdd where true select c).FirstOrDefault();

                    //Create a point in space to connect the pipe
                    XYZ direction = connectorToConnect.CoordinateSystem.BasisZ.Multiply(2);
                    XYZ origin = connectorToConnect.Origin;
                    XYZ pointInSpace = origin.Add(direction);

                    //Create the pipe
                    Pipe newPipe = Pipe.Create(doc, pipeTypeId, levelId, connectorToConnect, pointInSpace);

                    //Change the pipe system type to match the picked pipe (it is not always matching)
                    newPipe.SetSystemType(pipeSystemType);
                    doc.Regenerate();
                    trans1.Commit();
                    

                    trans1.Start("Delete the pipe");

                    //Delete the pipe
                    doc.Delete(newPipe.Id);

                    trans1.Commit();
                }

                return Result.Succeeded;
            }

            catch (Autodesk.Revit.Exceptions.OperationCanceledException) { return Result.Cancelled; }

            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class PEDclass : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiApp = commandData.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = uiApp.ActiveUIDocument;

            using (TransactionGroup txGp = new TransactionGroup(doc))
            {
                txGp.Start("Initialize PED data");

                using (Transaction trans1 = new Transaction(doc))
                {
                    trans1.Start("Create parameters");
                    ped ped = new PED.InitPED();
                    ped.CreateElementBindings(commandData);
                    trans1.Commit();
                }
            }

            return Result.Succeeded;
        }
    }
}

