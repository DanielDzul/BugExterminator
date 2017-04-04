
using BugExterminator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BugExterminator.Helpers
{
    public class TicketHistoryHelper
    {
        public static string ManageData(TicketHistory item, int index)
        {
            var data = "";
            switch(item.Property)
            {
                case "ProjectId":
                    data = index == 0 ? ProjectsHelper.GetProjectNameById(Convert.ToInt32(item.OldValue)) : ProjectsHelper.GetProjectNameById(Convert.ToInt32(item.NewValue));
                    break;
        
                case "AssignedToUserID":
                    data = index == 0 ? UserHelper.GetDisplayNameFromId(item.OldValue) : UserHelper.GetDisplayNameFromId(item.NewValue);
                    break;
                case "RoleId":
                    data = index == 0 ? RoleHelper.GetRoleNameById(item.OldValue) : RoleHelper.GetRoleNameById(item.NewValue);
                    break;
                case "TicketPriorityId":
                    data = index == 0 ? TicketHelper.GetTicketPriorityNameById(Convert.ToInt32(item.OldValue)) : TicketHelper.GetTicketPriorityNameById(Convert.ToInt32(item.NewValue));
                    break;
                case "TicketStatusId":
                    data = index == 0 ? TicketHelper.GetTicketStatusNameById(Convert.ToInt32(item.OldValue)) : TicketHelper.GetTicketStatusNameById(Convert.ToInt32(item.NewValue));
                    break;
                case "TicketTypeId":
                    data = index == 0 ? TicketHelper.GetTicketTypeNameById(Convert.ToInt32(item.OldValue)) : TicketHelper.GetTicketTypeNameById(Convert.ToInt32(item.NewValue));
                    break;
                case "OwnerUserId":
                    data = index == 0 ? UserHelper.GetDisplayNameFromId(item.OldValue) : UserHelper.GetDisplayNameFromId(item.NewValue);
                    break;
                default:
                    data = index == 0 ? item.OldValue : item.NewValue;
                    break;
            }
            return data;
        }
    }
}