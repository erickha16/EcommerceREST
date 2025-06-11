namespace EcommerceREST.Constants
{
    public class Messages
    {
        public static class Success
        {
            // Creación
            public const string BrandCreated = "Marca agregada exitosamente";
            public const string CategoryCreated = "Categoría agregada exitosamente.";
            public const string ProductCreated = "Producto agregado exitosamente.";

            // Actualización
            public const string ProductUpdated = "Producto actualizado exitosamente";
            public const string BrandUpdated = "Marca actualizada exitosamente";
            public const string CategoryUpdated = "Categoría actualizada exitosamente";

            // Eliminación
            public const string ProductDeleted = "Producto eliminado exitosamente";
            public const string BrandDeleted = "Marca eliminada exitosamente";
            public const string CategoryDeleted = "Categoría eliminada exitosamente";
        }

        public static class Error
        {
            // Búsqueda/Existencia
            public const string ProductNotFoundWithId = "Producto con ID {0} no encontrado";
            public const string ProductNotFound = "Producto no encontrado";
            public const string BrandNotFound = "Marca no encontrada";
            public const string CategoryNotFound = "Categoría no encontrada";

            // Creación
            public const string ProductCreateError = "Hubo un error al agregar el producto";
            public const string BrandCreateError = "Hubo un error al agregar la marca";
            public const string CategoryCreateError = "Hubo un error al agregar la categoría";

            // Actualización
            public const string ProductUpdateError = "Error al actualizar el producto";
            public const string BrandUpdateError = "Error al actualizar la marca";
            public const string CategoryUpdateError = "Error al actualizar la categoría";

            // Eliminación
            public const string ProductDeleteError = "Error al eliminar el producto";
            public const string BrandDeleteError = "Error al eliminar la marca";
            public const string CategoryDeleteError = "Error al eliminar la categoría";
            public const string ProductCannotBeDeleted = "No se puede eliminar el producto porque tiene referencias";
        }

        public static class Validation
        {
            public const string UnSupportedFileType = "El tipo de archivo no es soportado.";
        }

        public static class Info
        {

        }
    }
}
