namespace RealEstate.Domain.Common.Enums
{
    public enum PropertyDocumentType
    {
        TitleDeed = 1,          // صك الملكية

        BuildingPermit = 2,     // رخصة البناء

        RegaLicense = 3,        // ترخيص فال / رخصة الإعلان

        MunicipalityPermit = 4, // تصريح البلدية

        FloorPlan = 5,          // المخطط

        SurveyReport = 6,       // الرفع المساحي

        InspectionReport = 7,   // تقرير الفحص

        LeaseAgreement = 8,     // عقد الإيجار

        SaleAgreement = 9,      // عقد البيع

        InsurancePolicy = 10,   // وثيقة التأمين

        Other = 99
    }
}
