namespace CRM.Client.Helpers
{
    public static class QueryBuildingHelper
    {

        
        public static string BuildNameSearchQuery(string searchText)
        {
            string apiEndpoint = "api/searchphysicalperson";
            string queryText = $"$select=inn,pinnumber,firstname,lastname,fathername,clienttype&$filter=contains(fullname, '{searchText}')&$expand=tags($select=name,id)";
            return $"{apiEndpoint}?{queryText}";
        }

        public static string BuildCompanyNameSearchQuery(string searchText)
        {
            string apiEndpoint = "api/searchcompany";
            string queryText = $"$select=tinnumber,inn,companyname,clienttype&$filter=contains(companyname, '{searchText}')&$expand=tags($select=name,id)";
            return $"{apiEndpoint}?{queryText}";
        }


        public static string BuildPinSearchQuery(string searchText)
        {
            string apiEndpoint = "api/searchphysicalperson";
            string queryText = $"$select=inn,pinnumber,firstname,lastname,fathername,clienttype&$filter=pinnumber eq '{searchText}'&$expand=tags($select=name,id)";
            return $"{apiEndpoint}?{queryText}";
        }

        public static string BuildTinSearchQuery(string searchText)
        {
            string apiEndpoint = $"api/GetCompanyContractByDocNumber(DocNumber='{searchText}')";
            string queryText = "$select=tinnumber,inn,clienttype,firstname,lastname,fathername,companyname&$expand=tags($select=name,id)";
            return $"{apiEndpoint}?{queryText}";
        }

        public static string BuildNumberSearchQuery(string searchText)
        {
            string apiEndpoint = $"api/GetClientContractByContactInfo(contactsinfoValue='{searchText}')";
            string queryText = $"$select=pinnumber,tinnumber,firstname,lastname,fathername,companyname,INN,clienttype&$expand=tags($select=name,id)";
            return $"{apiEndpoint}?{queryText}";
        }

        public static string BuildDocumentSearchQuery(string searchText)
        {
            string apiEndpoint = $"api/GetClientContractByDocNumber(DocNumber='{searchText}')";
            string queryText = $"$select=tinnumber,firstname,lastname,fathername,INN,clienttype,companyname&$expand=tags($select=name,id)";
            return $"{apiEndpoint}?{queryText}";
        }

        public static string BuildAssetSearchQuery(string searchText)
        {
            string apiEndpoint = $"api/findClientByAssetAssetInfo='{searchText}'";
            string queryText = $"$select=firstname,lastname,fathername,INN,clienttype,companyname&$expand=tags($select=name,id)";
            return $"{apiEndpoint}?{queryText}";
        }
    }
}
