using System.Collections.Generic;

namespace Bugger.Proxy.TFS.Models
{
    internal static class TempTFSData
    {
        static TempTFSData()
        {
            CanRestore = false;

            TFSFields = new List<TFSField>();
            BugFilterFields = new List<TFSField>();
            PriorityValues = new List<CheckString>();

            StateValues = new List<string>();
        }

        public static bool CanRestore { get; set; }

        public static bool CanConnect { get; set; }

        public static List<TFSField> TFSFields { get; set; }
        public static List<TFSField> BugFilterFields { get; set; }
        public static List<CheckString> PriorityValues { get; set; }

        public static List<string> StateValues { get; set; }


        internal static void Clear()
        {
            CanConnect = false;

            TFSFields = new List<TFSField>();
            BugFilterFields = new List<TFSField>();
            PriorityValues = new List<CheckString>();

            StateValues = new List<string>();
        }
    }
}
