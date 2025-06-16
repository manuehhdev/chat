using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace ApplicationCore.Entities;

public partial class MensajeUbicacion
{
    public Guid Id { get; set; }

    public Geometry Ubicacion { get; set; } = null!;

    public virtual Mensajes IdNavigation { get; set; } = null!;
}
