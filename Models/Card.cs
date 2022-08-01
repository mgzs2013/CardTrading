using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace CardTrading.Models{

    public class Card {
        public int CardId {get; set;}
        
        public string? PokemonName { get; set;}
        [Required]
        [DisplayName("Name")]
        public string? CardCondition { get; set;}
        [Required]
        [DisplayName("Condition")]
        public string? CardPhoto { get; set;}
        [Required]
        [DisplayName("Photo")]
        public bool ForSale {get; set;}
        [DisplayName("For Sale?")]
        public AppUser? User {get; set;}
        

    }
}