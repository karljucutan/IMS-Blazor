using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.DTOs
{
    public class PurchaseOrderDTO
    {
        public string PurchaseNumber { get; set; }
        public Inventory Inventory { get; set; } // This should be also a DTO for actual projects. Entities should be used only in the Application or specfically in usecases. usescases will accept a DTO then DTO -> Entity
        public int Quantity { get; set; }
        public string DoneBy { get; set; }
    }
}
