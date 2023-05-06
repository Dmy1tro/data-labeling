using Newtonsoft.Json;

namespace DataLabeling.Services.Payment.Models
{
    public class LiqPayPayload
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("public_key")]
        public string PublicKey { get; set; }

        [JsonProperty("action")]
        public string ACtion { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("expired_date")]
        public string ExpiredDate { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("paytypes")]
        public string PayTypes { get; set; }

        [JsonProperty("result_url")]
        public string ResultUrl { get; set; }

        [JsonProperty("server_url")]
        public string ServerUrl { get; set; }

        [JsonProperty("verifycode")]
        public string VerifyCode { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }

        [JsonProperty("product_category")]
        public string ProductCategory { get; set; }

        [JsonProperty("product_description")]
        public string ProductDescription { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("product_url")]
        public string ProductUrl { get; set; }

        [JsonProperty("sandbox")]
        public int Sandbox { get; set; }
    }
}
