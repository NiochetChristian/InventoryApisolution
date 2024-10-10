using System.ComponentModel.DataAnnotations;

namespace InventoryClientAPI.Models
{
    public class Product
    {
        // ID único del producto
        [Key]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "El campo Nombre del Producto es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El Nombre del Producto no debe exceder de 100 caracteres.")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "El Nombre del Producto no puede comenzar con espacios en blanco.")]
        [Display(Name = "Nombre de producto")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "El campo Tipo de Producción es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El Tipo de Producción no debe exceder de 50 caracteres.")]
        [RegularExpression(@"^(Elaborado a mano|Elaborado a máquina|Elaborado a mano y máquina)$",
            ErrorMessage = "El Tipo de Producción debe ser 'Elaborado a mano', 'Elaborado a máquina' o 'Elaborado a mano y máquina'.")]
        [Display(Name = "Elaborado")]
        public string ProductionType { get; set; }

        [Required(ErrorMessage = "El campo Estado del Producto es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El Estado del Producto no debe exceder de 20 caracteres.")]
        [RegularExpression(@"^(Disponible|Defectuoso)$", ErrorMessage = "El Estado del Producto debe ser 'Disponible' o 'Defectuoso'.")]
        [Display(Name = "Estado")]
        public string ProductStatus { get; set; }

        [Required(ErrorMessage = "La fecha de agregado es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Adición")]
        public DateTime DateAdded { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Última Actualización")]
        public DateTime? LastUpdated { get; set; }
    }

    public enum ProductStatus
    {
        Disponible,    // El producto está disponible en el inventario
        Defectuoso     // El producto ha sido marcado como defectuoso
    }
}
