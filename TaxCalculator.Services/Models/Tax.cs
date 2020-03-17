namespace TaxCalculator.Services.Models
{
    public class Tax
    {
        public float OrderTotalAmount { get; set; }
        public float Shipping { get; set; }
        public double TaxableAmount { get; set; }
        public float AmountToCollect { get; set; }
        public float Rate { get; set; }
        public bool FreightTaxable { get; set; }
        public string TaxSource { get; set; }
    }
}
