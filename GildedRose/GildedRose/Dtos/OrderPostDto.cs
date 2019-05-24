using GildedRose.Dtos;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GildedRose.Models
{
	public class OrderPostDto : IValidatableObject
	{
		public ICollection<OrderItemPostDto> OrderItems { get; set; }

		public OrderPostDto()
		{
			OrderItems = new Collection<OrderItemPostDto>();
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var results = new List<ValidationResult>();

			if (OrderItems.Count <= 0)
				results.Add(new ValidationResult("Missing orders."));

			return results;
		}
	}
}