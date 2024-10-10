using System.ComponentModel.DataAnnotations;

namespace InventoryClientAPI.Models {
    public class Product {
        // ID único del producto
        [Key]
        public Guid ProductId { get; set; }

        // Nombre del producto
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        // Tipo de elaboración (elaborado a mano, elaborado a mano y máquina)
        [Required]
        [MaxLength(50)]
        public string ProductionType { get; set; }

        // Estado del producto (Disponible, Defectuoso, etc.)
        [Required]
        [MaxLength(20)]
        public string ProductStatus { get; set; }

        // Fecha en que se agregó el producto al inventario
        public DateTime DateAdded { get; set; }

        // Fecha en que se actualizó el estado del producto
        public DateTime? LastUpdated { get; set; }
    }

    public enum ProductStatus {
        Disponible,    // El producto está disponible en el inventario
        Defectuoso     // El producto ha sido marcado como defectuoso
    }
}
