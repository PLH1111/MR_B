using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.Common;
using Monitor.Protocol4851._0;

namespace Monitor.View
{
    public partial class FormCalibrate : Form
    {
        public FormCalibrate()
        {
            InitializeComponent();

            Load += FormCalibrate_Load;
        }

        public Action<MessageHelper> TiggerMessage;

        private void FormCalibrate_Load(object sender, EventArgs e)
        {
            calibrateUis.Add(new UcCalibrate() {CalibrateName = "Current",      Channel = 0 });
            calibrateUis.Add(new UcCalibrate() {CalibrateName = "PackNegVolt", Channel = 1 });
            calibrateUis.Add(new UcCalibrate() {CalibrateName = "AfeBatVolt",   Channel = 2 });
            calibrateUis.Add(new UcCalibrate() {CalibrateName = "McuBatVolt",   Channel = 3 });

            flowLayoutPanel1.Controls.AddRange(calibrateUis.Select(p => p as UcCalibrate).ToArray());

            calibrateUis.ForEach(x => x.ClickHandler +=  ButtonClick);
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            TiggerMessage(new MessageHelper(sender, e, (o, ergs) => { Calibrate(sender, e as EventArgsCalibrate); }));
        }

        List<ICalibrateUiInfo> calibrateUis = new List<ICalibrateUiInfo>();

        public EventHandler<EventArgsCalibrate> Calibrate;
    }

   
}
