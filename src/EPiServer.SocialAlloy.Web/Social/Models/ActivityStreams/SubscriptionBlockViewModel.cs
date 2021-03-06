﻿using EPiServer.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SubscriptionBlockViewModel class represents the model that will be used to
    /// feed data to the subscriptions block frontend view.
    /// </summary>
    public class SubscriptionBlockViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="block">A block reference to use as a key under which to save the model state.</param>
        /// <param name="form">A subscription form view model to get current form values for the block view model</param>
        public SubscriptionBlockViewModel(SubscriptionBlock block, PageReference currentPageLink)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            ShowSubscriptionForm = false;
            UserSubscribedToPage = false;
            CurrentPageLink = currentPageLink;
        }

        /// <summary>
        /// Gets or sets whether to show subscription form.
        /// </summary>
        public bool ShowSubscriptionForm { get; set; }

        /// <summary>
        /// Gets or sets the heading for the frontend subscriptions block display.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets whether to show the block heading in the frontend subscriptions block display.
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// Gets or sets whether the current user is subscribed to current page.
        /// </summary>
        public bool UserSubscribedToPage { get; set; }

        /// <summary>
        /// Gets or sets the current page link for the page that the block is on.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets and sets message details to be displayed to the user
        /// </summary>
        public List<MessageViewModel> Messages { get; set; }
    }
}