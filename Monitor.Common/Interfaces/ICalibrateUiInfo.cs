using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public interface ICalibrateUiInfo
    {
        byte            Channel { get; set; }
        string          CalibrateName { get; set; }
        EventHandler<EventArgsCalibrate>    ClickHandler { get; set; }
        List<string>    Input   { get; }
        string Output1 { set; get; }
        string Output2 { set; get; }
        float[] Result  { set; }
    }
    
     public class EventArgsCalibrate : EventArgs
    {
        public EventArgsCalibrate(byte step)
        {
            Step = step;
        }

        public byte Step { get; set; }
    }
}
