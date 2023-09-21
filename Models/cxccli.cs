using System;
using System.Collections.Generic;

namespace AppClientesMexaba.Models
{
    public class cxccli
    {
        public string? cve { get; set; }
        public string? vendedor { get; set; }
        public string? nom { get; set; }
        public string? dir1 { get; set; }
        public string? cd { get; set; }
        public string? est { get; set; }
        public string? pais { get; set; }
        public string? pos { get; set; }
        public string? tel2 { get; set; }
        public string? seg_mer { get; set; }
        public string? con_pag { get; set; }
        public string? for_pgo { get; set; }
        public string? suc { get; set; }
        public string? status { get; set; }
        public string? rfc { get; set; }
        public string? mail1 { get; set; }
        public string? usocfdi { get; set; }
        public int TotalRecords { get; set; } // Total de registros en la tabla
        public int Page { get; set; } // Página actual
        public int PageSize { get; set; } // Tamaño de página
        public List<cxccli> Data { get; set; } // Lista de datos para la página actual
    }
}
