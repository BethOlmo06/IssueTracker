using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }

        #region Parents / Children

        public int TicketId { get; set; }

        public virtual Tickets Ticket { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        #endregion

        #region Actual Properties
        public string Message { get; set; }

        public DateTime Created { get; set; }

        public bool IsRead { get; set; }
        #endregion
    }
}