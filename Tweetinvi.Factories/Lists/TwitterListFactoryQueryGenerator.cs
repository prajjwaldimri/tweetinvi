﻿using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Factories.Properties;

namespace Tweetinvi.Factories.Lists
{
    public interface ITwitterListFactoryQueryGenerator
    {
        string GetCreateListQuery(string name, PrivacyMode privacyMode, string description);
        string GetListByIdQuery(ITwitterListIdentifier twitterListIdentifier);
    }

    public class TwitterListFactoryQueryGenerator : ITwitterListFactoryQueryGenerator
    {
        private readonly ITwitterListQueryValidator _listsQueryValidator;
        private readonly ITwitterListQueryParameterGenerator _listQueryParameterGenerator;

        public TwitterListFactoryQueryGenerator(
            ITwitterListQueryValidator listsQueryValidator,
            ITwitterListQueryParameterGenerator listQueryParameterGenerator)
        {
            _listsQueryValidator = listsQueryValidator;
            _listQueryParameterGenerator = listQueryParameterGenerator;
        }

        public string GetCreateListQuery(string name, PrivacyMode privacyMode, string description)
        {
            var baseQuery = string.Format(Resources.List_Create, name, privacyMode.ToString().ToLower());

            baseQuery = baseQuery.AddParameterToQuery("description", description);

            return baseQuery;
        }

        public string GetListByIdQuery(ITwitterListIdentifier twitterListIdentifier)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(twitterListIdentifier);

            var identifierParameter = _listQueryParameterGenerator.GenerateIdentifierParameter(twitterListIdentifier);
            return string.Format(Resources.List_GetExistingList, identifierParameter);
        }
    }
}