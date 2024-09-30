namespace Geordi.Test.Database.Entities
{
    public class ContractType
    {
        public Guid Id { get; set; }
        public string Type { get; set; }

		public virtual ICollection<Contract> Contracts { get; set; } = new HashSet<Contract>();
    }
}
