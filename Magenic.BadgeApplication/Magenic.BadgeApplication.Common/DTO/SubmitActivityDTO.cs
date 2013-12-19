﻿using Magenic.BadgeApplication.Common.Interfaces;
using System;

namespace Magenic.BadgeApplication.Common.DTO
{
    /// <summary>
    /// Data Transfer Object for the CustomIdentity
    /// </summary>
    public class SubmitActivityDTO : ISubmitActivityDTO
    {
        /// <summary>
        /// The Id for this activity submission.  Zero if new.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The id of the activity this submission is for.
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// The date the activity occurred, should be set and saved in UTC.
        /// </summary>
        public DateTime ActivitySubmissionDate { get; set; }
        /// <summary>
        /// Any notes associated with this submission.
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// The AD user name of the person who this badge submission is for.  
        /// This should be the same as the name of the identity.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The current status of this activity submission.
        /// </summary>
        public Enums.ActivitySubmissionStatus Status { get; set; }
        /// <summary>
        /// The AD user name of the user who approved this activity.  Blank if the 
        /// activity status is approved and no managerial approval is required.
        /// </summary>
        public string ApprovedByUserName { get; set; }
    }
}
