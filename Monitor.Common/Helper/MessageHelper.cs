using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public class MessageHelper
    {
        private EventHandler _event;

        private object _sender;

        private EventArgs _eventArgs;

        public MessageHelper(object sender, EventArgs eventArgs, EventHandler eventHandler)
        {
            _sender = sender;
            _eventArgs = eventArgs;
            _event = eventHandler;
        }

        public MessageHelper(Action action)
        {
            _event = new EventHandler((sender, e) => { action(); });
        }

        public void Run()
        {
            _event(_sender, _eventArgs);
        }
    }
}
