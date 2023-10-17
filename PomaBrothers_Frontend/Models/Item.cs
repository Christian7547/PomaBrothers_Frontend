namespace PomaBrothers_Frontend.Models
{
    public class Item
    {
        public int id;
        public string name;
        public string serie;
        public string description;
        public decimal price;
        public string color;
        public byte durationWarranty;
        public string typeWarranty;
        public int categoryId;
        public byte status;
        public DateTime registerDate = DateTime.Now;
        public int modelId;
        public string urlImage;
        public Category category;
        public ItemModel itemModel;
        public ICollection<SaleDetail> saleDetails = new List<SaleDetail>();

       
        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Serie
        {
            get => serie;
            set => serie = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public decimal Price
        {
            get => price;
            set => price = value;
        }

        public string Color
        {
            get => color;
            set => color = value;
        }

        public byte DurationWarranty
        {
            get => durationWarranty;
            set => durationWarranty = value;
        }

        public string TypeWarranty
        {
            get => typeWarranty;
            set => typeWarranty = value;
        }

        public int CategoryId
        {
            get => categoryId;
            set => categoryId = value;
        }

        public byte Status
        {
            get => status;
            set => status = value;
        }

        public DateTime RegisterDate
        {
            get => registerDate;
            set => registerDate = value;
        }

        public int ModelId
        {
            get => modelId;
            set => modelId = value;
        }

        public string UrlImage
        {
            get => urlImage;
            set => urlImage = value;
        }

        public Category Category
        {
            get => category;
            set => category = value;
        }

        public ItemModel ItemModel
        {
            get => itemModel;
            set => itemModel = value;
        }

        public ICollection<SaleDetail> SaleDetails
        {
            get => saleDetails;
            set => saleDetails = value;
        }
    }

}
