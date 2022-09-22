using Newtonsoft.Json;
using System;
using static SaveWindowPositions.ExternalItems.ExternalDeclarations;

namespace SaveWindowPositions.Logic
{
    public class WindowInformation
    {
        public string WindowName { get; set; }

        public string HandlerAsString { get; set; }

        [JsonIgnore]
        public IntPtr Handler 
        { 
            get => new IntPtr(int.Parse(HandlerAsString));
        }

        public WINDOWPLACEMENT Placement { get; set; }

        public WindowInformation()
        {
        }

        public WindowInformation(string windowName, IntPtr handler, WINDOWPLACEMENT placement)
        {
            WindowName = windowName;
            HandlerAsString = handler.ToString();
            Placement = placement;
        }
    }
}
