﻿using Magenic.BadgeApplication.Common.Interfaces;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Magenic.BadgeApplication.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ActivityIndexViewModel
    {
        /// <summary>
        /// Gets or sets the possible activities.
        /// </summary>
        /// <value>
        /// The possible activities.
        /// </value>
        public IEnumerable<SelectListItem> PossibleActivities { get; set; }

        /// <summary>
        /// Gets or sets the new activity.
        /// </summary>
        /// <value>
        /// The new activity.
        /// </value>
        public ISubmitActivity SubmittedActivity { get; set; }

        /// <summary>
        /// Gets or sets the previous activities.
        /// </summary>
        /// <value>
        /// The previous activities.
        /// </value>
        public IEnumerable<ISubmittedActivityItem> PreviousActivities { get; set; }

        /// <summary>
        /// A collection of user information that the current user is allowed to enter activities for.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public IUserCollection AvailableUsers { get; set; }
    }
}