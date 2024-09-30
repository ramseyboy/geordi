namespace Geordi.Test.Database.Entities
{
    public class Contract
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public DateTime EndDate { get; set; }
        public Guid TypeId { get; set; }
        public string Status { get; set; }
        public string Supplier { get; set; }
        public string Buyer { get; set; }

        public virtual ContractType ContractType { get; set; }
    }
}
