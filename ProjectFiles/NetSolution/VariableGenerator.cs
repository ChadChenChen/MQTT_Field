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
            //Log.Info("value " + value);
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag1").Value = (Byte)value;
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag2").Value = (SByte)value;
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag3").Value = value;
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag4").Value = value;
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag5").Value = value;
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag6").Value = value;
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag7").Value = value;
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag8").Value = value;
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag9").Value = value;
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag10").Value = value;
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag11").Value = "String " + value;
            bool temp = Convert.ToBoolean(((Int64)value % 2));
            Project.Current.GetVariable("CommDrivers/ModbusDriver/ModbusStation/Tags/ModbusTag12").Value = temp;
        }
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        taskPeriodico.Dispose();
    }

    Double value;
}
