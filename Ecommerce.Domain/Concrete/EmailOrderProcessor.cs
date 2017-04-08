using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Ecommerce.Domain.Abstract;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Concrete
{
    public class EmailSettings
    {
        public bool WriteAsFile = false;
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            
        }
    }
}
