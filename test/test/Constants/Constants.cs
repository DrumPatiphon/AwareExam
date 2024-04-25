using System;
using System.Text;

namespace test.Constants
{
    public static class TestConstants
    {
        public class TaskStatus
        {
            public const string Save = "SAVED";
            public const string Cancel = "CANCELLED";
            public const string Complete = "COMPLETED";
        }

        public class Action
        {
            public const string Save = "Save";
            public const string Cancel = "Cancel";
        }

        public class RowState 
        {
            public const string Add = "Add";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
        }
    }
}
