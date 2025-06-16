using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class MensajeImagen
{
    public Guid Id { get; set; }

    public string UrlImagen { get; set; } = null!;

    public virtual Mensajes IdNavigation { get; set; } = null!;
}
