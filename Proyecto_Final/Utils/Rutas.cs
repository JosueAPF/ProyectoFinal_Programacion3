﻿namespace Proyecto_Final.Utils
{
    public class Rutas
    {
        public string Ruta { get; set; }

        /*IWebHostEnvironment en ASP.NET Core proporciona información sobre el entorno de hospedaje de la aplicación, 
         * como el directorio raíz de ese caso del contenido web*/


        public Rutas(IWebHostEnvironment ruta)
        {
            Ruta = Path.Combine(ruta.ContentRootPath, "Data");  
        }
        public string ObtenerArchivo(string NombreArchivo)
        {
            return Path.Combine(Ruta, NombreArchivo);
        }

    }
}
