//-------------------------------------------------------------------------------
// <copyright file="FrequentSearchesCriterion.cs" company="">
//     Copyright (c) All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------
namespace MostFrequentSearchesCriteria
{
    using System;
    using System.Security.Principal;
    using System.Web;
    using System.Linq;
    using System.Collections.Generic;
    using EPiServer.Filters;
    using EPiServer.Personalization.VisitorGroups;
    using EPiServer.Personalization.VisitorGroups.Criteria;
    using EPiServer.Configuration;
    using System.Text.RegularExpressions;

    [VisitorGroupCriterion(Category="URL Criteria", Description="Criteria can be set based on most frequent searches done by editors", DisplayName = "Most Frequent Searches")]
    public class FrequentSearchesCriterion : UriSessionStartCriterionBase<MostFrequentSearchesCriteriaModel>
    {
        #region Private Members
        private string searchQueryStringRegexpression;
        #endregion

        #region Constructors
        #endregion

        #region Properties
        private string searchKey;
        /// <summary>
        /// Session key
        /// </summary>
        public override string SessionKey
        {
            get
            {
                return "MFSReferrerKeyForSearchingWord";
            }
        }

        /// <summary>
        /// Search Regex
        /// </summary>
        public virtual string SearchRegex
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.searchQueryStringRegexpression))
                {
                    this.searchQueryStringRegexpression = EPiServerSection.Instance.VisitorGroup.SearchKeyWordCriteria.Pattern;
                }

                return this.searchQueryStringRegexpression;
            }
        }
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// ACtion on matching criteria
        /// </summary>
        /// <param name="principal">IPrincipal</param>
        /// <param name="httpContext">Http Context</param>
        /// <returns></returns>
        protected override bool IsMatch(Uri currentURL)
        {
            if (string.IsNullOrEmpty(currentURL.OriginalString))
            {
                return false;
            }
            
            if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Request.Url == null || string.IsNullOrEmpty(HttpContext.Current.Request.Url.OriginalString))
            {
                return false;
            }

            MatchCollection matchCollection = Regex.Matches(HttpContext.Current.Request.Url.OriginalString, this.SearchRegex);
            foreach (Match match in matchCollection)
            {
                Group group = match.Groups["query"];
                if (group.Success)
                {
                    string criteriaValue = HttpUtility.UrlDecode(group.Value.Replace('+', ' '));
                    if (StringMatchHelper.IsMatch(criteriaValue, base.Model.SearchWord, base.Model.MatchType))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Get URI
        /// </summary>
        /// <param name="httpContext">http Context</param>
        /// <returns>url referrer</returns>
        protected override Uri GetUri(HttpContextBase httpContext)
        {
            return httpContext.Request.Url;
        }
        #endregion

        #region Internal
        #endregion

        #region Protected
        #endregion

        #region Private       
        #endregion
        #endregion        
    }
}