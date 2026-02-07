using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Tubes_Alia_Richard_714240035_714240047.Helpers
{
    public class MidtransService
    {
        private readonly string serverKey;
        private readonly string clientKey;
        private readonly bool isProduction;
        private readonly string baseUrl;
        private readonly HttpClient httpClient;

        public MidtransService()
        {
            serverKey = ConfigurationManager.AppSettings["MidtransServerKey"];
            clientKey = ConfigurationManager.AppSettings["MidtransClientKey"];
            isProduction = bool.Parse(ConfigurationManager.AppSettings["MidtransIsProduction"] ?? "false");
            
            baseUrl = isProduction 
                ? "https://app.midtrans.com/snap/v1" 
                : "https://app.sandbox.midtrans.com/snap/v1";

            httpClient = new HttpClient();
            
            // Set Basic Auth header
            string authString = Convert.ToBase64String(Encoding.UTF8.GetBytes(serverKey + ":"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public string ClientKey => clientKey;
        
        public string SnapJsUrl => isProduction 
            ? "https://app.midtrans.com/snap/snap.js" 
            : "https://app.sandbox.midtrans.com/snap/snap.js";

        /// <summary>
        /// Create Snap transaction and get payment token
        /// </summary>
        public async Task<MidtransSnapResponse> CreateSnapTransactionAsync(
            string orderId, 
            decimal grossAmount, 
            string customerName,
            string customerEmail,
            List<MidtransItemDetail> items)
        {
            try
            {
                var transactionDetails = new Dictionary<string, object>
                {
                    { "order_id", orderId },
                    { "gross_amount", (int)grossAmount }
                };

                var customerDetails = new Dictionary<string, object>
                {
                    { "first_name", customerName },
                    { "email", customerEmail }
                };

                var itemDetails = new List<Dictionary<string, object>>();
                foreach (var item in items)
                {
                    itemDetails.Add(new Dictionary<string, object>
                    {
                        { "id", item.Id },
                        { "price", (int)item.Price },
                        { "quantity", item.Quantity },
                        { "name", item.Name.Length > 50 ? item.Name.Substring(0, 50) : item.Name }
                    });
                }

                var requestBody = new Dictionary<string, object>
                {
                    { "transaction_details", transactionDetails },
                    { "customer_details", customerDetails },
                    { "item_details", itemDetails }
                };

                var serializer = new JavaScriptSerializer();
                string jsonBody = serializer.Serialize(requestBody);

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"{baseUrl}/transactions", content);
                
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = serializer.Deserialize<Dictionary<string, object>>(responseBody);
                    return new MidtransSnapResponse
                    {
                        Success = true,
                        Token = result["token"]?.ToString(),
                        RedirectUrl = result["redirect_url"]?.ToString()
                    };
                }
                else
                {
                    return new MidtransSnapResponse
                    {
                        Success = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {responseBody}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new MidtransSnapResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Check transaction status from Midtrans
        /// </summary>
        public async Task<MidtransTransactionStatus> GetTransactionStatusAsync(string orderId)
        {
            try
            {
                string statusUrl = isProduction
                    ? $"https://api.midtrans.com/v2/{orderId}/status"
                    : $"https://api.sandbox.midtrans.com/v2/{orderId}/status";

                var response = await httpClient.GetAsync(statusUrl);
                string responseBody = await response.Content.ReadAsStringAsync();

                var serializer = new JavaScriptSerializer();
                var result = serializer.Deserialize<Dictionary<string, object>>(responseBody);

                return new MidtransTransactionStatus
                {
                    OrderId = result.ContainsKey("order_id") ? result["order_id"]?.ToString() : "",
                    TransactionStatus = result.ContainsKey("transaction_status") ? result["transaction_status"]?.ToString() : "",
                    PaymentType = result.ContainsKey("payment_type") ? result["payment_type"]?.ToString() : "",
                    GrossAmount = result.ContainsKey("gross_amount") ? result["gross_amount"]?.ToString() : "0",
                    TransactionId = result.ContainsKey("transaction_id") ? result["transaction_id"]?.ToString() : "",
                    FraudStatus = result.ContainsKey("fraud_status") ? result["fraud_status"]?.ToString() : ""
                };
            }
            catch (Exception ex)
            {
                return new MidtransTransactionStatus
                {
                    TransactionStatus = "error",
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Check if payment is successful based on transaction status
        /// </summary>
        public bool IsPaymentSuccess(string transactionStatus, string fraudStatus)
        {
            return (transactionStatus == "capture" && fraudStatus == "accept") ||
                   transactionStatus == "settlement";
        }

        /// <summary>
        /// Check if payment is pending
        /// </summary>
        public bool IsPaymentPending(string transactionStatus)
        {
            return transactionStatus == "pending";
        }
    }

    #region Models for Midtrans

    public class MidtransSnapResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string RedirectUrl { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class MidtransTransactionStatus
    {
        public string OrderId { get; set; }
        public string TransactionStatus { get; set; }
        public string PaymentType { get; set; }
        public string GrossAmount { get; set; }
        public string TransactionId { get; set; }
        public string FraudStatus { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class MidtransItemDetail
    {
        public string Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
    }

    #endregion
}
