using IssueTracker.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Helpers
{
    public class SlugMaker
    {
        Var slug = StringUtilities.URLFriendly(TicketAttachment.fileName);
    }
}