//-------------------------------------------------------------------------------
// <copyright file="FrequentSearchesSelection.cs" company="">
//     Copyright (c) All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------
namespace MostFrequentSearchesCriteria
{
    using EPiServer.Personalization.VisitorGroups;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
using EPiServer.Find.Framework;
    using System.Net;
    using log4net;

    public class FrequentSearchesSelection : ISelectionFactory
    {
        #region Private Members
        /// <summary>
        /// Error log manger.
        /// </summary>
        private ILog errorLog = LogManager.GetLogger(typeof(FrequentSearchesSelection));
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Items for dropdown
        /// </summary>
        /// <param name="propertyType">property Type</param>
        /// <returns>list of items</returns>
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in GetMostFrequentSearches())
            {
                items.Add(new SelectListItem() { Text = item.Display, Value = item.Key });
            }
            return items;
        }
        #endregion

        #region Internal
        #endregion

        #region Protected
        #endregion

        #region Private
        /// <summary>
        /// Get most frequent searches from Find
        /// </summary>
        /// <returns>List of frequent searches</returns>
        private IList<SearchModel> GetMostFrequentSearches()
        {
            List<SearchModel> list = new List<SearchModel>();
            using (var client = new WebClient())
            {
                int size = 25;
                string url = string.Format("{0}{1}/_stats/query/top?from={2}&to={3}&interval=hour&size={4}", SearchClient.Instance.ServiceUrl, SearchClient.Instance.DefaultIndex, System.DateTime.Now.AddDays(-30).Date.ToString("yyyy-MM-dd"), System.DateTime.Now.Date.ToString("yyyy-MM-dd"), size);
                var json = client.DownloadString(url);
                try
                {
                    dynamic hitsJson = System.Web.Helpers.Json.Decode(json);
                    foreach (var hit in hitsJson.hits)
                    {
                        var key = hit.query;
                        var cnt = hit.count;
                        list.Add(new SearchModel() { 
                            Key = key,
                            Display = string.Format("{1} - {0}",key,cnt)
                        });
                    }
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
                {
                    errorLog.Error(ex);
                }
                catch (Exception ex)
                {
                    errorLog.Error(ex);
                }
            }
            return list.AsReadOnly();
        }
        #endregion
        #endregion
        
    }
}