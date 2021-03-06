﻿using System;
using Diagnostics.ModelsAndUtils.Attributes;
using Diagnostics.ModelsAndUtils.Models.Storage;
using Diagnostics.ModelsAndUtils.ScriptUtilities;

namespace Diagnostics.ModelsAndUtils.Models
{
    /// <summary>
    /// Resource representing a Generic Arm Resource
    /// </summary>
    public class ArmResource : ArmResourceFilter, IResource
    {
        /// <summary>
        /// Subscription Id where the resource resides
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Resource group where the resource resides
        /// </summary>
        public string ResourceGroup { get; set; }

        /// <summary>
        /// Name of the resource
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Location of the resource
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Arm URI for the resource
        /// </summary>
        public string ResourceUri
        {
            get
            {
                return UriUtilities.BuildAzureResourceUri(SubscriptionId, ResourceGroup, Name, Provider, ResourceTypeName);
            }
        }

        /// <summary>
        /// Subscription Location Placement id
        /// </summary>
        public string SubscriptionLocationPlacementId
        {
            get; set;
        }

        public ArmResource(string subscriptionId, string resourceGroup, string provider, string resourceTypeName, string resourceName, string location = null, string subscriptionLocationPlacementId = null) : base(provider, resourceTypeName)
        {
            this.SubscriptionId = subscriptionId;
            this.ResourceGroup = resourceGroup;
            this.Name = resourceName;
            this.Location = location;
            SubscriptionLocationPlacementId = subscriptionLocationPlacementId;
        }

        /// <summary>
        /// Determines whether the current arm resource is applicable after filtering.
        /// </summary>
        /// <param name="filter">Resource Filter</param>
        /// <returns>True, if resource passes the filter. False otherwise</returns>
        public bool IsApplicable(IResourceFilter filter)
        {
            bool customFilterLogicResult = true;
            if (filter is ArmResourceFilter armResFilter)
            {
                customFilterLogicResult = !string.IsNullOrWhiteSpace(armResFilter.Provider) &&
                                        !string.IsNullOrWhiteSpace(armResFilter.ResourceTypeName) &&
                                        armResFilter.Provider.Equals(this.Provider, StringComparison.OrdinalIgnoreCase) &&
                                        armResFilter.ResourceTypeName.Equals(this.ResourceTypeName, StringComparison.OrdinalIgnoreCase);
            }

            return base.IsApplicable<ArmResourceFilter>(filter, this.Provider, this.ResourceTypeName, customFilterLogicResult);
        }

        /// <summary>
        /// Determines whether the diag entity retrieved from table is applicable after filtering.
        /// </summary>
        /// <param name="diagEntity">Diag Entity from table</param>
        /// <returns>True, if resource passes the filter. False otherwise</returns>
        public bool IsApplicable(DiagEntity diagEntity)
        {
            return base.IsApplicable(diagEntity, this.Provider, this.ResourceTypeName);
        }
    }
}
