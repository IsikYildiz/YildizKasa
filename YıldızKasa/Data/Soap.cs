using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ServiceReference;

namespace SoapClientApp
{
    class Soap
    {
        public static async Task<string> ProcessPayment(string cardNo,int secCode,string date,string adres,int fiyat,int urunAdet)
        {
            var binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.None;
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.TransferMode = TransferMode.Buffered;
            binding.UseDefaultWebProxy = false;
            binding.SendTimeout = TimeSpan.FromMinutes(2);
            binding.ReceiveTimeout = TimeSpan.FromMinutes(2);
            binding.OpenTimeout = TimeSpan.FromMinutes(2);
            binding.CloseTimeout = TimeSpan.FromMinutes(2);

            var endpoint = new EndpointAddress("http://localhost:3000/payment");
            
            // Proxy sınıfınızı buraya ekleyin
            var client = new PaymentServiceSoapClient(binding, endpoint);

            // Parametreleri hazırlayın
            var request = new ProcessPaymentRequest
            {
                cardNumber = cardNo,
                securityCode = secCode,
                expiryDate = date,
                address = adres,
                amount = fiyat,
                productCount = urunAdet
            };
            try
            {
                // SOAP servisine bağlanma ve yanıtı alma
                var response = await client.ProcessPaymentAsync(request);
                
                var responseJson = new
                {
                    Success = response.success,
                    Message = response.message,
                    ShippingFee = response.shippingFee,
                    TotalAmount = response.totalAmount
                };

                return JsonConvert.SerializeObject(responseJson);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    Message = ex.Message,
                    ShippingFee = 0,
                    TotalAmount = 0
                });
            }
        }
    }
}