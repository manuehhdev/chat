using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class MensajeTexto
{
    public Guid Id { get; set; }

    public string Contenido { get; set; } = null!;

    public virtual Mensajes IdNavigation { get; set; } = null!;
}
