using FBMS.Core.Entities;
using FBMS.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Events
{
    public class ToDoItemCompletedEvent : BaseDomainEvent
    {
        public ToDoItem CompletedItem { get; set; }

        public ToDoItemCompletedEvent(ToDoItem completedItem)
        {
            CompletedItem = completedItem;
        }
    }
}
