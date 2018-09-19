using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using BCServerlessDemo.DataAndFunctions.Core.Service.Search;
using Microsoft.Azure.Documents.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Tests.Service.Search
{
    [TestFixture]
    public class OrganisationQueryBuilderTests
    {
        IQueryable<Organisation> query;
        OrganisationQueryBuilder subject;

        [SetUp]
        public void Setup()
        {
            var docClient = new DocumentClient(new Uri("https://localhost:8081/"), "", connectionPolicy: new ConnectionPolicy { EnableEndpointDiscovery = false });

            //var collectionUri = UriFactory.CreateDocumentCollectionUri(_applicationConfig.Database, _applicationConfig.Collection);
            query = docClient.CreateDocumentQuery<Organisation>("");

            subject = new OrganisationQueryBuilder();
        }

        [Test]
        public void GivenUserIsAdmin_WhenSearchingOrganisations_ThenNoOrgFilterIsAdded()
        {
            var gridQuery = new GridQuery();
            var userDigest = new UserDigest()
            {
                AppRole = Role.Admin               
            };

            query = subject.GetQuery(query, gridQuery, userDigest);

            var actualQuery = query.ToString();
            string expected = @"{""query"":""SELECT * FROM root WHERE (root[\""deleted\""] = false) ""}";
            Assert.AreEqual(expected, actualQuery);
        }        

        [Test]
        public void GivenUserIsTrainingProviderAdmin_WhenSearchingOrganisations_ThenICanOnlySeeMyOwn()
        {
            var gridQuery = new GridQuery();
            var userDigest = new UserDigest()
            {
                AppRole = Role.OrgAdmin,
                OrganisationId = "ORG123"
            };

            query = subject.GetQuery(query, gridQuery, userDigest);

            var actualQuery = query.ToString();
            string expected = "(root[\\\"id\\\"] = \\\"ORG123\\\"))";

            StringAssert.Contains(expected, actualQuery);
        }

    }
}
