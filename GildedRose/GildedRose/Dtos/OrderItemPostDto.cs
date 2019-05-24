using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GildedRose.Dtos
{
	public class OrderItemPostDto : IValidatableObject
	{
		public int ItemId { get; set; }

		public int Quantity { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var results = new List<ValidationResult>();

			if (ItemId <= 0)
				results.Add(new ValidationResult("Invalid itemId."));
			if (Quantity <= 0)
				results.Add(new ValidationResult("Invalid quantity."));

			return results;
		}
	}
}