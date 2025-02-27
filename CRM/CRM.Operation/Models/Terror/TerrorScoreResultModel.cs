using System.Collections.Generic;

namespace CRM.Operation.Models.Terror
{
    public class TerrorScoreResultModel
    {
        public  int Inn { get; set; }
        public  List<fieldModel> fieldsScore { get; set; }
        public  decimal TotalLvScore { get; set; }
    }

    public class fieldModel
    {
        public  string FieldName { get; set; }
        public decimal LvScore { get; set; }
        
        public  int FieldWeight { get; set; }
        
    }


    
    #region terror config class used to serialize appsettings config into c#
    public class FieldLvCost
    {
        public string insert { get; set; }
        public string replace { get; set; }
        public string delete { get; set; }
    }

    public class FieldsConfig
    {
        public string fieldName { get; set; }
        public int fieldWeight { get; set; }
        public decimal fieldThreshold { get; set; }
        public FieldLvCost fieldLvCost { get; set; }
    }

    public class TerrorConfig
    {
        public string disabled { get; set; }
        public decimal thresholdScore { get; set; }
        public List<int> clientsWithTagId { get; set; }
        public List<FieldsConfig> fieldsConfig { get; set; }
        public string notificationEmail { get; set; }
    }

    public class Root
    {
        public TerrorConfig terrorConfig { get; set; }
    }
    
    #endregion
}