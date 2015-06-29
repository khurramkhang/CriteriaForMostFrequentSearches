//-------------------------------------------------------------------------------
// <copyright file="MostFrequentSearchesCriteriaModel.cs" company="">
//     Copyright (c) All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------
namespace MostFrequentSearchesCriteria
{    
    using System.ComponentModel.DataAnnotations;
    using EPiServer.Filters;
    using EPiServer.Personalization.VisitorGroups;
    using EPiServer.Web.Mvc.VisitorGroups;

    public class MostFrequentSearchesCriteriaModel : CriterionModelBase
    {
        #region Private Members
        #endregion

        #region Constructors
        #endregion

        #region Properties
        /// <summary>
        /// Match type
        /// </summary>
        [DojoWidget(
            LabelTranslationKey = "Match",
            SelectionFactoryType = typeof(EnumSelectionFactory),            
            AdditionalOptions = "{ selectOnClick: true }"
            ), Required]
        public MatchStringType MatchType
        {
            get;
            set;
        }

        /// <summary>
        /// Search String
        /// </summary>
        /// 
        [DojoWidget(
            LabelTranslationKey = "Search Keyword (No of time ssearched - Keyword)",
            SelectionFactoryType = typeof(FrequentSearchesSelection),            
            AdditionalOptions = "{ selectOnClick: true }"
            )
        , Required]
        public string SearchWord
        {
            get;
            set;
        }
        #endregion

        #region Methods
        #region Public
        public override ICriterionModel Copy()
        {
            return base.ShallowCopy();
        }
        #endregion
        
        #region Private
        #endregion
        #endregion

        
    }
}