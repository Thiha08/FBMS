using Ardalis.Specification;
using FBMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Specifications
{
    public class IncompleteItemsSpecification : Specification<ToDoItem>
    {
        public IncompleteItemsSpecification()
        {
            Query.Where(item => !item.IsDone);
        }
    }
}
