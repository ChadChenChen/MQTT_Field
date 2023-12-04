#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.NativeUI;
using FTOptix.NetLogic;
using FTOptix.HMIProject;
using FTOptix.DataLogger;
using FTOptix.UI;
using FTOptix.Alarm;
using FTOptix.EventLogger;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.Modbus;
using FTOptix.CoreBase;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using FTOptix.RAEtherNetIP;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
#endregion

public class VariableGenerator : BaseNetLogic
{
    private PeriodicTask taskPeriodico;
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        value = 0;
        taskPeriodico = new PeriodicTask(randomNum, 1000, LogicObject);
        taskPeriodico.Start();
    }

    public void randomNum()
    {
        Random r = new Random();
        if ((bool)Project.Current.GetVariable("Model/RandomEn").Value)
        {
            value++;
            var tags = Project.Current.GetObject("CommDrivers/ModbusDriver/ModbusStation/Tags");
            foreach (var tag in tags.Children) 
            {
                // Log.Info("tag name is" + tag.BrowseName);
                string path = "CommDrivers/ModbusDriver/ModbusStation/Tags/" + tag.BrowseName;
                var item = Project.Current.GetVariable(path);
                if (item.DataType.Equals(OpcUa.DataTypes.Boolean)) {
                    bool temp = Convert.ToBoolean(((Int64)value % 2));
                    item.Value = temp;
                } else if(item.DataType.Equals(OpcUa.DataTypes.String)) {
                    item.Value = "String " + value;
                } else if(item.DataType.Equals(OpcUa.DataTypes.Byte)) {
                    item.Value = (Byte)value;
                } else if(item.DataType.Equals(OpcUa.DataTypes.SByte)) {
                    item.Value = (SByte)value;
                } else if(item.DataType.Equals(OpcUa.DataTypes.Float)) 
                {
                    item.Value = value + 0.5;
                }
                else {
                    item.Value = value;
                }
            }
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag1").Value = (Byte)value;
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag2").Value = (SByte)value;
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag3").Value = value;
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag4").Value = value;
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag5").Value = value;
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag6").Value = value;
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag7").Value = value;
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag8").Value = value;
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag9").Value = value;
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag10").Value = value;
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag11").Value = "String " + value;
            // bool temp = Convert.ToBoolean(((Int64)value % 2));
            // Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag12").Value = temp;
        }
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        taskPeriodico.Dispose();
    }

    public NodeId ConvertCustomeType(string type)
    {
        NodeId dt = OpcUa.DataTypes.Boolean;
        if( type == "Boolean" ) {
            dt = OpcUa.DataTypes.Boolean;
        } else if( type == "SByte" ) {
            dt = OpcUa.DataTypes.SByte;
        } else if ( type == "Byte" ) {
            dt = OpcUa.DataTypes.Byte;
        } else if( type == "Int16" ) {
            dt = OpcUa.DataTypes.Int16;
        } else if( type == "UInt16" ) {
            dt = OpcUa.DataTypes.UInt16;
        } else if( type == "Int32" ) {
            dt = OpcUa.DataTypes.Int32;
        } else if( type == "UInt32" ) {
            dt = OpcUa.DataTypes.UInt32;
        } else if( type == "Int64" ) {
            dt = OpcUa.DataTypes.Int64;
        } else if( type == "UInt64" ) {
            dt = OpcUa.DataTypes.UInt64;
        } else if( type == "Float" ) {
            dt = OpcUa.DataTypes.Float;
        } else if( type == "Double" ) {
            dt = OpcUa.DataTypes.Double;
        } else if ( type == "String" ) {
            dt = OpcUa.DataTypes.String;
        } else {
            dt = OpcUa.DataTypes.Double;
        }
        return dt;
    }

    [ExportMethod]
    public void AddTag(string name, string type)
    {
        Folder contags = Project.Current.Get<Folder>("CommDrivers/ModbusDriver/ModbusStation/Tags");
        var modbusTag = InformationModel.MakeVariable<FTOptix.Modbus.Tag>(name, FTOptix.Core.DataTypes.NodePath);
        modbusTag.DataType = ConvertCustomeType(type);
        modbusTag.Value = 0;
        contags.Add(modbusTag);
    }

    [ExportMethod]
    public void ChangeType(string tagName, string type)
    {
        string browsePath = "CommDrivers/ModbusDriver/ModbusStation/Tags/" + tagName; 
        var tag = Project.Current.GetVariable(browsePath);
        // tag.VariableType = ConvertCustomeType(type);
        tag.DataType = ConvertCustomeType(type);
        var tag1 = Project.Current.GetVariable(browsePath);
        tag1.Value = "String 0";
        //tag.Value = "String 0";
    }

    [ExportMethod]
    public void ChangeName(string tagName, string name)
    {
        string browsePath = "CommDrivers/ModbusDriver/ModbusStation/Tags/" + tagName; 
        var tag = Project.Current.GetVariable(browsePath);
        tag.BrowseName = name;
    }

    Double value;
}
