using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProvidersFixture
{
   public static Provider GetTestProvider()
   {
      var id = Guid.NewGuid();
      var companyName = "BBC";
      var phoneNumber = "+3806666665";
      var email = Email.From("bbc@bbc.co.uk");
      
      return Provider.Create(id, companyName,phoneNumber,email);
   }
}